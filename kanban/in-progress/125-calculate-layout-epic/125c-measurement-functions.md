# Task 125c - Measurement Functions

## Summary

Port the node measurement functions that handle special cases: nodes with measure functions, nodes without children, and nodes with fixed sizes.

## Source Files

| Type       | Path                                 | Lines     |
| ---------- | ------------------------------------ | --------- |
| C++ Source | `yoga/algorithm/CalculateLayout.cpp` | 266-469   |

## Functions to Port

### measureNodeWithMeasureFunc (lines 266-376)
Handles nodes that have a custom measure function (e.g., text nodes). This is the most complex measurement function.

```cpp
static void measureNodeWithMeasureFunc(
    yoga::Node* node,
    Direction direction,
    float availableWidth,
    float availableHeight,
    SizingMode widthSizingMode,
    SizingMode heightSizingMode,
    float ownerWidth,
    float ownerHeight,
    LayoutData& layoutMarkerData,
    LayoutPassReason reason);
```

Key responsibilities:
- Convert MaxContent sizing to undefined dimensions
- Calculate inner dimensions (subtract padding/border)
- Call the measure function callback
- Apply bounds to the measured result
- Track measure callback metrics

### measureNodeWithoutChildren (lines 380-419)
Handles leaf nodes with no children - uses padding/border as minimum size.

```cpp
static void measureNodeWithoutChildren(
    yoga::Node* node,
    Direction direction,
    float availableWidth,
    float availableHeight,
    SizingMode widthSizingMode,
    SizingMode heightSizingMode,
    float ownerWidth,
    float ownerHeight);
```

### measureNodeWithFixedSize (lines 427-469)
Fast path optimization when both dimensions are already determined.

```cpp
static bool measureNodeWithFixedSize(
    yoga::Node* node,
    Direction direction,
    float availableWidth,
    float availableHeight,
    SizingMode widthSizingMode,
    SizingMode heightSizingMode,
    float ownerWidth,
    float ownerHeight);
```

Returns `true` if measurement was handled (both dimensions fixed), `false` otherwise.

## Target Files

| Type      | Path                                                     |
| --------- | -------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Algorithm/MeasureNode.cs`       |

## Todo List

- [ ] Port `measureNodeWithMeasureFunc`
  - Handle measure callback invocation
  - Apply padding/border calculations
  - Track metrics in LayoutData
  - Emit events (MeasureCallbackStart/End)
- [ ] Port `measureNodeWithoutChildren`
  - Return padding+border as minimum size
  - Apply boundAxis constraints
- [ ] Port `measureNodeWithFixedSize`
  - Check if both dimensions are fixed
  - Set measured dimensions directly
  - Return success/failure flag
- [ ] Add unit tests for each function
- [ ] Test edge cases (undefined dimensions, zero sizes, etc.)

## Dependencies

- Task 123: BoundAxis helpers
- Task 125a: LayoutData struct
- Task 125b: Helper functions
- Node.hasMeasureFunc() and Node.measure()
- Event system for measure callbacks

## Notes

### Measure Function Callback
When a node has a measure function, it's called with inner dimensions:
```csharp
var measuredSize = node.Measure(
    innerWidth,
    MeasureMode(widthSizingMode),
    innerHeight,
    MeasureMode(heightSizingMode));
```

The result is then bounded by min/max constraints.

### Sizing Mode to MeasureMode Conversion
- `SizingMode.StretchFit` -> `MeasureMode.Exactly`
- `SizingMode.MaxContent` -> `MeasureMode.Undefined`
- `SizingMode.FitContent` -> `MeasureMode.AtMost`

### Fixed Size Check
```csharp
static bool IsFixedSize(float dim, SizingMode sizingMode) =>
    sizingMode == SizingMode.StretchFit ||
    (Yoga.IsDefined(dim) && sizingMode == SizingMode.FitContent && dim <= 0.0f);
```
