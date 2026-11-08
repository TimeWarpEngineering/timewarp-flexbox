# Round 3 — merged findings
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
- Disposition notes: Fixed in 9ac7d1f, verified in round 2, and unchanged in round 3.

### M2 — Severity: bug — Status: fixed
- File: runfiles/port-generated-tests.cs:34
- Description: The porter wrote to `Generated` after the rename to `generated`.
- Disposition notes: Fixed in 9ac7d1f, verified in round 2, and unchanged in round 3.

### M3 — Severity: suggestion — Status: fixed
- File: readme.md:86
- Description: Stale pre-kebab paths in the readme, the flexbox skill, and the integration note.
- Disposition notes: Fixed in 9ac7d1f, verified in round 2, and unchanged in round 3.

### M4 — Severity: suggestion — Status: fixed
- File: .timewarp/line-cap-baseline.json:3
- Description: The baseline had no recorded reason or task id and covered the non-port layout demo.
- Disposition notes: Fixed in 9ac7d1f, verified in round 2. The round 3 audit still passes `source-file-line-cap`.

### M5 — Severity: nit — Status: fixed
- File: tools/dev-cli/global-usings.cs:19
- Description: The comment pointed at a dev.cs Design note that did not exist.
- Disposition notes: Fixed in 9ac7d1f, verified in round 2, and unchanged in round 3.

## Round 3 delta (1e77e8a)

No new findings. The MSB3030 failure reproduces with docs on, and the publish passes as committed. The native smoke binary prints PASS. Release build: 0 warnings. Tests: 1468 passed. Audit: exit 0.

## Duplicates / conflicts

- None (single reviewer).
