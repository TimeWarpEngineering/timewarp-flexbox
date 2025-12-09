# Task 060-add-physical-edge-enum

## Summary
Add PhysicalEdge enum (Left, Top, Right, Bottom) to match Yoga's internal PhysicalEdge. This distinguishes between logical edges (Start/End) and physical edges for layout result storage.

## Todo List
- [ ] Create PhysicalEdge enum with Left, Top, Right, Bottom values
- [ ] Add XML documentation for each PhysicalEdge value
- [ ] Update LayoutResult to use PhysicalEdge for position/margin/border/padding storage
- [ ] Add helper methods to convert Edge to PhysicalEdge based on direction
- [ ] Update internal layout code to use PhysicalEdge where appropriate
- [ ] Add unit tests for PhysicalEdge usage
- [ ] Ensure RTL direction correctly maps logical to physical edges

## Notes
Yoga reference (yoga/enums/PhysicalEdge.h):
```cpp
enum class PhysicalEdge : uint8_t {
  Left = 0,
  Top = 1,
  Right = 2,
  Bottom = 3,
};
```

Yoga LayoutResults uses PhysicalEdge for indexed storage:
```cpp
float position(PhysicalEdge physicalEdge) const {
  return position_[yoga::to_underlying(physicalEdge)];
}
```

PhysicalEdge is always the actual left/top/right/bottom, regardless of RTL direction.
