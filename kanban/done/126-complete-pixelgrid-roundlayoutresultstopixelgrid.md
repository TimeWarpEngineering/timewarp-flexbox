# Task 126 - Complete PixelGrid RoundLayoutResultsToPixelGrid

## Summary

Port the `roundLayoutResultsToPixelGrid` method from C++ to C#. This is the only missing method in `PixelGrid.cs`. This is a subtask of Task 123.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                           | Lines  |
| ---------- | ------------------------------ | ------ |
| C++ Source | `yoga/algorithm/PixelGrid.cpp` | 65-133 |

## Target Files

| Type      | Path                                              |
| --------- | ------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Algorithm/PixelGrid.cs` |

## Dependencies

- Task 122: Node (completed)
- `RoundValueToPixelGrid` already exists in PixelGrid.cs

## Checklist

- [x] Add `RoundLayoutResultsToPixelGrid` method to `PixelGrid.cs`
- [x] Handle text rounding logic (NodeType.Text)
- [x] Recursively process child nodes
- [x] Test that builds successfully

## Acceptance Criteria

- [x] Method signature matches C++ version
- [x] Text nodes get special rounding treatment (ceil for fractional widths/heights)
- [x] Recursively processes all children
- [x] Project builds without errors

## Notes

The method rounds layout positions and dimensions to the pixel grid. Key implementation details:

```csharp
public static void RoundLayoutResultsToPixelGrid(
    Node node,
    double absoluteLeft,
    double absoluteTop)
{
    // Get point scale factor from config
    // Round positions (left, top)
    // Calculate absolute positions for width/height rounding
    // Handle text rounding (NodeType.Text) - don't round down to avoid truncation
    // Recursively call for all children
}
```

C++ reference (lines 65-133 of PixelGrid.cpp):
- Gets `pointScaleFactor` from node's config
- Calculates absolute positions for proper rounding
- Text nodes use ceil for fractional dimensions to avoid truncation
- Recursively processes all children with updated absolute positions

## Results

- Implemented `RoundLayoutResultsToPixelGrid` method in `PixelGrid.cs`
- Added `ArgumentNullException.ThrowIfNull(node)` for CA1062 compliance
- All 501 tests pass
- Build succeeds with no warnings or errors
