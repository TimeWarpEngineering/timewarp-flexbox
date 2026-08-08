# GitHub Issue #2 Analysis: FlexLayoutEngine.CalculateLayout() Idempotency Bug

**Date**: 2025-12-09  
**Issue**: [#2 - FlexLayoutEngine.CalculateLayout() is not idempotent - second call produces 0x0 dimensions](https://github.com/TimeWarpEngineering/timewarp-flexbox/issues/2)  
**Status**: OPEN  
**Priority**: High (causes blank screens in TimeWarp.Tui)

---

## Executive Summary

The `FlexLayoutEngine.CalculateLayout()` method is not idempotent due to a **cache-aware design flaw**. On the second call, the parent gets a cache hit and returns early, but children's `Layout` values remain zeroed (from `ResetLayoutResults()`). The fix should align with Facebook Yoga's approach: use a **global generation counter** and **dirty flag checking** to invalidate cache, rather than resetting layout values.

---

## How Facebook Yoga Handles This

After reviewing [Facebook Yoga's CalculateLayout.cpp](https://github.com/facebook/yoga/blob/main/yoga/algorithm/CalculateLayout.cpp), the original library handles idempotency through several key mechanisms:

### 1. Global Generation Counter

```cpp
std::atomic<uint32_t> gCurrentGenerationCount(0);

void calculateLayout(...) {
    // Increment the generation count. This will force the recursive routine to
    // visit all dirty nodes at least once.
    gCurrentGenerationCount.fetch_add(1, std::memory_order_relaxed);
    // ...
}
```

Each layout pass increments a global generation counter. This ensures cache entries from previous passes are invalidated.

### 2. Dirty Flag + Generation Check

```cpp
bool calculateLayoutInternal(...) {
    const bool needToVisitNode =
        (node->isDirty() && layout->generationCount != generationCount) ||
        layout->configVersion != node->getConfig()->getVersion() ||
        layout->lastOwnerDirection != ownerDirection;

    if (needToVisitNode) {
        // Invalidate the cached results.
        layout->nextCachedMeasurementsIndex = 0;
        layout->cachedLayout.availableWidth = -1;
        // ...
    }
    // ...
}
```

Yoga checks if a node **needs to be visited** based on:
- `node->isDirty()` - Whether style properties changed
- `layout->generationCount != generationCount` - Whether this is a new layout pass
- `layout->configVersion` - Whether config changed
- `layout->lastOwnerDirection` - Whether direction changed

### 3. No Global Reset of Layout Values

**Yoga does NOT call a `ResetLayoutResults()` equivalent at the start of layout.** Instead, it:
- Only resets/zeros nodes with `Display::None` via `zeroOutLayoutRecursively()`
- Stores computed values directly into the layout structure
- Relies on the dirty/generation system to know when to recalculate

### 4. Cache Stores Measured Dimensions Separately

Yoga separates `measuredDimension` (calculated size) from `position` (final position):

```cpp
// After layout completes:
if (performLayout) {
    node->setLayoutDimension(
        node->getLayout().measuredDimension(Dimension::Width),
        Dimension::Width);
    // ...
    node->setHasNewLayout(true);
    node->setDirty(false);
}
```

### 5. Multiple Cached Measurements

Yoga maintains an array of cached measurements, not just one:

```cpp
static constexpr size_t MaxCachedMeasurements = 16;
CachedMeasurement cachedMeasurements[MaxCachedMeasurements];
```

This allows reuse when measuring the same node multiple times with different constraints.

---

## Current TimeWarp.Flexbox Implementation Flaws

### Problem 1: Global Reset Before Layout

```csharp
public void CalculateLayout(...)
{
    ResetLayoutResults(root);  // ❌ Zeros ALL nodes' Layout values
    LayoutNode(...);
}
```

This destroys previously calculated child layouts before checking if they're still valid.

### Problem 2: Cache Doesn't Account for Generation

The current cache key includes `Generation`, but it's per-node and only increments when style properties change:

```csharp
public uint Generation => LayoutGeneration;  // Only changes on style change
```

For idempotent calls (same style), generation stays the same, but layout values were zeroed.

### Problem 3: Early Return Doesn't Restore Children

```csharp
if (node.TryGetCachedLayout(..., out cached))
{
    node.Layout.Width = cached.ComputedWidth;
    // ...
    return;  // ❌ Children stay at {0, 0}
}
```

---

## Root Cause Summary

| Step | What Happens | Problem |
|------|--------------|---------|
| 1 | `ResetLayoutResults()` zeros ALL nodes | Children become `{0, 0}` |
| 2 | Parent gets cache hit | Returns early |
| 3 | Children never visited | Remain `{0, 0}` |

---

## Recommended Fix (Align with Yoga)

### Option 1: Add Global Generation Counter (Recommended)

```csharp
public sealed class FlexLayoutEngine
{
    private static uint GlobalGenerationCount = 0;

    public void CalculateLayout(...)
    {
        // Increment global generation - forces visit to dirty nodes
        uint currentGeneration = ++GlobalGenerationCount;
        
        // DON'T reset layout results
        // ResetLayoutResults(root);  ← REMOVE
        
        LayoutNode(root, ..., currentGeneration);
    }

    private void LayoutNode(..., uint generationCount)
    {
        // Check if we need to recalculate
        bool needToVisit = node.IsDirty || 
                           node.LayoutGeneration != generationCount;
        
        if (!needToVisit && node.TryGetCachedLayout(...))
        {
            // Restore from cache
            // Still need to restore children's positions!
            RestoreChildLayoutsFromCache(node);
            return;
        }
        
        // ... calculate layout ...
        
        // Mark as visited this generation
        node.LayoutGeneration = generationCount;
        node.ClearDirty();
    }
}
```

### Option 2: Remove Reset, Keep Simple Cache

If simpler is preferred:

```csharp
public void CalculateLayout(...)
{
    // REMOVE: ResetLayoutResults(root);
    
    // Let the layout algorithm overwrite values as needed
    LayoutNode(...);
}
```

This works because:
1. Layout algorithm sets Width, Height, Left, Top for every visited node
2. Cache hit → returns cached values (no reset occurred, so children keep their values)
3. No changes between calls → same generation → cache hit → correct values preserved

### Option 3: Clear Cache When Resetting (Least Preferred)

```csharp
private static void ResetLayoutResults(FlexNode node)
{
    node.Layout.Reset();
    node.ClearCache();  // ← Invalidate cache too
    
    foreach (FlexNode child in node.Children)
        ResetLayoutResults(child);
}
```

This ensures no stale cache hits but defeats the purpose of caching for idempotent calls.

---

## Suggested Implementation

**Remove `ResetLayoutResults()` and add global generation tracking:**

1. Add `uint LayoutGeneration` tracking to each node's layout (already exists partially)
2. Add static `GlobalGenerationCount` to `FlexLayoutEngine`
3. Increment generation at start of each `CalculateLayout()`
4. Check `node.LayoutGeneration != currentGeneration` before using cache
5. After calculating, set `node.LayoutGeneration = currentGeneration`

This aligns with Yoga's proven approach and maintains performance benefits of caching.

---

## Test Cases

```csharp
[Fact]
public void CalculateLayout_CalledTwice_ShouldBeIdempotent()
{
    var root = new FlexNode
    {
        Width = FlexValue.Point(80),
        Height = FlexValue.Point(24),
        FlexDirection = FlexDirection.Column
    };
    
    var child = new FlexNode
    {
        Height = FlexValue.Point(10),
        FlexGrow = 0
    };
    
    root.AddChild(child);
    var engine = new FlexLayoutEngine();
    
    // First call
    engine.CalculateLayout(root, 80, 24);
    Assert.Equal(80, child.Layout.Width);
    Assert.Equal(10, child.Layout.Height);
    
    // Second call - should produce identical results
    engine.CalculateLayout(root, 80, 24);
    Assert.Equal(80, child.Layout.Width);   // Currently FAILS
    Assert.Equal(10, child.Layout.Height);  // Currently FAILS
}

[Fact]
public void CalculateLayout_AfterStyleChange_ShouldRecalculate()
{
    var root = new FlexNode { Width = FlexValue.Point(100), Height = FlexValue.Point(100) };
    var child = new FlexNode { Height = FlexValue.Point(50) };
    root.AddChild(child);
    
    var engine = new FlexLayoutEngine();
    engine.CalculateLayout(root, 100, 100);
    Assert.Equal(100, child.Layout.Width);
    
    // Change style
    root.Width = FlexValue.Point(200);
    
    // Should recalculate with new width
    engine.CalculateLayout(root, 200, 100);
    Assert.Equal(200, child.Layout.Width);
}
```

---

## References

- [GitHub Issue #2](https://github.com/TimeWarpEngineering/timewarp-flexbox/issues/2)
- [Facebook Yoga - CalculateLayout.cpp](https://github.com/facebook/yoga/blob/main/yoga/algorithm/CalculateLayout.cpp)
- [Facebook Yoga - Cache.cpp](https://github.com/facebook/yoga/blob/main/yoga/algorithm/Cache.cpp)
- `source/timewarp-flexbox/layout/flex-layout-engine.cs`
- `source/timewarp-flexbox/nodes/flex-node.cache.cs`
