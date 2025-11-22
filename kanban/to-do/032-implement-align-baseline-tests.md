# Task 032-implement-align-baseline-tests

## Summary
Implement tests for baseline alignment behavior, covering align-items: baseline and align-self: baseline scenarios. Baseline alignment aligns flex items along their text baselines, which is critical for UI layouts with text elements of different sizes.

## Todo List
- [ ] Test align-items: baseline with children of different heights
- [ ] Test align-items: baseline with custom baseline from BaselineFunc
- [ ] Test align-self: baseline overriding parent's align-items
- [ ] Test baseline alignment with mixed content (text and boxes)
- [ ] Test baseline alignment in row direction
- [ ] Test baseline alignment in column direction
- [ ] Test baseline with nested flex containers
- [ ] Test first baseline vs last baseline behavior
- [ ] Test baseline when no BaselineFunc is provided (fallback to bottom)
- [ ] Test baseline with padding affecting position

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/AlignBaseline_/

Reference: 
- yoga/tests/generated/YGAlignBaselineTest.cpp
- yoga/tests/YGBaselineFuncTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.AlignBaseline_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class AlignItemsBaseline_Should_
{
  public static void AlignChildrenToFirstChildBaseline()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Baseline
    };
    
    FlexNode child1 = new() 
    { 
      Width = FlexValue.Point(30), 
      Height = FlexValue.Point(30)
    };
    child1.BaselineFunc = (node, width, height) => 25; // Baseline at 25px from top
    
    FlexNode child2 = new() 
    { 
      Width = FlexValue.Point(30), 
      Height = FlexValue.Point(20)
    };
    child2.BaselineFunc = (node, width, height) => 15; // Baseline at 15px from top
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(100, 100);
    
    // child1 baseline at 25, child2 baseline at 15
    // child2 should be offset by 10 to align baselines
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(10);
  }
}

[TestTag(TestTags.Fast)]
public class BaselineFunc_Should_
{
  public static void BeCalledDuringLayout()
  {
    bool baselineCalled = false;
    
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Baseline
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50)
    };
    child.BaselineFunc = (node, width, height) =>
    {
      baselineCalled = true;
      return height * 0.8f; // 80% from top
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, float.NaN);
    
    baselineCalled.ShouldBeTrue();
  }
  
  public static void ReceiveCorrectDimensions()
  {
    float receivedWidth = 0;
    float receivedHeight = 0;
    
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Baseline
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(40)
    };
    child.BaselineFunc = (node, width, height) =>
    {
      receivedWidth = width;
      receivedHeight = height;
      return height;
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, float.NaN);
    
    receivedWidth.ShouldBe(50);
    receivedHeight.ShouldBe(40);
  }
}

[TestTag(TestTags.Fast)]
public class AlignSelfBaseline_Should_
{
  public static void OverrideParentAlignItems()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.FlexStart
    };
    
    FlexNode child1 = new() 
    { 
      Width = FlexValue.Point(30), 
      Height = FlexValue.Point(30)
    };
    child1.BaselineFunc = (node, width, height) => 25;
    
    FlexNode child2 = new() 
    { 
      Width = FlexValue.Point(30), 
      Height = FlexValue.Point(20),
      AlignSelf = AlignSelf.Baseline
    };
    child2.BaselineFunc = (node, width, height) => 15;
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(100, 100);
    
    // Only child2 uses baseline alignment
    child1.Layout.Top.ShouldBe(0);
    // child2 aligns to child1's baseline
    child2.Layout.Top.ShouldBe(10);
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
