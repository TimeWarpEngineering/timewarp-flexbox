namespace TimeWarp.Flexbox;

/// <summary>
/// Manages flex lines during layout calculation.
/// Handles the wrapping of flex items into multiple lines.
/// </summary>
public sealed class FlexLines
{
  private readonly List<FlexLine> LinesInternal = [];

  /// <summary>
  /// Gets the read-only list of flex lines.
  /// </summary>
  public IReadOnlyList<FlexLine> Lines => LinesInternal;

  /// <summary>
  /// Gets the number of lines.
  /// </summary>
  public int LineCount => LinesInternal.Count;

  /// <summary>
  /// Gets the total cross size of all lines.
  /// </summary>
  public float TotalCrossSize
  {
    get
    {
      float total = 0;

      foreach (FlexLine line in LinesInternal)
      {
        total += line.CrossSize;
      }

      return total;
    }
  }

  /// <summary>
  /// Collects child nodes into flex lines based on container size and wrap mode.
  /// </summary>
  /// <param name="container">The container node.</param>
  /// <param name="availableMainSize">The available size on the main axis.</param>
  /// <param name="direction">The flex direction.</param>
  /// <param name="wrap">The flex wrap mode.</param>
  /// <param name="containerWidth">The container width for resolving percentage margins.</param>
  /// <param name="isRtl">Whether the layout direction is right-to-left.</param>
  /// <exception cref="ArgumentNullException">Thrown when container is null.</exception>
  public void CollectLines(
    FlexNode container,
    float availableMainSize,
    FlexDirection direction,
    FlexWrap wrap,
    float containerWidth = float.NaN,
    bool isRtl = false)
  {
    ArgumentNullException.ThrowIfNull(container);

    LinesInternal.Clear();

    if (container.ChildCount == 0)
      return;

    bool isWrapping = wrap != FlexWrap.NoWrap;
    bool isMainAxisRow = LayoutHelpers.IsRow(direction);

    // Determine main axis edges for margin calculation
    Edge mainLeadingEdge = LayoutHelpers.GetLeadingEdge(direction);
    Edge mainTrailingEdge = LayoutHelpers.GetTrailingEdge(direction);

    FlexLine currentLine = new();
    LinesInternal.Add(currentLine);

    float lineMainSize = 0;

    foreach (FlexNode child in container.Children)
    {
      // Skip nodes with Display.None or absolute positioning
      if (child.Display == Display.None)
        continue;

      if (child.PositionType == PositionType.Absolute)
        continue;

      // Calculate the hypothetical main size of the child including margins
      float childMainSize = CalculateHypotheticalMainSize(child, availableMainSize, isMainAxisRow);
      float marginMain = GetMarginForAxis(child, mainLeadingEdge, mainTrailingEdge, containerWidth, isRtl);
      float childOuterSize = childMainSize + marginMain;

      // Check if we need to wrap to a new line
      if (isWrapping &&
          currentLine.ItemCount > 0 &&
          !float.IsNaN(availableMainSize) &&
          lineMainSize + childOuterSize > availableMainSize)
      {
        // Finalize current line
        currentLine.MainSize = lineMainSize;

        // Start a new line
        currentLine = new FlexLine();
        LinesInternal.Add(currentLine);
        lineMainSize = 0;
      }

      // Add child to current line
      currentLine.AddItem(child);
      lineMainSize += childOuterSize;

      // Accumulate flex factors
      currentLine.TotalFlexGrow += child.FlexGrow;
      currentLine.TotalFlexShrink += child.FlexShrink;

      // Calculate weighted flex shrink (flex-shrink * flex-basis)
      float flexBasis = GetFlexBasis(child, availableMainSize, isMainAxisRow);
      currentLine.TotalWeightedFlexShrink += child.FlexShrink * flexBasis;
    }

    // Finalize the last line
    currentLine.MainSize = lineMainSize;

    // Calculate remaining free space for each line
    foreach (FlexLine line in LinesInternal)
    {
      if (!float.IsNaN(availableMainSize))
      {
        line.RemainingFreeSpace = availableMainSize - line.MainSize;
      }
    }

    // If wrap-reverse, reverse the order of lines
    if (wrap == FlexWrap.WrapReverse)
    {
      LinesInternal.Reverse();
    }
  }

  /// <summary>
  /// Gets the total margin for the main axis edges.
  /// </summary>
  private static float GetMarginForAxis(FlexNode node, Edge leadingEdge, Edge trailingEdge, float containerWidth, bool isRtl)
  {
    float leading = GetMargin(node, leadingEdge, containerWidth, isRtl);
    float trailing = GetMargin(node, trailingEdge, containerWidth, isRtl);
    return leading + trailing;
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

    // Auto margins are treated as 0 during line collection
    if (value.Unit == Unit.Auto)
      return 0;

    // Percentages are always resolved against container width (per CSS spec)
    return ValueResolver.ResolveValueOrDefault(value, containerWidth, 0);
  }

  /// <summary>
  /// Calculates the hypothetical main size of a child node.
  /// </summary>
  private static float CalculateHypotheticalMainSize(FlexNode child, float containerMainSize, bool isMainAxisRow)
  {
    // Get the flex basis or explicit size
    float flexBasis = GetFlexBasis(child, containerMainSize, isMainAxisRow);

    if (!float.IsNaN(flexBasis))
      return flexBasis;

    // If no explicit size, use a default
    return 0;
  }

  /// <summary>
  /// Gets the flex basis for a child, resolving auto to width/height.
  /// </summary>
  private static float GetFlexBasis(FlexNode child, float containerMainSize, bool isMainAxisRow)
  {
    FlexValue flexBasis = child.FlexBasis;

    // If flex-basis is auto, use width/height
    if (flexBasis.IsAuto)
    {
      FlexValue dimension = isMainAxisRow ? child.Width : child.Height;
      return ValueResolver.ResolveValue(dimension, containerMainSize);
    }

    return ValueResolver.ResolveValue(flexBasis, containerMainSize);
  }

  /// <summary>
  /// Resets the collection, clearing all lines.
  /// </summary>
  public void Reset()
  {
    LinesInternal.Clear();
  }
}
