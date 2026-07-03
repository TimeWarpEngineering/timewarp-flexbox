# Task 138 - Integration and Comprehensive Testing

## Summary

Wire all components together, ensure the public API works end-to-end, and port/run comprehensive test suites.

## Target Files

| Type      | Path                                                           |
| --------- | -------------------------------------------------------------- |
| C# Tests  | `test/timewarp-flexbox-tests/Algorithm/CalculateLayoutTests.cs`|
| C# Tests  | `test/timewarp-flexbox-tests/Algorithm/CacheTests.cs`          |
| C# Tests  | `test/timewarp-flexbox-tests/Generated/*.cs`                   |

## Test Files to Port

### Unit Tests (~3,700 lines total)
| Source Test                          | Priority |
|--------------------------------------|----------|
| YGMeasureTest.cpp                    | High     |
| YGMeasureCacheTest.cpp               | High     |
| YGMeasureModeTest.cpp                | High     |
| YGRelayoutTest.cpp                   | High     |
| YGBaselineFuncTest.cpp               | Medium   |
| YGAlignBaselineTest.cpp              | Medium   |
| YGAspectRatioTest.cpp                | Medium   |
| YGEdgeTest.cpp                       | Medium   |
| YGComputedMarginTest.cpp             | Low      |
| YGComputedPaddingTest.cpp            | Low      |
| YGHadOverflowTest.cpp                | Low      |
| YGRoundingFunctionTest.cpp           | Low      |
| YGRoundingMeasureFuncTest.cpp        | Low      |
| YGScaleChangeTest.cpp                | Low      |
| YGZeroOutLayoutRecursivelyTest.cpp   | Low      |
| YGPersistenceTest.cpp                | Low      |
| YGPersistentNodeCloningTest.cpp      | Low      |
| FlexGapTest.cpp                      | Medium   |

### Generated Tests (~29,000 lines total)
| Source Test                          | Priority |
|--------------------------------------|----------|
| YGFlexDirectionTest.cpp              | High     |
| YGJustifyContentTest.cpp             | High     |
| YGAlignItemsTest.cpp                 | High     |
| YGAlignContentTest.cpp               | High     |
| YGFlexWrapTest.cpp                   | High     |
| YGFlexTest.cpp                       | High     |
| YGAbsolutePositionTest.cpp           | High     |
| YGMarginTest.cpp                     | Medium   |
| YGPaddingTest.cpp                    | Medium   |
| YGBorderTest.cpp                     | Medium   |
| YGMinMaxDimensionTest.cpp            | Medium   |
| YGPercentageTest.cpp                 | Medium   |
| YGGapTest.cpp                        | Medium   |
| YGDisplayTest.cpp                    | Medium   |
| YGDisplayContentsTest.cpp            | Medium   |
| YGBoxSizingTest.cpp                  | Medium   |
| YGRoundingTest.cpp                   | Medium   |
| YGStaticPositionTest.cpp             | Medium   |
| YGIntrinsicSizeTest.cpp              | Medium   |
| YGDimensionTest.cpp                  | Low      |
| YGSizeOverflowTest.cpp               | Low      |
| YGAlignSelfTest.cpp                  | Low      |
| YGAutoTest.cpp                       | Low      |
| YGAspectRatioTest.cpp                | Low      |
| YGAndroidNewsFeed.cpp                | Low      |

## Todo List

- [x] Wire up all subtasks into complete CalculateLayout (fixed 2026-07-03: debug module,
      owner semantics, flex basis FitContent, measure func fast path — see PR #7)
- [x] Verify public API matches expected signature
- [ ] Port high-priority unit tests first (in progress: Measure, MeasureCache, MeasureMode, Relayout)
- [ ] Run tests and fix failures (in progress: 17 wrap/align-content/baseline conformance
      failures under investigation)
- [ ] Port medium-priority tests
- [ ] Port low-priority tests
- [x] Port generated tests (can be automated) — automated via `runfiles/port-generated-tests.cs`;
      first 7 files converted 2026-07-03 (FlexDirection, JustifyContent, AlignItems, AlignContent,
      FlexWrap, Flex, AbsolutePosition = 241 tests). Remaining generated files: re-run the
      converter per file and extend its mapping when it reports unsupported constructs.
- [ ] Performance benchmarking
- [ ] Memory usage profiling
- [ ] Cross-platform validation

## Progress Notes (2026-07-03)

- `runfiles/port-generated-tests.cs <yoga-repo> <Name>...` converts `tests/generated/YG<Name>Test.cpp`
  to `test/timewarp-flexbox-tests/Generated/<Name>Tests.cs`. Tests upstream marks GTEST_SKIP are
  excluded automatically and reported.
- First conversion batch: 241 tests; 224 passed immediately, 17 failed — all in wrapped-line
  cross-axis alignment (align-content with wrap ×11, wrap+align-items ×4, baseline multiline ×2).

## Dependencies

- All subtasks 125a-125i completed
- Test infrastructure from existing test project

## Acceptance Criteria

- [ ] All unit tests pass (100%)
- [ ] All generated tests pass (100%)
- [ ] Layout results match C++ Yoga exactly
- [ ] No memory leaks or excessive allocations
- [ ] Performance within 2x of C++ implementation
- [ ] API is idiomatic C# (not just a transliteration)

## Notes

### Test Porting Strategy
1. Start with smoke tests (basic layout scenarios)
2. Port measure tests (critical for correctness)
3. Port cache tests (critical for performance)
4. Port generated tests in batches by category

### Performance Targets
- Simple layout (10 nodes): < 0.1ms
- Medium layout (100 nodes): < 1ms
- Large layout (1000 nodes): < 10ms
- Memory: No unnecessary allocations per layout pass

### Common Test Patterns
Generated tests follow a standard pattern:
```csharp
[Fact]
public void TestName()
{
    var root = new Node();
    root.StyleSetWidth(100);
    // ... setup
    
    root.CalculateLayout(float.NaN, float.NaN, Direction.LTR);
    
    Assert.Equal(0, root.LayoutGetLeft());
    Assert.Equal(0, root.LayoutGetTop());
    Assert.Equal(100, root.LayoutGetWidth());
    Assert.Equal(100, root.LayoutGetHeight());
}
```

### Idempotency Testing
Critical test: consecutive layouts with no changes should:
1. Return quickly (cache hit)
2. Produce identical results
3. Not modify node state unnecessarily

### Edge Cases to Test
- Empty containers
- Single child
- All absolute children
- display:none children
- display:contents nodes
- RTL direction
- wrap-reverse
- Negative margins
- Percentage dimensions with undefined parent
- Circular aspect ratio dependencies
