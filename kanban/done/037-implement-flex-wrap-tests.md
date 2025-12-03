# Task 037-implement-flex-wrap-tests

## Summary
Implement comprehensive tests for flex-wrap behavior covering wrap, wrap-reverse, and nowrap modes. Tests verify line creation, item placement on lines, and interaction with other flex properties.

## Todo List
- [ ] Test flex-wrap: nowrap (default, single line overflow)
- [ ] Test flex-wrap: wrap creates multiple lines
- [ ] Test flex-wrap: wrap-reverse reverses line order
- [ ] Test wrap with explicit item sizes
- [ ] Test wrap with flex-grow items
- [ ] Test wrap with flex-shrink items
- [ ] Test wrap with min/max constraints
- [ ] Test wrap with different main-axis sizes
- [ ] Test wrap preserves item order within lines
- [ ] Test wrap with gap property
- [ ] Test wrap in column direction
- [ ] Test wrap-reverse in column direction

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/FlexWrap_/

Reference: yoga/tests/generated/YGFlexWrapTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.FlexWrap_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class NoWrap_Should_
{
  public static void KeepAllItemsOnSingleLine()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.NoWrap
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(100, float.NaN);
    
    // All on same line (top = 0), overflow allowed
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
    child3.Layout.Top.ShouldBe(0);
  }
}

[TestTag(TestTags.Fast)]
public class Wrap_Should_
{
  public static void CreateMultipleLinesWhenNeeded()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(100, float.NaN);
    
    // Line 1: child1, child2
    child1.Layout.Top.ShouldBe(0);
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
    child2.Layout.Left.ShouldBe(50);
    // Line 2: child3
    child3.Layout.Top.ShouldBe(20);
    child3.Layout.Left.ShouldBe(0);
  }
  
  public static void PreserveItemOrderWithinLines()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(60),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };
    FlexNode child3 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };
    FlexNode child4 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    root.AddChild(child4);
    
    root.CalculateLayout(60, float.NaN);
    
    // Line 1: 1, 2 - Line 2: 3, 4
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(30);
    child3.Layout.Left.ShouldBe(0);
    child4.Layout.Left.ShouldBe(30);
  }
}

[TestTag(TestTags.Fast)]
public class WrapReverse_Should_
{
  public static void ReverseCrossAxisLineOrder()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.WrapReverse
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(100, 100);
    
    // Lines reversed: Line 2 at top, Line 1 at bottom
    child3.Layout.Top.ShouldBe(0);   // Line 2 (last) now first
    child1.Layout.Top.ShouldBe(20);  // Line 1 (first) now second
    child2.Layout.Top.ShouldBe(20);
  }
}

[TestTag(TestTags.Fast)]
public class WrapWithGap_Should_
{
  public static void ApplyGapBetweenLines()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      RowGap = 10
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.CalculateLayout(100, float.NaN);
    
    // Line 2 offset by line height + gap
    child3.Layout.Top.ShouldBe(30);  // 20 + 10 gap
  }
}
```

## Results
Completed: 2025-12-03

Added 9 flex wrap tests covering:
- FlexWrap.NoWrap keeps items on single line (2 tests)
- FlexWrap.Wrap creates multiple lines (3 tests)
- FlexWrap.WrapReverse reverses line order (1 test)
- FlexWrap with RowGap and ColumnGap (2 tests)
- FlexWrap in column direction (1 test)

Test results: 339 passed, 0 failed
