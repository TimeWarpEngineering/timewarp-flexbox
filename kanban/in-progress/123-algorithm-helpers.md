# Task 123 - Algorithm Helpers

## Summary

Port the algorithm helper modules from C++ to C#. These are utilities used by the main layout algorithm. This is a Level 8 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                                | Lines    |
| ---------- | ----------------------------------- | -------- |
| C++ Header | `yoga/algorithm/PixelGrid.h`        | ~50      |
| C++ Source | `yoga/algorithm/PixelGrid.cpp`      | ~80      |
| C++ Header | `yoga/algorithm/Baseline.h`         | ~30      |
| C++ Source | `yoga/algorithm/Baseline.cpp`       | ~100     |
| C++ Header | `yoga/algorithm/FlexLine.h`         | ~80      |
| C++ Source | `yoga/algorithm/FlexLine.cpp`       | ~150     |
| C++ Header | `yoga/algorithm/Align.h`            | ~50      |
| C++ Header | `yoga/algorithm/BoundAxis.h`        | ~80      |
| C++ Header | `yoga/algorithm/TrailingPosition.h` | ~50      |
| C++ Header | `yoga/YGPixelGrid.h`                | ~30      |
| C++ Source | `yoga/YGPixelGrid.cpp`              | ~50      |
| **Total**  |                                     | **~750** |

## Target Files

| Type      | Path                                                     |
| --------- | -------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/algorithm/pixel-grid.cs`        |
| C# Source | `source/timewarp-flexbox/algorithm/baseline.cs`          |
| C# Source | `source/timewarp-flexbox/algorithm/flex-line.cs`         |
| C# Source | `source/timewarp-flexbox/algorithm/align-utils.cs`       |
| C# Source | `source/timewarp-flexbox/algorithm/bound-axis.cs`        |
| C# Source | `source/timewarp-flexbox/algorithm/trailing-position.cs` |
| C# Source | `source/timewarp-flexbox/yg-pixel-grid.cs`               |

## Dependencies

- Task 117: FlexDirection utilities
- Task 122: Node

## Todo List

- [x] Port `PixelGrid.h/.cpp` (pixel rounding) - `RoundValueToPixelGrid` complete
- [x] Port `Baseline.h/.cpp` (baseline calculation) - Complete
- [x] Port `FlexLine.h/.cpp` (flex line management) - Complete
- [x] Port `Align.h` (alignment utilities) - Complete
- [x] Port `BoundAxis.h` (axis bound calculations) - Complete
- [x] Port `TrailingPosition.h` (trailing position calculations) - Complete
- [x] Port `YGPixelGrid.h/.cpp` (public pixel grid API) - Not needed (C API wrapper only)

## Remaining Work (Subtasks)

This task was too large for a single context. Remaining work has been split into:

- **Task 126**: Complete PixelGrid `RoundLayoutResultsToPixelGrid` method - ✅ Done
- **Task 127**: Add Algorithm Helper Tests - ✅ Done
- **Task 128**: Verify Algorithm Helpers Against C++ Reference - ✅ Done

## Acceptance Criteria

- [x] Pixel rounding working correctly (basic `RoundValueToPixelGrid`)
- [x] Baseline calculation working
- [x] Flex line splitting working
- [x] Alignment calculations correct
- [x] Axis bounds correct
- [x] Trailing positions correct
- [x] `RoundLayoutResultsToPixelGrid` complete (Task 126)
- [x] Tests added (Task 127)
- [x] Verified against C++ (Task 128)
- [ ] Used correctly by CalculateLayout (Task 125)

## Notes

```csharp
public static class PixelGrid
{
    public static float RoundToPixelGrid(float value, float pointScaleFactor, bool ceil, bool floor);
}

public static class Baseline
{
    public static float Calculate(Node node, float width, float height);
}

public class FlexLine
{
    public List<Node> Items { get; } = new();
    public float MainSize { get; set; }
    public float CrossSize { get; set; }
}

public static class AlignUtils
{
    public static float GetAlignmentOffset(Align align, float remaining, int itemCount, float gap);
}
```
