# Review framework

## Budget (by-diff)

- Lines changed: 651
- Effort: 2
- TCB hits: none
- Roster axes: general
- Turn cap: 120 (--max-turns; cursor uncapped)

## Scope — task 153

**Date:** 2026-11-09
**Host task:** kanban/in-progress/153-make-dev-verify-samples-actually-run-the-samples/
**Diff scope:** branch `task/153-make-dev-verify-samples-actually-run-the-samples` vs merge-base with `master` (commit 63e4120 plus the kanban commits). The product files are `tools/dev-cli/endpoints/verify-samples-command.cs`, `tools/dev-cli/endpoints/verify-samples-discovery.cs` (new), `samples/layout-demo/verify-samples.args` (new), and a one-line comment in `samples/layout-demo/layout-demo.cs`.
**Plan / brief:** Replace the `dev verify-samples` stub. Discover samples under `samples/`, build and run each one on the TFM it declares (with a timeout), check the output files it declares, keep the tree clean, and exit non-zero on failure so `dev workflow` stops.
**Effort:** 2. One general reviewer (the review oracle), who also checked the plan alignment.
**Reviewer roster:** general
**Session IDs:** review oracle, claude-opus-5-5 (ganda task work, review node, round 1)

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/`
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims against the repo
- Prior rounds are immutable; new work goes in `round-(N+1)/`
