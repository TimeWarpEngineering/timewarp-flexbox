# Task 148 - Implement FixFlexBasisFitContent Experimental Feature

## Summary

Yoga's `ExperimentalFeature::FixFlexBasisFitContent` is not implemented (the
C# `ExperimentalFeature` enum only carries `WebFlexBasis`). It gates two
behaviors in `computeFlexBasisForChild` (CalculateLayout.cpp): a fit-content
fix for positive resolved flex-basis, and skipping height-axis FitContent for
non-measure container children inside scroll subtrees (prevents re-measure
cascades in ScrollViews). One generated conformance test is skipped because of
it (`flex_basis_in_scroll_content_container` in FlexBasisFitContentTests).

Backlog: experimental in upstream Yoga, default-off, no consumer demand yet.
Not a 1.0 blocker — documented as excluded in release notes.

## Todo List

- [ ] Add `FixFlexBasisFitContent` to the `ExperimentalFeature` enum
- [ ] Port the two gated blocks in `FlexBasis.ComputeFlexBasisForChild` from
      the current C++ (including the scroll-ancestor walk)
- [ ] Re-run the test converter for FlexBasisFitContent to include the skipped
      test; port any related upstream unit tests
- [ ] Full suite green

## Results

(Add after completion)
