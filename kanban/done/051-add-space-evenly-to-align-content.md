# Task 051-add-space-evenly-to-align-content

## Summary
Add missing `SpaceEvenly` value to the AlignContent enum. This is present in Yoga's YGAlign enum and is a valid CSS align-content value.

## Todo List
- [ ] Add `SpaceEvenly` to AlignContent enum with XML documentation
- [ ] Update FlexLayoutEngine.CalculateAlignContentOffset() to handle SpaceEvenly
- [ ] Update FlexLayoutEngine.CalculateAlignContentSpacing() to handle SpaceEvenly
- [ ] Add unit tests for align-content: space-evenly behavior
- [ ] Verify layout matches Yoga for space-evenly alignment

## Notes
Yoga reference (YGEnums.h lines 15-25):
```cpp
YG_ENUM_DECL(
    YGAlign,
    YGAlignAuto,
    YGAlignFlexStart,
    YGAlignCenter,
    YGAlignFlexEnd,
    YGAlignStretch,
    YGAlignBaseline,
    YGAlignSpaceBetween,
    YGAlignSpaceAround,
    YGAlignSpaceEvenly)  // <-- Missing in TimeWarp
```

SpaceEvenly distributes lines with equal space between and around them (same gap everywhere).
