# Review framework

## Budget (by-diff)

- Lines changed: 170
- Effort: 1
- TCB hits: none
- Roster axes: general
- Turn cap: 80 (--max-turns; cursor uncapped)

## Scope

**Date:** 2026-11-10
**Host task:** kanban/in-progress/155-fix-ganda-repo-audit-failures/
**Diff scope:** branch `task/155-fix-ganda-repo-audit-failures` vs `master` (merge base), commits 3ae49f8..bd656ce; product commit bcf9c13
**Plan / brief:** `## Goal (implementer restatement)` and `## Description` in task.md — make `ganda repo audit` exit 0 by fixing five checks (analyzer-suppressions, timewarp-signals-gitignore, workflow-pwsh-shell, memsearch-scaffold, ci-code-coverage)
**Effort:** 2
**Reviewer roster:** general
**Session IDs:** review oracle (claude, model claude-opus-5-5) under ganda task work; implementer session 01a1c63f-bbf6-7132-aca6-eafeec4cb8c2

**Round 2 scope:** fix delta `bd656ce..66f83e7` (product commit 5a15146: `tools/dev-cli/endpoints/test-command.cs`, `.github/workflows/workflow.yml`). Effort 1, roster general (see Budget above).

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/`
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims against the repo
- `ganda repo audit --fix` output is policy (global prefs); it is not flagged as unrequested
- Prior rounds are immutable; new work goes in `round-(N+1)/`
