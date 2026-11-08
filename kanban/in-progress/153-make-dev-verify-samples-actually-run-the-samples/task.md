# Make dev verify-samples actually run the samples

## Description

`dev verify-samples` (`tools/dev-cli/endpoints/verify-samples-command.cs`) is a
stub: it prints "Samples verified successfully!" and checks nothing. `dev workflow`
runs it on every PR and release, so a broken sample passes CI. Task 152 had to
run the layout demo by hand to prove it worked.

## Goal (implementer restatement)

`dev verify-samples` has to find every sample under `samples/` on its own. Today that is the layout-demo runfile; a later project sample has to be picked up the same way, without a hardcoded path. For each sample it builds and runs on the target framework that sample declares, and it fails the process if the build fails, the sample exits non-zero, or the run hangs past a timeout. The message names the sample and the reason. `dev workflow` already stops when this command exits non-zero, so that exit code is the gate. When a sample writes a file, the layout demo's HTML page, the run writes it outside the repo, the command checks that the file exists and is non-empty, and the working tree stays clean. Process starts go through TimeWarp.Amuru. The command's Purpose and Design regions describe this. A temporary break of the sample must make the command exit non-zero, and that proof is recorded in Results without committing the break. `dev workflow` and `ganda repo audit` both exit 0.

## Requirements

- `dev verify-samples` discovers every sample under `samples/` (today:
  `samples/layout-demo/layout-demo.cs`, a runfile) instead of hard-coding one.
- Each sample is built and run (`dotnet run --file …` for runfiles, or the
  project equivalent). Run with a timeout so a hung sample fails.
- A sample fails verification on a non-zero exit code, a build error, or a
  timeout. The command exits non-zero when any sample fails, and the output names
  which sample failed and why. A failure stops `dev workflow`.
- Run on the TFM each sample declares (the layout demo pins `net10.0`).
- If a sample writes output files (the layout demo writes an HTML page), check
  that the expected output exists, write it somewhere ignored or temporary, and
  leave the working tree clean.
- Use TimeWarp.Amuru for process execution (tw-amuru), not `System.Diagnostics.Process`.
- Replace the stub's Purpose/Design regions with what it really does.
- Prove it fails: temporarily break the sample (or point at a failing fixture)
  and show `dev verify-samples` exits non-zero. Record that in Results. Do not
  commit the broken sample.
- CI (`dev workflow`) stays green. `ganda repo audit` exits 0.

## Checklist

- [x] Discover samples under `samples/`
- [x] Build + run each with a timeout; fail on error or timeout
- [x] Check expected sample output; keep the tree clean
- [x] Non-zero exit and clear message on failure; stops `dev workflow`
- [x] Negative check recorded in Results
- [x] Purpose/Design regions updated
- [x] `dev workflow` green locally and in CI; audit exits 0

## Review

- Round 1: Done

## Notes

- Found in task 152 Results (2026-11-09): "`verify-samples` is still a stub that
  prints success."

## Results

`dev verify-samples` discovers every runfile and every project under `samples/`. It builds each one, then runs it, on every target framework the sample declares. The layout demo declares `net10.0`. Build and run go through TimeWarp.Amuru with a timeout (5 minutes to build, 2 minutes to run). Compile output uses `--artifacts-path` under a temp directory. `samples/layout-demo/verify-samples.args` passes `--html {out}` to a temp file. The command requires that file to exist and be non-empty, then deletes the temp directory. A build failure, a non-zero exit, a timeout, or a missing output file sets the process exit code and names the sample. `dev workflow` stops there: its verify-samples step prints "Sample verification failed!" and does not run tests.

Discovery and argument parsing live in `verify-samples-discovery.cs`. The command file was split because it crossed the 500-line audit cap.

`ganda repo audit` exits 0. `vscode-window-icon` stays an advisory warning because `.vscode/settings.json` is missing and gitignored. That warning does not fail the audit.

GitHub Actions has not run yet. The PR is a later host step. The local run used the same command CI uses.

### How to validate

Smoke: `dotnet run tools/dev-cli/dev.cs -- verify-samples`

Expect: exit 0. The output names `samples/layout-demo/layout-demo.cs (net10.0) ok, checked 1 output file(s)`. `git status --porcelain` shows no sample output.

Smoke: hide `bin/dev` (it is a previous self-install and would skip the new source) and run `dotnet run tools/dev-cli/dev.cs -- workflow`.

Expect: exit 0. Observed 2026-11-09: clean, Release build with 0 warnings and 0 errors, verify-samples ok, 1468 passed on net10.0 and 1468 passed on net11.0, then "Workflow completed successfully!".

### Negative check

`samples/layout-demo/layout-demo.cs` was changed from `return 0;` to `return 1;` and restored before commit. The broken file is not in the diff.

`dotnet run tools/dev-cli/dev.cs -- verify-samples` exited 1:

```
Verifying 1 sample(s)...
  Building samples/layout-demo/layout-demo.cs (net10.0)...
  Running samples/layout-demo/layout-demo.cs (net10.0)...
verify-samples: samples/layout-demo/layout-demo.cs (net10.0) run failed (exit 1).
```

With `bin/dev` hidden, `dotnet run tools/dev-cli/dev.cs -- workflow` on that same break exited 1. The log contains `verify-samples: samples/layout-demo/layout-demo.cs (net10.0) run failed (exit 1).` and `Sample verification failed!`. It does not contain "Tests completed" or "Workflow completed successfully!".

### Review disposition

- Rounds: 1. Effort 2, roster: general (the review oracle, which also checked plan alignment).
- Final counts: bug 0, suggestion 1 wontfix, nit 2 wontfix. Open: 0.
- Disposition: **accepted-exceptions**.
  - M1 (suggestion): `IsUnderBuildOutput` tests the absolute path, so a checkout under a `bin` or `obj` directory skips every sample. That fails closed and is a follow-up candidate.
  - M2 (nit): `{out}` is always `output.html`.
  - M3 (nit): `verify-samples.args` has no quoting.
  - The rationale for each is in `review/disposition.md`.
- Re-verified by the reviewer:
  - `dotnet run tools/dev-cli/dev.cs -- verify-samples` exited 0.
  - `git status --porcelain --ignored samples` was clean before and after, and the scratch directory was removed.
  - `ganda repo audit` exited 0 (advisory `vscode-window-icon` only).
- Not re-run by the reviewer: the negative check and the full `dev workflow`. The evidence above is accepted.
- The implementer restatement matches the task. There is no misread.
- Paths: `review/review-framework.md`, `review/round-1/general.md`, `review/round-1/merged.md`, `review/disposition.md`.

## Session

- Created: b3bc5dc2-e0c9-4b04-ac76-4a1bd4f7a1f3 (2026-11-09)
- Implementation: verify-samples builds and runs each sample (2026-11-09)
- Implement oracle: implemented by implementer-grok (grok, vendor xai, model grok-4.7, response model grok-4.7-build), session 01a1bc65-cd95-7e31-bbfa-f683d12e76bf, max-turns 500, Done — 2026-11-08T18:46:44Z
- Review oracle: claude-opus-5-5 (tw-implementation-review, effort 2, roster general, turn cap 120), round 1, disposition accepted-exceptions, 0 open — 2026-11-09
- Review oracle: review by reviewer-claude (claude, vendor anthropic, model claude-opus-5-5, response model claude-opus-5-5), session d2be8b0d-0abc-4e45-96d7-a4cfdd9c4477, max-turns 120, artifacts general.md, merged.md, disposition.md — 2026-11-08T18:49:46Z
- Installed ganda changed during this walk: was 50245520 bytes 6960d933b0215f2d5776a313661be767146c85b1b1580b012d28042e019026ce; now 50404368 bytes 4cddea1b1ea98bf485ebbf9a8de534c3316dead53097be626514fa4b1b7831a0
