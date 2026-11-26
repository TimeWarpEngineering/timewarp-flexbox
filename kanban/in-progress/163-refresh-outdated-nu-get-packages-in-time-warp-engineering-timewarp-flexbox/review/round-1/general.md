# Round 1 — general
**Date:** 2026-11-26
**Scope reviewed:** branch `task/163-refresh-outdated-nu-get-packages-in-time-warp` vs merge-base `1fd16a1`

## Summary

The commit bumps `TimeWarp.Nuru` and `TimeWarp.Nuru.DevCli` to 4.0.0-beta.17 in `Directory.Packages.props`. `<Version>` in `source/Directory.Build.props` stays `2.0.0-beta.3`. Both packages are referenced only from `tools/dev-cli/Directory.Build.props`.

The same commit adds `.UseTelemetry(options => options.ServiceName = "timewarp-flexbox-dev")` on the `NuruApp.CreateBuilder()` chain in `tools/dev-cli/dev.cs`, which is the call `dev-cli-telemetry` asks for (`{repoName}-dev`). It also records three tracked Markdown files in `.timewarp/line-cap-baseline.json`. Origin's baseline lists only C#, so these rows are the first Markdown adoption. The check allows that when each file on the origin default branch is already at least as long as the recorded count, and the ratchet still refuses growth.

`## Results` records `dotnet run --file tools/dev-cli/dev.cs -- self-install`, `./bin/dev build` (0 warnings, 0 errors), and `ganda repo audit` exit 0 after those edits, with the advisory `vscode-window-icon` warning left in place. No product library code or tests changed. That record covers the diff.

Targeted check, not a gate: suspected wrong line-cap counts (a high count would allow growth; a low count fails the ratchet). Command: `git ls-files` and `wc -l` on the three new baseline paths. All three are tracked. Counts are 1358, 642, and 584, matching the baseline. No finding.

The restatement's "no other file is edited for the bump" plus an audit acceptance line is wider than the task's "Edit Directory.Packages.props in this repo only." That mismatch is recorded under `## Learnings`.

## Issues

<!-- None. -->
