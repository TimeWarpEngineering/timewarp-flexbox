# Task 018-implement-flex-line-collection

## Summary
Implement FlexLine struct and collection for managing flex lines during wrapping layout calculations.

## Todo List
- [ ] Create Layout/FlexLine.cs struct
- [ ] Add Items property (List<FlexNode>) for nodes in this line
- [ ] Add MainSize property (float) - total size along main axis
- [ ] Add CrossSize property (float) - size along cross axis
- [ ] Add RemainingFreeSpace property (float)
- [ ] Add TotalFlexGrow property (float) - sum of flex-grow
- [ ] Add TotalFlexShrink property (float) - sum of flex-shrink
- [ ] Create Layout/FlexLineCollection.cs class
- [ ] Implement methods to collect children into lines based on wrap
- [ ] Implement line size calculation methods
- [ ] Verify code follows csharp-coding.md standards

## Notes
FlexLine represents a single line of flex items when wrapping:

```csharp
public struct FlexLine
{
  public List<FlexNode> Items { get; }
  public float MainSize { get; set; }
  public float CrossSize { get; set; }
  public float RemainingFreeSpace { get; set; }
  public float TotalFlexGrow { get; set; }
  public float TotalFlexShrink { get; set; }
  public float TotalWeightedFlexShrink { get; set; }
  
  public FlexLine()
  {
    Items = new List<FlexNode>();
  }
}

public class FlexLineCollection
{
  public List<FlexLine> Lines { get; } = new();
  
  // Collect children into lines based on container size and wrap mode
  public void CollectLines(
    FlexNode container,
    float availableMainSize,
    FlexDirection direction,
    FlexWrap wrap);
}
```

Line collection algorithm:
1. Iterate through children
2. Calculate hypothetical main size of each child
3. If wrap enabled and line would overflow, start new line
4. Track flex-grow and flex-shrink totals per line

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
