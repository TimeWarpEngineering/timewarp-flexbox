# Task 049-implement-github-actions-cicd

## Summary
Set up GitHub Actions CI/CD pipeline for automated building, testing, and publishing TimeWarp.Flexbox NuGet packages to nuget.org, following the pattern established in timewarp-nuru.

## Todo List
- [ ] Update `source/timewarp-flexbox/timewarp-flexbox.csproj` with NuGet package metadata
  - Set `IsPackable` to true
  - Add `PackageId`, `Authors`, `Company` properties
  - Add `RepositoryUrl`, `PackageProjectUrl`, `RepositoryType`
  - Add `PackageLicenseExpression` (MIT)
  - Add `PackageReadmeFile` pointing to readme.md
  - Add ItemGroup to include readme.md in package
- [ ] Create `nuget.config` in repository root with local feed for development
- [ ] Create `runfiles/build.cs` using TimeWarp.Amuru pattern (follow timewarp-nuru)
- [ ] Create `runfiles/check-version.cs` to verify package not already published
- [ ] Create `.github/workflows/ci-cd.yml` (single workflow like timewarp-nuru)
  - Trigger on push/PR to master with path filters
  - Trigger on release published
  - Trigger on workflow_dispatch with optional version input
  - Setup .NET 10
  - Build using runfile
  - Run tests
  - Check version before publishing (release only)
  - Publish to nuget.org (release or manual dispatch only)
  - Upload artifacts
- [ ] Add `NUGET_API_KEY` secret to repository settings
- [ ] Test CI workflow with a pull request
- [ ] Test publish workflow with a GitHub release
- [ ] Document release process in repository

## Notes

### Reference Implementation
Located at: `/home/steventcramer/worktrees/github.com/TimeWarpEngineering/timewarp-nuru/master/`

Key files to reference:
- `.github/workflows/ci-cd.yml` - Single workflow for CI and publishing
- `Scripts/Build.cs` - .NET 10 runfile using TimeWarp.Amuru
- `Scripts/CheckVersion.cs` - Verifies package not already on nuget.org
- `nuget.config` - Simple config with nuget.org and local feed

### nuget.config Pattern (from timewarp-nuru)

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="LocalNuGetFeed" value="artifacts/packages" />
  </packageSources>
</configuration>
```

This allows:
- Consuming packages from nuget.org
- Local development/testing with packages in `artifacts/packages`
- No GitHub Packages complexity - just publish directly to nuget.org

### ci-cd.yml Pattern (from timewarp-nuru)

```yaml
name: NuGet Publish

on:
  push:
    branches:
      - master
    paths:
      - 'source/**'
      - 'test/**'
      - 'runfiles/**'
      - '.github/workflows/**'
      - 'Directory.Build.props'
      - 'Directory.Packages.props'
  pull_request:
    branches:
      - master
    paths:
      - 'source/**'
      - 'test/**'
      - 'runfiles/**'
      - '.github/workflows/**'
      - 'Directory.Build.props'
      - 'Directory.Packages.props'
  release:
    types: [published]
  workflow_dispatch:
    inputs:
      version:
        description: 'Version to publish (e.g., 1.0.0-beta.1)'
        required: false
        type: string

jobs:
  build-and-publish:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          fetch-depth: 0

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Create artifacts directory
        run: mkdir -p artifacts/packages

      - name: Build
        run: dotnet ${{ github.workspace }}/runfiles/build.cs

      - name: Test
        run: dotnet test --configuration Release --no-build

      - name: Check if version already published (Releases only)
        if: github.event_name == 'release'
        run: dotnet ${{ github.workspace }}/runfiles/check-version.cs

      - name: Publish to NuGet.org (Releases only)
        if: github.event_name == 'release' || (github.event_name == 'workflow_dispatch' && github.event.inputs.version != '')
        run: |
          if [ "${{ github.event_name }}" == "release" ]; then
            VERSION="${{ github.event.release.tag_name }}"
            VERSION="${VERSION#v}"
          else
            VERSION="${{ github.event.inputs.version }}"
          fi
          
          echo "Publishing version: $VERSION"
          dotnet nuget push artifacts/packages/TimeWarp.Flexbox.$VERSION.nupkg \
            --api-key ${{ secrets.NUGET_API_KEY }} \
            --source https://api.nuget.org/v3/index.json \
            --skip-duplicate
        env:
          DOTNET_NUGET_SIGNATURE_VERIFICATION: false

      - name: Upload Artifacts
        uses: actions/upload-artifact@v4
        with:
          name: Packages-${{ github.run_number }}
          path: artifacts/packages/*.nupkg
```

### Build.cs Runfile Pattern (from timewarp-nuru)

```csharp
#!/usr/bin/dotnet --
#:package TimeWarp.Amuru@1.0.0-beta.5

// Build.cs - Build the TimeWarp.Flexbox library

string scriptDir = (AppContext.GetData("EntryPointFileDirectoryPath") as string)!;
Directory.SetCurrentDirectory(scriptDir);

WriteLine("Building TimeWarp.Flexbox library...");
WriteLine($"Working from: {Directory.GetCurrentDirectory()}");

string[] projectsToBuild = [
  "../source/timewarp-flexbox/timewarp-flexbox.csproj",
  "../test/timewarp-flexbox-tests/timewarp-flexbox-tests.csproj",
  "../benchmarks/timewarp-flexbox-benchmarks/TimeWarp.Flexbox.Benchmarks.csproj"
];

try
{
  foreach (string projectPath in projectsToBuild)
  {
    WriteLine($"Building {projectPath}...");
    CommandResult buildCommandResult = DotNet.Build()
      .WithProject(projectPath)
      .WithConfiguration("Release")
      .WithVerbosity("minimal")
      .Build();

    WriteLine("Running ...");
    WriteLine(buildCommandResult.ToCommandString());

    if (await buildCommandResult.RunAsync() != 0)
    {
      WriteLine($"Failed to build {projectPath}!");
      Environment.Exit(1);
    }
  }
}
catch (Exception ex)
{
  WriteLine($"Exception: {ex.Message}");
  Environment.Exit(1);
}
```

### CheckVersion.cs Runfile Pattern (from timewarp-nuru)

```csharp
#!/usr/bin/dotnet --
#:package TimeWarp.Amuru@1.0.0-beta.5

using System.Xml.Linq;

string scriptDir = (AppContext.GetData("EntryPointFileDirectoryPath") as string)!;
Directory.SetCurrentDirectory(scriptDir);

// Read version from source/Directory.Build.props or the .csproj
string propsPath = "../source/Directory.Build.props";
var doc = XDocument.Load(propsPath);
string? version = doc.Descendants("Version").FirstOrDefault()?.Value;

if (string.IsNullOrEmpty(version))
{
    WriteLine("Could not find version in Directory.Build.props");
    Environment.Exit(1);
}

WriteLine($"Checking if TimeWarp.Flexbox {version} is already published...");

CommandOutput result = await DotNet.PackageSearch("TimeWarp.Flexbox")
    .WithExactMatch()
    .WithPrerelease()
    .WithSource("https://api.nuget.org/v3/index.json")
    .Build()
    .CaptureAsync();

if (result.Stdout.Contains($"| {version} |", StringComparison.Ordinal))
{
    WriteLine($"WARNING: TimeWarp.Flexbox {version} is already published!");
    Environment.Exit(1);
}

WriteLine($"TimeWarp.Flexbox {version} is ready to publish!");
```

### Secrets Required
- `NUGET_API_KEY`: API key from nuget.org (create in repository settings > Secrets and variables > Actions)

### Release Process
1. Update version in `source/Directory.Build.props` or `.csproj`
2. Commit and push to master
3. Create a GitHub Release with tag (e.g., `v1.0.0`)
4. CI/CD automatically builds, tests, and publishes to nuget.org

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
