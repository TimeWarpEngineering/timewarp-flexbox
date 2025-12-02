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
  /// <exception cref="ArgumentNullException">Thrown when container is null.</exception>
  public void CollectLines(
    FlexNode container,
    float availableMainSize,
    FlexDirection direction,
    FlexWrap wrap)
  {
    ArgumentNullException.ThrowIfNull(container);

    LinesInternal.Clear();

    if (container.ChildCount == 0)
      return;

    bool isWrapping = wrap != FlexWrap.NoWrap;
    bool isMainAxisRow = LayoutHelpers.IsRow(direction);

    FlexLine currentLine = new();
    LinesInternal.Add(currentLine);

    float lineMainSize = 0;

    foreach (FlexNode child in container.Children)
    {
      // Skip nodes with Display.None
      if (child.Display == Display.None)
        continue;

      // Calculate the hypothetical main size of the child
      float childMainSize = CalculateHypotheticalMainSize(child, availableMainSize, isMainAxisRow);

      // Check if we need to wrap to a new line
      if (isWrapping &&
          currentLine.ItemCount > 0 &&
          !float.IsNaN(availableMainSize) &&
          lineMainSize + childMainSize > availableMainSize)
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
      lineMainSize += childMainSize;

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
