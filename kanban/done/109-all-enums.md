# Task 109 - All Yoga Enums

## Summary

Port all individual Yoga enum types from C++ to C#. This is a Level 2 task that ports the 17 enum files.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                               | Lines   |
| ---------- | ---------------------------------- | ------- |
| C++ Header | `yoga/enums/Align.h`               | 44      |
| C++ Header | `yoga/enums/BoxSizing.h`           | 40      |
| C++ Header | `yoga/enums/Dimension.h`           | 40      |
| C++ Header | `yoga/enums/Direction.h`           | 41      |
| C++ Header | `yoga/enums/Display.h`             | 41      |
| C++ Header | `yoga/enums/Edge.h`                | 47      |
| C++ Header | `yoga/enums/Errata.h`              | 41      |
| C++ Header | `yoga/enums/ExperimentalFeature.h` | 39      |
| C++ Header | `yoga/enums/FlexDirection.h`       | 42      |
| C++ Header | `yoga/enums/Gutter.h`              | 41      |
| C++ Header | `yoga/enums/Justify.h`             | 44      |
| C++ Header | `yoga/enums/LogLevel.h`            | 44      |
| C++ Header | `yoga/enums/MeasureMode.h`         | 41      |
| C++ Header | `yoga/enums/NodeType.h`            | 40      |
| C++ Header | `yoga/enums/Overflow.h`            | 41      |
| C++ Header | `yoga/enums/PhysicalEdge.h`        | 21      |
| C++ Header | `yoga/enums/PositionType.h`        | 41      |
| C++ Header | `yoga/enums/Unit.h`                | 45      |
| C++ Header | `yoga/enums/Wrap.h`                | 41      |
| **Total**  |                                    | **862** |

## Target Files

| Type      | Path                                                    |
| --------- | ------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/enums/align.cs`                |
| C# Source | `source/timewarp-flexbox/enums/box-sizing.cs`           |
| C# Source | `source/timewarp-flexbox/enums/dimension.cs`            |
| C# Source | `source/timewarp-flexbox/enums/direction.cs`            |
| C# Source | `source/timewarp-flexbox/enums/display.cs`              |
| C# Source | `source/timewarp-flexbox/enums/edge.cs`                 |
| C# Source | `source/timewarp-flexbox/enums/errata.cs`               |
| C# Source | `source/timewarp-flexbox/enums/experimental-feature.cs` |
| C# Source | `source/timewarp-flexbox/enums/flex-direction.cs`       |
| C# Source | `source/timewarp-flexbox/enums/gutter.cs`               |
| C# Source | `source/timewarp-flexbox/enums/justify.cs`              |
| C# Source | `source/timewarp-flexbox/enums/log-level.cs`            |
| C# Source | `source/timewarp-flexbox/enums/measure-mode.cs`         |
| C# Source | `source/timewarp-flexbox/enums/node-type.cs`            |
| C# Source | `source/timewarp-flexbox/enums/overflow.cs`             |
| C# Source | `source/timewarp-flexbox/enums/physical-edge.cs`        |
| C# Source | `source/timewarp-flexbox/enums/position-type.cs`        |
| C# Source | `source/timewarp-flexbox/enums/unit.cs`                 |
| C# Source | `source/timewarp-flexbox/enums/wrap.cs`                 |

## Dependencies

- Task 102: YogaEnums utilities
- Task 105: YGEnums base

## Todo List

- [ ] Port all 17 enum files to C#
- [ ] Ensure enum values match C++ exactly
- [ ] Add extension methods for enum operations (e.g., `isRow()`, `isColumn()`)
- [ ] Add `[Flags]` attribute where appropriate (Errata)

## Acceptance Criteria

- [ ] All 17 enums ported with correct values
- [ ] Extension methods working (dimension(), isReverse(), etc.)
- [ ] Flags enums working correctly
- [ ] Used by higher-level types correctly

## Notes

Example conversions:

```csharp
public enum Align
{
    Auto,
    FlexStart,
    Center,
    FlexEnd,
    Stretch,
    Baseline,
    SpaceBetween,
    SpaceAround,
    SpaceEvenly
}

public static class AlignExtensions
{
    public static bool IsSpacing(this Align align) =>
        align is Align.SpaceBetween or Align.SpaceAround or Align.SpaceEvenly;
}

[Flags]
public enum Errata
{
    None = 0,
    StretchFlexBasis = 1,
    AbsolutePositioning = 2,
    AbsolutePercentPaddingAndBorder = 4,
    All = StretchFlexBasis | AbsolutePositioning | AbsolutePercentPaddingAndBorder,
    Classic = StretchFlexBasis | AbsolutePositioning
}
```
