# Task 046-implement-tree-mutation-tests

## Summary
Implement tests for tree modification operations including adding, removing, inserting, and reordering children. Also covers node cloning functionality. These tests ensure the tree structure can be safely modified without corrupting layout state.

## Todo List
- [ ] Test AddChild appends to end
- [ ] Test InsertChild at specific index
- [ ] Test RemoveChild removes and updates indices
- [ ] Test RemoveAllChildren clears children
- [ ] Test ReplaceChild swaps nodes
- [ ] Test reordering children with InsertChild
- [ ] Test Clone creates deep copy
- [ ] Test Clone preserves styles
- [ ] Test Clone does not share layout state
- [ ] Test Clone with MeasureFunc handling
- [ ] Test parent reference updated on add/remove
- [ ] Test child count accuracy after mutations

## Notes
Test file: test/TimeWarp.Flexbox.Tests/TreeMutation/

Reference: 
- yoga/tests/YGTreeMutationTest.cpp
- yoga/tests/YGCloneNodeTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.TreeMutation.ChildManagement_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class AddChild_Should_
{
  public static void AppendToEnd()
  {
    FlexNode root = new();
    FlexNode child1 = new();
    FlexNode child2 = new();
    FlexNode child3 = new();
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.ChildCount.ShouldBe(3);
    root.GetChild(0).ShouldBeSameAs(child1);
    root.GetChild(1).ShouldBeSameAs(child2);
    root.GetChild(2).ShouldBeSameAs(child3);
  }
  
  public static void SetParentReference()
  {
    FlexNode root = new();
    FlexNode child = new();
    
    child.Parent.ShouldBeNull();
    
    root.AddChild(child);
    
    child.Parent.ShouldBeSameAs(root);
  }
}

[TestTag(TestTags.Fast)]
public class InsertChild_Should_
{
  public static void InsertAtSpecificIndex()
  {
    FlexNode root = new();
    FlexNode child1 = new();
    FlexNode child2 = new();
    FlexNode child3 = new();
    
    root.AddChild(child1);
    root.AddChild(child3);
    root.InsertChild(child2, 1);  // Insert between 1 and 3
    
    root.GetChild(0).ShouldBeSameAs(child1);
    root.GetChild(1).ShouldBeSameAs(child2);
    root.GetChild(2).ShouldBeSameAs(child3);
  }
  
  public static void HandleIndexZero()
  {
    FlexNode root = new();
    FlexNode child1 = new();
    FlexNode child2 = new();
    
    root.AddChild(child2);
    root.InsertChild(child1, 0);  // Insert at beginning
    
    root.GetChild(0).ShouldBeSameAs(child1);
    root.GetChild(1).ShouldBeSameAs(child2);
  }
}

[TestTag(TestTags.Fast)]
public class RemoveChild_Should_
{
  public static void RemoveAndUpdateIndices()
  {
    FlexNode root = new();
    FlexNode child1 = new();
    FlexNode child2 = new();
    FlexNode child3 = new();
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    root.RemoveChild(child2);
    
    root.ChildCount.ShouldBe(2);
    root.GetChild(0).ShouldBeSameAs(child1);
    root.GetChild(1).ShouldBeSameAs(child3);
  }
  
  public static void ClearParentReference()
  {
    FlexNode root = new();
    FlexNode child = new();
    
    root.AddChild(child);
    child.Parent.ShouldBeSameAs(root);
    
    root.RemoveChild(child);
    
    child.Parent.ShouldBeNull();
  }
}

[TestTag(TestTags.Fast)]
public class RemoveAllChildren_Should_
{
  public static void ClearAllChildren()
  {
    FlexNode root = new();
    root.AddChild(new FlexNode());
    root.AddChild(new FlexNode());
    root.AddChild(new FlexNode());
    
    root.RemoveAllChildren();
    
    root.ChildCount.ShouldBe(0);
  }
}

[TestTag(TestTags.Fast)]
public class Clone_Should_
{
  public static void CreateDeepCopy()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    
    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      FlexGrow = 1
    };
    
    root.AddChild(child);
    
    FlexNode clonedRoot = root.Clone();
    
    clonedRoot.ShouldNotBeSameAs(root);
    clonedRoot.GetChild(0).ShouldNotBeSameAs(child);
    clonedRoot.ChildCount.ShouldBe(1);
  }
  
  public static void PreserveStyles()
  {
    FlexNode original = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row,
      JustifyContent = JustifyContent.Center,
      FlexGrow = 2
    };
    
    FlexNode cloned = original.Clone();
    
    cloned.Width.ShouldBe(FlexValue.Point(100));
    cloned.Height.ShouldBe(FlexValue.Point(200));
    cloned.FlexDirection.ShouldBe(FlexDirection.Row);
    cloned.JustifyContent.ShouldBe(JustifyContent.Center);
    cloned.FlexGrow.ShouldBe(2);
  }
  
  public static void NotShareLayoutState()
  {
    FlexNode original = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    
    original.CalculateLayout(100, 100);
    
    FlexNode cloned = original.Clone();
    
    // Modify original layout
    original.Width = FlexValue.Point(200);
    original.CalculateLayout(200, 100);
    
    // Clone should be independent
    cloned.Layout.Width.ShouldBe(100);
  }
}

[TestTag(TestTags.Fast)]
public class Reordering_Should_
{
  public static void WorkViaRemoveAndInsert()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(30) };
    FlexNode child3 = new() { Width = FlexValue.Point(30) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    
    // Move child3 to front
    root.RemoveChild(child3);
    root.InsertChild(child3, 0);
    
    root.GetChild(0).ShouldBeSameAs(child3);
    root.GetChild(1).ShouldBeSameAs(child1);
    root.GetChild(2).ShouldBeSameAs(child2);
    
    root.CalculateLayout(100, float.NaN);
    
    child3.Layout.Left.ShouldBe(0);
    child1.Layout.Left.ShouldBe(30);
    child2.Layout.Left.ShouldBe(60);
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
