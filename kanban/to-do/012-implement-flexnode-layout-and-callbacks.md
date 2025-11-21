# Task 012-implement-flexnode-layout-and-callbacks

## Summary
Add LayoutResult property, measurement callbacks, and configuration reference to FlexNode.

## Todo List
- [ ] Add Layout property (LayoutResult, created in constructor)
- [ ] Add MeasureFunc property (MeasureFunc?, default: null)
- [ ] Add BaselineFunc property (BaselineFunc?, default: null)
- [ ] Add Config property (FlexConfig?, default: null uses global default)
- [ ] Add HasMeasureFunc computed property
- [ ] Add IsLeaf computed property (no children or has measure func)
- [ ] Add Context property (object?) for user data
- [ ] Implement CalculateLayout(float availableWidth, float availableHeight) method stub
- [ ] Verify code follows csharp-coding.md standards

## Notes
This completes the FlexNode class structure:

```csharp
public class FlexNode
{
  // Layout result (readonly, modified by layout algorithm)
  public LayoutResult Layout { get; }
  
  // Measurement callbacks
  public MeasureFunc? MeasureFunc { get; set; }
  public BaselineFunc? BaselineFunc { get; set; }
  
  // Configuration
  public FlexConfig? Config { get; set; }
  
  // Computed properties
  public bool HasMeasureFunc => MeasureFunc != null;
  public bool IsLeaf => Children.Count == 0 || HasMeasureFunc;
  
  // User data
  public object? Context { get; set; }
  
  // Layout entry point (implementation in later task)
  public void CalculateLayout(float availableWidth, float availableHeight);
}
```

Notes:
- Layout is created in constructor and reset on each calculation
- MeasureFunc is required for leaf nodes with intrinsic size (like text)
- BaselineFunc is optional, used for text baseline alignment
- Context allows users to associate arbitrary data with nodes

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
