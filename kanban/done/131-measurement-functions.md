# Task 131 - Measurement Functions

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

- [x] Port `measureNodeWithMeasureFunc`
  - Handle measure callback invocation
  - Apply padding/border calculations
  - Track metrics in LayoutData
  - Emit events (MeasureCallbackStart/End)
- [x] Port `measureNodeWithoutChildren`
  - Return padding+border as minimum size
  - Apply boundAxis constraints
- [x] Port `measureNodeWithFixedSize`
  - Check if both dimensions are fixed
  - Set measured dimensions directly
  - Return success/failure flag
- [x] Add unit tests for each function
- [x] Test edge cases (undefined dimensions, zero sizes, etc.)

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

## Results

### Implementation Summary

Created `source/timewarp-flexbox/Algorithm/MeasureNode.cs` with three measurement functions:

1. **MeasureNodeWithMeasureFunc** - Measures nodes with custom measure functions (e.g., text nodes)
   - Calculates padding/border for both axes
   - Passes inner dimensions (available - padding/border) to the measure callback
   - For MaxContent sizing mode, passes undefined dimensions
   - Converts SizingMode to MeasureMode using existing extension methods
   - Publishes MeasureCallbackStart/End events
   - Adds padding/border back to measured result
   - Applies min/max bounds using BoundAxisValue
   - Tracks metrics in LayoutData

2. **MeasureNodeWithoutChildren** - Measures leaf nodes without children
   - For StretchFit: uses available dimension
   - For MaxContent/FitContent: uses padding+border as minimum size
   - Applies min/max bounds

3. **MeasureNodeWithFixedSize** - Fast path optimization
   - Uses existing `LayoutHelpers.IsFixedSize()` to check if both dimensions are fixed
   - Returns true if measurement was handled, false to indicate regular measurement needed
   - Only sets measured dimensions when returning true

### Test Coverage

Created `test/timewarp-flexbox-tests/Algorithm/MeasureNodeTests.cs` with 31 test methods covering:

- Basic functionality of all three measurement functions
- Measure callback invocation and tracking
- Padding/border calculations
- Min/max bounds application
- SizingMode to MeasureMode conversion
- RTL direction handling
- Edge cases (zero dimensions, negative dimensions, undefined dimensions)
- Null parameter validation

### All tests pass: 667 passed, 3 skipped (mock-related), 0 failed
