# Task 026-implement-direction-rtl-support

## Summary
Implement right-to-left (RTL) text direction support for internationalization.

## Todo List
- [ ] Add Direction property to FlexNode (LTR, RTL, Inherit)
- [ ] Add Direction to FlexConfig for default direction
- [ ] Implement Start/End edge resolution based on direction
- [ ] Implement row direction reversal for RTL
- [ ] Update LayoutHelpers to resolve direction-aware edges
- [ ] Update alignment calculations for RTL
- [ ] Add tests for RTL layout scenarios
- [ ] Verify code follows csharp-coding.md standards

## Notes
RTL support allows flexbox layouts for right-to-left languages:

```csharp
public enum Direction
{
  Inherit,  // Use parent's direction (or config default for root)
  LTR,      // Left-to-right
  RTL       // Right-to-left
}

public partial class FlexNode
{
  public Direction Direction { get; set; } = Direction.Inherit;
  
  // Resolved direction (never Inherit)
  internal Direction ResolvedDirection => Direction != Direction.Inherit
    ? Direction
    : Parent?.ResolvedDirection ?? Config?.Direction ?? Direction.LTR;
}

public static class DirectionHelpers
{
  public static Edge ResolveStartEdge(Direction direction)
    => direction == Direction.RTL ? Edge.Right : Edge.Left;
  
  public static Edge ResolveEndEdge(Direction direction)
    => direction == Direction.RTL ? Edge.Left : Edge.Right;
}
```

RTL affects:
- Edge.Start maps to Right instead of Left
- Edge.End maps to Left instead of Right
- Row direction is visually reversed
- Flex-start/flex-end positions swap

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
