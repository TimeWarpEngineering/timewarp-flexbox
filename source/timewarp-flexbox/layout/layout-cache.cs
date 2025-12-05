namespace TimeWarp.Flexbox;

/// <summary>
/// Cache key for layout calculations based on available space and measure modes.
/// </summary>
public readonly struct LayoutCacheKey : IEquatable<LayoutCacheKey>
{
  /// <summary>
  /// The available width for layout.
  /// </summary>
  public float AvailableWidth { get; init; }

  /// <summary>
  /// The available height for layout.
  /// </summary>
  public float AvailableHeight { get; init; }

  /// <summary>
  /// The width measure mode.
  /// </summary>
  public MeasureMode WidthMode { get; init; }

  /// <summary>
  /// The height measure mode.
  /// </summary>
  public MeasureMode HeightMode { get; init; }

  /// <summary>
  /// The generation count when this cache entry was created.
  /// </summary>
  public uint Generation { get; init; }

  /// <inheritdoc />
  public bool Equals(LayoutCacheKey other) =>
    AvailableWidth.Equals(other.AvailableWidth) &&
    AvailableHeight.Equals(other.AvailableHeight) &&
    WidthMode == other.WidthMode &&
    HeightMode == other.HeightMode &&
    Generation == other.Generation;

  /// <inheritdoc />
  public override bool Equals(object? obj) =>
    obj is LayoutCacheKey other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode() =>
    HashCode.Combine(AvailableWidth, AvailableHeight, WidthMode, HeightMode, Generation);

  /// <summary>
  /// Equality operator.
  /// </summary>
  public static bool operator ==(LayoutCacheKey left, LayoutCacheKey right) => left.Equals(right);

  /// <summary>
  /// Inequality operator.
  /// </summary>
  public static bool operator !=(LayoutCacheKey left, LayoutCacheKey right) => !left.Equals(right);
}

/// <summary>
/// Cached layout result for a node.
/// </summary>
public readonly struct LayoutCacheEntry : IEquatable<LayoutCacheEntry>
{
  /// <summary>
  /// The computed width.
  /// </summary>
  public float ComputedWidth { get; init; }

  /// <summary>
  /// The computed height.
  /// </summary>
  public float ComputedHeight { get; init; }

  /// <summary>
  /// The computed left position.
  /// </summary>
  public float ComputedLeft { get; init; }

  /// <summary>
  /// The computed top position.
  /// </summary>
  public float ComputedTop { get; init; }

  /// <inheritdoc />
  public bool Equals(LayoutCacheEntry other) =>
    ComputedWidth.Equals(other.ComputedWidth) &&
    ComputedHeight.Equals(other.ComputedHeight) &&
    ComputedLeft.Equals(other.ComputedLeft) &&
    ComputedTop.Equals(other.ComputedTop);

  /// <inheritdoc />
  public override bool Equals(object? obj) =>
    obj is LayoutCacheEntry other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode() =>
    HashCode.Combine(ComputedWidth, ComputedHeight, ComputedLeft, ComputedTop);

  /// <summary>
  /// Equality operator.
  /// </summary>
  public static bool operator ==(LayoutCacheEntry left, LayoutCacheEntry right) => left.Equals(right);

  /// <summary>
  /// Inequality operator.
  /// </summary>
  public static bool operator !=(LayoutCacheEntry left, LayoutCacheEntry right) => !left.Equals(right);
}
