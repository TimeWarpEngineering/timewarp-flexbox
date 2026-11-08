# Target .NET 10 and 11 and release 2.0.0-beta.1

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

- [ ] Multi-target library for net10.0 and net11.0
- [ ] Tests on both TFMs
- [ ] AOT smoke covers the intended TFM(s)
- [ ] Version 2.0.0-beta.1; readme/skill snippets updated
- [ ] `ganda repo audit --fix`; commit results; audit exits 0
- [ ] Build + tests green locally and in CI
- [ ] After merge: cut the prerelease per tw-release (`dev release`, not hand tags)

## Notes

- Version is 2.0.0-beta.1 by request (2026-11-09). Adding a TFM alone is not a
  breaking change, so record any real breaking changes in release notes.
- Cutting the release is a human-approved step after the PR merges.

## Session

- Created: b3bc5dc2-e0c9-4b04-ac76-4a1bd4f7a1f3 (2026-11-09)
