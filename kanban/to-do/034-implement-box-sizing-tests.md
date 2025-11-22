# Task 034-implement-box-sizing-tests

## Summary
Implement tests for box-sizing behavior (content-box vs border-box). Box-sizing determines whether width/height include padding and border or only content. This is critical for CSS compatibility and predictable sizing.

## Todo List
- [ ] Test default box-sizing behavior (border-box for flexbox)
- [ ] Test content-box includes only content in width/height
- [ ] Test border-box includes padding in width/height
- [ ] Test border-box includes border in width/height
- [ ] Test box-sizing with percentage dimensions
- [ ] Test box-sizing with min/max constraints
- [ ] Test box-sizing inheritance/override
- [ ] Test box-sizing with flex-grow
- [ ] Test box-sizing with flex-shrink
- [ ] Test box-sizing with aspect ratio

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Layout/BoxSizing_/

Reference: yoga/tests/generated/YGBoxSizingTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Layout.BoxSizing_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class BorderBox_Should_
{
  public static void IncludePaddingInSpecifiedWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BoxSizing = BoxSizing.BorderBox,
      PaddingLeft = FlexValue.Point(10),
      PaddingRight = FlexValue.Point(10)
    };
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    // Total width is 100, content area is 80
    root.Layout.Width.ShouldBe(100);
  }
  
  public static void IncludeBorderInSpecifiedWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BoxSizing = BoxSizing.BorderBox,
      BorderLeft = 10,
      BorderRight = 10
    };
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    // Total width is 100 including borders
    root.Layout.Width.ShouldBe(100);
  }
}

[TestTag(TestTags.Fast)]
public class ContentBox_Should_
{
  public static void AddPaddingToSpecifiedWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BoxSizing = BoxSizing.ContentBox,
      PaddingLeft = FlexValue.Point(10),
      PaddingRight = FlexValue.Point(10)
    };
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    // Content is 100, total is 120 with padding
    root.Layout.Width.ShouldBe(120);
  }
  
  public static void AddBorderToSpecifiedWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BoxSizing = BoxSizing.ContentBox,
      BorderLeft = 10,
      BorderRight = 10
    };
    
    root.CalculateLayout(float.NaN, float.NaN);
    
    // Content is 100, total is 120 with borders
    root.Layout.Width.ShouldBe(120);
  }
}

[TestTag(TestTags.Fast)]
public class BoxSizingWithFlexGrow_Should_
{
  public static void CalculateGrowCorrectlyWithBorderBox()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child = new() 
    { 
      FlexGrow = 1,
      BoxSizing = BoxSizing.BorderBox,
      PaddingLeft = FlexValue.Point(10),
      PaddingRight = FlexValue.Point(10)
    };
    
    root.AddChild(child);
    
    root.CalculateLayout(200, float.NaN);
    
    // Child fills 200, padding is included in that
    child.Layout.Width.ShouldBe(200);
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
