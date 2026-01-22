# Task 133 - Flex Line Calculation

## Summary

Port the flex line calculation function that determines where flex items should wrap and collects items into lines.

## Source Files

| Type       | Path                          | Lines |
| ---------- | ----------------------------- | ----- |
| C++ Header | `yoga/algorithm/FlexLine.h`   | ~76   |
| C++ Source | `yoga/algorithm/FlexLine.cpp` | ~125  |

## Functions to Port

### calculateFlexLine (lines 16-122 in FlexLine.cpp)
Calculates where a line starting at a given index should break, returning information about the collected children on the line.

```cpp
FlexLine calculateFlexLine(
    yoga::Node* node,
    Direction ownerDirection,
    float ownerWidth,
    float mainAxisOwnerSize,
    float availableInnerWidth,
    float availableInnerMainDim,
    Node::LayoutableChildren::Iterator& iterator,
    size_t lineCount);
```

Key responsibilities:
- Iterate children from the iterator position
- Skip display:none and absolute positioned children
- Track auto margins count
- Calculate size consumed by each item
- Track flex grow/shrink factors
- Break when line is full (in wrap mode)
- Update iterator to point to next line's start

## Target Files

| Type      | Path                                                  |
| --------- | ----------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Algorithm/FlexLine.cs`       |

## Todo List

- [x] Port `calculateFlexLine` function
  - [x] Handle iterator-based iteration
  - [x] Skip display:none children
  - [x] Skip absolute positioned children
  - [x] Track numberOfAutoMargins
  - [x] Calculate sizeConsumed
  - [x] Track totalFlexGrowFactors
  - [x] Track totalFlexShrinkScaledFactors
  - [x] Handle line breaking in wrap mode
  - [x] Floor flex factors to 1 when small
- [x] Handle C++ iterator semantics in C#
  - Used `LayoutableChildren<Node>.Enumerator` with ref parameter
  - Added `PendingChild` property to handle C#/C++ iterator difference
- [x] Add unit tests for FlexLine and FlexLineRunningLayout structs
- [x] Add comprehensive unit tests for CalculateFlexLine method
  - Note: C++ Yoga has no dedicated unit tests for `calculateFlexLine`
  - Tests are integration tests via `YGFlexWrapTest.cpp` (~1,960 lines)
  - These require the full layout algorithm and will be ported in Task 138

## Dependencies

- Task 125a: FlexLine, FlexLineRunningLayout structs
- Task 123: BoundAxis helpers
- Node.getLayoutChildren() iteration
- Node.style().flexWrap()

## Notes

### Iterator Pattern
The C++ version uses an iterator that's passed by reference and updated:
```cpp
for (; iterator != childrenEnd; iterator++) {
    // ... process child
    if (shouldBreak) break;
}
// iterator now points to start of next line
```

In C#, use an index-based approach or IEnumerator:
```csharp
public static FlexLine CalculateFlexLine(
    Node node,
    Direction ownerDirection,
    float ownerWidth,
    float mainAxisOwnerSize,
    float availableInnerWidth,
    float availableInnerMainDim,
    ref int childIndex,  // replaces iterator
    int lineCount)
```

### Line Breaking Logic
```cpp
if (sizeConsumedIncludingMinConstraint + flexBasisWithMinAndMaxConstraints +
            childMarginMainAxis + childLeadingGapMainAxis >
        availableInnerMainDim &&
    isNodeFlexWrap && !itemsInFlow.empty()) {
    break;  // Start new line
}
```

### Flex Factor Flooring
Small flex factors are floored to 1 to prevent division issues:
```cpp
if (totalFlexGrowFactors > 0 && totalFlexGrowFactors < 1) {
    totalFlexGrowFactors = 1;
}
if (totalFlexShrinkScaledFactors > 0 && totalFlexShrinkScaledFactors < 1) {
    totalFlexShrinkScaledFactors = 1;
}
```

### Child Line Index
Each child tracks which line it belongs to:
```cpp
child->setLineIndex(lineCount);
```

### Gap Handling
Leading gap is only applied after the first item:
```cpp
const float childLeadingGapMainAxis =
    child == firstElementInLine ? 0.0f : gap;
```

## Results

### Implementation Complete ✅

The `CalculateFlexLine` function has been fully ported to C# in `source/timewarp-flexbox/Algorithm/FlexLine.cs`.

### Test Strategy

**Unit Tests (Complete):**
- `FlexLineTests.cs` tests the `FlexLine` and `FlexLineRunningLayout` structs
- Tests cover equality, hash codes, default values, and property modification

**Integration Tests (Task 138):**
- C++ Yoga has **no dedicated unit tests** for `calculateFlexLine`
- All flex line behavior is tested through integration tests in `YGFlexWrapTest.cpp` (~1,960 lines)
- These tests call `YGNodeCalculateLayout` and verify final layout positions
- Will be ported as part of Task 138 after the full layout algorithm is complete

### Files
- Implementation: `source/timewarp-flexbox/Algorithm/FlexLine.cs`
- Unit Tests: `test/timewarp-flexbox-tests/Algorithm/FlexLineTests.cs`
