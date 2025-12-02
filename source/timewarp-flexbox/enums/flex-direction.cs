namespace TimeWarp.Flexbox;

/// <summary>
/// Specifies the direction of the main axis in a flex container.
/// Maps to CSS flex-direction property.
/// </summary>
public enum FlexDirection
{
  /// <summary>
  /// Main axis runs horizontally from left to right (default).
  /// </summary>
  Row,

  /// <summary>
  /// Main axis runs vertically from top to bottom.
  /// </summary>
  Column,

  /// <summary>
  /// Main axis runs horizontally from right to left.
  /// </summary>
  RowReverse,

  /// <summary>
  /// Main axis runs vertically from bottom to top.
  /// </summary>
  ColumnReverse
}
