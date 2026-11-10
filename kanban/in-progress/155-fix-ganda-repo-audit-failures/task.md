# Fix ganda repo audit failures

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

- [ ] `ganda repo audit --fix` and commit its changes
- [ ] Convert `workflow.yml` bash steps to pwsh; add top-level `defaults: run: shell: pwsh`
- [ ] Coverage upload step present (via `--fix` or by hand)
- [ ] `ganda repo audit` exit 0
- [ ] `dotnet build timewarp-flexbox.slnx` and `dotnet test timewarp-flexbox.slnx` green
- [ ] `./bin/dev --capabilities` works
- [ ] PR CI green

## Session

- Created: d7943e80-818a-43bd-a36a-6968aed6437a (2026-11-10)

## Notes

Previous audit cleanup: task 151. NuGet bump just before this: task 154 (PR #15).
