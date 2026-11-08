# Review framework

## Budget (by-diff)

- Lines changed: 604
- Effort: 2
- TCB hits: none
- Roster axes: general
- Turn cap: 120 (--max-turns; cursor uncapped)

## Framework — task 151

**Date:** 2026-11-08
**Host task:** kanban/in-progress/151-fix-ganda-repo-audit-and-update-all-nugets-to-latest/
**Diff scope:** branch `task/151-fix-ganda-repo-audit-and-update-all-nugets-to-late` vs merge-base with `master` (07e5aa1), through facda76. Implementation commit: 2b860b7.
**Plan / brief:** task.md Requirements + `## Goal (implementer restatement)`: clean `ganda repo audit`, all NuGets latest, `bin/dev` builds, journal glob (folded 150), build/tests green.
**Effort:** 2 (budget by diff); roster axes: general
**Reviewer roster:** general (reviewer-claude, review oracle)
**Session IDs:** review oracle — reviewer-claude headless (claude-opus-5-5), 2026-11-08

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/`
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims against the repo
- Prior rounds are immutable; new work goes in `round-(N+1)/`
