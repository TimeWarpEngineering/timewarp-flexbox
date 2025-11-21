# Task 011-implement-flexnode-spacing-properties

## Summary
Add margin, padding, border, position, and gap spacing properties to FlexNode using EdgeValues<T>.

## Todo List
- [ ] Add Margin property (EdgeValues<FlexValue>)
- [ ] Add Padding property (EdgeValues<FlexValue>)
- [ ] Add Border property (EdgeValues<float>)
- [ ] Add Position property (EdgeValues<FlexValue>) for absolute positioning offsets
- [ ] Add Gap property (float, default: 0) for main axis gap
- [ ] Add RowGap property (float, default: 0)
- [ ] Add ColumnGap property (float, default: 0)
- [ ] Add convenience methods: SetMargin, SetPadding, SetBorder, SetPosition
- [ ] Verify all property setters mark node as dirty
- [ ] Verify code follows csharp-coding.md standards

## Notes
Spacing properties use EdgeValues for CSS-style cascade:

```csharp
public class FlexNode
{
  // Spacing with edge values
  public EdgeValues<FlexValue> Margin { get; set; }
  public EdgeValues<FlexValue> Padding { get; set; }
  public EdgeValues<float> Border { get; set; }
  public EdgeValues<FlexValue> Position { get; set; }
  
  // Gap (CSS gap property)
  public float Gap { get; set; } = 0f;      // Sets both row and column gap
  public float RowGap { get; set; } = 0f;
  public float ColumnGap { get; set; } = 0f;
  
  // Convenience methods
  public void SetMargin(Edge edge, FlexValue value);
  public void SetPadding(Edge edge, FlexValue value);
  public void SetBorder(Edge edge, float value);
  public void SetPosition(Edge edge, FlexValue value);
}
```

Usage example:
```csharp
node.SetMargin(Edge.All, FlexValue.Point(10));
node.SetMargin(Edge.Horizontal, FlexValue.Point(20)); // Overrides left/right
node.SetPadding(Edge.Top, FlexValue.Point(5));
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
