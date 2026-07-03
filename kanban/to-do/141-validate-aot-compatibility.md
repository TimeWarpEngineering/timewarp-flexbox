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

- [ ] Set `<IsAotCompatible>true</IsAotCompatible>` on
      `source/timewarp-flexbox/timewarp-flexbox.csproj` (implies `IsTrimmable` and
      enables the trim/AOT/single-file analyzers at build time)
- [ ] Build and resolve any IL2xxx (trim) / IL3xxx (AOT) analyzer warnings
      (note: `TreatWarningsAsErrors` is currently relaxed per task 139 — check the
      build output explicitly rather than relying on a red build)
- [ ] Add an AOT smoke test: a minimal console consumer published with
      `<PublishAot>true</PublishAot>` that builds a layout tree (grow, wrap, RTL,
      absolute), calculates layout, and asserts a few computed values — run the
      native binary and check its output/exit code
- [ ] Wire the AOT smoke test into CI (workflow.yml) so regressions fail the build;
      publish time is the main cost, so consider running it only on PRs to master
- [ ] Verify benchmark numbers under AOT out of curiosity (BenchmarkDotNet supports
      a NativeAOT toolchain) — optional, informational
- [ ] Document AOT/trimming support in the readme and add
      `<PackageTags>...aot...</PackageTags>` if validated
- [ ] Confirm the `TimeWarp.Build.Tasks` / analyzer packages (PrivateAssets=all)
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

## Results

(Add after completion)
