# Review framework

## Budget (by-diff)

- Lines changed: 1813
- Effort: 3
- TCB hits: none
- Roster axes: general
- Turn cap: 200 (--max-turns; cursor uncapped)

## Scope

**Date:** 2026-11-12
**Host task:** kanban/in-progress/156-bump-nugets-netanalyzers-11-buildtasks-testsdk-sourcegenerators/
**Diff scope:** branch `task/156-bump-nugets-netanalyzers-11-buildtasks-testsdk-sou` vs `master` (merge base `6f18251`, head `ef610e9`; product commit `bc7460b`, 132 files changed)
**Plan / brief:** `## Goal (implementer restatement)` and `## Description` / `## Requirements` in `task.md` — bump outdated central NuGet pins (`Microsoft.CodeAnalysis.NetAnalyzers` 11.0.100, `TimeWarp.Build.Tasks` 2.0.0, `Microsoft.NET.Test.Sdk` 18.12.0, `TimeWarp.SourceGenerators` 1.0.0-beta.14), resolve new analyzer diagnostics in code without unwarranted suppressions, keep TFMs and public API behavior unchanged, and pass build, test, dev CLI, and `ganda repo audit` gates
**Effort:** 3
**Reviewer roster:** general
**Session IDs:** review oracle round 1 (cursor, model gemini-4-carbon-high, headless ganda task work)

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/` (plus framework/session metadata)
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims against the repo
- `ganda repo audit --fix` output is policy (global prefs); it is not flagged as unrequested
- Prior rounds are immutable; new work goes in `round-(N+1)/`
