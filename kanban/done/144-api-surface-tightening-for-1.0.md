# Task 144 - API Surface Tightening for 1.0

## Summary

The package currently exports 93 public types, of which only ~a dozen are the
intended consumer API. The rest are engine internals that are public only
because the Yoga port translated C++ file-by-file. 1.0.0 freezes the public
surface under semver, so this must happen BEFORE the stable release —
internalizing anything afterward is a breaking change. This is the only
blocker for 1.0 (see task 147).

## Intended public surface (keep public)

`Node`, `Style`, `Config`, `CalculateLayout`, `LayoutResults`,
`StyleLength`, `StyleSizeLength`, `YGValue`, `YGSize`, `FloatOptional`,
`YogaAssertException`, the delegates (`MeasureFunc`, `BaselineFunc`,
`DirtiedFunc`, `CloneNodeFunc`, `YogaLogHandler`), and the enums
(`FlexDirection`, `Justify`, `Align`, `Wrap`, `Edge`, `Gutter`, `Dimension`,
`PhysicalEdge`, `Direction`, `PositionType`, `Overflow`, `Display`,
`BoxSizing`, `Unit`, `MeasureMode`, `NodeType`, `Errata`,
`ExperimentalFeature`, `LogLevel`). Review each remaining public member on
these types too (e.g. `Node.CloneChildrenIfNeeded`, layout setters) — some
are engine plumbing.

## Todo List

- [x] Internalize the algorithm machinery: `AbsoluteLayout`, `AlignUtils`,
      `Baseline`, `BoundAxis`, `Cache`, `CalculateLayoutCore`, `FlexBasis`,
      `FlexDirectionUtils`, `FlexDistribution`, `FlexLine`, `JustifyContent`,
      `LayoutHelpers`, `MeasureNode`, `PixelGrid`, `TrailingPosition`,
      `SizingMode`(+extensions), `LayoutData`, `LayoutPassReason`(+extensions)
- [x] Internalize storage/infra: `StyleValuePool`, `StyleValueHandle`,
      `SmallValueBuffer`(+generic), `LayoutableChildren`, `CachedMeasurement`,
      `Comparison`, `YogaEnums`(+extensions), `OrdinalCountAttribute`,
      `YogaEvent`/`LayoutData` event plumbing, `YogaLog`, `YogaAssert`
- [x] **Eliminate the mutable public static wiring**: `FlexBasis.CalculateLayoutInternal`
      is a public static settable delegate — any consumer can reassign it and break
      the engine globally. Make the wiring internal (or replace the delegate
      indirection with a direct internal call; it exists only to break a
      circular file dependency from the C++ port)
- [x] Add `[assembly: InternalsVisibleTo("TimeWarp.Flexbox.Tests")]` — the unit
      tests exercise the machinery directly (keep them; they're ported Yoga tests)
- [x] Confirm naming decisions that 1.0 freezes: `YGValue`/`YGSize` Yoga-parity
      names (recommended: keep — they signal lineage), `CalculateLayout.Calculate`
      static entry point vs. a `Node.CalculateLayout()` instance convenience
      (consider adding the instance method; additive later, but nicer at 1.0)
- [x] Rebuild + full suite (1335/0/3 pins behavior), AOT smoke, `dotnet pack`
      and inspect the package's public API (e.g. dotnet-inspect) to confirm the
      final surface
- [x] Update the flexbox skill / readme if any referenced type changed visibility

## Notes

- The 530 generated conformance tests use only the consumer API — they are the
  guarantee that internalization broke nothing behavioral.
- After this task, the public surface should be reviewable in one screenful.

## Results (2026-07-04)

Public surface reduced from 93 to 39 types: 19 enums, 6 value types
(YGValue, YGSize, FloatOptional, StyleLength, StyleSizeLength,
ExperimentalFeatureSet), 5 delegates, 9 classes (Node, Style, Config,
CalculateLayout, LayoutResults, Comparison, StyleLength/StyleSizeLength
extensions, YogaAssertException). All algorithm machinery, storage internals,
event system, and debug utilities are now internal; the mutable
FlexBasis.CalculateLayoutInternal delegate is internal with the type.

Deviations from the plan, deliberate:
- `Comparison` kept public: tiny, useful (IsUndefined/IsDefined for NaN
  semantics), documented in the flexbox skill.
- `ExperimentalFeatureSet` kept public (readonly struct surfaced by
  Config.EnabledExperiments).

API improvements while the surface was open:
- `Node.GetChild(int)` now returns `Node` (was ILayoutableNode); the duplicate
  `GetChildNode` removed; ILayoutableNode members explicitly implemented.
- Added `Node.CalculateLayout(availableWidth = NaN, availableHeight = NaN,
  ownerDirection = LTR)` instance convenience; docs/skill updated.
- InternalsVisibleTo("timewarp-flexbox-tests") added; MockNode test helper
  internalized — which removed 5 phantom Fixie-discovered entries (suite is
  now a clean 1333 passed / 0 failed / 0 skipped; previously 1335+3 included
  MockNode's methods).

Verified: gated Release build zero warnings, suite 1333/0/0, AOT smoke native
binary PASS, layout demo runs, dotnet format stable, ganda audit 22/22.
Naming frozen deliberately: YGValue/YGSize Yoga-parity names kept.
