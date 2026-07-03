# Task 139 - Restore Style Enforcement After Port

## Summary

Style enforcement (`TreatWarningsAsErrors` + `EnforceCodeStyleInBuild`) is temporarily
relaxed in `Directory.Build.props` so the Yoga port can build and CI can run. The ported
code is written with 4-space indentation while `.editorconfig` mandates 2-space, producing
~15,600 IDE0055 errors (plus IDE1006 naming and assorted CA/RCS diagnostics) on a plain
`dotnet build`. Once the port stabilizes, reformat the tree and re-enable the gates.

## Todo List

- [ ] Decide the indentation standard: either update `.editorconfig` to 4-space (matches the
      ported code and Yoga C++ style) or keep 2-space and reformat the code
- [ ] Run `dotnet format` across the solution and commit the result as a standalone
      formatting-only commit (no logic changes mixed in)
- [ ] Fix or explicitly suppress remaining analyzer diagnostics (IDE1006 naming,
      CA/RCS rules) with justification comments where suppressed
- [ ] Re-enable `TreatWarningsAsErrors`, `CodeAnalysisTreatWarningsAsErrors`, and
      `EnforceCodeStyleInBuild` in `Directory.Build.props` (remove the temporary relaxation
      block referencing this task)
- [ ] Verify plain `dotnet build` and `dotnet test` pass with gates re-enabled
- [ ] Verify CI workflow goes green

## Notes

- The relaxation was added 2026-07-03 while fixing the layout algorithm (debug module port,
  owner semantics, flex basis, measure func). See `Directory.Build.props` comment.
- Do the reformat in one dedicated commit so `git blame` stays useful via
  `.git-blame-ignore-revs` if desired.
- `agents.md` states the repo standard is 2-space indentation; if 4-space is chosen instead,
  update `agents.md` and `.editorconfig` together.

## Results

(Add after completion)
