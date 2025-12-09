# Task 055-add-dimension-enum

## Summary
Add Dimension enum (Width, Height) to match Yoga's YGDimension. This enables indexed dimension access and aligns with Yoga's API for dimension-related operations.

## Todo List
- [ ] Create Dimension enum with Width, Height values
- [ ] Add XML documentation for each Dimension value
- [ ] Add `GetDimension(Dimension dim)` method to FlexNode for width/height
- [ ] Add `SetDimension(Dimension dim, FlexValue value)` method to FlexNode
- [ ] Add `GetMinDimension(Dimension dim)` method to FlexNode
- [ ] Add `SetMinDimension(Dimension dim, FlexValue value)` method to FlexNode
- [ ] Add `GetMaxDimension(Dimension dim)` method to FlexNode
- [ ] Add `SetMaxDimension(Dimension dim, FlexValue value)` method to FlexNode
- [ ] Update LayoutResult to use Dimension for indexed access
- [ ] Add unit tests for Dimension-based access
- [ ] Ensure backward compatibility with Width/Height properties

## Notes
Yoga reference (YGEnums.h lines 32-35):
```cpp
YG_ENUM_DECL(
    YGDimension,
    YGDimensionWidth,
    YGDimensionHeight)
```

Yoga uses this for indexed access:
```cpp
Style::SizeLength dimension(Dimension axis) const {
  return dimensions_[to_underlying(axis)];
}
```

This enables generic code that works with either dimension.
