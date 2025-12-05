namespace TimeWarp.Flexbox;

/// <summary>
/// Stores the computed layout values after flexbox calculation completes.
/// All values are floats for W3C spec compliance.
/// </summary>
public class LayoutResult
{
  /// <summary>
  /// Gets or sets the left position relative to parent.
  /// </summary>
  public float Left { get; internal set; }

  /// <summary>
  /// Gets or sets the top position relative to parent.
  /// </summary>
  public float Top { get; internal set; }

  /// <summary>
  /// Gets or sets the content box width.
  /// </summary>
  public float Width { get; internal set; }

  /// <summary>
  /// Gets or sets the content box height.
  /// </summary>
  public float Height { get; internal set; }

  /// <summary>
  /// Gets the right edge position (Left + Width).
  /// </summary>
  public float Right => Left + Width;

  /// <summary>
  /// Gets the bottom edge position (Top + Height).
  /// </summary>
  public float Bottom => Top + Height;

  /// <summary>
  /// Gets or sets the resolved left padding.
  /// </summary>
  public float PaddingLeft { get; internal set; }

  /// <summary>
  /// Gets or sets the resolved top padding.
  /// </summary>
  public float PaddingTop { get; internal set; }

  /// <summary>
  /// Gets or sets the resolved right padding.
  /// </summary>
  public float PaddingRight { get; internal set; }

  /// <summary>
  /// Gets or sets the resolved bottom padding.
  /// </summary>
  public float PaddingBottom { get; internal set; }

  /// <summary>
  /// Gets or sets the resolved left border width.
  /// </summary>
  public float BorderLeft { get; internal set; }

  /// <summary>
  /// Gets or sets the resolved top border width.
  /// </summary>
  public float BorderTop { get; internal set; }

  /// <summary>
  /// Gets or sets the resolved right border width.
  /// </summary>
  public float BorderRight { get; internal set; }

  /// <summary>
  /// Gets or sets the resolved bottom border width.
  /// </summary>
  public float BorderBottom { get; internal set; }

  /// <summary>
  /// Gets or sets whether the layout calculation resulted in overflow.
  /// </summary>
  public bool HadOverflow { get; internal set; }

  /// <summary>
  /// Gets or sets the resolved layout direction.
  /// </summary>
  public FlexDirection Direction { get; internal set; }

  /// <summary>
  /// Sets the position for a specific physical edge.
  /// For row layouts, use Left/Right. For column layouts, use Top/Bottom.
  /// </summary>
  /// <param name="edge">The physical edge to set.</param>
  /// <param name="value">The position value.</param>
  /// <param name="containerSize">The container size on the same axis (width for Left/Right, height for Top/Bottom).</param>
  /// <param name="itemSize">The item size on the same axis (width for Left/Right, height for Top/Bottom).</param>
  internal void SetPosition(Edge edge, float value, float containerSize, float itemSize)
  {
    switch (edge)
    {
      case Edge.Left:
        Left = value;
        break;
      case Edge.Top:
        Top = value;
        break;
      case Edge.Right:
        // Convert Right position to Left: Left = containerSize - Right - itemWidth
        Left = containerSize - value - itemSize;
        break;
      case Edge.Bottom:
        // Convert Bottom position to Top: Top = containerSize - Bottom - itemHeight
        Top = containerSize - value - itemSize;
        break;
    }
  }

  /// <summary>
  /// Resets all layout values to their default state.
  /// </summary>
  public void Reset()
  {
    Left = 0;
    Top = 0;
    Width = 0;
    Height = 0;
    PaddingLeft = 0;
    PaddingTop = 0;
    PaddingRight = 0;
    PaddingBottom = 0;
    BorderLeft = 0;
    BorderTop = 0;
    BorderRight = 0;
    BorderBottom = 0;
    HadOverflow = false;
    Direction = FlexDirection.Row;
  }
}
