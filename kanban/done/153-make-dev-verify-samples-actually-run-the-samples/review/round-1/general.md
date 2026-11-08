# Round 1 — general
**Date:** 2026-11-09
**Scope reviewed:** Same as the framework: the branch vs merge-base with `master`, product files under `tools/dev-cli/endpoints/` and `samples/layout-demo/`.

## Summary

The stub is gone. `dev verify-samples` now finds runfiles and projects under `samples/`. It builds each one with `--artifacts-path` in a temp scratch directory, runs it with `--no-build` on every TFM the sample declares, and checks the `{out}` file declared in `verify-samples.args`. Build and run go through TimeWarp.Amuru with a timeout. Amuru 3 sends SIGINT and then kills the process tree, so a hung `dotnet run` child is not orphaned. Any failure sets `NuruCallContext.CurrentExitCode` and `Environment.ExitCode`. `workflow-command.cs:59` stops the workflow on a non-zero exit, so the gate works.

The change meets the task. The restatement matches the task, so there is no misread. Every requirement maps to code or to recorded evidence.

Verified in this review:

- `dotnet run tools/dev-cli/dev.cs -- verify-samples` exited 0 with `samples/layout-demo/layout-demo.cs (net10.0) ok, checked 1 output file(s)`.
- `git status --porcelain --ignored samples` was empty before and after the run. The `/tmp/timewarp-flexbox-verify-samples/` scratch directory was removed.
- `ganda repo audit` exited 0, with only the advisory `vscode-window-icon` warning.
- `git diff` shows `return 0;` unchanged in `layout-demo.cs`, so the negative-check break was not committed.

Not re-run here: the negative check and the full `dev workflow`. The product tree is read-only for the reviewer, and the full test suite is long. I accept the evidence recorded in Results.

I found no bugs. The issues below are small robustness and generality points.

## Issues

### Issue 1 — Severity: suggestion
- File: tools/dev-cli/endpoints/verify-samples-discovery.cs:112
- Description: `IsUnderBuildOutput` looks for `/bin/` or `/obj/` anywhere in the **absolute** path. If the repo is checked out under a directory named `bin` or `obj` (for example `~/bin/timewarp-flexbox`), every sample is skipped. The command then exits 1 with "samples/ has no runfiles or projects to verify", which points at the wrong cause. It fails closed, so a broken sample cannot pass; the risk is a confusing false failure, not a false success.
- Suggestion: Test the path relative to `samplesDirectory`, using `Path.GetRelativePath` and then checking each segment for `bin` or `obj`.
- Status: open

### Issue 2 — Severity: nit
- File: tools/dev-cli/endpoints/verify-samples-command.cs:115
- Description: The `{out}` file is always named `output.html`. That suits the layout demo, but a future sample that writes JSON or text gets a misleading extension. A sample that checks the extension would need a workaround.
- Suggestion: Let `verify-samples.args` choose the name, for example a `{outdir}/name.ext` form that counts as an expected output. Or document in the args-file header that `{out}` is always `.html`.
- Status: open

### Issue 3 — Severity: nit
- File: tools/dev-cli/endpoints/verify-samples-discovery.cs:158
- Description: `verify-samples.args` is split on single spaces with no quoting, so an argument that contains a space cannot be passed. Temp paths are not affected, because `{out}` is substituted after the split.
- Suggestion: Document "one token per whitespace-separated word, no quoting" in the args-file header, or read one argument per line.
- Status: open
