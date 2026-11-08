# Round 1 — merged findings
**Date:** 2026-11-08
**Sources:** general

## Counts

| Severity | open | fixed | wontfix |
|----------|------|-------|---------|
| bug | 2 | 0 | 0 |
| suggestion | 2 | 0 | 0 |
| nit | 1 | 0 | 0 |

## Issues

### M1 — Severity: bug — Status: open
- File: .github/workflows/workflow.yml:46
- Description: CI installs only the .NET 10 SDK, but the dev CLI that CI runs (`dotnet run tools/dev-cli/dev.cs -- workflow`) now targets net11.0, so it fails with NETSDK1045 (reproduced with SDK 10.0.403 pinned). CI and release publishing are broken.
- Suggestion: install `10.0.x` and `11.0.x` in `actions/setup-dotnet` (add `dotnet-quality: preview` while .NET 11 is pre-GA).
- Source: general
- Disposition notes:

### M2 — Severity: bug — Status: open
- File: runfiles/port-generated-tests.cs:34
- Description: The generator still writes to `test/timewarp-flexbox-tests/Generated`, which the audit fix renamed to `generated`. Rerunning it on a case-sensitive file system creates a duplicate tree, and the test project then fails with duplicate-type errors.
- Suggestion: use `"generated"`, and update the path comments on lines 8 and 197.
- Source: general
- Disposition notes:

### M3 — Severity: suggestion — Status: open
- File: readme.md:86
- Description: These references still use the old paths: `readme.md` links `LICENSE`, but the file is now `license`, so the link 404s on GitHub. `skills/flexbox/SKILL.md:293,296` use `Generated/` and `Style/Style.cs`. `.agent/workspace/2025-12-17-t12-00-00-integration-testing-investigation.md:153` uses the old filename.
- Suggestion: update them to the kebab-case paths.
- Source: general
- Disposition notes:

### M4 — Severity: suggestion — Status: open
- File: .timewarp/line-cap-baseline.json:3
- Description: The line-cap baseline replaces the `[ganda.audit]` exception "with a reason and this task id" that the task asked for, and the repo records no reason or task id for it. It also covers `samples/layout-demo/layout-demo.cs`, which is not a Yoga port.
- Suggestion: record the reason and task 151 in the repo next to the baseline or in `.editorconfig` `[ganda.audit]`. Either trim layout-demo.cs under 500 lines and remove its entry, or justify keeping it whole.
- Source: general
- Disposition notes:

### M5 — Severity: nit — Status: open
- File: tools/dev-cli/global-usings.cs:19
- Description: The comment sends the reader to the dev.cs Design region for the Mediator move, but that region does not mention it.
- Suggestion: drop the cross-reference or add the note to dev.cs Design.
- Source: general
- Disposition notes:

## Duplicates / conflicts

- None (single reviewer).
