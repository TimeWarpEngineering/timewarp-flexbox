# Task 132 - Flex Basis Calculation

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

- [x] Port `computeFlexBasisForChild`
  - [x] Handle explicit flex-basis
  - [x] Handle definite width/height
  - [x] Handle measurement fallback
  - [x] Handle aspect ratio
  - [x] Handle stretch alignment
  - [x] Apply constrainMaxSizeForMode
  - [x] Recursive calculateLayoutInternal call for measurement (via delegate pattern)
- [x] Port `computeFlexBasisForChildren`
  - [x] Single flex child optimization
  - [x] Skip display:none and absolute children
  - [x] Set initial positions (via ProcessDimensions)
  - [x] Accumulate totalOuterFlexBasis
- [x] Add unit tests (25 tests)
- [x] Test edge cases (aspect ratio, stretch, auto margins, RTL)

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

## Implementation Notes

### Files Created
- `source/timewarp-flexbox/Algorithm/FlexBasis.cs` - Main implementation
- `test/timewarp-flexbox-tests/Algorithm/FlexBasisTests.cs` - 25 unit tests

### Delegate Pattern for calculateLayoutInternal
To avoid circular dependencies (FlexBasis needs calculateLayoutInternal, but calculateLayoutInternal 
needs FlexBasis), we use a delegate pattern:

```csharp
public delegate bool CalculateLayoutInternalFunc(
    Node node,
    float availableWidth,
    float availableHeight,
    Direction ownerDirection,
    SizingMode widthSizingMode,
    SizingMode heightSizingMode,
    float ownerWidth,
    float ownerHeight,
    bool performLayout,
    LayoutPassReason reason,
    LayoutData layoutMarkerData,
    int depth,
    uint generationCount);

public static class FlexBasis
{
    public static CalculateLayoutInternalFunc? CalculateLayoutInternal { get; set; }
    // ...
}
```

The main layout algorithm will set this delegate before calling flex basis calculations.

### Test Coverage
- Null argument validation
- Explicit flex-basis usage
- Definite width/height for row/column directions
- Auto flex-basis measurement
- Generation count tracking
- Percentage flex-basis
- Padding and border minimum constraints
- Total outer flex basis calculation
- Display:none child handling
- Absolute positioned child handling
- Single flex child optimization
- Multiple flex children (no optimization)
- Margin inclusion
- Column direction
- RTL direction
- Aspect ratio handling
