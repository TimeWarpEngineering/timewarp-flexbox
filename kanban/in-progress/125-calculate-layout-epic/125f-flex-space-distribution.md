# Task 125f - Flex Space Distribution

## Summary

Port the flex space distribution functions that implement flex-grow and flex-shrink. This is the core of the flexbox sizing algorithm - a two-pass approach that handles min/max constraints.

## Source Files

| Type       | Path                                 | Lines     |
| ---------- | ------------------------------------ | --------- |
| C++ Source | `yoga/algorithm/CalculateLayout.cpp` | 622-980   |

## Functions to Port

### distributeFreeSpaceFirstPass (lines 821-906)
First pass: Detect flex items whose min/max constraints trigger and freeze them at those sizes.

```cpp
static void distributeFreeSpaceFirstPass(
    FlexLine& flexLine,
    const Direction direction,
    const FlexDirection mainAxis,
    const float ownerWidth,
    const float mainAxisOwnerSize,
    const float availableInnerMainDim,
    const float availableInnerWidth);
```

Key responsibilities:
- Identify items that hit min/max bounds
- Exclude their size from remaining space
- Adjust totalFlexGrowFactors/totalFlexShrinkScaledFactors

### distributeFreeSpaceSecondPass (lines 622-816)
Second pass: Distribute remaining space to flexible items and recursively layout.

```cpp
static float distributeFreeSpaceSecondPass(
    FlexLine& flexLine,
    yoga::Node* node,
    const FlexDirection mainAxis,
    const FlexDirection crossAxis,
    const Direction direction,
    const float ownerWidth,
    const float mainAxisOwnerSize,
    const float availableInnerMainDim,
    const float availableInnerCrossDim,
    const float availableInnerWidth,
    const float availableInnerHeight,
    const bool mainAxisOverflows,
    const SizingMode sizingModeCrossDim,
    const bool performLayout,
    LayoutData& layoutMarkerData,
    const uint32_t depth,
    const uint32_t generationCount);
```

Key responsibilities:
- Calculate final size for each flexible item
- Handle aspect ratio
- Handle cross-axis stretch
- Recursively call calculateLayoutInternal for each child

### resolveFlexibleLength (lines 930-980)
Orchestrates the two-pass flex resolution.

```cpp
static void resolveFlexibleLength(
    yoga::Node* node,
    FlexLine& flexLine,
    const FlexDirection mainAxis,
    const FlexDirection crossAxis,
    const Direction direction,
    const float ownerWidth,
    const float mainAxisOwnerSize,
    const float availableInnerMainDim,
    const float availableInnerCrossDim,
    const float availableInnerWidth,
    const float availableInnerHeight,
    const bool mainAxisOverflows,
    const SizingMode sizingModeCrossDim,
    const bool performLayout,
    LayoutData& layoutMarkerData,
    const uint32_t depth,
    const uint32_t generationCount);
```

## Target Files

| Type      | Path                                                        |
| --------- | ----------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Algorithm/FlexDistribution.cs`     |

## Todo List

- [ ] Port `distributeFreeSpaceFirstPass`
  - Handle shrink case (remainingFreeSpace < 0)
  - Handle grow case (remainingFreeSpace > 0)
  - Identify items that hit min/max bounds
  - Update totalFlexGrowFactors/totalFlexShrinkScaledFactors
- [ ] Port `distributeFreeSpaceSecondPass`
  - Calculate updatedMainSize for each item
  - Handle aspect ratio for cross size
  - Handle stretch alignment
  - Apply constrainMaxSizeForMode
  - Make recursive calculateLayoutInternal calls
  - Track deltaFreeSpace
- [ ] Port `resolveFlexibleLength`
  - Call first pass
  - Call second pass
  - Update remainingFreeSpace
- [ ] Add unit tests for flex grow
- [ ] Add unit tests for flex shrink
- [ ] Test min/max constraint handling

## Dependencies

- Task 125a: FlexLine struct
- Task 125b: constrainMaxSizeForMode
- Task 123: BoundAxis helpers
- calculateLayoutInternal (forward reference)

## Notes

### Two-Pass Algorithm
This is a **simplification** of the W3C spec algorithm. The spec describes a process that repeats until no items violate constraints. Yoga uses exactly two passes for performance:

1. **First Pass**: Find clamped items, exclude them from flex factor totals
2. **Second Pass**: Distribute space using adjusted totals

### Flex Shrink Scaling
Shrink factor is scaled by the item's flex basis:
```cpp
flexShrinkScaledFactor = -child->resolveFlexShrink() * childFlexBasis;
```

### Flex Grow Formula
```cpp
updatedMainSize = childFlexBasis +
    (remainingFreeSpace / totalFlexGrowFactors) * flexGrowFactor;
```

### Flex Shrink Formula
```cpp
childSize = childFlexBasis +
    (remainingFreeSpace / totalFlexShrinkScaledFactors) * flexShrinkScaledFactor;
```

### Cross-Axis Handling in Second Pass
The second pass also determines cross-axis sizing:
- If aspect ratio defined: derive from main size
- If stretch + no definite cross size: use available cross dim
- Otherwise: measure or use definite size

### Recursive Layout
Each child gets laid out recursively:
```cpp
calculateLayoutInternal(
    currentLineChild,
    childWidth,
    childHeight,
    node->getLayout().direction(),
    childWidthSizingMode,
    childHeightSizingMode,
    availableInnerWidth,
    availableInnerHeight,
    isLayoutPass,
    isLayoutPass ? LayoutPassReason::kFlexLayout : LayoutPassReason::kFlexMeasure,
    layoutMarkerData,
    depth,
    generationCount);
```
