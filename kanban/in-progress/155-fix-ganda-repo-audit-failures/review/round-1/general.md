# Round 1 — general
**Date:** 2026-11-10
**Scope reviewed:** branch `task/155-fix-ganda-repo-audit-failures` vs `master` (product commit bcf9c13): `.githooks/*.cs`, `.github/workflows/workflow.yml`, `.gitignore`, `.timewarp/suppression-baseline.json`, kanban move of task.md

## Summary

The branch clears the five audit findings. The `--fix` output is policy and is not flagged: the suppression baseline, the `.gitignore` entry, the refreshed attest hooks (pre-commit and pre-push now refuse when the root is missing, and the post-* hooks report failures on stderr), and the top-level `defaults: run: shell: pwsh`. The hand-ported pwsh steps keep the bash behavior. The release and non-release branches match the old ones. The API key now goes through `env:` instead of `${{ }}` interpolation into the script, which is safer. The AOT loop checks `$LASTEXITCODE` after each native command, which matches `bash -e`.

I re-ran `ganda repo audit` in the worktree. `analyzer-suppressions`, `ci-code-coverage`, `timewarp-signals-gitignore`, `workflow-pwsh-shell`, and `memsearch-scaffold` all PASS. The only failure is `kanban-landed` (KitchenNotLanded for 155). That is transient host walk state, not a product check, and it clears when the host lands the kitchen.

One finding remains. The `ci-code-coverage` check passes only because of an upload step for a directory that nothing creates.

## Issues

### Issue 1 — Severity: bug
- File: .github/workflows/workflow.yml:96
- Description: The new `coverage` step uploads `artifacts/coverage` with `if-no-files-found: warn`, but nothing in CI writes that path. `dev workflow` → `dev test` (`tools/dev-cli/endpoints/test-command.cs`) runs `DotNet.Test().WithConfiguration("Release")` with no `--collect "XPlat Code Coverage"` and no `--results-directory`. `coverlet.collector` (`test/timewarp-flexbox-tests/timewarp-flexbox-tests.csproj:17`) only runs when a collect argument is passed. The only effect of this step is to make the audit's `ci-code-coverage` check pass: it now reports "CI collects code coverage and uploads", which is false. The real result is an empty artifact and an "No files were found" warning on every run. The implementer's Results say the artifact "can be empty". In fact it is always empty. Per the review taxonomy, a change that makes an audit pass without doing the work it measures is a `bug`. Rule silenced: ganda audit `ci-code-coverage`.
- Suggestion: Make CI produce coverage at the uploaded path. Have `dev test` (or the workflow path through `dev workflow`) pass `--collect "XPlat Code Coverage" --results-directory artifacts/coverage`, or use the matching Amuru `DotNet.Test()` builder options. Then confirm that a local `dev test` writes `artifacts/coverage/**/coverage.cobertura.xml`. Once coverage is always expected, consider `if-no-files-found: error`. The other option is to decide that coverage is not wanted and remove `coverlet.collector` so the audit no longer infers collection. Either way, the upload must carry real data, or there must be no claim of coverage at all.
- Status: open

### Issue 2 — Severity: nit
- File: .github/workflows/workflow.yml:96
- Description: The step name `coverage` is lowercase. Every other step uses a Title Case phrase ("Run CI Pipeline", "AOT smoke test", "Upload Artifacts").
- Suggestion: Rename it to something like "Upload Coverage" when touching this step for Issue 1. If the audit fixer emits the name `coverage` by convention, leave it as is.
- Status: open
