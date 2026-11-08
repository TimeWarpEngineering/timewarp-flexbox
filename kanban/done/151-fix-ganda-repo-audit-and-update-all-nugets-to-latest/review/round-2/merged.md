# Round 2 — merged findings
**Date:** 2026-11-09
**Sources:** general

## Counts

| Severity | open | fixed | wontfix |
|----------|------|-------|---------|
| bug | 0 | 2 | 0 |
| suggestion | 0 | 2 | 0 |
| nit | 0 | 1 | 0 |

## Issues

### M1 — Severity: bug — Status: fixed
- File: .github/workflows/workflow.yml:43
- Description: CI installed only .NET 10, but the dev CLI targets net11.0.
- Disposition notes: 9ac7d1f adds a `11.0.x` preview setup step after `10.0.x`. Verified round 2.

### M2 — Severity: bug — Status: fixed
- File: runfiles/port-generated-tests.cs:34
- Description: The porter wrote to `Generated` after the rename to `generated`.
- Disposition notes: The output dir and both path comments use `generated`. Verified round 2.

### M3 — Severity: suggestion — Status: fixed
- File: readme.md:86
- Description: Stale pre-kebab paths in the readme, the flexbox skill, and the integration note.
- Disposition notes: Every updated path exists on disk, and a grep finds no remaining stale forms. Verified round 2.

### M4 — Severity: suggestion — Status: fixed
- File: .timewarp/line-cap-baseline.json:3
- Description: The baseline had no recorded reason or task id and covered the non-port layout demo.
- Disposition notes: The `.editorconfig` `[ganda.audit]` comment names task 151 and the reason. The layout demo is at 497 lines and has left the baseline. The audit passes `source-file-line-cap`. Verified round 2.

### M5 — Severity: nit — Status: fixed
- File: tools/dev-cli/global-usings.cs:19
- Description: The comment pointed at a dev.cs Design note that did not exist.
- Disposition notes: dev.cs Design now states that `ICommand` and `Unit` live in `TimeWarp.Mediator`. Verified round 2.

## Duplicates / conflicts

- None (single reviewer). No new findings this round.
