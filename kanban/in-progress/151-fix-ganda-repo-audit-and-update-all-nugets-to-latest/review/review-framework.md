# Review framework

## Budget (by-diff)

- Lines changed: 961
- Effort: 3
- TCB hits: none
- Roster axes: general
- Turn cap: 200 (--max-turns; cursor uncapped)

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

## Round 2

**Date:** 2026-11-09
**Diff scope:** fix-pass delta `facda76..9ac7d1f` (round-1 base → fix commit 9ac7d1f), product paths
**Effort / roster:** 2 / general (reviewer-claude, claude-opus-5-5, headless review-2 oracle)

## Round 3

**Date:** 2026-11-09
**Diff scope:** CI follow-up delta `9ac7d1f..8d57233`, product paths: commit 1e77e8a (`tests/timewarp-flexbox-aot-smoke/timewarp-flexbox-aot-smoke.csproj`, +4). Prior M1–M5 re-checked against HEAD.
**Effort / roster:** 3 (budget by diff, 961 lines on the branch) / general (reviewer-claude, claude-opus-5-5, headless review oracle)
