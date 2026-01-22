# Epic 125 - CalculateLayout Algorithm Port

## Summary

Port the main CalculateLayout algorithm from Yoga C++ to C#. This is the core flexbox layout algorithm - approximately 2,500 lines of complex recursive layout logic. This epic breaks down the monolithic task into manageable subtasks following a bottom-up porting strategy.

## Architecture Overview

The algorithm follows an 11-step process:
1. Calculate values for remainder of algorithm
2. Determine available size in main and cross directions
3. Determine flex basis for each item
4. Collect flex items into flex lines
5. Resolve flexible lengths on main axis
6. Main-axis justification & cross-axis size determination
7. Cross-axis alignment
8. Multi-line content alignment
9. Compute final dimensions
10. Set trailing positions for children
11. Size and position absolute children

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                                 | Lines      |
| ---------- | ------------------------------------ | ---------- |
| C++ Header | `yoga/algorithm/CalculateLayout.h`   | ~40        |
| C++ Source | `yoga/algorithm/CalculateLayout.cpp` | **~2,437** |

## Subtasks

| Task | Name | Description | Complexity |
|------|------|-------------|------------|
| 129 | Data Structures | FlexLine, FlexLineRunningLayout, LayoutData | Medium |
| 130 | Helper Functions | constrainMaxSizeForMode, isFixedSize, calculateAvailableInnerDimension | Medium |
| 131 | Measurement Functions | measureNodeWithoutChildren, measureNodeWithFixedSize, measureNodeWithMeasureFunc | Medium |
| 132 | Flex Basis Calculation | computeFlexBasisForChild, computeFlexBasisForChildren | High |
| 133 | Flex Line Calculation | calculateFlexLine (from FlexLine.cpp) | Medium |
| 134 | Flex Space Distribution | distributeFreeSpaceFirstPass, distributeFreeSpaceSecondPass, resolveFlexibleLength | High |
| 135 | Main Axis Justification | justifyMainAxis | Medium |
| 136 | Core Algorithm | calculateLayoutImpl (Steps 1-9) | Very High |
| 137 | Cache & Entry Point | calculateLayoutInternal, calculateLayout | High |
| 138 | Integration & Testing | Wire everything together, comprehensive testing | High |

## Dependencies

- Task 123: Algorithm Helpers (BoundAxis, etc.)
- Task 124: AbsoluteLayout
- All Node, Style, Config infrastructure

## Acceptance Criteria

- [ ] All subtasks completed
- [ ] Layout algorithm produces identical results to C++ Yoga
- [ ] All unit tests ported and passing (~3,700 lines)
- [ ] All generated tests ported and passing (~29,000 lines)
- [ ] No regressions in existing functionality
- [ ] Performance is acceptable for production use

## Notes

### Key Complexity Areas

1. **Generation Counter**: Global atomic state for cache invalidation
2. **Two-Pass Flex Distribution**: Simplified algorithm that deviates from W3C spec
3. **Recursive Layout**: Multiple contexts trigger recursive calculateLayoutInternal calls
4. **Layout Caching**: Complex cache lookup with floating-point tolerance
5. **Display: Contents**: Invisible container nodes require special handling
6. **Aspect Ratio**: Affects flex basis, cross-size, and stretch calculations
7. **RTL/Direction**: Logical-to-physical edge mapping
8. **Baseline Alignment**: Track maxAscent/maxDescent per line
9. **Wrap-Reverse**: Post-layout position flipping

### Porting Strategy

Bottom-up approach:
1. Port data structures first
2. Port helper functions (leaf dependencies)
3. Port measurement functions
4. Port flex calculation functions
5. Port the main algorithm
6. Wire up entry points
7. Comprehensive testing

## Test Files

### Unit Tests (~3,700 lines)
- YGMeasureTest, YGMeasureCacheTest, YGMeasureModeTest
- YGBaselineFuncTest, YGAlignBaselineTest
- YGAspectRatioTest, YGEdgeTest
- YGComputedMarginTest, YGComputedPaddingTest
- YGHadOverflowTest, YGRelayoutTest
- YGRoundingFunctionTest, YGRoundingMeasureFuncTest
- YGScaleChangeTest, YGZeroOutLayoutRecursivelyTest
- YGPersistenceTest, YGPersistentNodeCloningTest
- FlexGapTest

### Generated Tests (~29,000 lines)
- 25 test files covering all layout scenarios
