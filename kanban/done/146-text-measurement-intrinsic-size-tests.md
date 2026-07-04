# Task 146 - Text-Measurement Helper and IntrinsicSize Tests

## Summary

~15 of Yoga's generated IntrinsicSize conformance tests are skipped because
they need a text-measurement context: Yoga's test harness attaches an
"intrinsic text" measure function (via `YGNodeSetContext` + a shared measure
helper in `tests/util/TestUtil`) that simulates word-wrapping text. Build the
C# equivalent and unlock those tests. Recommended before 1.0 but not blocking.

## Todo List

- [x] Study Yoga's text-measure test helper (gentest fixtures use an
      "intrinsic" measure function that wraps words at ~10px per char, and the
      C++ tests pass the text via node context)
- [x] Implement an equivalent helper in the test project (e.g.
      `IntrinsicTextMeasure.MeasureFunc` reading `Node.Context` as the text)
- [x] Extend `runfiles/port-generated-tests.cs` to translate
      `YGNodeSetContext(node, (void*)"...")` + `YGNodeSetMeasureFunc(node,
      IntrinsicSizeMeasure)` instead of skipping those tests
- [x] Re-run the converter for IntrinsicSize; fix any engine divergences the
      new tests expose (compare against C++ as usual)
- [x] Full suite green; update skill/readme "known gaps" wording once covered

## Results (2026-07-04)

Complete. `test/timewarp-flexbox-tests/Generated/IntrinsicSizeMeasure.cs` is a
faithful port of Yoga's tests/util/TestUtil.cpp IntrinsicSizeMeasure (10px per
char, word-wrap height simulation, text via Node.Context). The converter now
translates `YGNodeSetContext(node, (void*)"...")` -> `node.Context = "..."`
and the IntrinsicSizeMeasure SetMeasureFunc pattern, and emits 2-space
indentation matching .editorconfig.

IntrinsicSize conversion went from 5 to 17 tests (12 text-measurement tests
unlocked); the remaining 28 exclusions are all upstream GTEST_SKIPs
(intrinsic sizing keywords Yoga itself disables). ALL 12 new tests passed on
the first run - no engine changes were needed for intrinsic text measurement.
Suite: 1468/0/0; format stable; audit 22/22. Neither the readme nor the skill
listed this as a gap, so no doc wording changes were needed (the 1.0 release
notes simply won't carry the beta.4 caveat).
