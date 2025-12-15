namespace TimeWarp.Flexbox;

/// <summary>
/// Specifies the display behavior of an element.
/// Maps to CSS display property values used in flexbox contexts.
/// </summary>
public enum Display
{
  /// <summary>
  /// Element generates a flex container box (default).
  /// </summary>
  Flex,

  /// <summary>
  /// Element generates no boxes and is not rendered.
  /// </summary>
  None,

  /// <summary>
  /// Element generates no box itself, but its children are laid out as if they
  /// were direct children of the element's parent. Effectively "unwraps" the element
  /// for layout purposes while preserving the logical tree structure.
  /// </summary>
  Contents
}
