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
| C# Source | `source/timewarp-flexbox/Algorithm/AbsoluteLayout.cs`  |

## Dependencies

- Task 113: Event system
- Task 122: Node
- Task 123: Algorithm helpers (Align, BoundAxis, TrailingPosition)

## Todo List

- [x] Port `AbsoluteLayout.h` to C#
- [x] Port `AbsoluteLayout.cpp` to C#
- [x] Handle all absolute positioning edge cases

## Acceptance Criteria

- [x] Absolute positioning with insets working
- [x] Percentage-based positioning working
- [x] Auto insets working
- [x] Stretch behavior working
- [ ] Used correctly by CalculateLayout (Task 125)

## Notes

### Implementation Details

The AbsoluteLayout module was ported with a delegate pattern for `CalculateLayoutInternal` to avoid circular compile-time dependencies:

```csharp
public static class AbsoluteLayout
{
    // Set by CalculateLayout during initialization
    public static Func<Node, float, float, Direction, SizingMode, SizingMode, 
        float, float, bool, LayoutPassReason, LayoutData, int, int, bool>?
        CalculateLayoutInternal { get; set; }

    public static void LayoutAbsoluteChild(
        Node containingNode,
        Node node,
        Node child,
        float containingBlockWidth,
        float containingBlockHeight,
        SizingMode widthMode,
        Direction direction,
        LayoutData layoutMarkerData,
        int depth,
        int generationCount);

    public static bool LayoutAbsoluteDescendants(
        Node containingNode,
        Node currentNode,
        SizingMode widthSizingMode,
        Direction currentNodeDirection,
        LayoutData layoutMarkerData,
        int currentDepth,
        int generationCount,
        float currentNodeLeftOffsetFromContainingBlock,
        float currentNodeTopOffsetFromContainingBlock,
        float containingNodeAvailableInnerWidth,
        float containingNodeAvailableInnerHeight);
}
```

### Private Helpers

The following private helper methods were implemented:
- `SetFlexStartLayoutPosition` - Position child at flex-start
- `SetFlexEndLayoutPosition` - Position child at flex-end
- `SetCenterLayoutPosition` - Center child within parent
- `JustifyAbsoluteChild` - Apply justify-content to absolute child
- `AlignAbsoluteChild` - Apply align-items to absolute child
- `PositionAbsoluteChild` - Main positioning logic handling insets

### Errata Support

The implementation handles the following errata:
- `Errata.AbsolutePositionWithoutInsetsExcludesPadding`
- `Errata.AbsolutePercentAgainstInnerSize`

## Results (2026-07-03)

AbsoluteLayout.cs ported and verified: all 34 generated AbsolutePosition
conformance tests and all 62 StaticPosition conformance tests pass, LTR and
RTL passes included.
