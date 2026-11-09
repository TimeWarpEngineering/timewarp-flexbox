# Review framework

## Budget (by-diff)

- Lines changed: 83
- Effort: 1
- TCB hits: none
- Roster axes: general
- Turn cap: 80 (--max-turns; cursor uncapped)

## Scope

**Date:** 2026-11-09
**Host task:** kanban/in-progress/154-update-all-nuget-packages-to-latest/
**Diff scope:** branch `task/154-update-all-nuget-packages-to-latest` vs `origin/master` (merge-base e6898b6, head 40ef7b7)
**Plan / brief:** bump the central NuGet pins to the latest (Amuru/Amuru.Tools 3.0.0-beta.2, Nuru/Nuru.DevCli 4.0.0-beta.12, SourceGenerators 1.0.0-beta.12); move the dev CLI handlers off the removed `ICommandHandler` (Mediator 15)
**Effort:** 1 (general only)
**Reviewer roster:** general
**Session IDs:** review oracle (claude-opus-5-5, ganda task work headless)

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/`
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims against the repo
- Prior rounds are immutable; new work goes in `round-(N+1)/`
