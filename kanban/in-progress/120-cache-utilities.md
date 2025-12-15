# Task 120 - Cache Utilities

## Summary

Port the Cache utilities from C++ to C#. This handles layout caching logic. This is a Level 6 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                       | Lines |
| ---------- | -------------------------- | ----- |
| C++ Header | `yoga/algorithm/Cache.h`   | ~80   |
| C++ Source | `yoga/algorithm/Cache.cpp` | ~100  |

## Target Files

| Type      | Path                                         |
| --------- | -------------------------------------------- |
| C# Source | `source/timewarp-flexbox/algorithm/cache.cs` |

## Dependencies

- Task 114: Config
- Task 115: SizingMode

## Todo List

- [ ] Port `Cache.h/.cpp` to C#
- [ ] Implement cache lookup logic
- [ ] Implement cache storage logic

## Acceptance Criteria

- [ ] Cache lookup working
- [ ] Cache storage working
- [ ] Used by CalculateLayout (Task 125)

## Notes

```csharp
public static class Cache
{
    public static bool CanUseCachedMeasurement(
        SizingMode widthSizingMode,
        float availableWidth,
        SizingMode heightSizingMode,
        float availableHeight,
        SizingMode lastWidthSizingMode,
        float lastAvailableWidth,
        SizingMode lastHeightSizingMode,
        float lastAvailableHeight,
        float lastComputedWidth,
        float lastComputedHeight,
        float marginRow,
        float marginColumn,
        Config config);
}
```
