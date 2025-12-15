# Task 069-add-flex-shorthand-property

## Summary
Add Flex shorthand property to FlexNode to match Yoga's flex style property. The flex property is a shorthand that sets flex-grow, flex-shrink, and flex-basis together.

## Todo List
- [ ] Add `Flex` property to FlexNode (nullable float)
- [ ] When Flex is set positive: flex-grow=Flex, flex-shrink=1, flex-basis=0 (or auto for web)
- [ ] When Flex is set negative: flex-grow=0, flex-shrink=-Flex, flex-basis=auto
- [ ] When Flex is set to 0: flex-grow=0, flex-shrink=0, flex-basis=0%
- [ ] Update resolveFlexGrow to check Flex property
- [ ] Update resolveFlexShrink to check Flex property
- [ ] Update processFlexBasis to check Flex property
- [ ] Add XML documentation for Flex property
- [ ] Add unit tests for Flex shorthand behavior
- [ ] Verify behavior matches Yoga's flex resolution

## Notes
Yoga reference (Style.h lines 117-122):
```cpp
FloatOptional flex() const {
  return pool_.getNumber(flex_);
}
void setFlex(FloatOptional value) {
  pool_.store(flex_, value);
}
```

Yoga Node.cpp resolveFlexGrow/resolveFlexShrink/processFlexBasis use flex:
```cpp
float Node::resolveFlexGrow() const {
  if (style_.flexGrow().isDefined()) {
    return style_.flexGrow().unwrap();
  }
  if (style_.flex().isDefined() && style_.flex().unwrap() > 0.0f) {
    return style_.flex().unwrap();
  }
  return Style::DefaultFlexGrow;
}
```

The flex shorthand provides CSS-like convenience for common flex patterns.
