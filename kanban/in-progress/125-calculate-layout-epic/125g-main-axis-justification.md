# Task 125g - Main Axis Justification

## Summary

Port the justifyMainAxis function that positions flex items along the main axis based on justify-content and handles baseline alignment calculations.

## Source Files

| Type       | Path                                 | Lines     |
| ---------- | ------------------------------------ | --------- |
| C++ Source | `yoga/algorithm/CalculateLayout.cpp` | 982-1156  |

## Function to Port

### justifyMainAxis (lines 982-1156)
Positions items along the main axis according to justify-content, handles auto margins, and calculates cross dimension based on item sizes.

```cpp
static void justifyMainAxis(
    yoga::Node* node,
    FlexLine& flexLine,
    const FlexDirection mainAxis,
    const FlexDirection crossAxis,
    const Direction direction,
    const SizingMode sizingModeMainDim,
    const SizingMode sizingModeCrossDim,
    const float mainAxisOwnerSize,
    const float ownerWidth,
    const float availableInnerMainDim,
    const float availableInnerCrossDim,
    const float availableInnerWidth,
    const bool performLayout);
```

## Target Files

| Type      | Path                                                        |
| --------- | ----------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Algorithm/JustifyContent.cs`       |

## Todo List

- [ ] Port `justifyMainAxis` function
  - Handle min-dimension constraint on remaining free space
  - Calculate leadingMainDim and betweenMainDim based on justify-content
  - Handle auto margins (flexStartMarginIsAuto, flexEndMarginIsAuto)
  - Position each item along main axis
  - Calculate crossDim based on baseline or max child size
- [ ] Implement justify-content variants:
  - FlexStart (default)
  - FlexEnd
  - Center
  - SpaceBetween
  - SpaceAround
  - SpaceEvenly
- [ ] Handle baseline alignment for cross dimension
- [ ] Add unit tests for each justify-content value
- [ ] Test auto margin distribution

## Dependencies

- Task 125a: FlexLine struct
- Task 123: calculateBaseline helper
- Node.style().justifyContent()
- isBaselineLayout() helper

## Notes

### Justify Content Mapping
```cpp
switch (justifyContent) {
    case Justify::Center:
        leadingMainDim = flexLine.layout.remainingFreeSpace / 2;
        break;
    case Justify::FlexEnd:
        leadingMainDim = flexLine.layout.remainingFreeSpace;
        break;
    case Justify::SpaceBetween:
        if (flexLine.itemsInFlow.size() > 1) {
            betweenMainDim += remainingFreeSpace / (items.size() - 1);
        }
        break;
    case Justify::SpaceEvenly:
        leadingMainDim = remainingFreeSpace / (items.size() + 1);
        betweenMainDim += leadingMainDim;
        break;
    case Justify::SpaceAround:
        leadingMainDim = 0.5f * remainingFreeSpace / items.size();
        betweenMainDim += leadingMainDim * 2;
        break;
    case Justify::FlexStart:
        break;  // No adjustment needed
}
```

### Auto Margin Handling
Auto margins absorb remaining free space:
```cpp
if (child->style().flexStartMarginIsAuto(mainAxis, direction) &&
    flexLine.layout.remainingFreeSpace > 0.0f) {
    flexLine.layout.mainDim += remainingFreeSpace / numberOfAutoMargins;
}
```

### Overflow Fallback
When there's negative free space, some justify values fall back:
```cpp
const Justify justifyContent = flexLine.layout.remainingFreeSpace >= 0
    ? node->style().justifyContent()
    : fallbackAlignment(node->style().justifyContent());
```

### Baseline Cross Dimension
For baseline-aligned containers, the cross dimension is calculated from max ascent + max descent:
```cpp
if (isNodeBaselineLayout) {
    const float ascent = calculateBaseline(child) + startMargin;
    const float descent = measuredHeight + margin - ascent;
    maxAscentForCurrentLine = max(maxAscentForCurrentLine, ascent);
    maxDescentForCurrentLine = max(maxDescentForCurrentLine, descent);
}
flexLine.layout.crossDim = maxAscentForCurrentLine + maxDescentForCurrentLine;
```

### Gap Handling
Gap is added to betweenMainDim and is not applied to the first item:
```cpp
const float gap = node->style().computeGapForAxis(mainAxis, availableInnerMainDim);
float betweenMainDim = gap;
```
