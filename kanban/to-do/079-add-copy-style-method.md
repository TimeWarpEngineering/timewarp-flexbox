# Task 079-add-copy-style-method

## Summary
Add CopyStyle method to FlexNode to match Yoga's YGNodeCopyStyle. This copies all style properties from one node to another.

## Todo List
- [ ] Add `CopyStyleFrom(FlexNode source)` method to FlexNode
- [ ] Copy all style properties (Direction, FlexDirection, etc.)
- [ ] Copy all spacing properties (Margin, Padding, Border, Position, Gap)
- [ ] Mark destination node as dirty if any property changed
- [ ] Optimize to only mark dirty if styles actually differ
- [ ] Add XML documentation for CopyStyleFrom
- [ ] Add unit tests for CopyStyleFrom behavior
- [ ] Verify style equality comparison works correctly

## Notes
Yoga reference (YGNodeStyle.cpp lines 37-45):
```cpp
void YGNodeCopyStyle(YGNodeRef dstNode, YGNodeConstRef srcNode) {
  auto dst = resolveRef(dstNode);
  auto src = resolveRef(srcNode);

  if (dst->style() != src->style()) {
    dst->setStyle(src->style());
    dst->markDirtyAndPropagate();
  }
}
```

This is useful for applying style templates to multiple nodes.
