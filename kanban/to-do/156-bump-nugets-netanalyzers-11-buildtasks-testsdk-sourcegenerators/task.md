# Bump NuGets: NetAnalyzers 11, Build.Tasks, Test.Sdk, SourceGenerators

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

- [ ] Bump the four pins
- [ ] `dotnet build timewarp-flexbox.slnx`: 0 warnings, 0 errors
- [ ] `dotnet test timewarp-flexbox.slnx` green on net10.0 and net11.0
- [ ] `./bin/dev --capabilities` works
- [ ] `ganda repo audit` exit 0 (commit whatever `--fix` changes)
- [ ] `ganda nuget outdated` all up to date
- [ ] Record new diagnostics hit and how each was resolved in Results

## Session

- Created: d7943e80-818a-43bd-a36a-6968aed6437a (2026-11-11)

## Notes

Previous bumps: task 154 (PR #15), audit cleanup task 155 (PR #16).
