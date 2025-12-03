# Task 028-implement-fluent-api

## Summary
Implement a fluent API for FlexNode to enable declarative, chainable node configuration.

## Todo List
- [ ] Create Nodes/FlexNodeExtensions.cs with fluent methods
- [ ] Implement Direction(FlexDirection) returning FlexNode
- [ ] Implement Wrap(FlexWrap) returning FlexNode
- [ ] Implement Justify(JustifyContent) returning FlexNode
- [ ] Implement AlignItems(AlignItems) returning FlexNode
- [ ] Implement AlignContent(AlignContent) returning FlexNode
- [ ] Implement AlignSelf(AlignSelf) returning FlexNode
- [ ] Implement Grow(float), Shrink(float), Basis(FlexValue)
- [ ] Implement Size(float w, float h), Width(float), Height(float)
- [ ] Implement Margin(...), Padding(...), Border(...) overloads
- [ ] Implement AddChildren(params FlexNode[]) returning FlexNode
- [ ] Add tests for fluent API usage
- [ ] Verify code follows csharp-coding.md standards

## Notes
Fluent API enables clean, declarative layout definitions:

```csharp
public static class FlexNodeExtensions
{
  public static FlexNode Direction(this FlexNode node, FlexDirection direction)
  {
    node.FlexDirection = direction;
    return node;
  }
  
  public static FlexNode Size(this FlexNode node, float width, float height)
  {
    node.Width = FlexValue.Point(width);
    node.Height = FlexValue.Point(height);
    return node;
  }
  
  public static FlexNode Margin(this FlexNode node, float all)
  {
    node.SetMargin(Edge.All, FlexValue.Point(all));
    return node;
  }
  
  public static FlexNode AddChildren(this FlexNode node, params FlexNode[] children)
  {
    foreach (FlexNode child in children)
      node.AddChild(child);
    return node;
  }
}
```

Usage example:
```csharp
FlexNode root = new FlexNode()
  .Direction(FlexDirection.Column)
  .Size(800, 600)
  .Padding(10)
  .AddChildren(
    new FlexNode()
      .Direction(FlexDirection.Row)
      .Grow(1)
      .AddChildren(
        new FlexNode().Grow(1),
        new FlexNode().Size(200, 100)
      ),
    new FlexNode().Height(50)
  );
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
