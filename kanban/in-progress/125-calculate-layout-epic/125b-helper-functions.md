# Task 125b - CalculateLayout Helper Functions

## Summary

Port the small utility functions used by the main layout algorithm. These are leaf-level functions with minimal dependencies.

## Source Files

| Type       | Path                                 | Lines   |
| ---------- | ------------------------------------ | ------- |
| C++ Source | `yoga/algorithm/CalculateLayout.cpp` | Various |

## Functions to Port

### constrainMaxSizeForMode (lines 38-64)
Applies max-size constraints to sizing mode and dimension. Modifies both mode and size by reference.

```cpp
static void constrainMaxSizeForMode(
    const yoga::Node* node,
    Direction direction,
    FlexDirection axis,
    float ownerAxisSize,
    float ownerWidth,
    /*in_out*/ SizingMode* mode,
    /*in_out*/ float* size);
```

### isFixedSize (lines 421-425)
Checks if a dimension is already fixed (no measurement needed).

```cpp
inline bool isFixedSize(float dim, SizingMode sizingMode);
```

### calculateAvailableInnerDimension (lines 501-534)
Computes available inner dimension respecting min/max constraints.

```cpp
static float calculateAvailableInnerDimension(
    const yoga::Node* node,
    const Direction direction,
    const Dimension dimension,
    const float availableDim,
    const float paddingAndBorder,
    const float ownerDim,
    const float ownerWidth);
```

### zeroOutLayoutRecursively (lines 471-481)
Resets layout for display:none nodes.

```cpp
static void zeroOutLayoutRecursively(yoga::Node* node);
```

### cleanupContentsNodesRecursively (lines 483-499)
Handles display:contents nodes (invisible containers).

```cpp
static void cleanupContentsNodesRecursively(yoga::Node* node);
```

## Target Files

| Type      | Path                                                     |
| --------- | -------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Algorithm/LayoutHelpers.cs`     |

## Todo List

- [ ] Port `constrainMaxSizeForMode` - must handle in/out parameters (ref in C#)
- [ ] Port `isFixedSize` - simple boolean check
- [ ] Port `calculateAvailableInnerDimension` - applies min/max to available space
- [ ] Port `zeroOutLayoutRecursively` - recursive reset for display:none
- [ ] Port `cleanupContentsNodesRecursively` - handle display:contents
- [ ] Add unit tests

## Dependencies

- Task 123: BoundAxis helpers (already ported)
- Node.Style access methods
- FloatOptional

## Notes

### C# out/ref parameters
`constrainMaxSizeForMode` modifies both mode and size by reference:

```csharp
internal static void ConstrainMaxSizeForMode(
    Node node,
    Direction direction,
    FlexDirection axis,
    float ownerAxisSize,
    float ownerWidth,
    ref SizingMode mode,
    ref float size)
```

### isFixedSize Logic
```csharp
internal static bool IsFixedSize(float dim, SizingMode sizingMode) =>
    sizingMode == SizingMode.StretchFit ||
    (Yoga.IsDefined(dim) && sizingMode == SizingMode.FitContent && dim <= 0.0f);
```

### display:contents handling
Nodes with `display: contents` are special - their children participate in layout as if they were direct children of the grandparent, but the contents node itself gets zero layout. This requires special handling in `cleanupContentsNodesRecursively`.
