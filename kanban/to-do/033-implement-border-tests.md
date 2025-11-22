# Task 033-implement-border-tests

## Summary
Implement tests for border handling in layout calculations. Borders affect the positioning of child content similar to padding but are tracked separately. Tests verify that borders correctly offset content and are included in total element size.

## Todo List
- [ ] Test uniform border on all edges
- [ ] Test individual edge borders (top, right, bottom, left)
- [ ] Test border affects child positioning
- [ ] Test border contributes to element size
- [ ] Test border with padding combined
- [ ] Test border with percentage children
- [ ] Test border with flex-grow children
- [ ] Test border with RTL direction
- [ ] Test border start/end (logical properties)
- [ ] Test border with absolute positioned children
- [ ] Test border values of zero

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/Border_/

Reference: yoga/tests/generated/YGBorderTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.Border_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class UniformBorder_Should_
{
  public static void OffsetChildContent()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BorderTop = 10,
      BorderRight = 10,
      BorderBottom = 10,
      BorderLeft = 10
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    child.Layout.Left.ShouldBe(10);
    child.Layout.Top.ShouldBe(10);
  }
  
  public static void ReduceAvailableSpaceForChildren()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BorderTop = 10,
      BorderRight = 10,
      BorderBottom = 10,
      BorderLeft = 10
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Percent(100), 
      Height = FlexValue.Percent(100)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    // Available space is 100 - 10 - 10 = 80
    child.Layout.Width.ShouldBe(80);
    child.Layout.Height.ShouldBe(80);
  }
}

[TestTag(TestTags.Fast)]
public class IndividualBorders_Should_
{
  public static void ApplyOnlyToSpecifiedEdge()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BorderLeft = 20
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    child.Layout.Left.ShouldBe(20);
    child.Layout.Top.ShouldBe(0);
  }
}

[TestTag(TestTags.Fast)]
public class BorderWithPadding_Should_
{
  public static void StackCorrectly()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BorderTop = 10,
      BorderLeft = 10,
      PaddingTop = FlexValue.Point(5),
      PaddingLeft = FlexValue.Point(5)
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    // Child offset by border + padding
    child.Layout.Left.ShouldBe(15);  // 10 + 5
    child.Layout.Top.ShouldBe(15);   // 10 + 5
  }
}

[TestTag(TestTags.Fast)]
public class BorderWithFlexGrow_Should_
{
  public static void AccountForBorderInFlexCalculation()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      BorderLeft = 10,
      BorderRight = 10
    };
    
    FlexNode child = new() { FlexGrow = 1 };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    // Child grows to fill available space minus borders
    child.Layout.Width.ShouldBe(80);  // 100 - 10 - 10
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
