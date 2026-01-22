# Task 062-add-flex-value-negation-operator

## Summary
Add negation operator to FlexValue to match Yoga's YGValue operator-. This allows negating FlexValue instances for calculations.

## Todo List
- [ ] Add `public static FlexValue operator -(FlexValue value)` to FlexValue struct
- [ ] Handle negation for Point unit (negate the value)
- [ ] Handle negation for Percent unit (negate the value)
- [ ] Handle negation for Undefined/Auto (return as-is or throw)
- [ ] Add XML documentation for the operator
- [ ] Add unit tests for negation behavior
- [ ] Verify behavior matches Yoga's negation

## Notes
Yoga reference (YGValue.h lines 84-86):
```cpp
inline YGValue operator-(const YGValue& value) {
  return {-value.value, value.unit};
}
```

This is useful for calculations like computing opposite edge positions.
