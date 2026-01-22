# Task 053c-apply-relative-position-in-layout

## Summary
Modify `PositionChildren()` (or equivalent) in FlexLayoutEngine to apply relative position offsets using the new `GetRelativePosition()` helper. This enables the actual position:static behavior.

**Parent Task:** 053-add-position-static

## Todo List
- [ ] Find where child positions are finalized in layout algorithm
- [ ] Call GetRelativePosition for main axis and apply offset
- [ ] Call GetRelativePosition for cross axis and apply offset
- [ ] Ensure Static children have no offset applied
- [ ] Ensure Relative children get their inset offsets
- [ ] Add integration test: Static child ignores left/top insets
- [ ] Add integration test: Relative child uses left/top insets

## Notes
### Yoga Reference Pattern
In Yoga's CalculateLayout.cpp, relative positioning is applied after flex layout:
```cpp
child.setLayoutPosition(
    child.getLayout().position(flexStartEdge(mainAxis)) + 
    child.relativePosition(mainAxis, direction, availableInnerMainDim),
    flexStartEdge(mainAxis));
```

### Expected Behavior
```
Static node with Left=10:  final left position = normal flow position (ignores 10)
Relative node with Left=10: final left position = normal flow position + 10
```

### Test Cases to Add
1. Static node with insets - verify insets ignored
2. Relative node with insets - verify offsets applied
3. Mixed Static/Relative siblings - verify correct individual behavior
