# Task 038-implement-intrinsic-size-tests

## Summary
Implement tests for intrinsic sizing behavior, covering min-content, max-content, and fit-content sizing modes. These tests verify how elements size themselves based on their content when no explicit dimensions are provided.

## Todo List
- [ ] Test intrinsic width calculation (size to content)
- [ ] Test intrinsic height calculation
- [ ] Test min-content width behavior
- [ ] Test max-content width behavior
- [ ] Test fit-content width behavior
- [ ] Test intrinsic sizing with MeasureFunc
- [ ] Test intrinsic sizing with nested content
- [ ] Test intrinsic sizing with flex-grow
- [ ] Test intrinsic sizing with min/max constraints
- [ ] Test intrinsic sizing in column direction

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/IntrinsicSize_/

Reference: yoga/tests/generated/YGIntrinsicSizeTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.IntrinsicSize_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class IntrinsicWidth_Should_
{
  public static void SizeToChildrenWidth()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(50) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    root.Layout.Width.ShouldBe(80);  // 50 + 30
  }
  
  public static void UseMaxChildWidthInColumn()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(20) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    root.Layout.Width.ShouldBe(80);  // Max of children
  }
}

[TestTag(TestTags.Fast)]
public class IntrinsicHeight_Should_
{
  public static void SizeToChildrenHeightInColumn()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(40) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    root.Layout.Height.ShouldBe(70);  // 30 + 40
  }
}

[TestTag(TestTags.Fast)]
public class IntrinsicSizeWithMeasureFunc_Should_
{
  public static void UseMeasureFuncResult()
  {
    FlexNode root = new();
    
    FlexNode child = new();
    child.MeasureFunc = (node, width, widthMode, height, heightMode) =>
    {
      return new MeasureResult(100, 50);
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    child.Layout.Width.ShouldBe(100);
    child.Layout.Height.ShouldBe(50);
    root.Layout.Width.ShouldBe(100);
    root.Layout.Height.ShouldBe(50);
  }
  
  public static void PassCorrectModeForUnconstrained()
  {
    MeasureMode receivedWidthMode = MeasureMode.Exactly;
    MeasureMode receivedHeightMode = MeasureMode.Exactly;
    
    FlexNode root = new();
    
    FlexNode child = new();
    child.MeasureFunc = (node, width, widthMode, height, heightMode) =>
    {
      receivedWidthMode = widthMode;
      receivedHeightMode = heightMode;
      return new MeasureResult(50, 50);
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    receivedWidthMode.ShouldBe(MeasureMode.Undefined);
    receivedHeightMode.ShouldBe(MeasureMode.Undefined);
  }
}

[TestTag(TestTags.Fast)]
public class IntrinsicSizeWithConstraints_Should_
{
  public static void RespectMinWidth()
  {
    FlexNode root = new()
    {
      MinWidth = FlexValue.Point(100)
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    root.Layout.Width.ShouldBe(100);  // Min enforced
  }
  
  public static void RespectMaxWidth()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      MaxWidth = FlexValue.Point(80)
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    root.Layout.Width.ShouldBe(80);  // Max enforced
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
