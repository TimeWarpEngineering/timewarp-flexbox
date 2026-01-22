# Task 108 - StyleLength Types

## Summary

Port the StyleLength and StyleSizeLength types from C++ to C#. These represent CSS-like length values with units. This is a Level 3 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                           | Lines |
| ---------- | ------------------------------ | ----- |
| C++ Header | `yoga/style/StyleLength.h`     | 121   |
| C++ Header | `yoga/style/StyleSizeLength.h` | 146   |

## Target Files

| Type      | Path                                                 |
| --------- | ---------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/style/style-length.cs`      |
| C# Source | `source/timewarp-flexbox/style/style-size-length.cs` |

## Dependencies

- Task 105: YGEnums (for Unit enum)
- Task 106: FloatOptional

## Todo List

- [ ] Port `StyleLength.h` to C# readonly struct
- [ ] Port `StyleSizeLength.h` to C# readonly struct
- [ ] Implement factory methods (points, percent, auto)
- [ ] Implement resolve methods

## Acceptance Criteria

- [ ] StyleLength supports point, percent, auto, undefined
- [ ] StyleSizeLength supports point, percent, auto, maxContent, fitContent, stretch
- [ ] Resolution of percent values working
- [ ] Used correctly by Style (Task 121)

## Notes

Key C++ constructs to convert:

- `CompactValue` encoding -> Consider bitfield struct or union simulation
- Factory methods for each unit type
- `resolve()` method for percentage resolution

```csharp
public readonly struct StyleLength : IEquatable<StyleLength>
{
    public static StyleLength Points(float value) => ...
    public static StyleLength Percent(float value) => ...
    public static StyleLength Auto => ...
    public static StyleLength Undefined => ...

    public FloatOptional Resolve(float referenceLength) => ...
}
```
