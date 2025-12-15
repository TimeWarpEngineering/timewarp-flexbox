# Task 117 - FlexDirection Utilities

## Summary

Port the FlexDirection algorithm utilities from C++ to C#. These provide helpers for working with flex directions (main/cross axis, leading/trailing edges). This is a Level 5 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                             | Lines |
| ---------- | -------------------------------- | ----- |
| C++ Header | `yoga/algorithm/FlexDirection.h` | ~150  |

## Target Files

| Type      | Path                                                        |
| --------- | ----------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/algorithm/flex-direction-utils.cs` |

## Dependencies

- Task 109: FlexDirection, Dimension, Direction, Edge, PhysicalEdge enums
- Task 112: AssertFatal

## Todo List

- [ ] Port `FlexDirection.h` utilities to C#
- [ ] Implement `isRow()`, `isColumn()`, `isReverse()`
- [ ] Implement main/cross axis dimension helpers
- [ ] Implement leading/trailing edge helpers

## Acceptance Criteria

- [ ] All direction utilities working
- [ ] Main axis dimension mapping correct
- [ ] Cross axis dimension mapping correct
- [ ] Leading/trailing edge mapping correct
- [ ] Used by layout algorithm (Task 125)

## Notes

```csharp
public static class FlexDirectionUtils
{
    public static bool IsRow(this FlexDirection direction) =>
        direction is FlexDirection.Row or FlexDirection.RowReverse;

    public static bool IsColumn(this FlexDirection direction) =>
        direction is FlexDirection.Column or FlexDirection.ColumnReverse;

    public static bool IsReverse(this FlexDirection direction) =>
        direction is FlexDirection.RowReverse or FlexDirection.ColumnReverse;

    public static Dimension MainDimension(this FlexDirection direction) =>
        direction.IsRow() ? Dimension.Width : Dimension.Height;

    public static Dimension CrossDimension(this FlexDirection direction) =>
        direction.IsRow() ? Dimension.Height : Dimension.Width;

    public static Edge LeadingEdge(this FlexDirection direction, Direction layoutDirection);
    public static Edge TrailingEdge(this FlexDirection direction, Direction layoutDirection);
}
```
