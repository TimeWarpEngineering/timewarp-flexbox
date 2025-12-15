namespace TimeWarp.Flexbox;

/// <summary>
/// Core flexbox layout engine that calculates positions and sizes for all nodes.
/// Implements the CSS Flexbox layout algorithm.
/// </summary>
public sealed class FlexLayoutEngine
{
  private readonly FlexLines FlexLinesCache = new();

  /// <summary>
  /// Global generation counter used to track layout passes.
  /// Incremented each time CalculateLayout is called.
  /// This allows the cache to detect when a new layout pass has started.
  /// </summary>
  private static uint GlobalGenerationCount;

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
    CalculateLayout(root, availableWidth, availableHeight, direction, roundToPixelGrid: false);
  }

  /// <summary>
  /// Calculates the layout for a node tree with optional pixel grid rounding.
  /// </summary>
  /// <param name="root">The root node of the layout tree.</param>
  /// <param name="availableWidth">The available width for layout.</param>
  /// <param name="availableHeight">The available height for layout.</param>
  /// <param name="direction">The layout direction (LTR/RTL).</param>
  /// <param name="roundToPixelGrid">Whether to round layout values to the pixel grid.</param>
  /// <exception cref="ArgumentNullException">Thrown when root is null.</exception>
  public void CalculateLayout(
    FlexNode root,
    float availableWidth,
    float availableHeight,
    Direction direction,
    bool roundToPixelGrid)
  {
    ArgumentNullException.ThrowIfNull(root);

    // Increment the global generation count. This forces the recursive routine to
    // visit all dirty nodes at least once. Subsequent visits will use cached results
    // if the input parameters match and the node hasn't changed.
    uint currentGeneration = ++GlobalGenerationCount;

    // Resolve dimensions from the node if available dimensions are undefined
    // This matches Yoga's behavior in calculateLayout()
    float width = availableWidth;
    float height = availableHeight;
    MeasureMode widthMode = MeasureMode.Undefined;
    MeasureMode heightMode = MeasureMode.Undefined;

    // If available width is undefined but node has a definite width, use it
    if (float.IsNaN(availableWidth) && root.Width.Unit == Unit.Point)
    {
      width = root.Width.Value;
      widthMode = MeasureMode.Exactly;
    }
    else if (!float.IsNaN(availableWidth))
    {
      widthMode = MeasureMode.Exactly;
    }

    // If available height is undefined but node has a definite height, use it
    if (float.IsNaN(availableHeight) && root.Height.Unit == Unit.Point)
    {
      height = root.Height.Value;
      heightMode = MeasureMode.Exactly;
    }
    else if (!float.IsNaN(availableHeight))
    {
      heightMode = MeasureMode.Exactly;
    }

    // Calculate layout recursively starting from root
    LayoutNode(
      root,
      width,
      widthMode,
      height,
      heightMode,
      direction,
      currentGeneration);

    // Apply pixel grid rounding if requested
    if (roundToPixelGrid)
    {
      float scaleFactor = root.EffectiveConfig.PointScaleFactor;
      PixelGrid.RoundLayoutToPixelGrid(root, scaleFactor);
    }
  }

  /// <summary>
  /// Performs layout calculation for a single node.
  /// </summary>
  /// <param name="node">The node to layout.</param>
  /// <param name="availableWidth">The available width.</param>
  /// <param name="widthMode">The width measure mode.</param>
  /// <param name="availableHeight">The available height.</param>
  /// <param name="heightMode">The height measure mode.</param>
  /// <param name="direction">The layout direction.</param>
  /// <param name="generationCount">The current layout generation counter.</param>
  private void LayoutNode(
    FlexNode node,
    float availableWidth,
    MeasureMode widthMode,
    float availableHeight,
    MeasureMode heightMode,
    Direction direction,
    uint generationCount)
  {
    // Skip nodes with Display.None
    if (node.Display == Display.None)
      return;

    // Skip nodes with Display.Contents - they don't generate a box
    // Their children are handled by the parent via GetLayoutChildren()
    if (node.Display == Display.Contents)
      return;

    // Determine if we need to visit this node based on dirty flag and generation
    bool needToVisitNode = node.IsDirty || node.LastLayoutGeneration != generationCount;

    // Check cache for existing layout result (only if we don't need to visit)
    if (!needToVisitNode &&
        node.TryGetCachedLayout(availableWidth, availableHeight, widthMode, heightMode, out LayoutCacheEntry cached))
    {
      node.Layout.Width = cached.ComputedWidth;
      node.Layout.Height = cached.ComputedHeight;
      node.Layout.Left = cached.ComputedLeft;
      node.Layout.Top = cached.ComputedTop;
      return;
    }

    // Resolve the effective flex direction based on RTL
    FlexDirection resolvedDirection = LayoutHelpers.ResolveFlexDirection(node.FlexDirection, direction);
    bool isMainAxisRow = LayoutHelpers.IsRow(resolvedDirection);

    // Calculate available inner dimensions (excluding padding and border)
    float paddingBorderLeft = GetPaddingAndBorder(node, Edge.Left, direction);
    float paddingBorderRight = GetPaddingAndBorder(node, Edge.Right, direction);
    float paddingBorderTop = GetPaddingAndBorder(node, Edge.Top, direction);
    float paddingBorderBottom = GetPaddingAndBorder(node, Edge.Bottom, direction);

    // Get the leading/trailing edges based on the resolved flex direction
    Edge mainLeadingEdge = LayoutHelpers.GetLeadingEdge(resolvedDirection);
    Edge mainTrailingEdge = LayoutHelpers.GetTrailingEdge(resolvedDirection);

    // Map edges to padding values
    float paddingBorderMainStart = mainLeadingEdge switch
    {
      Edge.Left => paddingBorderLeft,
      Edge.Right => paddingBorderRight,
      Edge.Top => paddingBorderTop,
      Edge.Bottom => paddingBorderBottom,
      _ => 0
    };
    float paddingBorderMainEnd = mainTrailingEdge switch
    {
      Edge.Left => paddingBorderLeft,
      Edge.Right => paddingBorderRight,
      Edge.Top => paddingBorderTop,
      Edge.Bottom => paddingBorderBottom,
      _ => 0
    };
    float paddingBorderCrossStart = isMainAxisRow ? paddingBorderTop : paddingBorderLeft;
    float paddingBorderCrossEnd = isMainAxisRow ? paddingBorderBottom : paddingBorderRight;

    float availableInnerWidth = availableWidth - paddingBorderLeft - paddingBorderRight;
    float availableInnerHeight = availableHeight - paddingBorderTop - paddingBorderBottom;

    float availableInnerMainSize = isMainAxisRow ? availableInnerWidth : availableInnerHeight;
    float availableInnerCrossSize = isMainAxisRow ? availableInnerHeight : availableInnerWidth;

    // Calculate gap values based on flex direction
    // For row: column-gap is main axis gap, row-gap is cross axis gap
    // For column: row-gap is main axis gap, column-gap is cross axis gap
    float mainAxisGap = isMainAxisRow ? node.ColumnGap : node.RowGap;
    float crossAxisGap = isMainAxisRow ? node.RowGap : node.ColumnGap;

    // Handle leaf nodes with measure function
    if (node.IsLeaf && node.HasMeasureFunc)
    {
      MeasureLeafNode(node, availableWidth, widthMode, availableHeight, heightMode);
      FinalizeNodeLayout(node, availableWidth, availableHeight, widthMode, heightMode, generationCount);
      return;
    }

    // Handle leaf nodes without children
    if (node.ChildCount == 0)
    {
      CalculateNodeSize(node, availableWidth, widthMode, availableHeight, heightMode, direction);
      FinalizeNodeLayout(node, availableWidth, availableHeight, widthMode, heightMode, generationCount);
      return;
    }

    // Collect children into flex lines
    bool isRtl = direction == Direction.Rtl;
    FlexLinesCache.CollectLines(node, availableInnerMainSize, resolvedDirection, node.FlexWrap, availableWidth, isRtl);

    // Process each flex line
    float totalLineCrossSize = 0;

    foreach (FlexLine line in FlexLinesCache.Lines.ToList())
    {
      // Calculate main axis sizes for items in this line
      CalculateMainAxisSizes(line, availableInnerMainSize, resolvedDirection, direction, generationCount);

      // Resolve flexible lengths (grow/shrink)
      ResolveFlexibleLengths(line, availableInnerMainSize, resolvedDirection, mainAxisGap, availableWidth, isRtl);

      // Calculate cross axis sizes
      float lineCrossSize = CalculateCrossAxisSizes(
        line,
        availableInnerCrossSize,
        resolvedDirection,
        node.AlignItems,
        direction);

      line.CrossSize = lineCrossSize;
      totalLineCrossSize += lineCrossSize;
    }

    // Add cross axis gaps between lines
    int lineCount = FlexLinesCache.LineCount;

    if (lineCount > 1)
    {
      totalLineCrossSize += crossAxisGap * (lineCount - 1);
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
      paddingBorderCrossStart,
      availableInnerCrossSize,
      totalLineCrossSize,
      mainAxisGap,
      crossAxisGap,
      direction);

    // Detect overflow
    DetectOverflow(node, FlexLinesCache, availableInnerMainSize, availableInnerCrossSize, crossAxisGap);

    // Recursively layout children (using GetLayoutChildren to flatten Display.Contents nodes)
    foreach (FlexNode child in node.GetLayoutChildren())
    {
      if (child.PositionType == PositionType.Absolute)
      {
        LayoutAbsoluteChild(node, child, direction);
        continue;
      }

      // Layout child with its calculated size
      LayoutNode(
        child,
        child.Layout.Width,
        MeasureMode.Exactly,
        child.Layout.Height,
        MeasureMode.Exactly,
        direction,
        generationCount);
    }

    FinalizeNodeLayout(node, availableWidth, availableHeight, widthMode, heightMode, generationCount);
  }

  /// <summary>
  /// Finalizes layout for a node by storing results in cache and updating generation tracking.
  /// </summary>
  private static void FinalizeNodeLayout(
    FlexNode node,
    float availableWidth,
    float availableHeight,
    MeasureMode widthMode,
    MeasureMode heightMode,
    uint generationCount)
  {
    // Store layout result in cache
    node.SetCachedLayout(
      availableWidth,
      availableHeight,
      widthMode,
      heightMode,
      new LayoutCacheEntry
      {
        ComputedWidth = node.Layout.Width,
        ComputedHeight = node.Layout.Height,
        ComputedLeft = node.Layout.Left,
        ComputedTop = node.Layout.Top
      });

    // Mark node as having been laid out in this generation and clear dirty flag
    node.LastLayoutGeneration = generationCount;
    node.ClearDirty();
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
    MeasureMode heightMode,
    Direction direction)
  {
    bool isRtl = direction == Direction.Rtl;

    // Calculate padding and border for box sizing
    float paddingBorderLeft = GetPaddingAndBorder(node, Edge.Left, direction);
    float paddingBorderRight = GetPaddingAndBorder(node, Edge.Right, direction);
    float paddingBorderTop = GetPaddingAndBorder(node, Edge.Top, direction);
    float paddingBorderBottom = GetPaddingAndBorder(node, Edge.Bottom, direction);
    float paddingBorderWidth = paddingBorderLeft + paddingBorderRight;
    float paddingBorderHeight = paddingBorderTop + paddingBorderBottom;

    float width;
    float height;

    // When mode is Exactly, use the provided size (parent already calculated it)
    if (widthMode == MeasureMode.Exactly)
    {
      width = availableWidth;
    }
    else
    {
      width = ValueResolver.ResolveWidth(node, availableWidth, paddingBorderWidth);

      if (float.IsNaN(width))
        width = 0;
    }

    if (heightMode == MeasureMode.Exactly)
    {
      height = availableHeight;
    }
    else
    {
      height = ValueResolver.ResolveHeight(node, availableHeight, paddingBorderHeight);

      if (float.IsNaN(height))
        height = 0;
    }

    node.Layout.Width = width;
    node.Layout.Height = height;
  }

  /// <summary>
  /// Calculates main axis sizes for items in a flex line.
  /// </summary>
  private void CalculateMainAxisSizes(
    FlexLine line,
    float availableMainSize,
    FlexDirection direction,
    Direction layoutDirection,
    uint generationCount)
  {
    bool isRow = LayoutHelpers.IsRow(direction);

    foreach (FlexNode child in line.Items)
    {
      // Calculate child padding/border for box sizing
      float childPaddingBorderLeft = GetPaddingAndBorder(child, Edge.Left, layoutDirection);
      float childPaddingBorderRight = GetPaddingAndBorder(child, Edge.Right, layoutDirection);
      float childPaddingBorderTop = GetPaddingAndBorder(child, Edge.Top, layoutDirection);
      float childPaddingBorderBottom = GetPaddingAndBorder(child, Edge.Bottom, layoutDirection);
      float childPaddingBorderWidth = childPaddingBorderLeft + childPaddingBorderRight;
      float childPaddingBorderHeight = childPaddingBorderTop + childPaddingBorderBottom;
      float childPaddingBorderMain = isRow ? childPaddingBorderWidth : childPaddingBorderHeight;

      float childMainSize;

      // Resolve flex-basis or use explicit width/height
      if (child.FlexBasis.IsDefined)
      {
        childMainSize = ValueResolver.ResolveValue(child.FlexBasis, availableMainSize);
        // Apply ContentBox adjustment to flex-basis
        if (!float.IsNaN(childMainSize) && child.BoxSizing == BoxSizing.ContentBox)
        {
          childMainSize += childPaddingBorderMain;
        }
      }
      else
      {
        FlexValue dimension = isRow ? child.Width : child.Height;
        childMainSize = ValueResolver.ResolveValue(dimension, availableMainSize);
        // Apply ContentBox adjustment
        if (!float.IsNaN(childMainSize) && child.BoxSizing == BoxSizing.ContentBox)
        {
          childMainSize += childPaddingBorderMain;
        }
      }

      if (float.IsNaN(childMainSize))
      {
        // If child has a measure function, call it to get intrinsic size
        if (child.HasMeasureFunc && child.MeasureFunc is not null)
        {
          Size measuredSize = child.MeasureFunc(
            child,
            availableMainSize,
            float.IsNaN(availableMainSize) ? MeasureMode.Undefined : MeasureMode.AtMost,
            float.NaN,
            MeasureMode.Undefined);

          childMainSize = isRow ? measuredSize.Width : measuredSize.Height;

          // Also set cross size from measure
          float childCrossSize = isRow ? measuredSize.Height : measuredSize.Width;
          if (isRow)
          {
            child.Layout.Height = childCrossSize;
          }
          else
          {
            child.Layout.Width = childCrossSize;
          }
        }
        else if (child.ChildCount > 0)
        {
          // Child has children but no explicit size - recursively calculate intrinsic size
          LayoutNode(
            child,
            float.NaN,
            MeasureMode.Undefined,
            float.NaN,
            MeasureMode.Undefined,
            layoutDirection,
            generationCount);

          childMainSize = isRow ? child.Layout.Width : child.Layout.Height;
        }
        else
        {
          childMainSize = 0;
        }
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
  private static void ResolveFlexibleLengths(
    FlexLine line,
    float availableMainSize,
    FlexDirection direction,
    float mainAxisGap,
    float containerWidth,
    bool isRtl)
  {
    if (float.IsNaN(availableMainSize) || line.ItemCount == 0)
      return;

    bool isRow = LayoutHelpers.IsRow(direction);
    Edge mainLeadingEdge = LayoutHelpers.GetLeadingEdge(direction);
    Edge mainTrailingEdge = LayoutHelpers.GetTrailingEdge(direction);

    // Track frozen state for each item (items that hit min/max constraints)
    bool[] frozen = new bool[line.ItemCount];
    float[] targetMainSizes = new float[line.ItemCount];
    float[] flexBases = new float[line.ItemCount];
    float[] margins = new float[line.ItemCount];

    // Initialize target sizes, flex bases, and margins
    float totalMargins = 0;
    for (int i = 0; i < line.ItemCount; i++)
    {
      FlexNode child = line.Items[i];
      float currentSize = isRow ? child.Layout.Width : child.Layout.Height;
      targetMainSizes[i] = currentSize;
      flexBases[i] = currentSize;

      // Calculate margin for this item (non-auto margins only)
      float leadingMargin = GetMargin(child, mainLeadingEdge, containerWidth, isRtl);
      float trailingMargin = GetMargin(child, mainTrailingEdge, containerWidth, isRtl);
      margins[i] = leadingMargin + trailingMargin;
      totalMargins += margins[i];
    }

    // Calculate total gap space
    float totalGapSpace = line.ItemCount > 1 ? mainAxisGap * (line.ItemCount - 1) : 0;

    // Calculate initial free space (accounting for gaps AND margins)
    float usedMainSize = 0;

    for (int i = 0; i < line.ItemCount; i++)
    {
      usedMainSize += targetMainSizes[i];
    }

    float freeSpace = availableMainSize - usedMainSize - totalGapSpace - totalMargins;
    line.RemainingFreeSpace = freeSpace;
    line.InitialFreeSpace = freeSpace; // Store initial value for overflow detection

    // Determine if we're growing or shrinking
    bool isGrowing = freeSpace > 0;
    bool isShrinking = freeSpace < 0;

    // If no flex factors apply, we're done
    if (isGrowing && line.TotalFlexGrow <= 0)
      return;

    if (isShrinking && line.TotalFlexShrink <= 0)
      return;

    // Iterative resolution loop
    const int MaxIterations = 10;

    for (int iteration = 0; iteration < MaxIterations; iteration++)
    {
      // Calculate unfrozen flex factor totals
      float unfrozenFlexGrow = 0;
      float unfrozenFlexShrink = 0;
      float unfrozenWeightedShrink = 0;
      float unfrozenUsedSpace = 0;
      int unfrozenCount = 0;

      for (int i = 0; i < line.ItemCount; i++)
      {
        if (frozen[i])
          continue;

        FlexNode child = line.Items[i];
        unfrozenFlexGrow += child.FlexGrow;
        unfrozenFlexShrink += child.FlexShrink;
        unfrozenWeightedShrink += child.FlexShrink * flexBases[i];
        unfrozenUsedSpace += targetMainSizes[i];
        unfrozenCount++;
      }

      // If all items are frozen, we're done
      if (unfrozenCount == 0)
        break;

      // Calculate remaining free space for unfrozen items
      float frozenSpace = 0;

      for (int i = 0; i < line.ItemCount; i++)
      {
        if (frozen[i])
        {
          frozenSpace += targetMainSizes[i];
        }
      }

      float remainingSpace = availableMainSize - frozenSpace - unfrozenUsedSpace - totalGapSpace - totalMargins;

      // Distribute space
      bool anyViolation = false;

      for (int i = 0; i < line.ItemCount; i++)
      {
        if (frozen[i])
          continue;

        FlexNode child = line.Items[i];
        float adjustment = 0;

        if (isGrowing && unfrozenFlexGrow > 0 && child.FlexGrow > 0)
        {
          float ratio = child.FlexGrow / unfrozenFlexGrow;
          adjustment = remainingSpace * ratio;
        }
        else if (isShrinking && unfrozenWeightedShrink > 0 && child.FlexShrink > 0)
        {
          // Use scaled shrink factor (flex-shrink * flex-basis)
          float scaledShrink = child.FlexShrink * flexBases[i];
          float ratio = scaledShrink / unfrozenWeightedShrink;
          adjustment = remainingSpace * ratio; // remainingSpace is negative when shrinking
        }

        float newSize = targetMainSizes[i] + adjustment;

        // Get min/max constraints
        float minSize = isRow
          ? ValueResolver.ResolveValueOrDefault(child.MinWidth, availableMainSize, 0)
          : ValueResolver.ResolveValueOrDefault(child.MinHeight, availableMainSize, 0);

        float maxSize = isRow
          ? ValueResolver.ResolveValueOrDefault(child.MaxWidth, availableMainSize, float.PositiveInfinity)
          : ValueResolver.ResolveValueOrDefault(child.MaxHeight, availableMainSize, float.PositiveInfinity);

        // Check for constraint violations
        if (newSize < minSize)
        {
          newSize = minSize;
          frozen[i] = true;
          anyViolation = true;
        }
        else if (newSize > maxSize)
        {
          newSize = maxSize;
          frozen[i] = true;
          anyViolation = true;
        }

        targetMainSizes[i] = newSize;
      }

      // If no violations occurred, freeze all remaining items and exit
      if (!anyViolation)
      {
        for (int i = 0; i < line.ItemCount; i++)
        {
          frozen[i] = true;
        }

        break;
      }
    }

    // Apply final sizes
    for (int i = 0; i < line.ItemCount; i++)
    {
      FlexNode child = line.Items[i];

      if (isRow)
      {
        child.Layout.Width = targetMainSizes[i];
      }
      else
      {
        child.Layout.Height = targetMainSizes[i];
      }
    }

    // Update remaining free space
    float finalUsedSpace = 0;

    for (int i = 0; i < line.ItemCount; i++)
    {
      finalUsedSpace += targetMainSizes[i];
    }

    line.RemainingFreeSpace = availableMainSize - finalUsedSpace;
  }

  /// <summary>
  /// Calculates cross axis sizes for items in a flex line.
  /// </summary>
  private static float CalculateCrossAxisSizes(
    FlexLine line,
    float availableCrossSize,
    FlexDirection direction,
    AlignItems alignItems,
    Direction layoutDirection)
  {
    bool isRow = LayoutHelpers.IsRow(direction);
    float maxCrossSize = 0;

    foreach (FlexNode child in line.Items)
    {
      // Calculate child padding/border for box sizing
      float childPaddingBorderLeft = GetPaddingAndBorder(child, Edge.Left, layoutDirection);
      float childPaddingBorderRight = GetPaddingAndBorder(child, Edge.Right, layoutDirection);
      float childPaddingBorderTop = GetPaddingAndBorder(child, Edge.Top, layoutDirection);
      float childPaddingBorderBottom = GetPaddingAndBorder(child, Edge.Bottom, layoutDirection);
      float childPaddingBorderWidth = childPaddingBorderLeft + childPaddingBorderRight;
      float childPaddingBorderHeight = childPaddingBorderTop + childPaddingBorderBottom;
      float childPaddingBorderCross = isRow ? childPaddingBorderHeight : childPaddingBorderWidth;

      float childCrossSize;

      FlexValue dimension = isRow ? child.Height : child.Width;
      childCrossSize = ValueResolver.ResolveValue(dimension, availableCrossSize);

      // Apply ContentBox adjustment
      if (!float.IsNaN(childCrossSize) && child.BoxSizing == BoxSizing.ContentBox)
      {
        childCrossSize += childPaddingBorderCross;
      }

      // Handle stretch alignment
      if (float.IsNaN(childCrossSize))
      {
        AlignSelf alignSelf = child.AlignSelf == AlignSelf.Auto
          ? ConvertAlignItemsToAlignSelf(alignItems)
          : child.AlignSelf;

        if (alignSelf == AlignSelf.Stretch && !float.IsNaN(availableCrossSize))
        {
          childCrossSize = availableCrossSize;
        }
        else
        {
          // Check if cross size was already set (e.g., by MeasureFunc in CalculateMainAxisSizes)
          // Only use this as fallback when not stretching
          float existingCrossSize = isRow ? child.Layout.Height : child.Layout.Width;
          childCrossSize = existingCrossSize > 0 ? existingCrossSize : 0;
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
    // Resolve explicit dimensions (pass padding/border for ContentBox sizing)
    float width = ValueResolver.ResolveWidth(node, availableWidth, paddingBorderWidth);
    float height = ValueResolver.ResolveHeight(node, availableHeight, paddingBorderHeight);

    bool isRow = LayoutHelpers.IsRow(node.FlexDirection);

    if (float.IsNaN(width))
    {
      if (widthMode == MeasureMode.Exactly)
      {
        width = availableWidth;
      }
      else
      {
        // Calculate from children based on flex direction
        float contentWidth = 0;
        int childCount = 0;

        foreach (FlexNode child in node.GetLayoutChildren())
        {
          if (child.PositionType == PositionType.Absolute)
            continue;

          // Get child margins (margins use availableWidth for percentage resolution)
          FlexValue marginLeft = child.Margin.ComputedLeft(FlexValue.Undefined);
          FlexValue marginRight = child.Margin.ComputedRight(FlexValue.Undefined);
          float childMarginLeft = ValueResolver.ResolveValueOrDefault(marginLeft, availableWidth, 0);
          float childMarginRight = ValueResolver.ResolveValueOrDefault(marginRight, availableWidth, 0);
          float childTotalWidth = child.Layout.Width + childMarginLeft + childMarginRight;

          if (isRow)
          {
            // In row direction, sum widths along main axis
            contentWidth += childTotalWidth;
          }
          else
          {
            // In column direction, take max width (cross axis)
            contentWidth = Math.Max(contentWidth, childTotalWidth);
          }

          childCount++;
        }

        // Add gaps for row direction
        if (isRow && childCount > 1)
        {
          contentWidth += node.ColumnGap * (childCount - 1);
        }

        width = contentWidth + paddingBorderWidth;

        // Apply min/max constraints
        float minWidth = ValueResolver.ResolveValue(node.MinWidth, availableWidth);
        float maxWidth = ValueResolver.ResolveValue(node.MaxWidth, availableWidth);

        if (!float.IsNaN(minWidth))
          width = Math.Max(width, minWidth);
        if (!float.IsNaN(maxWidth))
          width = Math.Min(width, maxWidth);
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
        // Calculate from children based on flex direction
        float contentHeight = 0;
        int childCount = 0;

        foreach (FlexNode child in node.GetLayoutChildren())
        {
          if (child.PositionType == PositionType.Absolute)
            continue;

          // Get child margins (margins use availableWidth for percentage resolution per CSS spec)
          FlexValue marginTop = child.Margin.ComputedTop(FlexValue.Undefined);
          FlexValue marginBottom = child.Margin.ComputedBottom(FlexValue.Undefined);
          float childMarginTop = ValueResolver.ResolveValueOrDefault(marginTop, availableWidth, 0);
          float childMarginBottom = ValueResolver.ResolveValueOrDefault(marginBottom, availableWidth, 0);
          float childTotalHeight = child.Layout.Height + childMarginTop + childMarginBottom;

          if (isRow)
          {
            // In row direction, take max height (cross axis)
            contentHeight = Math.Max(contentHeight, childTotalHeight);
          }
          else
          {
            // In column direction, sum heights along main axis
            contentHeight += childTotalHeight;
          }

          childCount++;
        }

        // Add gaps for column direction
        if (!isRow && childCount > 1)
        {
          contentHeight += node.RowGap * (childCount - 1);
        }

        height = contentHeight + paddingBorderHeight;

        // Apply min/max constraints
        float minHeight = ValueResolver.ResolveValue(node.MinHeight, availableHeight);
        float maxHeight = ValueResolver.ResolveValue(node.MaxHeight, availableHeight);

        if (!float.IsNaN(minHeight))
          height = Math.Max(height, minHeight);
        if (!float.IsNaN(maxHeight))
          height = Math.Min(height, maxHeight);
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
    float paddingBorderCrossStart,
    float availableCrossSize,
    float totalLineCrossSize,
    float mainAxisGap,
    float crossAxisGap,
    Direction layoutDirection)
  {
    bool isRow = LayoutHelpers.IsRow(direction);
    bool isReverse = LayoutHelpers.IsReverse(direction);
    bool isRtl = layoutDirection == Direction.Rtl;
    int lineCount = flexLines.LineCount;
    float containerWidth = node.Layout.Width;

    // Get main axis edges
    Edge mainLeadingEdge = LayoutHelpers.GetLeadingEdge(direction);
    Edge mainTrailingEdge = LayoutHelpers.GetTrailingEdge(direction);
    Edge crossLeadingEdge = isRow ? Edge.Top : Edge.Left;
    Edge crossTrailingEdge = isRow ? Edge.Bottom : Edge.Right;

    // Calculate align-content distribution
    float crossFreeSpace = availableCrossSize - totalLineCrossSize;

    // Handle align-content: stretch by expanding line cross sizes
    if (node.AlignContent == AlignContent.Stretch && crossFreeSpace > 0 && lineCount > 0)
    {
      float extraPerLine = crossFreeSpace / lineCount;

      foreach (FlexLine line in flexLines.Lines)
      {
        line.CrossSize += extraPerLine;
      }

      // Recalculate total after stretch
      crossFreeSpace = 0;
    }

    float crossOffset = CalculateAlignContentOffset(
      node.AlignContent,
      paddingBorderCrossStart,
      crossFreeSpace,
      lineCount);

    float crossSpacing = CalculateAlignContentSpacing(
      node.AlignContent,
      crossFreeSpace,
      lineCount);

    float crossPosition = crossOffset;

    foreach (FlexLine line in flexLines.Lines)
    {
      // Calculate auto margin distribution for main axis
      float autoMarginFreeSpace = line.RemainingFreeSpace;
      int autoMarginCount = 0;

      foreach (FlexNode child in line.Items)
      {
        if (child.PositionType == PositionType.Absolute)
          continue;

        if (IsMarginAuto(child, mainLeadingEdge, isRtl))
          autoMarginCount++;

        if (IsMarginAuto(child, mainTrailingEdge, isRtl))
          autoMarginCount++;
      }

      float autoMarginSize = autoMarginCount > 0 && autoMarginFreeSpace > 0
        ? autoMarginFreeSpace / autoMarginCount
        : 0;

      // If there are auto margins, they consume free space instead of justify-content
      float effectiveFreeSpace = autoMarginCount > 0 ? 0 : line.RemainingFreeSpace;

      // Calculate justify-content offset
      float mainPosition = CalculateJustifyContentOffset(
        node.JustifyContent,
        paddingBorderMainStart,
        effectiveFreeSpace,
        line.ItemCount);

      float spaceBetween = CalculateJustifyContentSpacing(
        node.JustifyContent,
        effectiveFreeSpace,
        line.ItemCount);

      // Calculate line baseline for baseline alignment
      float lineBaseline = CalculateLineBaseline(line, node.AlignItems, isRow);

      // Position items in the line
      float containerMainSize = isRow ? node.Layout.Width : node.Layout.Height;
      bool isFirstItem = true;

      foreach (FlexNode child in line.Items)
      {
        if (child.PositionType == PositionType.Absolute)
          continue;

        // Add gap before item (except first item)
        if (!isFirstItem)
        {
          mainPosition += mainAxisGap;
        }

        isFirstItem = false;

        // Add leading margin (fixed or auto)
        float leadingMargin = IsMarginAuto(child, mainLeadingEdge, isRtl)
          ? autoMarginSize
          : GetMargin(child, mainLeadingEdge, containerWidth, isRtl);
        mainPosition += leadingMargin;

        float childMainSize = isRow ? child.Layout.Width : child.Layout.Height;

        // Set main axis position using the leading edge
        child.Layout.SetPosition(mainLeadingEdge, mainPosition, containerMainSize, childMainSize);

        // Calculate cross axis margins and auto-margin behavior
        float crossLeadingMargin = GetMargin(child, crossLeadingEdge, containerWidth, isRtl);
        float crossTrailingMargin = GetMargin(child, crossTrailingEdge, containerWidth, isRtl);
        bool hasAutoLeadingCross = IsMarginAuto(child, crossLeadingEdge, isRtl);
        bool hasAutoTrailingCross = IsMarginAuto(child, crossTrailingEdge, isRtl);

        float childCrossSize = isRow ? child.Layout.Height : child.Layout.Width;
        // Use container cross size for align-items (like Yoga's containerCrossAxis)
        float containerCrossAxis = lineCount == 1 ? availableCrossSize : line.CrossSize;
        float crossFreeSpaceForChild = containerCrossAxis - childCrossSize - crossLeadingMargin - crossTrailingMargin;

        float childCrossPosition;

        if (hasAutoLeadingCross && hasAutoTrailingCross)
        {
          // Both auto: center the item
          childCrossPosition = crossPosition + crossFreeSpaceForChild / 2 + crossLeadingMargin;
        }
        else if (hasAutoLeadingCross)
        {
          // Only leading auto: push to trailing edge
          childCrossPosition = crossPosition + crossFreeSpaceForChild + crossLeadingMargin;
        }
        else if (hasAutoTrailingCross)
        {
          // Only trailing auto: stay at leading edge with margin
          childCrossPosition = crossPosition + crossLeadingMargin;
        }
        else
        {
          // No auto margins: use align-items/align-self
          float itemBaseline = GetItemBaseline(child, isRow);
          childCrossPosition = CalculateAlignItemsOffset(
            node.AlignItems,
            child.AlignSelf,
            crossPosition + crossLeadingMargin,
            containerCrossAxis - crossLeadingMargin - crossTrailingMargin,
            childCrossSize,
            lineBaseline,
            itemBaseline);
        }

        if (isRow)
        {
          child.Layout.Top = childCrossPosition;
        }
        else
        {
          child.Layout.Left = childCrossPosition;
        }

        // Add trailing margin
        float trailingMargin = IsMarginAuto(child, mainTrailingEdge, isRtl)
          ? autoMarginSize
          : GetMargin(child, mainTrailingEdge, containerWidth, isRtl);

        // Move to next position (spacing from justify-content)
        mainPosition += childMainSize + trailingMargin + spaceBetween;
      }

      crossPosition += line.CrossSize + crossSpacing + crossAxisGap;
    }
  }

  /// <summary>
  /// Calculates the starting offset for align-content.
  /// </summary>
  private static float CalculateAlignContentOffset(
    AlignContent alignContent,
    float paddingStart,
    float freeSpace,
    int lineCount)
  {
    if (freeSpace <= 0 || lineCount == 0)
      return paddingStart;

    return alignContent switch
    {
      AlignContent.FlexStart => paddingStart,
      AlignContent.FlexEnd => paddingStart + freeSpace,
      AlignContent.Center => paddingStart + freeSpace / 2,
      AlignContent.SpaceBetween => paddingStart,
      AlignContent.SpaceAround => paddingStart + freeSpace / lineCount / 2,
      AlignContent.SpaceEvenly => paddingStart + freeSpace / (lineCount + 1),
      AlignContent.Stretch => paddingStart, // Stretch is handled before this
      _ => paddingStart
    };
  }

  /// <summary>
  /// Calculates the spacing between lines for align-content.
  /// </summary>
  private static float CalculateAlignContentSpacing(
    AlignContent alignContent,
    float freeSpace,
    int lineCount)
  {
    if (freeSpace <= 0 || lineCount <= 1)
      return 0;

    return alignContent switch
    {
      AlignContent.SpaceBetween => freeSpace / (lineCount - 1),
      AlignContent.SpaceAround => freeSpace / lineCount,
      AlignContent.SpaceEvenly => freeSpace / (lineCount + 1),
      _ => 0
    };
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
    float itemSize,
    float lineBaseline = 0,
    float itemBaseline = 0)
  {
    AlignSelf effectiveAlign = alignSelf == AlignSelf.Auto
      ? ConvertAlignItemsToAlignSelf(alignItems)
      : alignSelf;

    return effectiveAlign switch
    {
      AlignSelf.FlexStart => lineStart,
      AlignSelf.FlexEnd => lineStart + lineSize - itemSize,
      AlignSelf.Center => lineStart + (lineSize - itemSize) / 2,
      AlignSelf.Stretch => lineStart,
      AlignSelf.Baseline => lineStart + lineBaseline - itemBaseline,
      _ => lineStart
    };
  }

  /// <summary>
  /// Gets the baseline for a flex item.
  /// If the item has a BaselineFunc, it is called with the item's dimensions.
  /// Otherwise, the baseline defaults to the bottom of the item (its height).
  /// </summary>
  private static float GetItemBaseline(FlexNode item, bool isRow)
  {
    float width = item.Layout.Width;
    float height = item.Layout.Height;

    if (item.BaselineFunc is not null)
    {
      return item.BaselineFunc(item, width, height);
    }

    // Default baseline is at the bottom of the item
    return isRow ? height : width;
  }

  /// <summary>
  /// Calculates the maximum baseline for items in a line that use baseline alignment.
  /// </summary>
  private static float CalculateLineBaseline(FlexLine line, AlignItems alignItems, bool isRow)
  {
    float maxBaseline = 0;

    foreach (FlexNode child in line.Items)
    {
      if (child.PositionType == PositionType.Absolute)
        continue;

      AlignSelf effectiveAlign = child.AlignSelf == AlignSelf.Auto
        ? ConvertAlignItemsToAlignSelf(alignItems)
        : child.AlignSelf;

      if (effectiveAlign == AlignSelf.Baseline)
      {
        float itemBaseline = GetItemBaseline(child, isRow);
        maxBaseline = Math.Max(maxBaseline, itemBaseline);
      }
    }

    return maxBaseline;
  }

  /// <summary>
  /// Layouts an absolutely positioned child.
  /// </summary>
  private static void LayoutAbsoluteChild(FlexNode parent, FlexNode child, Direction direction)
  {
    bool isRtl = direction == Direction.Rtl;
    float containerWidth = parent.Layout.Width;
    float containerHeight = parent.Layout.Height;

    // Get parent border values - absolute children are positioned relative to padding box
    float borderLeft = GetFloatEdge(parent.Border, Edge.Left, isRtl);
    float borderTop = GetFloatEdge(parent.Border, Edge.Top, isRtl);
    float borderRight = GetFloatEdge(parent.Border, Edge.Right, isRtl);
    float borderBottom = GetFloatEdge(parent.Border, Edge.Bottom, isRtl);

    // Calculate container dimensions inside the border (padding box)
    float paddingBoxWidth = containerWidth - borderLeft - borderRight;
    float paddingBoxHeight = containerHeight - borderTop - borderBottom;

    // Get position insets
    FlexValue leftInset = child.Position.ComputedLeft(FlexValue.Undefined, isRtl);
    FlexValue topInset = child.Position.ComputedTop(FlexValue.Undefined);
    FlexValue rightInset = child.Position.ComputedRight(FlexValue.Undefined, isRtl);
    FlexValue bottomInset = child.Position.ComputedBottom(FlexValue.Undefined);

    float left = ValueResolver.ResolveValue(leftInset, paddingBoxWidth);
    float top = ValueResolver.ResolveValue(topInset, paddingBoxHeight);
    float right = ValueResolver.ResolveValue(rightInset, paddingBoxWidth);
    float bottom = ValueResolver.ResolveValue(bottomInset, paddingBoxHeight);

    bool hasLeft = ValueResolver.IsDefined(left);
    bool hasTop = ValueResolver.IsDefined(top);
    bool hasRight = ValueResolver.IsDefined(right);
    bool hasBottom = ValueResolver.IsDefined(bottom);

    // Calculate child's padding and border for box sizing
    float childPaddingBorderLeft = GetPaddingAndBorder(child, Edge.Left, direction);
    float childPaddingBorderRight = GetPaddingAndBorder(child, Edge.Right, direction);
    float childPaddingBorderTop = GetPaddingAndBorder(child, Edge.Top, direction);
    float childPaddingBorderBottom = GetPaddingAndBorder(child, Edge.Bottom, direction);
    float childPaddingBorderWidth = childPaddingBorderLeft + childPaddingBorderRight;
    float childPaddingBorderHeight = childPaddingBorderTop + childPaddingBorderBottom;

    // Calculate width
    float width = ValueResolver.ResolveWidth(child, paddingBoxWidth, childPaddingBorderWidth);

    if (float.IsNaN(width))
    {
      // If both left and right are set, stretch to fill
      if (hasLeft && hasRight)
      {
        width = paddingBoxWidth - left - right;
        width = Math.Max(0, width);

        // Apply min/max constraints
        float minWidth = ValueResolver.ResolveValueOrDefault(child.MinWidth, paddingBoxWidth, 0);
        float maxWidth = ValueResolver.ResolveValueOrDefault(child.MaxWidth, paddingBoxWidth, float.PositiveInfinity);
        width = Math.Clamp(width, minWidth, maxWidth);
      }
      else
      {
        // Default to 0 (or could measure if has measure function)
        width = 0;
      }
    }

    // Calculate height
    float height = ValueResolver.ResolveHeight(child, paddingBoxHeight, childPaddingBorderHeight);

    if (float.IsNaN(height))
    {
      // If both top and bottom are set, stretch to fill
      if (hasTop && hasBottom)
      {
        height = paddingBoxHeight - top - bottom;
        height = Math.Max(0, height);

        // Apply min/max constraints
        float minHeight = ValueResolver.ResolveValueOrDefault(child.MinHeight, paddingBoxHeight, 0);
        float maxHeight = ValueResolver.ResolveValueOrDefault(child.MaxHeight, paddingBoxHeight, float.PositiveInfinity);
        height = Math.Clamp(height, minHeight, maxHeight);
      }
      else
      {
        // Default to 0 (or could measure if has measure function)
        height = 0;
      }
    }

    child.Layout.Width = width;
    child.Layout.Height = height;

    // Calculate left position (offset by border since absolute is relative to padding box)
    if (hasLeft)
    {
      child.Layout.Left = borderLeft + left;
    }
    else if (hasRight)
    {
      child.Layout.Left = borderLeft + paddingBoxWidth - right - width;
    }
    else
    {
      // Default to border offset if neither left nor right is set
      child.Layout.Left = borderLeft;
    }

    // Calculate top position (offset by border since absolute is relative to padding box)
    if (hasTop)
    {
      child.Layout.Top = borderTop + top;
    }
    else if (hasBottom)
    {
      child.Layout.Top = borderTop + paddingBoxHeight - bottom - height;
    }
    else
    {
      // Default to border offset if neither top nor bottom is set
      child.Layout.Top = borderTop;
    }
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
  /// Gets the resolved margin value for an edge, resolving percentages against container width.
  /// </summary>
  private static float GetMargin(FlexNode node, Edge edge, float containerWidth, bool isRtl)
  {
    FlexValue value = edge switch
    {
      Edge.Left => node.Margin.ComputedLeft(FlexValue.Point(0), isRtl),
      Edge.Right => node.Margin.ComputedRight(FlexValue.Point(0), isRtl),
      Edge.Top => node.Margin.ComputedTop(FlexValue.Point(0)),
      Edge.Bottom => node.Margin.ComputedBottom(FlexValue.Point(0)),
      _ => FlexValue.Point(0)
    };

    // Auto margins return 0 here; they are handled separately
    if (value.Unit == Unit.Auto)
      return 0;

    // Percentages are always resolved against container width (per CSS spec)
    return ValueResolver.ResolveValueOrDefault(value, containerWidth, 0);
  }

  /// <summary>
  /// Checks if a margin edge is set to auto.
  /// </summary>
  private static bool IsMarginAuto(FlexNode node, Edge edge, bool isRtl)
  {
    FlexValue value = edge switch
    {
      Edge.Left => node.Margin.ComputedLeft(FlexValue.Undefined, isRtl),
      Edge.Right => node.Margin.ComputedRight(FlexValue.Undefined, isRtl),
      Edge.Top => node.Margin.ComputedTop(FlexValue.Undefined),
      Edge.Bottom => node.Margin.ComputedBottom(FlexValue.Undefined),
      _ => FlexValue.Undefined
    };

    return value.Unit == Unit.Auto;
  }

  /// <summary>
  /// Gets the total margin for both edges on an axis.
  /// </summary>
  private static float GetMarginForAxis(FlexNode node, bool isMainAxisRow, bool isMainAxis, float containerWidth, bool isRtl)
  {
    if (isMainAxis)
    {
      if (isMainAxisRow)
      {
        return GetMargin(node, Edge.Left, containerWidth, isRtl) +
               GetMargin(node, Edge.Right, containerWidth, isRtl);
      }
      else
      {
        return GetMargin(node, Edge.Top, containerWidth, isRtl) +
               GetMargin(node, Edge.Bottom, containerWidth, isRtl);
      }
    }
    else
    {
      if (isMainAxisRow)
      {
        return GetMargin(node, Edge.Top, containerWidth, isRtl) +
               GetMargin(node, Edge.Bottom, containerWidth, isRtl);
      }
      else
      {
        return GetMargin(node, Edge.Left, containerWidth, isRtl) +
               GetMargin(node, Edge.Right, containerWidth, isRtl);
      }
    }
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

  /// <summary>
  /// Converts AlignItems to the equivalent AlignSelf value.
  /// AlignSelf has an extra Auto value at position 0, so values are offset by 1.
  /// </summary>
  private static AlignSelf ConvertAlignItemsToAlignSelf(AlignItems alignItems) => alignItems switch
  {
    AlignItems.FlexStart => AlignSelf.FlexStart,
    AlignItems.FlexEnd => AlignSelf.FlexEnd,
    AlignItems.Center => AlignSelf.Center,
    AlignItems.Baseline => AlignSelf.Baseline,
    AlignItems.Stretch => AlignSelf.Stretch,
    _ => AlignSelf.Stretch
  };

  /// <summary>
  /// Detects if children overflow the container bounds.
  /// Sets HadOverflow on the node's layout result.
  /// Overflow is detected based on whether children's base sizes exceed available space,
  /// before flex shrink is applied.
  /// </summary>
  private static void DetectOverflow(
    FlexNode node,
    FlexLines flexLines,
    float availableMainSize,
    float availableCrossSize,
    float crossAxisGap)
  {
    // Reset overflow flag - it should be false by default
    node.Layout.HadOverflow = false;

    // If sizes are undefined (NaN), we can't detect overflow
    if (float.IsNaN(availableMainSize) && float.IsNaN(availableCrossSize))
      return;

    // Check each line for overflow based on RemainingFreeSpace (after flex adjustments)
    // If RemainingFreeSpace is negative after shrinking, we have actual overflow
    float totalCrossSize = 0;
    int lineCount = flexLines.LineCount;

    foreach (FlexLine line in flexLines.Lines)
    {
      // Check main axis overflow using final remaining free space (after shrink)
      // Negative remaining space means children actually exceed available space
      if (!float.IsNaN(availableMainSize) && line.RemainingFreeSpace < -0.0001f)
      {
        node.Layout.HadOverflow = true;
        return;
      }

      // Accumulate cross size
      totalCrossSize += line.CrossSize;
    }

    // Add cross axis gaps between lines
    if (lineCount > 1)
    {
      totalCrossSize += crossAxisGap * (lineCount - 1);
    }

    // Check for cross axis overflow
    if (!float.IsNaN(availableCrossSize) && totalCrossSize > availableCrossSize + 0.0001f)
    {
      node.Layout.HadOverflow = true;
    }
  }
}
