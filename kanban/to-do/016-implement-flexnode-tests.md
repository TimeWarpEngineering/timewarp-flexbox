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
Test file: test/TimeWarp.Flexbox.Tests/Nodes/FlexNode_/Constructor_Should_.cs

Uses TimeWarp.Fixie conventions:
- Public methods are tests (no attributes needed)
- Use Shouldly assertions

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Nodes.FlexNode_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class Constructor_Should_
{
  public static void CreateNodeWithDefaults()
  {
    FlexNode node = new();
    
    node.Parent.ShouldBeNull();
    node.Children.ShouldBeEmpty();
    node.FlexDirection.ShouldBe(FlexDirection.Row);
    node.FlexWrap.ShouldBe(FlexWrap.NoWrap);
    node.JustifyContent.ShouldBe(JustifyContent.FlexStart);
    node.AlignItems.ShouldBe(AlignItems.Stretch);
    node.FlexGrow.ShouldBe(0f);
    node.FlexShrink.ShouldBe(1f);
  }
}

[TestTag(TestTags.Fast)]
public class AddChild_Should_
{
  public static void SetParentReference()
  {
    FlexNode parent = new();
    FlexNode child = new();
    
    parent.AddChild(child);
    
    child.Parent.ShouldBe(parent);
    parent.Children.ShouldContain(child);
  }
  
  public static void RemoveChildFromPreviousParent()
  {
    FlexNode oldParent = new();
    FlexNode newParent = new();
    FlexNode child = new();
    
    oldParent.AddChild(child);
    newParent.AddChild(child);
    
    oldParent.Children.ShouldNotContain(child);
    newParent.Children.ShouldContain(child);
    child.Parent.ShouldBe(newParent);
  }
}

[TestTag(TestTags.Fast)]
public class RemoveChild_Should_
{
  public static void ClearParentReference()
  {
    FlexNode parent = new();
    FlexNode child = new();
    
    parent.AddChild(child);
    parent.RemoveChild(child);
    
    child.Parent.ShouldBeNull();
    parent.Children.ShouldNotContain(child);
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
