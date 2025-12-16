# Task 125e - Flex Line Calculation

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

- [ ] Port `calculateFlexLine` function
  - Handle iterator-based iteration
  - Skip display:none children
  - Skip absolute positioned children
  - Track numberOfAutoMargins
  - Calculate sizeConsumed
  - Track totalFlexGrowFactors
  - Track totalFlexShrinkScaledFactors
  - Handle line breaking in wrap mode
  - Floor flex factors to 1 when small
- [ ] Handle C++ iterator semantics in C#
- [ ] Add unit tests for line breaking
- [ ] Test edge cases (single item, all absolute, no wrap)

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
