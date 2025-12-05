namespace TimeWarp.Flexbox;

/// <summary>
/// Fluent extension methods for FlexNode configuration.
/// Enables declarative, chainable node setup.
/// </summary>
public static class FlexNodeExtensions
{
  #region Direction & Wrapping

  /// <summary>
  /// Sets the flex direction (main axis orientation).
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="direction">The flex direction.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Direction(this FlexNode node, FlexDirection direction)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.FlexDirection = direction;
    return node;
  }

  /// <summary>
  /// Sets the layout direction (LTR/RTL).
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="direction">The layout direction.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode LayoutDirection(this FlexNode node, Direction direction)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.Direction = direction;
    return node;
  }

  /// <summary>
  /// Sets the flex wrap behavior.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="wrap">The wrap behavior.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Wrap(this FlexNode node, FlexWrap wrap)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.FlexWrap = wrap;
    return node;
  }

  #endregion

  #region Alignment

  /// <summary>
  /// Sets how flex items are distributed along the main axis.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="justify">The justify content value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Justify(this FlexNode node, JustifyContent justify)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.JustifyContent = justify;
    return node;
  }

  /// <summary>
  /// Sets the default alignment for items along the cross axis.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="align">The align items value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode ItemsAlign(this FlexNode node, AlignItems align)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.AlignItems = align;
    return node;
  }

  /// <summary>
  /// Sets how flex lines are distributed along the cross axis.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="align">The align content value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode ContentAlign(this FlexNode node, AlignContent align)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.AlignContent = align;
    return node;
  }

  /// <summary>
  /// Sets the alignment for this specific item, overriding parent's AlignItems.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="align">The align self value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode SelfAlign(this FlexNode node, AlignSelf align)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.AlignSelf = align;
    return node;
  }

  #endregion

  #region Flex Factors

  /// <summary>
  /// Sets the flex grow factor.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="grow">The grow factor.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Grow(this FlexNode node, float grow)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.FlexGrow = grow;
    return node;
  }

  /// <summary>
  /// Sets the flex shrink factor.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="shrink">The shrink factor.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Shrink(this FlexNode node, float shrink)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.FlexShrink = shrink;
    return node;
  }

  /// <summary>
  /// Sets the flex basis (initial main size).
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="basis">The flex basis value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Basis(this FlexNode node, FlexValue basis)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.FlexBasis = basis;
    return node;
  }

  /// <summary>
  /// Sets the flex basis to a point value.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="basis">The flex basis in points.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Basis(this FlexNode node, float basis)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.FlexBasis = FlexValue.Point(basis);
    return node;
  }

  #endregion

  #region Dimensions

  /// <summary>
  /// Sets both width and height.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="width">The width in points.</param>
  /// <param name="height">The height in points.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Size(this FlexNode node, float width, float height)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.Width = FlexValue.Point(width);
    node.Height = FlexValue.Point(height);
    return node;
  }

  /// <summary>
  /// Sets both width and height using FlexValue.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="width">The width value.</param>
  /// <param name="height">The height value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Size(this FlexNode node, FlexValue width, FlexValue height)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.Width = width;
    node.Height = height;
    return node;
  }

  /// <summary>
  /// Sets the width.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="width">The width in points.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Width(this FlexNode node, float width)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.Width = FlexValue.Point(width);
    return node;
  }

  /// <summary>
  /// Sets the width using FlexValue.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="width">The width value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Width(this FlexNode node, FlexValue width)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.Width = width;
    return node;
  }

  /// <summary>
  /// Sets the height.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="height">The height in points.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Height(this FlexNode node, float height)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.Height = FlexValue.Point(height);
    return node;
  }

  /// <summary>
  /// Sets the height using FlexValue.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="height">The height value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Height(this FlexNode node, FlexValue height)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.Height = height;
    return node;
  }

  /// <summary>
  /// Sets the minimum width.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="minWidth">The minimum width in points.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode MinWidth(this FlexNode node, float minWidth)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.MinWidth = FlexValue.Point(minWidth);
    return node;
  }

  /// <summary>
  /// Sets the minimum height.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="minHeight">The minimum height in points.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode MinHeight(this FlexNode node, float minHeight)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.MinHeight = FlexValue.Point(minHeight);
    return node;
  }

  /// <summary>
  /// Sets the maximum width.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="maxWidth">The maximum width in points.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode MaxWidth(this FlexNode node, float maxWidth)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.MaxWidth = FlexValue.Point(maxWidth);
    return node;
  }

  /// <summary>
  /// Sets the maximum height.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="maxHeight">The maximum height in points.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode MaxHeight(this FlexNode node, float maxHeight)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.MaxHeight = FlexValue.Point(maxHeight);
    return node;
  }

  /// <summary>
  /// Sets the aspect ratio (width / height).
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="ratio">The aspect ratio.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode AspectRatio(this FlexNode node, float ratio)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.AspectRatio = ratio;
    return node;
  }

  #endregion

  #region Margin

  /// <summary>
  /// Sets margin on all edges.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="all">The margin value for all edges.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Margin(this FlexNode node, float all)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetMargin(Edge.All, FlexValue.Point(all));
    return node;
  }

  /// <summary>
  /// Sets margin with vertical and horizontal values.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="vertical">The top and bottom margin.</param>
  /// <param name="horizontal">The left and right margin.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Margin(this FlexNode node, float vertical, float horizontal)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetMargin(Edge.Vertical, FlexValue.Point(vertical));
    node.SetMargin(Edge.Horizontal, FlexValue.Point(horizontal));
    return node;
  }

  /// <summary>
  /// Sets margin on each edge individually.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="top">The top margin.</param>
  /// <param name="right">The right margin.</param>
  /// <param name="bottom">The bottom margin.</param>
  /// <param name="left">The left margin.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Margin(this FlexNode node, float top, float right, float bottom, float left)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetMargin(Edge.Top, FlexValue.Point(top));
    node.SetMargin(Edge.Right, FlexValue.Point(right));
    node.SetMargin(Edge.Bottom, FlexValue.Point(bottom));
    node.SetMargin(Edge.Left, FlexValue.Point(left));
    return node;
  }

  /// <summary>
  /// Sets margin on a specific edge.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="edge">The edge to set.</param>
  /// <param name="value">The margin value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Margin(this FlexNode node, Edge edge, FlexValue value)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetMargin(edge, value);
    return node;
  }

  #endregion

  #region Padding

  /// <summary>
  /// Sets padding on all edges.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="all">The padding value for all edges.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Padding(this FlexNode node, float all)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetPadding(Edge.All, FlexValue.Point(all));
    return node;
  }

  /// <summary>
  /// Sets padding with vertical and horizontal values.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="vertical">The top and bottom padding.</param>
  /// <param name="horizontal">The left and right padding.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Padding(this FlexNode node, float vertical, float horizontal)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetPadding(Edge.Vertical, FlexValue.Point(vertical));
    node.SetPadding(Edge.Horizontal, FlexValue.Point(horizontal));
    return node;
  }

  /// <summary>
  /// Sets padding on each edge individually.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="top">The top padding.</param>
  /// <param name="right">The right padding.</param>
  /// <param name="bottom">The bottom padding.</param>
  /// <param name="left">The left padding.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Padding(this FlexNode node, float top, float right, float bottom, float left)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetPadding(Edge.Top, FlexValue.Point(top));
    node.SetPadding(Edge.Right, FlexValue.Point(right));
    node.SetPadding(Edge.Bottom, FlexValue.Point(bottom));
    node.SetPadding(Edge.Left, FlexValue.Point(left));
    return node;
  }

  /// <summary>
  /// Sets padding on a specific edge.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="edge">The edge to set.</param>
  /// <param name="value">The padding value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Padding(this FlexNode node, Edge edge, FlexValue value)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetPadding(edge, value);
    return node;
  }

  #endregion

  #region Border

  /// <summary>
  /// Sets border width on all edges.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="all">The border width for all edges.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Border(this FlexNode node, float all)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetBorder(Edge.All, all);
    return node;
  }

  /// <summary>
  /// Sets border width with vertical and horizontal values.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="vertical">The top and bottom border width.</param>
  /// <param name="horizontal">The left and right border width.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Border(this FlexNode node, float vertical, float horizontal)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetBorder(Edge.Vertical, vertical);
    node.SetBorder(Edge.Horizontal, horizontal);
    return node;
  }

  /// <summary>
  /// Sets border width on each edge individually.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="top">The top border width.</param>
  /// <param name="right">The right border width.</param>
  /// <param name="bottom">The bottom border width.</param>
  /// <param name="left">The left border width.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Border(this FlexNode node, float top, float right, float bottom, float left)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetBorder(Edge.Top, top);
    node.SetBorder(Edge.Right, right);
    node.SetBorder(Edge.Bottom, bottom);
    node.SetBorder(Edge.Left, left);
    return node;
  }

  /// <summary>
  /// Sets border width on a specific edge.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="edge">The edge to set.</param>
  /// <param name="value">The border width.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Border(this FlexNode node, Edge edge, float value)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetBorder(edge, value);
    return node;
  }

  #endregion

  #region Position

  /// <summary>
  /// Sets the position type.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="positionType">The position type.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Position(this FlexNode node, PositionType positionType)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.PositionType = positionType;
    return node;
  }

  /// <summary>
  /// Sets position offset on a specific edge.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="edge">The edge to set.</param>
  /// <param name="value">The position offset.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode PositionOffset(this FlexNode node, Edge edge, float value)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetPosition(edge, FlexValue.Point(value));
    return node;
  }

  /// <summary>
  /// Sets position offset on a specific edge using FlexValue.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="edge">The edge to set.</param>
  /// <param name="value">The position offset value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode PositionOffset(this FlexNode node, Edge edge, FlexValue value)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.SetPosition(edge, value);
    return node;
  }

  #endregion

  #region Gap

  /// <summary>
  /// Sets gap between items on both axes.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="gap">The gap value.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Gap(this FlexNode node, float gap)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.Gap = gap;
    return node;
  }

  /// <summary>
  /// Sets row and column gap separately.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="rowGap">The gap between rows.</param>
  /// <param name="columnGap">The gap between columns.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Gap(this FlexNode node, float rowGap, float columnGap)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.RowGap = rowGap;
    node.ColumnGap = columnGap;
    return node;
  }

  #endregion

  #region Other Properties

  /// <summary>
  /// Sets the display mode.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="display">The display mode.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Display(this FlexNode node, Display display)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.Display = display;
    return node;
  }

  /// <summary>
  /// Sets the overflow behavior.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="overflow">The overflow behavior.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode Overflow(this FlexNode node, Overflow overflow)
  {
    ArgumentNullException.ThrowIfNull(node);
    node.Overflow = overflow;
    return node;
  }

  #endregion

  #region Children

  /// <summary>
  /// Adds multiple children to the node.
  /// </summary>
  /// <param name="node">The node to configure.</param>
  /// <param name="children">The children to add.</param>
  /// <returns>The node for chaining.</returns>
  public static FlexNode AddChildren(this FlexNode node, params FlexNode[] children)
  {
    ArgumentNullException.ThrowIfNull(node);
    ArgumentNullException.ThrowIfNull(children);

    foreach (FlexNode child in children)
    {
      node.AddChild(child);
    }

    return node;
  }

  #endregion
}
