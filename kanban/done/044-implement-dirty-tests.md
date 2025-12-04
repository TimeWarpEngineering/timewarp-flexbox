# Task 044-implement-dirty-tests

## Summary
Implement tests for dirty flag behavior including dirtied callback functionality and dirty flag propagation through the node tree. The dirty system is essential for incremental layout optimization, avoiding unnecessary recalculation of unchanged subtrees.

## Todo List
- [x] Test node starts clean after layout
- [x] Test modifying style property marks node dirty
- [x] Test dirty propagates to parent nodes
- [x] Test dirty does not propagate to siblings
- [x] Test dirty does not propagate to children
- [x] Test DirtiedFunc callback is invoked
- [x] Test DirtiedFunc receives correct node
- [x] Test multiple changes trigger single callback
- [x] Test clearing dirty flag
- [x] Test IsDirty property accuracy
- [x] Test adding/removing children marks dirty
- [x] Test layout clears dirty flag

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Dirty/

Reference: 
- yoga/tests/YGDirtiedTest.cpp
- yoga/tests/YGDirtyMarkingTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Dirty.DirtyFlag_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class AfterLayout_Should_
{
  public static void BeClean()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    
    root.CalculateLayout(100, 100);
    
    root.IsDirty.ShouldBeFalse();
  }
}

[TestTag(TestTags.Fast)]
public class StyleChange_Should_
{
  public static void MarkNodeDirty()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    
    root.CalculateLayout(100, 100);
    root.IsDirty.ShouldBeFalse();
    
    // Change a style property
    root.Width = FlexValue.Point(200);
    
    root.IsDirty.ShouldBeTrue();
  }
  
  public static void PropagateToParent()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    
    FlexNode child = new() 
    { 
      Width = FlexValue.Point(50), 
      Height = FlexValue.Point(50)
    };
    
    root.AddChild(child);
    root.CalculateLayout(100, 100);
    
    // Change child's style
    child.Width = FlexValue.Point(60);
    
    root.IsDirty.ShouldBeTrue();
    child.IsDirty.ShouldBeTrue();
  }
  
  public static void NotPropagateToSiblings()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };
    
    FlexNode child1 = new() { Width = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50) };
    
    root.AddChild(child1);
    root.AddChild(child2);
    root.CalculateLayout(100, float.NaN);
    
    // Change child1's style
    child1.Width = FlexValue.Point(60);
    
    child1.IsDirty.ShouldBeTrue();
    child2.IsDirty.ShouldBeFalse();  // Sibling unchanged
  }
}

[TestTag(TestTags.Fast)]
public class DirtiedCallback_Should_
{
  public static void BeInvokedOnDirty()
  {
    bool callbackInvoked = false;
    FlexNode dirtiedNode = null!;
    
    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };
    root.DirtiedFunc = (node) =>
    {
      callbackInvoked = true;
      dirtiedNode = node;
    };
    
    root.CalculateLayout(100, float.NaN);
    
    root.Width = FlexValue.Point(200);
    
    callbackInvoked.ShouldBeTrue();
    dirtiedNode.ShouldBeSameAs(root);
  }
  
  public static void OnlyBeCalledOnce()
  {
    int callCount = 0;
    
    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };
    root.DirtiedFunc = (node) => callCount++;
    
    root.CalculateLayout(100, float.NaN);
    
    // Multiple changes
    root.Width = FlexValue.Point(200);
    root.Height = FlexValue.Point(200);
    root.FlexDirection = FlexDirection.Row;
    
    // Should only be called once (first time it becomes dirty)
    callCount.ShouldBe(1);
  }
}

[TestTag(TestTags.Fast)]
public class AddingChild_Should_
{
  public static void MarkParentDirty()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };
    
    root.CalculateLayout(100, float.NaN);
    root.IsDirty.ShouldBeFalse();
    
    FlexNode child = new() { Width = FlexValue.Point(50) };
    root.AddChild(child);
    
    root.IsDirty.ShouldBeTrue();
  }
}

[TestTag(TestTags.Fast)]
public class RemovingChild_Should_
{
  public static void MarkParentDirty()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };
    
    FlexNode child = new() { Width = FlexValue.Point(50) };
    root.AddChild(child);
    root.CalculateLayout(100, float.NaN);
    root.IsDirty.ShouldBeFalse();
    
    root.RemoveChild(child);
    
    root.IsDirty.ShouldBeTrue();
  }
}
```

## Results
- **Implementation**: Added `DirtiedFunc` property (`Action<FlexNode>?`) to FlexNode and updated `MarkDirty()` to invoke callback
- **Tests added**: 8 new tests in `FlexNodeDirtiedCallbackTests` class (test/timewarp-flexbox-tests/nodes/flex-node-tests.cs)
- **Total tests**: 400 passing (up from 392)
- **Deviation**: Tests added to existing `flex-node-tests.cs` file rather than creating separate Dirty/ directory, maintaining consistency with existing test organization
- **Pre-existing tests**: Many dirty flag tests already existed in `FlexNodeDirtyTrackingTests` class covering style changes, propagation, and basic dirty flag behavior
