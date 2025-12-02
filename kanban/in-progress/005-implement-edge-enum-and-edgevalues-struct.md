# Task 005-implement-edge-enum-and-edgevalues-struct

## Summary
Implement the Edge enum and EdgeValues<T> generic struct for handling margin, padding, border, and position values with CSS-style cascade logic.

## Todo List
- [ ] Create Enums/Edge.cs (Left, Top, Right, Bottom, Start, End, Horizontal, Vertical, All)
- [ ] Create Values/EdgeValues.cs as generic struct EdgeValues<T>
- [ ] Implement indexer for getting/setting edge values by Edge enum
- [ ] Implement cascade resolution (All -> Horizontal/Vertical -> Start/End -> Left/Right/Top/Bottom)
- [ ] Add helper methods: SetAll(T), SetHorizontal(T), SetVertical(T)
- [ ] Implement computed getters: ComputedLeft, ComputedTop, ComputedRight, ComputedBottom
- [ ] Verify code follows csharp-coding.md standards

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
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
