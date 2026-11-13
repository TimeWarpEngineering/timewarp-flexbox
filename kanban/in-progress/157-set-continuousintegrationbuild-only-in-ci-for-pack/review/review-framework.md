# Review framework

## Budget (by-diff)

- Lines changed: 146
- Effort: 1
- TCB hits: none
- Roster axes: general
- Turn cap: 80 (--max-turns; cursor uncapped)

## Scope

**Date:** 2026-11-14
**Host task:** kanban/in-progress/157-set-continuousintegrationbuild-only-in-ci-for-pack/
**Diff scope:** branch `task/157-set-continuousintegrationbuild-only-in-ci-for-pack` vs `master` (merge-base...HEAD, product commit `4696bdf`; kitchen commits `c6c2508`, `febb7c4`)
**Plan / brief:** `## Goal (implementer restatement)` and the original task in `task.md` — stop `dev workflow` from forcing `ContinuousIntegrationBuild=true` on every pack, and set it from MSBuild only when `GITHUB_ACTIONS` is true (Mediator `Directory.Build.props` gate)
**Effort:** 1 (general only)
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
