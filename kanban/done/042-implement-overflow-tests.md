# Task 042-implement-overflow-tests

## Summary
Implement tests for overflow handling including size overflow detection and had-overflow flag behavior. These tests verify that the layout engine correctly tracks when content overflows its container, which is essential for scroll container implementation.

## Todo List
- [ ] Test overflow: visible (default, content extends beyond bounds)
- [ ] Test overflow: hidden (clips content)
- [ ] Test overflow: scroll behavior tracking
- [ ] Test HadOverflow flag is set when children overflow
- [ ] Test HadOverflow flag is false when content fits
- [ ] Test HadOverflow with nested containers
- [ ] Test HadOverflow with flex-wrap preventing overflow
- [ ] Test overflow with absolute positioned children
- [ ] Test overflow in both axes
- [ ] Test overflow detection with padding/border

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/Overflow_/

Reference: 
- yoga/tests/generated/YGSizeOverflowTest.cpp
- yoga/tests/YGHadOverflowTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.Overflow_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class HadOverflow_Should_
{
  public static void BeTrueWhenChildrenOverflow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(100, 100);
    
    // 160px of children in 100px container = overflow
    root.Layout.HadOverflow.ShouldBeTrue();
  }
  
  public static void BeFalseWhenContentFits()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(40), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(40), Height = FlexValue.Point(50) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(100, 100);
    
    // 80px of children in 100px container = no overflow
    root.Layout.HadOverflow.ShouldBeFalse();
  }
  
  public static void BeFalseWhenWrapPreventsOverflow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(40) };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(40) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(100, 100);
    
    // Wrapping prevents main-axis overflow
    root.Layout.HadOverflow.ShouldBeFalse();
  }
}

[TestTag(TestTags.Fast)]
public class OverflowDetection_Should_
{
  public static void DetectVerticalOverflow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Column
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(80) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(80) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(100, 100);
    
    root.Layout.HadOverflow.ShouldBeTrue();
  }
  
  public static void NotCountPaddingAsOverflow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      PaddingTop = FlexValue.Point(10),
      PaddingBottom = FlexValue.Point(10)
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(80)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    // 80 + 20 padding = 100, exactly fits
    root.Layout.HadOverflow.ShouldBeFalse();
  }
}

[TestTag(TestTags.Fast)]
public class OverflowWithAbsolutePositioning_Should_
{
  public static void NotAffectHadOverflow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    
    FlexNode absoluteChild = new() 
    { 
      Width = FlexValue.Point(200), 
      Height = FlexValue.Point(200),
      PositionType = PositionType.Absolute
    };
    
    root.AddChild(absoluteChild);
    
    root.CalculateLayout(100, 100);
    
    // Absolute children don't cause HadOverflow
    root.Layout.HadOverflow.ShouldBeFalse();
  }
}

[TestTag(TestTags.Fast)]
public class NestedOverflow_Should_
{
  public static void OnlyAffectImmediateParent()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };
    
    FlexNode parent = new() 
    { 
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50) };
    
    parent.AddChild(child1);
    parent.AddChild(child2);
    root.AddChild(parent);
    
    root.CalculateLayout(200, 200);
    
    parent.Layout.HadOverflow.ShouldBeTrue();
    root.Layout.HadOverflow.ShouldBeFalse();
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
