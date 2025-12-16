# Task 125i - Cache Logic and Public Entry Points

## Summary

Port the caching wrapper (`calculateLayoutInternal`) and the public entry point (`calculateLayout`). These handle cache invalidation, generation counting, and initial setup.

## Source Files

| Type       | Path                                 | Lines       |
| ---------- | ------------------------------------ | ----------- |
| C++ Source | `yoga/algorithm/CalculateLayout.cpp` | 2141-2434   |
| C++ Header | `yoga/algorithm/Cache.h`             | ~31         |
| C++ Source | `yoga/algorithm/Cache.cpp`           | ~100        |

## Functions to Port

### calculateLayoutInternal (lines 2149-2345)
Cache wrapper that checks if layout can be skipped based on cached results.

```cpp
bool calculateLayoutInternal(
    yoga::Node* node,
    float availableWidth,
    float availableHeight,
    Direction ownerDirection,
    SizingMode widthSizingMode,
    SizingMode heightSizingMode,
    float ownerWidth,
    float ownerHeight,
    bool performLayout,
    LayoutPassReason reason,
    LayoutData& layoutMarkerData,
    uint32_t depth,
    uint32_t generationCount);
```

### calculateLayout (lines 2347-2434)
Public entry point that sets up initial state and starts the layout process.

```cpp
void calculateLayout(
    yoga::Node* node,
    float ownerWidth,
    float ownerHeight,
    Direction ownerDirection);
```

### canUseCachedMeasurement (from Cache.cpp)
Determines if a cached measurement can be reused.

```cpp
bool canUseCachedMeasurement(
    SizingMode widthMode,
    float availableWidth,
    SizingMode heightMode,
    float availableHeight,
    SizingMode lastWidthMode,
    float lastAvailableWidth,
    SizingMode lastHeightMode,
    float lastAvailableHeight,
    float lastComputedWidth,
    float lastComputedHeight,
    float marginRow,
    float marginColumn,
    const yoga::Config* config);
```

## Target Files

| Type      | Path                                                        |
| --------- | ----------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Algorithm/CalculateLayout.cs`      |
| C# Source | `source/timewarp-flexbox/Algorithm/LayoutCache.cs`          |

## Todo List

- [ ] Port `canUseCachedMeasurement`
  - Handle floating-point comparison tolerance
  - Handle different sizing mode combinations
- [ ] Port `calculateLayoutInternal`
  - Check needToVisitNode (dirty, generation, config version, direction)
  - Invalidate cache when needed
  - Try cachedLayout for measure nodes
  - Try cachedMeasurements[] array
  - Call calculateLayoutImpl on cache miss
  - Update cache entries
  - Set hasNewLayout flag
- [ ] Port `calculateLayout` (public entry point)
  - Increment global generation counter
  - Resolve initial sizing modes
  - Call calculateLayoutInternal
  - Set final position
  - Call roundLayoutResultsToPixelGrid
  - Emit events
- [ ] Port global generation counter (atomic)
- [ ] Add unit tests for cache behavior
- [ ] Test idempotency (consecutive layouts with no changes)

## Dependencies

- Task 125h: calculateLayoutImpl
- Task 123: PixelGrid rounding
- Config.getVersion()
- Event system

## Notes

### Global Generation Counter
```cpp
std::atomic<uint32_t> gCurrentGenerationCount(0);
// In calculateLayout:
gCurrentGenerationCount.fetch_add(1, std::memory_order_relaxed);
```

C# equivalent:
```csharp
private static int _currentGenerationCount;
// In CalculateLayout:
Interlocked.Increment(ref _currentGenerationCount);
```

### Cache Invalidation Trigger
```cpp
const bool needToVisitNode =
    (node->isDirty() && layout->generationCount != generationCount) ||
    layout->configVersion != node->getConfig()->getVersion() ||
    layout->lastOwnerDirection != ownerDirection;
```

### Cache Lookup Strategy
1. For nodes with measure functions:
   - First try `cachedLayout`
   - Then iterate `cachedMeasurements[]`
2. For layout operations (performLayout=true):
   - Only use `cachedLayout` with exact match
3. For measure operations (performLayout=false):
   - Search `cachedMeasurements[]` for exact match

### Cache Entry Update
```cpp
if (cachedResults == nullptr) {
    CachedMeasurement* newCacheEntry = performLayout
        ? &layout->cachedLayout
        : &layout->cachedMeasurements[layout->nextCachedMeasurementsIndex++];
    // ... populate cache entry
}
```

### MaxCachedMeasurements
The cache array is fixed-size (8 entries) and wraps around:
```cpp
if (layout->nextCachedMeasurementsIndex == LayoutResults::MaxCachedMeasurements) {
    layout->nextCachedMeasurementsIndex = 0;
}
```

### Initial Sizing Mode Resolution
```cpp
if (node->hasDefiniteLength(Dimension::Width, ownerWidth)) {
    width = resolvedWidth + margin;
    widthSizingMode = SizingMode::StretchFit;
} else if (maxWidth.isDefined()) {
    width = maxWidth;
    widthSizingMode = SizingMode::FitContent;
} else {
    width = ownerWidth;
    widthSizingMode = isUndefined(width) ? SizingMode::MaxContent : SizingMode::StretchFit;
}
```
