# Review framework

## Budget (by-diff)

- Lines changed: 73
- Effort: 1
- TCB hits: none
- Roster axes: general
- Turn cap: 80 (--max-turns; cursor uncapped)

## Scope

**Date:** 2026-11-14
**Host task:** kanban/in-progress/159-refresh-outdated-nu-get-packages-in-time-warp-engineering-timewarp-flexbox/
**Diff scope:** branch `task/159-refresh-outdated-nu-get-packages-in-time-warp` vs `master`. Product commits `8d873a7` and `70a2ceb`. Files: `Directory.Packages.props`, `source/Directory.Build.props`.
**Plan / brief:** `## Goal (implementer restatement)` and the original task in `task.md` — bump the four outdated central pins, set `<Version>` to `2.0.0-beta.3` (hyphen suffix), and leave the release to the wave after merge.
**Effort:** 1 (general only)
**Reviewer roster:** general
**Session IDs:** review oracle round 1 (grok, model grok-4.7, headless ganda task work; session id not reported by the harness). Implementer session `42c25a7c-9214-4513-89cf-e271eb160f90`.

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/` (plus framework, disposition, and task.md Results/Session/Learnings)
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims by reading the diff and the kitchen `## Results` and `### How to validate` record
- Do not re-run a full build, a full test run, an audit, or the hooks; those belong to the implementer's recorded results, the host audit node, and CI
- Exception: one targeted command may check one suspected bug; the finding names the command and the reason. A full build, full test run, or audit is never that command
- A missing or thin recorded result is a finding, not a reason to run the gates
- Prior rounds are immutable; new work goes in `round-(N+1)/`
