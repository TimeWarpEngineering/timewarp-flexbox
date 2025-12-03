namespace TimeWarp.Flexbox;

/// <summary>
/// Provides pixel grid rounding functionality for crisp rendering on displays.
/// Handles high DPI scaling and terminal cell alignment.
/// </summary>
public static class PixelGrid
{
  /// <summary>
  /// Rounds a value to the pixel grid based on the scale factor.
  /// </summary>
  /// <param name="value">The value to round.</param>
  /// <param name="pointScaleFactor">The point scale factor (e.g., 2.0 for 2x DPI).</param>
  /// <param name="forceCeil">Force rounding up.</param>
  /// <param name="forceFloor">Force rounding down.</param>
  /// <returns>The rounded value.</returns>
  public static float RoundToPixelGrid(
    float value,
    float pointScaleFactor,
    bool forceCeil = false,
    bool forceFloor = false)
  {
    if (pointScaleFactor <= 0)
      return value;

    float scaledValue = value * pointScaleFactor;
    float roundedValue;

    if (forceCeil)
    {
      roundedValue = MathF.Ceiling(scaledValue);
    }
    else if (forceFloor)
    {
      roundedValue = MathF.Floor(scaledValue);
    }
    else
    {
      roundedValue = MathF.Round(scaledValue);
    }

    return roundedValue / pointScaleFactor;
  }

  /// <summary>
  /// Rounds all layout values in a node tree to the pixel grid.
  /// Uses accumulated absolute positions to avoid cumulative rounding errors.
  /// </summary>
  /// <param name="node">The root node to round.</param>
  /// <param name="pointScaleFactor">The point scale factor.</param>
  /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
  public static void RoundLayoutToPixelGrid(FlexNode node, float pointScaleFactor)
  {
    ArgumentNullException.ThrowIfNull(node);

    if (pointScaleFactor <= 0 || pointScaleFactor == 1.0f)
      return;

    RoundNodeRecursive(node, pointScaleFactor, 0, 0);
  }

  /// <summary>
  /// Recursively rounds node layout values to the pixel grid.
  /// </summary>
  private static void RoundNodeRecursive(
    FlexNode node,
    float pointScaleFactor,
    float absoluteLeft,
    float absoluteTop)
  {
    // Calculate absolute position of this node
    float nodeAbsoluteLeft = absoluteLeft + node.Layout.Left;
    float nodeAbsoluteTop = absoluteTop + node.Layout.Top;

    // Round absolute position
    float roundedAbsoluteLeft = RoundToPixelGrid(nodeAbsoluteLeft, pointScaleFactor);
    float roundedAbsoluteTop = RoundToPixelGrid(nodeAbsoluteTop, pointScaleFactor);

    // Calculate rounded relative position
    float roundedLeft = roundedAbsoluteLeft - absoluteLeft;
    float roundedTop = roundedAbsoluteTop - absoluteTop;

    // Round the right and bottom edges based on absolute coordinates
    float absoluteRight = nodeAbsoluteLeft + node.Layout.Width;
    float absoluteBottom = nodeAbsoluteTop + node.Layout.Height;

    float roundedAbsoluteRight = RoundToPixelGrid(absoluteRight, pointScaleFactor);
    float roundedAbsoluteBottom = RoundToPixelGrid(absoluteBottom, pointScaleFactor);

    // Calculate rounded dimensions from rounded edges
    float roundedWidth = roundedAbsoluteRight - roundedAbsoluteLeft;
    float roundedHeight = roundedAbsoluteBottom - roundedAbsoluteTop;

    // Apply rounded values
    node.Layout.Left = roundedLeft;
    node.Layout.Top = roundedTop;
    node.Layout.Width = roundedWidth;
    node.Layout.Height = roundedHeight;

    // Recursively round children using the rounded absolute position
    foreach (FlexNode child in node.Children)
    {
      RoundNodeRecursive(child, pointScaleFactor, roundedAbsoluteLeft, roundedAbsoluteTop);
    }
  }
}
