# Task 031-implement-align-content-tests

## Summary
Implement comprehensive tests for align-content behavior with multi-line flex containers, covering all alignment values (flex-start, flex-end, center, space-between, space-around, space-evenly, stretch). These tests verify cross-axis line distribution when flex-wrap creates multiple lines.

## Todo List
- [ ] Test align-content: flex-start positions lines at container start
- [ ] Test align-content: flex-end positions lines at container end
- [ ] Test align-content: center positions lines in center
- [ ] Test align-content: space-between distributes lines with space between
- [ ] Test align-content: space-around distributes lines with equal space around
- [ ] Test align-content: space-evenly distributes lines with equal space
- [ ] Test align-content: stretch expands lines to fill container
- [ ] Test align-content with single line (should have no effect)
- [ ] Test align-content with varying line heights
- [ ] Test align-content with fixed vs auto cross-axis container size
- [ ] Test align-content combined with align-items

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/AlignContent_/

Reference: yoga/tests/generated/YGAlignContentTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.AlignContent_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class FlexStart_Should_
{
  public static void PositionLinesAtContainerStart()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.FlexStart
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(100, 100);
    
    // Line 1: child1, child2 (top = 0)
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
    // Line 2: child3 (top = 20)
    child3.Layout.Top.ShouldBe(20);
  }
}

[TestTag(TestTags.Fast)]
public class SpaceBetween_Should_
{
  public static void DistributeLinesWithSpaceBetween()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.SpaceBetween
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(100, 100);
    
    // Line 1 at top, Line 2 at bottom
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
    child3.Layout.Top.ShouldBe(80);  // 100 - 20
  }
}

[TestTag(TestTags.Fast)]
public class Stretch_Should_
{
  public static void ExpandLinesToFillContainer()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.Stretch
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50) };
    FlexNode child3 = new() { Width = FlexValue.Point(50) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(100, 100);
    
    // Two lines, each 50px tall (100 / 2)
    child1.Layout.Height.ShouldBe(50);
    child2.Layout.Height.ShouldBe(50);
    child3.Layout.Height.ShouldBe(50);
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
