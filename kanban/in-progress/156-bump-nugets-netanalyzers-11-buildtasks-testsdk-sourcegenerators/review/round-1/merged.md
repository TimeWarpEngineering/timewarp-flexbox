# Round 1 — merged findings
**Date:** 2026-11-12
**Sources:** general

## Counts

| Severity | open | fixed | wontfix |
|----------|------|-------|---------|
| bug | 2 | 0 | 0 |
| suggestion | 1 | 0 | 0 |
| nit | 1 | 0 | 0 |

## Issues

### M1 — Severity: bug — Status: open
- File: runfiles/port-generated-tests.cs:339
- Description: `dotnet build runfiles/port-generated-tests.cs` fails on `net11.0` with two `CA1307` errors from `Microsoft.CodeAnalysis.NetAnalyzers` 11.0.100 (`'string.EndsWith(char)' has a method overload that takes a 'StringComparison' parameter` on `token.EndsWith('f') || token.EndsWith('F')`). Commit `bc7460b` edited this file without compiling it, and `task.md` `## Results` inaccurately states that NetAnalyzers 11 produced no diagnostics.
- Suggestion: Fix `Converter.FloatLiteral` in code (e.g., `token.EndsWith('f', StringComparison.OrdinalIgnoreCase)` or `token is [.., 'f' or 'F']`, without adding `CA1307` to `NoWarn`), verify `dotnet build` passes on `runfiles/*.cs` with 0 warnings/0 errors, and update `task.md` `## Results` and `### How to validate` to record `CA1307` and its resolution.
- Source: general
- Disposition notes:

### M2 — Severity: bug — Status: open
- File: test/timewarp-flexbox-tests/timewarp-flexbox-tests.csproj:38
- Description: `WriteFixieMainGlobalConfig` writes `[$(_FixieMainPath)]` from raw `%(_FixieMain.FullPath)`. Roslyn's `AnalyzerConfig` matches global config section names against forward-slash paths and treats `\` inside `[...]` as an escape character, so Windows paths (`C:\...\Fixie.Main.cs`) never match and fail with `TWA0004`. In addition, `<WriteLinesToFile>` lacks `WriteOnlyWhenDifferent="true"` (and `<FileWrites>` registration), rewriting `fixie-main.generated.globalconfig` on every build and defeating incremental `CoreCompile` up-to-date checks.
- Suggestion: Normalize `$(_FixieMainPath)` with `.Replace('\', '/')` in the section header, set `WriteOnlyWhenDifferent="true"` on `<WriteLinesToFile>`, and add `<FileWrites Include="$(_FixieMainConfig)" />`.
- Source: general
- Disposition notes:

### M3 — Severity: suggestion — Status: open
- File: tools/dev-cli/.globalconfig:9
- Description: Unsectioned `dotnet_diagnostic.TWA0038.max_lines = 704` in `tools/dev-cli/.globalconfig` (with no `[*.cs]` `dotnet_diagnostic.TWA0038.max_lines = 500` in `.editorconfig`) raises the `TWA0038` limit to 704 for all 7 in-repo `.cs` files under `tools/dev-cli/`, contradicting both the comment in `.globalconfig` ("In-repo files stay on 500") and `task.md` `## Results` ("In-repo files are not covered by that config").
- Suggestion: Either add `dotnet_diagnostic.TWA0038.max_lines = 500` under `[*.cs]` in root `.editorconfig` (which overrides unsectioned globalconfig for repo files while keeping NuGet cache files outside the repo tree on `704`, and still lets the 35 per-file sections at the bottom of `.editorconfig` override `[*.cs]`), or replace `tools/dev-cli/.globalconfig` with a `BeforeTargets="CoreCompile"` path-scoped generated globalconfig in `tools/dev-cli/Directory.Build.props` targeting only `release-command.cs` (`609`) and `nuget-version-service.cs` (`704`) with forward-slash paths and `WriteOnlyWhenDifferent="true"`. Update the comment and `task.md` accordingly.
- Source: general
- Disposition notes:

### M4 — Severity: nit — Status: open
- File: spikes/cramer/test-shrink.cs:6
- Description: Commit `bc7460b` added a duplicate blank line after `#endregion` in `spikes/cramer/test-shrink.cs` (`IDE2000`).
- Suggestion: Remove the extra blank line at `spikes/cramer/test-shrink.cs:6`.
- Source: general
- Disposition notes:

## Duplicates / conflicts

- None (single reviewer).
