namespace TimeWarp.Flexbox;

/// <summary>
/// Specifies whether flex items wrap onto multiple lines.
/// Maps to CSS flex-wrap property.
/// </summary>
public enum FlexWrap
{
  /// <summary>
  /// Flex items are laid out in a single line (default).
  /// </summary>
  NoWrap,

  /// <summary>
  /// Flex items wrap onto multiple lines from top to bottom.
  /// </summary>
  Wrap,

  /// <summary>
  /// Flex items wrap onto multiple lines from bottom to top.
  /// </summary>
  WrapReverse
}
