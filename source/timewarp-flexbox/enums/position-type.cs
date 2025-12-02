namespace TimeWarp.Flexbox;

/// <summary>
/// Specifies the positioning method for an element.
/// Maps to CSS position property values used in flexbox contexts.
/// </summary>
public enum PositionType
{
  /// <summary>
  /// Element is positioned according to the normal flow (default).
  /// </summary>
  Relative,

  /// <summary>
  /// Element is removed from normal flow and positioned relative to its containing block.
  /// </summary>
  Absolute
}
