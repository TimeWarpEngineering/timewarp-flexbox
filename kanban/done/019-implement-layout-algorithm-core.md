# Task 019-implement-layout-algorithm-core

## Summary
Implement the core flexbox layout algorithm that calculates positions and sizes for all nodes in the tree.

## Todo List
- [ ] Create Layout/FlexLayoutEngine.cs class
- [ ] Implement CalculateLayout(FlexNode root, float width, float height) entry point
- [ ] Implement node measurement for leaf nodes (using MeasureFunc)
- [ ] Implement main axis size calculation
- [ ] Implement cross axis size calculation
- [ ] Implement flex-grow distribution
- [ ] Implement flex-shrink distribution
- [ ] Implement main axis alignment (justify-content)
- [ ] Implement cross axis alignment (align-items)
- [ ] Handle absolute positioned children separately
- [ ] Recursively layout children
- [ ] Write results to node.Layout properties
- [ ] Verify code follows csharp-coding.md standards

## Notes
This is the most complex task - the core Yoga algorithm. Follow Yoga's algorithm structure:

```csharp
public class FlexLayoutEngine
{
  public void CalculateLayout(FlexNode root, float availableWidth, float availableHeight)
  {
    // 1. Reset layout results
    ResetLayoutResults(root);
    
    // 2. Calculate layout recursively
    LayoutNode(
      root,
      availableWidth,
      MeasureMode.Exactly,
      availableHeight,
      MeasureMode.Exactly,
      Direction.LTR
    );
  }
  
  private void LayoutNode(
    FlexNode node,
    float availableWidth,
    MeasureMode widthMode,
    float availableHeight,
    MeasureMode heightMode,
    Direction direction)
  {
    // Handle leaf nodes with measure function
    if (node.IsLeaf && node.HasMeasureFunc)
    {
      MeasureLeafNode(node, availableWidth, widthMode, availableHeight, heightMode);
      return;
    }
    
    // Collect children into flex lines
    // Calculate main axis sizes
    // Resolve flexible lengths (grow/shrink)
    // Calculate cross axis sizes
    // Perform alignment
    // Position children
    // Recursively layout children
  }
}
```

Reference Yoga's algorithm/CalculateLayout.cpp for detailed logic.

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
