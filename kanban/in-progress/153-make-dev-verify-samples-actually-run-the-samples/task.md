# Make dev verify-samples actually run the samples

## Description

`dev verify-samples` (`tools/dev-cli/endpoints/verify-samples-command.cs`) is a
stub: it prints "Samples verified successfully!" and checks nothing. `dev workflow`
runs it on every PR and release, so a broken sample passes CI. Task 152 had to
run the layout demo by hand to prove it worked.

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

- [ ] Discover samples under `samples/`
- [ ] Build + run each with a timeout; fail on error or timeout
- [ ] Check expected sample output; keep the tree clean
- [ ] Non-zero exit and clear message on failure; stops `dev workflow`
- [ ] Negative check recorded in Results
- [ ] Purpose/Design regions updated
- [ ] `dev workflow` green locally and in CI; audit exits 0

## Notes

- Found in task 152 Results (2026-11-09): "`verify-samples` is still a stub that
  prints success."

## Session

- Created: b3bc5dc2-e0c9-4b04-ac76-4a1bd4f7a1f3 (2026-11-09)
