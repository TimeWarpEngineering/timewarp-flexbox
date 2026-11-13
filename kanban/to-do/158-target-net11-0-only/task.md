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

## Checklist

- [ ] Root default and project TFMs to net11.0
- [ ] Remove redundant net11.0 overrides; fix comments
- [ ] Sample and benchmarks on net11.0
- [ ] CI: single 11.0.x SDK; AOT smoke net11.0 only
- [ ] Dev CLI TFM assumptions checked
- [ ] Docs and release notes updated; Version 2.0.0-beta.2
- [ ] `dotnet build timewarp-flexbox.slnx` 0 warnings / 0 errors
- [ ] `dotnet test timewarp-flexbox.slnx` green on net11.0
- [ ] `./bin/dev verify-samples` and `./bin/dev --capabilities` work
- [ ] `ganda repo audit` exit 0
- [ ] PR CI green

## Session

- Created: d7943e80-818a-43bd-a36a-6968aed6437a (2026-11-14)

## Notes

Multi-targeting came in with task 152 (PR #13, 2.0.0-beta.1). Last release: v2.0.0-beta.1 (2026-11-08); stable is v1.0.0.
