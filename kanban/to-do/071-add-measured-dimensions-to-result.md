# Task 071-add-measured-dimensions-to-result

## Summary
Add measured dimensions to LayoutResult to match Yoga's LayoutResults measuredDimensions. These store dimensions before pixel grid rounding.

## Todo List
- [ ] Add MeasuredWidth, MeasuredHeight properties to LayoutResult
- [ ] Add internal setters for measured dimensions
- [ ] Update layout algorithm to store measured dimensions before rounding
- [ ] Add XML documentation explaining difference from Width/Height
- [ ] Add unit tests for measured dimensions
- [ ] Verify measured dimensions are set correctly

## Notes
Yoga reference (LayoutResults.h lines 65-74):
```cpp
float measuredDimension(Dimension axis) const {
  return measuredDimensions_[yoga::to_underlying(axis)];
}

void setMeasuredDimension(Dimension axis, float dimension) {
  measuredDimensions_[yoga::to_underlying(axis)] = dimension;
}
```

Yoga stores both measured (pre-rounding) and final dimensions:
```cpp
std::array<float, 2> dimensions_ = {{YGUndefined, YGUndefined}};
std::array<float, 2> measuredDimensions_ = {{YGUndefined, YGUndefined}};
```

Measured dimensions are useful for debugging rounding issues.
