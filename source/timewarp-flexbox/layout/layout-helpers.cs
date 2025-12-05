namespace TimeWarp.Flexbox;

/// <summary>
/// Static helper methods for layout calculations.
/// </summary>
public static class LayoutHelpers
{
  /// <summary>
  /// Gets the main axis for a flex direction.
  /// </summary>
  /// <param name="direction">The flex direction.</param>
  /// <returns>The main axis (Row or Column).</returns>
  public static Axis GetMainAxis(FlexDirection direction) => direction switch
  {
    FlexDirection.Row or FlexDirection.RowReverse => Axis.Row,
    FlexDirection.Column or FlexDirection.ColumnReverse => Axis.Column,
    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid flex direction")
  };

  /// <summary>
  /// Gets the cross axis for a flex direction.
  /// </summary>
  /// <param name="direction">The flex direction.</param>
  /// <returns>The cross axis (perpendicular to main axis).</returns>
  public static Axis GetCrossAxis(FlexDirection direction) => direction switch
  {
    FlexDirection.Row or FlexDirection.RowReverse => Axis.Column,
    FlexDirection.Column or FlexDirection.ColumnReverse => Axis.Row,
    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid flex direction")
  };

  /// <summary>
  /// Determines if the flex direction is row-based.
  /// </summary>
  /// <param name="direction">The flex direction.</param>
  /// <returns>True if direction is Row or RowReverse.</returns>
  public static bool IsRow(FlexDirection direction) =>
    direction is FlexDirection.Row or FlexDirection.RowReverse;

  /// <summary>
  /// Determines if the flex direction is column-based.
  /// </summary>
  /// <param name="direction">The flex direction.</param>
  /// <returns>True if direction is Column or ColumnReverse.</returns>
  public static bool IsColumn(FlexDirection direction) =>
    direction is FlexDirection.Column or FlexDirection.ColumnReverse;

  /// <summary>
  /// Determines if the flex direction is reversed.
  /// </summary>
  /// <param name="direction">The flex direction.</param>
  /// <returns>True if direction is RowReverse or ColumnReverse.</returns>
  public static bool IsReverse(FlexDirection direction) =>
    direction is FlexDirection.RowReverse or FlexDirection.ColumnReverse;

  /// <summary>
  /// Determines if the node's main axis is row-based.
  /// </summary>
  /// <param name="node">The flex node.</param>
  /// <returns>True if the node's flex direction is row-based.</returns>
  /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
  public static bool IsMainAxisRow(FlexNode node)
  {
    ArgumentNullException.ThrowIfNull(node);
    return IsRow(node.FlexDirection);
  }

  /// <summary>
  /// Determines if the node's main axis is column-based.
  /// </summary>
  /// <param name="node">The flex node.</param>
  /// <returns>True if the node's flex direction is column-based.</returns>
  /// <exception cref="ArgumentNullException">Thrown when node is null.</exception>
  public static bool IsMainAxisColumn(FlexNode node)
  {
    ArgumentNullException.ThrowIfNull(node);
    return IsColumn(node.FlexDirection);
  }

  /// <summary>
  /// Resolves the effective flex direction based on layout direction (RTL support).
  /// In RTL mode, Row becomes RowReverse and vice versa.
  /// </summary>
  /// <param name="flexDirection">The flex direction.</param>
  /// <param name="layoutDirection">The layout direction (LTR/RTL).</param>
  /// <returns>The resolved flex direction.</returns>
  public static FlexDirection ResolveFlexDirection(FlexDirection flexDirection, Direction layoutDirection)
  {
    if (layoutDirection != Direction.Rtl)
      return flexDirection;

    return flexDirection switch
    {
      FlexDirection.Row => FlexDirection.RowReverse,
      FlexDirection.RowReverse => FlexDirection.Row,
      _ => flexDirection // Column directions are not affected by RTL
    };
  }

  /// <summary>
  /// Gets the leading edge for the main axis.
  /// </summary>
  /// <param name="direction">The flex direction.</param>
  /// <returns>The leading edge.</returns>
  public static Edge GetLeadingEdge(FlexDirection direction) => direction switch
  {
    FlexDirection.Row => Edge.Left,
    FlexDirection.RowReverse => Edge.Right,
    FlexDirection.Column => Edge.Top,
    FlexDirection.ColumnReverse => Edge.Bottom,
    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid flex direction")
  };

  /// <summary>
  /// Gets the trailing edge for the main axis.
  /// </summary>
  /// <param name="direction">The flex direction.</param>
  /// <returns>The trailing edge.</returns>
  public static Edge GetTrailingEdge(FlexDirection direction) => direction switch
  {
    FlexDirection.Row => Edge.Right,
    FlexDirection.RowReverse => Edge.Left,
    FlexDirection.Column => Edge.Bottom,
    FlexDirection.ColumnReverse => Edge.Top,
    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid flex direction")
  };
}
