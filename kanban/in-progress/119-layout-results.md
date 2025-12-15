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
| C# Source | `source/timewarp-flexbox/node/layout-results.cs` |

## Dependencies

- Task 106: FloatOptional
- Task 109: Dimension, Direction, Edge, PhysicalEdge enums
- Task 112: AssertFatal
- Task 118: CachedMeasurement

## Todo List

- [ ] Port `LayoutResults.h/.cpp` to C#
- [ ] Implement position/dimension storage
- [ ] Implement margin/border/padding storage
- [ ] Implement cached measurement array

## Acceptance Criteria

- [ ] Position (left, top) storage working
- [ ] Dimensions (width, height) storage working
- [ ] Margin values storage working
- [ ] Border values storage working
- [ ] Padding values storage working
- [ ] Cached measurements working
- [ ] Used by Node (Task 122)

## Notes

```csharp
public sealed class LayoutResults
{
    // Computed layout
    public float Left { get; set; }
    public float Top { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    // Computed box model
    private readonly float[] _margin = new float[4];
    private readonly float[] _border = new float[4];
    private readonly float[] _padding = new float[4];

    public float GetMargin(PhysicalEdge edge);
    public float GetBorder(PhysicalEdge edge);
    public float GetPadding(PhysicalEdge edge);

    // Measure cache
    private readonly CachedMeasurement[] _measureCache = new CachedMeasurement[16];
}
```
