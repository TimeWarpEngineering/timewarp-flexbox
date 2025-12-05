namespace TimeWarp.Flexbox;

/// <summary>
/// Specifies how content that overflows an element's box is handled.
/// Maps to CSS overflow property.
/// </summary>
public enum Overflow
{
  /// <summary>
  /// Content is not clipped and may render outside the element's box (default).
  /// </summary>
  Visible,

  /// <summary>
  /// Content is clipped and no scrollbars are provided.
  /// </summary>
  Hidden,

  /// <summary>
  /// Content is clipped and scrollbars are provided for scrolling.
  /// </summary>
  Scroll
}
