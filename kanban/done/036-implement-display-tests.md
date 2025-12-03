# Task 036-implement-display-tests

## Summary
Implement tests for display property behavior, specifically display: none vs display: flex. Elements with display: none should be excluded from layout calculations entirely, taking up no space and not affecting sibling positioning.

## Todo List
- [ ] Test display: none excludes element from layout
- [ ] Test display: none element has zero dimensions
- [ ] Test display: none does not affect sibling positions
- [ ] Test display: flex (default behavior)
- [ ] Test toggling display affects layout
- [ ] Test display: none with flex-grow siblings (space redistribution)
- [ ] Test display: none nested containers
- [ ] Test display: none with absolute positioning
- [ ] Test display: none children not measured
- [ ] Test display: none preserves style properties

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/Display_/

Reference: yoga/tests/generated/YGDisplayTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.Display_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class DisplayNone_Should_
{
  public static void ExcludeElementFromLayout()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(30), Display = Display.None };
    FlexNode child3 = new() { Width = FlexValue.Point(30) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(100, float.NaN);
    
    child1.Layout.Left.ShouldBe(0);
    // child2 skipped, child3 comes right after child1
    child3.Layout.Left.ShouldBe(30);
  }
  
  public static void HaveZeroDimensions()
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
      Display = Display.None
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(100, 100);
    
    child.Layout.Width.ShouldBe(0);
    child.Layout.Height.ShouldBe(0);
  }
  
  public static void NotAffectParentSizing()
  {
    FlexNode root = new();
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(100), 
      Height = FlexValue.Point(100),
      Display = Display.None
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    // Parent sizes to zero, ignoring hidden child
    root.Layout.Width.ShouldBe(0);
    root.Layout.Height.ShouldBe(0);
  }
}

[TestTag(TestTags.Fast)]
public class DisplayFlex_Should_
{
  public static void IncludeElementInLayout()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(30), Display = Display.Flex };
    FlexNode child2 = new() { Width = FlexValue.Point(30), Display = Display.Flex };
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    root.CalculateLayout(100, float.NaN);
    
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(30);
  }
}

[TestTag(TestTags.Fast)]
public class DisplayNoneWithFlexGrow_Should_
{
  public static void RedistributeSpaceToVisibleSiblings()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { FlexGrow = 1 };
    FlexNode child2 = new() { FlexGrow = 1, Display = Display.None };
    FlexNode child3 = new() { FlexGrow = 1 };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(100, float.NaN);
    
    // Space split between two visible children
    child1.Layout.Width.ShouldBe(50);
    child2.Layout.Width.ShouldBe(0);
    child3.Layout.Width.ShouldBe(50);
  }
}

[TestTag(TestTags.Fast)]
public class DisplayNoneNested_Should_
{
  public static void HideEntireSubtree()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };
    
    FlexNode parent = new() 
    { 
      Width = FlexValue.Point(50),
      Display = Display.None
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(30), 
      Height = FlexValue.Point(30)
    };
    
    parent.AddChild(child);
    root.AddChild(parent);
    
    root.CalculateLayout(100, float.NaN);
    
    parent.Layout.Width.ShouldBe(0);
    child.Layout.Width.ShouldBe(0);
  }
}
```

## Results
Completed: 2025-12-03

Added 9 display tests covering:
- Display.None excludes element from layout (3 tests)
- Display.Flex includes element and is default (2 tests)
- Display.None with flex-grow redistributes space (1 test)
- Display.None hides entire subtree (2 tests)
- Display.None with absolute positioning (1 test)

Test results: 330 passed, 0 failed
