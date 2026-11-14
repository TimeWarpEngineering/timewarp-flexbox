# Refresh outdated NuGet packages in TimeWarpEngineering/timewarp-flexbox

## Description

Refresh outdated NuGet packages in TimeWarpEngineering/timewarp-flexbox.
Filter: (all)
Outdated:
  TimeWarp.Nuru 4.0.0-beta.12 -> 4.0.0-beta.13 (patch)
  TimeWarp.Nuru.DevCli 4.0.0-beta.12 -> 4.0.0-beta.13 (patch)
  TimeWarp.SourceGenerators 1.0.0-beta.15 -> 1.0.0-beta.16 (patch)
  Microsoft.CodeAnalysis.CSharp.CodeStyle 5.9.0 -> 5.12.0 (minor)
Edit Directory.Packages.props in this repo only.

Version: set <Version> in source/Directory.Build.props from 2.0.0-beta.2 to 2.0.0-beta.3 in this pull request. The prerelease suffix is -beta.N, never +beta.N. The wave releases it after merge.

## Goal (implementer restatement)

Bump four outdated central package versions in `Directory.Packages.props` (this repo only):
TimeWarp.Nuru and TimeWarp.Nuru.DevCli 4.0.0-beta.12 → 4.0.0-beta.13, TimeWarp.SourceGenerators
1.0.0-beta.15 → 1.0.0-beta.16, Microsoft.CodeAnalysis.CSharp.CodeStyle 5.9.0 → 5.12.0. Bump
`<Version>` in `source/Directory.Build.props` from 2.0.0-beta.2 to 2.0.0-beta.3.

Acceptance criteria:
- The four PackageVersion entries carry the new versions; no other package changes.
- `<Version>` is `2.0.0-beta.3` (hyphen prerelease suffix).
- `./bin/dev build`, `./bin/dev test`, and `ganda repo audit` pass; any new analyzer
  diagnostics from CodeStyle 5.12.0 are fixed, not suppressed.

## Session

- Minted by ganda task wave run 20261114T054552Z-9a0f39
- Implement: bumped TimeWarp.Nuru / TimeWarp.Nuru.DevCli → 4.0.0-beta.13, TimeWarp.SourceGenerators →
  1.0.0-beta.16, Microsoft.CodeAnalysis.CSharp.CodeStyle → 5.12.0 in `Directory.Packages.props`;
  `<Version>` 2.0.0-beta.2 → 2.0.0-beta.3 in `source/Directory.Build.props`. No code changes needed.
- Boy-scout: `ganda repo audit` flagged advisory `vscode-window-icon` (`.vscode/settings.json` missing);
  `ganda repo audit --fix` created it locally (git-ignored, nothing to commit).
- Boy-scout: post-commit attest failed `package-pin-currency` — TimeWarp.Build.Tasks 2.0.1-beta.1 was
  published 2026-11-14 after this task was minted; `ganda repo audit --fix` bumped the pin 2.0.0 →
  2.0.1-beta.1. Build, test, and audit re-run green afterwards.

## Results

Package refresh landed with no source or analyzer fallout. Comments in `tools/dev-cli/` that cite
"Nuru 4.0.0-beta.12" are historical context for an API change and were left as-is.

### Gates

| Gate | Command | Result |
|------|---------|--------|
| Build | `./bin/dev build` | PASS — 0 warnings, 0 errors; packed TimeWarp.Flexbox.2.0.0-beta.3.nupkg (re-run green after Build.Tasks bump) |
| Test | `./bin/dev test` | PASS — 1468 passed, 0 failed, 0 skipped (re-run green after Build.Tasks bump) |
| Dev CLI compile | `dotnet run --file tools/dev-cli/dev.cs -- self-install` | PASS — dev CLI compiles against Nuru/DevCli beta.13 |
| Audit | `ganda repo audit` | PASS — "Repository passes all audit checks" after `--fix` of `vscode-window-icon` and `package-pin-currency` (TimeWarp.Build.Tasks → 2.0.1-beta.1) |

### How to validate

Smoke:
```bash
grep -E 'TimeWarp.Nuru|TimeWarp.SourceGenerators|CSharp.CodeStyle|TimeWarp.Build.Tasks' Directory.Packages.props
grep '<Version>' source/Directory.Build.props
./bin/dev build && ./bin/dev test
```

Expect: Nuru and Nuru.DevCli at 4.0.0-beta.13, SourceGenerators at 1.0.0-beta.16, CodeStyle at 5.12.0,
Build.Tasks at 2.0.1-beta.1;
`<Version>2.0.0-beta.3</Version>`; build has 0 warnings and 0 errors, and all 1468 tests pass.
