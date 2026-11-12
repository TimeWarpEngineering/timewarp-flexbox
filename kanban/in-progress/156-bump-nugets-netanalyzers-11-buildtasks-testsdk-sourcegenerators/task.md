# Bump NuGets: NetAnalyzers 11, Build.Tasks, Test.Sdk, SourceGenerators

## Goal (implementer restatement)

Bring the four outdated central package pins up to the versions `ganda nuget outdated` reported on 2026-11-11, then leave the solution building and testing clean. The pins are Microsoft.CodeAnalysis.NetAnalyzers 10.0.403 → 11.0.100, TimeWarp.Build.Tasks 1.0.0 → 1.0.2, Microsoft.NET.Test.Sdk 18.11.0 → 18.11.1, and TimeWarp.SourceGenerators 1.0.0-beta.12 → 1.0.0-beta.14, all in `Directory.Packages.props`.

Acceptance: `ganda nuget outdated` shows every package current; `dotnet build timewarp-flexbox.slnx` is 0 warnings and 0 errors; `dotnet test timewarp-flexbox.slnx` passes on net10.0 and net11.0; `./bin/dev --capabilities` runs; `ganda repo audit` exits 0. New NetAnalyzers 11 and TimeWarp.SourceGenerators TW00xx diagnostics are fixed in code. A suppression is allowed only when a rule truly does not apply, with a reason recorded here, and `.timewarp/suppression-baseline.json` does not grow without that justification. Public library behavior and TFMs stay as they are (library and tests net10.0 + net11.0; dev CLI, runfiles, and hooks net11.0).

## Description

`ganda nuget outdated` on master (2026-11-11) reports 4 of 19 packages outdated:

| Package | Current | Latest | Type |
|---|---|---|---|
| Microsoft.CodeAnalysis.NetAnalyzers | 10.0.403 | 11.0.100 | major |
| TimeWarp.Build.Tasks | 1.0.0 | 1.0.2 | patch |
| Microsoft.NET.Test.Sdk | 18.11.0 | 18.11.1 | patch |
| TimeWarp.SourceGenerators | 1.0.0-beta.12 | 1.0.0-beta.14 | prerelease |

Bump all four pins in `Directory.Packages.props` and get the build back to 0 warnings / 0 errors.

## Requirements

- `ganda nuget outdated` reports all packages up to date.
- The NetAnalyzers major bump is in scope (major bumps are not an exception). Fix the code for any
  new analyzer diagnostics. Do not suppress or lower severity to make them go away unless a rule
  truly does not apply. Any such suppression needs a reason, and the suppression baseline
  (`.timewarp/suppression-baseline.json`) must not grow without justification in Results.
- Same for any new TW00xx diagnostics from TimeWarp.SourceGenerators beta.14.
- No public API behavior changes in the library. Keep TFMs unchanged (library and tests net10.0 + net11.0; dev CLI, runfiles and hooks net11.0).

## Checklist

- [x] Bump the four pins
- [x] `dotnet build timewarp-flexbox.slnx`: 0 warnings, 0 errors
- [x] `dotnet test timewarp-flexbox.slnx` green on net10.0 and net11.0
- [x] `./bin/dev --capabilities` works
- [x] `ganda repo audit` exit 0 (commit whatever `--fix` changes)
- [x] `ganda nuget outdated` all up to date
- [x] Record new diagnostics hit and how each was resolved in Results

## Session

- Created: d7943e80-818a-43bd-a36a-6968aed6437a (2026-11-11)
- Implemented 2026-11-12. Pins moved to the versions `ganda nuget outdated` reported that day, which is past the 2026-11-11 snapshot in the description: TimeWarp.Build.Tasks 2.0.0 (the 1.0.2 patch was already behind 2.0.0) and Microsoft.NET.Test.Sdk 18.12.0. `ganda repo audit --fix` redeployed the attest-baseline git hooks and wrote the gitignored `.vscode/settings.json` window title.

## Results

Central pins are current: Microsoft.CodeAnalysis.NetAnalyzers 11.0.100, TimeWarp.SourceGenerators 1.0.0-beta.14, TimeWarp.Build.Tasks 2.0.0, Microsoft.NET.Test.Sdk 18.12.0. Library behavior and TFMs are unchanged. `.timewarp/suppression-baseline.json` and `.timewarp/line-cap-baseline.json` are unchanged.

Diagnostics from the bump:

- **TWA0004** (SourceGenerators beta.14) on in-repo C# files. Each file now has a `#region Purpose` block. Yoga copyright headers were converted in place so baselined files did not gain lines. `runfiles/port-generated-tests.cs` emits that same header.
- **TWA0038** on the Yoga ports and generated fixtures already listed in `.timewarp/line-cap-baseline.json` (task 151). Splitting those files invents boundaries the port does not have. Each baselined path sets `dotnet_diagnostic.TWA0038.max_lines` to its recorded count. Every other in-repo file keeps the 500-line default, and `source-file-line-cap` still refuses growth.
- **TWA0004** on `Fixie.Main.cs`. Fixie says not to modify that package file, and the repo `.editorconfig` cannot reach the NuGet cache. The test project writes a per-TFM global config that marks the absolute path generated and sets `TWA0004` severity to none. The rule does not apply to source this repo cannot edit.
- **TWA0038** on TimeWarp.Nuru.DevCli 4.0.0-beta.12 content compiled into the dev CLI (`release-command.cs` 609 lines, `nuget-version-service.cs` 704). Those files are package-owned. `tools/dev-cli/.globalconfig` sets `TWA0038.max_lines` to 704 for that compilation only. timewarp-nuru task 577 tracks the split. In-repo files are not covered by that config.
- **NetAnalyzers 11** and **TW00xx**: no diagnostics. The solution build is 0 warnings and 0 errors.
- **Build.Tasks 2.0.0** loads its task assembly on the .NET 11 SDK via `dotnet build`. The library and tests stay `net10.0` and `net11.0`.

`ganda repo audit` exits 0 (44 passed, 0 failed). `ganda nuget outdated` reports all 19 packages up to date.

### How to validate

Smoke: `dotnet build timewarp-flexbox.slnx && dotnet test timewarp-flexbox.slnx && dotnet build tools/dev-cli/dev.cs && ./bin/dev --capabilities && ganda nuget outdated && ganda repo audit`

Expect: the solution build finishes with 0 warnings and 0 errors. Tests pass 1468 on net10.0 and 1468 on net11.0. The dev CLI build finishes with 0 warnings. `--capabilities` prints the `dev` command list, including `build`, `test`, `verify-samples`, `workflow`, `check-version`, `clean`, `release`, and `self-install`. `ganda nuget outdated` prints `All packages are up to date!` and exits 0. `ganda repo audit` exits 0.

## Notes

Previous bumps: task 154 (PR #15), audit cleanup task 155 (PR #16).
