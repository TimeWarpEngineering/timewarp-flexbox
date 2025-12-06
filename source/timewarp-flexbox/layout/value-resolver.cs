namespace TimeWarp.Flexbox;

/// <summary>
/// Static helper methods for resolving FlexValue to concrete float values.
/// </summary>
public static class ValueResolver
{
  /// <summary>
  /// Resolves a FlexValue to a concrete float value.
  /// </summary>
  /// <param name="value">The flex value to resolve.</param>
  /// <param name="containerSize">The container size for percentage calculations.</param>
  /// <returns>
  /// The resolved value in points, or float.NaN for Auto/Undefined/intrinsic values.
  /// </returns>
  /// <remarks>
  /// Intrinsic sizing units (MaxContent, FitContent, Stretch) return float.NaN from this method
  /// because they require content measurement and cannot be resolved from container size alone.
  /// The layout algorithm handles these units specially during layout calculation.
  /// </remarks>
  public static float ResolveValue(FlexValue value, float containerSize) => value.Unit switch
  {
    Unit.Point => value.Value,
    Unit.Percent => containerSize * value.Value / 100f,
    Unit.Auto => float.NaN,
    Unit.Undefined => float.NaN,
    // Intrinsic sizing units require content measurement and are handled by the layout algorithm
    Unit.MaxContent => float.NaN,
    Unit.FitContent => float.NaN,
    Unit.Stretch => float.NaN,
    _ => float.NaN
  };

  /// <summary>
  /// Resolves a FlexValue to a concrete float value, with a default for Auto/Undefined/intrinsic values.
  /// </summary>
  /// <param name="value">The flex value to resolve.</param>
  /// <param name="containerSize">The container size for percentage calculations.</param>
  /// <param name="defaultValue">The default value to use for Auto/Undefined/intrinsic values.</param>
  /// <returns>The resolved value in points.</returns>
  /// <remarks>
  /// Intrinsic sizing units (MaxContent, FitContent, Stretch) return the default value from this method
  /// because they require content measurement and cannot be resolved from container size alone.
  /// </remarks>
  public static float ResolveValueOrDefault(FlexValue value, float containerSize, float defaultValue) => value.Unit switch
  {
    Unit.Point => value.Value,
    Unit.Percent => containerSize * value.Value / 100f,
    Unit.Auto => defaultValue,
    Unit.Undefined => defaultValue,
    // Intrinsic sizing units require content measurement and are handled by the layout algorithm
    Unit.MaxContent => defaultValue,
    Unit.FitContent => defaultValue,
    Unit.Stretch => defaultValue,
    _ => defaultValue
  };

  /// <summary>
  /// Determines if a FlexValue is defined (Point, Percent, or FitContent).
  /// </summary>
  /// <param name="value">The flex value to check.</param>
  /// <returns>True if the value is defined.</returns>
  /// <remarks>
  /// FitContent is considered defined because it has a numeric value component
  /// representing the maximum size to clamp to.
  /// </remarks>
  public static bool IsDefined(FlexValue value) =>
    value.Unit is Unit.Point or Unit.Percent or Unit.FitContent;

  /// <summary>
  /// Determines if a float value is defined (not NaN).
  /// </summary>
  /// <param name="value">The float value to check.</param>
  /// <returns>True if the value is not NaN.</returns>
  public static bool IsDefined(float value) => !float.IsNaN(value);

  /// <summary>
  /// Resolves the width of a node, considering min/max constraints and box sizing.
  /// </summary>
  /// <param name="node">The flex node.</param>
  /// <param name="containerWidth">The container width for percentage calculations.</param>
  /// <param name="paddingBorderWidth">Optional padding and border width (for ContentBox calculations).</param>
  /// <returns>The resolved width, or float.NaN if undefined.</returns>
  /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
  public static float ResolveWidth(FlexNode node, float containerWidth, float paddingBorderWidth = 0)
  {
    ArgumentNullException.ThrowIfNull(node);

    float width = ResolveValue(node.Width, containerWidth);

    if (!IsDefined(width))
      return float.NaN;

    // With ContentBox, add padding and border to the specified content width
    if (node.BoxSizing == BoxSizing.ContentBox)
    {
      width += paddingBorderWidth;
    }

    float minWidth = ResolveValueOrDefault(node.MinWidth, containerWidth, 0);
    float maxWidth = ResolveValueOrDefault(node.MaxWidth, containerWidth, float.PositiveInfinity);

    // Min/max constraints also need box sizing adjustment for ContentBox
    if (node.BoxSizing == BoxSizing.ContentBox)
    {
      if (IsDefined(node.MinWidth))
        minWidth += paddingBorderWidth;
      if (IsDefined(node.MaxWidth) && !float.IsPositiveInfinity(maxWidth))
        maxWidth += paddingBorderWidth;
    }

    return Math.Clamp(width, minWidth, maxWidth);
  }

  /// <summary>
  /// Resolves the height of a node, considering min/max constraints and box sizing.
  /// </summary>
  /// <param name="node">The flex node.</param>
  /// <param name="containerHeight">The container height for percentage calculations.</param>
  /// <param name="paddingBorderHeight">Optional padding and border height (for ContentBox calculations).</param>
  /// <returns>The resolved height, or float.NaN if undefined.</returns>
  /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
  public static float ResolveHeight(FlexNode node, float containerHeight, float paddingBorderHeight = 0)
  {
    ArgumentNullException.ThrowIfNull(node);

    float height = ResolveValue(node.Height, containerHeight);

    if (!IsDefined(height))
      return float.NaN;

    // With ContentBox, add padding and border to the specified content height
    if (node.BoxSizing == BoxSizing.ContentBox)
    {
      height += paddingBorderHeight;
    }

    float minHeight = ResolveValueOrDefault(node.MinHeight, containerHeight, 0);
    float maxHeight = ResolveValueOrDefault(node.MaxHeight, containerHeight, float.PositiveInfinity);

    // Min/max constraints also need box sizing adjustment for ContentBox
    if (node.BoxSizing == BoxSizing.ContentBox)
    {
      if (IsDefined(node.MinHeight))
        minHeight += paddingBorderHeight;
      if (IsDefined(node.MaxHeight) && !float.IsPositiveInfinity(maxHeight))
        maxHeight += paddingBorderHeight;
    }

    return Math.Clamp(height, minHeight, maxHeight);
  }
}
