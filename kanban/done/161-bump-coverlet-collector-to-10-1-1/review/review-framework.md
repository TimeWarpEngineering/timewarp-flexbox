# Review framework — task 161

**Date:** 2026-11-17
**Host task:** kanban/in-progress/161-bump-coverlet-collector-to-10-1-1/
**Diff scope:** branch `task/161-bump-coverlet-collector-to-10-1-1` vs merge-base `99c7264` (`master`). Product change is `Directory.Packages.props` only (`coverlet.collector` 10.1.0 → 10.1.1). Kitchen moves live in the same range. The branch is one commit behind `master`: `44c1989` lands the same to-do → in-progress rename with no content change.
**Plan / brief:** Patch-bump the dev-only `coverlet.collector` pin to 10.1.1 through central package management. Build, test, and `ganda nuget outdated` must pass. No product version change.
**Effort:** 1 (general only)
**Reviewer roster:** general
**Session IDs:** grok `01a1e902-3db7-71d2-9c17-0cc867cdb63c` (reviewer); implementer-claude `c0012a08-8d9a-4043-ba1d-9db998fa59a6`

## Budget (by-diff)

- Lines changed: 81
- Effort: 1
- TCB hits: none
- Roster axes: general
- Turn cap: 80 (--max-turns; cursor uncapped)

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/`
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims by reading the diff and the kitchen `## Results` and `### How to validate` record
- Do not re-run a full build, a full test run, an audit, or the hooks; those belong to the implementer's recorded results, the host audit node, and CI
- Exception: one targeted command may check one suspected bug; the finding names the command and the reason. A full build, full test run, or audit is never that command
- A missing or thin recorded result is a finding, not a reason to run the gates
- Prior rounds are immutable; new work goes in `round-(N+1)/`
