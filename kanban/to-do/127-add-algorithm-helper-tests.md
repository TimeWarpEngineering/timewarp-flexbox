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

- [ ] Create `PixelGridTests.cs`
  - [ ] Test `RoundValueToPixelGrid` with various scale factors
  - [ ] Test ceil/floor forcing
  - [ ] Test negative values
  - [ ] Test NaN handling
- [ ] Create `BaselineTests.cs`
  - [ ] Test `CalculateBaseline` with custom baseline func
  - [ ] Test `CalculateBaseline` without baseline func
  - [ ] Test `IsBaselineLayout` for row/column directions
- [ ] Create `FlexLineTests.cs`
  - [ ] Test `CalculateFlexLine` line breaking
  - [ ] Test auto margin counting
  - [ ] Test flex factor accumulation
- [ ] Create `AlignUtilsTests.cs`
  - [ ] Test `ResolveChildAlignment` with various align values
  - [ ] Test `FallbackAlignment` for Align enum
  - [ ] Test `FallbackAlignment` for Justify enum
- [ ] Create `BoundAxisTests.cs`
  - [ ] Test `PaddingAndBorderForAxis`
  - [ ] Test `BoundAxisWithinMinAndMax` with min/max constraints
  - [ ] Test `BoundAxisValue` padding floor
- [ ] Create `TrailingPositionTests.cs`
  - [ ] Test `GetPositionOfOppositeEdge`
  - [ ] Test `SetChildTrailingPosition`
  - [ ] Test `NeedsTrailingPosition` for reversed axes

## Acceptance Criteria

- [ ] All tests pass
- [ ] Each algorithm helper class has corresponding test file
- [ ] Edge cases covered (NaN, negative values, zero scale factor)

## Notes

Follow the existing test patterns from `CacheTests.cs`:
- Use Shouldly assertions
- Group tests by region
- Test both success and failure cases
