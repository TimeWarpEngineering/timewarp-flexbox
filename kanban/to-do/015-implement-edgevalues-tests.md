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
Test file: test/TimeWarp.Flexbox.Tests/Values/EdgeValuesTests.cs

Example tests:
```csharp
public class EdgeValuesTests
{
  [Fact]
  public void SetAll_ShouldAffectAllEdges()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    
    edges.ComputedLeft.Should().Be(FlexValue.Point(10));
    edges.ComputedRight.Should().Be(FlexValue.Point(10));
    edges.ComputedTop.Should().Be(FlexValue.Point(10));
    edges.ComputedBottom.Should().Be(FlexValue.Point(10));
  }
  
  [Fact]
  public void SpecificEdge_ShouldOverrideAll()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    edges[Edge.Left] = FlexValue.Point(20);
    
    edges.ComputedLeft.Should().Be(FlexValue.Point(20));
    edges.ComputedRight.Should().Be(FlexValue.Point(10));
  }
  
  [Fact]
  public void Horizontal_ShouldOverrideAll_ButNotSpecific()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    edges[Edge.Horizontal] = FlexValue.Point(20);
    edges[Edge.Left] = FlexValue.Point(30);
    
    edges.ComputedLeft.Should().Be(FlexValue.Point(30));  // Specific wins
    edges.ComputedRight.Should().Be(FlexValue.Point(20)); // Horizontal wins over All
    edges.ComputedTop.Should().Be(FlexValue.Point(10));   // All
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
