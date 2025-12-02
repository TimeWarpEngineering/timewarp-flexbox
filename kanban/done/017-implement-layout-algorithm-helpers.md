# Task 017-implement-layout-algorithm-helpers

## Summary
Implement helper utilities for the layout algorithm including axis resolution, value computation, and dimension helpers.

## Todo List
- [ ] Create Layout/LayoutHelpers.cs static class
- [ ] Implement ResolveFlexDirection(FlexDirection, Direction) for RTL support
- [ ] Implement GetMainAxis(FlexDirection) -> returns axis enum
- [ ] Implement GetCrossAxis(FlexDirection) -> returns axis enum
- [ ] Implement IsRow(FlexDirection) and IsColumn(FlexDirection)
- [ ] Implement IsMainAxisRow(FlexNode) and IsMainAxisColumn(FlexNode)
- [ ] Create Layout/ValueResolver.cs static class
- [ ] Implement ResolveValue(FlexValue, float containerSize) -> float
- [ ] Implement ResolveValueOrDefault(FlexValue, float containerSize, float defaultValue)
- [ ] Create Enums/Axis.cs enum (Row, Column)
- [ ] Create Enums/Direction.cs enum (LTR, RTL, Inherit)
- [ ] Verify code follows csharp-coding.md standards

## Notes
These helpers are used throughout the layout algorithm:

```csharp
public static class LayoutHelpers
{
  public static Axis GetMainAxis(FlexDirection direction) => direction switch
  {
    FlexDirection.Row or FlexDirection.RowReverse => Axis.Row,
    FlexDirection.Column or FlexDirection.ColumnReverse => Axis.Column,
    _ => throw new ArgumentOutOfRangeException()
  };
  
  public static bool IsRow(FlexDirection direction) => 
    direction == FlexDirection.Row || direction == FlexDirection.RowReverse;
}

public static class ValueResolver
{
  public static float ResolveValue(FlexValue value, float containerSize) => value.Unit switch
  {
    Unit.Point => value.Value,
    Unit.Percent => containerSize * value.Value / 100f,
    Unit.Auto => float.NaN,  // Auto needs special handling
    Unit.Undefined => float.NaN,
    _ => float.NaN
  };
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
