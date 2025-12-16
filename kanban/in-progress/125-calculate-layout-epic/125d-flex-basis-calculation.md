# Task 125d - Flex Basis Calculation

## Summary

Port the flex basis calculation functions. These determine the initial size of flex items before flex grow/shrink is applied.

## Source Files

| Type       | Path                                 | Lines     |
| ---------- | ------------------------------------ | --------- |
| C++ Source | `yoga/algorithm/CalculateLayout.cpp` | 66-264, 536-616 |

## Functions to Port

### computeFlexBasisForChild (lines 66-264)
Computes the flex basis for a single child. This is a complex function (~200 lines) that handles:
- Explicit flex-basis values
- Definite width/height as flex-basis
- Measurement when flex-basis is auto
- Aspect ratio considerations
- Stretch alignment impact

```cpp
static void computeFlexBasisForChild(
    const yoga::Node* node,
    yoga::Node* child,
    float width,
    SizingMode widthMode,
    float height,
    float ownerWidth,
    float ownerHeight,
    SizingMode heightMode,
    Direction direction,
    LayoutData& layoutMarkerData,
    uint32_t depth,
    uint32_t generationCount);
```

### computeFlexBasisForChildren (lines 536-616)
Iterates all children and computes their flex basis. Also handles:
- Single flex child optimization
- display:none children (zero layout)
- Absolute positioned children (skip)
- Initial position setting

```cpp
static float computeFlexBasisForChildren(
    yoga::Node* node,
    float availableInnerWidth,
    float availableInnerHeight,
    SizingMode widthSizingMode,
    SizingMode heightSizingMode,
    Direction direction,
    FlexDirection mainAxis,
    bool performLayout,
    LayoutData& layoutMarkerData,
    uint32_t depth,
    uint32_t generationCount);
```

Returns `totalOuterFlexBasis` - sum of all children's flex basis + margins.

## Target Files

| Type      | Path                                                     |
| --------- | -------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Algorithm/FlexBasis.cs`         |

## Todo List

- [ ] Port `computeFlexBasisForChild`
  - Handle explicit flex-basis
  - Handle definite width/height
  - Handle measurement fallback
  - Handle aspect ratio
  - Handle stretch alignment
  - Apply constrainMaxSizeForMode
  - Recursive calculateLayoutInternal call for measurement
- [ ] Port `computeFlexBasisForChildren`
  - Single flex child optimization
  - Skip display:none and absolute children
  - Set initial positions
  - Accumulate totalOuterFlexBasis
- [ ] Add unit tests
- [ ] Test edge cases (aspect ratio, stretch, auto margins)

## Dependencies

- Task 123: BoundAxis helpers
- Task 125a: LayoutData struct
- Task 125b: constrainMaxSizeForMode
- Task 125c: Measurement functions
- Node.resolveFlexBasis()
- calculateLayoutInternal (forward declaration/partial)

## Notes

### Flex Basis Resolution Priority
1. Explicit flex-basis if defined and main axis size is defined
2. Definite dimension in main axis direction
3. Measure the content (may recurse into calculateLayoutInternal)

### Single Flex Child Optimization
When there's only one flexible child with flexGrow AND flexShrink:
- Set its computedFlexBasis to 0
- Skip measurement entirely
- The child will grow/shrink to fill available space

```cpp
if (sizingModeMainDim == SizingMode::StretchFit) {
    for (auto child : children) {
        if (child->isNodeFlexible()) {
            if (singleFlexChild != nullptr || ...) {
                singleFlexChild = nullptr;
                break;
            } else {
                singleFlexChild = child;
            }
        }
    }
}
```

### Aspect Ratio Handling
When aspect ratio is defined, the cross-axis size affects the main-axis size:
```cpp
if (childStyle.aspectRatio().isDefined()) {
    if (!isMainAxisRow && childWidthSizingMode == SizingMode::StretchFit) {
        childHeight = marginColumn +
            (childWidth - marginRow) / childStyle.aspectRatio().unwrap();
    }
    // ... similar for row
}
```

### Generation Count
The `generationCount` is used to track cache validity:
```cpp
if (child->getLayout().computedFlexBasisGeneration != generationCount) {
    // Need to recompute
}
child->setLayoutComputedFlexBasisGeneration(generationCount);
```
