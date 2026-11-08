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
- [x] CI installs the .NET 10 SDK and the .NET 11 preview SDK
- [x] Yoga porter writes under `generated`
- [x] Stale kebab paths in the readme, skill, and integration-testing note
- [x] Line-cap reason names task 151; layout demo is under 500 lines
- [x] `dev.cs` Design names the `TimeWarp.Mediator` move
- [x] AOT smoke publish on SDK 11 does not look for a missing XML doc file

## Review

- Round 1: M1–M5 addressed on this branch.
- Round 2: Done
- Round 3: Done
## Notes

- Task 150 (routine-journals-gitignore) was folded into this task and archived on 2026-11-08.
- At claim, the worktree on-create handler left uncommitted edits in
  `tools/dev-cli/dev.cs` and `tools/dev-cli/global-usings.cs` (Nuru/Mediator
  migration). Review them as the starting point for the dev-CLI fix.

## Goal (implementer restatement)

Bring this repo to a clean `ganda repo audit` (exit 0, no Error-severity failures) and put every NuGet pin on its latest version so `bin/dev` builds and the test suite still passes.

Fix pass (review round 1): close the five open findings without a new review. CI must be able to compile the net11.0 dev CLI. The Yoga test porter must write into `generated`. Stale kebab paths in the readme, the flexbox skill, and the integration-testing note must match the files on disk. The line-cap ratchet must name task 151 and why the Yoga ports stay whole, and the layout demo must sit at or under 500 lines. The Mediator comment must point at a Design note that actually mentions that move.

CI follow-up: PR #12's AOT smoke step fails on SDK 11.0.100-rc.3 with MSB3030 because publish looks for `TimeWarp.Flexbox.AotSmoke.xml` under `bin/Release/net10.0/linux-x64` and that file is never copied there. The smoke project must publish and the native binary must pass, the same way the dev CLI already turns documentation generation off for its own AOT publish.

Acceptance:

- `ganda repo audit --fix` lands, and a later `ganda repo audit` exits 0.
- The dev CLI registers the services the current TimeWarp.Nuru.DevCli endpoints require, `bin/dev` builds, and `dev --capabilities` runs.
- `.editorconfig` uses an unquoted `charset = utf-8`.
- Every C# file is at or under the 500-line audit cap, or a ported Yoga file that should stay whole has a `[ganda.audit]` exception that names this task and why it was not split.
- Every `PackageVersion` in `Directory.Packages.props` and every runfile `#:package` is the latest version. TimeWarp packages resolve from the GitHub TimeWarpEngineering NuGet feed.
- Build and tests pass after those bumps.
- Root `.gitignore` ignores `*.journal.json` as one glob (not six exact names), no `*.journal.json` file is tracked, and `git check-ignore -v` on `kanban/to-do/task-work.journal.json` matches that glob.
- CI installs the .NET 10 SDK and the .NET 11 preview SDK before it runs `tools/dev-cli/dev.cs`.
- `runfiles/port-generated-tests.cs` writes under `test/timewarp-flexbox-tests/generated`, and its path comments match.
- `readme.md` links `license`. The flexbox skill and the integration-testing note use the kebab-case paths.
- `[ganda.audit]` records why the line-cap baseline exists and names task 151. `samples/layout-demo/layout-demo.cs` is at or under 500 lines and is not in the baseline.
- `tools/dev-cli/dev.cs` Design says that `ICommand` and `Unit` live in `TimeWarp.Mediator`.
- `dotnet publish` of `tests/timewarp-flexbox-aot-smoke` with `-c Release -r linux-x64` exits 0 on SDK 11, and the published native binary prints `AOT smoke: PASS`. The smoke project does not ask the publish layout to copy an XML doc file that Native AOT never writes into `bin`.

## Session

- Created: b3bc5dc2-e0c9-4b04-ac76-4a1bd4f7a1f3 (2026-11-08)
- Implementation: grok task-work (2026-11-08)
- Line-cap: shrink-only `.timewarp/line-cap-baseline.json` for the 36 Yoga ports and generated fixtures (task 151). Splitting invents boundaries the port must not have. `source-file-line-cap.severity = off` would hide later over-cap files. Origin has no baseline, so this first ratchet is legal.
- Implement oracle: implemented by implementer-grok (grok, vendor xai, model grok-4.7, response model grok-4.7-build), session 01a1bbf3-b482-76a2-b863-84e42461802a, max-turns 500, Done — 2026-11-08T16:47:12Z
- Review oracle round 1: reviewer-claude (claude-opus-5-5, headless), effort 2, roster general, turn cap 120 — 2026-11-08. 5 open findings (2 bug, 2 suggestion, 1 nit); fix pass requested.
- Review oracle: review by reviewer-claude (claude, vendor anthropic, model claude-opus-5-5), session not reported, max-turns 120, artifacts general.md, merged.md — 2026-11-08T16:50:21Z
- Fix pass (implement-fix-2): M1–M5 on this branch. Audit 38 passed, 0 failed, 2 skipped. Tests 1468 passed, 0 failed. Layout demo is 497 lines.
- Review oracle round 2: reviewer-claude (claude-opus-5-5, headless), effort 2, roster general, turn cap 120 — 2026-11-09. Delta `facda76..9ac7d1f`; M1–M5 fixed, 0 new, 0 open. Disposition `clean`.
- CI fix: implementer-grok (2026-11-09). PR #12 run 42503142992 failed `dotnet publish tests/timewarp-flexbox-aot-smoke` with MSB3030 for `bin/Release/net10.0/linux-x64/TimeWarp.Flexbox.AotSmoke.xml` on SDK 11.0.100-rc.3. The doc file is produced under `obj` and the SDK 11 publish layout never copies it into that `bin` path.
- Implement oracle: implemented by implementer-grok (grok, vendor xai, model grok-4.7, response model grok-4.7-build), session 01a1bc21-22c5-70b1-a769-bfa931d64d7b, max-turns 500, Done — 2026-11-08T17:13:19Z
- Review oracle round 3: reviewer-claude (claude-opus-5-5, headless), effort 3, roster general, turn cap 200 — 2026-11-09. Delta `9ac7d1f..8d57233` (1e77e8a). 0 new findings, 0 open. Disposition `clean`.
- Review oracle: review by reviewer-claude (claude, vendor anthropic, model claude-opus-5-5, response model claude-opus-5-5), session 4fc64aa4-f7e0-40f7-a930-5c8e87b86c0c, max-turns 200, artifacts general.md, merged.md, disposition.md — 2026-11-08T17:16:22Z

## Results

`ganda repo audit` is clean, every central pin is the latest version `ganda nuget outdated` reports, `bin/dev` builds, and the Flexbox suite passes.

- Audit `--fix` landed the auto-fixable checks. Kebab renames needed `ganda repo audit --fix --checks kebab-path-names` because that check does not run on a blind `--fix`. `timewarp-flexbox.slnx` points at the renamed benchmarks project. `.editorconfig` charset is unquoted `charset = utf-8`.
- Dev CLI targets net11.0 and registers `IRepoCleanService`, `NuGetVersionService`, `IRepoConfigService`, and `IPackableProjectService`. Handlers return `Task<Unit>` for published Nuru 4.0.0-beta.11 / Mediator Contracts 14. Product projects stay net10.0. `runfiles/build.cs` and `runfiles/test.cs` take unversioned `TimeWarp.Amuru` and `TimeWarp.Amuru.Tools` so central management supplies 3.0.0-beta.1.
- Pins (nuget.org; confirming `ganda nuget outdated` reports all 19 up to date): TimeWarp.Amuru 3.0.0-beta.1, TimeWarp.Amuru.Tools 3.0.0-beta.1, TimeWarp.Terminal 1.0.2, TimeWarp.Build.Tasks 1.0.0, Microsoft.CodeAnalysis.BannedApiAnalyzers 5.6.0, TimeWarp.Nuru 4.0.0-beta.11, TimeWarp.Nuru.DevCli 4.0.0-beta.11, TimeWarp.SourceGenerators 1.0.0-beta.11, TimeWarp.Fixie 3.1.0, Fixie.TestAdapter 4.2.0, Microsoft.NET.Test.Sdk 18.11.0, Shouldly 4.3.0, coverlet.collector 10.1.0, BenchmarkDotNet 0.15.8, Roslynator.Analyzers / CodeAnalysis.Analyzers / Formatting.Analyzers 5.0.1, Microsoft.CodeAnalysis.NetAnalyzers 10.0.403, Microsoft.CodeAnalysis.CSharp.CodeStyle 5.9.0. The GitHub TimeWarpEngineering feed returned NAME_UNKNOWN for TimeWarp.Nuru and TimeWarp.Amuru. `nuget.config` maps only `TimeWarp.Flexbox*` to that feed, so these dependencies resolve from nuget.org.
- Folded task 150: `.gitignore` line 423 is `*.journal.json`. `git ls-files '*.journal.json'` is empty. `git check-ignore -v kanban/to-do/task-work.journal.json` matches that line.
- `dotnet build timewarp-flexbox.slnx`: 0 warnings, 0 errors. `dotnet test timewarp-flexbox.slnx`: 1468 passed, 0 failed, 0 skipped. `runfiles/build.cs` and `runfiles/test.cs` both exit 0 (Fixie: 1468 passed). `./bin/dev --capabilities` returns the timewarp-flexbox dev CLI description.
- Context at the start of the fix pass reported this branch 7 commits behind `master`. Those commits were not merged.

Fix pass for review round 1:

- M1: `.github/workflows/workflow.yml` installs SDK `10.0.x`, then SDK `11.0.x` with `dotnet-quality: preview`. Product projects stay net10.0. The dev CLI and runfiles target net11.0. The machine has SDK 10.0.403 and 11.0.100-rc.3.26522.148.
- M2: `runfiles/port-generated-tests.cs` writes to `test/timewarp-flexbox-tests/generated`. The header comment and the IntrinsicSizeMeasure comment use that directory.
- M3: `readme.md` links `license`. `skills/flexbox/SKILL.md` points at `test/timewarp-flexbox-tests/generated/`, `source/timewarp-flexbox/style/Style.cs`, and `source/timewarp-flexbox/enums/YGEnums.cs`. The integration-testing note points at `.agent/workspace/2025-12-09-t12-30-00-gh-issue-2-idempotency-analysis.md`.
- M4: `[ganda.audit]` in `.editorconfig` names task 151 and why `.timewarp/line-cap-baseline.json` exists. The check stays on. `samples/layout-demo/layout-demo.cs` is 497 lines and is not in the baseline (35 entries remain). The demo's ASCII body matches the previous run. Its HTML matches after whitespace is collapsed. Nested engine boxes stay parent-relative; the first level is still positioned from the origin.
- M5: `tools/dev-cli/dev.cs` Design states that `ICommand` and `Unit` live in `TimeWarp.Mediator`.
- This pass: `ganda repo audit` exits 0 (38 passed, 0 failed, 2 skipped), including `source-file-line-cap`. `dotnet test timewarp-flexbox.slnx` reports 1468 passed, 0 failed, 0 skipped. `dotnet run samples/layout-demo/layout-demo.cs -- --html /tmp/layout-demo-new.html` exits 0. The only demo warnings are the pre-existing RS0030 `Console` bans.

### Review

- Rounds: 3. Rounds 1–2 ran at effort 2 and round 3 at effort 3. Roster general (reviewer-claude, claude-opus-5-5).
- Round 1: 2 bug, 2 suggestion, 1 nit (M1–M5), all open. Fix commit 9ac7d1f addressed them.
- Round 2 (delta `facda76..9ac7d1f`): M1–M5 verified fixed, no new findings. `ganda repo audit` exits 0, `source-file-line-cap` passes, the layout demo exits 0 at 497 lines, and every renamed path resolves.
- Round 3 (delta `9ac7d1f..8d57233`, CI fix 1e77e8a): no new findings. MSB3030 reproduces when docs are forced on. As committed, the AOT publish exits 0 and the binary prints `AOT smoke: PASS`. The Release solution build has 0 warnings, `dotnet test` reports 1468 passed, and `ganda repo audit` exits 0.
- Final counts: bug 0 open / 2 fixed, suggestion 0 open / 2 fixed, nit 0 open / 1 fixed. 0 wontfix.
- Disposition: **clean**. No exceptions and no escalations. The operator follow-ups (not findings) are in `review/round-3/general.md`: CI builds the net10.0 product on the SDK 11 preview because there is no `global.json`, and the smoke project loses IDE0005.
- Paths: `review/review-framework.md`, `review/round-1/{general,merged}.md`, `review/round-2/{general,merged}.md`, `review/round-3/{general,merged}.md` (final ledger), `review/disposition.md`.

CI follow-up (PR #12, run 42503142992): SDK 11 Native AOT publish tried to copy `TimeWarp.Flexbox.AotSmoke.xml` from `bin/Release/net10.0/linux-x64` and the file was not there. `tests/timewarp-flexbox-aot-smoke/timewarp-flexbox-aot-smoke.csproj` now sets `GenerateDocumentationFile` to false, the same reason the dev CLI already does. The library still generates documentation from the root `Directory.Build.props`.

Verified on this machine (SDK 11.0.100-rc.3.26522.148 and SDK 10.0.403): `dotnet publish tests/timewarp-flexbox-aot-smoke/timewarp-flexbox-aot-smoke.csproj --configuration Release -r linux-x64` exits 0, and the published `TimeWarp.Flexbox.AotSmoke` prints `AOT smoke: PASS (all layout checks)`. `dotnet build timewarp-flexbox.slnx --configuration Release` reports 0 warnings and 0 errors. `ganda repo audit` exits 0 (38 passed, 0 failed, 2 skipped). The 1468-test suite was not re-run; this change does not touch the test sources.

### How to validate

Smoke: from this worktree, with the .NET 11 preview SDK selected, run `dotnet publish tests/timewarp-flexbox-aot-smoke/timewarp-flexbox-aot-smoke.csproj --configuration Release -r linux-x64 -o /tmp/flexbox-aot-smoke` and then `/tmp/flexbox-aot-smoke/TimeWarp.Flexbox.AotSmoke`. Then run `ganda repo audit`.

Expect: publish exits 0 with no MSB3030, the binary prints `AOT smoke: PASS (all layout checks)` and exits 0, and the audit reports 0 failed checks.
