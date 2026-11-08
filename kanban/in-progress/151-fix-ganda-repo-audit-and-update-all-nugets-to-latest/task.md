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
- Folded from task 150: root `.gitignore` has the `*.journal.json` glob
  (not the six exact names), `git ls-files '*.journal.json'` is empty
  (`git rm --cached` any tracked journal, never product `task.md`), and
  `git check-ignore -v kanban/to-do/task-work.journal.json` hits that line
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

- [x] `ganda repo audit --fix`; commit the result
- [x] Fix the dev CLI against the current Nuru so `bin/dev` builds
- [x] Fix editorconfig-charset
- [x] Resolve source-file-line-cap
- [x] Update every NuGet package to latest
- [x] Build and tests green
- [x] `ganda repo audit` exits 0
- [x] Folded 150: `*.journal.json` ignored, no tracked journals, check-ignore confirms

## Notes

- Task 150 (routine-journals-gitignore) was folded into this task and archived on 2026-11-08.
- At claim, the worktree on-create handler left uncommitted edits in
  `tools/dev-cli/dev.cs` and `tools/dev-cli/global-usings.cs` (Nuru/Mediator
  migration). Review them as the starting point for the dev-CLI fix.

## Goal (implementer restatement)

Bring this repo to a clean `ganda repo audit` (exit 0, no Error-severity failures) and put every NuGet pin on its latest version so `bin/dev` builds and the test suite still passes.

Acceptance:

- `ganda repo audit --fix` lands, and a later `ganda repo audit` exits 0.
- The dev CLI registers the services the current TimeWarp.Nuru.DevCli endpoints require, `bin/dev` builds, and `dev --capabilities` runs.
- `.editorconfig` uses an unquoted `charset = utf-8`.
- Every C# file is at or under the 500-line audit cap, or a ported Yoga file that should stay whole has a `[ganda.audit]` exception that names this task and why it was not split.
- Every `PackageVersion` in `Directory.Packages.props` and every runfile `#:package` is the latest version. TimeWarp packages resolve from the GitHub TimeWarpEngineering NuGet feed.
- Build and tests pass after those bumps.
- Root `.gitignore` ignores `*.journal.json` as one glob (not six exact names), no `*.journal.json` file is tracked, and `git check-ignore -v` on `kanban/to-do/task-work.journal.json` matches that glob.

## Session

- Created: b3bc5dc2-e0c9-4b04-ac76-4a1bd4f7a1f3 (2026-11-08)
- Implementation: grok task-work (2026-11-08)
- Line-cap: shrink-only `.timewarp/line-cap-baseline.json` for the 36 Yoga ports and generated fixtures (task 151). Splitting invents boundaries the port must not have. `source-file-line-cap.severity = off` would hide later over-cap files. Origin has no baseline, so this first ratchet is legal.
- Implement oracle: implemented by implementer-grok (grok, vendor xai, model grok-4.7, response model grok-4.7-build), session 01a1bbf3-b482-76a2-b863-84e42461802a, max-turns 500, Done — 2026-11-08T16:47:12Z

## Results

`ganda repo audit` is clean, every central pin is the latest version `ganda nuget outdated` reports, `bin/dev` builds, and the Flexbox suite passes.

- Audit `--fix` landed the auto-fixable checks. Kebab renames needed `ganda repo audit --fix --checks kebab-path-names` because that check does not run on a blind `--fix`. `timewarp-flexbox.slnx` points at the renamed benchmarks project. `.editorconfig` charset is unquoted `charset = utf-8`.
- Dev CLI targets net11.0 and registers `IRepoCleanService`, `NuGetVersionService`, `IRepoConfigService`, and `IPackableProjectService`. Handlers return `Task<Unit>` for published Nuru 4.0.0-beta.11 / Mediator Contracts 14. Product projects stay net10.0. `runfiles/build.cs` and `runfiles/test.cs` take unversioned `TimeWarp.Amuru` and `TimeWarp.Amuru.Tools` so central management supplies 3.0.0-beta.1.
- Pins (nuget.org; confirming `ganda nuget outdated` reports all 19 up to date): TimeWarp.Amuru 3.0.0-beta.1, TimeWarp.Amuru.Tools 3.0.0-beta.1, TimeWarp.Terminal 1.0.2, TimeWarp.Build.Tasks 1.0.0, Microsoft.CodeAnalysis.BannedApiAnalyzers 5.6.0, TimeWarp.Nuru 4.0.0-beta.11, TimeWarp.Nuru.DevCli 4.0.0-beta.11, TimeWarp.SourceGenerators 1.0.0-beta.11, TimeWarp.Fixie 3.1.0, Fixie.TestAdapter 4.2.0, Microsoft.NET.Test.Sdk 18.11.0, Shouldly 4.3.0, coverlet.collector 10.1.0, BenchmarkDotNet 0.15.8, Roslynator.Analyzers / CodeAnalysis.Analyzers / Formatting.Analyzers 5.0.1, Microsoft.CodeAnalysis.NetAnalyzers 10.0.403, Microsoft.CodeAnalysis.CSharp.CodeStyle 5.9.0. The GitHub TimeWarpEngineering feed returned NAME_UNKNOWN for TimeWarp.Nuru and TimeWarp.Amuru. `nuget.config` maps only `TimeWarp.Flexbox*` to that feed, so these dependencies resolve from nuget.org.
- Folded task 150: `.gitignore` line 423 is `*.journal.json`. `git ls-files '*.journal.json'` is empty. `git check-ignore -v kanban/to-do/task-work.journal.json` matches that line.
- `dotnet build timewarp-flexbox.slnx`: 0 warnings, 0 errors. `dotnet test timewarp-flexbox.slnx`: 1468 passed, 0 failed, 0 skipped. `runfiles/build.cs` and `runfiles/test.cs` both exit 0 (Fixie: 1468 passed). `./bin/dev --capabilities` returns the timewarp-flexbox dev CLI description.
- This branch stays 5 commits behind `origin/master`. Those commits are kanban land commits only and were not merged.

### How to validate

Smoke: from this worktree run `ganda repo audit`, then `./bin/dev --capabilities`, then `dotnet test timewarp-flexbox.slnx`.

Expect: audit exits 0 with no Error-severity failures, capabilities JSON names this repo's development CLI, and the test host reports 1468 passed, 0 failed.
