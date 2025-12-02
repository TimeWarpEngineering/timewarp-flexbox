# Task 021-implement-layout-algorithm-alignment

## Summary
Implement main axis and cross axis alignment algorithms for justify-content, align-items, align-content, and align-self.

## Todo List
- [ ] Implement justify-content alignment (main axis)
  - [ ] FlexStart - items at start
  - [ ] FlexEnd - items at end
  - [ ] Center - items centered
  - [ ] SpaceBetween - equal space between items
  - [ ] SpaceAround - equal space around items
  - [ ] SpaceEvenly - equal space between and around
- [ ] Implement align-items alignment (cross axis for items)
  - [ ] FlexStart, FlexEnd, Center
  - [ ] Stretch - fill cross axis
  - [ ] Baseline - align text baselines
- [ ] Implement align-content alignment (cross axis for lines)
- [ ] Implement align-self override per item
- [ ] Handle gap property in alignment calculations
- [ ] Verify code follows csharp-coding.md standards

## Notes
Alignment implementation:

```csharp
public class AlignmentResolver
{
  public void AlignMainAxis(
    FlexLine line,
    JustifyContent justifyContent,
    float containerMainSize,
    float gap)
  {
    float totalGap = gap * (line.Items.Count - 1);
    float freeSpace = containerMainSize - line.MainSize - totalGap;
    
    float offset = justifyContent switch
    {
      JustifyContent.FlexStart => 0,
      JustifyContent.FlexEnd => freeSpace,
      JustifyContent.Center => freeSpace / 2,
      _ => 0
    };
    
    float spacing = justifyContent switch
    {
      JustifyContent.SpaceBetween => freeSpace / (line.Items.Count - 1),
      JustifyContent.SpaceAround => freeSpace / line.Items.Count,
      JustifyContent.SpaceEvenly => freeSpace / (line.Items.Count + 1),
      _ => 0
    };
    
    // Position items with offset and spacing
  }
  
  public void AlignCrossAxis(
    FlexNode item,
    AlignItems alignItems,
    AlignSelf alignSelf,
    float lineCrossSize)
  {
    AlignItems effectiveAlign = alignSelf == AlignSelf.Auto 
      ? alignItems 
      : (AlignItems)alignSelf;
    
    // Apply alignment based on effective value
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
