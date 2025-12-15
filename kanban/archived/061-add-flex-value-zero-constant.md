# Task 061-add-flex-value-zero-constant

## Summary
Add Zero constant to FlexValue to match Yoga's YGValueZero. This provides a convenient constant for zero-point values.

## Todo List
- [ ] Add `public static readonly FlexValue Zero = FlexValue.Point(0)` to FlexValue struct
- [ ] Add XML documentation for Zero constant
- [ ] Update code that uses `FlexValue.Point(0)` to use `FlexValue.Zero` where appropriate
- [ ] Add unit test verifying Zero constant behavior
- [ ] Verify Zero equals Point(0) in equality comparisons

## Notes
Yoga reference (YGValue.cpp):
```cpp
const YGValue YGValueZero = {0, YGUnitPoint};
const YGValue YGValueUndefined = {YGUndefined, YGUnitUndefined};
const YGValue YGValueAuto = {YGUndefined, YGUnitAuto};
```

TimeWarp already has Undefined and Auto, just missing Zero.
