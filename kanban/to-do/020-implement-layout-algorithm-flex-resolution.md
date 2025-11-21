# Task 020-implement-layout-algorithm-flex-resolution

## Summary
Implement the flex-grow and flex-shrink resolution algorithms that distribute space among flexible items.

## Todo List
- [ ] Implement CalculateFlexBasis for each item
- [ ] Implement freeze/unfreeze logic for min/max constraints
- [ ] Implement flex-grow space distribution algorithm
- [ ] Implement flex-shrink space distribution (with scaled shrink factor)
- [ ] Handle min-width/min-height constraints during shrinking
- [ ] Handle max-width/max-height constraints during growing
- [ ] Implement violation resolution (when item size violates constraints)
- [ ] Iterate until all items frozen or no violations
- [ ] Add tests for complex flex scenarios
- [ ] Verify code follows csharp-coding.md standards

## Notes
Flex resolution follows the CSS Flexbox spec algorithm:

```csharp
public class FlexResolver
{
  public void ResolveFlexibleLengths(FlexLine line, float availableMainSize)
  {
    // 1. Calculate initial free space
    float freeSpace = availableMainSize - line.MainSize;
    
    // 2. If free space > 0, distribute with flex-grow
    // 3. If free space < 0, distribute with flex-shrink (using scaled factor)
    
    if (freeSpace > 0 && line.TotalFlexGrow > 0)
    {
      foreach (FlexNode item in line.Items)
      {
        float ratio = item.FlexGrow / line.TotalFlexGrow;
        float extraSpace = freeSpace * ratio;
        // Apply to item, respecting max constraints
      }
    }
    else if (freeSpace < 0 && line.TotalFlexShrink > 0)
    {
      // Scaled shrink factor = flex-shrink * flex-basis
      foreach (FlexNode item in line.Items)
      {
        float scaledShrink = item.FlexShrink * item.FlexBasisResolved;
        float ratio = scaledShrink / line.TotalWeightedFlexShrink;
        float shrinkAmount = -freeSpace * ratio;
        // Apply to item, respecting min constraints
      }
    }
  }
}
```

Key points:
- Flex-shrink uses weighted distribution (multiplied by flex-basis)
- Items that hit min/max constraints are "frozen"
- Algorithm iterates until all items frozen

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
