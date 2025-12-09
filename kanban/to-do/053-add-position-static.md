# Task 053-add-position-static

## Summary
Add `Static` value to the PositionType enum and implement position:static behavior. In Yoga, this is `YGPositionTypeStatic`. Static positioned elements ignore inset properties (left, top, right, bottom).

## Todo List
- [ ] Add `Static` to PositionType enum with XML documentation
- [ ] Update FlexLayoutEngine to skip relative positioning for static elements
- [ ] Update Node.relativePosition equivalent to return 0 for static
- [ ] Ensure static elements don't use position insets
- [ ] Add unit tests for position:static behavior
- [ ] Verify static elements flow normally in flex layout

## Notes
Yoga reference (YGEnums.h lines 123-127):
```cpp
YG_ENUM_DECL(
    YGPositionType,
    YGPositionTypeStatic,   // <-- Missing in TimeWarp
    YGPositionTypeRelative,
    YGPositionTypeAbsolute)
```

Yoga Node.cpp relativePosition():
```cpp
float Node::relativePosition(...) const {
  if (style_.positionType() == PositionType::Static) {
    return 0;  // Static elements ignore insets
  }
  // ... handle relative positioning
}
```

position:static is the CSS default where inset properties have no effect.
