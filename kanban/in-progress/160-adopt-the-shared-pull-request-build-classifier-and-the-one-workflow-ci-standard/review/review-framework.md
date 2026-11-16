# Review framework — task 160

## Budget (by-diff)

- Lines changed: 364
- Effort: 2
- TCB hits: none
- Roster axes: general
- Turn cap: 120 (--max-turns; cursor uncapped)

## Scope

**Date:** 2026-11-16
**Host task:** kanban/in-progress/160-adopt-the-shared-pull-request-build-classifier-and-the-one-workflow-ci-standard/
**Diff scope:** branch `task/160-adopt-the-shared-pull-request-build-classifier-and` vs `master` (three-dot). Product commits `23171a6` (audit `--fix` hook) and `3ab7680` (classifier adoption). Files: `.githooks/prepare-commit-msg`, `.githooks/prepare-commit-msg.cs`, `.github/workflows/workflow.yml`, `.timewarp/dev.jsonc`, `Directory.Packages.props`, `tools/dev-cli/Directory.Build.props`, `tools/dev-cli/endpoints/workflow-command.cs`, plus the task file move.
**Plan / brief:** `## Goal (implementer restatement)` and the original task in `task.md` — one `CI` check on every pull request, shared `PullRequestBuildClassifier` skip for non-build diffs, push filter kept, extra build paths in config.
**Effort:** 2 (general only; by-diff budget, no specialist axes)
**Reviewer roster:** general
**Session IDs:** review oracle round 1 (grok, model grok-4.7, headless ganda task work, session `01a1e4fa-e8d7-7720-bf48-fcd99d4ce052`). Implementer session not reported.

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/` (plus framework, disposition, and task.md Results/Session/Learnings)
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims by reading the diff and the kitchen `## Results` and `### How to validate` record
- Do not re-run a full build, a full test run, an audit, or the hooks; those belong to the implementer's recorded results, the host audit node, and CI
- Exception: one targeted command may check one suspected bug; the finding names the command and the reason. A full build, full test run, or audit is never that command
- A missing or thin recorded result is a finding, not a reason to run the gates
- Prior rounds are immutable; new work goes in `round-(N+1)/`
