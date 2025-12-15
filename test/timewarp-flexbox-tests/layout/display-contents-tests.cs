namespace TimeWarp.Flexbox.Tests.Layout;

/// <summary>
/// Tests for Display.Contents behavior.
/// Display.Contents makes an element's children behave as direct children
/// of the element's parent, effectively "unwrapping" the element.
/// </summary>
[TestTag(TestTags.Fast)]
public class DisplayContentsBasicTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldFlattenContentsNodeChildren()
  {
    // Arrange: root -> wrapper (display:contents) -> child
    // Expected: child should be positioned as if it's a direct child of root
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode wrapper = new()
    {
      Display = Display.Contents
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    wrapper.AddChild(child);
    root.AddChild(wrapper);

    // Act
    Engine.CalculateLayout(root, 200, 100);

    // Assert: child should be positioned at (0, 0) as if it's a direct child
    child.Layout.Left.ShouldBe(0);
    child.Layout.Top.ShouldBe(0);
    child.Layout.Width.ShouldBe(50);
    child.Layout.Height.ShouldBe(50);
  }

  public void ShouldPositionMultipleGrandchildrenInRow()
  {
    // Arrange: root (row) -> wrapper (contents) -> [child1, child2, child3]
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode wrapper = new() { Display = Display.Contents };

    FlexNode child1 = new() { Width = FlexValue.Point(100) };
    FlexNode child2 = new() { Width = FlexValue.Point(100) };
    FlexNode child3 = new() { Width = FlexValue.Point(100) };

    wrapper.AddChild(child1);
    wrapper.AddChild(child2);
    wrapper.AddChild(child3);
    root.AddChild(wrapper);

    // Act
    Engine.CalculateLayout(root, 300, 100);

    // Assert: children should be positioned sequentially as if direct children of root
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(100);
    child3.Layout.Left.ShouldBe(200);
  }

  public void ShouldMixContentsAndNormalChildren()
  {
    // Arrange: root -> [normalChild, wrapper (contents) -> [contentsChild1, contentsChild2]]
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode normalChild = new() { Width = FlexValue.Point(100) };

    FlexNode wrapper = new() { Display = Display.Contents };
    FlexNode contentsChild1 = new() { Width = FlexValue.Point(100) };
    FlexNode contentsChild2 = new() { Width = FlexValue.Point(100) };

    wrapper.AddChild(contentsChild1);
    wrapper.AddChild(contentsChild2);

    root.AddChild(normalChild);
    root.AddChild(wrapper);

    // Act
    Engine.CalculateLayout(root, 300, 100);

    // Assert: all children positioned sequentially
    normalChild.Layout.Left.ShouldBe(0);
    contentsChild1.Layout.Left.ShouldBe(100);
    contentsChild2.Layout.Left.ShouldBe(200);
  }

  public void ShouldApplyFlexGrowToContentsChildren()
  {
    // Arrange: wrapper children should participate in flex grow
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode wrapper = new() { Display = Display.Contents };
    FlexNode child1 = new() { FlexGrow = 1 };
    FlexNode child2 = new() { FlexGrow = 2 };

    wrapper.AddChild(child1);
    wrapper.AddChild(child2);
    root.AddChild(wrapper);

    // Act
    Engine.CalculateLayout(root, 300, 100);

    // Assert: children should grow proportionally (1:2 ratio)
    child1.Layout.Width.ShouldBe(100); // 1/3 of 300
    child2.Layout.Width.ShouldBe(200); // 2/3 of 300
  }

  public void ShouldApplyFlexShrinkToContentsChildren()
  {
    // Arrange
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode wrapper = new() { Display = Display.Contents };
    FlexNode child1 = new() { Width = FlexValue.Point(150), FlexShrink = 1 };
    FlexNode child2 = new() { Width = FlexValue.Point(150), FlexShrink = 1 };

    wrapper.AddChild(child1);
    wrapper.AddChild(child2);
    root.AddChild(wrapper);

    // Act
    Engine.CalculateLayout(root, 200, 100);

    // Assert: children should shrink equally
    child1.Layout.Width.ShouldBe(100);
    child2.Layout.Width.ShouldBe(100);
  }

  public void ShouldApplyGapToFlattenedChildren()
  {
    // Arrange
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      ColumnGap = 20
    };

    FlexNode wrapper = new() { Display = Display.Contents };
    FlexNode child1 = new() { Width = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50) };
    FlexNode child3 = new() { Width = FlexValue.Point(50) };

    wrapper.AddChild(child1);
    wrapper.AddChild(child2);
    wrapper.AddChild(child3);
    root.AddChild(wrapper);

    // Act
    Engine.CalculateLayout(root, 300, 100);

    // Assert: gap should be applied between flattened children
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(70);  // 50 + 20
    child3.Layout.Left.ShouldBe(140); // 50 + 20 + 50 + 20
  }

  public void ShouldApplyJustifyContentToFlattenedChildren()
  {
    // Arrange
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      JustifyContent = JustifyContent.Center
    };

    FlexNode wrapper = new() { Display = Display.Contents };
    FlexNode child = new() { Width = FlexValue.Point(100) };

    wrapper.AddChild(child);
    root.AddChild(wrapper);

    // Act
    Engine.CalculateLayout(root, 300, 100);

    // Assert: child should be centered
    child.Layout.Left.ShouldBe(100); // (300 - 100) / 2
  }

  public void ShouldApplyAlignItemsToFlattenedChildren()
  {
    // Arrange
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Center
    };

    FlexNode wrapper = new() { Display = Display.Contents };
    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    wrapper.AddChild(child);
    root.AddChild(wrapper);

    // Act
    Engine.CalculateLayout(root, 100, 200);

    // Assert: child should be vertically centered
    child.Layout.Top.ShouldBe(75); // (200 - 50) / 2
  }

  public void ShouldNotGenerateBoxForContentsNode()
  {
    // Arrange
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100)
    };

    FlexNode wrapper = new()
    {
      Display = Display.Contents,
      Width = FlexValue.Point(100),  // These dimensions should be ignored
      Height = FlexValue.Point(50)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    wrapper.AddChild(child);
    root.AddChild(wrapper);

    // Act
    Engine.CalculateLayout(root, 200, 100);

    // Assert: wrapper's dimensions should remain at default (0)
    // since display:contents doesn't generate a box
    wrapper.Layout.Width.ShouldBe(0);
    wrapper.Layout.Height.ShouldBe(0);
  }

  public void ShouldHandleEmptyContentsNode()
  {
    // Arrange
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode normalChild = new() { Width = FlexValue.Point(100) };
    FlexNode emptyWrapper = new() { Display = Display.Contents };

    root.AddChild(normalChild);
    root.AddChild(emptyWrapper);

    // Act
    Engine.CalculateLayout(root, 200, 100);

    // Assert: should work fine, empty contents node contributes nothing
    normalChild.Layout.Left.ShouldBe(0);
    normalChild.Layout.Width.ShouldBe(100);
  }
}

/// <summary>
/// Tests for nested Display.Contents scenarios.
/// </summary>
[TestTag(TestTags.Fast)]
public class DisplayContentsNestedTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldFlattenNestedContentsNodes()
  {
    // Arrange: root -> wrapper1 (contents) -> wrapper2 (contents) -> child
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode wrapper1 = new() { Display = Display.Contents };
    FlexNode wrapper2 = new() { Display = Display.Contents };
    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    wrapper2.AddChild(child);
    wrapper1.AddChild(wrapper2);
    root.AddChild(wrapper1);

    // Act
    Engine.CalculateLayout(root, 200, 100);

    // Assert: child should be positioned as direct child of root
    child.Layout.Left.ShouldBe(0);
    child.Layout.Top.ShouldBe(0);
  }

  public void ShouldFlattenDeeplyNestedContentsNodes()
  {
    // Arrange: root -> w1 -> w2 -> w3 -> w4 -> child (all wrappers are contents)
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode wrapper1 = new() { Display = Display.Contents };
    FlexNode wrapper2 = new() { Display = Display.Contents };
    FlexNode wrapper3 = new() { Display = Display.Contents };
    FlexNode wrapper4 = new() { Display = Display.Contents };
    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    wrapper4.AddChild(child);
    wrapper3.AddChild(wrapper4);
    wrapper2.AddChild(wrapper3);
    wrapper1.AddChild(wrapper2);
    root.AddChild(wrapper1);

    // Act
    Engine.CalculateLayout(root, 200, 100);

    // Assert
    child.Layout.Left.ShouldBe(0);
    child.Layout.Top.ShouldBe(0);
    child.Layout.Width.ShouldBe(50);
    child.Layout.Height.ShouldBe(50);
  }

  public void ShouldHandleMultipleNestedContentsWithMultipleChildren()
  {
    // Arrange:
    // root -> [
    //   wrapper1 (contents) -> [child1, wrapper2 (contents) -> [child2, child3]],
    //   child4
    // ]
    FlexNode root = new()
    {
      Width = FlexValue.Point(400),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode wrapper1 = new() { Display = Display.Contents };
    FlexNode child1 = new() { Width = FlexValue.Point(100) };
    FlexNode wrapper2 = new() { Display = Display.Contents };
    FlexNode child2 = new() { Width = FlexValue.Point(100) };
    FlexNode child3 = new() { Width = FlexValue.Point(100) };
    FlexNode child4 = new() { Width = FlexValue.Point(100) };

    wrapper2.AddChild(child2);
    wrapper2.AddChild(child3);
    wrapper1.AddChild(child1);
    wrapper1.AddChild(wrapper2);
    root.AddChild(wrapper1);
    root.AddChild(child4);

    // Act
    Engine.CalculateLayout(root, 400, 100);

    // Assert: all children positioned sequentially
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(100);
    child3.Layout.Left.ShouldBe(200);
    child4.Layout.Left.ShouldBe(300);
  }
}

/// <summary>
/// Tests for HasContentsChildren tracking.
/// </summary>
[TestTag(TestTags.Fast)]
public class HasContentsChildrenTrackingTests
{
  public void ShouldReturnFalseWhenNoContentsChildren()
  {
    FlexNode parent = new();
    FlexNode child1 = new() { Display = Display.Flex };
    FlexNode child2 = new() { Display = Display.None };

    parent.AddChild(child1);
    parent.AddChild(child2);

    parent.HasContentsChildren.ShouldBeFalse();
  }

  public void ShouldReturnTrueWhenHasContentsChild()
  {
    FlexNode parent = new();
    FlexNode child = new() { Display = Display.Contents };

    parent.AddChild(child);

    parent.HasContentsChildren.ShouldBeTrue();
  }

  public void ShouldUpdateWhenContentsChildRemoved()
  {
    FlexNode parent = new();
    FlexNode contentsChild = new() { Display = Display.Contents };
    FlexNode normalChild = new();

    parent.AddChild(contentsChild);
    parent.AddChild(normalChild);

    parent.HasContentsChildren.ShouldBeTrue();

    parent.RemoveChild(contentsChild);

    parent.HasContentsChildren.ShouldBeFalse();
  }

  public void ShouldUpdateWhenContentsChildReplaced()
  {
    FlexNode parent = new();
    FlexNode contentsChild = new() { Display = Display.Contents };
    FlexNode normalChild = new();

    parent.AddChild(contentsChild);
    parent.HasContentsChildren.ShouldBeTrue();

    parent.ReplaceChild(contentsChild, normalChild);

    parent.HasContentsChildren.ShouldBeFalse();
  }

  public void ShouldUpdateWhenNormalChildReplacedWithContents()
  {
    FlexNode parent = new();
    FlexNode normalChild = new();
    FlexNode contentsChild = new() { Display = Display.Contents };

    parent.AddChild(normalChild);
    parent.HasContentsChildren.ShouldBeFalse();

    parent.ReplaceChild(normalChild, contentsChild);

    parent.HasContentsChildren.ShouldBeTrue();
  }

  public void ShouldResetOnRemoveAllChildren()
  {
    FlexNode parent = new();
    FlexNode contentsChild1 = new() { Display = Display.Contents };
    FlexNode contentsChild2 = new() { Display = Display.Contents };

    parent.AddChild(contentsChild1);
    parent.AddChild(contentsChild2);

    parent.HasContentsChildren.ShouldBeTrue();

    parent.RemoveAllChildren();

    parent.HasContentsChildren.ShouldBeFalse();
  }

  public void ShouldTrackViaInsertChild()
  {
    FlexNode parent = new();
    FlexNode normalChild = new();
    FlexNode contentsChild = new() { Display = Display.Contents };

    parent.AddChild(normalChild);
    parent.InsertChild(contentsChild, 0);

    parent.HasContentsChildren.ShouldBeTrue();
  }
}

/// <summary>
/// Tests for GetLayoutChildren enumeration.
/// </summary>
[TestTag(TestTags.Fast)]
public class GetLayoutChildrenTests
{
  public void ShouldExcludeDisplayNone()
  {
    FlexNode parent = new();
    FlexNode visible = new();
    FlexNode hidden = new() { Display = Display.None };

    parent.AddChild(visible);
    parent.AddChild(hidden);

    List<FlexNode> layoutChildren = [..parent.GetLayoutChildren()];

    layoutChildren.Count.ShouldBe(1);
    layoutChildren[0].ShouldBe(visible);
  }

  public void ShouldFlattenContentsNode()
  {
    FlexNode parent = new();
    FlexNode wrapper = new() { Display = Display.Contents };
    FlexNode child1 = new();
    FlexNode child2 = new();

    wrapper.AddChild(child1);
    wrapper.AddChild(child2);
    parent.AddChild(wrapper);

    List<FlexNode> layoutChildren = [..parent.GetLayoutChildren()];

    layoutChildren.Count.ShouldBe(2);
    layoutChildren[0].ShouldBe(child1);
    layoutChildren[1].ShouldBe(child2);
  }

  public void ShouldPreserveOrderWithMixedChildren()
  {
    FlexNode parent = new();
    FlexNode child1 = new();
    FlexNode wrapper = new() { Display = Display.Contents };
    FlexNode wrappedChild = new();
    FlexNode child2 = new();

    wrapper.AddChild(wrappedChild);
    parent.AddChild(child1);
    parent.AddChild(wrapper);
    parent.AddChild(child2);

    List<FlexNode> layoutChildren = [..parent.GetLayoutChildren()];

    layoutChildren.Count.ShouldBe(3);
    layoutChildren[0].ShouldBe(child1);
    layoutChildren[1].ShouldBe(wrappedChild);
    layoutChildren[2].ShouldBe(child2);
  }

  public void ShouldSkipHiddenChildrenOfContentsNode()
  {
    FlexNode parent = new();
    FlexNode wrapper = new() { Display = Display.Contents };
    FlexNode visible = new();
    FlexNode hidden = new() { Display = Display.None };

    wrapper.AddChild(visible);
    wrapper.AddChild(hidden);
    parent.AddChild(wrapper);

    List<FlexNode> layoutChildren = [..parent.GetLayoutChildren()];

    layoutChildren.Count.ShouldBe(1);
    layoutChildren[0].ShouldBe(visible);
  }

  public void ShouldReturnEmptyForNodeWithOnlyHiddenChildren()
  {
    FlexNode parent = new();
    FlexNode hidden = new() { Display = Display.None };

    parent.AddChild(hidden);

    List<FlexNode> layoutChildren = [..parent.GetLayoutChildren()];

    layoutChildren.Count.ShouldBe(0);
  }

  public void ShouldReturnEmptyForNodeWithOnlyEmptyContentsChild()
  {
    FlexNode parent = new();
    FlexNode emptyWrapper = new() { Display = Display.Contents };

    parent.AddChild(emptyWrapper);

    List<FlexNode> layoutChildren = [..parent.GetLayoutChildren()];

    layoutChildren.Count.ShouldBe(0);
  }
}

/// <summary>
/// Tests for display:contents with wrapping behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class DisplayContentsWrappingTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldWrapFlattenedChildren()
  {
    // Arrange: 3 children of 50px in a 100px container with wrap
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap
    };

    FlexNode wrapper = new() { Display = Display.Contents };
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };

    wrapper.AddChild(child1);
    wrapper.AddChild(child2);
    wrapper.AddChild(child3);
    root.AddChild(wrapper);

    // Act
    Engine.CalculateLayout(root, 100, 100);

    // Assert: child1 and child2 on first line, child3 wraps to second line
    child1.Layout.Left.ShouldBe(0);
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Left.ShouldBe(50);
    child2.Layout.Top.ShouldBe(0);
    child3.Layout.Left.ShouldBe(0);
    child3.Layout.Top.ShouldBe(20);
  }
}
