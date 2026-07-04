#region Purpose
// Executes the full CI/CD workflow
// For PR/push:  clean -> build -> verify-samples -> test
// For release:  clean -> build -> verify-samples -> test -> check-version -> pack -> push -> notify timewarp-software
#endregion
#region Design
// Steps delegate to ./bin/dev subcommands where one exists (clean, build,
// verify-samples, test, check-version); pack/push/notify run inline.
// Release mode is detected from --api-key or GITHUB_EVENT_NAME=release,
// mirroring timewarp-terminal's workflow endpoint.
// The timewarp-software dispatch is best-effort: a failure must never fail
// a release that already pushed (the site rebuilds nightly regardless).
#endregion

namespace DevCli.Commands;

[NuruRoute("workflow", Description = "Execute full CI/CD workflow")]
internal sealed class WorkflowCommand : ICommand<Unit>
{
  [Option("api-key", Description = "NuGet API key for publishing (from OIDC Trusted Publishing)")]
  public string? ApiKey { get; set; }

  internal sealed class Handler : ICommandHandler<WorkflowCommand, Unit>
  {
    private const string PackageId = "TimeWarp.Flexbox";

    private readonly ITerminal Terminal;
    private CancellationToken Ct;
    private string RepoRoot = null!;
    private string DevBin = null!;

    public Handler(ITerminal terminal)
    {
      Terminal = terminal;
    }

    public async ValueTask<Unit> Handle(WorkflowCommand command, CancellationToken ct)
    {
      Ct = ct;

      string? eventName = Environment.GetEnvironmentVariable("GITHUB_EVENT_NAME");
      bool isRelease = eventName == "release" || !string.IsNullOrEmpty(command.ApiKey);

      if (!FindRepoRoot(isRelease))
      {
        return Value;
      }

      if (!await RunStepAsync("clean", "Clean failed!"))
      {
        return Value;
      }

      if (!await RunStepAsync("build", "Build failed!"))
      {
        return Value;
      }

      if (!await RunStepAsync("verify-samples", "Sample verification failed!"))
      {
        return Value;
      }

      if (!await RunStepAsync("test", "Tests failed!"))
      {
        return Value;
      }

      if (!isRelease)
      {
        Terminal.WriteLine("\nWorkflow completed successfully!".Green());
        return Value;
      }

      if (!await RunStepAsync("check-version", "Version check failed!"))
      {
        return Value;
      }

      if (!await PackAsync())
      {
        return Value;
      }

      if (!string.IsNullOrEmpty(command.ApiKey))
      {
        if (!await PushAsync(command.ApiKey))
        {
          return Value;
        }

        await NotifySoftwareSiteAsync();
      }
      else
      {
        Terminal.WriteLine("\nNo --api-key provided; skipping push and site notification (dry run).");
      }

      Terminal.WriteLine("\nRelease workflow completed successfully!".Green());
      return Value;
    }

    private bool FindRepoRoot(bool isRelease)
    {
      string? root = Git.FindRoot();
      if (root is null)
      {
        Terminal.WriteErrorLine("Error: could not find repository root.");
        Environment.ExitCode = 1;
        return false;
      }

      RepoRoot = root;
      DevBin = Path.Combine(RepoRoot, "bin", "dev");
      Terminal.WriteLine(isRelease ? "Starting release workflow..." : "Starting CI workflow...");
      return true;
    }

    private async Task<bool> RunStepAsync(string subcommand, string failureMessage)
    {
      // bin/dev is a local self-install artifact and is not committed; in CI
      // (or before self-install) fall back to running the CLI as a runfile.
      string[] runner = File.Exists(DevBin)
        ? [DevBin, subcommand]
        : ["dotnet", Path.Combine(RepoRoot, "tools", "dev-cli", "dev.cs"), subcommand];

      int exitCode = await Shell.Builder(runner[0])
        .WithArguments(runner[1..])
        .WithWorkingDirectory(RepoRoot)
        .WithNoValidation()
        .RunAsync(Ct);

      if (exitCode != 0)
      {
        Terminal.WriteErrorLine(failureMessage.Red());
        Environment.ExitCode = exitCode;
        return false;
      }

      return true;
    }

    private async Task<bool> PackAsync()
    {
      Terminal.WriteLine("\nPacking...");
      string artifactsDir = Path.Combine(RepoRoot, "artifacts", "packages");
      Directory.CreateDirectory(artifactsDir);

      int exitCode = await Shell.Builder("dotnet")
        .WithArguments(
          "pack",
          Path.Combine(RepoRoot, "source", "timewarp-flexbox", "timewarp-flexbox.csproj"),
          "-c", "Release",
          "-o", artifactsDir,
          "-p:ContinuousIntegrationBuild=true")
        .WithWorkingDirectory(RepoRoot)
        .WithNoValidation()
        .RunAsync(Ct);

      if (exitCode != 0)
      {
        Terminal.WriteErrorLine("Pack failed!".Red());
        Environment.ExitCode = exitCode;
        return false;
      }

      Terminal.WriteLine($"Packages created in: {artifactsDir}");
      return true;
    }

    private async Task<bool> PushAsync(string apiKey)
    {
      Terminal.WriteLine("\nPushing packages to NuGet...");
      string artifactsDir = Path.Combine(RepoRoot, "artifacts", "packages");
      string[] packages = Directory.GetFiles(artifactsDir, "*.nupkg");

      foreach (string package in packages)
      {
        string packageName = Path.GetFileName(package);
        Terminal.WriteLine($"  Pushing {packageName}...");

        int exitCode = await Shell.Builder("dotnet")
          .WithArguments(
            "nuget", "push", package,
            "--source", "https://api.nuget.org/v3/index.json",
            "--api-key", apiKey,
            "--skip-duplicate")
          .WithWorkingDirectory(RepoRoot)
          .WithNoValidation()
          .RunAsync(Ct);

        if (exitCode != 0)
        {
          Terminal.WriteErrorLine($"NuGet push failed: {packageName}".Red());
          Environment.ExitCode = exitCode;
          return false;
        }
      }

      Terminal.WriteLine("Packages pushed to NuGet.org".Green());
      return true;
    }

    private async Task NotifySoftwareSiteAsync()
    {
      // Signal timewarp-software to rebuild the site so the new release shows up
      // immediately instead of waiting for its nightly cron backstop. Best effort:
      // a failure here must never fail a release that already pushed to NuGet.
      // Cross-repo repository_dispatch needs a credential with write access to
      // timewarp-software — locally gh's stored auth suffices; in GitHub Actions
      // the default GITHUB_TOKEN cannot reach other repos, so workflow.yml mints a
      // short-lived installation token from the org's Rebuild Dispatcher GitHub App
      // and passes it as GH_TOKEN.
      Terminal.WriteLine("\nNotifying timewarp-software to rebuild the site...");
      string version = await ReadVersionAsync();

      int exitCode = await Shell.Builder("gh")
        .WithArguments(
          "api",
          "repos/TimeWarpEngineering/timewarp-software/dispatches",
          "-f", "event_type=rebuild",
          "-f", $"client_payload[package]={PackageId}",
          "-f", $"client_payload[version]={version}")
        .WithWorkingDirectory(RepoRoot)
        .WithNoValidation()
        .RunAsync(Ct);

      if (exitCode == 0)
      {
        Terminal.WriteLine("timewarp-software rebuild dispatched".Green());
      }
      else
      {
        Terminal.WriteLine("Could not dispatch timewarp-software rebuild (non-fatal; the site rebuilds nightly)".Yellow());
      }
    }

    private async Task<string> ReadVersionAsync()
    {
      string propsPath = Path.Combine(RepoRoot, "source", "Directory.Build.props");
      if (File.Exists(propsPath))
      {
        string content = await File.ReadAllTextAsync(propsPath, Ct);
        int start = content.IndexOf("<Version>", StringComparison.Ordinal);
        if (start >= 0)
        {
          start += "<Version>".Length;
          int end = content.IndexOf("</Version>", start, StringComparison.Ordinal);
          if (end > start)
          {
            return content[start..end];
          }
        }
      }

      return "unknown";
    }
  }
}
