# Task 118 - CachedMeasurement

## Summary

Port the CachedMeasurement struct from C++ to C#. This caches measure function results to avoid redundant calls. This is a Level 5 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                            | Lines |
| ---------- | ------------------------------- | ----- |
| C++ Header | `yoga/node/CachedMeasurement.h` | ~80   |

## Target Files

| Type      | Path                                                 |
| --------- | ---------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/node/cached-measurement.cs` |

## Dependencies

- Task 104: Comparison utilities
- Task 115: SizingMode

## Todo List

- [ ] Port `CachedMeasurement.h` to C# struct
- [ ] Implement cache key matching
- [ ] Implement cache invalidation logic

## Acceptance Criteria

- [ ] Cache stores width/height constraints correctly
- [ ] Cache stores sizing modes correctly
- [ ] Cache matching logic working
- [ ] Used by Node layout caching (Task 122)

## Notes

```csharp
public struct CachedMeasurement
{
    public float AvailableWidth;
    public float AvailableHeight;
    public SizingMode WidthSizingMode;
    public SizingMode HeightSizingMode;
    public float ComputedWidth;
    public float ComputedHeight;

    public bool CanUse(
        SizingMode widthMode, float width,
        SizingMode heightMode, float height,
        float lastComputedWidth, float lastComputedHeight);
}
```
