# Task 139 - Restore Style Enforcement After Port

## Summary

Style enforcement (`TreatWarningsAsErrors` + `EnforceCodeStyleInBuild`) is temporarily
relaxed in `Directory.Build.props` so the Yoga port can build and CI can run. The ported
code is written with 4-space indentation while `.editorconfig` mandates 2-space, producing
~15,600 IDE0055 errors (plus IDE1006 naming and assorted CA/RCS diagnostics) on a plain
`dotnet build`. Once the port stabilizes, reformat the tree and re-enable the gates.

## Todo List

- [x] Decide the indentation standard — kept 2-space per .editorconfig
- [x] Run `dotnet format` across the solution and commit the result as a standalone
      formatting-only commit (no logic changes mixed in) — commit eb29f1a, 88 files,
      ~15.5k IDE0055 errors eliminated; suite verified unchanged (1335/0/3)
- [ ] Fix or explicitly suppress remaining analyzer diagnostics. Remaining with gates on
      (~120 unique errors as of 2026-07-03):
      - **IDE1006 (~83, DECISION NEEDED):** ported code uses `_camelCase` private fields
        paired with PascalCase properties (`_direction`/`Direction`); .editorconfig forbids
        the `_` prefix and PascalCase field names would collide with the properties.
        Either add a naming rule allowing `_camelCase` (+ `s_` for static) private fields,
        or restructure field/property pairs (invasive).
      - **IDE0078/0072/0010/0011/0251/0370/0004 (~37):** pattern matching, switch
        exhaustiveness, braces, readonly members — no batch fixer; hand-fix per file
        (`dotnet format style --diagnostics ...` only resolved 4 files)
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
