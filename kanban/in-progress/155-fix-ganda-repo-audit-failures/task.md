# Fix ganda repo audit failures

## Goal (implementer restatement)

After task 154, `ganda repo audit` on this repo fails five checks. Three are errors (`analyzer-suppressions` has no baseline file, `.gitignore` omits `/.timewarp/signals/`, and the GitHub workflow still runs bash instead of a top-level pwsh default). Two are warnings (`memsearch` githooks are stale, and CI collects coverage without uploading it).

This task is done when those five checks pass and `ganda repo audit` exits 0. Auto-fix output from `ganda repo audit --fix` is committed as the intended baseline. The two workflow steps that are still bash ("Run CI Pipeline" and "AOT smoke test") are rewritten in pwsh with the same release versus non-release branch and the same AOT smoke loop. Refreshed `.githooks/*.cs` still build on net11.0 with Amuru 3. `dotnet build`, `dotnet test`, and `./bin/dev --capabilities` succeed on this branch. Pull-request CI is the host's later gate; this walk commits and pushes the product fix so that gate can run.

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
- [x] `ganda repo audit` exit 0
- [x] `dotnet build timewarp-flexbox.slnx` and `dotnet test timewarp-flexbox.slnx` green
- [x] `./bin/dev --capabilities` works
- [ ] PR CI green

## Session

- Created: d7943e80-818a-43bd-a36a-6968aed6437a (2026-11-10)
- Implementation: 01a1c63f-bbf6-7132-aca6-eafeec4cb8c2 (2026-11-10)
- Implement oracle: implemented by implementer-grok (grok, vendor xai, model grok-4.7, response model grok-4.7-build), session 01a1c63f-bbf6-7132-aca6-eafeec4cb8c2, max-turns 500, Done — 2026-11-10T16:28:35Z

## Results

`ganda repo audit` exits 0 (44 passed, 0 failed, 2 skipped).

`ganda repo audit --fix` seeded `.timewarp/suppression-baseline.json` (105 suppressions, none above the baseline), ignored `/.timewarp/signals/`, refreshed the five `.githooks/*.cs` attest hooks, and inserted top-level `defaults: run: shell: pwsh`. It also filled the local `.vscode/settings.json` window-icon keys. That file stays gitignored. The same run reported an extra `vscode-window-icon` warning that was not in the original five findings; the fix is local and the re-audit passes.

`--fix` did not rewrite the bash step bodies, and it did not insert the coverage upload. The workflow is not an MTP `dotnet test` invocation the fixer can patch: collection is inferred from the `coverlet.collector` package reference. The two bash steps are now pwsh. Release still runs `dotnet run tools/dev-cli/dev.cs -- workflow --api-key` from `NUGET_API_KEY`; every other event runs the same command without the key. The AOT smoke step still publishes and runs `net10.0` and `net11.0` for `linux-x64`. Each native command exits the step when `$LASTEXITCODE` is non-zero, which is what bash `-e` did. A `coverage` upload step (`actions/upload-artifact@v4`, path `artifacts/coverage`, `if-no-files-found: warn`) follows the pipeline step.

`.githooks/pre-commit.cs` builds as `net11.0` and references TimeWarp.Amuru 3.0.0-beta.2. The other four hooks build with the same command.

`dotnet build timewarp-flexbox.slnx`: 0 warnings, 0 errors. `dotnet test timewarp-flexbox.slnx`: 1468 passed on net10.0 and 1468 passed on net11.0. `./bin/dev --capabilities` prints the dev CLI, including `build`, `test`, `verify-samples`, `workflow`, `check-version`, `clean`, `release`, and `self-install`.

PR CI is the host open-pr gate. This walk does not open the pull request. `dev test` does not pass a coverlet collector argument, so the coverage artifact can be empty and the upload step warns instead of failing.

### How to validate

Smoke: from this worktree run `ganda repo audit`, `dotnet build timewarp-flexbox.slnx`, `dotnet test timewarp-flexbox.slnx`, `./bin/dev --capabilities`, and `dotnet build .githooks/pre-commit.cs`.

Expect: `ganda repo audit` exits 0 with `workflow-pwsh-shell`, `ci-code-coverage`, `analyzer-suppressions`, `timewarp-signals-gitignore`, and `memsearch-scaffold` passing. The solution build finishes with 0 warnings and 0 errors. Both net10.0 and net11.0 test runs pass 1468 tests. Capabilities prints JSON for the dev CLI. The hook build targets net11.0 and succeeds.

## Notes

Previous audit cleanup: task 151. NuGet bump just before this: task 154 (PR #15).
