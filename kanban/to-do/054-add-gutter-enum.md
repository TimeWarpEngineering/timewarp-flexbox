# Task 054-add-gutter-enum

## Summary
Add Gutter enum to match Yoga's YGGutter for indexed gap access. This provides a more consistent API with Yoga's gap handling using Column, Row, and All gutters.

## Todo List
- [ ] Create Gutter enum with Column, Row, All values
- [ ] Add XML documentation for each Gutter value
- [ ] Add `GetGap(Gutter gutter)` method to FlexNode
- [ ] Add `SetGap(Gutter gutter, float value)` method to FlexNode
- [ ] Update internal gap storage to use Gutter-indexed approach
- [ ] Ensure backward compatibility with existing Gap/RowGap/ColumnGap properties
- [ ] Add unit tests for Gutter-based gap access
- [ ] Verify gap resolution matches Yoga (All fallback for unset gutters)

## Notes
Yoga reference (YGEnums.h lines 82-86):
```cpp
YG_ENUM_DECL(
    YGGutter,
    YGGutterColumn,
    YGGutterRow,
    YGGutterAll)
```

Yoga Style.h gap resolution:
```cpp
Style::Length computeColumnGap() const {
  if (gap_[Gutter::Column].isDefined()) {
    return gap_[Gutter::Column];
  } else {
    return gap_[Gutter::All];  // Fallback to All
  }
}
```

The Gutter enum provides indexed access matching Yoga's API style.
