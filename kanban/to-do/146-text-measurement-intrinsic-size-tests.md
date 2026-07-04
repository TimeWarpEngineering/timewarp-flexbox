# Task 146 - Text-Measurement Helper and IntrinsicSize Tests

## Summary

~15 of Yoga's generated IntrinsicSize conformance tests are skipped because
they need a text-measurement context: Yoga's test harness attaches an
"intrinsic text" measure function (via `YGNodeSetContext` + a shared measure
helper in `tests/util/TestUtil`) that simulates word-wrapping text. Build the
C# equivalent and unlock those tests. Recommended before 1.0 but not blocking.

## Todo List

- [ ] Study Yoga's text-measure test helper (gentest fixtures use an
      "intrinsic" measure function that wraps words at ~10px per char, and the
      C++ tests pass the text via node context)
- [ ] Implement an equivalent helper in the test project (e.g.
      `IntrinsicTextMeasure.MeasureFunc` reading `Node.Context` as the text)
- [ ] Extend `runfiles/port-generated-tests.cs` to translate
      `YGNodeSetContext(node, (void*)"...")` + `YGNodeSetMeasureFunc(node,
      IntrinsicSizeMeasure)` instead of skipping those tests
- [ ] Re-run the converter for IntrinsicSize; fix any engine divergences the
      new tests expose (compare against C++ as usual)
- [ ] Full suite green; update skill/readme "known gaps" wording once covered

## Results

(Add after completion)
