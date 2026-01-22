# Task 068-add-has-baseline-func-property

## Summary
Add HasBaselineFunc property to FlexNode to match Yoga's YGNodeHasBaselineFunc. This provides a convenient way to check if a baseline function is set.

## Todo List
- [ ] Add `HasBaselineFunc` property to FlexNode
- [ ] Implement as `BaselineFunc is not null`
- [ ] Add XML documentation for the property
- [ ] Add unit test for HasBaselineFunc behavior
- [ ] Use HasBaselineFunc in baseline alignment code where appropriate

## Notes
Yoga reference (YGNode.h lines 236-239):
```cpp
YG_EXPORT bool YGNodeHasBaselineFunc(YGNodeConstRef node);
```

Yoga Node.h:
```cpp
bool hasBaselineFunc() const noexcept {
  return baselineFunc_ != nullptr;
}
```

This mirrors the existing HasMeasureFunc property pattern.
