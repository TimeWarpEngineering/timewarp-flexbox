#region Purpose
// Builds and runs every sample under samples/ and checks the files a sample declares.
#endregion
#region Design
// SampleDiscovery finds each runfile and project under samples/.
// Each entry is built, then run, on every TFM it declares. No pin means one
// build and run on the TFM MSBuild resolves (Directory.Build.props for a project).
// Library projects are built only.
// dotnet build and dotnet run go through TimeWarp.Amuru Shell.Builder with a timeout.
// --artifacts-path keeps compile output in a temp directory. A sibling verify-samples.args
// may pass {out} and {outdir}; {out} must exist and be non-empty when the sample exits.
// The temp directory is deleted afterward so the working tree stays clean.
// A build error, non-zero exit, timeout, or missing output sets the process exit code,
// which stops dev workflow.
#endregion

namespace DevCli.Commands;

[NuruRoute("verify-samples", Description = "Build and run every sample")]
internal sealed class VerifySamplesCommand : ICommand<Unit>
{
  internal sealed class Handler : ICommandHandler<VerifySamplesCommand, Unit>
  {
    private static readonly TimeSpan BuildTimeout = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan RunTimeout = TimeSpan.FromMinutes(2);

    private readonly ITerminal Terminal;
    private CancellationToken Ct;
    private string RepoRoot = null!;

    public Handler(ITerminal terminal)
    {
      Terminal = terminal;
    }

    public async Task<Unit> Handle(VerifySamplesCommand command, CancellationToken ct)
    {
      _ = command;
      Ct = ct;

      if (!FindRepoRoot())
      {
        return Unit.Value;
      }

      string samplesDirectory = Path.Combine(RepoRoot, "samples");
      if (!Directory.Exists(samplesDirectory))
      {
        Terminal.WriteErrorLine("verify-samples: samples/ does not exist.".Red());
        SetExitCode(1);
        return Unit.Value;
      }

      List<SampleEntry> samples = SampleDiscovery.Discover(samplesDirectory);
      if (samples.Count == 0)
      {
        Terminal.WriteErrorLine("verify-samples: samples/ has no runfiles or projects to verify.".Red());
        SetExitCode(1);
        return Unit.Value;
      }

      Terminal.WriteLine($"Verifying {samples.Count} sample(s)...");
      foreach (SampleEntry sample in samples)
      {
        Ct.ThrowIfCancellationRequested();
        if (!await VerifySampleAsync(sample))
        {
          return Unit.Value;
        }
      }

      Terminal.WriteLine("\nSamples verified successfully!".Green());
      return Unit.Value;
    }

    private bool FindRepoRoot()
    {
      string? root = Git.FindRoot();
      if (root is null)
      {
        Terminal.WriteErrorLine("Error: could not find repository root.");
        SetExitCode(1);
        return false;
      }

      RepoRoot = root;
      return true;
    }

    private async Task<bool> VerifySampleAsync(SampleEntry sample)
    {
      if (sample.Frameworks.Length == 0)
      {
        return await VerifyFrameworkAsync(sample, framework: null);
      }

      foreach (string framework in sample.Frameworks)
      {
        if (!await VerifyFrameworkAsync(sample, framework))
        {
          return false;
        }
      }

      return true;
    }

    private async Task<bool> VerifyFrameworkAsync(SampleEntry sample, string? framework)
    {
      string relative = Path.GetRelativePath(RepoRoot, sample.EntryPath);
      string label = framework ?? "default TFM";
      string scratch = Path.Combine(Path.GetTempPath(), "timewarp-flexbox-verify-samples", Guid.NewGuid().ToString("N"));
      string artifactsPath = Path.Combine(scratch, "artifacts");
      string outputDirectory = Path.Combine(scratch, "output");
      string outputFile = Path.Combine(outputDirectory, "output.html");
      Directory.CreateDirectory(artifactsPath);
      Directory.CreateDirectory(outputDirectory);

      try
      {
        Terminal.WriteLine($"  Building {relative} ({label})...");
        CommandOutput build = await RunDotNetAsync(BuildArguments(sample, framework, artifactsPath), BuildTimeout);
        if (!AcceptProcess(build, relative, label, "build", BuildTimeout))
        {
          return false;
        }

        if (!sample.RunAfterBuild)
        {
          Terminal.WriteLine($"  Built {relative} ({label}); library projects are not run.");
          return true;
        }

        string[] applicationArguments = SampleDiscovery.BindArguments(
          sample.ArgumentTemplate,
          outputFile,
          outputDirectory,
          out string[] expectedOutputs);
        Terminal.WriteLine($"  Running {relative} ({label})...");
        CommandOutput run = await RunDotNetAsync(RunArguments(sample, framework, artifactsPath, applicationArguments), RunTimeout);
        if (!AcceptProcess(run, relative, label, "run", RunTimeout))
        {
          return false;
        }

        if (!CheckOutputs(relative, label, expectedOutputs))
        {
          return false;
        }

        string outputNote = expectedOutputs.Length == 0
          ? string.Empty
          : $", checked {expectedOutputs.Length} output file(s)";
        Terminal.WriteLine($"  {relative} ({label}) ok{outputNote}");
        return true;
      }
      finally
      {
        DeleteScratch(scratch);
      }
    }

    private Task<CommandOutput> RunDotNetAsync(List<string> arguments, TimeSpan timeout)
    {
      return Shell.Builder("dotnet")
        .WithArguments([.. arguments])
        .WithWorkingDirectory(RepoRoot)
        .WithNoValidation()
        .WithTimeout(timeout)
        .CaptureAsync(Ct);
    }

    private bool AcceptProcess(CommandOutput output, string relative, string label, string action, TimeSpan timeout)
    {
      if (output.TimedOut)
      {
        int seconds = (int)timeout.TotalSeconds;
        Terminal.WriteErrorLine($"verify-samples: {relative} ({label}) {action} timed out after {seconds}s.".Red());
        WriteToolOutput(output);
        SetExitCode(CommandResult.TimeoutExitCode);
        return false;
      }

      if (!output.Success)
      {
        Terminal.WriteErrorLine($"verify-samples: {relative} ({label}) {action} failed (exit {output.ExitCode}).".Red());
        WriteToolOutput(output);
        SetExitCode(output.ExitCode == 0 ? 1 : output.ExitCode);
        return false;
      }

      return true;
    }

    private bool CheckOutputs(string relative, string label, string[] expectedOutputs)
    {
      foreach (string expected in expectedOutputs)
      {
        if (!File.Exists(expected) || new FileInfo(expected).Length == 0)
        {
          Terminal.WriteErrorLine($"verify-samples: {relative} ({label}) did not write expected output {expected}.".Red());
          SetExitCode(1);
          return false;
        }
      }

      return true;
    }

    private void WriteToolOutput(CommandOutput output)
    {
      if (!string.IsNullOrWhiteSpace(output.Combined))
      {
        Terminal.WriteErrorLine(output.Combined);
      }
    }

    private void DeleteScratch(string scratch)
    {
      try
      {
        if (Directory.Exists(scratch))
        {
          Directory.Delete(scratch, recursive: true);
        }
      }
      catch (IOException exception)
      {
        Terminal.WriteLine($"Could not delete {scratch}: {exception.Message}".Yellow());
      }
    }

    private static void SetExitCode(int exitCode)
    {
      NuruCallContext.CurrentExitCode = exitCode;
      Environment.ExitCode = exitCode;
    }

    private static List<string> BuildArguments(SampleEntry sample, string? framework, string artifactsPath)
    {
      List<string> arguments = ["build", sample.EntryPath, "-c", "Release", "--artifacts-path", artifactsPath, "-v", "q"];
      AppendFramework(arguments, framework);
      return arguments;
    }

    private static List<string> RunArguments(SampleEntry sample, string? framework, string artifactsPath, string[] applicationArguments)
    {
      List<string> arguments =
      [
        "run",
        sample.IsRunfile ? "--file" : "--project",
        sample.EntryPath,
        "-c", "Release",
        "--artifacts-path", artifactsPath,
        "--no-build",
        "--no-launch-profile"
      ];
      AppendFramework(arguments, framework);
      if (applicationArguments.Length > 0)
      {
        arguments.Add("--");
        arguments.AddRange(applicationArguments);
      }

      return arguments;
    }

    private static void AppendFramework(List<string> arguments, string? framework)
    {
      if (framework is null)
      {
        return;
      }

      arguments.Add("-f");
      arguments.Add(framework);
    }
  }
}
