# Review framework

## Budget (by-diff)

- Lines changed: 257
- Effort: 2
- TCB hits: none
- Roster axes: general
- Turn cap: 120 (--max-turns; cursor uncapped)

## Scope

**Date:** 2026-11-14
**Host task:** kanban/in-progress/158-target-net11-0-only/
**Diff scope:** branch `task/158-target-net11-0-only` vs `master` (merge-base `88e0112`...HEAD, product commit `2c592e5`; kitchen commits `86183bf`, `84d26b6`)
**Plan / brief:** `## Goal (implementer restatement)` and the original task in `task.md` — make net11.0 the only target framework, drop the net10.0 multi-target and the redundant net11.0 overrides, install one GA 11.0.x SDK in CI, and ship that as 2.0.0-beta.2 without cutting a release
**Effort:** 2 (general only)
**Reviewer roster:** general
**Session IDs:** review oracle round 1 (cursor, model grok-4.7, headless ganda task work, session not reported)

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/` (plus framework, disposition, and task.md Results/Session)
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims by reading the diff and the kitchen `## Results` and `### How to validate` record
- Do not re-run a full build, a full test run, an audit, or the hooks; those belong to the implementer's recorded results, the host audit node, and CI
- Exception: one targeted command may check one suspected bug; the finding names the command and the reason. A full build, full test run, or audit is never that command
- A missing or thin recorded result is a finding, not a reason to run the gates
- Prior rounds are immutable; new work goes in `round-(N+1)/`
