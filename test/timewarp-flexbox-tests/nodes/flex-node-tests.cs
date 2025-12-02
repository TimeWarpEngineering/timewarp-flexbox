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
