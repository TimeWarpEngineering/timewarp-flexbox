# Task 053b-implement-get-relative-position

## Summary
Implement `GetRelativePosition()` helper method in FlexLayoutEngine that returns 0 for Static positioned elements and the computed inset offset for Relative positioned elements.

**Parent Task:** 053-add-position-static

## Todo List
- [ ] Add `GetRelativePosition(FlexNode node, FlexDirection axis, float axisSize)` helper
- [ ] Return 0 immediately if node.PositionType is Static
- [ ] For Relative, compute position using leading/trailing insets
- [ ] Handle percentage-based insets correctly
- [ ] Add unit tests for GetRelativePosition with Static, Relative, and Absolute nodes

## Notes
### Yoga Reference (Node.cpp)
```cpp
float Node::relativePosition(
    FlexDirection axis,
    Direction direction,
    float axisSize) const {
  if (style_.positionType() == PositionType::Static) {
    return 0;
  }
  if (isInlineStartPositionDefined(axis, direction)) {
    return getInlineStartPosition(axis, direction, axisSize);
  }
  return -getInlineEndPosition(axis, direction, axisSize);
}
```

### Implementation Location
Add to `source/timewarp-flexbox/layout/flex-layout-engine.cs`

### Key Points
- Static returns 0 (ignores all insets)
- Relative uses leading inset if defined, otherwise negated trailing inset
- Absolute positioning is handled separately (not by this method)
