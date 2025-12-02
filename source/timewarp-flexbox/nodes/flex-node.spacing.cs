namespace TimeWarp.Flexbox;

/// <summary>
/// FlexNode partial class containing spacing properties (margin, padding, border, position, gap).
/// </summary>
public partial class FlexNode
{
  // Backing fields for spacing properties
  private EdgeValues<FlexValue> SpacingMargin;
  private EdgeValues<FlexValue> SpacingPadding;
  private EdgeValues<float> SpacingBorder;
  private EdgeValues<FlexValue> SpacingPosition;
  private float SpacingGap;
  private float SpacingRowGap;
  private float SpacingColumnGap;

  #region Edge Value Properties

  /// <summary>
  /// Gets or sets the margin values for each edge.
  /// Use SetMargin for convenient edge-specific setting.
  /// </summary>
  public EdgeValues<FlexValue> Margin
  {
    get => SpacingMargin;
    set
    {
      SpacingMargin = value;
      MarkDirty();
    }
  }

  /// <summary>
  /// Gets or sets the padding values for each edge.
  /// Use SetPadding for convenient edge-specific setting.
  /// </summary>
  public EdgeValues<FlexValue> Padding
  {
    get => SpacingPadding;
    set
    {
      SpacingPadding = value;
      MarkDirty();
    }
  }

  /// <summary>
  /// Gets or sets the border width values for each edge.
  /// Use SetBorder for convenient edge-specific setting.
  /// </summary>
  public EdgeValues<float> Border
  {
    get => SpacingBorder;
    set
    {
      SpacingBorder = value;
      MarkDirty();
    }
  }

  /// <summary>
  /// Gets or sets the position offset values for absolute positioning.
  /// Use SetPosition for convenient edge-specific setting.
  /// </summary>
  public EdgeValues<FlexValue> Position
  {
    get => SpacingPosition;
    set
    {
      SpacingPosition = value;
      MarkDirty();
    }
  }

  #endregion

  #region Gap Properties

  /// <summary>
  /// Gets or sets the gap between items on both axes.
  /// Setting this sets both RowGap and ColumnGap.
  /// Default: 0
  /// </summary>
  public float Gap
  {
    get => SpacingGap;
    set
    {
      if (SpacingGap.Equals(value))
        return;

      SpacingGap = value;
      SpacingRowGap = value;
      SpacingColumnGap = value;
      MarkDirty();
    }
  }

  /// <summary>
  /// Gets or sets the gap between rows (cross axis spacing for wrapped items).
  /// Default: 0
  /// </summary>
  public float RowGap
  {
    get => SpacingRowGap;
    set => SetStyleProperty(ref SpacingRowGap, value);
  }

  /// <summary>
  /// Gets or sets the gap between columns (main axis spacing).
  /// Default: 0
  /// </summary>
  public float ColumnGap
  {
    get => SpacingColumnGap;
    set => SetStyleProperty(ref SpacingColumnGap, value);
  }

  #endregion

  #region Convenience Methods

  /// <summary>
  /// Sets the margin value for a specific edge.
  /// </summary>
  /// <param name="edge">The edge to set.</param>
  /// <param name="value">The margin value.</param>
  public void SetMargin(Edge edge, FlexValue value)
  {
    SpacingMargin[edge] = value;
    MarkDirty();
  }

  /// <summary>
  /// Sets the padding value for a specific edge.
  /// </summary>
  /// <param name="edge">The edge to set.</param>
  /// <param name="value">The padding value.</param>
  public void SetPadding(Edge edge, FlexValue value)
  {
    SpacingPadding[edge] = value;
    MarkDirty();
  }

  /// <summary>
  /// Sets the border width for a specific edge.
  /// </summary>
  /// <param name="edge">The edge to set.</param>
  /// <param name="value">The border width.</param>
  public void SetBorder(Edge edge, float value)
  {
    SpacingBorder[edge] = value;
    MarkDirty();
  }

  /// <summary>
  /// Sets the position offset for a specific edge (used with absolute positioning).
  /// </summary>
  /// <param name="edge">The edge to set.</param>
  /// <param name="value">The position offset value.</param>
  public void SetPosition(Edge edge, FlexValue value)
  {
    SpacingPosition[edge] = value;
    MarkDirty();
  }

  #endregion
}
