namespace TimeWarp.Flexbox;

/// <summary>
/// Defines how flex lines are distributed along the cross axis when there is extra space.
/// Maps to CSS align-content property. Only applies when flex-wrap is enabled.
/// </summary>
public enum AlignContent
{
  /// <summary>
  /// Lines are packed toward the start of the cross axis.
  /// </summary>
  FlexStart,

  /// <summary>
  /// Lines are packed toward the end of the cross axis.
  /// </summary>
  FlexEnd,

  /// <summary>
  /// Lines are centered along the cross axis.
  /// </summary>
  Center,

  /// <summary>
  /// Lines are evenly distributed; first line at start, last line at end.
  /// </summary>
  SpaceBetween,

  /// <summary>
  /// Lines are evenly distributed with equal space around each line.
  /// </summary>
  SpaceAround,

  /// <summary>
  /// Lines are evenly distributed with equal space between them.
  /// </summary>
  SpaceEvenly,

  /// <summary>
  /// Lines are stretched to fill the container (default).
  /// </summary>
  Stretch
}
