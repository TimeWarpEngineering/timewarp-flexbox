# Task 115 - SizingMode

## Summary

Port the SizingMode enum and utilities from C++ to C#. This represents how sizing constraints are applied. This is a Level 4 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                          | Lines |
| ---------- | ----------------------------- | ----- |
| C++ Header | `yoga/algorithm/SizingMode.h` | ~50   |

## Target Files

| Type      | Path                                               |
| --------- | -------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/algorithm/sizing-mode.cs` |

## Dependencies

- Task 109: MeasureMode enum
- Task 112: AssertFatal

## Todo List

- [ ] Port `SizingMode.h` to C#
- [ ] Implement conversion from MeasureMode
- [ ] Implement sizing constraint utilities

## Acceptance Criteria

- [ ] SizingMode enum defined correctly
- [ ] MeasureMode to SizingMode conversion working
- [ ] Used by CachedMeasurement (Task 118)

## Notes

```csharp
public enum SizingMode
{
    StretchFit,
    FitContent,
    MaxContent
}

public static class SizingModeExtensions
{
    public static SizingMode FromMeasureMode(MeasureMode mode) => mode switch
    {
        MeasureMode.Exactly => SizingMode.StretchFit,
        MeasureMode.AtMost => SizingMode.FitContent,
        MeasureMode.Undefined => SizingMode.MaxContent,
        _ => throw new ArgumentOutOfRangeException(nameof(mode))
    };
}
```
