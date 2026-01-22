# Task 121 - Style

## Summary

Port the Style class from C++ to C#. This is the largest style file containing all node style properties. This is a Level 6 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                  | Lines |
| ---------- | --------------------- | ----- |
| C++ Header | `yoga/style/Style.h`  | 759   |
| C++ Test   | `tests/StyleTest.cpp` | ~45   |

## Target Files

| Type      | Path                                               |
| --------- | -------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Style/Style.cs`           |
| C# Test   | `test/timewarp-flexbox-tests/Style/StyleTests.cs`  |

## Dependencies

- Task 106: FloatOptional
- Task 108: StyleLength, StyleSizeLength
- Task 109: All enums
- Task 116: StyleValuePool
- Task 117: FlexDirection utilities

## Todo List

- [x] Port `Style.h` to C#
- [x] Implement all style properties
- [x] Implement style getters/setters
- [x] Port `StyleTest.cpp` tests
- [x] Ensure all tests pass

## Acceptance Criteria

- [x] All C++ test logic ported (4 tests from StyleTest.cpp)
- [x] All flex properties working (direction, wrap, grow, shrink, basis)
- [x] All alignment properties working (justify, align items/self/content)
- [x] All dimension properties working (width, height, min/max)
- [x] All spacing properties working (margin, padding, border)
- [x] All position properties working (position type, insets)
- [x] All tests pass with identical behavior to C++

## Notes

Implemented ~819 lines in Style.cs porting all functionality from C++ Style.h:

### Enum Properties
- Direction, FlexDirection, JustifyContent, AlignContent, AlignItems, AlignSelf
- PositionType, FlexWrap, Overflow, Display, BoxSizing

### Flex Properties
- Flex, FlexGrow, FlexShrink, FlexBasis (using StyleValuePool)

### Edge Properties
- Margin, Position, Padding, Border (per Edge)
- Gap (per Gutter)

### Dimension Properties
- Dimensions (Width, Height)
- MinDimensions, MaxDimensions
- ResolvedMinDimension/ResolvedMaxDimension (accounting for BoxSizing)
- AspectRatio (with validation for zero/infinity)

### Compute Methods
- ComputeFlexStart/End Position/Margin/Border/Padding
- ComputeInlineStart/End Position/Margin/Border/Padding
- ComputePaddingAndBorder combinations
- ComputeGapForAxis, ComputeMarginForAxis, ComputeBorderForAxis

### Query Methods
- HorizontalInsetsDefined, VerticalInsetsDefined
- IsFlexStart/End PositionDefined/Auto
- IsInlineStart/End PositionDefined/Auto
- FlexStart/End MarginIsAuto

### Equality
- Full IEquatable<Style> implementation comparing all properties

## Results

- Style.cs: 821 lines
- StyleTests.cs: 26 tests (4 from C++ + 22 additional coverage tests)
- All 454 tests pass (was 418 before)
