# Disposition — task 153

**Date:** 2026-11-09
**Outcome:** accepted-exceptions
**Rounds:** 1
**Final open count:** 0

## Summary

`dev verify-samples` now discovers, builds, runs, and checks every sample under `samples/` through TimeWarp.Amuru with timeouts. It keeps output outside the repo and stops `dev workflow` on failure. The review re-ran `verify-samples` (exit 0, tree clean, scratch removed) and `ganda repo audit` (exit 0). There are no bugs. One suggestion and two nits are accepted as wontfix with rationale. The implementer restatement matches the task, so there is no misread.

## Exception log (if accepted-exceptions)

| ID | Severity | Rationale | Decided by |
|----|----------|-----------|------------|
| M1 | suggestion | `IsUnderBuildOutput` uses the absolute path. It fails closed (exit 1, a misleading message) only for checkouts under a `bin`/`obj` directory, and CI and ganda worktrees are not affected. Follow-up candidate. | review oracle |
| M2 | nit | `{out}` is always `output.html`. That is fine for the single HTML sample; generalize when a second output-writing sample lands. | review oracle |
| M3 | nit | The args file has no quoting. No current argument needs it. | review oracle |

## Escalations

- None.
