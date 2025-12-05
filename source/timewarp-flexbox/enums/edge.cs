namespace TimeWarp.Flexbox;

/// <summary>
/// Represents an edge or group of edges for margin, padding, border, and position values.
/// Supports CSS-style cascade logic.
/// </summary>
public enum Edge
{
  /// <summary>
  /// The left edge.
  /// </summary>
  Left,

  /// <summary>
  /// The top edge.
  /// </summary>
  Top,

  /// <summary>
  /// The right edge.
  /// </summary>
  Right,

  /// <summary>
  /// The bottom edge.
  /// </summary>
  Bottom,

  /// <summary>
  /// The start edge (Left in LTR, Right in RTL).
  /// </summary>
  Start,

  /// <summary>
  /// The end edge (Right in LTR, Left in RTL).
  /// </summary>
  End,

  /// <summary>
  /// Both horizontal edges (Left and Right).
  /// </summary>
  Horizontal,

  /// <summary>
  /// Both vertical edges (Top and Bottom).
  /// </summary>
  Vertical,

  /// <summary>
  /// All four edges.
  /// </summary>
  All
}
