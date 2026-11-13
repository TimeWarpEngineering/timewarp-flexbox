# Target net11.0 only

## Description

Drop net10.0 everywhere and target only net11.0. Today the repo default is net10.0
(`Directory.Build.props:20`), and the library, unit tests and AOT smoke multi-target
`net10.0;net11.0`. The dev CLI, runfiles and `.githooks` already override to net11.0.

The readme, `skills/flexbox/SKILL.md` and `PackageReleaseNotes` already say net10.0 support ends
at 2.0.0 GA, after .NET 11 GA. The .NET 11 SDK 11.0.100 (GA) is installed. Users who need
net10.0 stay on 1.x. The package is 2.0.0-beta.1 (a prerelease), so dropping a TFM is allowed.

## Requirements

- Root `Directory.Build.props`: default `TargetFramework` becomes `net11.0`. Update the comment above it.
- Library, unit tests and AOT smoke: single `TargetFramework` `net11.0` (no `TargetFrameworks` list;
  drop the empty `<TargetFramework></TargetFramework>` reset lines).
- Benchmarks and `samples/layout-demo/layout-demo.cs` (`#:property TargetFramework=net10.0`) move to net11.0.
- The net11.0 overrides in `tools/dev-cli/`, `runfiles/` and `.githooks/` `Directory.Build.props` become
  redundant. Remove them if they now only restate the root default, and fix their comments
  ("override the root net10.0 pin").
- CI `.github/workflows/workflow.yml`: drop the 10.0.x SDK setup; install 11.0.x GA (drop
  `dotnet-quality: preview` if GA is available on setup-dotnet). The AOT smoke loop
  (`foreach ($tfm in @('net10.0', 'net11.0'))`) runs net11.0 only. Update the comment at line ~53.
- Docs: readme, `skills/flexbox/SKILL.md` and `source/Directory.Build.props` `PackageReleaseNotes`
  say the next 2.0.0 prerelease targets net11.0 only; 1.x stays the net10.0 line.
- Bump `source/Directory.Build.props` `Version` to `2.0.0-beta.2`. Do **not** cut a release
  (releases wait for an explicit ask).
- Check `global.json` / `rollForward` and any `dev` CLI code that hard-codes TFMs
  (for example `tools/dev-cli/endpoints/verify-samples-discovery.cs`, `test-command.cs`, `workflow-command.cs`).
- After the change, `grep -rn net10 --exclude-dir={.git,bin,obj,artifacts,kanban} .` finds nothing
  except deliberate 1.x guidance in docs.

## Goal (implementer restatement)

Make net11.0 the only target framework in the repo. The root default moves from net10.0 to
net11.0, the library, unit tests and AOT smoke drop their multi-target lists, and the sample and
benchmarks follow the root. The per-folder net11.0 overrides (dev CLI, runfiles, githooks) go
away because they now restate the default. CI installs one GA 11.0.x SDK and runs the AOT smoke
on net11.0 only. Readme, the flexbox skill and the package release notes say 2.0.0-beta.2 and
later target net11.0 only and 1.x is the net10.0 line. Version becomes 2.0.0-beta.2; no release.

Acceptance: `grep -rn net10` (excluding .git/bin/obj/artifacts/kanban) finds only 1.x guidance in
docs; `./bin/dev build` and `./bin/dev test` pass with 0 warnings on net11.0; `ganda repo audit`
exits 0; dev CLI TFM discovery still works for the sample.

## Checklist

- [x] Root default and project TFMs to net11.0
- [x] Remove redundant net11.0 overrides; fix comments
- [x] Sample and benchmarks on net11.0
- [x] CI: single 11.0.x SDK; AOT smoke net11.0 only
- [x] Dev CLI TFM assumptions checked
- [x] Docs and release notes updated; Version 2.0.0-beta.2
- [x] `dotnet build timewarp-flexbox.slnx` 0 warnings / 0 errors
- [x] `dotnet test timewarp-flexbox.slnx` green on net11.0
- [x] `./bin/dev verify-samples` and `./bin/dev --capabilities` work
- [x] `ganda repo audit` exit 0
- [ ] PR CI green

## Session

- Created: d7943e80-818a-43bd-a36a-6968aed6437a (2026-11-14)

- 2026-11-14 implement oracle: switched the root default to net11.0, dropped multi-targeting,
  removed redundant net11.0 overrides, single-SDK CI, docs and Version 2.0.0-beta.2.

## Results

### Changes

- `Directory.Build.props`: default `TargetFramework` net10.0 → net11.0; comment updated.
- Library, unit tests, AOT smoke: removed `<TargetFramework></TargetFramework>` reset and the
  `TargetFrameworks` list; they inherit net11.0 from the root.
- `samples/layout-demo/layout-demo.cs`: `#:property TargetFramework=net11.0`. Benchmarks had no TFM
  of their own and follow the root (now net11.0).
- Removed `runfiles/Directory.Build.props` and `.githooks/Directory.Build.props` (they only
  imported the root and set net11.0). Removed the net11.0 override and its comment from
  `tools/dev-cli/Directory.Build.props`.
- CI: dropped the 10.0.x setup and `dotnet-quality: preview`; one `11.0.x` setup. The AOT smoke
  publishes and runs once (no `--framework` loop) into `artifacts/aot-smoke`.
- Dev CLI: no hard-coded TFMs. `verify-samples-discovery.cs` reads TFMs from the sample's
  `#:property` / csproj, so it picks up net11.0 (verified). `test-command.cs` and
  `workflow-command.cs` are TimeWarp.Nuru.DevCli package sources with no in-repo TFM.
- No `global.json` exists, so there is no `rollForward` to change.
- `source/Directory.Build.props`: Version 2.0.0-beta.2; release notes say net11.0 only, 1.x for
  net10.0. Readme and `skills/flexbox/SKILL.md` updated the same way (example version beta.2).
  No release cut.
- `grep -rn net10 --exclude-dir={.git,bin,obj,artifacts,kanban} .` now finds only the 1.x
  guidance (readme, SKILL.md, release notes) and the root props comment.

### Gates

- `dotnet run --file tools/dev-cli/dev.cs -- self-install` (dev-cli props changed): installed.
- `./bin/dev build`: Build succeeded, 0 warnings, 0 errors (all projects net11.0; package
  `TimeWarp.Flexbox.2.0.0-beta.2.nupkg`).
- `./bin/dev test`: Passed 1468, Failed 0, Skipped 0 (net11.0).
- `ganda repo audit`: exit 0, one advisory warning (`vscode-window-icon`, `.vscode/settings.json`
  missing). `ganda repo audit --fix` created the ignored local file; audit then passed all checks.
- Extra checks: `./bin/dev verify-samples` ok (layout-demo on net11.0); `./bin/dev --capabilities`
  ok; AOT smoke `dotnet publish … -r linux-x64` + run printed `AOT smoke: PASS`; runfiles
  `.githooks/pre-commit.cs` and `runfiles/build.cs` build without their removed props files.

### How to validate

Smoke:

```bash
./bin/dev build && ./bin/dev test && ./bin/dev verify-samples
grep -rn net10 --exclude-dir={.git,bin,obj,artifacts,kanban} .
dotnet publish tests/timewarp-flexbox-aot-smoke/timewarp-flexbox-aot-smoke.csproj -c Release -r linux-x64 -o artifacts/aot-smoke && ./artifacts/aot-smoke/TimeWarp.Flexbox.AotSmoke
```

Expect: build 0 warnings/0 errors with only `net11.0` output folders; 1468 tests pass on net11.0;
the sample verifies on net11.0; grep shows only 1.x net10.0 guidance in readme, SKILL.md,
release notes and the root props comment; the smoke prints `AOT smoke: PASS`.

## Notes

Multi-targeting came in with task 152 (PR #13, 2.0.0-beta.1). Last release: v2.0.0-beta.1 (2026-11-08); stable is v1.0.0.
