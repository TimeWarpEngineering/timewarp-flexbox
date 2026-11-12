# Round 1 — general
**Date:** 2026-11-12
**Scope reviewed:** branch `task/156-bump-nugets-netanalyzers-11-buildtasks-testsdk-sou` vs merge base `6f18251` (`master`, head `ef610e9`, product commit `bc7460b`): `Directory.Packages.props`, `.editorconfig`, `tools/dev-cli/.globalconfig`, `test/timewarp-flexbox-tests/timewarp-flexbox-tests.csproj`, `runfiles/*.cs`, `samples/layout-demo/layout-demo.cs`, `benchmarks/timewarp-flexbox-benchmarks/*.cs`, `tests/timewarp-flexbox-aot-smoke/program.cs`, `spikes/cramer/*.cs`, `source/timewarp-flexbox/**/*.cs`, `test/timewarp-flexbox-tests/**/*.cs`, and `task.md`

## Summary

Commit `bc7460b` bumps the four outdated pins in `Directory.Packages.props` (`Microsoft.CodeAnalysis.NetAnalyzers` 11.0.100, `TimeWarp.SourceGenerators` 1.0.0-beta.14, `TimeWarp.Build.Tasks` 2.0.0, `Microsoft.NET.Test.Sdk` 18.12.0), converts/adds `#region Purpose` across 126 in-repo C# files for `TWA0004`, mirrors the 35 `.timewarp/line-cap-baseline.json` counts into per-file `dotnet_diagnostic.TWA0038.max_lines` sections in `.editorconfig`, adds `WriteFixieMainGlobalConfig` in `timewarp-flexbox-tests.csproj` for package-owned `Fixie.Main.cs`, and adds `tools/dev-cli/.globalconfig` for `TimeWarp.Nuru.DevCli` content files.

Independent verification in the claim worktree confirmed `ganda nuget outdated` (19/19 up to date), `ganda repo audit` (44 passed, 0 failed, 2 skipped), `dotnet build timewarp-flexbox.slnx` in Debug and Release (0 warnings, 0 errors), `dotnet test timewarp-flexbox.slnx` (1468 passed on `net10.0` and 1468 passed on `net11.0`), `TimeWarp.Build.Tasks` 2.0.0 `CommitHash`/`CommitDate` `AssemblyMetadata` injection, `./bin/dev --capabilities`, `dev verify-samples`, and Native AOT smoke publish/execution on `net10.0` and `net11.0`. Every entry in `.timewarp/line-cap-baseline.json` matches its `.editorconfig` `TWA0038.max_lines` value and line count on disk, and `.timewarp/suppression-baseline.json` did not grow.

Four issues were found by building standalone runfiles and inspecting Roslyn's `AnalyzerConfig` / `AnalyzerConfigSet` implementation: `runfiles/port-generated-tests.cs` fails to compile with two NetAnalyzers 11 `CA1307` errors, `WriteFixieMainGlobalConfig` writes unescaped Windows backslashes into a globalconfig section header (and rewrites the file on every build), `tools/dev-cli/.globalconfig` raises `TWA0038.max_lines` to 704 for in-repo `tools/dev-cli/**/*.cs` files contrary to its comment and `Results`, and `spikes/cramer/test-shrink.cs` has a duplicate blank line (`IDE2000`).

Goal vs task: `## Goal (implementer restatement)` matches the task intent and constraints (`ganda nuget outdated` clean, fix new NetAnalyzers 11 and SourceGenerators diagnostics in code, justify any non-applicable rule exemptions, preserve TFMs and library API behavior). Moving `TimeWarp.Build.Tasks` to `2.0.0` and `Microsoft.NET.Test.Sdk` to `18.12.0` past the 2026-11-11 description table was required to satisfy "`ganda nuget outdated` reports all packages up to date" on 2026-11-12; no misread. The restatement's build gate named only `timewarp-flexbox.slnx` and omitted standalone `runfiles/*.cs`, which allowed `CA1307` in `runfiles/port-generated-tests.cs` to slip past verification.

## Issues

### Issue 1 — Severity: bug
- File: runfiles/port-generated-tests.cs:339
- Description: `dotnet build runfiles/port-generated-tests.cs` fails on `net11.0` with two `CA1307` errors from `Microsoft.CodeAnalysis.NetAnalyzers` 11.0.100 (`TreatWarningsAsErrors=true` via `Directory.Build.props`): `'string.EndsWith(char)' has a method overload that takes a 'StringComparison' parameter` on `token.EndsWith('f') || token.EndsWith('F')`. Commit `bc7460b` modified `runfiles/port-generated-tests.cs` without compiling it, and `task.md` (`## Results`, line 58) inaccurately reports "**NetAnalyzers 11** and **TW00xx**: no diagnostics."
- Suggestion: Fix `Converter.FloatLiteral` in code (do not add `CA1307` to `NoWarn`), e.g. `token.EndsWith('f', StringComparison.OrdinalIgnoreCase)` or `token is [.., 'f' or 'F']`. Verify `dotnet build runfiles/port-generated-tests.cs` exits 0 with 0 warnings and 0 errors, and update `task.md` (`## Results` and `### How to validate`) to document the `CA1307` diagnostic and include building the runfiles.
- Status: open

### Issue 2 — Severity: bug
- File: test/timewarp-flexbox-tests/timewarp-flexbox-tests.csproj:38
- Description: `WriteFixieMainGlobalConfig` writes `[$(_FixieMainPath)]` using raw `%(_FixieMain.FullPath)`. In Roslyn's `AnalyzerConfig` (`SectionNameLexer` and `AnalyzerConfigSet.GetOptionsForSourcePath`), global config section headers are matched against forward-slash-normalized paths (`PathUtilities.CollapseWithForwardSlash`) and `\` inside `[...]` is treated as an escape character. On Windows, `%(_FixieMain.FullPath)` contains backslashes (`C:\Users\...\.nuget\packages\fixie\4.2.0\build\Fixie.Main.cs`), so `UnescapeSectionName` strips the backslashes and the section never matches `Fixie.Main.cs`, leaving Windows builds broken with `error TWA0004`. In addition, `<WriteLinesToFile>` at line 42 omits `WriteOnlyWhenDifferent="true"` (and `<FileWrites>` registration), rewriting `fixie-main.generated.globalconfig` with a fresh timestamp before every `CoreCompile` and defeating incremental build up-to-date checks.
- Suggestion: Normalize backslashes to forward slashes before writing the section header (`<_FixieMainSectionPath>$(_FixieMainPath.Replace('\', '/'))</_FixieMainSectionPath>` and `[$(_FixieMainSectionPath)]`), add `WriteOnlyWhenDifferent="true"` on `<WriteLinesToFile>`, and add `<FileWrites Include="$(_FixieMainConfig)" />` so `Clean` removes it.
- Status: open

### Issue 3 — Severity: suggestion
- File: tools/dev-cli/.globalconfig:9
- Description: `tools/dev-cli/.globalconfig` sets unsectioned `dotnet_diagnostic.TWA0038.max_lines = 704` with `is_global = true`. Because root `.editorconfig` has no `[*.cs]` `dotnet_diagnostic.TWA0038.max_lines = 500` rule (only the 35 explicit Yoga port/fixture sections), Roslyn's `AnalyzerConfigSet` applies `max_lines = 704` to every file in the `tools/dev-cli` compilation — including all 7 in-repo `.cs` files under `tools/dev-cli/` (`dev.cs`, `global-usings.cs`, and `endpoints/*.cs`), not just the two `TimeWarp.Nuru.DevCli` NuGet cache files (`release-command.cs` and `nuget-version-service.cs`). That contradicts both the comment in `tools/dev-cli/.globalconfig` ("In-repo files stay on 500") and `task.md` line 57 ("In-repo files are not covered by that config").
- Suggestion: Either set `dotnet_diagnostic.TWA0038.max_lines = 500` under `[*.cs]` in root `.editorconfig` (in Roslyn's `AnalyzerConfigSet`, directory-scoped `.editorconfig` sections override unsectioned `is_global = true` defaults for files under the repo root while leaving `~/.nuget/packages/...` outside the repo root on `704`, and the 35 per-file sections later in `.editorconfig` still override `[*.cs]`), or replace `tools/dev-cli/.globalconfig` with a `BeforeTargets="CoreCompile"` generated globalconfig in `tools/dev-cli/Directory.Build.props` scoped to the exact forward-slash paths of `release-command.cs` (`609`) and `nuget-version-service.cs` (`704`) with `WriteOnlyWhenDifferent="true"`. Reconcile the comment and `task.md` `## Results` to match.
- Status: open

### Issue 4 — Severity: nit
- File: spikes/cramer/test-shrink.cs:6
- Description: Commit `bc7460b` added a second consecutive blank line after `#endregion` in `spikes/cramer/test-shrink.cs`, triggering `IDE2000` ("Avoid multiple blank lines") on the edited header.
- Suggestion: Remove the extra blank line at `spikes/cramer/test-shrink.cs:6`.
- Status: open
