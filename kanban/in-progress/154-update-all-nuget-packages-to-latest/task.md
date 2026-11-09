# Update all NuGet packages to latest

## Goal (implementer restatement)

Bring every centrally pinned NuGet package, and any runfile or `.githooks` directive that names a version, up to the newest version NuGet currently publishes. Packages already on a prerelease stay on the newest prerelease of that line. Major bumps are allowed; any compile or test break they cause gets fixed in this task. The target framework split stays as it is: the library and its tests stay on net10.0, and the dev CLI, runfiles, and `.githooks` stay on net11.0. Do not add or drop packages except where an upgrade forces it. Done means `ganda nuget outdated` is clean, the solution builds with no new warnings, the test suite is green, `./bin/dev --capabilities` still runs, and `ganda repo audit` exits 0.

## Description

Bump every central package pin in `Directory.Packages.props` (and any `#:package` versions in
runfiles / `.githooks` that pin explicitly) to the latest available version, then make the repo
build and test green on the new versions.

Known drift at creation: `TimeWarp.Nuru` / `TimeWarp.Nuru.DevCli` are pinned at `4.0.0-beta.11`
while `4.0.0-beta.12` exists. Check the rest with `ganda nuget outdated`.

## Requirements

- `ganda nuget outdated` reports every package up to date.
- Major-version bumps are in scope (per global prefs, not an exception); fix any API breaks they cause.
- Keep the TFM split: library and tests on net10.0; dev CLI, runfiles and `.githooks` on net11.0 (Amuru 3).
- Prerelease packages stay on the latest prerelease where already on a prerelease line.
- Do not add or remove packages beyond what an upgrade requires.

## Checklist

- [x] Run `ganda nuget outdated` and record the before/after list in Results
- [x] Update pins in `Directory.Packages.props`
- [x] Update any explicitly versioned `#:package` directives
- [x] `dotnet build timewarp-flexbox.slnx` clean (no new warnings)
- [x] `dotnet test timewarp-flexbox.slnx` green
- [x] `./bin/dev --capabilities` works (dev CLI still builds on new Nuru/Amuru)
- [x] `ganda repo audit` exit 0 (commit whatever `--fix` changes)
- [ ] Open PR

## Session

- Created: d7943e80-818a-43bd-a36a-6968aed6437a (2026-11-09)
- Implementation: 01a1bf99-510d-7cc3-9123-a8ce6cb1bd37 (2026-11-09)
- Implement oracle: implemented by implementer-grok (grok, vendor xai, model grok-4.7, response model grok-4.7-build), session 01a1bf99-510d-7cc3-9123-a8ce6cb1bd37, max-turns 500, Done — 2026-11-09T09:28:14Z

## Results

Five central pins moved. The other fourteen were already current. After the bump, `ganda nuget outdated` reports all 19 packages up to date on nuget.org.

Before, the five that moved:

- TimeWarp.Amuru 3.0.0-beta.1 → 3.0.0-beta.2
- TimeWarp.Amuru.Tools 3.0.0-beta.1 → 3.0.0-beta.2
- TimeWarp.Nuru 4.0.0-beta.11 → 4.0.0-beta.12
- TimeWarp.Nuru.DevCli 4.0.0-beta.11 → 4.0.0-beta.12
- TimeWarp.SourceGenerators 1.0.0-beta.11 → 1.0.0-beta.12

Already current: TimeWarp.Terminal 1.0.2, TimeWarp.Build.Tasks 1.0.0, Microsoft.CodeAnalysis.BannedApiAnalyzers 5.6.0, TimeWarp.Fixie 3.1.0, Fixie.TestAdapter 4.2.0, Microsoft.NET.Test.Sdk 18.11.0, Shouldly 4.3.0, coverlet.collector 10.1.0, BenchmarkDotNet 0.15.8, Roslynator.Analyzers 5.0.1, Roslynator.CodeAnalysis.Analyzers 5.0.1, Roslynator.Formatting.Analyzers 5.0.1, Microsoft.CodeAnalysis.NetAnalyzers 10.0.403, Microsoft.CodeAnalysis.CSharp.CodeStyle 5.9.0.

Runfiles and `.githooks` use `#:package TimeWarp.Amuru` and `#:package TimeWarp.Amuru.Tools` with no version, so they follow the central pin. `dotnet build` of `runfiles/build.cs` and `.githooks/pre-commit.cs` succeeded on net11.0. The three example pins in `runfiles/overview.md` moved from `TimeWarp.Amuru@1.0.0-beta.5` to `@3.0.0-beta.2`.

Nuru 4.0.0-beta.12 brings TimeWarp.Mediator 15.0.0-beta.4. That release removed `ICommandHandler`. The four dev CLI handlers now implement `IRequestHandler<TCommand, Unit>` and `Handle` returns `ValueTask<Unit>`. Target frameworks are unchanged: the library still multi-targets net10.0 and net11.0, tests the same, and the dev CLI, runfiles, and hooks stay on net11.0.

`dotnet build timewarp-flexbox.slnx` succeeded with 0 warnings and 0 errors. `dotnet test timewarp-flexbox.slnx` passed 1468 tests on net10.0 and 1468 on net11.0, 0 failed. `./bin/dev --capabilities` (after `self-install` on the new pins) prints the dev command list, 8 endpoints. `ganda repo audit` exits 0 (39 passed, 0 failed). `ganda repo audit --fix` wrote only the gitignored `.vscode/settings.json` window title; no tracked audit diff.

The pull request is left for the host open-pr node.

### How to validate

Smoke: from this worktree run `ganda nuget outdated`, `dotnet build timewarp-flexbox.slnx`, `dotnet test timewarp-flexbox.slnx`, and `./bin/dev --capabilities`.

Expect: outdated reports every package up to date. The build finishes with 0 warnings and 0 errors. Both net10.0 and net11.0 test runs pass. Capabilities prints JSON for the `dev` CLI, including `build`, `test`, `verify-samples`, `workflow`, `clean`, `check-version`, `release`, and `self-install`.

## Notes

Prior bump: task 151 (`kanban/done/151-fix-ganda-repo-audit-and-update-all-nugets-to-latest/`).
TimeWarp packages resolve from nuget.org; `nuget.config` maps only `TimeWarp.Flexbox*` to the GitHub feed.
