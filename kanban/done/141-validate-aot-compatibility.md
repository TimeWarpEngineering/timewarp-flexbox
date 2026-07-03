# Task 141 - Validate AOT Compatibility

## Summary

Validate that TimeWarp.Flexbox is fully compatible with Native AOT compilation and
trimming, mark the package accordingly, and add a guard so compatibility cannot
silently regress. A layout engine is a natural fit for AOT consumers (CLI tools,
games, mobile via NativeAOT); the library should advertise and guarantee it.

Initial scan (2026-07-03) is promising: the library is pure computation with no
reflection, no `Enum.GetValues`, no serialization, no dynamic code — enum iteration
uses `Unsafe.As` over static ordinal counts, and callbacks are plain delegates.

## Todo List

- [x] Set `<IsAotCompatible>true</IsAotCompatible>` on
      `source/timewarp-flexbox/timewarp-flexbox.csproj` (implies `IsTrimmable` and
      enables the trim/AOT/single-file analyzers at build time)
- [x] Build and resolve any IL2xxx (trim) / IL3xxx (AOT) analyzer warnings — ZERO produced
      (note: `TreatWarningsAsErrors` is currently relaxed per task 139 — check the
      build output explicitly rather than relying on a red build)
- [x] Add an AOT smoke test: a minimal console consumer published with
      `<PublishAot>true</PublishAot>` that builds a layout tree (grow, wrap, RTL,
      absolute), calculates layout, and asserts a few computed values — run the
      native binary and check its output/exit code
- [x] Wire the AOT smoke test into CI (workflow.yml) so regressions fail the build;
      publish time is the main cost, so consider running it only on PRs to master
- [ ] (skipped, optional) Verify benchmark numbers under AOT out of curiosity (BenchmarkDotNet supports
      a NativeAOT toolchain) — optional, informational
- [x] Document AOT/trimming support in the readme and add
      `<PackageTags>...aot...</PackageTags>` if validated
- [x] Confirm the `TimeWarp.Build.Tasks` / analyzer packages (PrivateAssets=all)
      contribute nothing to the consumer's closure (they should not, but verify the
      nuspec has no dependency leakage)

## Notes

- Only the library (`source/timewarp-flexbox`) needs to be AOT-compatible; tests,
  benchmarks, and tools do not.
- `Event/Event.cs` (YogaEvent) and the measure/baseline delegates are plain C#
  delegates — AOT-safe, but the analyzers will confirm.
- `Config.Context` / `Node.Context` are `object?` bags; fine for AOT as long as the
  library never reflects over them (it does not).
- If analyzers surface anything in `StyleValuePool`/`SmallValueBuffer` (bit
  manipulation, `Unsafe`), these are AOT-safe patterns; warnings there would come
  from the analyzer being conservative — prefer targeted `UnconditionalSuppressMessage`
  with justification over blanket suppressions.

## Results (2026-07-04)

Validated. The library is fully Native AOT and trimming compatible.

- `IsAotCompatible=true` on the library: trim/AOT/single-file analyzers produce
  ZERO diagnostics (with warnings-as-errors active, so enforced). Package tags
  gained `aot;trimming`.
- New `tests/timewarp-flexbox-aot-smoke`: a zero-dependency PublishAot console
  app exercising grow, row positions, RTL, absolute insets, and wrap+gap with
  exact-value assertions. Verified under JIT (dotnet run) and as a native ELF
  binary (2.0 MB, linux-x64): "AOT smoke: PASS (all layout checks)", exit 0.
- CI: new "AOT smoke test" step in workflow.yml publishes and runs the native
  binary on every workflow run. Project added to the .slnx.
- nuspec verified: empty dependency group (analyzers/build-tasks do not leak),
  and the package now ships XML docs (from task 139's GenerateDocumentationFile).
- Readme documents the AOT/trimming guarantee.
- Skipped (optional): BenchmarkDotNet NativeAOT toolchain run — informational
  only; JIT numbers already recorded in task 138.
