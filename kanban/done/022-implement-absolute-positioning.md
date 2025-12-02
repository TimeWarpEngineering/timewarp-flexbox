# Task 022-implement-absolute-positioning

## Summary
Implement absolute positioning support for flex items with position: absolute.

## Todo List
- [ ] Filter absolute children from normal flex flow
- [ ] Implement absolute child positioning relative to container
- [ ] Handle position offsets (left, top, right, bottom)
- [ ] Handle percentage position values
- [ ] Handle auto-sized absolute children
- [ ] Handle stretch behavior for absolute children
- [ ] Implement containing block logic
- [ ] Add tests for absolute positioning scenarios
- [ ] Verify code follows csharp-coding.md standards

## Notes
Absolute positioning in flexbox:

```csharp
public class AbsoluteLayoutResolver
{
  public void LayoutAbsoluteChildren(FlexNode container)
  {
    foreach (FlexNode child in container.Children)
    {
      if (child.PositionType != PositionType.Absolute)
        continue;
      
      // Absolute children don't participate in flex flow
      // They're positioned relative to their containing block
      
      float left = ResolvePosition(child.Position.ComputedLeft, container.Layout.Width);
      float top = ResolvePosition(child.Position.ComputedTop, container.Layout.Height);
      float right = ResolvePosition(child.Position.ComputedRight, container.Layout.Width);
      float bottom = ResolvePosition(child.Position.ComputedBottom, container.Layout.Height);
      
      // Calculate size
      float width = CalculateAbsoluteWidth(child, container, left, right);
      float height = CalculateAbsoluteHeight(child, container, top, bottom);
      
      // Set layout results
      child.Layout.Left = left;
      child.Layout.Top = top;
      child.Layout.Width = width;
      child.Layout.Height = height;
      
      // Recursively layout absolute child's children
    }
  }
}
```

Key behaviors:
- Absolute children removed from flex calculation
- Position values relative to padding box of containing block
- If both left and right set (and width auto), stretch to fill
- Same for top/bottom with auto height

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
