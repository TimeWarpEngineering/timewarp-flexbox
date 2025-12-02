namespace TimeWarp.Flexbox;

/// <summary>
/// Overrides the parent's AlignItems value for a specific flex item.
/// Maps to CSS align-self property.
/// </summary>
public enum AlignSelf
{
  /// <summary>
  /// Inherits the align-items value from the parent (default).
  /// </summary>
  Auto,

  /// <summary>
  /// Item is packed toward the start of the cross axis.
  /// </summary>
  FlexStart,

  /// <summary>
  /// Item is packed toward the end of the cross axis.
  /// </summary>
  FlexEnd,

  /// <summary>
  /// Item is centered along the cross axis.
  /// </summary>
  Center,

  /// <summary>
  /// Item is aligned such that its baseline aligns with other baselines.
  /// </summary>
  Baseline,

  /// <summary>
  /// Item is stretched to fill the container.
  /// </summary>
  Stretch
}
