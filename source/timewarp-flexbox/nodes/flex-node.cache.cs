namespace TimeWarp.Flexbox;

/// <summary>
/// FlexNode partial class containing layout caching functionality.
/// </summary>
public partial class FlexNode
{
  // Cache fields
  private LayoutCacheKey CachedLayoutKey;
  private LayoutCacheEntry? CachedLayoutEntry;
  private uint LayoutGeneration;

  /// <summary>
  /// Gets the generation counter for this node.
  /// Incremented when any style property changes.
  /// </summary>
  public uint Generation => LayoutGeneration;

  /// <summary>
  /// Gets or sets the last layout generation when this node was calculated.
  /// Used to determine if the node needs recalculation in the current layout pass.
  /// </summary>
  internal uint LastLayoutGeneration { get; set; }

  /// <summary>
  /// Gets whether this node has a valid cached layout.
  /// </summary>
  public bool HasCachedLayout => CachedLayoutEntry.HasValue;

  /// <summary>
  /// Attempts to retrieve a cached layout result for the given constraints.
  /// </summary>
  /// <param name="availableWidth">The available width.</param>
  /// <param name="availableHeight">The available height.</param>
  /// <param name="widthMode">The width measure mode.</param>
  /// <param name="heightMode">The height measure mode.</param>
  /// <param name="entry">The cached entry if found.</param>
  /// <returns>True if a valid cache entry was found.</returns>
  internal bool TryGetCachedLayout(
    float availableWidth,
    float availableHeight,
    MeasureMode widthMode,
    MeasureMode heightMode,
    out LayoutCacheEntry entry)
  {
    if (!CachedLayoutEntry.HasValue)
    {
      entry = default;
      return false;
    }

    LayoutCacheKey key = new()
    {
      AvailableWidth = availableWidth,
      AvailableHeight = availableHeight,
      WidthMode = widthMode,
      HeightMode = heightMode,
      Generation = LayoutGeneration
    };

    if (CachedLayoutKey == key)
    {
      entry = CachedLayoutEntry.Value;
      return true;
    }

    entry = default;
    return false;
  }

  /// <summary>
  /// Stores a layout result in the cache.
  /// </summary>
  /// <param name="availableWidth">The available width.</param>
  /// <param name="availableHeight">The available height.</param>
  /// <param name="widthMode">The width measure mode.</param>
  /// <param name="heightMode">The height measure mode.</param>
  /// <param name="entry">The layout entry to cache.</param>
  internal void SetCachedLayout(
    float availableWidth,
    float availableHeight,
    MeasureMode widthMode,
    MeasureMode heightMode,
    LayoutCacheEntry entry)
  {
    CachedLayoutKey = new LayoutCacheKey
    {
      AvailableWidth = availableWidth,
      AvailableHeight = availableHeight,
      WidthMode = widthMode,
      HeightMode = heightMode,
      Generation = LayoutGeneration
    };
    CachedLayoutEntry = entry;
  }

  /// <summary>
  /// Invalidates the layout cache for this node.
  /// Called automatically when style properties change.
  /// </summary>
  internal void InvalidateCache()
  {
    LayoutGeneration++;
    CachedLayoutEntry = null;
  }

  /// <summary>
  /// Clears the layout cache without incrementing generation.
  /// Used when explicitly resetting layout.
  /// </summary>
  internal void ClearCache()
  {
    CachedLayoutEntry = null;
  }
}
