/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for LayoutableChildren iterator
 */

namespace TimeWarp.Flexbox.Tests.Node;

/// <summary>
/// Mock node implementation for testing LayoutableChildren.
/// </summary>
public sealed class MockNode : ILayoutableNode
{
  private readonly List<MockNode> _children = [];
  private Display _display = Display.Flex;

  /// <summary>
  /// Gets or sets the name for debugging purposes.
  /// </summary>
  public string Name { get; init; } = string.Empty;

  /// <inheritdoc />
  public ILayoutableNode GetChild(int index) => _children[index];

  /// <inheritdoc />
  public int GetChildCount() => _children.Count;

  /// <inheritdoc />
  public Display GetDisplay() => _display;

  /// <summary>
  /// Sets the display type for this mock node.
  /// </summary>
  public MockNode WithDisplay(Display display)
  {
    _display = display;
    return this;
  }

  /// <summary>
  /// Adds a child to this mock node.
  /// </summary>
  public MockNode AddChild(MockNode child)
  {
    _children.Add(child);
    return this;
  }
}

/// <summary>
/// Tests for LayoutableChildren iterator.
/// </summary>
public class LayoutableChildrenTests
{
  #region Empty and Null Cases

  public void EmptyParentShouldReturnNoChildren()
  {
    // Arrange
    MockNode parent = new() { Name = "Parent" };
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert
    children.ShouldBeEmpty();
  }

  public void NullParentShouldReturnNoChildren()
  {
    // Arrange
    LayoutableChildren<MockNode> layoutableChildren = new(null);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert
    children.ShouldBeEmpty();
  }

  #endregion

  #region Basic Iteration

  public void SingleChildShouldReturnChild()
  {
    // Arrange
    MockNode child = new() { Name = "Child" };
    MockNode parent = new() { Name = "Parent" };
    parent.AddChild(child);
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert
    children.Count.ShouldBe(1);
    children[0].ShouldBeSameAs(child);
  }

  public void MultipleChildrenShouldReturnAllInOrder()
  {
    // Arrange
    MockNode child1 = new() { Name = "Child1" };
    MockNode child2 = new() { Name = "Child2" };
    MockNode child3 = new() { Name = "Child3" };
    MockNode parent = new() { Name = "Parent" };
    parent.AddChild(child1).AddChild(child2).AddChild(child3);
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert
    children.Count.ShouldBe(3);
    children[0].ShouldBeSameAs(child1);
    children[1].ShouldBeSameAs(child2);
    children[2].ShouldBeSameAs(child3);
  }

  #endregion

  #region Display:Contents Handling

  public void DisplayContentsChildrenShouldBeFlattened()
  {
    // Arrange
    // Parent
    //   -> ContentsNode (display: contents)
    //      -> GrandChild1
    //      -> GrandChild2
    MockNode grandChild1 = new() { Name = "GrandChild1" };
    MockNode grandChild2 = new() { Name = "GrandChild2" };
    MockNode contentsNode = new MockNode { Name = "ContentsNode" }
        .WithDisplay(Display.Contents)
        .AddChild(grandChild1)
        .AddChild(grandChild2);
    MockNode parent = new() { Name = "Parent" };
    parent.AddChild(contentsNode);
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert - Should return grandchildren, not the contents node
    children.Count.ShouldBe(2);
    children[0].ShouldBeSameAs(grandChild1);
    children[1].ShouldBeSameAs(grandChild2);
  }

  public void DisplayContentsWithSiblingsShouldFlattenCorrectly()
  {
    // Arrange
    // Parent
    //   -> Child1
    //   -> ContentsNode (display: contents)
    //      -> GrandChild
    //   -> Child3
    MockNode child1 = new() { Name = "Child1" };
    MockNode grandChild = new() { Name = "GrandChild" };
    MockNode contentsNode = new MockNode { Name = "ContentsNode" }
        .WithDisplay(Display.Contents)
        .AddChild(grandChild);
    MockNode child3 = new() { Name = "Child3" };
    MockNode parent = new() { Name = "Parent" };
    parent.AddChild(child1).AddChild(contentsNode).AddChild(child3);
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert
    children.Count.ShouldBe(3);
    children[0].ShouldBeSameAs(child1);
    children[1].ShouldBeSameAs(grandChild);
    children[2].ShouldBeSameAs(child3);
  }

  public void NestedDisplayContentsShouldFlattenDeeply()
  {
    // Arrange
    // Parent
    //   -> ContentsNode1 (display: contents)
    //      -> ContentsNode2 (display: contents)
    //         -> DeepChild
    MockNode deepChild = new() { Name = "DeepChild" };
    MockNode contentsNode2 = new MockNode { Name = "ContentsNode2" }
        .WithDisplay(Display.Contents)
        .AddChild(deepChild);
    MockNode contentsNode1 = new MockNode { Name = "ContentsNode1" }
        .WithDisplay(Display.Contents)
        .AddChild(contentsNode2);
    MockNode parent = new() { Name = "Parent" };
    parent.AddChild(contentsNode1);
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert
    children.Count.ShouldBe(1);
    children[0].ShouldBeSameAs(deepChild);
  }

  public void DisplayContentsEmptyContentsNodeShouldBeSkipped()
  {
    // Arrange
    // Parent
    //   -> Child1
    //   -> EmptyContentsNode (display: contents, no children)
    //   -> Child2
    MockNode child1 = new() { Name = "Child1" };
    MockNode emptyContentsNode = new MockNode { Name = "EmptyContentsNode" }
        .WithDisplay(Display.Contents);
    MockNode child2 = new() { Name = "Child2" };
    MockNode parent = new() { Name = "Parent" };
    parent.AddChild(child1).AddChild(emptyContentsNode).AddChild(child2);
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert
    children.Count.ShouldBe(2);
    children[0].ShouldBeSameAs(child1);
    children[1].ShouldBeSameAs(child2);
  }

  public void DisplayContentsFirstChildIsContentsShouldFlatten()
  {
    // Arrange
    // Parent
    //   -> ContentsNode (display: contents)
    //      -> GrandChild
    //   -> Child2
    MockNode grandChild = new() { Name = "GrandChild" };
    MockNode contentsNode = new MockNode { Name = "ContentsNode" }
        .WithDisplay(Display.Contents)
        .AddChild(grandChild);
    MockNode child2 = new() { Name = "Child2" };
    MockNode parent = new() { Name = "Parent" };
    parent.AddChild(contentsNode).AddChild(child2);
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert
    children.Count.ShouldBe(2);
    children[0].ShouldBeSameAs(grandChild);
    children[1].ShouldBeSameAs(child2);
  }

  #endregion

  #region Display:None is NOT filtered

  public void DisplayNoneShouldNotBeFilteredByIterator()
  {
    // Arrange - display:none filtering is done in algorithm, not iterator
    MockNode visibleChild = new() { Name = "VisibleChild" };
    MockNode hiddenChild = new MockNode { Name = "HiddenChild" }
        .WithDisplay(Display.None);
    MockNode parent = new() { Name = "Parent" };
    parent.AddChild(visibleChild).AddChild(hiddenChild);
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert - Both should be returned (filtering happens elsewhere)
    children.Count.ShouldBe(2);
    children[0].ShouldBeSameAs(visibleChild);
    children[1].ShouldBeSameAs(hiddenChild);
  }

  #endregion

  #region Equality

  public void EqualitySameParentShouldBeEqual()
  {
    // Arrange
    MockNode parent = new() { Name = "Parent" };
    LayoutableChildren<MockNode> children1 = new(parent);
    LayoutableChildren<MockNode> children2 = new(parent);

    // Assert
    children1.ShouldBe(children2);
    (children1 == children2).ShouldBeTrue();
    (children1 != children2).ShouldBeFalse();
    children1.GetHashCode().ShouldBe(children2.GetHashCode());
  }

  public void EqualityDifferentParentShouldNotBeEqual()
  {
    // Arrange
    MockNode parent1 = new() { Name = "Parent1" };
    MockNode parent2 = new() { Name = "Parent2" };
    LayoutableChildren<MockNode> children1 = new(parent1);
    LayoutableChildren<MockNode> children2 = new(parent2);

    // Assert - Use Equals directly since ShouldNotBe compares by enumerable contents
    children1.Equals(children2).ShouldBeFalse();
    (children1 == children2).ShouldBeFalse();
    (children1 != children2).ShouldBeTrue();
  }

  public void EqualityNullParentsShouldBeEqual()
  {
    // Arrange
    LayoutableChildren<MockNode> children1 = new(null);
    LayoutableChildren<MockNode> children2 = new(null);

    // Assert
    children1.ShouldBe(children2);
  }

  #endregion

  #region Foreach Support

  public void ForeachShouldWorkCorrectly()
  {
    // Arrange
    MockNode child1 = new() { Name = "Child1" };
    MockNode child2 = new() { Name = "Child2" };
    MockNode parent = new() { Name = "Parent" };
    parent.AddChild(child1).AddChild(child2);
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act - Using manual foreach loop to test the foreach functionality
    int count = 0;
    MockNode? firstChild = null;
    MockNode? secondChild = null;
    foreach (MockNode child in layoutableChildren)
    {
      if (count == 0) firstChild = child;
      else if (count == 1) secondChild = child;
      count++;
    }

    // Assert
    count.ShouldBe(2);
    firstChild.ShouldBeSameAs(child1);
    secondChild.ShouldBeSameAs(child2);
  }

  #endregion

  #region Enumerator Reset

  public void ResetShouldThrowNotSupportedException()
  {
    // Arrange
    MockNode parent = new() { Name = "Parent" };
    LayoutableChildren<MockNode> layoutableChildren = new(parent);
    using LayoutableChildren<MockNode>.Enumerator enumerator = layoutableChildren.GetEnumerator();

    // Act & Assert
    Should.Throw<NotSupportedException>(() => enumerator.Reset());
  }

  #endregion

  #region Complex Scenarios

  public void ComplexTreeMultipleContentsWithBacktracking()
  {
    // Arrange
    // Parent
    //   -> Contents1 (display: contents)
    //      -> GrandChild1
    //      -> Contents1_1 (display: contents)
    //         -> GreatGrandChild1
    //   -> Child2
    //   -> Contents2 (display: contents)
    //      -> GrandChild2
    MockNode greatGrandChild1 = new() { Name = "GreatGrandChild1" };
    MockNode contents11 = new MockNode { Name = "Contents1_1" }
        .WithDisplay(Display.Contents)
        .AddChild(greatGrandChild1);
    MockNode grandChild1 = new() { Name = "GrandChild1" };
    MockNode contents1 = new MockNode { Name = "Contents1" }
        .WithDisplay(Display.Contents)
        .AddChild(grandChild1)
        .AddChild(contents11);
    MockNode child2 = new() { Name = "Child2" };
    MockNode grandChild2 = new() { Name = "GrandChild2" };
    MockNode contents2 = new MockNode { Name = "Contents2" }
        .WithDisplay(Display.Contents)
        .AddChild(grandChild2);
    MockNode parent = new() { Name = "Parent" };
    parent.AddChild(contents1).AddChild(child2).AddChild(contents2);
    LayoutableChildren<MockNode> layoutableChildren = new(parent);

    // Act
    List<MockNode> children = [.. layoutableChildren];

    // Assert
    children.Count.ShouldBe(4);
    children[0].ShouldBeSameAs(grandChild1);
    children[1].ShouldBeSameAs(greatGrandChild1);
    children[2].ShouldBeSameAs(child2);
    children[3].ShouldBeSameAs(grandChild2);
  }

  #endregion
}
