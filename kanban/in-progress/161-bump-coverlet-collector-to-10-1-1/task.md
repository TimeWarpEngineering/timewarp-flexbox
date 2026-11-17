# Bump coverlet.collector to 10.1.1

## Description

`ganda nuget outdated` reports one outdated package: `coverlet.collector` 10.1.0 → 10.1.1
(patch, dev-only, used by the test project). All other packages are current.

## Goal (implementer restatement)

Bring the test-only `coverlet.collector` package from 10.1.0 to the 10.1.1 patch release via
central package management in `Directory.Packages.props`.

Acceptance:
- `Directory.Packages.props` pins `coverlet.collector` at `10.1.1`.
- `./bin/dev build` and `./bin/dev test` pass with the new version.
- `ganda nuget outdated` reports no outdated packages.
- No product version change (dev-only patch bump).

## Checklist

- [x] Update `coverlet.collector` to `10.1.1` in `Directory.Packages.props`
- [x] `./bin/dev build` and `./bin/dev test` pass
- [x] `ganda nuget outdated` reports no outdated packages

## Session

- Created: 344df8f6-db0a-4171-88ee-c067d2de3f47 (2026-11-17)
- Implemented 2026-11-17 (implement oracle): bumped coverlet.collector, gates green; audit --fix
  added the local (gitignored) `.vscode/settings.json` window title for `vscode-window-icon`.

## Results

- `Directory.Packages.props:19` now pins `coverlet.collector` at `10.1.1` (was `10.1.0`).
  No other files changed; no product version change (dev-only patch bump).

### Gates

| Gate | Command | Result |
|------|---------|--------|
| Build | `./bin/dev build` | PASS — 0 warnings, 0 errors |
| Test | `./bin/dev test` | PASS — 1468 passed, 0 failed, 0 skipped (coverlet coverage attachment produced) |
| Outdated | `ganda nuget outdated` | PASS — "All packages are up to date!" (coverlet.collector 10.1.1 up to date) |
| Audit | `ganda repo audit` | PASS with 1 advisory warning (`vscode-window-icon`: `.vscode/settings.json` missing); `ganda repo audit --fix` created the gitignored local `.vscode/settings.json`, and its re-audit reported "Repository passes all audit checks" (45 passed, 0 failed, 2 skipped). Nothing tracked changed. |

### How to validate

- Smoke: `grep -n coverlet.collector Directory.Packages.props && ./bin/dev test && ganda nuget outdated`
- Expect: the grep shows `Version="10.1.1"`; tests report 1468 passed / 0 failed and a
  `coverage.cobertura.xml` attachment under `artifacts/coverage/`; `ganda nuget outdated`
  ends with "All packages are up to date!".

## Notes

- Only reference: `Directory.Packages.props:19` (central package management); consumed by
  `test/timewarp-flexbox-tests/timewarp-flexbox-tests.csproj`.
- Patch bump of a dev-only package — no product version change needed.
