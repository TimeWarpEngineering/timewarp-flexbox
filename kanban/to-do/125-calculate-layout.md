# Task 125 - CalculateLayout (THE BIG ONE)

## Summary

Port the main CalculateLayout algorithm from C++ to C#. This is the core flexbox layout algorithm - the trunk that depends on everything else. This is a Level 9 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                                 | Lines      |
| ---------- | ------------------------------------ | ---------- |
| C++ Header | `yoga/algorithm/CalculateLayout.h`   | ~50        |
| C++ Source | `yoga/algorithm/CalculateLayout.cpp` | **~2,500** |

## Test Files - Unit Tests

| Type                 | Path                                       | Lines      |
| -------------------- | ------------------------------------------ | ---------- |
| C++ Test             | `tests/YGMeasureTest.cpp`                  | ~400       |
| C++ Test             | `tests/YGMeasureCacheTest.cpp`             | ~200       |
| C++ Test             | `tests/YGMeasureModeTest.cpp`              | ~300       |
| C++ Test             | `tests/YGBaselineFuncTest.cpp`             | ~150       |
| C++ Test             | `tests/YGAlignBaselineTest.cpp`            | ~200       |
| C++ Test             | `tests/YGAspectRatioTest.cpp`              | ~400       |
| C++ Test             | `tests/YGEdgeTest.cpp`                     | ~200       |
| C++ Test             | `tests/YGComputedMarginTest.cpp`           | ~150       |
| C++ Test             | `tests/YGComputedPaddingTest.cpp`          | ~150       |
| C++ Test             | `tests/YGHadOverflowTest.cpp`              | ~150       |
| C++ Test             | `tests/YGRelayoutTest.cpp`                 | ~200       |
| C++ Test             | `tests/YGRoundingFunctionTest.cpp`         | ~100       |
| C++ Test             | `tests/YGRoundingMeasureFuncTest.cpp`      | ~150       |
| C++ Test             | `tests/YGScaleChangeTest.cpp`              | ~100       |
| C++ Test             | `tests/YGZeroOutLayoutRecursivelyTest.cpp` | ~100       |
| C++ Test             | `tests/YGPersistenceTest.cpp`              | ~200       |
| C++ Test             | `tests/YGPersistentNodeCloningTest.cpp`    | ~150       |
| C++ Test             | `tests/FlexGapTest.cpp`                    | ~300       |
| **Unit Tests Total** |                                            | **~3,700** |

## Test Files - Generated Layout Tests

| Type                      | Path                                         | Lines       |
| ------------------------- | -------------------------------------------- | ----------- |
| C++ Test                  | `tests/generated/YGDimensionTest.cpp`        | ~50         |
| C++ Test                  | `tests/generated/YGSizeOverflowTest.cpp`     | ~100        |
| C++ Test                  | `tests/generated/YGBorderTest.cpp`           | ~150        |
| C++ Test                  | `tests/generated/YGPaddingTest.cpp`          | ~200        |
| C++ Test                  | `tests/generated/YGMarginTest.cpp`           | ~600        |
| C++ Test                  | `tests/generated/YGFlexTest.cpp`             | ~400        |
| C++ Test                  | `tests/generated/YGFlexDirectionTest.cpp`    | ~2,000      |
| C++ Test                  | `tests/generated/YGFlexWrapTest.cpp`         | ~1,200      |
| C++ Test                  | `tests/generated/YGJustifyContentTest.cpp`   | ~1,500      |
| C++ Test                  | `tests/generated/YGAlignItemsTest.cpp`       | ~2,000      |
| C++ Test                  | `tests/generated/YGAlignSelfTest.cpp`        | ~200        |
| C++ Test                  | `tests/generated/YGAlignContentTest.cpp`     | ~3,000      |
| C++ Test                  | `tests/generated/YGAbsolutePositionTest.cpp` | ~1,500      |
| C++ Test                  | `tests/generated/YGMinMaxDimensionTest.cpp`  | ~1,000      |
| C++ Test                  | `tests/generated/YGPercentageTest.cpp`       | ~1,200      |
| C++ Test                  | `tests/generated/YGDisplayTest.cpp`          | ~600        |
| C++ Test                  | `tests/generated/YGDisplayContentsTest.cpp`  | ~400        |
| C++ Test                  | `tests/generated/YGGapTest.cpp`              | ~2,000      |
| C++ Test                  | `tests/generated/YGRoundingTest.cpp`         | ~800        |
| C++ Test                  | `tests/generated/YGBoxSizingTest.cpp`        | ~1,500      |
| C++ Test                  | `tests/generated/YGStaticPositionTest.cpp`   | ~5,000      |
| C++ Test                  | `tests/generated/YGAutoTest.cpp`             | ~200        |
| C++ Test                  | `tests/generated/YGAspectRatioTest.cpp`      | ~100        |
| C++ Test                  | `tests/generated/YGIntrinsicSizeTest.cpp`    | ~3,000      |
| C++ Test                  | `tests/generated/YGAndroidNewsFeed.cpp`      | ~300        |
| **Generated Tests Total** |                                              | **~29,000** |

## Target Files

| Type      | Path                                                     |
| --------- | -------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/algorithm/calculate-layout.cs`  |
| C# Tests  | `tests/timewarp-flexbox-tests/layout/*.cs` (18+ files)   |
| C# Tests  | `tests/timewarp-flexbox-tests/generated/*.cs` (25 files) |

## Dependencies

- **ALL PREVIOUS TASKS (002-024)**

## Todo List

- [ ] Port `CalculateLayout.h` header
- [ ] Port `CalculateLayout.cpp` (~2,500 lines) - this is the main algorithm
- [ ] Port unit test files (18 files, ~3,700 lines)
- [ ] Port generated test files (25 files, ~29,000 lines)
- [ ] Run all tests and fix any discrepancies

## Acceptance Criteria

- [ ] Main layout algorithm working
- [ ] All ~3,700 lines of unit tests ported and passing
- [ ] All ~29,000 lines of generated tests ported and passing
- [ ] Layout results match C++ implementation exactly
- [ ] Performance is acceptable

## Notes

This is the **largest and most complex task**. Key algorithm sections:

1. **resolveFlexibleLength** - Calculate flex grow/shrink
2. **distributeFreeSpaceMainAxis** - Distribute space along main axis
3. **justifyMainAxis** - Apply justify-content
4. **alignCrossAxis** - Apply align-items/align-self
5. **calculateLayoutImpl** - Main recursive layout function

Consider breaking into sub-tasks:

- 025a: Basic single-axis layout
- 025b: Flex grow/shrink
- 025c: Wrapping
- 025d: Alignment
- 025e: Absolute positioning
- 025f: Pixel rounding

```csharp
public static class CalculateLayout
{
    public static void Calculate(
        Node node,
        float ownerWidth,
        float ownerHeight,
        Direction ownerDirection);
}
```
