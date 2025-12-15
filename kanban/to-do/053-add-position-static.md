# Task 053-add-position-static

## Summary
**Parent Task** - Add `Static` value to the PositionType enum and implement position:static behavior. In Yoga, this is `YGPositionTypeStatic`. Static positioned elements ignore inset properties (left, top, right, bottom).

This task has been broken into subtasks due to scope (62 tests in Yoga's YGStaticPositionTest.cpp).

## Subtasks
- [ ] 053a - Add Static enum value to PositionType
- [ ] 053b - Implement GetRelativePosition helper in FlexLayoutEngine
- [ ] 053c - Apply relative position offsets in PositionChildren
- [ ] 053d - Port static position tests: core inset behavior (tests 1-12)
- [ ] 053e - Port static position tests: absolute child in static parent (tests 13-24)
- [ ] 053f - Port static position tests: percentage-based positioning (tests 25-36)
- [ ] 053g - Port static position tests: alignment and justification (tests 37-48)
- [ ] 053h - Port static position tests: complex amalgamation (tests 49-62)

## Notes
### Research Completed
Yoga reference (YGEnums.h):
```cpp
YG_ENUM_DECL(
    YGPositionType,
    YGPositionTypeStatic,   // <-- Missing in TimeWarp (index 0)
    YGPositionTypeRelative, // index 1
    YGPositionTypeAbsolute) // index 2
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

### Key Implementation Points
1. `Static` should be first in enum (index 0) to match Yoga
2. `GetRelativePosition()` helper returns 0 for Static, computed inset for Relative
3. `PositionChildren()` applies relative offset for non-static elements
4. position:static is the CSS default where inset properties have no effect

### Files to Modify
- `source/timewarp-flexbox/enums/position-type.cs`
- `source/timewarp-flexbox/layout/flex-layout-engine.cs`
- `test/timewarp-flexbox-tests/layout/static-position-tests.cs` (new)
