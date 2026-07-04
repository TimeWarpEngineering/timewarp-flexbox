# Task 149 - Auto Min-Size Extension (Min-Content Measure)

## Summary

Upstream Yoga carries an auto-min-size extension implementing CSS Flexbox §4.5
(automatic minimum size of flex items floored at min-content), gated behind the
`YGErrataMinSizeUndefinedInsteadOfAuto` errata bit, with supporting node APIs:
`YGNodeSetMinContentMeasureFunc`, `YGNodeHasMinContentMeasureFunc`,
`YGNodeSet/GetMinContentWidth/Height`. None of this exists in the C# port.

16 tests from `YGAutoMinSizeTest.cpp` were omitted during task 145 for this
reason (the one portable test, `default_config_preserves_existing_shrink`, was
ported and passes). See the omission list in the header of
`test/timewarp-flexbox-tests/Algorithm/AutoMinSizeTests.cs`.

Backlog: default-off behavior upstream, no consumer demand yet; documented as
excluded in release notes alongside task 148 (FixFlexBasisFitContent).

## Todo List

- [ ] Add the errata bit to the `Errata` enum and thread it through
      `Config.HasErrata` checks per the C++
- [ ] Port the min-content measure plumbing on `Node`
      (min-content measure func + static min-content width/height)
- [ ] Port the algorithm changes (min-content flooring in
      `boundAxisWithinMinAndMax` / flex basis paths - locate in current C++)
- [ ] Port the 16 omitted YGAutoMinSizeTest tests; full suite green

## Results

(Add after completion)
