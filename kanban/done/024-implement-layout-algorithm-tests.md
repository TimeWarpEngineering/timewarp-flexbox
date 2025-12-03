# Task 024-implement-layout-algorithm-tests

## Summary
Create comprehensive layout algorithm tests covering all flexbox scenarios, matching Yoga test cases for compatibility.

## Todo List
- [ ] Create test fixtures for common layout scenarios
- [ ] Test basic row layout
- [ ] Test basic column layout
- [ ] Test flex-grow distribution
- [ ] Test flex-shrink distribution
- [ ] Test flex-wrap wrapping behavior
- [ ] Test justify-content all values
- [ ] Test align-items all values
- [ ] Test align-content all values
- [ ] Test align-self override
- [ ] Test min/max constraints
- [ ] Test percentage dimensions
- [ ] Test absolute positioning
- [ ] Test nested flex containers
- [ ] Test gap property
- [ ] Test aspect ratio
- [ ] Compare results with Yoga reference implementation

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/LayoutAlgorithm_/

Uses TimeWarp.Fixie conventions:
- Public methods are tests (no attributes needed)
- Use Shouldly assertions
- Port key test cases from Yoga's test suite for algorithmic compatibility

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.LayoutAlgorithm_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class RowLayout_Should_
{
  public static void ArrangeChildrenHorizontally()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(100) };
    FlexNode child2 = new() { Width = FlexValue.Point(100) };
    FlexNode child3 = new() { Width = FlexValue.Point(100) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(300, 100);
    
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(100);
    child3.Layout.Left.ShouldBe(200);
  }
}

[TestTag(TestTags.Fast)]
public class FlexGrow_Should_
{
  public static void DistributeRemainingSpaceProportionally()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { FlexGrow = 1 };
    FlexNode child2 = new() { FlexGrow = 2 };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(300, float.NaN);
    
    child1.Layout.Width.ShouldBe(100);  // 1/3 of 300
    child2.Layout.Width.ShouldBe(200);  // 2/3 of 300
  }
}

[TestTag(TestTags.Fast)]
public class FlexShrink_Should_
{
  public static void ShrinkChildrenWhenOverflowing()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(150), FlexShrink = 1 };
    FlexNode child2 = new() { Width = FlexValue.Point(150), FlexShrink = 1 };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(200, float.NaN);
    
    child1.Layout.Width.ShouldBe(100);  // Shrunk equally
    child2.Layout.Width.ShouldBe(100);
  }
}

[TestTag(TestTags.Fast)]
public class JustifyContent_Should_
{
  [Input(JustifyContent.FlexStart, 0f)]
  [Input(JustifyContent.FlexEnd, 100f)]
  [Input(JustifyContent.Center, 50f)]
  public static void PositionChildCorrectly(JustifyContent justify, float expectedLeft)
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row,
      JustifyContent = justify
    };
    
    FlexNode child = new() { Width = FlexValue.Point(100) };
    root.AddChild(child);
    
    root.CalculateLayout(200, float.NaN);
    
    child.Layout.Left.ShouldBe(expectedLeft);
  }
}
```

Reference Yoga's gentest fixtures for comprehensive coverage.

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
