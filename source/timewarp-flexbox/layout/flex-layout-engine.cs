namespace TimeWarp.Flexbox;

/// <summary>
/// Core flexbox layout engine that calculates positions and sizes for all nodes.
/// Implements the CSS Flexbox layout algorithm.
/// </summary>
public sealed class FlexLayoutEngine
{
  private readonly FlexLines FlexLinesCache = new();

  /// <summary>
  /// Calculates the layout for a node tree.
  /// </summary>
  /// <param name="root">The root node of the layout tree.</param>
  /// <param name="availableWidth">The available width for layout.</param>
  /// <param name="availableHeight">The available height for layout.</param>
  /// <param name="direction">The layout direction (LTR/RTL).</param>
  /// <exception cref="ArgumentNullException">Thrown when root is null.</exception>
  public void CalculateLayout(
    FlexNode root,
    float availableWidth,
    float availableHeight,
    Direction direction = Direction.Ltr)
  {
    ArgumentNullException.ThrowIfNull(root);

    // Reset layout results for the entire tree
    ResetLayoutResults(root);

    // Calculate layout recursively starting from root
    LayoutNode(
      root,
      availableWidth,
      MeasureMode.Exactly,
      availableHeight,
      MeasureMode.Exactly,
      direction);
  }

  /// <summary>
  /// Recursively resets layout results for a node and its descendants.
  /// </summary>
  private static void ResetLayoutResults(FlexNode node)
  {
    node.Layout.Reset();

    foreach (FlexNode child in node.Children)
    {
      ResetLayoutResults(child);
    }
  }

  /// <summary>
  /// Performs layout calculation for a single node.
  /// </summary>
  private void LayoutNode(
    FlexNode node,
    float availableWidth,
    MeasureMode widthMode,
    float availableHeight,
    MeasureMode heightMode,
    Direction direction)
  {
    // Skip nodes with Display.None
    if (node.Display == Display.None)
      return;

    // Resolve the effective flex direction based on RTL
    FlexDirection resolvedDirection = LayoutHelpers.ResolveFlexDirection(node.FlexDirection, direction);
    bool isMainAxisRow = LayoutHelpers.IsRow(resolvedDirection);

    // Calculate available inner dimensions (excluding padding and border)
    float paddingBorderLeft = GetPaddingAndBorder(node, Edge.Left, direction);
    float paddingBorderRight = GetPaddingAndBorder(node, Edge.Right, direction);
    float paddingBorderTop = GetPaddingAndBorder(node, Edge.Top, direction);
    float paddingBorderBottom = GetPaddingAndBorder(node, Edge.Bottom, direction);

    float paddingBorderMainStart = isMainAxisRow ? paddingBorderLeft : paddingBorderTop;
    float paddingBorderMainEnd = isMainAxisRow ? paddingBorderRight : paddingBorderBottom;
    float paddingBorderCrossStart = isMainAxisRow ? paddingBorderTop : paddingBorderLeft;
    float paddingBorderCrossEnd = isMainAxisRow ? paddingBorderBottom : paddingBorderRight;

    float availableInnerWidth = availableWidth - paddingBorderLeft - paddingBorderRight;
    float availableInnerHeight = availableHeight - paddingBorderTop - paddingBorderBottom;

    float availableInnerMainSize = isMainAxisRow ? availableInnerWidth : availableInnerHeight;
    float availableInnerCrossSize = isMainAxisRow ? availableInnerHeight : availableInnerWidth;

    // Handle leaf nodes with measure function
    if (node.IsLeaf && node.HasMeasureFunc)
    {
      MeasureLeafNode(node, availableWidth, widthMode, availableHeight, heightMode);
      return;
    }

    // Handle leaf nodes without children
    if (node.ChildCount == 0)
    {
      CalculateNodeSize(node, availableWidth, widthMode, availableHeight, heightMode);
      return;
    }

    // Collect children into flex lines
    FlexLinesCache.CollectLines(node, availableInnerMainSize, resolvedDirection, node.FlexWrap);

    // Process each flex line
    float totalLineCrossSize = 0;

    foreach (FlexLine line in FlexLinesCache.Lines)
    {
      // Calculate main axis sizes for items in this line
      CalculateMainAxisSizes(line, availableInnerMainSize, resolvedDirection);

      // Resolve flexible lengths (grow/shrink)
      ResolveFlexibleLengths(line, availableInnerMainSize);

      // Calculate cross axis sizes
      float lineCrossSize = CalculateCrossAxisSizes(
        line,
        availableInnerCrossSize,
        resolvedDirection,
        node.AlignItems);

      line.CrossSize = lineCrossSize;
      totalLineCrossSize += lineCrossSize;
    }

    // Calculate the node's own size
    CalculateNodeSizeFromChildren(
      node,
      availableWidth,
      widthMode,
      availableHeight,
      heightMode,
      paddingBorderLeft + paddingBorderRight,
      paddingBorderTop + paddingBorderBottom);

    // Position children within the node
    PositionChildren(
      node,
      FlexLinesCache,
      resolvedDirection,
      paddingBorderMainStart,
      paddingBorderCrossStart);

    // Recursively layout children
    foreach (FlexNode child in node.Children)
    {
      if (child.Display == Display.None)
        continue;

      if (child.PositionType == PositionType.Absolute)
      {
        LayoutAbsoluteChild(node, child);
        continue;
      }

      // Layout child with its calculated size
      LayoutNode(
        child,
        child.Layout.Width,
        MeasureMode.Exactly,
        child.Layout.Height,
        MeasureMode.Exactly,
        direction);
    }
  }

  /// <summary>
  /// Measures a leaf node using its measure function.
  /// </summary>
  private static void MeasureLeafNode(
    FlexNode node,
    float availableWidth,
    MeasureMode widthMode,
    float availableHeight,
    MeasureMode heightMode)
  {
    if (node.MeasureFunc is null)
      return;

    Size measuredSize = node.MeasureFunc(node, availableWidth, widthMode, availableHeight, heightMode);

    node.Layout.Width = ConstrainSize(measuredSize.Width, node.MinWidth, node.MaxWidth, availableWidth);
    node.Layout.Height = ConstrainSize(measuredSize.Height, node.MinHeight, node.MaxHeight, availableHeight);
  }

  /// <summary>
  /// Calculates the size of a node without children.
  /// </summary>
  private static void CalculateNodeSize(
    FlexNode node,
    float availableWidth,
    MeasureMode widthMode,
    float availableHeight,
    MeasureMode heightMode)
  {
    float width = ValueResolver.ResolveWidth(node, availableWidth);
    float height = ValueResolver.ResolveHeight(node, availableHeight);

    if (float.IsNaN(width))
    {
      width = widthMode == MeasureMode.Exactly ? availableWidth : 0;
    }

    if (float.IsNaN(height))
    {
      height = heightMode == MeasureMode.Exactly ? availableHeight : 0;
    }

    node.Layout.Width = width;
    node.Layout.Height = height;
  }

  /// <summary>
  /// Calculates main axis sizes for items in a flex line.
  /// </summary>
  private static void CalculateMainAxisSizes(
    FlexLine line,
    float availableMainSize,
    FlexDirection direction)
  {
    bool isRow = LayoutHelpers.IsRow(direction);

    foreach (FlexNode child in line.Items)
    {
      float childMainSize;

      // Resolve flex-basis or use explicit width/height
      if (child.FlexBasis.IsDefined)
      {
        childMainSize = ValueResolver.ResolveValue(child.FlexBasis, availableMainSize);
      }
      else
      {
        FlexValue dimension = isRow ? child.Width : child.Height;
        childMainSize = ValueResolver.ResolveValue(dimension, availableMainSize);
      }

      if (float.IsNaN(childMainSize))
      {
        childMainSize = 0;
      }

      // Apply min/max constraints
      if (isRow)
      {
        childMainSize = ConstrainSize(
          childMainSize,
          child.MinWidth,
          child.MaxWidth,
          availableMainSize);

        child.Layout.Width = childMainSize;
      }
      else
      {
        childMainSize = ConstrainSize(
          childMainSize,
          child.MinHeight,
          child.MaxHeight,
          availableMainSize);

        child.Layout.Height = childMainSize;
      }
    }
  }

  /// <summary>
  /// Resolves flexible lengths by distributing remaining space.
  /// </summary>
  private static void ResolveFlexibleLengths(FlexLine line, float availableMainSize)
  {
    if (float.IsNaN(availableMainSize))
      return;

    // Calculate used main size
    float usedMainSize = 0;

    foreach (FlexNode child in line.Items)
    {
      usedMainSize += child.Layout.Width; // Simplified - should use main axis size
    }

    float remainingSpace = availableMainSize - usedMainSize;
    line.RemainingFreeSpace = remainingSpace;

    if (remainingSpace > 0 && line.TotalFlexGrow > 0)
    {
      // Distribute extra space according to flex-grow
      DistributeFlexGrow(line, remainingSpace);
    }
    else if (remainingSpace < 0 && line.TotalFlexShrink > 0)
    {
      // Shrink items according to flex-shrink
      DistributeFlexShrink(line, -remainingSpace);
    }
  }

  /// <summary>
  /// Distributes extra space according to flex-grow factors.
  /// </summary>
  private static void DistributeFlexGrow(FlexLine line, float extraSpace)
  {
    foreach (FlexNode child in line.Items)
    {
      if (child.FlexGrow <= 0)
        continue;

      float growAmount = extraSpace * (child.FlexGrow / line.TotalFlexGrow);
      child.Layout.Width += growAmount;
    }
  }

  /// <summary>
  /// Shrinks items according to flex-shrink factors.
  /// </summary>
  private static void DistributeFlexShrink(FlexLine line, float shrinkAmount)
  {
    foreach (FlexNode child in line.Items)
    {
      if (child.FlexShrink <= 0)
        continue;

      float shrinkRatio = child.FlexShrink / line.TotalFlexShrink;
      float itemShrink = shrinkAmount * shrinkRatio;
      child.Layout.Width = Math.Max(0, child.Layout.Width - itemShrink);
    }
  }

  /// <summary>
  /// Calculates cross axis sizes for items in a flex line.
  /// </summary>
  private static float CalculateCrossAxisSizes(
    FlexLine line,
    float availableCrossSize,
    FlexDirection direction,
    AlignItems alignItems)
  {
    bool isRow = LayoutHelpers.IsRow(direction);
    float maxCrossSize = 0;

    foreach (FlexNode child in line.Items)
    {
      float childCrossSize;

      FlexValue dimension = isRow ? child.Height : child.Width;
      childCrossSize = ValueResolver.ResolveValue(dimension, availableCrossSize);

      // Handle stretch alignment
      if (float.IsNaN(childCrossSize))
      {
        AlignSelf alignSelf = child.AlignSelf == AlignSelf.Auto
          ? (AlignSelf)(int)alignItems
          : child.AlignSelf;

        if (alignSelf == AlignSelf.Stretch && !float.IsNaN(availableCrossSize))
        {
          childCrossSize = availableCrossSize;
        }
        else
        {
          childCrossSize = 0;
        }
      }

      // Apply min/max constraints
      if (isRow)
      {
        childCrossSize = ConstrainSize(
          childCrossSize,
          child.MinHeight,
          child.MaxHeight,
          availableCrossSize);

        child.Layout.Height = childCrossSize;
      }
      else
      {
        childCrossSize = ConstrainSize(
          childCrossSize,
          child.MinWidth,
          child.MaxWidth,
          availableCrossSize);

        child.Layout.Width = childCrossSize;
      }

      maxCrossSize = Math.Max(maxCrossSize, childCrossSize);
    }

    return maxCrossSize;
  }

  /// <summary>
  /// Calculates the node's size based on its children.
  /// </summary>
  private static void CalculateNodeSizeFromChildren(
    FlexNode node,
    float availableWidth,
    MeasureMode widthMode,
    float availableHeight,
    MeasureMode heightMode,
    float paddingBorderWidth,
    float paddingBorderHeight)
  {
    // Resolve explicit dimensions
    float width = ValueResolver.ResolveWidth(node, availableWidth);
    float height = ValueResolver.ResolveHeight(node, availableHeight);

    if (float.IsNaN(width))
    {
      if (widthMode == MeasureMode.Exactly)
      {
        width = availableWidth;
      }
      else
      {
        // Calculate from children
        float contentWidth = 0;

        foreach (FlexNode child in node.Children)
        {
          if (child.Display == Display.None || child.PositionType == PositionType.Absolute)
            continue;

          contentWidth = Math.Max(contentWidth, child.Layout.Left + child.Layout.Width);
        }

        width = contentWidth + paddingBorderWidth;
      }
    }

    if (float.IsNaN(height))
    {
      if (heightMode == MeasureMode.Exactly)
      {
        height = availableHeight;
      }
      else
      {
        // Calculate from children
        float contentHeight = 0;

        foreach (FlexNode child in node.Children)
        {
          if (child.Display == Display.None || child.PositionType == PositionType.Absolute)
            continue;

          contentHeight = Math.Max(contentHeight, child.Layout.Top + child.Layout.Height);
        }

        height = contentHeight + paddingBorderHeight;
      }
    }

    node.Layout.Width = width;
    node.Layout.Height = height;
  }

  /// <summary>
  /// Positions children within the node based on alignment properties.
  /// </summary>
  private static void PositionChildren(
    FlexNode node,
    FlexLines flexLines,
    FlexDirection direction,
    float paddingBorderMainStart,
    float paddingBorderCrossStart)
  {
    bool isRow = LayoutHelpers.IsRow(direction);
    bool isReverse = LayoutHelpers.IsReverse(direction);

    float crossPosition = paddingBorderCrossStart;

    foreach (FlexLine line in flexLines.Lines)
    {
      // Calculate justify-content offset
      float mainPosition = CalculateJustifyContentOffset(
        node.JustifyContent,
        paddingBorderMainStart,
        line.RemainingFreeSpace,
        line.ItemCount);

      float spaceBetween = CalculateJustifyContentSpacing(
        node.JustifyContent,
        line.RemainingFreeSpace,
        line.ItemCount);

      // Position items in the line
      IEnumerable<FlexNode> items = isReverse ? line.Items.Reverse() : line.Items;

      foreach (FlexNode child in items)
      {
        if (child.PositionType == PositionType.Absolute)
          continue;

        // Set main axis position
        if (isRow)
        {
          child.Layout.Left = mainPosition;
        }
        else
        {
          child.Layout.Top = mainPosition;
        }

        // Calculate cross axis position based on align-items/align-self
        float childCrossPosition = CalculateAlignItemsOffset(
          node.AlignItems,
          child.AlignSelf,
          crossPosition,
          line.CrossSize,
          isRow ? child.Layout.Height : child.Layout.Width);

        if (isRow)
        {
          child.Layout.Top = childCrossPosition;
        }
        else
        {
          child.Layout.Left = childCrossPosition;
        }

        // Move to next position
        mainPosition += (isRow ? child.Layout.Width : child.Layout.Height) + spaceBetween;
      }

      crossPosition += line.CrossSize;
    }
  }

  /// <summary>
  /// Calculates the starting offset for justify-content.
  /// </summary>
  private static float CalculateJustifyContentOffset(
    JustifyContent justify,
    float paddingStart,
    float freeSpace,
    int itemCount)
  {
    if (freeSpace <= 0 || itemCount == 0)
      return paddingStart;

    return justify switch
    {
      JustifyContent.FlexStart => paddingStart,
      JustifyContent.FlexEnd => paddingStart + freeSpace,
      JustifyContent.Center => paddingStart + freeSpace / 2,
      JustifyContent.SpaceBetween => paddingStart,
      JustifyContent.SpaceAround => paddingStart + freeSpace / itemCount / 2,
      JustifyContent.SpaceEvenly => paddingStart + freeSpace / (itemCount + 1),
      _ => paddingStart
    };
  }

  /// <summary>
  /// Calculates the spacing between items for justify-content.
  /// </summary>
  private static float CalculateJustifyContentSpacing(
    JustifyContent justify,
    float freeSpace,
    int itemCount)
  {
    if (freeSpace <= 0 || itemCount <= 1)
      return 0;

    return justify switch
    {
      JustifyContent.SpaceBetween => freeSpace / (itemCount - 1),
      JustifyContent.SpaceAround => freeSpace / itemCount,
      JustifyContent.SpaceEvenly => freeSpace / (itemCount + 1),
      _ => 0
    };
  }

  /// <summary>
  /// Calculates the cross axis offset for align-items/align-self.
  /// </summary>
  private static float CalculateAlignItemsOffset(
    AlignItems alignItems,
    AlignSelf alignSelf,
    float lineStart,
    float lineSize,
    float itemSize)
  {
    AlignSelf effectiveAlign = alignSelf == AlignSelf.Auto
      ? (AlignSelf)(int)alignItems
      : alignSelf;

    return effectiveAlign switch
    {
      AlignSelf.FlexStart => lineStart,
      AlignSelf.FlexEnd => lineStart + lineSize - itemSize,
      AlignSelf.Center => lineStart + (lineSize - itemSize) / 2,
      AlignSelf.Stretch => lineStart,
      AlignSelf.Baseline => lineStart, // TODO: Implement baseline alignment
      _ => lineStart
    };
  }

  /// <summary>
  /// Layouts an absolutely positioned child.
  /// </summary>
  private static void LayoutAbsoluteChild(FlexNode parent, FlexNode child)
  {
    // Absolute children are positioned relative to their containing block
    float width = ValueResolver.ResolveWidth(child, parent.Layout.Width);
    float height = ValueResolver.ResolveHeight(child, parent.Layout.Height);

    if (float.IsNaN(width))
      width = 0;

    if (float.IsNaN(height))
      height = 0;

    child.Layout.Width = width;
    child.Layout.Height = height;

    // Position based on inset properties (left, top, right, bottom)
    // TODO: Implement full absolute positioning with insets
    child.Layout.Left = 0;
    child.Layout.Top = 0;
  }

  /// <summary>
  /// Gets the total padding and border for an edge.
  /// </summary>
  private static float GetPaddingAndBorder(FlexNode node, Edge edge, Direction direction)
  {
    bool isRtl = direction == Direction.Rtl;

    float padding = GetFlexValueEdge(node.Padding, edge, isRtl);
    float border = GetFloatEdge(node.Border, edge, isRtl);

    return padding + border;
  }

  /// <summary>
  /// Gets the computed value for an edge from FlexValue EdgeValues.
  /// </summary>
  private static float GetFlexValueEdge(EdgeValues<FlexValue> edges, Edge edge, bool isRtl)
  {
    FlexValue value = edge switch
    {
      Edge.Left => edges.ComputedLeft(FlexValue.Point(0), isRtl),
      Edge.Right => edges.ComputedRight(FlexValue.Point(0), isRtl),
      Edge.Top => edges.ComputedTop(FlexValue.Point(0)),
      Edge.Bottom => edges.ComputedBottom(FlexValue.Point(0)),
      _ => FlexValue.Point(0)
    };

    return ValueResolver.ResolveValueOrDefault(value, 0, 0);
  }

  /// <summary>
  /// Gets the computed value for an edge from float EdgeValues.
  /// </summary>
  private static float GetFloatEdge(EdgeValues<float> edges, Edge edge, bool isRtl)
  {
    return edge switch
    {
      Edge.Left => edges.ComputedLeft(0, isRtl),
      Edge.Right => edges.ComputedRight(0, isRtl),
      Edge.Top => edges.ComputedTop(0),
      Edge.Bottom => edges.ComputedBottom(0),
      _ => 0
    };
  }

  /// <summary>
  /// Constrains a size value within min/max bounds.
  /// </summary>
  private static float ConstrainSize(
    float size,
    FlexValue minSize,
    FlexValue maxSize,
    float containerSize)
  {
    float min = ValueResolver.ResolveValueOrDefault(minSize, containerSize, 0);
    float max = ValueResolver.ResolveValueOrDefault(maxSize, containerSize, float.PositiveInfinity);

    return Math.Clamp(size, min, max);
  }
}
