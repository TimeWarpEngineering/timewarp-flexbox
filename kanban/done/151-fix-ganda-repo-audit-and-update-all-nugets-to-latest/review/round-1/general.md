# Round 1 — general
**Date:** 2026-11-08
**Scope reviewed:** branch vs merge-base 07e5aa1 (implementation commit 2b860b7), all non-rename hunks plus call sites of the renamed paths.

## Summary

The change runs `ganda repo audit --fix` (kebab renames, hooks, gitignore, glossary, SourceGenerators), moves the dev CLI to the current Nuru/Mediator API, bumps every central pin, and ratchets the 500-line cap with a shrink-only baseline. Locally verified: `ganda repo audit` exit 0, `dotnet test timewarp-flexbox.slnx` 1468 passed / 0 failed, `./bin/dev --capabilities` returns JSON, `ganda nuget outdated` reports 19/19 up to date, `git ls-files '*.journal.json'` empty, `git check-ignore -v` hits `.gitignore:423 *.journal.json`. Main risk: the dev CLI now targets net11.0 but CI installs only the .NET 10 SDK, so CI breaks. The kebab renames also left a few call sites pointing at the old casing.

Restatement vs task: consistent; no misread.

## Issues

### Issue 1 — Severity: bug
- File: .github/workflows/workflow.yml:46
- Description: CI installs only `dotnet-version: '10.0.x'`, then runs `dotnet run tools/dev-cli/dev.cs -- workflow`. This change sets `tools/dev-cli/Directory.Build.props` to `<TargetFramework>net11.0</TargetFramework>` (Amuru 3 is net11.0 only). With only SDK 10, the dev CLI fails to compile. Reproduced in a temp copy pinned by global.json to SDK 10.0.403: `error NETSDK1045: The current .NET SDK does not support targeting .NET 11.0`. Every PR or push that touches `tools/**`, `Directory.Packages.props`, `source/**`, etc. (this PR included) fails the "Run CI Pipeline" step, and release publishing is broken.
- Suggestion: install both SDKs in `actions/setup-dotnet` (`dotnet-version: |` `10.0.x` / `11.0.x`, plus `dotnet-quality: preview` while .NET 11 is pre-GA). Product projects stay net10.0.
- Status: open

### Issue 2 — Severity: bug
- File: runfiles/port-generated-tests.cs:34
- Description: The generator still writes to `Path.Combine(repoRoot, "test", "timewarp-flexbox-tests", "Generated")`. The audit fix renamed that directory to `generated`. On a case-sensitive file system (Linux, CI), running the porter again creates a second `Generated/` tree whose classes duplicate the ones in `generated/` (same namespace and type names), so the test project fails with duplicate-type errors. Regenerated output also never replaces the tracked files.
- Suggestion: change the segment to `"generated"`. Update the comments on lines 8 and 197 to the new path.
- Status: open

### Issue 3 — Severity: suggestion
- File: readme.md:86
- Description: Other references to the renamed paths are now stale. `readme.md:86` links `[LICENSE](LICENSE)`, but the file is now `license`, and GitHub resolves links case-sensitively, so the link 404s. `skills/flexbox/SKILL.md:293` and `:296` point at `test/timewarp-flexbox-tests/Generated/` and `source/timewarp-flexbox/Style/Style.cs`. `.agent/workspace/2025-12-17-t12-00-00-integration-testing-investigation.md:153` names the old `2025-12-09T12-30-00_…` filename.
- Suggestion: update these references to the kebab-case paths.
- Status: open

### Issue 4 — Severity: suggestion
- File: .timewarp/line-cap-baseline.json:3
- Description: The task and the restatement say to split over-cap files, or to record a `[ganda.audit]` exception "with a reason and this task id" for ported files that should stay whole. The implementation instead adds a shrink-only baseline with 36 entries and no reason or task id in the repo. The rationale exists only in task.md `## Session`. The baseline also grandfathers `samples/layout-demo/layout-demo.cs` (526 lines), a repo-authored sample runfile rather than a Yoga port, so the "splitting invents boundaries the port must not have" rationale does not cover it. The ratchet is a sound mechanism and is stricter than `severity = off`. The gap is traceability, plus one entry outside the stated rationale.
- Suggestion: put the reason and task 151 where a future reader of the repo will find them (for example a `[ganda.audit]` `.editorconfig` comment or note that points at the baseline, or a sibling readme if ganda supports one). Trim `layout-demo.cs` under 500 lines and drop its baseline entry, or record why the sample stays whole.
- Status: open

### Issue 5 — Severity: nit
- File: tools/dev-cli/global-usings.cs:19
- Description: The comment says "See tools/dev-cli/dev.cs Design" for the Mediator move, but the Design region in dev.cs covers only the DI registrations. It says nothing about `TimeWarp.Mediator`, `ICommand`, or `Unit`.
- Suggestion: drop the cross-reference, or add one line about the Mediator namespace to the dev.cs Design region.
- Status: open
