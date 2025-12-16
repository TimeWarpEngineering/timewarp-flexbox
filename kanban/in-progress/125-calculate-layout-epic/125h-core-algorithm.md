# Task 125h - Core Layout Algorithm (calculateLayoutImpl)

## Summary

Port the main `calculateLayoutImpl` function - the heart of the flexbox layout algorithm implementing all 11 steps. This is the largest and most complex single function (~900 lines).

## Source Files

| Type       | Path                                 | Lines       |
| ---------- | ------------------------------------ | ----------- |
| C++ Source | `yoga/algorithm/CalculateLayout.cpp` | 1214-2139   |

## Function to Port

### calculateLayoutImpl (lines 1214-2139)
The main recursive layout function that implements the 11-step flexbox algorithm.

```cpp
static void calculateLayoutImpl(
    yoga::Node* node,
    const float availableWidth,
    const float availableHeight,
    const Direction ownerDirection,
    const SizingMode widthSizingMode,
    const SizingMode heightSizingMode,
    const float ownerWidth,
    const float ownerHeight,
    const bool performLayout,
    const LayoutPassReason reason,
    LayoutData& layoutMarkerData,
    const uint32_t depth,
    const uint32_t generationCount);
```

## Algorithm Steps

### STEP 1: Calculate values for remainder of algorithm (lines 1370-1396)
- Resolve main/cross axes
- Calculate padding, border, margin
- Determine sizing modes for main/cross

### STEP 2: Determine available size (lines 1398-1420)
- Calculate availableInnerWidth/Height
- Apply min/max constraints

### STEP 3: Determine flex basis for each item (lines 1422-1443)
- Call computeFlexBasisForChildren
- Add gap spacing

### STEP 4: Collect flex items into flex lines (lines 1453-1479)
- Loop calling calculateFlexLine until all children processed
- Track line count and accumulated cross dimensions

### STEP 5: Resolve flexible lengths (lines 1483-1582)
- Apply min/max to available main dimension
- Call resolveFlexibleLength

### STEP 6: Main-axis justification & cross-axis size (lines 1588-1644)
- Call justifyMainAxis
- Bound cross dimension

### STEP 7: Cross-axis alignment (lines 1646-1760)
- Handle stretch alignment (may recurse)
- Handle auto margins
- Apply align-items/align-self

### STEP 8: Multi-line content alignment (lines 1768-1988)
- Apply align-content for wrapped containers
- Handle baseline alignment across lines

### STEP 9: Compute final dimensions (lines 1990-2081)
- Set measuredDimensions
- Handle overflow:scroll

### STEP 10: Set trailing positions (lines 2097-2119)
- Set flexEnd positions for children

### STEP 11: Size and position absolute children (lines 2121-2138)
- Call layoutAbsoluteDescendants

## Target Files

| Type      | Path                                                           |
| --------- | -------------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Algorithm/CalculateLayoutImpl.cs`     |

## Todo List

- [ ] Set up function skeleton with all parameters
- [ ] STEP 1: Port axis resolution and padding/border/margin setup
- [ ] STEP 2: Port available dimension calculation
- [ ] STEP 3: Wire up computeFlexBasisForChildren
- [ ] STEP 4: Wire up line collection loop
- [ ] STEP 5: Wire up resolveFlexibleLength
- [ ] STEP 6: Wire up justifyMainAxis
- [ ] STEP 7: Port cross-axis alignment (stretch re-layout)
- [ ] STEP 8: Port multi-line content alignment
- [ ] STEP 9: Port final dimension computation
- [ ] STEP 10: Port trailing position setting
- [ ] STEP 11: Wire up layoutAbsoluteDescendants
- [ ] Handle wrap-reverse position flipping
- [ ] Add integration tests

## Dependencies

- All previous subtasks (125a-125g)
- Task 124: AbsoluteLayout (layoutAbsoluteDescendants)
- Task 123: TrailingPosition helpers
- Node structure and style accessors

## Notes

### Early Returns
The function has several early return paths:
1. Node has measure function -> measureNodeWithMeasureFunc
2. No children -> measureNodeWithoutChildren
3. Fixed size optimization -> measureNodeWithFixedSize

### Recursive Calls
calculateLayoutInternal is called recursively in multiple places:
- computeFlexBasisForChild (Step 3)
- distributeFreeSpaceSecondPass (Step 5)
- Stretch re-layout (Step 7)
- Multi-line stretch (Step 8)

### Display:Contents Handling
After each early return and after main algorithm:
```cpp
cleanupContentsNodesRecursively(node);
```

### Line Loop Structure
```cpp
for (; startOfLineIterator != node->getLayoutChildren().end(); lineCount++) {
    auto flexLine = calculateFlexLine(...);
    // Steps 5-7 for this line
    totalLineCrossDim += flexLine.layout.crossDim + appliedCrossGap;
    maxLineMainDim = max(maxLineMainDim, flexLine.layout.mainDim);
}
```

### Overflow Handling
```cpp
node->setLayoutHadOverflow(
    node->getLayout().hadOverflow() ||
    (flexLine.layout.remainingFreeSpace < 0));
```

### performLayout Flag
When false, we're only measuring (not positioning):
- Skip position setting
- Skip trailing positions
- Skip absolute layout
- Use cheaper code paths where possible
