namespace TimeWarp.Flexbox;

/// <summary>
/// Defines how flex items are distributed along the main axis.
/// Maps to CSS justify-content property.
/// </summary>
public enum JustifyContent
{
  /// <summary>
  /// Items are packed toward the start of the main axis (default).
  /// </summary>
  FlexStart,

  /// <summary>
  /// Items are packed toward the end of the main axis.
  /// </summary>
  FlexEnd,

  /// <summary>
  /// Items are centered along the main axis.
  /// </summary>
  Center,

  /// <summary>
  /// Items are evenly distributed; first item at start, last item at end.
  /// </summary>
  SpaceBetween,

  /// <summary>
  /// Items are evenly distributed with equal space around each item.
  /// </summary>
  SpaceAround,

  /// <summary>
  /// Items are evenly distributed with equal space between them.
  /// </summary>
  SpaceEvenly
}
