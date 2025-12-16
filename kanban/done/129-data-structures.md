# Task 129 - CalculateLayout Data Structures

## Summary

Port the data structures required by the CalculateLayout algorithm: FlexLine, FlexLineRunningLayout, and LayoutData. These are used to track flex line state during layout and collect metrics.

## Source Files

| Type       | Path                          | Lines |
| ---------- | ----------------------------- | ----- |
| C++ Header | `yoga/algorithm/FlexLine.h`   | ~76   |
| C++ Source | `yoga/event/event.h`          | ~50 (LayoutData portion) |

## Target Files

| Type      | Path                                              |
| --------- | ------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Algorithm/FlexLine.cs`   |
| C# Source | `source/timewarp-flexbox/Event/Event.cs`          |

## Todo List

- [x] Port `FlexLineRunningLayout` struct
  - totalFlexGrowFactors (float)
  - totalFlexShrinkScaledFactors (float)
  - remainingFreeSpace (float)
  - mainDim (float)
  - crossDim (float)
- [x] Port `FlexLine` struct
  - itemsInFlow (List<Node>)
  - sizeConsumed (float)
  - numberOfAutoMargins (int)
  - layout (FlexLineRunningLayout)
- [x] Port `LayoutData` struct (metrics tracking)
  - layouts, measures, cachedLayouts, cachedMeasures (int)
  - maxMeasureCache (uint)
  - measureCallbacks (int)
  - measureCallbackReasonsCount (array)
- [x] Port `LayoutPassReason` enum
- [x] Port `LayoutType` enum
- [x] Add unit tests for data structures

## Dependencies

- Node class (already ported)
- LayoutPassReason enum values

## Notes

### FlexLineRunningLayout
Running calculation state that gets mutated as flex distribution progresses:
```csharp
public struct FlexLineRunningLayout
{
    public float TotalFlexGrowFactors;
    public float TotalFlexShrinkScaledFactors;
    public float RemainingFreeSpace;
    public float MainDim;
    public float CrossDim;
}
```

### FlexLine
Represents one line of flex items (in wrap mode, there can be multiple lines):
```csharp
public struct FlexLine
{
    public List<Node> ItemsInFlow;    // Children in normal flow
    public float SizeConsumed;         // Total space taken by items
    public int NumberOfAutoMargins;    // For auto margin distribution
    public FlexLineRunningLayout Layout;
}
```

### LayoutData (for metrics/events)
```csharp
public struct LayoutData
{
    public int Layouts;
    public int Measures;
    public uint MaxMeasureCache;
    public int CachedLayouts;
    public int CachedMeasures;
    public int MeasureCallbacks;
    public int[] MeasureCallbackReasonsCount; // indexed by LayoutPassReason
}
```
