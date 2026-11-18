# Round 1 — general
**Date:** 2026-11-19
**Scope reviewed:** branch `task/162-refresh-outdated-nu-get-packages-in-time-warp` vs merge-base `aa3cfe99`. Product files: `Directory.Packages.props`, `tools/dev-cli/endpoints/verify-samples-command.cs`, `tools/dev-cli/endpoints/workflow-command.cs`. Call sites: `Directory.Build.props` (`TimeWarp.SourceGenerators` with `PrivateAssets=all`), `tools/dev-cli/Directory.Build.props` (Nuru, Nuru.DevCli, Amuru, Amuru.Tools references with no inline versions), `source/Directory.Build.props` (`<Version>`).

## Summary

The five pins named in the task are updated in `Directory.Packages.props`: TimeWarp.Amuru and TimeWarp.Amuru.Tools `3.0.0-beta.10`, TimeWarp.Nuru and TimeWarp.Nuru.DevCli `4.0.0-beta.16`, TimeWarp.SourceGenerators `1.0.0-beta.17`. No other `PackageVersion` changed. `<Version>` in `source/Directory.Build.props` is still `2.0.0-beta.3`. The diff does not touch that file. No release was cut. No analyzer suppression is in the diff.

Central `PackageReference`s for those five packages carry no inline version, so they take the central pins. Amuru, Amuru.Tools, Nuru, and Nuru.DevCli are referenced from `tools/dev-cli/Directory.Build.props`. SourceGenerators is `PrivateAssets=all` on the root `Directory.Build.props`, so the generator bump participates in the library compile and does not flow to package consumers.

Amuru 3.0.0-beta.10's AMURU0001 error stopped the dev CLI compile on four literal `Shell.Builder("dotnet")` / `Shell.Builder("gh")` calls. The port keeps the same process contract:

- `RunDotNetAsync` is `DotNet.Builder().WithArguments(...).WithWorkingDirectory(RepoRoot).WithNoValidation().WithTimeout(timeout).CaptureAsync(Ct)`. `DotNetBuilder.Build` runs `dotnet` with those arguments. `BuildArguments` and `RunArguments` still start at `build` / `run`. `./bin/dev verify-samples` is the recorded exercise of this path.
- `PackAsync` is `dotnet pack <csproj> --configuration Release --output <artifactsDir>` with the same working directory and `WithNoValidation`. That is the same command as `dotnet pack <csproj> -c Release -o <artifactsDir>`.
- `PushAsync` calls `WithWorkingDirectory` on `DotNetNuGetBuilder` before `Push`. `Push` copies that `CommandOptions` into `DotNetNuGetPushBuilder`. The assembled command is `dotnet nuget push <package> --source https://api.nuget.org/v3/index.json --api-key <key> --skip-duplicate`, and `WithNoValidation` is set on the push builder.
- `NotifySoftwareSiteAsync` assembles `gh api <endpoint> -f event_type=rebuild -f client_payload[package]=TimeWarp.Flexbox -f client_payload[version]=<version>` via `WithRawField` (`-f` plus `key=value`). `WithWorkingDirectory` and `WithNoValidation` live on `GhCommandBuilder`. A non-zero exit is still logged and does not fail the release.

`RunStepAsync` still uses `Shell.Builder(runner[0])` where `runner[0]` is `bin/dev` or the `dotnet` runfile fallback. That call is not one of the four literal tool invocations. The recorded warning-free dev CLI compile is the evidence AMURU0001 accepts it.

The restatement's acceptance line allows product edits beyond `Directory.Packages.props` when the bump forces them. The task says to edit `Directory.Packages.props` in this repo only. That loosening is a misread and is recorded under `## Learnings`. The five pins and the unchanged `<Version>` match the task. The dev CLI edits are the compile fix for AMURU0001. They are not a finding: a pin-only bump does not compile, and the port does not suppress the analyzer.

`## Results` records `dotnet run --file tools/dev-cli/dev.cs -- self-install` (PASS, no warnings), `./bin/dev build` (PASS, 0 warnings, 0 errors), `./bin/dev verify-samples` (PASS; no test covers `VerifySamplesCommand` or `WorkflowCommand`), and `ganda repo audit` (PASS, one pre-existing advisory `vscode-window-icon`). `### How to validate` names those commands and the expected pin versions. The implementer log states the same outcomes and states that pack, NuGet push, and `gh api` did not run. That record covers the diff. Gates were not re-run.

The release-only pack, push, and dispatch paths were checked by decompiling TimeWarp.Amuru.Tools 3.0.0-beta.10 (`ilspycmd -t` on `DotNetPackBuilder`, `DotNetNuGetBuilder`, `DotNetNuGetPushBuilder`, `GhApiBuilder`, and `GhCommandBuilder`1`, plus `DotNetBuilder`). Reason: those three calls were not in the recorded smoke, and a typed builder can drop the working directory, the no-validation option, or the flag shape. The decompile shows the commands above. This was not a build, a test run, or an audit.

The Design comment in `verify-samples-command.cs` still says dotnet build and run go through `Shell.Builder`. The method now uses `DotNet.Builder` and still sets the timeout. That comment is stale and is not a finding.

`master` is two kanban land commits ahead (`6c27a64`, `1745257`). They touch only the task file. They are not part of this product diff.

## Issues

None.
