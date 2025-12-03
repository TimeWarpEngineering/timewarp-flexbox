namespace TimeWarp.Flexbox.Tests.Nodes;

/// <summary>
/// Tests for FlexNodeExtensions fluent API - direction and wrapping methods.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsDirectionTests
{
  public void ShouldSetFlexDirection()
  {
    FlexNode node = new FlexNode().Direction(FlexDirection.Column);

    node.FlexDirection.ShouldBe(FlexDirection.Column);
  }

  public void ShouldSetLayoutDirection()
  {
    FlexNode node = new FlexNode().LayoutDirection(Direction.Rtl);

    node.Direction.ShouldBe(Direction.Rtl);
  }

  public void ShouldSetFlexWrap()
  {
    FlexNode node = new FlexNode().Wrap(FlexWrap.Wrap);

    node.FlexWrap.ShouldBe(FlexWrap.Wrap);
  }

  public void ShouldChainDirectionMethods()
  {
    FlexNode node = new FlexNode()
      .Direction(FlexDirection.Column)
      .Wrap(FlexWrap.Wrap)
      .LayoutDirection(Direction.Rtl);

    node.FlexDirection.ShouldBe(FlexDirection.Column);
    node.FlexWrap.ShouldBe(FlexWrap.Wrap);
    node.Direction.ShouldBe(Direction.Rtl);
  }
}

/// <summary>
/// Tests for FlexNodeExtensions fluent API - alignment methods.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsAlignmentTests
{
  public void ShouldSetJustifyContent()
  {
    FlexNode node = new FlexNode().Justify(JustifyContent.Center);

    node.JustifyContent.ShouldBe(JustifyContent.Center);
  }

  public void ShouldSetAlignItems()
  {
    FlexNode node = new FlexNode().ItemsAlign(AlignItems.Center);

    node.AlignItems.ShouldBe(AlignItems.Center);
  }

  public void ShouldSetAlignContent()
  {
    FlexNode node = new FlexNode().ContentAlign(AlignContent.SpaceBetween);

    node.AlignContent.ShouldBe(AlignContent.SpaceBetween);
  }

  public void ShouldSetAlignSelf()
  {
    FlexNode node = new FlexNode().SelfAlign(AlignSelf.FlexEnd);

    node.AlignSelf.ShouldBe(AlignSelf.FlexEnd);
  }

  public void ShouldChainAlignmentMethods()
  {
    FlexNode node = new FlexNode()
      .Justify(JustifyContent.SpaceEvenly)
      .ItemsAlign(AlignItems.FlexStart)
      .ContentAlign(AlignContent.Center);

    node.JustifyContent.ShouldBe(JustifyContent.SpaceEvenly);
    node.AlignItems.ShouldBe(AlignItems.FlexStart);
    node.AlignContent.ShouldBe(AlignContent.Center);
  }
}

/// <summary>
/// Tests for FlexNodeExtensions fluent API - flex factor methods.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsFlexFactorTests
{
  public void ShouldSetFlexGrow()
  {
    FlexNode node = new FlexNode().Grow(2);

    node.FlexGrow.ShouldBe(2);
  }

  public void ShouldSetFlexShrink()
  {
    FlexNode node = new FlexNode().Shrink(0);

    node.FlexShrink.ShouldBe(0);
  }

  public void ShouldSetFlexBasisWithFlexValue()
  {
    FlexNode node = new FlexNode().Basis(FlexValue.Percent(50));

    node.FlexBasis.ShouldBe(FlexValue.Percent(50));
  }

  public void ShouldSetFlexBasisWithFloat()
  {
    FlexNode node = new FlexNode().Basis(100);

    node.FlexBasis.ShouldBe(FlexValue.Point(100));
  }

  public void ShouldChainFlexFactorMethods()
  {
    FlexNode node = new FlexNode()
      .Grow(1)
      .Shrink(0)
      .Basis(50);

    node.FlexGrow.ShouldBe(1);
    node.FlexShrink.ShouldBe(0);
    node.FlexBasis.ShouldBe(FlexValue.Point(50));
  }
}

/// <summary>
/// Tests for FlexNodeExtensions fluent API - dimension methods.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsDimensionTests
{
  public void ShouldSetSizeWithFloats()
  {
    FlexNode node = new FlexNode().Size(100, 50);

    node.Width.ShouldBe(FlexValue.Point(100));
    node.Height.ShouldBe(FlexValue.Point(50));
  }

  public void ShouldSetSizeWithFlexValues()
  {
    FlexNode node = new FlexNode().Size(FlexValue.Percent(100), FlexValue.Auto);

    node.Width.ShouldBe(FlexValue.Percent(100));
    node.Height.ShouldBe(FlexValue.Auto);
  }

  public void ShouldSetWidthWithFloat()
  {
    FlexNode node = new FlexNode().Width(200);

    node.Width.ShouldBe(FlexValue.Point(200));
  }

  public void ShouldSetWidthWithFlexValue()
  {
    FlexNode node = new FlexNode().Width(FlexValue.Percent(50));

    node.Width.ShouldBe(FlexValue.Percent(50));
  }

  public void ShouldSetHeightWithFloat()
  {
    FlexNode node = new FlexNode().Height(150);

    node.Height.ShouldBe(FlexValue.Point(150));
  }

  public void ShouldSetHeightWithFlexValue()
  {
    FlexNode node = new FlexNode().Height(FlexValue.Auto);

    node.Height.ShouldBe(FlexValue.Auto);
  }

  public void ShouldSetMinWidth()
  {
    FlexNode node = new FlexNode().MinWidth(50);

    node.MinWidth.ShouldBe(FlexValue.Point(50));
  }

  public void ShouldSetMinHeight()
  {
    FlexNode node = new FlexNode().MinHeight(25);

    node.MinHeight.ShouldBe(FlexValue.Point(25));
  }

  public void ShouldSetMaxWidth()
  {
    FlexNode node = new FlexNode().MaxWidth(500);

    node.MaxWidth.ShouldBe(FlexValue.Point(500));
  }

  public void ShouldSetMaxHeight()
  {
    FlexNode node = new FlexNode().MaxHeight(300);

    node.MaxHeight.ShouldBe(FlexValue.Point(300));
  }

  public void ShouldSetAspectRatio()
  {
    FlexNode node = new FlexNode().AspectRatio(16f / 9f);

    node.AspectRatio.ShouldBe(16f / 9f);
  }
}

/// <summary>
/// Tests for FlexNodeExtensions fluent API - margin methods.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsMarginTests
{
  private static readonly FlexValue DefaultValue = FlexValue.Undefined;

  public void ShouldSetMarginAll()
  {
    FlexNode node = new FlexNode().Margin(10);

    node.Margin.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(10));
    node.Margin.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(10));
    node.Margin.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(10));
    node.Margin.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(10));
  }

  public void ShouldSetMarginVerticalHorizontal()
  {
    FlexNode node = new FlexNode().Margin(10, 20);

    node.Margin.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(10));
    node.Margin.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(10));
    node.Margin.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(20));
    node.Margin.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(20));
  }

  public void ShouldSetMarginIndividual()
  {
    FlexNode node = new FlexNode().Margin(1, 2, 3, 4);

    node.Margin[Edge.Top].ShouldBe(FlexValue.Point(1));
    node.Margin[Edge.Right].ShouldBe(FlexValue.Point(2));
    node.Margin[Edge.Bottom].ShouldBe(FlexValue.Point(3));
    node.Margin[Edge.Left].ShouldBe(FlexValue.Point(4));
  }

  public void ShouldSetMarginEdge()
  {
    FlexNode node = new FlexNode().Margin(Edge.Start, FlexValue.Point(15));

    node.Margin[Edge.Start].ShouldBe(FlexValue.Point(15));
  }
}

/// <summary>
/// Tests for FlexNodeExtensions fluent API - padding methods.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsPaddingTests
{
  private static readonly FlexValue DefaultValue = FlexValue.Undefined;

  public void ShouldSetPaddingAll()
  {
    FlexNode node = new FlexNode().Padding(5);

    node.Padding.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(5));
    node.Padding.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(5));
    node.Padding.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(5));
    node.Padding.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(5));
  }

  public void ShouldSetPaddingVerticalHorizontal()
  {
    FlexNode node = new FlexNode().Padding(8, 16);

    node.Padding.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(8));
    node.Padding.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(8));
    node.Padding.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(16));
    node.Padding.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(16));
  }

  public void ShouldSetPaddingIndividual()
  {
    FlexNode node = new FlexNode().Padding(4, 8, 12, 16);

    node.Padding[Edge.Top].ShouldBe(FlexValue.Point(4));
    node.Padding[Edge.Right].ShouldBe(FlexValue.Point(8));
    node.Padding[Edge.Bottom].ShouldBe(FlexValue.Point(12));
    node.Padding[Edge.Left].ShouldBe(FlexValue.Point(16));
  }
}

/// <summary>
/// Tests for FlexNodeExtensions fluent API - border methods.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsBorderTests
{
  public void ShouldSetBorderAll()
  {
    FlexNode node = new FlexNode().Border(2);

    node.Border.ComputedTop(0).ShouldBe(2);
    node.Border.ComputedRight(0).ShouldBe(2);
    node.Border.ComputedBottom(0).ShouldBe(2);
    node.Border.ComputedLeft(0).ShouldBe(2);
  }

  public void ShouldSetBorderVerticalHorizontal()
  {
    FlexNode node = new FlexNode().Border(1, 3);

    node.Border.ComputedTop(0).ShouldBe(1);
    node.Border.ComputedBottom(0).ShouldBe(1);
    node.Border.ComputedLeft(0).ShouldBe(3);
    node.Border.ComputedRight(0).ShouldBe(3);
  }

  public void ShouldSetBorderIndividual()
  {
    FlexNode node = new FlexNode().Border(1, 2, 3, 4);

    node.Border[Edge.Top].ShouldBe(1);
    node.Border[Edge.Right].ShouldBe(2);
    node.Border[Edge.Bottom].ShouldBe(3);
    node.Border[Edge.Left].ShouldBe(4);
  }
}

/// <summary>
/// Tests for FlexNodeExtensions fluent API - position and gap methods.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsPositionGapTests
{
  public void ShouldSetPositionType()
  {
    FlexNode node = new FlexNode().Position(PositionType.Absolute);

    node.PositionType.ShouldBe(PositionType.Absolute);
  }

  public void ShouldSetPositionOffset()
  {
    FlexNode node = new FlexNode().PositionOffset(Edge.Left, 10);

    node.Position[Edge.Left].ShouldBe(FlexValue.Point(10));
  }

  public void ShouldSetPositionOffsetWithFlexValue()
  {
    FlexNode node = new FlexNode().PositionOffset(Edge.Top, FlexValue.Percent(25));

    node.Position[Edge.Top].ShouldBe(FlexValue.Percent(25));
  }

  public void ShouldSetGapBoth()
  {
    FlexNode node = new FlexNode().Gap(10);

    node.RowGap.ShouldBe(10);
    node.ColumnGap.ShouldBe(10);
  }

  public void ShouldSetGapSeparate()
  {
    FlexNode node = new FlexNode().Gap(5, 15);

    node.RowGap.ShouldBe(5);
    node.ColumnGap.ShouldBe(15);
  }
}

/// <summary>
/// Tests for FlexNodeExtensions fluent API - other properties.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsOtherTests
{
  public void ShouldSetDisplay()
  {
    FlexNode node = new FlexNode().Display(Flexbox.Display.None);

    node.Display.ShouldBe(Flexbox.Display.None);
  }

  public void ShouldSetOverflow()
  {
    FlexNode node = new FlexNode().Overflow(Flexbox.Overflow.Hidden);

    node.Overflow.ShouldBe(Flexbox.Overflow.Hidden);
  }
}

/// <summary>
/// Tests for FlexNodeExtensions fluent API - children methods.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsChildrenTests
{
  public void ShouldAddSingleChild()
  {
    FlexNode child = new();
    FlexNode parent = new FlexNode().AddChildren(child);

    parent.Children.Count.ShouldBe(1);
    parent.Children[0].ShouldBe(child);
  }

  public void ShouldAddMultipleChildren()
  {
    FlexNode child1 = new();
    FlexNode child2 = new();
    FlexNode child3 = new();

    FlexNode parent = new FlexNode().AddChildren(child1, child2, child3);

    parent.Children.Count.ShouldBe(3);
    parent.Children[0].ShouldBe(child1);
    parent.Children[1].ShouldBe(child2);
    parent.Children[2].ShouldBe(child3);
  }

  public void ShouldSetParentReference()
  {
    FlexNode child = new();
    FlexNode parent = new FlexNode().AddChildren(child);

    child.Parent.ShouldBe(parent);
  }
}

/// <summary>
/// Tests for FlexNodeExtensions fluent API - full chaining scenarios.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsChainingTests
{
  public void ShouldBuildCompleteLayoutWithFluent()
  {
    FlexNode root = new FlexNode()
      .Direction(FlexDirection.Column)
      .Size(800, 600)
      .Padding(10)
      .AddChildren(
        new FlexNode()
          .Direction(FlexDirection.Row)
          .Grow(1)
          .Gap(5)
          .AddChildren(
            new FlexNode().Grow(1),
            new FlexNode().Size(200, 100)
          ),
        new FlexNode().Height(50)
      );

    root.FlexDirection.ShouldBe(FlexDirection.Column);
    root.Width.ShouldBe(FlexValue.Point(800));
    root.Height.ShouldBe(FlexValue.Point(600));
    root.Children.Count.ShouldBe(2);
    root.Children[0].FlexDirection.ShouldBe(FlexDirection.Row);
    root.Children[0].FlexGrow.ShouldBe(1);
    root.Children[0].Children.Count.ShouldBe(2);
    root.Children[1].Height.ShouldBe(FlexValue.Point(50));
  }

  public void ShouldWorkWithLayoutEngine()
  {
    FlexNode root = new FlexNode()
      .Direction(FlexDirection.Row)
      .Size(300, 100)
      .AddChildren(
        new FlexNode().Width(100),
        new FlexNode().Grow(1),
        new FlexNode().Width(50)
      );

    FlexLayoutEngine engine = new();
    engine.CalculateLayout(root, 300, 100);

    root.Layout.Width.ShouldBe(300);
    root.Layout.Height.ShouldBe(100);
    root.Children[0].Layout.Width.ShouldBe(100);
    root.Children[1].Layout.Width.ShouldBe(150); // Remaining space
    root.Children[2].Layout.Width.ShouldBe(50);
  }
}

/// <summary>
/// Tests for FlexNodeExtensions null argument handling.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodeExtensionsNullTests
{
  public void ShouldThrowOnNullNodeForDirection()
  {
    FlexNode? node = null;
    Should.Throw<ArgumentNullException>(() => node!.Direction(FlexDirection.Row));
  }

  public void ShouldThrowOnNullNodeForSize()
  {
    FlexNode? node = null;
    Should.Throw<ArgumentNullException>(() => node!.Size(100, 100));
  }

  public void ShouldThrowOnNullNodeForAddChildren()
  {
    FlexNode? node = null;
    Should.Throw<ArgumentNullException>(() => node!.AddChildren(new FlexNode()));
  }

  public void ShouldThrowOnNullChildrenArray()
  {
    FlexNode node = new();
    Should.Throw<ArgumentNullException>(() => node.AddChildren(null!));
  }
}
