# Task 128 - Verify Algorithm Helpers Against C++ Reference

## Summary

Verify all algorithm helper implementations match the C++ Yoga reference. Fix any discrepancies found. This is a subtask of Task 123.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| C++ File                          | C# File                                    |
| --------------------------------- | ------------------------------------------ |
| `yoga/algorithm/PixelGrid.cpp`    | `source/timewarp-flexbox/Algorithm/PixelGrid.cs` |
| `yoga/algorithm/Baseline.cpp`     | `source/timewarp-flexbox/Algorithm/Baseline.cs` |
| `yoga/algorithm/FlexLine.cpp`     | `source/timewarp-flexbox/Algorithm/FlexLine.cs` |
| `yoga/algorithm/Align.h`          | `source/timewarp-flexbox/Algorithm/AlignUtils.cs` |
| `yoga/algorithm/BoundAxis.h`      | `source/timewarp-flexbox/Algorithm/BoundAxis.cs` |
| `yoga/algorithm/TrailingPosition.h` | `source/timewarp-flexbox/Algorithm/TrailingPosition.cs` |

## Dependencies

- Task 126: Complete PixelGrid
- Task 127: Add Algorithm Helper Tests

## Checklist

- [x] Verify `PixelGrid.cs`
  - [x] Compare `RoundValueToPixelGrid` implementation
  - [x] Compare `RoundLayoutResultsToPixelGrid` implementation
- [x] Verify `Baseline.cs`
  - [x] Compare `CalculateBaseline` logic
  - [x] Compare `IsBaselineLayout` logic
- [x] Verify `FlexLine.cs`
  - [x] Compare `FlexLineRunningLayout` struct
  - [x] Compare `FlexLine` class
  - [x] Compare `CalculateFlexLine` method
  - [x] Fix: Added `PendingChild` property and `pendingChild` parameter to handle C# iterator differences
- [x] Verify `AlignUtils.cs`
  - [x] Compare `ResolveChildAlignment` logic
  - [x] Compare `FallbackAlignment` for Align
  - [x] Compare `FallbackAlignment` for Justify
- [x] Verify `BoundAxis.cs`
  - [x] Compare `PaddingAndBorderForAxis`
  - [x] Compare `BoundAxisWithinMinAndMax`
  - [x] Compare `BoundAxisValue` (named `boundAxis` in C++)
- [x] Verify `TrailingPosition.cs`
  - [x] Compare `GetPositionOfOppositeEdge`
  - [x] Compare `SetChildTrailingPosition`
  - [x] Compare `NeedsTrailingPosition`

## Acceptance Criteria

- [x] All implementations match C++ behavior
- [x] Any discrepancies documented and fixed
- [x] All tests still pass after any fixes

## Notes

Known differences to account for:
1. C# uses `ref struct Enumerator` vs C++ iterator pattern in FlexLine
2. Method naming: `BoundAxisValue` (C#) vs `boundAxis` (C++)
3. C# uses `ArgumentNullException.ThrowIfNull` for null checks

When verifying, pay attention to:
- Float vs double usage
- NaN handling
- Edge cases (zero, negative values)
- Operator overloads (FloatOptional comparisons)

## Verification Results

### 2025-12-16 Verification Summary

| File | Status | Notes |
|------|--------|-------|
| `PixelGrid.cs` | Verified | Logic matches C++ exactly |
| `Baseline.cs` | Verified | Logic matches C++ exactly |
| `FlexLine.cs` | Fixed | Iterator pattern issue fixed (see below) |
| `AlignUtils.cs` | Verified | Logic matches C++ exactly |
| `BoundAxis.cs` | Verified | Logic matches C++ exactly |
| `TrailingPosition.cs` | Verified | Logic matches C++ exactly |

### FlexLine.cs Fix

**Problem:** The C++ `calculateFlexLine` function uses a for-loop where the iterator is incremented at the end of each iteration. When a child triggers a line break, the `break` statement leaves the iterator pointing to that child, which becomes the first item in the next line.

The original C# implementation used `while (iterator.MoveNext())` which advances the iterator at the start. When `break` was called, the child that triggered the break had been consumed but not added to the line, causing that child to be lost.

**Solution:** 
1. Added `PendingChild` property to `FlexLine` to store the child that triggered the break
2. Added optional `pendingChild` parameter to `CalculateFlexLine` to receive the pending child from the previous line
3. The calling code must pass `previousLine.PendingChild` when calculating subsequent lines

This maintains semantic equivalence with the C++ implementation while working within C#'s iterator pattern.
