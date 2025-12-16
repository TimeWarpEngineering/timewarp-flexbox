# Task 127 - Add Algorithm Helper Tests

## Summary

Create unit tests for all algorithm helper classes. Currently only `CacheTests.cs` exists. This is a subtask of Task 123.

## Target Files

| Type      | Path                                                           |
| --------- | -------------------------------------------------------------- |
| C# Tests  | `test/timewarp-flexbox-tests/Algorithm/PixelGridTests.cs`      |
| C# Tests  | `test/timewarp-flexbox-tests/Algorithm/BaselineTests.cs`       |
| C# Tests  | `test/timewarp-flexbox-tests/Algorithm/FlexLineTests.cs`       |
| C# Tests  | `test/timewarp-flexbox-tests/Algorithm/AlignUtilsTests.cs`     |
| C# Tests  | `test/timewarp-flexbox-tests/Algorithm/BoundAxisTests.cs`      |
| C# Tests  | `test/timewarp-flexbox-tests/Algorithm/TrailingPositionTests.cs` |

## Dependencies

- Task 126: Complete PixelGrid (for full PixelGrid tests)

## Checklist

- [x] Create `PixelGridTests.cs`
  - [x] Test `RoundValueToPixelGrid` with various scale factors
  - [x] Test ceil/floor forcing
  - [x] Test negative values
  - [x] Test NaN handling
- [x] Create `BaselineTests.cs`
  - [x] Test `CalculateBaseline` with custom baseline func
  - [x] Test `CalculateBaseline` without baseline func
  - [x] Test `IsBaselineLayout` for row/column directions
- [x] Create `FlexLineTests.cs`
  - [x] Test `CalculateFlexLine` line breaking
  - [x] Test auto margin counting
  - [x] Test flex factor accumulation
- [x] Create `AlignUtilsTests.cs`
  - [x] Test `ResolveChildAlignment` with various align values
  - [x] Test `FallbackAlignment` for Align enum
  - [x] Test `FallbackAlignment` for Justify enum
- [x] Create `BoundAxisTests.cs`
  - [x] Test `PaddingAndBorderForAxis`
  - [x] Test `BoundAxisWithinMinAndMax` with min/max constraints
  - [x] Test `BoundAxisValue` padding floor
- [x] Create `TrailingPositionTests.cs`
  - [x] Test `GetPositionOfOppositeEdge`
  - [x] Test `SetChildTrailingPosition`
  - [x] Test `NeedsTrailingPosition` for reversed axes

## Acceptance Criteria

- [x] All tests pass
- [x] Each algorithm helper class has corresponding test file
- [x] Edge cases covered (NaN, negative values, zero scale factor)

## Notes

Follow the existing test patterns from `CacheTests.cs`:
- Use Shouldly assertions
- Group tests by region
- Test both success and failure cases

## Results

All 6 test files created with comprehensive test coverage:
- **AlignUtilsTests.cs**: Tests for ResolveChildAlignment and FallbackAlignment
- **TrailingPositionTests.cs**: Tests for GetPositionOfOppositeEdge, SetChildTrailingPosition, NeedsTrailingPosition
- **PixelGridTests.cs**: Tests for RoundValueToPixelGrid with scale factors, ceil/floor, negative values, NaN
- **BaselineTests.cs**: Tests for IsBaselineLayout and CalculateBaseline
- **FlexLineTests.cs**: Tests for FlexLine and FlexLineRunningLayout
- **BoundAxisTests.cs**: Tests for PaddingAndBorderForAxis, BoundAxisWithinMinAndMax, BoundAxisValue

Total: 108 new algorithm tests, all passing.
