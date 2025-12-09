# Task 073-add-multiple-cached-measurements

## Summary
Implement multiple cached measurements to match Yoga's caching strategy. Yoga caches up to 8 measurements per node for different constraint combinations.

## Todo List
- [ ] Create CachedMeasurement struct matching Yoga's structure
- [ ] Add array of cached measurements to FlexNode (max 8)
- [ ] Add nextCachedMeasurementsIndex tracking
- [ ] Update TryGetCachedLayout to search all cached entries
- [ ] Update SetCachedLayout to add to cache array with rotation
- [ ] Implement cache hit logic matching Yoga's canUseCachedMeasurement
- [ ] Add cache statistics/debugging support
- [ ] Add unit tests for multiple cache entries
- [ ] Benchmark performance improvement from enhanced caching

## Notes
Yoga reference (LayoutResults.h lines 23-39):
```cpp
static constexpr int32_t MaxCachedMeasurements = 8;

uint32_t nextCachedMeasurementsIndex = 0;
std::array<CachedMeasurement, MaxCachedMeasurements> cachedMeasurements = {};
CachedMeasurement cachedLayout{};
```

Yoga CachedMeasurement.h:
```cpp
struct CachedMeasurement {
  float availableWidth = YGUndefined;
  float availableHeight = YGUndefined;
  SizingMode widthSizingMode = SizingMode::MaxContent;
  SizingMode heightSizingMode = SizingMode::MaxContent;
  float computedWidth = -1;
  float computedHeight = -1;
};
```

Multiple cache entries help when nodes are measured multiple times with different constraints (common in multi-pass layout).
