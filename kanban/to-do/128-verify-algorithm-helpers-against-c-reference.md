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

- [ ] Verify `PixelGrid.cs`
  - [ ] Compare `RoundValueToPixelGrid` implementation
  - [ ] Compare `RoundLayoutResultsToPixelGrid` implementation
- [ ] Verify `Baseline.cs`
  - [ ] Compare `CalculateBaseline` logic
  - [ ] Compare `IsBaselineLayout` logic
- [ ] Verify `FlexLine.cs`
  - [ ] Compare `FlexLineRunningLayout` struct
  - [ ] Compare `FlexLine` class
  - [ ] Compare `CalculateFlexLine` method
  - [ ] Note: C# uses different iterator pattern than C++
- [ ] Verify `AlignUtils.cs`
  - [ ] Compare `ResolveChildAlignment` logic
  - [ ] Compare `FallbackAlignment` for Align
  - [ ] Compare `FallbackAlignment` for Justify
- [ ] Verify `BoundAxis.cs`
  - [ ] Compare `PaddingAndBorderForAxis`
  - [ ] Compare `BoundAxisWithinMinAndMax`
  - [ ] Compare `BoundAxisValue` (named `boundAxis` in C++)
- [ ] Verify `TrailingPosition.cs`
  - [ ] Compare `GetPositionOfOppositeEdge`
  - [ ] Compare `SetChildTrailingPosition`
  - [ ] Compare `NeedsTrailingPosition`

## Acceptance Criteria

- [ ] All implementations match C++ behavior
- [ ] Any discrepancies documented and fixed
- [ ] All tests still pass after any fixes

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
