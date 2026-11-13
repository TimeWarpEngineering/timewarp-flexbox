# Round 1 — general
**Date:** 2026-11-14
**Scope reviewed:** branch `task/158-target-net11-0-only` vs `master`. Product files: `Directory.Build.props`, `source/Directory.Build.props`, `source/timewarp-flexbox/timewarp-flexbox.csproj`, `test/timewarp-flexbox-tests/timewarp-flexbox-tests.csproj`, `tests/timewarp-flexbox-aot-smoke/timewarp-flexbox-aot-smoke.csproj`, `tools/dev-cli/Directory.Build.props`, deleted `runfiles/Directory.Build.props` and `.githooks/Directory.Build.props`, `.github/workflows/workflow.yml`, `samples/layout-demo/layout-demo.cs`, `readme.md`, `skills/flexbox/SKILL.md`. Call sites: `benchmarks/timewarp-flexbox-benchmarks/time-warp.flexbox.benchmarks.csproj`, `test/Directory.Build.props`, `tools/dev-cli/source/Directory.Build.props`, `tools/dev-cli/endpoints/verify-samples-discovery.cs`, `verify-samples-command.cs`, `test-command.cs`, `workflow-command.cs`, `timewarp-flexbox.slnx`.

## Summary

The root `Directory.Build.props` default is `net11.0`. The library, unit tests, and AOT smoke no longer clear that pin or list `TargetFrameworks`, so they inherit `net11.0`. Benchmarks never set a TFM and follow the same root. The layout demo's `#:property TargetFramework` is `net11.0`. `runfiles/Directory.Build.props` and `.githooks/Directory.Build.props` only imported the root and restated `net11.0`; both are gone, so MSBuild walks to the root. `tools/dev-cli/Directory.Build.props` still imports the root for its warning and AOT settings and no longer overrides the framework. CI installs `11.0.x` without `dotnet-quality: preview` and publishes the AOT smoke once, with no `--framework` loop. Version is `2.0.0-beta.2`. Readme, `skills/flexbox/SKILL.md`, and `PackageReleaseNotes` say that prerelease targets `net11.0` only and that 1.x is the net10.0 line. No release was cut. There is no `global.json`.

The restatement matches the task, including the grep acceptance and the build, test, and audit bar. No misread.

`verify-samples-discovery.cs` reads `#:property TargetFramework` from the sample, so the layout demo is discovered as `net11.0` and `verify-samples-command.cs` passes `-f net11.0`. `test-command.cs` and `workflow-command.cs` do not name a TFM. `## Results` records `./bin/dev build` (0 warnings, 0 errors, package `TimeWarp.Flexbox.2.0.0-beta.2.nupkg`), `./bin/dev test` (1468 passed, 0 failed, net11.0), `ganda repo audit` (exit 0), `./bin/dev verify-samples`, `./bin/dev --capabilities`, the AOT smoke (`AOT smoke: PASS`), and builds of `runfiles/build.cs` and `.githooks/pre-commit.cs` after the props files were removed. That record covers the diff. Gates were not re-run. PR CI stays an open checklist item for the host.

`grep` for `net10` outside kanban, bin, obj, and artifacts hits only the 1.x guidance the task required (readme, the flexbox skill, release notes) and the root props comment that restates the same line next to the new pin. `samples/overview.md` and `runfiles/overview.md` still say ".NET 10" for the file-based-app feature. That string does not match `net10`, those files are outside the task's doc list, and they are not a TFM pin.

## Issues

None.
