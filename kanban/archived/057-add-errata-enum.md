# Task 057-add-errata-enum

## Summary
Add Errata flags enum to match Yoga's YGErrata. Errata are backward compatibility flags that enable specific behaviors from earlier Yoga versions for migration purposes.

## Todo List
- [ ] Create Errata enum as flags with None, StretchFlexBasis, AbsolutePositionWithoutInsetsExcludesPadding, AbsolutePercentAgainstInnerSize, All, Classic values
- [ ] Add XML documentation for each Errata value
- [ ] Mark enum with [Flags] attribute
- [ ] Replace FlexConfig.UseErrata bool with Errata flags property
- [ ] Add `HasErrata(Errata errata)` helper method to FlexConfig
- [ ] Update layout algorithm to check errata flags where applicable
- [ ] Implement StretchFlexBasis errata behavior
- [ ] Implement AbsolutePositionWithoutInsetsExcludesPadding errata behavior
- [ ] Implement AbsolutePercentAgainstInnerSize errata behavior
- [ ] Add unit tests for each errata flag
- [ ] Document migration path from Yoga 1.x behaviors

## Notes
Yoga reference (YGEnums.h lines 61-69):
```cpp
YG_ENUM_DECL(
    YGErrata,
    YGErrataNone = 0,
    YGErrataStretchFlexBasis = 1,
    YGErrataAbsolutePositionWithoutInsetsExcludesPadding = 2,
    YGErrataAbsolutePercentAgainstInnerSize = 4,
    YGErrataAll = 2147483647,
    YGErrataClassic = 2147483646)
YG_DEFINE_ENUM_FLAG_OPERATORS(YGErrata)
```

Classic = All except StretchFlexBasis, used for Yoga 1.x compatibility.
