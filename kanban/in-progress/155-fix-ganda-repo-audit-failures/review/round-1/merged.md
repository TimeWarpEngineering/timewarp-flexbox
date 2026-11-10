# Round 1 — merged findings
**Date:** 2026-11-10
**Sources:** general

## Counts

| Severity | open | fixed | wontfix |
|----------|------|-------|---------|
| bug | 1 | 0 | 0 |
| suggestion | 0 | 0 | 0 |
| nit | 1 | 0 | 0 |

## Issues

### M1 — Severity: bug — Status: open
- File: .github/workflows/workflow.yml:96
- Description: The `coverage` upload step points at `artifacts/coverage`, which nothing in CI creates. `dev test` runs `dotnet test` without `--collect "XPlat Code Coverage"` or `--results-directory`, so `coverlet.collector` never runs. The step only makes the ganda audit `ci-code-coverage` check pass ("CI collects code coverage and uploads"), and that claim is false. Every run uploads an empty artifact with a warning.
- Suggestion: Make CI collect coverage into `artifacts/coverage`. The change goes in `tools/dev-cli/endpoints/test-command.cs` (collect plus results directory), with local proof that `coverage.cobertura.xml` lands there. Then consider `if-no-files-found: error`. The other option is to drop `coverlet.collector` if coverage is not wanted.
- Source: general
- Disposition notes:

### M2 — Severity: nit — Status: open
- File: .github/workflows/workflow.yml:96
- Description: The step name `coverage` is lowercase. The other steps use Title Case.
- Suggestion: Rename it to "Upload Coverage" while fixing M1, unless the audit fixer requires the name.
- Source: general
- Disposition notes:

## Duplicates / conflicts

- None (single reviewer).
