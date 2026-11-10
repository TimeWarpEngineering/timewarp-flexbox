# Round 2 — general
**Date:** 2026-11-10
**Scope reviewed:** delta since round 1 (`bd656ce..66f83e7`). The product commit is 5a15146: `tools/dev-cli/endpoints/test-command.cs` and `.github/workflows/workflow.yml`. The merge 66f83e7 adds no product change beyond 5a15146.

## Summary

The fix pass closes round-1 M1 and M2. `dev test` now deletes and recreates `artifacts/coverage`. It then runs `dotnet test` with `--collect "XPlat Code Coverage"` and `--results-directory artifacts/coverage`. A passing suite fails the command when no `coverage.cobertura.xml` was written. The `--quiet` and streaming paths both reach that check. `dev workflow` calls `dev test` on both the release and non-release branches, so CI always produces the directory before the "Upload Coverage" step runs. That step now uses `if-no-files-found: error`. The artifact `name` stays `coverage`.

I checked these claims against the repo:
- `dotnet run tools/dev-cli/dev.cs -- test --quiet` printed "Tests completed successfully!". It wrote two Cobertura files under `artifacts/coverage/<guid>/`, one for each TFM (net10.0 and net11.0).
- `ganda repo audit` reported 44 passed, 0 failed, 2 skipped.
- The test project uses VSTest (`Microsoft.NET.Test.Sdk` + `Fixie.TestAdapter`) and references `coverlet.collector`, so the `--collect` data collector applies.
- `artifacts/` is gitignored, so deleting and recreating it touches no tracked files.

There is no suppression, stub, or audit-only change in the delta. The coverage claim the audit measures is now true.

## Issues

<!-- No new findings on the fix delta. -->

None.

## Resolved prior

- M1 (bug): fixed. CI now collects coverage into the uploaded path, and an empty result fails both `dev test` and the upload step.
- M2 (nit): fixed. The step is named "Upload Coverage".
