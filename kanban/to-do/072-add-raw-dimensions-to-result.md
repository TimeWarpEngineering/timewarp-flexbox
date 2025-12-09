# Task 072-add-raw-dimensions-to-result

## Summary
Add raw dimensions to LayoutResult to match Yoga's LayoutResults rawDimensions. These store the original calculated dimensions.

## Todo List
- [ ] Add RawWidth, RawHeight properties to LayoutResult
- [ ] Add internal setters for raw dimensions
- [ ] Update layout algorithm to store raw dimensions
- [ ] Add XML documentation explaining difference from Width/Height
- [ ] Add unit tests for raw dimensions
- [ ] Verify raw dimensions are set correctly

## Notes
Yoga reference (LayoutResults.h lines 69-78):
```cpp
float rawDimension(Dimension axis) const {
  return rawDimensions_[yoga::to_underlying(axis)];
}

void setRawDimension(Dimension axis, float dimension) {
  rawDimensions_[yoga::to_underlying(axis)] = dimension;
}
```

Yoga Node.cpp setLayoutDimension:
```cpp
void Node::setLayoutDimension(float lengthValue, Dimension dimension) {
  layout_.setDimension(dimension, lengthValue);
  layout_.setRawDimension(dimension, lengthValue);  // Store raw value
}
```

Raw dimensions preserve the original calculated value before any adjustments.
