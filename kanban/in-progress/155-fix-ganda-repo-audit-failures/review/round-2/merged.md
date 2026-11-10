# Round 2 — merged findings
**Date:** 2026-11-10
**Sources:** general

## Counts

| Severity | open | fixed | wontfix |
|----------|------|-------|---------|
| bug | 0 | 1 | 0 |
| suggestion | 0 | 0 | 0 |
| nit | 0 | 1 | 0 |

## Issues

### M1 — Severity: bug — Status: fixed
- File: .github/workflows/workflow.yml:96, tools/dev-cli/endpoints/test-command.cs:71
- Description (round 1): the coverage upload pointed at `artifacts/coverage`, which CI never created. That made the audit's `ci-code-coverage` check pass without any coverage being collected.
- Source: general
- Disposition notes: fixed in 5a15146. `dev test` collects XPlat Code Coverage into `artifacts/coverage` and fails when the suite passes but no Cobertura file is written. The upload step uses `if-no-files-found: error`. Round 2 verified this locally: two Cobertura files (one per TFM), and `ganda repo audit` reported 44 passed, 0 failed.

### M2 — Severity: nit — Status: fixed
- File: .github/workflows/workflow.yml:96
- Description (round 1): the step name `coverage` was lowercase.
- Source: general
- Disposition notes: fixed in 5a15146. The step is renamed "Upload Coverage", and the artifact name stays `coverage` for the audit line-rate reader.

## New findings

None.

## Duplicates / conflicts

- None (single reviewer).
