namespace TimeWarp.Flexbox;

/// <summary>
/// Determines how the total width and height of an element are calculated.
/// </summary>
public enum BoxSizing
{
  /// <summary>
  /// Width and height include content, padding, and border.
  /// This is the default for flexbox layouts.
  /// </summary>
  BorderBox,

  /// <summary>
  /// Width and height include only the content area.
  /// Padding and border are added to the specified dimensions.
  /// </summary>
  ContentBox
}
