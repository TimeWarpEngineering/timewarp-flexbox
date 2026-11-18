# Refresh outdated NuGet packages in TimeWarpEngineering/timewarp-flexbox

## Description

Refresh outdated NuGet packages in TimeWarpEngineering/timewarp-flexbox.
Filter: (all)
Outdated:
  TimeWarp.Amuru 3.0.0-beta.5 -> 3.0.0-beta.10 (dev-only: tools/dev-cli/Directory.Build.props)
  TimeWarp.Amuru.Tools 3.0.0-beta.5 -> 3.0.0-beta.10 (dev-only: tools/dev-cli/Directory.Build.props)
  TimeWarp.Nuru 4.0.0-beta.15 -> 4.0.0-beta.16 (dev-only: tools/dev-cli/Directory.Build.props)
  TimeWarp.Nuru.DevCli 4.0.0-beta.15 -> 4.0.0-beta.16 (dev-only: tools/dev-cli/Directory.Build.props)
  TimeWarp.SourceGenerators 1.0.0-beta.16 -> 1.0.0-beta.17 (dev-only: PrivateAssets=all on Directory.Build.props)
Every outdated pin is dev-only. Bump them. Do not change <Version> in source/Directory.Build.props: the wave merges this task and does not release this repo.
Edit Directory.Packages.props in this repo only.

## Goal (implementer restatement)

Bump the five dev-only TimeWarp package pins in `Directory.Packages.props` to their latest
prereleases (Amuru + Amuru.Tools 3.0.0-beta.10, Nuru + Nuru.DevCli 4.0.0-beta.16,
SourceGenerators 1.0.0-beta.17). Leave `<Version>` in `source/Directory.Build.props` alone.

Acceptance criteria:
- Only `Directory.Packages.props` changes in product files; the five versions match the targets.
- `./bin/dev build` passes and the dev CLI still compiles against the new Nuru/Amuru.
- `ganda repo audit` passes.

## Session

- Minted by ganda task wave run 20261118T170809Z-b6e473
- Implementer: bumped the five pins in `Directory.Packages.props`. Amuru 3.0.0-beta.10 ships
  analyzer AMURU0001 (error), which broke the dev CLI compile on four `Shell.Builder("dotnet"/"gh")`
  calls. Moved them to the typed Amuru.Tools builders, keeping `WithWorkingDirectory` and
  `WithNoValidation` so exit-code handling is unchanged:
  - `verify-samples-command.cs` `RunDotNetAsync`: `DotNet.Builder().WithArguments(...)`
  - `workflow-command.cs` `PackAsync`: `DotNet.Pack(csproj).WithConfiguration("Release").WithOutput(dir)`
  - `workflow-command.cs` `PushAsync`: `DotNet.NuGet().Push(pkg).WithSource(...).WithApiKey(...).WithSkipDuplicate()`
  - `workflow-command.cs` `NotifySoftwareSiteAsync`: `Gh.Api(endpoint).WithRawField(...)` (same `-f` flags)
- `<Version>` in `source/Directory.Build.props` unchanged.

## Results

- `Directory.Packages.props`: TimeWarp.Amuru / Amuru.Tools 3.0.0-beta.5 → 3.0.0-beta.10,
  TimeWarp.Nuru / Nuru.DevCli 4.0.0-beta.15 → 4.0.0-beta.16, TimeWarp.SourceGenerators
  1.0.0-beta.16 → 1.0.0-beta.17.
- Dev CLI ported off `Shell.Builder` for AMURU0001 (see Session).

### Gates

- `dotnet run --file tools/dev-cli/dev.cs -- self-install` (dev CLI changed) → PASS, no warnings
- `./bin/dev build` → PASS (0 warnings, 0 errors)
- Test files: none added or edited. Changed production code is the dev CLI only; no test covers
  `VerifySamplesCommand` or `WorkflowCommand` (grep of test/ and tests/). Smoke instead:
  `./bin/dev verify-samples` → PASS (layout-demo built, ran, output checked; exercises `DotNet.Builder`).
- `ganda repo audit` → PASS (1 advisory warning, pre-existing: `vscode-window-icon`,
  `.vscode/settings.json` missing; non-blocking, left as is)

### How to validate

Smoke:
```bash
dotnet run --file tools/dev-cli/dev.cs -- self-install
./bin/dev build
./bin/dev verify-samples
grep -oE 'TimeWarp\.(Amuru(\.Tools)?|Nuru(\.DevCli)?|SourceGenerators)" Version="[^"]+"' Directory.Packages.props
```

Expect: self-install compiles with no AMURU0001 errors; build and verify-samples succeed; grep
shows Amuru/Amuru.Tools 3.0.0-beta.10, Nuru/Nuru.DevCli 4.0.0-beta.16, SourceGenerators 1.0.0-beta.17.
