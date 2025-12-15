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
