# Task 145 - Port Remaining Hand-Written Yoga Unit Tests

## Summary

Task 138 ported the generated conformance suite (all 25 files) and the four
high-priority hand-written suites (Measure, MeasureCache, MeasureMode,
Relayout). The remaining hand-written Yoga unit tests are still unported —
coverage gaps, not known bugs. Recommended before 1.0 but not blocking
(task 147 may proceed in parallel).

## Source Files

Yoga clone `tests/` directory (clone facebook/yoga; see the conventions and
API mapping in `kanban/done/138-integration-testing.md` and the agent prompt
patterns used there):

- YGBaselineFuncTest.cpp, YGAlignBaselineTest.cpp (baseline callbacks)
- YGAspectRatioTest.cpp
- YGEdgeTest.cpp, YGComputedMarginTest.cpp, YGComputedPaddingTest.cpp
- YGHadOverflowTest.cpp
- YGRoundingFunctionTest.cpp, YGRoundingMeasureFuncTest.cpp, YGScaleChangeTest.cpp
- YGZeroOutLayoutRecursivelyTest.cpp
- YGPersistenceTest.cpp, YGPersistentNodeCloningTest.cpp
- YGDirtiedTest.cpp, YGDirtyMarkingTest.cpp (partially covered by NodeTests)
- YGNodeChildTest.cpp, YGCloneNodeTest.cpp, YGNodeCallbackTest.cpp
- FlexGapTest.cpp, YGAutoMinSizeTest.cpp, YGDefaultValuesTest.cpp

## Todo List

- [ ] Port the suites above to `test/timewarp-flexbox-tests/` (Fixie
      conventions, snake_case Yoga names, 2-space indent)
- [ ] Omit tests using unsupported features (GTEST_SKIP, experimental features
      other than WebFlexBasis) and record omissions
- [ ] Expect failures to be real port defects — do not bend tests; fix the
      engine against the C++ (the task-138 pattern: every failure found a
      genuine mistranslation)
- [ ] Full suite green; note the new test count

## Results

(Add after completion)
