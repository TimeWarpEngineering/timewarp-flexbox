# Target .NET 10 and 11 and release 2.0.0-beta.1

## Goal (implementer restatement)

Ship TimeWarp.Flexbox 2.0.0-beta.1 as a prerelease that still contains both
`net10.0` and `net11.0`. The library project multi-targets those two frameworks,
and the packed nupkg carries `lib/net10.0` and `lib/net11.0`. Unit tests run on
both. The Native AOT smoke project publishes a native binary for both, with XML
doc generation left off so the SDK 11 publish does not look for a documentation
file the AOT layout never copies. The layout demo and the benchmarks stay on
`net10.0` through the root `Directory.Build.props` pin. Install snippets in the
readme and the flexbox skill use `--prerelease` and pin `2.0.0-beta.1`; `1.0.0`
remains the stable install with no prerelease flag. Release notes say that
`net10.0` support ends at 2.0.0 GA, which follows .NET 11 GA, and that `net10.0`
callers should stay on 1.x. This task does not drop `net10.0` and does not cut
the NuGet release. `ganda repo audit` exits 0, and the build and tests pass on
both frameworks.

## Description

The library, tests and samples target only `net10.0` (root `Directory.Build.props`).
Repo tooling (dev CLI, runfiles, git hooks) already targets `net11.0`, and CI
installs both SDKs (10.0.x and 11.0.x preview, added in task 151). Multi-target
the shipped package for `net10.0;net11.0`, set the version to `2.0.0-beta.1`, and
leave `ganda repo audit` clean.

## Requirements

- Library project(s) use `<TargetFrameworks>net10.0;net11.0</TargetFrameworks>`.
  The NuGet package carries `lib/net10.0` and `lib/net11.0`.
- Tests run on both TFMs (or a stated reason why one TFM is enough for tests).
- AOT smoke test (`tests/timewarp-flexbox-aot-smoke`) publishes on both TFMs, or
  states which TFM it covers. Watch the XML-docs skip from task 151.
- Samples may stay on one TFM. Say which.
- `<Version>` in `source/Directory.Build.props` is `2.0.0-beta.1`.
- Readme and skill install snippets say `--prerelease` / `2.0.0-beta.1` where they
  pin a version. 1.0.0 stays the stable line.
- `ganda repo audit` exits 0. Fixes are policy. At creation (2026-11-09) on
  origin-home the audit had 3 failures: `bin-dev` and `dev-cli-capabilities`
  (both because `bin/dev` was not built in that checkout) and `vscode-window-icon`
  (warning, fixable). Fix whatever the audit reports on the task branch.
- CI green on both TFMs.

## Checklist

- [x] Multi-target library for net10.0 and net11.0
- [x] Tests on both TFMs
- [x] AOT smoke covers the intended TFM(s)
- [x] Version 2.0.0-beta.1; readme/skill snippets updated
- [x] `ganda repo audit --fix`; commit results; audit exits 0
- [x] Build + tests green locally and in CI
- [ ] After merge: cut the prerelease per tw-release (`dev release`, not hand tags)

## Review

- Round 1: Done

## Notes

- Why 2.0.0: the 2.0.0 line will drop `net10.0` once .NET 11 is GA. That drop is
  the breaking change. 2.0.0-beta.1 is the first beta toward it and still ships
  both TFMs. Do not drop net10.0 in this task. (Steve, 2026-11-09)
- Release notes for beta.1 should say: net10.0 support ends at 2.0.0 GA, which
  follows .NET 11 GA; stay on 1.x for net10.0.
- Cutting the release is a human-approved step after the PR merges.

## Session

- Created: b3bc5dc2-e0c9-4b04-ac76-4a1bd4f7a1f3 (2026-11-09)
- Implement: grok (2026-11-09). Library, unit tests, and AOT smoke multi-target `net10.0;net11.0`. Version `2.0.0-beta.1`. Layout demo and benchmarks stay on `net10.0`. Audit exit 0. Local `./bin/dev workflow` green. GitHub run [42508073157](https://github.com/TimeWarpEngineering/timewarp-flexbox/actions/runs/42508073157) green on both TFMs. The prerelease cut stays after merge.
- Implement oracle: implemented by implementer-grok (grok, vendor xai, model grok-4.7, response model grok-4.7-build), session 01a1bc4c-0118-7d10-9a0b-43a78a1a454d, max-turns 500, Done — 2026-11-08T18:06:30Z
- Review oracle: reviewer-claude (claude-opus-5-5, headless), tw-implementation-review at effort 2 with roster `general` and turn cap 120. Ran 1 round, disposition clean (2026-11-09).
- Review oracle: review by reviewer-claude (claude, vendor anthropic, model claude-opus-5-5, response model claude-opus-5-5), session ed2d3b87-711d-48da-a67e-2a058feb8747, max-turns 120, artifacts general.md, merged.md, disposition.md — 2026-11-08T18:08:37Z

## Results

### Review disposition

Disposition: **clean**. One round ran at effort 2 with one reviewer (`general`).
Final counts are 0 for every severity (bug, suggestion, nit) and every status
(open, fixed, wontfix). The reviewer found no defects and no misread of the task
in the goal restatement. The reviewer checked each requirement against the diff
and the packed nupkg (`lib/net10.0` and `lib/net11.0`). The reviewer re-ran
`ganda repo audit`, and it exited 0. The CI result comes from the implementer's
run 42508073157. The reviewer did not re-fetch that run.

Artifacts:

- `review/review-framework.md`
- `review/round-1/general.md`
- `review/round-1/merged.md`
- `review/disposition.md`

The library, the unit-test project, and `tests/timewarp-flexbox-aot-smoke` set `<TargetFrameworks>net10.0;net11.0</TargetFrameworks>` and clear `<TargetFramework>` so the root pin does not trip NETSDK1013. The root `Directory.Build.props` pin stays `net10.0`. Benchmarks keep that pin. The layout demo sets `#:property TargetFramework=net10.0`, so the sample stays on `net10.0`.

`<Version>` in `source/Directory.Build.props` is `2.0.0-beta.1`. `PackageReleaseNotes` says: net10.0 support ends at 2.0.0 GA, which follows .NET 11 GA; stay on 1.x for net10.0. The readme and `skills/flexbox/SKILL.md` install with `--prerelease` and pin `2.0.0-beta.1`. `1.0.0` stays the stable line (`dotnet add package TimeWarp.Flexbox` with no `--prerelease`). This task does not drop `net10.0` and does not cut the release.

CI publishes the AOT smoke once per TFM into `artifacts/aot-smoke/<tfm>` and runs that native binary. `GenerateDocumentationFile` stays false on the smoke project so SDK 11 publish does not look for the XML file task 151 removed from the AOT layout.

`ganda repo audit --fix` wrote the VS Code window title into gitignored `.vscode/settings.json` (`vscode-window-icon`). No tracked audit diff. A following audit passed 38, failed 0, skipped 2.

`./bin/dev check-version` printed source `2.0.0-beta.1`, latest NuGet `1.0.0`, and `Version in source is new — safe to release.`

`./bin/dev workflow` (clean, Release build, verify-samples, test) exited 0: 0 warnings, 0 errors, 1468 passed on net10.0 and 1468 passed on net11.0. `verify-samples` is still a stub that prints success. The layout demo was run separately: `dotnet run --file samples/layout-demo/layout-demo.cs` exited 0. The only demo warnings are the pre-existing RS0030 `Console` bans.

Native AOT publish for `net10.0` and `net11.0` (`-c Release -r linux-x64`) each printed `AOT smoke: PASS (all layout checks)`.

GitHub Actions run [42508073157](https://github.com/TimeWarpEngineering/timewarp-flexbox/actions/runs/42508073157) (`workflow_dispatch` on this branch) succeeded. The pipeline built both TFMs with 0 errors, tests passed 1468 on net10.0 and 1468 on net11.0, and the AOT step printed `AOT smoke: PASS (all layout checks)` for both `artifacts/aot-smoke/net10.0` and `artifacts/aot-smoke/net11.0`.

The packed `artifacts/packages/TimeWarp.Flexbox.2.0.0-beta.1.nupkg` contains `lib/net10.0/TimeWarp.Flexbox.dll`, `lib/net10.0/TimeWarp.Flexbox.xml`, `lib/net11.0/TimeWarp.Flexbox.dll`, and `lib/net11.0/TimeWarp.Flexbox.xml`. The nuspec release notes are the sunset sentence. Both TFM dependency groups are empty.

Cutting `2.0.0-beta.1` stays a human `dev release` from a clean master after this PR merges. The GitHub release notes should repeat the `PackageReleaseNotes` sentence.

### How to validate

Smoke: from this worktree run `./bin/dev workflow`, then

```bash
for tfm in net10.0 net11.0; do
  dotnet publish tests/timewarp-flexbox-aot-smoke/timewarp-flexbox-aot-smoke.csproj \
    --configuration Release --framework "$tfm" -r linux-x64 \
    -o "artifacts/aot-smoke/$tfm"
  "./artifacts/aot-smoke/$tfm/TimeWarp.Flexbox.AotSmoke"
done
unzip -l artifacts/packages/TimeWarp.Flexbox.2.0.0-beta.1.nupkg
ganda repo audit
```

Expect: workflow prints `0 Warning(s)`, `0 Error(s)`, `Passed: 1468` for `net10.0`, `Passed: 1468` for `net11.0`, and `Workflow completed successfully!`. Each AOT binary prints `AOT smoke: PASS (all layout checks)`. The nupkg lists `lib/net10.0/TimeWarp.Flexbox.dll` and `lib/net11.0/TimeWarp.Flexbox.dll`. `ganda repo audit` exits 0.
