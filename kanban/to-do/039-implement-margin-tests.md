# Task 039-implement-margin-tests

## Summary
Implement comprehensive tests for margin calculations including uniform margins, individual edge margins, auto margins for centering, percentage margins, and margin collapse behavior. Auto margins are particularly important for centering content in flexbox.

## Todo List
- [ ] Test uniform margin on all edges
- [ ] Test individual edge margins (top, right, bottom, left)
- [ ] Test margin affects element positioning
- [ ] Test margin: auto for centering on main axis
- [ ] Test margin: auto for centering on cross axis
- [ ] Test margin: auto with flex-grow (auto margins consume free space)
- [ ] Test percentage margins relative to parent width
- [ ] Test margin with RTL direction
- [ ] Test margin start/end (logical properties)
- [ ] Test negative margins
- [ ] Test margin on absolute positioned elements
- [ ] Test computed margin value resolution

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/Margin_/

Reference: 
- yoga/tests/generated/YGMarginTest.cpp
- yoga/tests/YGComputedMarginTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.Margin_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class UniformMargin_Should_
{
  public static void OffsetElementPosition()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50),
      MarginTop = FlexValue.Point(10),
      MarginLeft = FlexValue.Point(10)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    child.Layout.Left.ShouldBe(10);
    child.Layout.Top.ShouldBe(10);
  }
  
  public static void AffectParentSizing()
  {
    FlexNode root = new();
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50),
      MarginTop = FlexValue.Point(10),
      MarginBottom = FlexValue.Point(10),
      MarginLeft = FlexValue.Point(10),
      MarginRight = FlexValue.Point(10)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    root.Layout.Width.ShouldBe(70);   // 50 + 10 + 10
    root.Layout.Height.ShouldBe(70);  // 50 + 10 + 10
  }
}

[TestTag(TestTags.Fast)]
public class MarginAuto_Should_
{
  public static void CenterOnMainAxis()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30),
      MarginLeft = FlexValue.Auto,
      MarginRight = FlexValue.Auto
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, float.NaN);
    
    child.Layout.Left.ShouldBe(35);  // (100 - 30) / 2
  }
  
  public static void CenterOnCrossAxis()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30),
      MarginTop = FlexValue.Auto,
      MarginBottom = FlexValue.Auto
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    child.Layout.Top.ShouldBe(35);  // (100 - 30) / 2
  }
  
  public static void PushItemToEnd()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(20), Height = FlexValue.Point(20) };
    FlexNode child2 = new() 
    { 
      Width = FlexValue.Point(20), 
      Height = FlexValue.Point(20),
      MarginLeft = FlexValue.Auto
    };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(100, float.NaN);
    
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(80);  // Pushed to end
  }
}

[TestTag(TestTags.Fast)]
public class PercentageMargin_Should_
{
  public static void ResolveRelativeToParentWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30),
      MarginLeft = FlexValue.Percent(10),  // 10% of 100 = 10
      MarginTop = FlexValue.Percent(20)    // 20% of 100 = 20 (always parent width)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    child.Layout.Left.ShouldBe(10);
    child.Layout.Top.ShouldBe(20);
  }
}

[TestTag(TestTags.Fast)]
public class NegativeMargin_Should_
{
  public static void PullElementCloser()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode child2 = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50),
      MarginLeft = FlexValue.Point(-10)
    };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(100, float.NaN);
    
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(40);  // 50 - 10 overlap
  }
}

[TestTag(TestTags.Fast)]
public class ComputedMargin_Should_
{
  public static void ResolvePercentagesToPoints()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100)
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50),
      MarginLeft = FlexValue.Percent(10)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(200, 100);
    
    // After layout, computed margin should be resolved
    child.GetComputedMargin(Edge.Left).ShouldBe(20);  // 10% of 200
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
