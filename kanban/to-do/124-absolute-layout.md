# Task 124 - AbsoluteLayout

## Summary

Port the AbsoluteLayout module from C++ to C#. This handles layout of absolutely positioned children. This is a Level 8 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                                | Lines |
| ---------- | ----------------------------------- | ----- |
| C++ Header | `yoga/algorithm/AbsoluteLayout.h`   | ~50   |
| C++ Source | `yoga/algorithm/AbsoluteLayout.cpp` | ~400  |

## Target Files

| Type      | Path                                                   |
| --------- | ------------------------------------------------------ |
| C# Source | `source/timewarp-flexbox/algorithm/absolute-layout.cs` |

## Dependencies

- Task 113: Event system
- Task 122: Node
- Task 123: Algorithm helpers (Align, BoundAxis, TrailingPosition)

## Todo List

- [ ] Port `AbsoluteLayout.h` to C#
- [ ] Port `AbsoluteLayout.cpp` to C#
- [ ] Handle all absolute positioning edge cases

## Acceptance Criteria

- [ ] Absolute positioning with insets working
- [ ] Percentage-based positioning working
- [ ] Auto insets working
- [ ] Stretch behavior working
- [ ] Used correctly by CalculateLayout (Task 125)

## Notes

```csharp
public static class AbsoluteLayout
{
    public static void LayoutAbsoluteChild(
        Node containingBlock,
        Node node,
        float containingBlockWidth,
        float containingBlockHeight,
        SizingMode widthSizingMode,
        Direction direction,
        LayoutData layoutData,
        int depth,
        int generationCount);

    public static void LayoutAbsoluteChildren(
        Node node,
        Direction direction,
        LayoutData layoutData,
        int depth,
        int generationCount);
}
```
