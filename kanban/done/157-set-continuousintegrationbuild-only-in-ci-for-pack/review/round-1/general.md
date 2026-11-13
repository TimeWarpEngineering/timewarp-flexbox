# Round 1 — general
**Date:** 2026-11-14
**Scope reviewed:** branch `task/157-set-continuousintegrationbuild-only-in-ci-for-pack` vs `master`. Product files: `Directory.Build.props`, `tools/dev-cli/endpoints/workflow-command.cs`, `Directory.Packages.props`, `.githooks/{post-checkout,post-commit,post-merge,pre-commit,pre-push}.cs`. Call sites: `source/Directory.Build.props` (parent import), `source/timewarp-flexbox/timewarp-flexbox.csproj`, `.github/workflows/workflow.yml`, `tools/dev-cli/Directory.Build.props`.

## Summary

`PackAsync` no longer passes `-p:ContinuousIntegrationBuild=true`. The root `Directory.Build.props` sets `ContinuousIntegrationBuild` to true only when `'$(GITHUB_ACTIONS)' == 'true'`. `source/Directory.Build.props` imports that file with `GetPathOfFileAbove`, so the packed library evaluates the same property. GitHub Actions sets `GITHUB_ACTIONS=true` for the `workflow.yml` step that runs `dev workflow`, and release runs are the ones that pack. That matches Mediator `Directory.Build.props` (condition on `GITHUB_ACTIONS` only) and the task's requested gate.

The restatement matches the task. "Or an equivalent CI env" is the Mediator `GITHUB_ACTIONS` signal this repo's only workflow already provides, not a second `CI` condition. No misread.

The audit side edits are version pins (`TimeWarp.Terminal` 1.0.2 → 2.0.0-beta.1, `TimeWarp.SourceGenerators` 1.0.0-beta.14 → 1.0.0-beta.15) and `#region Purpose` comments on the five hooks. Those comments match the hook bodies (attest hooks return 0; pre-commit and pre-push refuse the cases they describe). `TimeWarp.Terminal` is referenced only from the dev CLI. `## Results` records `./bin/dev build`, `./bin/dev test` (1468/1468 on net10.0 and net11.0), and `ganda repo audit` (44 passed, 0 failed) after those edits, plus the `dotnet msbuild -getProperty:ContinuousIntegrationBuild` check (empty without `GITHUB_ACTIONS`, `true` with it). That record covers the diff. Gates were not re-run.

## Issues

None.
