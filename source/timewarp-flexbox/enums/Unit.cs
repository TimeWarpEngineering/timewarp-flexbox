namespace TimeWarp.Flexbox;

/// <summary>
/// Represents the unit type for a FlexValue.
/// </summary>
public enum Unit
{
  /// <summary>
  /// Value represents an undefined/unset state.
  /// </summary>
  Undefined,

  /// <summary>
  /// Value represents an absolute point measurement.
  /// </summary>
  Point,

  /// <summary>
  /// Value represents a percentage relative to the parent.
  /// </summary>
  Percent,

  /// <summary>
  /// Value should be automatically calculated by the layout algorithm.
  /// </summary>
  Auto
}
