# Task 005-implement-edge-enum-and-edgevalues-struct

## Summary
Implement the Edge enum and EdgeValues<T> generic struct for handling margin, padding, border, and position values with CSS-style cascade logic.

## Todo List
- [x] Create enums/edge.cs (Left, Top, Right, Bottom, Start, End, Horizontal, Vertical, All)
- [x] Create values/edge-values.cs as generic struct EdgeValues<T>
- [x] Implement indexer for getting/setting edge values by Edge enum
- [x] Implement cascade resolution (All -> Horizontal/Vertical -> Start/End -> Left/Right/Top/Bottom)
- [x] Add helper methods: SetAll(T), SetHorizontal(T), SetVertical(T)
- [x] Implement computed getters: ComputedLeft, ComputedTop, ComputedRight, ComputedBottom
- [x] Verify code follows csharp-coding.md standards

## Notes
EdgeValues handles the CSS shorthand cascade:
- Setting `All` affects all edges
- Setting `Horizontal` affects Left and Right
- Setting `Vertical` affects Top and Bottom
- `Start`/`End` are RTL-aware (Start = Left in LTR, Right in RTL)

Example usage:
```csharp
EdgeValues<FlexValue> Margin;
Margin[Edge.All] = FlexValue.Point(10);      // All edges = 10
Margin[Edge.Horizontal] = FlexValue.Point(5); // Left/Right = 5, Top/Bottom still 10
```

Consider using nullable T or a "has value" flag pattern for tracking which edges were explicitly set.

## Results
- Created enums/edge.cs with 9 values: Left, Top, Right, Bottom, Start, End, Horizontal, Vertical, All
- Created values/edge-values.cs as generic struct with nullable T? fields for tracking explicit values
- Implemented IEquatable<EdgeValues<T>> and equality operators (required by CA1815 analyzer)
- Cascade resolution properly handles RTL via isRtl parameter in ComputedLeft/ComputedRight
- Added Reset() method to clear all edge values
- Build verified: 0 warnings, 0 errors
