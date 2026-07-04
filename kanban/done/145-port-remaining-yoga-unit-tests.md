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

- [x] Port the suites above to `test/timewarp-flexbox-tests/` (Fixie
      conventions, snake_case Yoga names, 2-space indent)
- [x] Omit tests using unsupported features (GTEST_SKIP, experimental features
      other than WebFlexBasis) and record omissions
- [x] Expect failures to be real port defects — do not bend tests; fix the
      engine against the C++ (the task-138 pattern: every failure found a
      genuine mistranslation)
- [x] Full suite green; note the new test count

## Results

(Add after completion)

## Results (2026-07-04)

All 21 suites ported (123 new tests; suite grew 1333 -> 1456, all passing).
Ported via three parallel worktree agents; every failure was treated as an
engine defect and fixed against the C++. FIVE genuine engine defects found
and fixed:

1. PixelGrid rounding compared the fractional part as float, not double -
   `RoundValueToPixelGrid(-3.5001, 1, false, false)` returned -3 instead of
   -4 (float cast collapsed the 1e-4 tolerance boundary). Now uses the
   double InexactEquals overload like C++.
2. ZeroOutLayoutRecursively only zeroed dimensions; C++ resets the ENTIRE
   layout (positions, margins, cached measurements) via `getLayout() = {}`
   and calls cloneChildrenIfNeeded before recursing. Also fixed the missing
   performLayout guard at the display:none call site (C++ avoids mutating
   during measure-only passes to prevent leaked hasNewLayout flags).
3. Node.Clone() kept the source's Owner; C++ YGNodeClone clears it (a clone
   starts detached).
4. CloneChildrenIfNeeded missed the branch where a cloned child is itself
   display:contents (its children must be cloned recursively).
5. CleanupContentsNodesRecursively diverged wholesale: missing
   didPerformLayout parameter, missing CloneContentsChildrenIfNeeded /
   CloneChildrenIfNeeded calls, only zeroed dimensions instead of full
   layout reset, and wrongly recursed into regular (non-contents) children.
   Rewritten to C++ parity; call sites pass the correct flag per C++.

Three stale hand-written tests that codified the old behaviors were updated.

Recorded omissions: 16 YGAutoMinSizeTest tests (auto-min-size extension:
errata bit + min-content measure funcs not in the engine - candidate for a
backlog task), 3 free-semantics tests (no GC equivalent), 1 duplicate
already in NodeTests, 1 null-return GetChild assertion (C# throws).

Verified: gated Release build clean, suite 1456/0/0, AOT smoke PASS,
dotnet format stable, ganda audit 22/22.
