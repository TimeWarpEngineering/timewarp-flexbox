# Task 101 - Port Yoga to 100% C#

## Summary

Create a complete C# implementation of the Yoga flexbox layout engine, porting from the existing C++ codebase. This is an epic task broken into levels based on the dependency graph - each level only depends on previously completed levels.

**Each task includes both implementation AND its corresponding tests ported 1:1 from C++.**

## Todo List

### Implementation + Tests (by dependency level)

- [ ] **Level 0**: Zero-dependency foundations
  - 002: YogaEnums utilities + `OrdinalsTest.cpp`
  - 003: SmallValueBuffer + `SmallValueBufferTest.cpp`
- [ ] **Level 1**: Basic types
  - 004: Comparison utilities
  - 005: YGEnums base definitions
- [ ] **Level 2**: Numeric and value types
  - 006: FloatOptional + `FloatOptionalTest.cpp` (~180 lines)
  - 007: YGValue + `YGValueTest.cpp`
- [ ] **Level 3**: Style primitives
  - 008: StyleLength types
  - 009: PhysicalEdge
  - 010: LayoutableChildren
- [ ] **Level 4**: Infrastructure
  - 011: StyleValueHandle
  - 012: Debug utilities
  - 013: Event system + `EventsTest.cpp` (~330 lines)
  - 014: Config + `YGConfigTest.cpp`
  - 015: SizingMode
- [ ] **Level 5**: Style system
  - 016: StyleValuePool + `StyleValuePoolTest.cpp`
  - 017: FlexDirection utilities
  - 018: CachedMeasurement
- [ ] **Level 6**: Layout results
  - 019: LayoutResults
  - 020: Cache utilities
  - 021: Style + `StyleTest.cpp`
- [ ] **Level 7**: Node
  - 022: Node + multiple test files (~1,150 lines):
    - `YGDefaultValuesTest.cpp`
    - `YGDirtiedTest.cpp`
    - `YGDirtyMarkingTest.cpp`
    - `YGNodeChildTest.cpp`
    - `YGCloneNodeTest.cpp`
    - `YGTreeMutationTest.cpp`
    - `YGLayoutableChildrenTest.cpp`
    - `YGNodeCallbackTest.cpp`
- [ ] **Level 8**: Algorithm helpers
  - 023: PixelGrid, Baseline, FlexLine, Align, BoundAxis, TrailingPosition
  - 024: AbsoluteLayout

## Notes

### 2025-12-15 Implementation Progress

**Completed Tasks:**
- Task 102 (YogaEnums): Implemented `OrdinalCountAttribute`, `YogaEnums.OrdinalCount<T>()`, `BitCount<T>()`, `ToUnderlying<T>()`, and `Ordinals<T>()` iterator. 5 tests passing.
- Task 103 (SmallValueBuffer): Ported memory-efficient 32/64-bit value storage with overflow handling. Uses C# generics with `IBufferSize` marker interface for compile-time buffer sizing. 17 tests passing.
- Task 104 (Comparison): Implemented float comparison utilities including `IsUndefined`, `IsDefined`, `MaxOrDefined`, `MinOrDefined`, and `InexactEquals` with 0.0001f tolerance. 39 tests passing.
- Task 106 (FloatOptional): Ported the core optional float type using NaN as undefined sentinel. Full operator overloads. 39 tests passing.
- Task 105 (YGEnums Base): Ported all 17 Yoga enums (Align, BoxSizing, Dimension, Direction, Display, Edge, Errata, ExperimentalFeature, FlexDirection, Gutter, Justify, LogLevel, MeasureMode, NodeType, Overflow, PositionType, Unit, Wrap) with `ToCssString()` extension methods for CSS-compatible string conversion. 85 new tests (180 total).
- Task 107 (YGValue): Ported the core dimension value type with unit support (Point, Percent, Auto, Undefined, FitContent, MaxContent, Stretch). Implements C++ equality semantics where unit-only types (Auto, Undefined, etc.) compare equal regardless of float value. Includes `YGValueUtilities` with `YGUndefined` constant and `YGFloatIsUndefined()` function. 37 new tests (217 total).

**Key Design Decisions Made:**
1. `OrdinalCountAttribute` pattern: C++ uses template specialization; C# uses attribute on enum types
2. `SmallValueBuffer` uses `IBufferSize` interface with static abstract members for compile-time buffer sizing
3. `FloatOptional.Undefined` returns `new FloatOptional(float.NaN)` since C# struct default is 0 (unlike C++ which defaults to NaN)
4. All value types use `readonly struct` for proper value semantics and zero allocation
5. All enums consolidated into single `YGEnums.cs` file with `ToCssString()` extension methods (mirrors C++ `XxxToString()` functions)
6. `Errata` enum uses `[Flags]` with CA2217 suppressed - intentional non-power-of-two values matching C++ Yoga
7. `YGValue` equality follows C++ semantics: unit-only types (Auto, Undefined, FitContent, MaxContent, Stretch) ignore the float value in comparisons

**Test Count:** 217 tests total, all passing

### Test-Driven Approach

Every task includes porting its corresponding C++ tests **before** or **alongside** the implementation:

- Unit tests: `tests/*.cpp` (37 files, ~5,500 lines)
- Generated tests: `tests/generated/*.cpp` (25 files, ~44,000 lines)
- **Total: ~50,000 lines of tests to port**

A task is NOT complete until all its tests pass with identical results to C++.

### Conversion Order Rationale

The C++ codebase has a clear dependency hierarchy. We port from leaves (zero dependencies) toward the trunk (CalculateLayout which depends on everything). This ensures:

1. Each level compiles independently
2. Tests validate each component before moving on
3. No forward declarations or circular dependency issues
4. Clear progress tracking

### Key Design Decisions for C#

- Use `readonly struct` for value types (FloatOptional, StyleLength, etc.)
- Use `record struct` where equality semantics matter
- Replace C++ templates with C# generics where applicable
- Use `Span<T>` for performance-critical paths
- Consider source generators for enum utilities

### Reference Files

See `dependency-map.md` in this folder for the complete C++ file dependency analysis.

## Results

(Add after completion)
