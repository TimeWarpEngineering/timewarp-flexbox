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
  Auto,

  /// <summary>
  /// Size to fit content without wrapping (CSS intrinsic sizing).
  /// The element will be sized to its maximum content size.
  /// </summary>
  MaxContent,

  /// <summary>
  /// Size to fit content, but not exceed available space (CSS intrinsic sizing).
  /// The element will be sized to min(max-content, max(min-content, stretch-fit-size)).
  /// </summary>
  FitContent,

  /// <summary>
  /// Stretch to fill available space (CSS intrinsic sizing).
  /// For flex items, this causes the item to fill the available space in the flex container.
  /// </summary>
  Stretch
}
