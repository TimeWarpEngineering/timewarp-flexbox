# Fix ganda repo audit and update all NuGets to latest

## Description

`ganda repo audit` fails on master (2026-11-08: 21 pass, 17 fail, 2 skipped).
`bin/dev` does not build either, because the dev CLI is behind the current
TimeWarp.Nuru (NURU050 `GitTagCheckService` not registered, `TimeWarp.Mediator`
namespace missing, `PackableProjectService` not found). Bring the repo to a
clean audit and move every NuGet package to its latest version.

## Requirements

- `ganda repo audit` exits 0 (no Error-severity failures)
- Every `PackageVersion` in `Directory.Packages.props` (and any runfile `#:package`)
  is on the latest version. TimeWarp packages come from
  https://nuget.pkg.github.com/TimeWarpEngineering/index.json
- `bin/dev` builds and `dev --capabilities` runs
- Build and tests pass after the bumps

### Failing checks at creation

Auto-fixable (`ganda repo audit --fix`; fixes are policy, commit them):
bin-dev, global-usings-analyzer, glossary, kanban-porcelain-gitignore,
kebab-path-names (23 paths), memsearch-scaffold (warn), nuget-package-urls,
nuru (TimeWarp.Nuru outdated), required-gitignore-entries,
routine-journals-gitignore, runfile-shebang, vscode-settings-gitignore,
vscode-window-icon (warn), workflow-file.

Manual:
- dev-cli-capabilities (blocked on bin/dev building)
- editorconfig-charset (`charset = "utf-8"` is quoted)
- source-file-line-cap (36 C# files over 500 lines). Split them, or record a
  `[ganda.audit]` `.editorconfig` exception with a reason and this task id
  if splitting a ported file is wrong.

## Checklist

- [ ] `ganda repo audit --fix`; commit the result
- [ ] Fix the dev CLI against the current Nuru so `bin/dev` builds
- [ ] Fix editorconfig-charset
- [ ] Resolve source-file-line-cap
- [ ] Update every NuGet package to latest
- [ ] Build and tests green
- [ ] `ganda repo audit` exits 0
- [ ] Task 150 requirements met (`*.journal.json` in `.gitignore`); note it in Results

## Notes

- Task 150 (routine-journals-gitignore) is covered by the `--fix` here.
- At claim, the worktree on-create handler left uncommitted edits in
  `tools/dev-cli/dev.cs` and `tools/dev-cli/global-usings.cs` (Nuru/Mediator
  migration). Review them as the starting point for the dev-CLI fix.

## Session

- Created: b3bc5dc2-e0c9-4b04-ac76-4a1bd4f7a1f3 (2026-11-08)
