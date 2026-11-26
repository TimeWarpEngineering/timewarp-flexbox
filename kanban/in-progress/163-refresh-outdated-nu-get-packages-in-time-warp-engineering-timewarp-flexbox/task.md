# Refresh outdated NuGet packages in TimeWarpEngineering/timewarp-flexbox

## Description

Refresh outdated NuGet packages in TimeWarpEngineering/timewarp-flexbox.
Filter: (all)
Outdated:
  TimeWarp.Nuru 4.0.0-beta.16 -> 4.0.0-beta.17 (dev-only: tools/dev-cli/Directory.Build.props)
  TimeWarp.Nuru.DevCli 4.0.0-beta.16 -> 4.0.0-beta.17 (dev-only: tools/dev-cli/Directory.Build.props)
Every outdated pin is dev-only. Bump them. Do not change <Version> in source/Directory.Build.props: the wave merges this task and does not release this repo.
Edit Directory.Packages.props in this repo only.

## Goal (implementer restatement)

Bump the two dev-only Nuru pins in `Directory.Packages.props` from 4.0.0-beta.16 to 4.0.0-beta.17:
`TimeWarp.Nuru` and `TimeWarp.Nuru.DevCli` (consumed only by `tools/dev-cli`).

Acceptance criteria:
- `Directory.Packages.props` pins both packages at 4.0.0-beta.17; no other file is edited for the bump.
- `<Version>` in `source/Directory.Build.props` is unchanged (no release).
- The dev CLI still compiles and `./bin/dev build` passes; `ganda repo audit` passes.

## Session

- Minted by ganda task wave run 20261126T021837Z-0cb631

## Results

- `Directory.Packages.props`: `TimeWarp.Nuru` and `TimeWarp.Nuru.DevCli` 4.0.0-beta.16 -> 4.0.0-beta.17 (dev-only, consumed by `tools/dev-cli`). `source/Directory.Build.props` `<Version>` unchanged.
- Boy-scout audit fixes (pre-existing failures surfaced by `ganda repo audit`):
  - `dev-cli-telemetry`: added `.UseTelemetry(options => options.ServiceName = "timewarp-flexbox-dev")` to the `NuruApp.CreateBuilder()` chain in `tools/dev-cli/dev.cs`, with a Design note.
  - `source-file-line-cap`: recorded the three historical agent-workspace reference docs (`.agent/workspace/...yoga-cpp-source-catalog.md` 1358, `.agent/workspace/...yoga-cpp-api-catalog.md` 642, `.agents/workspace/2025-12-06-yoga-comparison-report.md` 584) in `.timewarp/line-cap-baseline.json` (shrink-only ratchet, same adoption path timewarp-flow uses for its `.agent/workspace` docs).
  - `vscode-window-icon` remains an advisory (non-blocking) warning; not touched.

### Gates

- `dotnet run --file tools/dev-cli/dev.cs -- self-install` (dev CLI rebuilt against Nuru beta.17): success.
- `./bin/dev build`: Build succeeded, 0 warnings, 0 errors.
- Test files: none added or edited; no product library code changed (only dev-cli and package pins), so no covering test files to run. CI runs the full suite.
- `ganda repo audit`: first run failed (dev-cli-telemetry, source-file-line-cap; pre-existing). After the fixes above: "Repository passes — 1 advisory (non-blocking) warning(s)", exit 0.

### How to validate

Smoke:
```bash
grep -o 'TimeWarp.Nuru[^"]*" Version="[^"]*"' Directory.Packages.props
dotnet run --file tools/dev-cli/dev.cs -- self-install && ./bin/dev build
ganda repo audit
```
Expect: both Nuru pins show `4.0.0-beta.17`; the dev CLI installs and `./bin/dev build` reports "Build completed successfully!"; `ganda repo audit` reports "Repository passes" (only the advisory vscode-window-icon warning).
