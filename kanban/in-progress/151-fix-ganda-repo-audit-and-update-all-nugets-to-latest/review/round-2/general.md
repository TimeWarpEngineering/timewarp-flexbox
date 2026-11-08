# Round 2 — general
**Date:** 2026-11-09
**Scope reviewed:** fix-pass delta `facda76..9ac7d1f` (round-1 base.sha → fix commit), product paths only. Carries round-1 IDs M1–M5.

## Summary

The fix pass closes all five round-1 findings with small, targeted edits. CI installs the .NET 10 SDK and then the .NET 11 preview SDK. The Yoga porter and its comments use `generated`. Stale kebab links now point at files that exist. `[ganda.audit]` records why the line-cap baseline exists and names task 151. The layout demo is 497 lines, so it left the baseline. The merge of `AppendEngineBox` and `AppendEngineChildBox` gives the same output: top-level boxes pass origin `(0,0)`, which matches the old absolute `box.X`/`box.Y`, and nested boxes subtract the parent's position as before. Risk is low.

## Verification (this round)

- `ganda repo audit`: exit 0, "Repository passes all audit checks", `source-file-line-cap` PASS.
- `dotnet run samples/layout-demo/layout-demo.cs -- --html /tmp/r2-demo.html`: exit 0, HTML written.
- `wc -l samples/layout-demo/layout-demo.cs`: 497. The baseline has no layout-demo entry.
- These files exist: `license`, `source/timewarp-flexbox/style/Style.cs`, `source/timewarp-flexbox/enums/YGEnums.cs`, `test/timewarp-flexbox-tests/generated/IntrinsicSizeMeasure.cs`, `.agent/workspace/2025-12-09-t12-30-00-gh-issue-2-idempotency-analysis.md`.
- `git grep` (outside kanban) for `Generated/`, `/Style/`, `Enums/YGEnums`, `(LICENSE)`, and `T12-30-00_` finds nothing.
- No `global.json` pins the SDK, so the 11.0.x preview SDK on the runner can build the net11.0 dev CLI.
- `tools/dev-cli/global-usings.cs:19` points at dev.cs Design, and `dev.cs` Design now has the matching `TimeWarp.Mediator` line.

## Prior findings

| ID | Severity | Status | Evidence |
|----|----------|--------|----------|
| M1 | bug | fixed | `.github/workflows/workflow.yml:43-54` installs `10.0.x`, then `11.0.x` with `dotnet-quality: preview` |
| M2 | bug | fixed | `runfiles/port-generated-tests.cs:8,34,197` use `generated` |
| M3 | suggestion | fixed | readme `license`, SKILL.md kebab paths, and the integration note's kebab filename all resolve |
| M4 | suggestion | fixed | `.editorconfig` `[ganda.audit]` comment names task 151 and the reason; layout-demo is out of the baseline at 497 lines |
| M5 | nit | fixed | `tools/dev-cli/dev.cs:25` Design names `TimeWarp.Mediator` |

## Issues

None new.
