# Task 040-implement-padding-tests

## Summary
Implement comprehensive tests for padding calculations including uniform padding, individual edge padding, percentage padding, and computed padding resolution. Padding creates internal space within an element, affecting where children are positioned.

## Todo List
- [ ] Test uniform padding on all edges
- [ ] Test individual edge padding (top, right, bottom, left)
- [ ] Test padding offsets child positioning
- [ ] Test padding reduces available space for children
- [ ] Test padding with percentage children
- [ ] Test percentage padding relative to parent width
- [ ] Test padding with flex-grow children
- [ ] Test padding with RTL direction
- [ ] Test padding start/end (logical properties)
- [ ] Test padding combined with border
- [ ] Test computed padding value resolution

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/Padding_/

Reference: 
- yoga/tests/generated/YGPaddingTest.cpp
- yoga/tests/YGComputedPaddingTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.Padding_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class UniformPadding_Should_
{
  public static void OffsetChildPosition()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      PaddingTop = FlexValue.Point(10),
      PaddingRight = FlexValue.Point(10),
      PaddingBottom = FlexValue.Point(10),
      PaddingLeft = FlexValue.Point(10)
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
      PaddingTop = FlexValue.Point(10),
      PaddingRight = FlexValue.Point(10),
      PaddingBottom = FlexValue.Point(10),
      PaddingLeft = FlexValue.Point(10)
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
public class IndividualPadding_Should_
{
  public static void ApplyOnlyToSpecifiedEdge()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      PaddingLeft = FlexValue.Point(20)
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
public class PercentagePadding_Should_
{
  public static void ResolveRelativeToParentWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      PaddingLeft = FlexValue.Percent(10),  // 10% of 200 = 20
      PaddingTop = FlexValue.Percent(5)     // 5% of 200 = 10 (always parent width)
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(200, 100);
    
    child.Layout.Left.ShouldBe(20);
    child.Layout.Top.ShouldBe(10);
  }
}

[TestTag(TestTags.Fast)]
public class PaddingWithFlexGrow_Should_
{
  public static void AccountForPaddingInFlexCalculation()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      PaddingLeft = FlexValue.Point(10),
      PaddingRight = FlexValue.Point(10)
    };
    
    FlexNode child = new() { FlexGrow = 1 };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    // Child grows to fill available space minus padding
    child.Layout.Width.ShouldBe(80);  // 100 - 10 - 10
  }
}

[TestTag(TestTags.Fast)]
public class ComputedPadding_Should_
{
  public static void ResolvePercentagesToPoints()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      PaddingLeft = FlexValue.Percent(10)
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(200, 100);
    
    // After layout, computed padding should be resolved
    root.GetComputedPadding(Edge.Left).ShouldBe(20);  // 10% of 200
  }
  
  public static void ReturnZeroForUnsetPadding()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    
    root.CalculateLayout(100, 100);
    
    root.GetComputedPadding(Edge.Left).ShouldBe(0);
    root.GetComputedPadding(Edge.Top).ShouldBe(0);
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
