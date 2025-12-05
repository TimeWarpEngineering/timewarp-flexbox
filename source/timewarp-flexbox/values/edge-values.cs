namespace TimeWarp.Flexbox;

/// <summary>
/// Stores values for each edge with CSS-style cascade resolution.
/// Used for margin, padding, border, and position values.
/// </summary>
/// <typeparam name="T">The value type (typically FlexValue).</typeparam>
public struct EdgeValues<T> : IEquatable<EdgeValues<T>> where T : struct
{
  // Individual edge values (explicitly set)
  private T? LeftValue;
  private T? TopValue;
  private T? RightValue;
  private T? BottomValue;

  // Logical edge values (RTL-aware)
  private T? StartValue;
  private T? EndValue;

  // Group values
  private T? HorizontalValue;
  private T? VerticalValue;
  private T? AllValue;

  /// <summary>
  /// Gets or sets the value for a specific edge.
  /// Setting a group edge (All, Horizontal, Vertical) affects multiple edges.
  /// </summary>
  public T? this[Edge edge]
  {
    get => edge switch
    {
      Edge.Left => LeftValue,
      Edge.Top => TopValue,
      Edge.Right => RightValue,
      Edge.Bottom => BottomValue,
      Edge.Start => StartValue,
      Edge.End => EndValue,
      Edge.Horizontal => HorizontalValue,
      Edge.Vertical => VerticalValue,
      Edge.All => AllValue,
      _ => default
    };
    set
    {
      switch (edge)
      {
        case Edge.Left:
          LeftValue = value;
          break;
        case Edge.Top:
          TopValue = value;
          break;
        case Edge.Right:
          RightValue = value;
          break;
        case Edge.Bottom:
          BottomValue = value;
          break;
        case Edge.Start:
          StartValue = value;
          break;
        case Edge.End:
          EndValue = value;
          break;
        case Edge.Horizontal:
          HorizontalValue = value;
          break;
        case Edge.Vertical:
          VerticalValue = value;
          break;
        case Edge.All:
          AllValue = value;
          break;
      }
    }
  }

  /// <summary>
  /// Gets the computed left value with cascade resolution.
  /// Resolution order: Left -> Start (LTR) -> Horizontal -> All -> default.
  /// </summary>
  /// <param name="defaultValue">The default value if no edge is set.</param>
  /// <param name="isRtl">Whether the layout direction is right-to-left.</param>
  public T ComputedLeft(T defaultValue, bool isRtl = false)
  {
    if (LeftValue.HasValue)
      return LeftValue.Value;

    // In LTR, Start maps to Left; in RTL, End maps to Left
    T? logicalValue = isRtl ? EndValue : StartValue;
    if (logicalValue.HasValue)
      return logicalValue.Value;

    if (HorizontalValue.HasValue)
      return HorizontalValue.Value;

    if (AllValue.HasValue)
      return AllValue.Value;

    return defaultValue;
  }

  /// <summary>
  /// Gets the computed right value with cascade resolution.
  /// Resolution order: Right -> End (LTR) -> Horizontal -> All -> default.
  /// </summary>
  /// <param name="defaultValue">The default value if no edge is set.</param>
  /// <param name="isRtl">Whether the layout direction is right-to-left.</param>
  public T ComputedRight(T defaultValue, bool isRtl = false)
  {
    if (RightValue.HasValue)
      return RightValue.Value;

    // In LTR, End maps to Right; in RTL, Start maps to Right
    T? logicalValue = isRtl ? StartValue : EndValue;
    if (logicalValue.HasValue)
      return logicalValue.Value;

    if (HorizontalValue.HasValue)
      return HorizontalValue.Value;

    if (AllValue.HasValue)
      return AllValue.Value;

    return defaultValue;
  }

  /// <summary>
  /// Gets the computed top value with cascade resolution.
  /// Resolution order: Top -> Vertical -> All -> default.
  /// </summary>
  /// <param name="defaultValue">The default value if no edge is set.</param>
  public T ComputedTop(T defaultValue)
  {
    if (TopValue.HasValue)
      return TopValue.Value;

    if (VerticalValue.HasValue)
      return VerticalValue.Value;

    if (AllValue.HasValue)
      return AllValue.Value;

    return defaultValue;
  }

  /// <summary>
  /// Gets the computed bottom value with cascade resolution.
  /// Resolution order: Bottom -> Vertical -> All -> default.
  /// </summary>
  /// <param name="defaultValue">The default value if no edge is set.</param>
  public T ComputedBottom(T defaultValue)
  {
    if (BottomValue.HasValue)
      return BottomValue.Value;

    if (VerticalValue.HasValue)
      return VerticalValue.Value;

    if (AllValue.HasValue)
      return AllValue.Value;

    return defaultValue;
  }

  /// <summary>
  /// Sets the value for all edges.
  /// </summary>
  public void SetAll(T value)
  {
    AllValue = value;
  }

  /// <summary>
  /// Sets the value for horizontal edges (Left and Right).
  /// </summary>
  public void SetHorizontal(T value)
  {
    HorizontalValue = value;
  }

  /// <summary>
  /// Sets the value for vertical edges (Top and Bottom).
  /// </summary>
  public void SetVertical(T value)
  {
    VerticalValue = value;
  }

  /// <summary>
  /// Resets all edge values to their default (unset) state.
  /// </summary>
  public void Reset()
  {
    LeftValue = default;
    TopValue = default;
    RightValue = default;
    BottomValue = default;
    StartValue = default;
    EndValue = default;
    HorizontalValue = default;
    VerticalValue = default;
    AllValue = default;
  }

  public bool Equals(EdgeValues<T> other)
  {
    return Nullable.Equals(LeftValue, other.LeftValue) &&
           Nullable.Equals(TopValue, other.TopValue) &&
           Nullable.Equals(RightValue, other.RightValue) &&
           Nullable.Equals(BottomValue, other.BottomValue) &&
           Nullable.Equals(StartValue, other.StartValue) &&
           Nullable.Equals(EndValue, other.EndValue) &&
           Nullable.Equals(HorizontalValue, other.HorizontalValue) &&
           Nullable.Equals(VerticalValue, other.VerticalValue) &&
           Nullable.Equals(AllValue, other.AllValue);
  }

  public override bool Equals(object? obj) => obj is EdgeValues<T> other && Equals(other);

  public override int GetHashCode()
  {
    HashCode hash = new();
    hash.Add(LeftValue);
    hash.Add(TopValue);
    hash.Add(RightValue);
    hash.Add(BottomValue);
    hash.Add(StartValue);
    hash.Add(EndValue);
    hash.Add(HorizontalValue);
    hash.Add(VerticalValue);
    hash.Add(AllValue);
    return hash.ToHashCode();
  }

  public static bool operator ==(EdgeValues<T> left, EdgeValues<T> right) => left.Equals(right);

  public static bool operator !=(EdgeValues<T> left, EdgeValues<T> right) => !left.Equals(right);
}
