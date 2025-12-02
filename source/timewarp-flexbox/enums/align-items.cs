namespace TimeWarp.Flexbox;

/// <summary>
/// Defines the default alignment for flex items along the cross axis.
/// Maps to CSS align-items property.
/// </summary>
public enum AlignItems
{
  /// <summary>
  /// Items are packed toward the start of the cross axis.
  /// </summary>
  FlexStart,

  /// <summary>
  /// Items are packed toward the end of the cross axis.
  /// </summary>
  FlexEnd,

  /// <summary>
  /// Items are centered along the cross axis.
  /// </summary>
  Center,

  /// <summary>
  /// Items are aligned such that their baselines align.
  /// </summary>
  Baseline,

  /// <summary>
  /// Items are stretched to fill the container (default).
  /// </summary>
  Stretch
}
