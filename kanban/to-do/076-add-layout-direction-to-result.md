# Task 076-add-layout-direction-to-result

## Summary
Fix LayoutResult.Direction to store the resolved Direction (LTR/RTL) instead of FlexDirection. The layout direction determines how Start/End edges map to Left/Right.

## Todo List
- [ ] Change LayoutResult.Direction property type from FlexDirection to Direction
- [ ] Rename property if needed to avoid confusion (e.g., LayoutDirection)
- [ ] Update layout algorithm to store resolved direction
- [ ] Update places that read Direction from layout
- [ ] Add XML documentation clarifying this is the resolved LTR/RTL direction
- [ ] Add unit tests for layout direction
- [ ] Verify RTL layouts store correct direction

## Notes
Yoga reference (LayoutResults.h lines 41-47):
```cpp
Direction direction() const {
  return direction_;
}

void setDirection(Direction direction) {
  direction_ = direction;
}
```

The layout direction is Direction (LTR/RTL/Inherit), not FlexDirection (Row/Column/etc.).
This is used to resolve logical edges (Start/End) to physical edges (Left/Right).
