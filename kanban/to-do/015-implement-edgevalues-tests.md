# Task 015-implement-edgevalues-tests

## Summary
Create unit tests for EdgeValues<T> struct covering cascade resolution, edge indexer, and computed values.

## Todo List
- [ ] Test setting individual edges (Left, Top, Right, Bottom)
- [ ] Test Edge.All sets all edges
- [ ] Test Edge.Horizontal sets Left and Right
- [ ] Test Edge.Vertical sets Top and Bottom
- [ ] Test cascade precedence (specific overrides general)
- [ ] Test Start/End edges (for RTL support)
- [ ] Test computed getters return correct resolved values
- [ ] Test default values when edges not explicitly set
- [ ] Test with both FlexValue and float type parameters

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Values/EdgeValues_/Indexer_Should_.cs

Uses TimeWarp.Fixie conventions:
- Public methods are tests (no attributes needed)
- Use Shouldly assertions

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Values.EdgeValues_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class Indexer_Should_
{
  public static void SetAllEdgesWhenUsingEdgeAll()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    
    edges.ComputedLeft.ShouldBe(FlexValue.Point(10));
    edges.ComputedRight.ShouldBe(FlexValue.Point(10));
    edges.ComputedTop.ShouldBe(FlexValue.Point(10));
    edges.ComputedBottom.ShouldBe(FlexValue.Point(10));
  }
  
  public static void AllowSpecificEdgeToOverrideAll()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    edges[Edge.Left] = FlexValue.Point(20);
    
    edges.ComputedLeft.ShouldBe(FlexValue.Point(20));
    edges.ComputedRight.ShouldBe(FlexValue.Point(10));
  }
}

[TestTag(TestTags.Fast)]
public class Cascade_Should_
{
  public static void ResolveHorizontalOverAllButNotSpecific()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    edges[Edge.Horizontal] = FlexValue.Point(20);
    edges[Edge.Left] = FlexValue.Point(30);
    
    edges.ComputedLeft.ShouldBe(FlexValue.Point(30));   // Specific wins
    edges.ComputedRight.ShouldBe(FlexValue.Point(20));  // Horizontal wins over All
    edges.ComputedTop.ShouldBe(FlexValue.Point(10));    // All
  }
  
  public static void ResolveVerticalOverAll()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    edges[Edge.Vertical] = FlexValue.Point(20);
    
    edges.ComputedTop.ShouldBe(FlexValue.Point(20));
    edges.ComputedBottom.ShouldBe(FlexValue.Point(20));
    edges.ComputedLeft.ShouldBe(FlexValue.Point(10));
    edges.ComputedRight.ShouldBe(FlexValue.Point(10));
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
