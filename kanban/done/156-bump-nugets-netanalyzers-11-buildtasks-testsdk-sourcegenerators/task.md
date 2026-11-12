# Bump NuGets: NetAnalyzers 11, Build.Tasks, Test.Sdk, SourceGenerators

## Goal (implementer restatement)

The four central pins are already on the versions `ganda nuget outdated` reports today: Microsoft.CodeAnalysis.NetAnalyzers 11.0.100, TimeWarp.SourceGenerators 1.0.0-beta.14, TimeWarp.Build.Tasks 2.0.0, and Microsoft.NET.Test.Sdk 18.12.0. This pass closes the review findings that left that bump incomplete.

Acceptance: `FloatLiteral` in `runfiles/port-generated-tests.cs` no longer calls `string.EndsWith(char)` without a comparison, so NetAnalyzers 11 `CA1307` is fixed in code and every `runfiles/*.cs` file builds with 0 warnings and 0 errors. The Fixie entry-point global config uses a forward-slash section path, is written only when its text changes, and is registered as a build output. `TWA0038.max_lines` goes above 500 only for the two package-owned Nuru.DevCli files that exceed it (`release-command.cs` at 609, `nuget-version-service.cs` at 704); in-repo dev-cli files stay on the 500-line default. The extra blank line after `#endregion` in `spikes/cramer/test-shrink.cs` is gone. The solution build is 0 warnings and 0 errors, tests pass on net10.0 and net11.0, `./bin/dev --capabilities` runs, `ganda nuget outdated` is clean, and `ganda repo audit` exits 0. Library behavior, TFMs, and `.timewarp/suppression-baseline.json` stay as they are.

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
- Implement oracle: implemented by implementer-cursor (cursor, vendor xai, model grok-4.7-high), session not reported, max-turns uncapped (no --max-turns flag; bound 500), Done — 2026-11-12T10:26:57Z
- Review oracle round 1: cursor (model gemini-4-carbon-high), body tw-implementation-review, effort 3, roster general, turn cap 200. Result: 2 open bugs (M1, M2), 1 open suggestion (M3), 1 open nit (M4). Fix pass requested, 2026-11-12
- Review oracle: review by implementer-cursor (cursor, vendor google, model gemini-4-carbon-high), session not reported, max-turns uncapped (no --max-turns flag; bound 200), artifacts general.md, merged.md — 2026-11-12T10:45:28Z
- Fix pass 2026-11-12 for review round 1. M1: `CA1307` on `FloatLiteral` is fixed in code (`EndsWith(char, StringComparison.OrdinalIgnoreCase)`), with no `NoWarn` entry. M2: the Fixie global-config section path is forward-slash, `WriteLinesToFile` is `WriteOnlyWhenDifferent`, and the file is in `FileWrites`. M3: the unsectioned `tools/dev-cli/.globalconfig` is replaced by a path-scoped generated config for `release-command.cs` (609) and `nuget-version-service.cs` (704). M4: the extra blank line after `#endregion` in `spikes/cramer/test-shrink.cs` is gone. `.timewarp/suppression-baseline.json` is unchanged.
- Review oracle round 2: cursor (model gemini-4-carbon-high), body tw-implementation-review, effort 1, roster general, turn cap 80. Result: 0 open (M1, M2, M3, M4 verified fixed; 0 new issues), disposition clean, 2026-11-12

## Results

Central pins are current: Microsoft.CodeAnalysis.NetAnalyzers 11.0.100, TimeWarp.SourceGenerators 1.0.0-beta.14, TimeWarp.Build.Tasks 2.0.0, Microsoft.NET.Test.Sdk 18.12.0. Library behavior and TFMs are unchanged. `.timewarp/suppression-baseline.json` and `.timewarp/line-cap-baseline.json` are unchanged.

Diagnostics from the bump:

- **TWA0004** (SourceGenerators beta.14) on in-repo C# files. Each file now has a `#region Purpose` block. Yoga copyright headers were converted in place so baselined files did not gain lines. `runfiles/port-generated-tests.cs` emits that same header.
- **TWA0038** on the Yoga ports and generated fixtures already listed in `.timewarp/line-cap-baseline.json` (task 151). Splitting those files invents boundaries the port does not have. Each baselined path sets `dotnet_diagnostic.TWA0038.max_lines` to its recorded count. Every other in-repo file keeps the 500-line default, and `source-file-line-cap` still refuses growth.
- **TWA0004** on `Fixie.Main.cs`. Fixie says not to modify that package file, and the repo `.editorconfig` cannot reach the NuGet cache. The test project writes a per-TFM global config that marks the absolute path generated and sets `TWA0004` severity to none. The section header is the full path with backslashes replaced by `/`, because `AnalyzerConfig` treats `\` as an escape and matches forward-slash paths. `WriteLinesToFile` sets `WriteOnlyWhenDifferent="true"`, and the file is registered in `FileWrites`, so a second build leaves the timestamp unchanged. The rule does not apply to source this repo cannot edit.
- **TWA0038** on TimeWarp.Nuru.DevCli 4.0.0-beta.12 content compiled into the dev CLI (`release-command.cs` 609 lines, `nuget-version-service.cs` 704). Those files are package-owned. An unsectioned `tools/dev-cli/.globalconfig` raised `max_lines` to 704 for every file in that compilation, including the seven in-repo sources. That file is gone. `WriteNuruDevCliLineCapGlobalConfig` writes a global config whose sections are the forward-slash paths of those two package files only (`609` and `704`), with `WriteOnlyWhenDifferent="true"`. In-repo dev-cli files stay on the 500-line default. timewarp-nuru task 577 tracks the split.
- **CA1307** (NetAnalyzers 11) on `runfiles/port-generated-tests.cs` `FloatLiteral`. `token.EndsWith('f') || token.EndsWith('F')` called the `char` overload that has a `StringComparison` overload. It is now `token.EndsWith('f', StringComparison.OrdinalIgnoreCase)`. `CA1307` is not in `NoWarn`. `dotnet build` of `runfiles/port-generated-tests.cs`, `runfiles/build.cs`, and `runfiles/test.cs` succeeds on net11.0.
- **IDE2000** on `spikes/cramer/test-shrink.cs`. The extra blank line after `#endregion` is removed.
- **NetAnalyzers 11** and **TW00xx** on the solution: no remaining diagnostics. `dotnet build timewarp-flexbox.slnx` is 0 warnings and 0 errors.
- **Build.Tasks 2.0.0** loads its task assembly on the .NET 11 SDK via `dotnet build`. The library and tests stay `net10.0` and `net11.0`.

`ganda repo audit` exits 0 (44 passed, 0 failed). `ganda nuget outdated` reports all 19 packages up to date.

### Review disposition

- **Rounds & roster:** 2 rounds, roster `general` (round 1 at effort 3, round 2 at effort 1)
- **Final counts:** `bug` 0 open / 2 fixed / 0 wontfix (`M1`, `M2`); `suggestion` 0 open / 1 fixed / 0 wontfix (`M3`); `nit` 0 open / 1 fixed / 0 wontfix (`M4`)
- **Disposition:** `clean` (0 open, 0 `wontfix` exceptions, 0 escalations)
- **Review artifacts:** `kanban/in-progress/156-bump-nugets-netanalyzers-11-buildtasks-testsdk-sourcegenerators/review/review-framework.md`, `kanban/in-progress/156-bump-nugets-netanalyzers-11-buildtasks-testsdk-sourcegenerators/review/round-2/merged.md`, `kanban/in-progress/156-bump-nugets-netanalyzers-11-buildtasks-testsdk-sourcegenerators/review/disposition.md`

### How to validate

Smoke: `dotnet build runfiles/port-generated-tests.cs && dotnet build runfiles/build.cs && dotnet build runfiles/test.cs && dotnet build timewarp-flexbox.slnx && dotnet test timewarp-flexbox.slnx && dotnet build tools/dev-cli/dev.cs && ./bin/dev --capabilities && ganda nuget outdated && ganda repo audit`

Expect: each runfile build finishes with 0 warnings and 0 errors on net11.0. The solution build finishes with 0 warnings and 0 errors. Tests pass 1468 on net10.0 and 1468 on net11.0. The dev CLI build finishes with 0 warnings, and `obj/debug/nuru-devcli-linecap.generated.globalconfig` names only `release-command.cs` (`max_lines` 609) and `nuget-version-service.cs` (`max_lines` 704) with forward-slash paths. `--capabilities` prints the `dev` command list, including `build`, `test`, `verify-samples`, `workflow`, `check-version`, `clean`, `release`, and `self-install`. `ganda nuget outdated` prints `All packages are up to date!` and exits 0. `ganda repo audit` exits 0 (44 passed, 0 failed).

## Review

- Round 1: Blocked — open M1, M2, M3, M4
- Round 2: Done
## Notes

Previous bumps: task 154 (PR #15), audit cleanup task 155 (PR #16).
