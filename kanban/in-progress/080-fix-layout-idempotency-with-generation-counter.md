# Task 080-fix-layout-idempotency-with-generation-counter

## Summary

Fix GitHub Issue #2: `FlexLayoutEngine.CalculateLayout()` is not idempotent. Calling it twice on the same unchanged tree produces incorrect results (child dimensions become 0x0). Implement global generation counter approach matching Facebook Yoga's design.

## Todo List

- [ ] Add global generation counter to `FlexLayoutEngine`
- [ ] Add `LayoutGeneration` field to `LayoutResult` or `FlexNode` to track when node was last calculated
- [ ] Remove `ResetLayoutResults()` call from `CalculateLayout()`
- [ ] Update `LayoutNode()` to accept and use generation counter
- [ ] Modify cache check to include generation validation (`node.LayoutGeneration != currentGeneration`)
- [ ] After calculating node, set `node.LayoutGeneration = currentGeneration`
- [ ] Clear dirty flag after successful layout calculation
- [ ] Add idempotency test: `CalculateLayout_CalledTwice_ShouldBeIdempotent`
- [ ] Add test: `CalculateLayout_AfterStyleChange_ShouldRecalculate`
- [ ] Verify existing tests still pass
- [ ] Update any documentation affected by the change

## Notes

### Root Cause

The current implementation calls `ResetLayoutResults()` at the start of every `CalculateLayout()`, zeroing all node Layout values. When the parent gets a cache hit and returns early, children remain at `{0, 0}`.

### How Facebook Yoga Handles This

From [CalculateLayout.cpp](https://github.com/facebook/yoga/blob/main/yoga/algorithm/CalculateLayout.cpp):

1. **Global Generation Counter**: Each layout pass increments `gCurrentGenerationCount`
2. **Visit Check**: `needToVisitNode = (node->isDirty() && layout->generationCount != generationCount)`
3. **No Global Reset**: Yoga never zeros all layout values at start
4. **Cache Per Generation**: Cache invalidated when generation mismatches

### Implementation Approach

```csharp
public sealed class FlexLayoutEngine
{
    private static uint GlobalGenerationCount = 0;

    public void CalculateLayout(...)
    {
        uint currentGeneration = ++GlobalGenerationCount;
        
        // REMOVE: ResetLayoutResults(root);
        
        LayoutNode(root, ..., currentGeneration);
    }

    private void LayoutNode(..., uint generationCount)
    {
        bool needToVisit = node.IsDirty || 
                           node.LayoutGeneration != generationCount;
        
        if (!needToVisit && node.TryGetCachedLayout(...))
        {
            // Cache hit - values already correct from previous pass
            return;
        }
        
        // ... calculate layout ...
        
        node.LayoutGeneration = generationCount;
        node.ClearDirty();
    }
}
```

### Key Test Case

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
    
    var child = new FlexNode { Height = FlexValue.Point(10) };
    root.AddChild(child);
    
    var engine = new FlexLayoutEngine();
    
    engine.CalculateLayout(root, 80, 24);
    Assert.Equal(80, child.Layout.Width);
    Assert.Equal(10, child.Layout.Height);
    
    // Second call - must produce identical results
    engine.CalculateLayout(root, 80, 24);
    Assert.Equal(80, child.Layout.Width);
    Assert.Equal(10, child.Layout.Height);
}
```

### References

- GitHub Issue: https://github.com/TimeWarpEngineering/timewarp-flexbox/issues/2
- Analysis: `.agent/workspace/2025-12-09T12-30-00_gh-issue-2-idempotency-analysis.md`
- Yoga Source: https://github.com/facebook/yoga/blob/main/yoga/algorithm/CalculateLayout.cpp

## Results

(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
