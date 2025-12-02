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
  /// The resolved value in points, or float.NaN for Auto/Undefined values.
  /// </returns>
  public static float ResolveValue(FlexValue value, float containerSize) => value.Unit switch
  {
    Unit.Point => value.Value,
    Unit.Percent => containerSize * value.Value / 100f,
    Unit.Auto => float.NaN,
    Unit.Undefined => float.NaN,
    _ => float.NaN
  };

  /// <summary>
  /// Resolves a FlexValue to a concrete float value, with a default for Auto/Undefined.
  /// </summary>
  /// <param name="value">The flex value to resolve.</param>
  /// <param name="containerSize">The container size for percentage calculations.</param>
  /// <param name="defaultValue">The default value to use for Auto/Undefined.</param>
  /// <returns>The resolved value in points.</returns>
  public static float ResolveValueOrDefault(FlexValue value, float containerSize, float defaultValue) => value.Unit switch
  {
    Unit.Point => value.Value,
    Unit.Percent => containerSize * value.Value / 100f,
    Unit.Auto => defaultValue,
    Unit.Undefined => defaultValue,
    _ => defaultValue
  };

  /// <summary>
  /// Determines if a FlexValue is defined (Point or Percent).
  /// </summary>
  /// <param name="value">The flex value to check.</param>
  /// <returns>True if the value is defined.</returns>
  public static bool IsDefined(FlexValue value) =>
    value.Unit is Unit.Point or Unit.Percent;

  /// <summary>
  /// Determines if a float value is defined (not NaN).
  /// </summary>
  /// <param name="value">The float value to check.</param>
  /// <returns>True if the value is not NaN.</returns>
  public static bool IsDefined(float value) => !float.IsNaN(value);

  /// <summary>
  /// Resolves the width of a node, considering min/max constraints.
  /// </summary>
  /// <param name="node">The flex node.</param>
  /// <param name="containerWidth">The container width for percentage calculations.</param>
  /// <returns>The resolved width, or float.NaN if undefined.</returns>
  /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
  public static float ResolveWidth(FlexNode node, float containerWidth)
  {
    ArgumentNullException.ThrowIfNull(node);

    float width = ResolveValue(node.Width, containerWidth);

    if (!IsDefined(width))
      return float.NaN;

    float minWidth = ResolveValueOrDefault(node.MinWidth, containerWidth, 0);
    float maxWidth = ResolveValueOrDefault(node.MaxWidth, containerWidth, float.PositiveInfinity);

    return Math.Clamp(width, minWidth, maxWidth);
  }

  /// <summary>
  /// Resolves the height of a node, considering min/max constraints.
  /// </summary>
  /// <param name="node">The flex node.</param>
  /// <param name="containerHeight">The container height for percentage calculations.</param>
  /// <returns>The resolved height, or float.NaN if undefined.</returns>
  /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
  public static float ResolveHeight(FlexNode node, float containerHeight)
  {
    ArgumentNullException.ThrowIfNull(node);

    float height = ResolveValue(node.Height, containerHeight);

    if (!IsDefined(height))
      return float.NaN;

    float minHeight = ResolveValueOrDefault(node.MinHeight, containerHeight, 0);
    float maxHeight = ResolveValueOrDefault(node.MaxHeight, containerHeight, float.PositiveInfinity);

    return Math.Clamp(height, minHeight, maxHeight);
  }
}
