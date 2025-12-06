# Task 050-add-intrinsic-sizing-units

## Summary
Add missing CSS intrinsic sizing units to the Unit enum: MaxContent, FitContent, and Stretch. These are present in Yoga's YGUnit enum and are required for complete CSS sizing support.

## Todo List
- [x] Add `MaxContent` to Unit enum with XML documentation
- [x] Add `FitContent` to Unit enum with XML documentation
- [x] Add `Stretch` to Unit enum with XML documentation
- [x] Update FlexValue to handle new units in `IsDefined` property
- [x] Update FlexValue.ToString() to format new units
- [x] Add factory methods: `FlexValue.MaxContent()`, `FlexValue.FitContent()`, `FlexValue.Stretch()`
- [x] Update ValueResolver to handle new units during layout
- [x] Add unit tests for new unit types
- [ ] Verify layout behavior matches Yoga for intrinsic sizing

## Notes
Yoga reference (YGEnums.h lines 130-137):
```cpp
YG_ENUM_DECL(
    YGUnit,
    YGUnitUndefined,
    YGUnitPoint,
    YGUnitPercent,
    YGUnitAuto,
    YGUnitMaxContent,
    YGUnitFitContent,
    YGUnitStretch)
```

These units are used for CSS intrinsic sizing:
- **MaxContent**: Size to fit content without wrapping
- **FitContent**: Size to fit content, but not exceed available space
- **Stretch**: Stretch to fill available space (for flex items)
