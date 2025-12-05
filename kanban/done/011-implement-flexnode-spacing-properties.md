# Task 011-implement-flexnode-spacing-properties

## Summary
Add margin, padding, border, position, and gap spacing properties to FlexNode using EdgeValues<T>.

## Todo List
- [x] Add Margin property (EdgeValues<FlexValue>)
- [x] Add Padding property (EdgeValues<FlexValue>)
- [x] Add Border property (EdgeValues<float>)
- [x] Add Position property (EdgeValues<FlexValue>) for absolute positioning offsets
- [x] Add Gap property (float, default: 0) for main axis gap
- [x] Add RowGap property (float, default: 0)
- [x] Add ColumnGap property (float, default: 0)
- [x] Add convenience methods: SetMargin, SetPadding, SetBorder, SetPosition
- [x] Verify all property setters mark node as dirty
- [x] Verify code follows csharp-coding.md standards

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
- Created nodes/flex-node.spacing.cs as separate partial class file
- Edge value properties: Margin, Padding, Border, Position (all with EdgeValues<T>)
- Gap properties: Gap (sets both), RowGap, ColumnGap
- Convenience methods: SetMargin, SetPadding, SetBorder, SetPosition
- Gap setter sets both RowGap and ColumnGap simultaneously
- All setters mark node dirty
- Full XML documentation
- Build verified: 0 warnings, 0 errors
