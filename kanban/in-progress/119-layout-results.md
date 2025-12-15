# Task 119 - LayoutResults

## Summary

Port the LayoutResults struct from C++ to C#. This stores the computed layout output for a node. This is a Level 6 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                          | Lines |
| ---------- | ----------------------------- | ----- |
| C++ Header | `yoga/node/LayoutResults.h`   | ~200  |
| C++ Source | `yoga/node/LayoutResults.cpp` | ~100  |

## Target Files

| Type      | Path                                             |
| --------- | ------------------------------------------------ |
| C# Source | `source/timewarp-flexbox/Node/LayoutResults.cs`  |
| C# Tests  | `test/timewarp-flexbox-tests/Node/LayoutResultsTests.cs` |

## Dependencies

- Task 106: FloatOptional
- Task 109: Dimension, Direction, Edge, PhysicalEdge enums
- Task 112: AssertFatal
- Task 118: CachedMeasurement

## Todo List

- [x] Port `LayoutResults.h/.cpp` to C#
- [x] Implement position/dimension storage
- [x] Implement margin/border/padding storage
- [x] Implement cached measurement array

## Acceptance Criteria

- [x] Position (left, top, right, bottom) storage working
- [x] Dimensions (width, height) storage working
- [x] MeasuredDimensions storage working
- [x] RawDimensions storage working
- [x] Margin values storage working
- [x] Border values storage working
- [x] Padding values storage working
- [x] Cached measurements working (8 entries)
- [x] CachedLayout working
- [x] Generation count and config version for caching
- [x] Direction and HadOverflow flags
- [x] ComputedFlexBasis storage
- [x] Equality comparison with inexact float matching
- [ ] Used by Node (Task 122)

## Implementation Notes

The implementation follows the C++ Yoga LayoutResults closely:

```csharp
public sealed class LayoutResults : IEquatable<LayoutResults>
{
    public const int MaxCachedMeasurements = 8;

    // Cache fields
    public uint ComputedFlexBasisGeneration { get; set; }
    public FloatOptional ComputedFlexBasis { get; set; } = FloatOptional.Undefined;
    public uint GenerationCount { get; set; }
    public uint ConfigVersion { get; set; }
    public Direction LastOwnerDirection { get; set; } = Direction.Inherit;
    public uint NextCachedMeasurementsIndex { get; set; }
    public CachedMeasurement CachedLayout { get; set; }

    // Computed layout
    public float GetPosition(PhysicalEdge edge);
    public float GetDimension(Dimension axis);
    public float GetMeasuredDimension(Dimension axis);
    public float GetRawDimension(Dimension axis);

    // Computed box model
    public float GetMargin(PhysicalEdge edge);
    public float GetBorder(PhysicalEdge edge);
    public float GetPadding(PhysicalEdge edge);

    // Cached measurements (8 entries as per C++ empirical data)
    public CachedMeasurement GetCachedMeasurement(int index);
    public void SetCachedMeasurement(int index, CachedMeasurement measurement);
}
```

Key differences from the original task notes:
- Used sealed class instead of struct for reference semantics (matches C++ where LayoutResults is passed by reference)
- Dimensions are stored using Dimension enum (Width, Height) not as individual Left/Top/Width/Height properties
- Position uses PhysicalEdge enum (Left, Top, Right, Bottom)
- MaxCachedMeasurements is 8, not 16 (based on C++ empirical data: 98% of layouts need < 8)
- Implemented full equality comparison with inexact float matching
