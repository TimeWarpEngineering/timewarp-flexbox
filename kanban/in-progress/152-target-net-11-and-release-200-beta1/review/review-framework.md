# Review framework

## Budget (by-diff)

- Lines changed: 215
- Effort: 2
- TCB hits: none
- Roster axes: general
- Turn cap: 120 (--max-turns; cursor uncapped)

## Framework — task 152

**Date:** 2026-11-09
**Host task:** kanban/in-progress/152-target-net-11-and-release-200-beta1/
**Diff scope:** branch `task/152-target-net-11-and-release-200-beta1` vs `master` (merge base 5515b57). The product commit is 14a7714. Master is 2 commits ahead, and both of those are kanban-only.
**Plan / brief:** task.md `## Goal (implementer restatement)` and `## Requirements`. The library, unit tests, and AOT smoke multi-target `net10.0;net11.0`. The version becomes `2.0.0-beta.1`. Install snippets use `--prerelease`. The release notes carry the net10.0 sunset sentence.
**Effort:** 2 (by-diff budget above)
**Reviewer roster:** general
**Session IDs:** reviewer-claude (claude-opus-5-5, headless review oracle under ganda task work)

## Ground rules

- Reviewers are read-only on product code. They write only under `review/round-N/`.
- Severity is bug, suggestion, or nit. Status starts as open.
- Do not invent issues to fill space. Zero issues is a valid outcome.
- Review the diff and the call sites around it. Re-check falsifiable claims against the repo.
- Prior rounds are immutable. New work goes in `round-(N+1)/`.
