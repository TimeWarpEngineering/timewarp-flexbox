# Fix ganda repo audit failures

## Goal (implementer restatement)

`ganda repo audit` must exit 0. The five checks from the original brief (suppression baseline, signals gitignore, pwsh workflow shell, memsearch hooks, and a real coverage upload) stay green. Round 1 showed the upload is hollow: `dev test` never runs coverlet, so CI publishes an empty `artifacts/coverage` tree and the audit still says coverage was collected.

This fix pass is done when `dev test` collects XPlat Code Coverage into `artifacts/coverage` and a local run leaves `coverage.cobertura.xml` there. The workflow step is named "Upload Coverage", keeps the artifact name `coverage` (the audit line-rate reader looks for that name), and fails the job when the directory is empty. `ganda repo audit` still exits 0, and the branch is committed and pushed for the host's later PR gate.

## Description

`ganda repo audit` on master (2026-11-10, after task 154 merged) reports 39 passed / 5 failed / 2 skipped.
Newer ganda checks flag the following; make the audit exit 0.

| Check | Severity | Finding | Fix |
|---|---|---|---|
| `analyzer-suppressions` | Error | `.timewarp/suppression-baseline.json` missing | `ganda repo audit --fix` seeds it |
| `timewarp-signals-gitignore` | Error | `/.timewarp/signals/` not in `.gitignore` | `--fix` |
| `workflow-pwsh-shell` | Error | `.github/workflows/workflow.yml` lacks top-level `defaults: run: shell: pwsh`; steps "Run CI Pipeline" and "AOT smoke test" are bash | Hand-convert to pwsh |
| `memsearch-scaffold` | Warning | `.githooks/{post-commit,post-merge,post-checkout,pre-commit,pre-push}.cs` outdated | `--fix` |
| `ci-code-coverage` | Warning | CI collects coverage but does not upload it | `--fix` (or add an `actions/upload-artifact` step for the coverage output) |

## Requirements

- `ganda repo audit` exits 0 with all five checks passing.
- Commit whatever `ganda repo audit --fix` changes (audit fixes are policy).
- The pwsh port of the workflow steps must keep identical behavior (release vs non-release branching, AOT smoke loop).
- Refreshed `.githooks/*.cs` still build on net11.0 (Amuru 3).
- CI must be green on the PR.

## Checklist

- [x] `ganda repo audit --fix` and commit its changes
- [x] Convert `workflow.yml` bash steps to pwsh; add top-level `defaults: run: shell: pwsh`
- [x] Coverage upload step present (via `--fix` or by hand)
- [x] `dev test` collects XPlat Code Coverage into `artifacts/coverage`
- [x] `ganda repo audit` exit 0
- [x] `dotnet build timewarp-flexbox.slnx` and `dotnet test timewarp-flexbox.slnx` green
- [x] `./bin/dev --capabilities` works
- [ ] PR CI green

## Session

- Created: d7943e80-818a-43bd-a36a-6968aed6437a (2026-11-10)
- Implementation: 01a1c63f-bbf6-7132-aca6-eafeec4cb8c2 (2026-11-10)
- Implement oracle: implemented by implementer-grok (grok, vendor xai, model grok-4.7, response model grok-4.7-build), session 01a1c63f-bbf6-7132-aca6-eafeec4cb8c2, max-turns 500, Done — 2026-11-10T16:28:35Z
- Review oracle round 1: claude (model claude-opus-5-5), body tw-implementation-review, effort 2, roster general, turn cap 120. Result: 1 open bug (M1) and 1 open nit (M2). Fix pass requested, 2026-11-10
- Review oracle: review by reviewer-claude (claude, vendor anthropic, model claude-opus-5-5, response model claude-opus-5-5), session 9e3ebf5b-2103-416c-bbc2-28be2a646418, max-turns 120, artifacts general.md, merged.md — 2026-11-10T16:31:22Z
- Fix pass round 2: implement-fix-2, session 01a1c64c-c703-71d1-9797-0b61adf6d8fe (2026-11-10). `dev test` now collects coverlet output, and the upload step fails when that directory is empty.
- Review oracle round 2: claude (model claude-opus-5-5), body tw-implementation-review, effort 1, roster general, turn cap 80. Result: 0 open findings, disposition clean, 2026-11-10

## Results

`ganda repo audit` exits 0 (44 passed, 0 failed, 2 skipped).

`ganda repo audit --fix` seeded `.timewarp/suppression-baseline.json` (105 suppressions, none above the baseline), ignored `/.timewarp/signals/`, refreshed the five `.githooks/*.cs` attest hooks, and inserted top-level `defaults: run: shell: pwsh`. It also filled the local `.vscode/settings.json` window-icon keys. That file stays gitignored. The same run reported an extra `vscode-window-icon` warning that was not in the original five findings; the fix is local and the re-audit passes.

`--fix` did not rewrite the bash step bodies, and it did not insert the coverage upload. The workflow is not an MTP `dotnet test` invocation the fixer can patch: collection is inferred from the `coverlet.collector` package reference. The two bash steps are now pwsh. Release still runs `dotnet run tools/dev-cli/dev.cs -- workflow --api-key` from `NUGET_API_KEY`; every other event runs the same command without the key. The AOT smoke step still publishes and runs `net10.0` and `net11.0` for `linux-x64`. Each native command exits the step when `$LASTEXITCODE` is non-zero, which is what bash `-e` did.

`dev test` recreates `artifacts/coverage`, then runs `dotnet test` with `--collect "XPlat Code Coverage"` and that results directory. The command fails when the suite passes and no `coverage.cobertura.xml` is present. The workflow step is "Upload Coverage". The artifact name stays `coverage`, which is the name the audit line-rate reader and the fixer constant use. `if-no-files-found` is `error`, because the pipeline always runs tests before that step and an empty directory is a failed collection.

`.githooks/pre-commit.cs` builds as `net11.0` and references TimeWarp.Amuru 3.0.0-beta.2. The other four hooks build with the same command.

`dotnet build timewarp-flexbox.slnx`: 0 warnings, 0 errors. `dotnet test timewarp-flexbox.slnx`: 1468 passed on net10.0 and 1468 passed on net11.0. `./bin/dev --capabilities` prints the dev CLI, including `build`, `test`, `verify-samples`, `workflow`, `check-version`, `clean`, `release`, and `self-install`.

On this fix pass, `dotnet run tools/dev-cli/dev.cs -- test --quiet` exited 0. It wrote two Cobertura files, one per target framework:

- `artifacts/coverage/1bf420f0-650a-4e7d-8441-ffcbccfd9a57/coverage.cobertura.xml` (868722 bytes)
- `artifacts/coverage/2259deb4-b7a0-4e31-baf0-79e87b3af7ae/coverage.cobertura.xml` (868722 bytes)

PR CI is the host open-pr gate. This walk does not open the pull request.

### How to validate

Smoke: from this worktree run `dotnet run tools/dev-cli/dev.cs -- test --quiet`, then `find artifacts/coverage -name coverage.cobertura.xml`, then `ganda repo audit`.

Expect: `dev test` exits 0 and `find` prints a `coverage.cobertura.xml` under `artifacts/coverage` for each target framework. `ganda repo audit` exits 0. `ci-code-coverage` passes because the workflow uploads `artifacts/coverage` and `dev test` collects into that directory. `workflow-pwsh-shell`, `analyzer-suppressions`, `timewarp-signals-gitignore`, and `memsearch-scaffold` still pass.

### Review

Round 1 ran at effort 2 with roster `general`. Final counts before this pass: bug 1 open, nit 1 open. The reviewer re-ran `ganda repo audit` and the five target checks passed. `kanban-landed` fails until the host lands the kitchen.

- M1 (bug, fixed): `tools/dev-cli/endpoints/test-command.cs` collects XPlat Code Coverage into `artifacts/coverage` and fails when `coverage.cobertura.xml` is missing. The upload step uses `if-no-files-found: error`. Local `dev test --quiet` wrote two Cobertura files.
- M2 (nit, fixed): the step display name is "Upload Coverage". The artifact `name` stays `coverage` so the audit line-rate reader still finds it.

Artifacts: `review/review-framework.md`, `review/round-1/general.md`, `review/round-1/merged.md`.

Round 2 ran at effort 1 with roster `general` and reviewed the fix delta (5a15146). It re-ran `dev test --quiet`, which wrote two Cobertura files (net10.0 and net11.0). It also re-ran `ganda repo audit`, which reported 44 passed, 0 failed, 2 skipped. Final counts: bug 1 fixed, nit 1 fixed, 0 open, 0 wontfix, no new findings.

**Review disposition: clean** (2 rounds, final open count 0). Artifacts: `review/round-2/general.md`, `review/round-2/merged.md`, `review/disposition.md`.

## Review

- Round 1: fixed — M1 and M2 addressed on this branch
- Round 2: Done
## Notes

Previous audit cleanup: task 151. NuGet bump just before this: task 154 (PR #15).
