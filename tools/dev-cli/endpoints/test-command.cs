#region Purpose
// Run the test suite
#endregion
#region Design
// Executes dotnet test for all tests in the repository.
// Handler stores Command and Ct as fields so private methods are zero-parameter.
// Streams test output via Amuru RunAsync by default; --quiet uses CaptureAsync.
// The test project references coverlet.collector. The run passes
// --collect "XPlat Code Coverage" and writes under artifacts/coverage.
// That directory is recreated first so a stale Cobertura file cannot satisfy the check.
// A green suite with no coverage.cobertura.xml fails the command.
#endregion

namespace DevCli.Commands;

[NuruRoute("test", Description = "Run the test suite")]
internal sealed class TestCommand : ICommand<Unit>
{
  [Option("quiet", "q", Description = "Hide test output unless the command fails")]
  public bool Quiet { get; set; }

  internal sealed class Handler : IRequestHandler<TestCommand, Unit>
  {
    private readonly ITerminal Terminal;
    private TestCommand Command = null!;
    private CancellationToken Ct;
    private string RepoRoot = null!;

    public Handler(ITerminal terminal)
    {
      Terminal = terminal;
    }

    public async ValueTask<Unit> Handle(TestCommand command, CancellationToken ct)
    {
      Command = command;
      Ct = ct;

      if (!FindRepoRoot())

      {

        return Unit.Value;

      }

      if (!await TestAsync())
      {
        return Unit.Value;
      }

      Terminal.WriteLine("\nTests completed successfully!".Green());
      return Unit.Value;
    }

    private bool FindRepoRoot()
    {
      string? root = Git.FindRoot();
      if (root is null)
      {
        Terminal.WriteErrorLine("Error: could not find repository root.");
        Environment.ExitCode = 1;
        return false;
      }

      RepoRoot = root;
      Terminal.WriteLine("Running test suite...");
      return true;
    }

    private async Task<bool> TestAsync()
    {
      string coverageDirectory = Path.Combine(RepoRoot, "artifacts", "coverage");
      if (Directory.Exists(coverageDirectory))
      {
        Directory.Delete(coverageDirectory, recursive: true);
      }

      Directory.CreateDirectory(coverageDirectory);

      CommandResult command = DotNet.Test()
        .WithConfiguration("Release")
        .WithCollect("XPlat Code Coverage")
        .WithResultsDirectory(coverageDirectory)
        .WithWorkingDirectory(RepoRoot)
        .WithNoValidation()
        .Build();

      if (Command.Quiet)
      {
        CommandOutput result = await command.CaptureAsync(Ct);
        if (!result.Success)
        {
          Terminal.WriteErrorLine(result.Combined);
          Terminal.WriteErrorLine("Tests failed!".Red());
          Environment.ExitCode = 1;
          return false;
        }
      }
      else
      {
        int exitCode = await command.RunAsync(Ct);
        if (exitCode != 0)
        {
          Terminal.WriteErrorLine("Tests failed!".Red());
          Environment.ExitCode = exitCode;
          return false;
        }
      }

      if (!HasCobertura(coverageDirectory))
      {
        Terminal.WriteErrorLine("Tests passed but artifacts/coverage has no coverage.cobertura.xml.".Red());
        Environment.ExitCode = 1;
        return false;
      }

      return true;
    }

    /// <summary>True when coverlet wrote a Cobertura file under the results directory.</summary>
    private static bool HasCobertura(string coverageDirectory)
    {
      return Directory.Exists(coverageDirectory)
        && Directory.EnumerateFiles(coverageDirectory, "coverage.cobertura.xml", SearchOption.AllDirectories).Any();
    }
  }
}