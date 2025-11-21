# Task 025-implement-pixel-grid-rounding

## Summary
Implement optional pixel grid rounding for high DPI displays and terminal cell alignment.

## Todo List
- [ ] Add PointScaleFactor support in FlexConfig
- [ ] Create Layout/PixelGrid.cs static class
- [ ] Implement RoundValueToPixelGrid(float value, float scaleFactor)
- [ ] Implement rounding mode options (floor, ceil, round)
- [ ] Add RoundLayoutToPixelGrid method for post-layout rounding
- [ ] Handle cumulative rounding errors (positions vs sizes)
- [ ] Add RoundToPixelGrid option to CalculateLayout
- [ ] Add tests for rounding behavior
- [ ] Verify code follows csharp-coding.md standards

## Notes
Pixel grid rounding ensures crisp rendering on displays:

```csharp
public static class PixelGrid
{
  public static float RoundToPixelGrid(
    float value,
    float pointScaleFactor,
    bool forceCeil = false,
    bool forceFloor = false)
  {
    float scaledValue = value * pointScaleFactor;
    float roundedValue;
    
    if (forceCeil)
      roundedValue = MathF.Ceiling(scaledValue);
    else if (forceFloor)
      roundedValue = MathF.Floor(scaledValue);
    else
      roundedValue = MathF.Round(scaledValue);
    
    return roundedValue / pointScaleFactor;
  }
  
  public static void RoundLayoutToPixelGrid(FlexNode node, float scaleFactor)
  {
    // Round positions first (using accumulated position)
    float absoluteLeft = GetAbsoluteLeft(node);
    float absoluteTop = GetAbsoluteTop(node);
    
    float roundedAbsoluteLeft = RoundToPixelGrid(absoluteLeft, scaleFactor);
    float roundedAbsoluteTop = RoundToPixelGrid(absoluteTop, scaleFactor);
    
    // Adjust relative position
    node.Layout.Left = roundedAbsoluteLeft - (node.Parent?.Layout.Left ?? 0);
    node.Layout.Top = roundedAbsoluteTop - (node.Parent?.Layout.Top ?? 0);
    
    // Round size based on rounded position
    float roundedRight = RoundToPixelGrid(absoluteLeft + node.Layout.Width, scaleFactor);
    float roundedBottom = RoundToPixelGrid(absoluteTop + node.Layout.Height, scaleFactor);
    
    node.Layout.Width = roundedRight - roundedAbsoluteLeft;
    node.Layout.Height = roundedBottom - roundedAbsoluteTop;
    
    // Recursively round children
    foreach (FlexNode child in node.Children)
      RoundLayoutToPixelGrid(child, scaleFactor);
  }
}
```

This is important for terminal rendering where cells are discrete units.

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
