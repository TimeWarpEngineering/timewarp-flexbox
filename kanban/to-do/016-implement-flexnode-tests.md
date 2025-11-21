# Task 016-implement-flexnode-tests

## Summary
Create unit tests for FlexNode class covering tree operations, property defaults, and dirty tracking.

## Todo List
- [ ] Test FlexNode constructor creates valid node with defaults
- [ ] Test AddChild adds child and sets parent
- [ ] Test AddChild removes child from previous parent
- [ ] Test RemoveChild clears parent reference
- [ ] Test RemoveAllChildren clears all children
- [ ] Test InsertChild at specific index
- [ ] Test GetChild returns correct child by index
- [ ] Test index out of bounds throws exception
- [ ] Test MarkDirty sets IsDirty flag
- [ ] Test property changes mark node as dirty
- [ ] Test all default property values match CSS spec
- [ ] Test Children list is read-only (cannot modify externally)

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Nodes/FlexNodeTests.cs

Example tests:
```csharp
public class FlexNodeTests
{
  [Fact]
  public void Constructor_ShouldCreateNodeWithDefaults()
  {
    FlexNode node = new();
    
    node.Parent.Should().BeNull();
    node.Children.Should().BeEmpty();
    node.FlexDirection.Should().Be(FlexDirection.Row);
    node.FlexWrap.Should().Be(FlexWrap.NoWrap);
    node.JustifyContent.Should().Be(JustifyContent.FlexStart);
    node.AlignItems.Should().Be(AlignItems.Stretch);
    node.FlexGrow.Should().Be(0f);
    node.FlexShrink.Should().Be(1f);
  }
  
  [Fact]
  public void AddChild_ShouldSetParentReference()
  {
    FlexNode parent = new();
    FlexNode child = new();
    
    parent.AddChild(child);
    
    child.Parent.Should().Be(parent);
    parent.Children.Should().Contain(child);
  }
  
  [Fact]
  public void AddChild_ShouldRemoveFromPreviousParent()
  {
    FlexNode oldParent = new();
    FlexNode newParent = new();
    FlexNode child = new();
    
    oldParent.AddChild(child);
    newParent.AddChild(child);
    
    oldParent.Children.Should().NotContain(child);
    newParent.Children.Should().Contain(child);
    child.Parent.Should().Be(newParent);
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
