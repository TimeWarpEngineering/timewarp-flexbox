# Task 063-add-has-new-layout-flag

## Summary
Add HasNewLayout flag to FlexNode to match Yoga's hasNewLayout_. This flag indicates whether the layout has changed since the last time it was read, enabling optimization in UI frameworks.

## Todo List
- [ ] Add `HasNewLayout` property to FlexNode with default true
- [ ] Add `SetHasNewLayout(bool)` method or make property settable
- [ ] Set HasNewLayout to true when layout is calculated
- [ ] Allow consumers to set HasNewLayout to false after reading layout
- [ ] Add XML documentation explaining the flag's purpose
- [ ] Add unit tests for HasNewLayout behavior
- [ ] Document usage pattern for UI framework integration

## Notes
Yoga reference (YGNode.h lines 87-92):
```cpp
YG_EXPORT bool YGNodeGetHasNewLayout(YGNodeConstRef node);
YG_EXPORT void YGNodeSetHasNewLayout(YGNodeRef node, bool hasNewLayout);
```

Yoga Node.h:
```cpp
bool hasNewLayout_ : 1 = true;

bool getHasNewLayout() const {
  return hasNewLayout_;
}
void setHasNewLayout(bool hasNewLayout) {
  hasNewLayout_ = hasNewLayout;
}
```

This optimization allows UI frameworks to skip re-rendering nodes whose layout hasn't changed.
