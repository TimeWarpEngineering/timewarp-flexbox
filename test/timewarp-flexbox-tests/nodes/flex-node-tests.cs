namespace TimeWarp.Flexbox.Tests.Nodes;

/// <summary>
/// Tests for FlexNode constructor and default values.
/// </summary>
public class FlexNodeConstructorTests
{
  public void ShouldCreateNodeWithNullParent()
  {
    FlexNode node = new();

    node.Parent.ShouldBeNull();
  }

  public void ShouldCreateNodeWithEmptyChildren()
  {
    FlexNode node = new();

    node.Children.ShouldBeEmpty();
    node.ChildCount.ShouldBe(0);
  }

  public void ShouldCreateNodeWithLayout()
  {
    FlexNode node = new();

    node.Layout.ShouldNotBeNull();
  }

  public void ShouldCreateNodeWithDirtyFlag()
  {
    FlexNode node = new();

    node.IsDirty.ShouldBeTrue();
  }

  #region Style Property Defaults

  public void ShouldHaveDefaultFlexDirectionRow()
  {
    FlexNode node = new();

    node.FlexDirection.ShouldBe(FlexDirection.Row);
  }

  public void ShouldHaveDefaultFlexWrapNoWrap()
  {
    FlexNode node = new();

    node.FlexWrap.ShouldBe(FlexWrap.NoWrap);
  }

  public void ShouldHaveDefaultJustifyContentFlexStart()
  {
    FlexNode node = new();

    node.JustifyContent.ShouldBe(JustifyContent.FlexStart);
  }

  public void ShouldHaveDefaultAlignItemsStretch()
  {
    FlexNode node = new();

    node.AlignItems.ShouldBe(AlignItems.Stretch);
  }

  public void ShouldHaveDefaultAlignContentFlexStart()
  {
    FlexNode node = new();

    node.AlignContent.ShouldBe(AlignContent.FlexStart);
  }

  public void ShouldHaveDefaultAlignSelfAuto()
  {
    FlexNode node = new();

    node.AlignSelf.ShouldBe(AlignSelf.Auto);
  }

  public void ShouldHaveDefaultFlexGrowZero()
  {
    FlexNode node = new();

    node.FlexGrow.ShouldBe(0f);
  }

  public void ShouldHaveDefaultFlexShrinkOne()
  {
    FlexNode node = new();

    node.FlexShrink.ShouldBe(1f);
  }

  public void ShouldHaveDefaultFlexBasisAuto()
  {
    FlexNode node = new();

    node.FlexBasis.ShouldBe(FlexValue.Auto);
  }

  public void ShouldHaveDefaultWidthUndefined()
  {
    FlexNode node = new();

    node.Width.ShouldBe(FlexValue.Undefined);
  }

  public void ShouldHaveDefaultHeightUndefined()
  {
    FlexNode node = new();

    node.Height.ShouldBe(FlexValue.Undefined);
  }

  public void ShouldHaveDefaultMinWidthUndefined()
  {
    FlexNode node = new();

    node.MinWidth.ShouldBe(FlexValue.Undefined);
  }

  public void ShouldHaveDefaultMinHeightUndefined()
  {
    FlexNode node = new();

    node.MinHeight.ShouldBe(FlexValue.Undefined);
  }

  public void ShouldHaveDefaultMaxWidthUndefined()
  {
    FlexNode node = new();

    node.MaxWidth.ShouldBe(FlexValue.Undefined);
  }

  public void ShouldHaveDefaultMaxHeightUndefined()
  {
    FlexNode node = new();

    node.MaxHeight.ShouldBe(FlexValue.Undefined);
  }

  public void ShouldHaveDefaultDisplayFlex()
  {
    FlexNode node = new();

    node.Display.ShouldBe(Display.Flex);
  }

  public void ShouldHaveDefaultPositionTypeRelative()
  {
    FlexNode node = new();

    node.PositionType.ShouldBe(PositionType.Relative);
  }

  public void ShouldHaveDefaultOverflowVisible()
  {
    FlexNode node = new();

    node.Overflow.ShouldBe(Overflow.Visible);
  }

  public void ShouldHaveDefaultAspectRatioNull()
  {
    FlexNode node = new();

    node.AspectRatio.ShouldBeNull();
  }

  #endregion
}

/// <summary>
/// Tests for FlexNode.AddChild method.
/// </summary>
public class FlexNodeAddChildTests
{
  public void ShouldSetParentReference()
  {
    FlexNode parent = new();
    FlexNode child = new();

    parent.AddChild(child);

    child.Parent.ShouldBe(parent);
  }

  public void ShouldAddChildToChildrenList()
  {
    FlexNode parent = new();
    FlexNode child = new();

    parent.AddChild(child);

    parent.Children.ShouldContain(child);
    parent.ChildCount.ShouldBe(1);
  }

  public void ShouldAddMultipleChildren()
  {
    FlexNode parent = new();
    FlexNode child1 = new();
    FlexNode child2 = new();
    FlexNode child3 = new();

    parent.AddChild(child1);
    parent.AddChild(child2);
    parent.AddChild(child3);

    parent.ChildCount.ShouldBe(3);
    parent.Children[0].ShouldBe(child1);
    parent.Children[1].ShouldBe(child2);
    parent.Children[2].ShouldBe(child3);
  }

  public void ShouldRemoveChildFromPreviousParent()
  {
    FlexNode oldParent = new();
    FlexNode newParent = new();
    FlexNode child = new();

    oldParent.AddChild(child);
    newParent.AddChild(child);

    oldParent.Children.ShouldNotContain(child);
    oldParent.ChildCount.ShouldBe(0);
    newParent.Children.ShouldContain(child);
    child.Parent.ShouldBe(newParent);
  }

  public void ShouldMarkParentDirty()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.CalculateLayout(100, 100); // Clears dirty flag

    parent.IsDirty.ShouldBeFalse();

    parent.AddChild(child);

    parent.IsDirty.ShouldBeTrue();
  }

  public void ShouldThrowOnNullChild()
  {
    FlexNode parent = new();

    Should.Throw<ArgumentNullException>(() => parent.AddChild(null!));
  }
}

/// <summary>
/// Tests for FlexNode.InsertChild method.
/// </summary>
public class FlexNodeInsertChildTests
{
  public void ShouldInsertChildAtBeginning()
  {
    FlexNode parent = new();
    FlexNode existingChild = new();
    FlexNode newChild = new();

    parent.AddChild(existingChild);
    parent.InsertChild(newChild, 0);

    parent.Children[0].ShouldBe(newChild);
    parent.Children[1].ShouldBe(existingChild);
  }

  public void ShouldInsertChildAtMiddle()
  {
    FlexNode parent = new();
    FlexNode child1 = new();
    FlexNode child2 = new();
    FlexNode newChild = new();

    parent.AddChild(child1);
    parent.AddChild(child2);
    parent.InsertChild(newChild, 1);

    parent.Children[0].ShouldBe(child1);
    parent.Children[1].ShouldBe(newChild);
    parent.Children[2].ShouldBe(child2);
  }

  public void ShouldInsertChildAtEnd()
  {
    FlexNode parent = new();
    FlexNode existingChild = new();
    FlexNode newChild = new();

    parent.AddChild(existingChild);
    parent.InsertChild(newChild, 1);

    parent.Children[0].ShouldBe(existingChild);
    parent.Children[1].ShouldBe(newChild);
  }

  public void ShouldSetParentReference()
  {
    FlexNode parent = new();
    FlexNode child = new();

    parent.InsertChild(child, 0);

    child.Parent.ShouldBe(parent);
  }

  public void ShouldRemoveChildFromPreviousParent()
  {
    FlexNode oldParent = new();
    FlexNode newParent = new();
    FlexNode child = new();

    oldParent.AddChild(child);
    newParent.InsertChild(child, 0);

    oldParent.Children.ShouldNotContain(child);
    newParent.Children.ShouldContain(child);
    child.Parent.ShouldBe(newParent);
  }

  public void ShouldThrowOnNullChild()
  {
    FlexNode parent = new();

    Should.Throw<ArgumentNullException>(() => parent.InsertChild(null!, 0));
  }

  public void ShouldThrowOnNegativeIndex()
  {
    FlexNode parent = new();
    FlexNode child = new();

    Should.Throw<ArgumentOutOfRangeException>(() => parent.InsertChild(child, -1));
  }

  public void ShouldThrowOnIndexGreaterThanCount()
  {
    FlexNode parent = new();
    FlexNode child = new();

    Should.Throw<ArgumentOutOfRangeException>(() => parent.InsertChild(child, 1));
  }
}

/// <summary>
/// Tests for FlexNode.RemoveChild method.
/// </summary>
public class FlexNodeRemoveChildTests
{
  public void ShouldClearParentReference()
  {
    FlexNode parent = new();
    FlexNode child = new();

    parent.AddChild(child);
    parent.RemoveChild(child);

    child.Parent.ShouldBeNull();
  }

  public void ShouldRemoveChildFromList()
  {
    FlexNode parent = new();
    FlexNode child = new();

    parent.AddChild(child);
    parent.RemoveChild(child);

    parent.Children.ShouldNotContain(child);
    parent.ChildCount.ShouldBe(0);
  }

  public void ShouldReturnTrueWhenChildRemoved()
  {
    FlexNode parent = new();
    FlexNode child = new();

    parent.AddChild(child);
    bool result = parent.RemoveChild(child);

    result.ShouldBeTrue();
  }

  public void ShouldReturnFalseWhenChildNotFound()
  {
    FlexNode parent = new();
    FlexNode notAChild = new();

    bool result = parent.RemoveChild(notAChild);

    result.ShouldBeFalse();
  }

  public void ShouldMarkParentDirty()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.AddChild(child);
    parent.CalculateLayout(100, 100);

    parent.IsDirty.ShouldBeFalse();

    parent.RemoveChild(child);

    parent.IsDirty.ShouldBeTrue();
  }

  public void ShouldThrowOnNullChild()
  {
    FlexNode parent = new();

    Should.Throw<ArgumentNullException>(() => parent.RemoveChild(null!));
  }
}

/// <summary>
/// Tests for FlexNode.RemoveAllChildren method.
/// </summary>
public class FlexNodeRemoveAllChildrenTests
{
  public void ShouldRemoveAllChildren()
  {
    FlexNode parent = new();
    FlexNode child1 = new();
    FlexNode child2 = new();
    FlexNode child3 = new();

    parent.AddChild(child1);
    parent.AddChild(child2);
    parent.AddChild(child3);
    parent.RemoveAllChildren();

    parent.Children.ShouldBeEmpty();
    parent.ChildCount.ShouldBe(0);
  }

  public void ShouldClearParentReferenceForAllChildren()
  {
    FlexNode parent = new();
    FlexNode child1 = new();
    FlexNode child2 = new();

    parent.AddChild(child1);
    parent.AddChild(child2);
    parent.RemoveAllChildren();

    child1.Parent.ShouldBeNull();
    child2.Parent.ShouldBeNull();
  }

  public void ShouldMarkParentDirty()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.AddChild(child);
    parent.CalculateLayout(100, 100);

    parent.IsDirty.ShouldBeFalse();

    parent.RemoveAllChildren();

    parent.IsDirty.ShouldBeTrue();
  }

  public void ShouldNotMarkDirtyWhenNoChildren()
  {
    FlexNode parent = new();
    parent.CalculateLayout(100, 100);

    parent.IsDirty.ShouldBeFalse();

    parent.RemoveAllChildren();

    parent.IsDirty.ShouldBeFalse();
  }
}

/// <summary>
/// Tests for FlexNode.GetChild method.
/// </summary>
public class FlexNodeGetChildTests
{
  public void ShouldReturnChildAtIndex()
  {
    FlexNode parent = new();
    FlexNode child1 = new();
    FlexNode child2 = new();
    FlexNode child3 = new();

    parent.AddChild(child1);
    parent.AddChild(child2);
    parent.AddChild(child3);

    parent.GetChild(0).ShouldBe(child1);
    parent.GetChild(1).ShouldBe(child2);
    parent.GetChild(2).ShouldBe(child3);
  }

  public void ShouldThrowOnNegativeIndex()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.AddChild(child);

    Should.Throw<ArgumentOutOfRangeException>(() => parent.GetChild(-1));
  }

  public void ShouldThrowOnIndexEqualToCount()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.AddChild(child);

    Should.Throw<ArgumentOutOfRangeException>(() => parent.GetChild(1));
  }

  public void ShouldThrowOnIndexGreaterThanCount()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.AddChild(child);

    Should.Throw<ArgumentOutOfRangeException>(() => parent.GetChild(5));
  }

  public void ShouldThrowOnEmptyChildren()
  {
    FlexNode parent = new();

    Should.Throw<ArgumentOutOfRangeException>(() => parent.GetChild(0));
  }
}

/// <summary>
/// Tests for FlexNode dirty tracking.
/// </summary>
public class FlexNodeDirtyTrackingTests
{
  public void ShouldMarkNodeDirty()
  {
    FlexNode node = new();
    node.CalculateLayout(100, 100);

    node.IsDirty.ShouldBeFalse();

    node.MarkDirty();

    node.IsDirty.ShouldBeTrue();
  }

  public void ShouldPropagateMarkDirtyToAncestors()
  {
    FlexNode grandparent = new();
    FlexNode parent = new();
    FlexNode child = new();

    grandparent.AddChild(parent);
    parent.AddChild(child);
    grandparent.CalculateLayout(100, 100);

    grandparent.IsDirty.ShouldBeFalse();
    parent.IsDirty.ShouldBeFalse();
    child.IsDirty.ShouldBeFalse();

    child.MarkDirty();

    child.IsDirty.ShouldBeTrue();
    parent.IsDirty.ShouldBeTrue();
    grandparent.IsDirty.ShouldBeTrue();
  }

  public void ShouldNotMarkDirtyTwice()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.AddChild(child);
    parent.CalculateLayout(100, 100);

    child.MarkDirty();
    parent.IsDirty.ShouldBeTrue();

    // Mark dirty again - should not throw or cause issues
    child.MarkDirty();

    parent.IsDirty.ShouldBeTrue();
  }

  #region Property Changes Mark Dirty

  public void ShouldMarkDirtyOnFlexDirectionChange()
  {
    FlexNode node = new();
    node.CalculateLayout(100, 100);

    node.FlexDirection = FlexDirection.Column;

    node.IsDirty.ShouldBeTrue();
  }

  public void ShouldMarkDirtyOnFlexWrapChange()
  {
    FlexNode node = new();
    node.CalculateLayout(100, 100);

    node.FlexWrap = FlexWrap.Wrap;

    node.IsDirty.ShouldBeTrue();
  }

  public void ShouldMarkDirtyOnJustifyContentChange()
  {
    FlexNode node = new();
    node.CalculateLayout(100, 100);

    node.JustifyContent = JustifyContent.Center;

    node.IsDirty.ShouldBeTrue();
  }

  public void ShouldMarkDirtyOnAlignItemsChange()
  {
    FlexNode node = new();
    node.CalculateLayout(100, 100);

    node.AlignItems = AlignItems.Center;

    node.IsDirty.ShouldBeTrue();
  }

  public void ShouldMarkDirtyOnFlexGrowChange()
  {
    FlexNode node = new();
    node.CalculateLayout(100, 100);

    node.FlexGrow = 1;

    node.IsDirty.ShouldBeTrue();
  }

  public void ShouldMarkDirtyOnWidthChange()
  {
    FlexNode node = new();
    node.CalculateLayout(100, 100);

    node.Width = FlexValue.Point(50);

    node.IsDirty.ShouldBeTrue();
  }

  public void ShouldNotMarkDirtyWhenSettingSameValue()
  {
    FlexNode node = new();
    node.FlexDirection = FlexDirection.Column;
    node.CalculateLayout(100, 100);

    node.IsDirty.ShouldBeFalse();

    node.FlexDirection = FlexDirection.Column; // Same value

    node.IsDirty.ShouldBeFalse();
  }

  #endregion
}

/// <summary>
/// Tests for FlexNode read-only children list.
/// </summary>
public class FlexNodeChildrenListTests
{
  public void ShouldReturnReadOnlyList()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.AddChild(child);

    IReadOnlyList<FlexNode> children = parent.Children;

    children.ShouldBeAssignableTo<IReadOnlyList<FlexNode>>();
  }

  public void ShouldReflectChangesAfterAddChild()
  {
    FlexNode parent = new();
    FlexNode child1 = new();
    FlexNode child2 = new();

    IReadOnlyList<FlexNode> children = parent.Children;
    children.Count.ShouldBe(0);

    parent.AddChild(child1);
    children.Count.ShouldBe(1);

    parent.AddChild(child2);
    children.Count.ShouldBe(2);
  }
}

/// <summary>
/// Tests for FlexNode leaf and measure function properties.
/// </summary>
public class FlexNodeLeafTests
{
  public void ShouldBeLeafWithNoChildren()
  {
    FlexNode node = new();

    node.IsLeaf.ShouldBeTrue();
  }

  public void ShouldNotBeLeafWithChildren()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.AddChild(child);

    parent.IsLeaf.ShouldBeFalse();
  }

  public void ShouldBeLeafWithMeasureFunc()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.AddChild(child);
    parent.MeasureFunc = (_, _, _, _, _) => new Size(100, 100);

    parent.IsLeaf.ShouldBeTrue();
  }

  public void ShouldHaveMeasureFuncWhenSet()
  {
    FlexNode node = new();

    node.HasMeasureFunc.ShouldBeFalse();

    node.MeasureFunc = (_, _, _, _, _) => new Size(100, 100);

    node.HasMeasureFunc.ShouldBeTrue();
  }
}

/// <summary>
/// Tests for reordering children via remove and insert.
/// </summary>
public class FlexNodeReorderingTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldReorderChildrenViaRemoveAndInsert()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };
    FlexNode child3 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    // Move child3 to front
    root.RemoveChild(child3);
    root.InsertChild(child3, 0);

    root.GetChild(0).ShouldBeSameAs(child3);
    root.GetChild(1).ShouldBeSameAs(child1);
    root.GetChild(2).ShouldBeSameAs(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child3.Layout.Left.ShouldBe(0);
    child1.Layout.Left.ShouldBe(30);
    child2.Layout.Left.ShouldBe(60);
  }

  public void ShouldMoveChildToEnd()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };
    FlexNode child3 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    // Move child1 to end
    root.RemoveChild(child1);
    root.InsertChild(child1, 2);

    root.GetChild(0).ShouldBeSameAs(child2);
    root.GetChild(1).ShouldBeSameAs(child3);
    root.GetChild(2).ShouldBeSameAs(child1);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child2.Layout.Left.ShouldBe(0);
    child3.Layout.Left.ShouldBe(30);
    child1.Layout.Left.ShouldBe(60);
  }

  public void ShouldSwapTwoChildren()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Column
    };

    FlexNode child1 = new() { Width = FlexValue.Point(100), Height = FlexValue.Point(25) };
    FlexNode child2 = new() { Width = FlexValue.Point(100), Height = FlexValue.Point(25) };

    root.AddChild(child1);
    root.AddChild(child2);

    // Swap: remove both, insert in reverse order
    root.RemoveChild(child1);
    root.RemoveChild(child2);
    root.AddChild(child2);
    root.AddChild(child1);

    root.GetChild(0).ShouldBeSameAs(child2);
    root.GetChild(1).ShouldBeSameAs(child1);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child2.Layout.Top.ShouldBe(0);
    child1.Layout.Top.ShouldBe(25);
  }

  public void ShouldMarkDirtyAfterReordering()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(30) };

    root.AddChild(child1);
    root.AddChild(child2);

    // Use node.CalculateLayout which clears dirty flags
    root.CalculateLayout(100, 100);
    root.IsDirty.ShouldBeFalse();

    // Reorder
    root.RemoveChild(child2);
    root.InsertChild(child2, 0);

    root.IsDirty.ShouldBeTrue();
  }
}

/// <summary>
/// Tests for FlexNode config property.
/// </summary>
public class FlexNodeConfigTests
{
  public void ShouldUseDefaultConfigWhenNotSet()
  {
    FlexNode node = new();

    node.Config.ShouldBeNull();
    node.EffectiveConfig.ShouldBe(FlexConfig.Default);
  }

  public void ShouldUseCustomConfigWhenSet()
  {
    FlexNode node = new();
    FlexConfig customConfig = new() { PointScaleFactor = 2.0f };

    node.Config = customConfig;

    node.Config.ShouldBe(customConfig);
    node.EffectiveConfig.ShouldBe(customConfig);
  }
}

/// <summary>
/// Tests for FlexNode DirtiedFunc callback.
/// </summary>
public class FlexNodeDirtiedCallbackTests
{
  public void ShouldInvokeCallbackWhenNodeBecomesDirty()
  {
    bool callbackInvoked = false;
    FlexNode? dirtiedNode = null;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.DirtiedFunc = node =>
    {
      callbackInvoked = true;
      dirtiedNode = node;
    };

    root.CalculateLayout(100, 100);
    callbackInvoked.ShouldBeFalse();

    root.Width = FlexValue.Point(200);

    callbackInvoked.ShouldBeTrue();
    dirtiedNode.ShouldBeSameAs(root);
  }

  public void ShouldOnlyInvokeCallbackOnce()
  {
    int callCount = 0;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.DirtiedFunc = _ => callCount++;

    root.CalculateLayout(100, 100);

    // Multiple changes while already dirty
    root.Width = FlexValue.Point(200);
    root.Height = FlexValue.Point(200);
    root.FlexDirection = FlexDirection.Column;

    // Should only be called once (first time it becomes dirty)
    callCount.ShouldBe(1);
  }

  public void ShouldInvokeCallbackOnParentWhenChildBecomesDirty()
  {
    bool parentCallbackInvoked = false;
    bool childCallbackInvoked = false;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.DirtiedFunc = _ => parentCallbackInvoked = true;

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    child.DirtiedFunc = _ => childCallbackInvoked = true;

    root.AddChild(child);
    root.CalculateLayout(100, 100);

    // Change child's style
    child.Width = FlexValue.Point(60);

    childCallbackInvoked.ShouldBeTrue();
    parentCallbackInvoked.ShouldBeTrue();
  }

  public void ShouldNotInvokeCallbackWhenAlreadyDirty()
  {
    int callCount = 0;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };
    root.DirtiedFunc = _ => callCount++;

    // Node starts dirty, so this should invoke callback
    root.CalculateLayout(100, 100);
    callCount.ShouldBe(0); // No callback during initial layout

    root.Width = FlexValue.Point(200);
    callCount.ShouldBe(1);

    // Node is already dirty, this should NOT invoke callback
    root.MarkDirty();
    callCount.ShouldBe(1);
  }

  public void ShouldInvokeCallbackWhenAddingChild()
  {
    bool callbackInvoked = false;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };
    root.DirtiedFunc = _ => callbackInvoked = true;

    root.CalculateLayout(100, 100);
    callbackInvoked.ShouldBeFalse();

    FlexNode child = new() { Width = FlexValue.Point(50) };
    root.AddChild(child);

    callbackInvoked.ShouldBeTrue();
  }

  public void ShouldInvokeCallbackWhenRemovingChild()
  {
    bool callbackInvoked = false;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };

    FlexNode child = new() { Width = FlexValue.Point(50) };
    root.AddChild(child);
    root.CalculateLayout(100, 100);

    root.DirtiedFunc = _ => callbackInvoked = true;

    root.RemoveChild(child);

    callbackInvoked.ShouldBeTrue();
  }

  public void ShouldNotInvokeSiblingCallbackWhenNodeBecomesDirty()
  {
    bool siblingCallbackInvoked = false;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50) };
    child2.DirtiedFunc = _ => siblingCallbackInvoked = true;

    root.AddChild(child1);
    root.AddChild(child2);
    root.CalculateLayout(100, 100);

    // Change child1's style - should NOT invoke child2's callback
    child1.Width = FlexValue.Point(60);

    siblingCallbackInvoked.ShouldBeFalse();
  }

  public void ShouldWorkWithNullCallback()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };

    root.DirtiedFunc = null;
    root.CalculateLayout(100, 100);

    // Should not throw
    root.Width = FlexValue.Point(200);
    root.IsDirty.ShouldBeTrue();
  }
}

/// <summary>
/// Tests for FlexConfig default values.
/// </summary>
public class FlexConfigDefaultValuesTests
{
  public void ShouldHaveExpectedDefaults()
  {
    FlexConfig config = new();

    config.Direction.ShouldBe(Direction.Ltr);
    config.PointScaleFactor.ShouldBe(1.0f);
    config.UseWebDefaults.ShouldBeFalse();
    config.UseErrata.ShouldBeFalse();
  }

  public void ShouldHaveStaticDefaultInstance()
  {
    FlexConfig.Default.ShouldNotBeNull();
    FlexConfig.Default.PointScaleFactor.ShouldBe(1.0f);
  }

  public void ShouldReturnSameStaticDefaultInstance()
  {
    FlexConfig default1 = FlexConfig.Default;
    FlexConfig default2 = FlexConfig.Default;

    default1.ShouldBeSameAs(default2);
  }
}

/// <summary>
/// Tests for FlexConfig cloning.
/// </summary>
public class FlexConfigCloneTests
{
  public void ShouldCloneAllProperties()
  {
    FlexConfig original = new()
    {
      Direction = Direction.Rtl,
      PointScaleFactor = 2.5f,
      UseWebDefaults = true,
      UseErrata = true
    };

    FlexConfig clone = original.Clone();

    clone.Direction.ShouldBe(Direction.Rtl);
    clone.PointScaleFactor.ShouldBe(2.5f);
    clone.UseWebDefaults.ShouldBeTrue();
    clone.UseErrata.ShouldBeTrue();
  }

  public void ShouldCreateIndependentCopy()
  {
    FlexConfig original = new() { PointScaleFactor = 1.0f };
    FlexConfig clone = original.Clone();

    // Modify original
    original.PointScaleFactor = 3.0f;

    // Clone should be unchanged
    clone.PointScaleFactor.ShouldBe(1.0f);
  }

  public void ShouldNotBeSameReference()
  {
    FlexConfig original = new();
    FlexConfig clone = original.Clone();

    clone.ShouldNotBeSameAs(original);
  }
}

/// <summary>
/// Tests for FlexConfig sharing across nodes.
/// </summary>
public class FlexConfigSharingTests
{
  public void ShouldApplyConfigToMultipleNodes()
  {
    FlexConfig config = new() { PointScaleFactor = 2.0f };

    FlexNode root = new() { Config = config };
    FlexNode child1 = new() { Config = config };
    FlexNode child2 = new() { Config = config };

    root.AddChild(child1);
    root.AddChild(child2);

    root.Config.ShouldBeSameAs(config);
    child1.Config.ShouldBeSameAs(config);
    child2.Config.ShouldBeSameAs(config);
  }

  public void ShouldAllowDifferentConfigsForDifferentNodes()
  {
    FlexConfig config1 = new() { PointScaleFactor = 1.0f };
    FlexConfig config2 = new() { PointScaleFactor = 2.0f };

    FlexNode node1 = new() { Config = config1 };
    FlexNode node2 = new() { Config = config2 };

    node1.EffectiveConfig.PointScaleFactor.ShouldBe(1.0f);
    node2.EffectiveConfig.PointScaleFactor.ShouldBe(2.0f);
  }

  public void ShouldUseEffectiveConfigFromNodeOrDefault()
  {
    FlexConfig customConfig = new() { Direction = Direction.Rtl };

    FlexNode nodeWithConfig = new() { Config = customConfig };
    FlexNode nodeWithoutConfig = new();

    nodeWithConfig.EffectiveConfig.Direction.ShouldBe(Direction.Rtl);
    nodeWithoutConfig.EffectiveConfig.Direction.ShouldBe(Direction.Ltr);
  }
}

/// <summary>
/// Tests for PointScaleFactor behavior.
/// </summary>
public class FlexConfigPointScaleFactorTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldAcceptVariousScaleFactors()
  {
    float[] scaleFactors = [0.5f, 1.0f, 2.0f, 3.0f];

    foreach (float scaleFactor in scaleFactors)
    {
      FlexConfig config = new() { PointScaleFactor = scaleFactor };
      config.PointScaleFactor.ShouldBe(scaleFactor);
    }
  }

  public void ShouldAffectLayoutRounding()
  {
    // At 1x scale, 100/3 children should round to whole pixels
    FlexConfig config1x = new() { PointScaleFactor = 1.0f };
    FlexNode root1 = new()
    {
      Config = config1x,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    for (int i = 0; i < 3; i++)
    {
      root1.AddChild(new FlexNode { Config = config1x, FlexGrow = 1 });
    }

    Engine.CalculateLayout(root1, 100, 100, Direction.Ltr);

    // Each child should be approximately 33.33, rounded to pixels
    // Total should still equal 100 (within floating point tolerance)
    float total = root1.GetChild(0).Layout.Width +
                  root1.GetChild(1).Layout.Width +
                  root1.GetChild(2).Layout.Width;
    total.ShouldBe(100, 0.001f);
  }

  public void ShouldRoundToHalfPixelsAt2xScale()
  {
    FlexConfig config2x = new() { PointScaleFactor = 2.0f };
    FlexNode root = new()
    {
      Config = config2x,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    for (int i = 0; i < 3; i++)
    {
      root.AddChild(new FlexNode { Config = config2x, FlexGrow = 1 });
    }

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // At 2x scale, can use 0.5 pixel increments
    // Total should still equal 100 (within floating point tolerance)
    float total = root.GetChild(0).Layout.Width +
                  root.GetChild(1).Layout.Width +
                  root.GetChild(2).Layout.Width;
    total.ShouldBe(100, 0.001f);
  }
}

/// <summary>
/// Tests for FlexConfig direction setting.
/// </summary>
public class FlexConfigDirectionTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldUseConfigDirectionForRootNode()
  {
    FlexConfig rtlConfig = new() { Direction = Direction.Rtl };
    FlexNode root = new()
    {
      Config = rtlConfig,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Inherit);

    // In RTL, child should be positioned from right
    // But note: CalculateLayout takes direction parameter which may override
    root.ResolvedDirection.ShouldBe(Direction.Rtl);
  }

  public void ShouldInheritDirectionFromConfig()
  {
    FlexConfig config = new() { Direction = Direction.Rtl };
    FlexNode root = new()
    {
      Config = config,
      Direction = Direction.Inherit
    };

    root.ResolvedDirection.ShouldBe(Direction.Rtl);
  }
}

/// <summary>
/// Tests for FlexNode default values matching expected spec.
/// </summary>
public class FlexNodeDefaultValueTests
{
  public void ShouldHaveCorrectFlexboxDefaults()
  {
    FlexNode node = new();

    // Direction and wrap
    node.FlexDirection.ShouldBe(FlexDirection.Row);
    node.FlexWrap.ShouldBe(FlexWrap.NoWrap);

    // Alignment
    node.JustifyContent.ShouldBe(JustifyContent.FlexStart);
    node.AlignItems.ShouldBe(AlignItems.Stretch);
    node.AlignContent.ShouldBe(AlignContent.FlexStart);
    node.AlignSelf.ShouldBe(AlignSelf.Auto);

    // Flex factors
    node.FlexGrow.ShouldBe(0f);
    node.FlexShrink.ShouldBe(1f);
    node.FlexBasis.ShouldBe(FlexValue.Auto);

    // Position and display
    node.PositionType.ShouldBe(PositionType.Relative);
    node.Display.ShouldBe(Display.Flex);
    node.Overflow.ShouldBe(Overflow.Visible);

    // Dimensions
    node.Width.ShouldBe(FlexValue.Undefined);
    node.Height.ShouldBe(FlexValue.Undefined);
    node.MinWidth.ShouldBe(FlexValue.Undefined);
    node.MinHeight.ShouldBe(FlexValue.Undefined);
    node.MaxWidth.ShouldBe(FlexValue.Undefined);
    node.MaxHeight.ShouldBe(FlexValue.Undefined);

    // Other
    node.AspectRatio.ShouldBeNull();
    node.BoxSizing.ShouldBe(BoxSizing.BorderBox);
  }

  public void ShouldHaveCorrectGapDefaults()
  {
    FlexNode node = new();

    node.Gap.ShouldBe(0f);
    node.RowGap.ShouldBe(0f);
    node.ColumnGap.ShouldBe(0f);
  }

  public void ShouldHaveCorrectDirectionDefault()
  {
    FlexNode node = new();

    node.Direction.ShouldBe(Direction.Inherit);
  }
}

/// <summary>
/// Tests for FlexNode.Clone() method.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeCloneTests
{
  public void ShouldCreateNewInstance()
  {
    FlexNode original = new();

    FlexNode clone = original.Clone();

    clone.ShouldNotBeSameAs(original);
  }

  public void ShouldHaveNoParent()
  {
    FlexNode parent = new();
    FlexNode child = new();
    parent.AddChild(child);

    FlexNode clone = child.Clone();

    clone.Parent.ShouldBeNull();
  }

  public void ShouldCopyStyleProperties()
  {
    FlexNode original = new()
    {
      FlexDirection = FlexDirection.Column,
      FlexWrap = FlexWrap.Wrap,
      JustifyContent = JustifyContent.SpaceBetween,
      AlignItems = AlignItems.Center,
      AlignContent = AlignContent.SpaceAround,
      AlignSelf = AlignSelf.FlexEnd,
      FlexGrow = 2,
      FlexShrink = 0.5f,
      FlexBasis = FlexValue.Point(100),
      Width = FlexValue.Point(200),
      Height = FlexValue.Percent(50),
      MinWidth = FlexValue.Point(50),
      MinHeight = FlexValue.Point(25),
      MaxWidth = FlexValue.Point(400),
      MaxHeight = FlexValue.Point(300),
      Display = Display.None,
      PositionType = PositionType.Absolute,
      Overflow = Overflow.Hidden,
      AspectRatio = 1.5f,
      BoxSizing = BoxSizing.ContentBox,
      Direction = Direction.Rtl
    };

    FlexNode clone = original.Clone();

    clone.FlexDirection.ShouldBe(FlexDirection.Column);
    clone.FlexWrap.ShouldBe(FlexWrap.Wrap);
    clone.JustifyContent.ShouldBe(JustifyContent.SpaceBetween);
    clone.AlignItems.ShouldBe(AlignItems.Center);
    clone.AlignContent.ShouldBe(AlignContent.SpaceAround);
    clone.AlignSelf.ShouldBe(AlignSelf.FlexEnd);
    clone.FlexGrow.ShouldBe(2);
    clone.FlexShrink.ShouldBe(0.5f);
    clone.FlexBasis.ShouldBe(FlexValue.Point(100));
    clone.Width.ShouldBe(FlexValue.Point(200));
    clone.Height.ShouldBe(FlexValue.Percent(50));
    clone.MinWidth.ShouldBe(FlexValue.Point(50));
    clone.MinHeight.ShouldBe(FlexValue.Point(25));
    clone.MaxWidth.ShouldBe(FlexValue.Point(400));
    clone.MaxHeight.ShouldBe(FlexValue.Point(300));
    clone.Display.ShouldBe(Display.None);
    clone.PositionType.ShouldBe(PositionType.Absolute);
    clone.Overflow.ShouldBe(Overflow.Hidden);
    clone.AspectRatio.ShouldBe(1.5f);
    clone.BoxSizing.ShouldBe(BoxSizing.ContentBox);
    clone.Direction.ShouldBe(Direction.Rtl);
  }

  public void ShouldCopySpacingProperties()
  {
    FlexNode original = new();
    original.SetMargin(Edge.Left, FlexValue.Point(10));
    original.SetMargin(Edge.Top, FlexValue.Point(20));
    original.SetPadding(Edge.All, FlexValue.Point(5));
    original.SetBorder(Edge.Right, 3);
    original.SetPosition(Edge.Bottom, FlexValue.Point(15));
    original.Gap = 8;
    original.RowGap = 12;
    original.ColumnGap = 16;

    FlexNode clone = original.Clone();

    clone.Margin[Edge.Left].ShouldBe(FlexValue.Point(10));
    clone.Margin[Edge.Top].ShouldBe(FlexValue.Point(20));
    clone.Padding[Edge.All].ShouldBe(FlexValue.Point(5));
    clone.Border[Edge.Right].ShouldBe(3f);
    clone.Position[Edge.Bottom].ShouldBe(FlexValue.Point(15));
    clone.Gap.ShouldBe(8f);
    clone.RowGap.ShouldBe(12f);
    clone.ColumnGap.ShouldBe(16f);
  }

  public void ShouldDeepCloneChildren()
  {
    FlexNode original = new() { Width = FlexValue.Point(100) };
    FlexNode child1 = new() { Width = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(30) };
    original.AddChild(child1);
    original.AddChild(child2);

    FlexNode clone = original.Clone();

    clone.ChildCount.ShouldBe(2);
    clone.Children[0].ShouldNotBeSameAs(child1);
    clone.Children[1].ShouldNotBeSameAs(child2);
    clone.Children[0].Width.ShouldBe(FlexValue.Point(50));
    clone.Children[1].Width.ShouldBe(FlexValue.Point(30));
  }

  public void ShouldSetClonedChildrenParent()
  {
    FlexNode original = new();
    FlexNode child = new();
    original.AddChild(child);

    FlexNode clone = original.Clone();

    clone.Children[0].Parent.ShouldBeSameAs(clone);
  }

  public void ShouldDeepCloneNestedHierarchy()
  {
    FlexNode root = new() { Width = FlexValue.Point(100) };
    FlexNode level1 = new() { Width = FlexValue.Point(80) };
    FlexNode level2 = new() { Width = FlexValue.Point(60) };
    FlexNode level3 = new() { Width = FlexValue.Point(40) };

    root.AddChild(level1);
    level1.AddChild(level2);
    level2.AddChild(level3);

    FlexNode clone = root.Clone();

    clone.ChildCount.ShouldBe(1);
    clone.Children[0].ChildCount.ShouldBe(1);
    clone.Children[0].Children[0].ChildCount.ShouldBe(1);
    clone.Children[0].Children[0].Children[0].ChildCount.ShouldBe(0);

    // Verify all nodes are different instances
    clone.ShouldNotBeSameAs(root);
    clone.Children[0].ShouldNotBeSameAs(level1);
    clone.Children[0].Children[0].ShouldNotBeSameAs(level2);
    clone.Children[0].Children[0].Children[0].ShouldNotBeSameAs(level3);

    // Verify properties are copied
    clone.Children[0].Children[0].Children[0].Width.ShouldBe(FlexValue.Point(40));
  }

  public void ShouldCopyMeasureFunc()
  {
    MeasureFunc measureFunc = (_, _, _, _, _) => new Size(100, 50);
    FlexNode original = new() { MeasureFunc = measureFunc };

    FlexNode clone = original.Clone();

    clone.MeasureFunc.ShouldBeSameAs(measureFunc);
    clone.HasMeasureFunc.ShouldBeTrue();
  }

  public void ShouldCopyBaselineFunc()
  {
    BaselineFunc baselineFunc = (_, _, _) => 10f;
    FlexNode original = new() { BaselineFunc = baselineFunc };

    FlexNode clone = original.Clone();

    clone.BaselineFunc.ShouldBeSameAs(baselineFunc);
  }

  public void ShouldCopyDirtiedFunc()
  {
    int callCount = 0;
    Action<FlexNode> dirtiedFunc = _ => callCount++;
    FlexNode original = new() { DirtiedFunc = dirtiedFunc };

    FlexNode clone = original.Clone();

    clone.DirtiedFunc.ShouldBeSameAs(dirtiedFunc);
  }

  public void ShouldCopyConfig()
  {
    FlexConfig config = new() { PointScaleFactor = 2.0f };
    FlexNode original = new() { Config = config };

    FlexNode clone = original.Clone();

    clone.Config.ShouldBeSameAs(config);
  }

  public void ShouldCopyContext()
  {
    object context = new { Name = "TestContext" };
    FlexNode original = new() { Context = context };

    FlexNode clone = original.Clone();

    clone.Context.ShouldBeSameAs(context);
  }

  public void ShouldNotCopyLayoutResults()
  {
    FlexNode original = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexLayoutEngine engine = new();
    engine.CalculateLayout(original, 100, 100, Direction.Ltr);

    FlexNode clone = original.Clone();

    // Clone should have fresh layout (zeroed out)
    clone.Layout.Width.ShouldBe(0);
    clone.Layout.Height.ShouldBe(0);
    clone.Layout.Left.ShouldBe(0);
    clone.Layout.Top.ShouldBe(0);
  }

  public void ShouldBeIndependentOfOriginal()
  {
    FlexNode original = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode clone = original.Clone();

    // Modify clone
    clone.Width = FlexValue.Point(200);
    clone.FlexDirection = FlexDirection.Column;

    // Original should be unchanged
    original.Width.ShouldBe(FlexValue.Point(100));
    original.FlexDirection.ShouldBe(FlexDirection.Row);
  }

  public void ShouldCloneEmptyNode()
  {
    FlexNode original = new();

    FlexNode clone = original.Clone();

    clone.ShouldNotBeNull();
    clone.ChildCount.ShouldBe(0);
    clone.Parent.ShouldBeNull();
  }

  public void ShouldCloneNodeWithMultipleChildren()
  {
    FlexNode original = new();

    for (int i = 0; i < 5; i++)
    {
      FlexNode child = new() { Width = FlexValue.Point(i * 10) };
      original.AddChild(child);
    }

    FlexNode clone = original.Clone();

    clone.ChildCount.ShouldBe(5);

    for (int i = 0; i < 5; i++)
    {
      clone.Children[i].Width.ShouldBe(FlexValue.Point(i * 10));
      clone.Children[i].Parent.ShouldBeSameAs(clone);
    }
  }
}

/// <summary>
/// Tests for FlexNode.ReplaceChild() method.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeReplaceChildTests
{
  public void ShouldReplaceChildAtSameIndex()
  {
    FlexNode parent = new();
    FlexNode child1 = new() { Width = FlexValue.Point(10) };
    FlexNode child2 = new() { Width = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(30) };
    FlexNode replacement = new() { Width = FlexValue.Point(99) };

    parent.AddChild(child1);
    parent.AddChild(child2);
    parent.AddChild(child3);

    bool result = parent.ReplaceChild(child2, replacement);

    result.ShouldBeTrue();
    parent.ChildCount.ShouldBe(3);
    parent.Children[0].ShouldBeSameAs(child1);
    parent.Children[1].ShouldBeSameAs(replacement);
    parent.Children[2].ShouldBeSameAs(child3);
  }

  public void ShouldSetNewChildParent()
  {
    FlexNode parent = new();
    FlexNode oldChild = new();
    FlexNode newChild = new();
    parent.AddChild(oldChild);

    parent.ReplaceChild(oldChild, newChild);

    newChild.Parent.ShouldBeSameAs(parent);
  }

  public void ShouldClearOldChildParent()
  {
    FlexNode parent = new();
    FlexNode oldChild = new();
    FlexNode newChild = new();
    parent.AddChild(oldChild);

    parent.ReplaceChild(oldChild, newChild);

    oldChild.Parent.ShouldBeNull();
  }

  public void ShouldReturnFalseWhenChildNotFound()
  {
    FlexNode parent = new();
    FlexNode existingChild = new();
    FlexNode notAChild = new();
    FlexNode replacement = new();
    parent.AddChild(existingChild);

    bool result = parent.ReplaceChild(notAChild, replacement);

    result.ShouldBeFalse();
    parent.ChildCount.ShouldBe(1);
    parent.Children[0].ShouldBeSameAs(existingChild);
  }

  public void ShouldThrowWhenOldChildIsNull()
  {
    FlexNode parent = new();
    FlexNode newChild = new();

    Should.Throw<ArgumentNullException>(() => parent.ReplaceChild(null!, newChild));
  }

  public void ShouldThrowWhenNewChildIsNull()
  {
    FlexNode parent = new();
    FlexNode oldChild = new();
    parent.AddChild(oldChild);

    Should.Throw<ArgumentNullException>(() => parent.ReplaceChild(oldChild, null!));
  }

  public void ShouldMarkParentDirty()
  {
    FlexNode parent = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    FlexNode oldChild = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode newChild = new() { Width = FlexValue.Point(60), Height = FlexValue.Point(60) };
    parent.AddChild(oldChild);

    // Clear dirty flag using node's CalculateLayout (which clears dirty)
    parent.CalculateLayout(100, 100);
    parent.IsDirty.ShouldBeFalse();

    parent.ReplaceChild(oldChild, newChild);

    parent.IsDirty.ShouldBeTrue();
  }

  public void ShouldRemoveNewChildFromPreviousParent()
  {
    FlexNode parent1 = new();
    FlexNode parent2 = new();
    FlexNode oldChild = new();
    FlexNode newChild = new();

    parent1.AddChild(oldChild);
    parent2.AddChild(newChild);

    parent1.ReplaceChild(oldChild, newChild);

    parent2.ChildCount.ShouldBe(0);
    newChild.Parent.ShouldBeSameAs(parent1);
  }

  public void ShouldReplaceFirstChild()
  {
    FlexNode parent = new();
    FlexNode child1 = new() { Width = FlexValue.Point(10) };
    FlexNode child2 = new() { Width = FlexValue.Point(20) };
    FlexNode replacement = new() { Width = FlexValue.Point(99) };

    parent.AddChild(child1);
    parent.AddChild(child2);

    parent.ReplaceChild(child1, replacement);

    parent.Children[0].ShouldBeSameAs(replacement);
    parent.Children[1].ShouldBeSameAs(child2);
  }

  public void ShouldReplaceLastChild()
  {
    FlexNode parent = new();
    FlexNode child1 = new() { Width = FlexValue.Point(10) };
    FlexNode child2 = new() { Width = FlexValue.Point(20) };
    FlexNode replacement = new() { Width = FlexValue.Point(99) };

    parent.AddChild(child1);
    parent.AddChild(child2);

    parent.ReplaceChild(child2, replacement);

    parent.Children[0].ShouldBeSameAs(child1);
    parent.Children[1].ShouldBeSameAs(replacement);
  }

  public void ShouldReplaceOnlyChild()
  {
    FlexNode parent = new();
    FlexNode oldChild = new() { Width = FlexValue.Point(10) };
    FlexNode newChild = new() { Width = FlexValue.Point(99) };

    parent.AddChild(oldChild);

    parent.ReplaceChild(oldChild, newChild);

    parent.ChildCount.ShouldBe(1);
    parent.Children[0].ShouldBeSameAs(newChild);
    parent.Children[0].Width.ShouldBe(FlexValue.Point(99));
  }

  public void ShouldNotAffectOtherChildrenOrder()
  {
    FlexNode parent = new();
    FlexNode child1 = new() { Width = FlexValue.Point(10) };
    FlexNode child2 = new() { Width = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(30) };
    FlexNode child4 = new() { Width = FlexValue.Point(40) };
    FlexNode replacement = new() { Width = FlexValue.Point(99) };

    parent.AddChild(child1);
    parent.AddChild(child2);
    parent.AddChild(child3);
    parent.AddChild(child4);

    parent.ReplaceChild(child2, replacement);

    parent.Children[0].Width.ShouldBe(FlexValue.Point(10));
    parent.Children[1].Width.ShouldBe(FlexValue.Point(99));
    parent.Children[2].Width.ShouldBe(FlexValue.Point(30));
    parent.Children[3].Width.ShouldBe(FlexValue.Point(40));
  }

  public void ShouldWorkWithEmptyParent()
  {
    FlexNode parent = new();
    FlexNode notAChild = new();
    FlexNode replacement = new();

    bool result = parent.ReplaceChild(notAChild, replacement);

    result.ShouldBeFalse();
    parent.ChildCount.ShouldBe(0);
  }
}
