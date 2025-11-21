# Task 002-implement-unit-enum-and-flexvalue-struct

## Summary
Implement the Unit enum and FlexValue readonly struct - the foundational value types for representing CSS-style lengths and sizes.

## Todo List
- [ ] Create Enums/Unit.cs with Point, Percent, Auto, Undefined values
- [ ] Create Values/FlexValue.cs as readonly struct
- [ ] Implement FlexValue.Value (float) and FlexValue.Unit properties
- [ ] Add static factory methods: Point(float), Percent(float)
- [ ] Add static readonly fields: Auto, Undefined
- [ ] Implement equality operators and IEquatable<FlexValue>
- [ ] Add ToString() for debugging
- [ ] Verify code follows csharp-coding.md standards (2-space indent, Allman style)

## Notes
```csharp
public readonly struct FlexValue : IEquatable<FlexValue>
{
  public float Value { get; }
  public Unit Unit { get; }
  
  public static FlexValue Point(float value);
  public static FlexValue Percent(float value);
  public static readonly FlexValue Auto;
  public static readonly FlexValue Undefined;
}
```

- FlexValue is immutable (readonly struct for performance)
- Auto and Undefined have special semantics in layout calculations
- Follow W3C CSS naming conventions

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
