# Task 070-add-layout-margins-to-result

## Summary
Add resolved margin values to LayoutResult to match Yoga's LayoutResults margin storage. This allows reading the computed margins after layout.

## Todo List
- [ ] Add MarginLeft, MarginTop, MarginRight, MarginBottom properties to LayoutResult
- [ ] Add internal setters for margin values
- [ ] Update layout algorithm to store resolved margins during layout
- [ ] Add GetMargin(Edge) method for edge-based access
- [ ] Add XML documentation for margin properties
- [ ] Add unit tests for layout margin values
- [ ] Verify margins are correctly resolved including RTL

## Notes
Yoga reference (LayoutResults.h lines 89-94):
```cpp
float margin(PhysicalEdge physicalEdge) const {
  return margin_[yoga::to_underlying(physicalEdge)];
}

void setMargin(PhysicalEdge physicalEdge, float dimension) {
  margin_[yoga::to_underlying(physicalEdge)] = dimension;
}
```

Yoga stores margins in layout:
```cpp
std::array<float, 4> margin_ = {};
```

This allows consumers to read the final computed margins without recalculating.
