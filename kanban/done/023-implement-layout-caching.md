# Task 023-implement-layout-caching

## Summary
Implement layout caching to avoid redundant calculations when node styles haven't changed.

## Todo List
- [ ] Add generation counter to FlexNode for cache invalidation
- [ ] Track style changes that require relayout
- [ ] Implement cached measurement results
- [ ] Add cache hit/miss detection
- [ ] Implement cache key based on constraints (width, height, measure modes)
- [ ] Add cache entry struct with layout results
- [ ] Implement cache invalidation on style property changes
- [ ] Implement subtree cache invalidation on structural changes
- [ ] Add diagnostic counters for cache performance
- [ ] Verify code follows csharp-coding.md standards

## Notes
Layout caching significantly improves performance:

```csharp
public class LayoutCache
{
  public struct CacheKey : IEquatable<CacheKey>
  {
    public float AvailableWidth;
    public float AvailableHeight;
    public MeasureMode WidthMode;
    public MeasureMode HeightMode;
    public uint GenerationCount;
  }
  
  public struct CacheEntry
  {
    public float ComputedWidth;
    public float ComputedHeight;
  }
}

public partial class FlexNode
{
  // Cache fields
  private LayoutCache.CacheKey lastCacheKey;
  private LayoutCache.CacheEntry? cachedLayout;
  private uint generationCount = 0;
  
  // Increment generation on any style change
  private void InvalidateCache()
  {
    generationCount++;
    cachedLayout = null;
    Parent?.InvalidateCache(); // Propagate up
  }
  
  // Check cache before calculating
  internal bool TryGetCachedLayout(CacheKey key, out CacheEntry entry)
  {
    if (cachedLayout.HasValue && lastCacheKey.Equals(key))
    {
      entry = cachedLayout.Value;
      return true;
    }
    entry = default;
    return false;
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
