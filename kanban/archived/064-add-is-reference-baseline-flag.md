# Task 064-add-is-reference-baseline-flag

## Summary
Add IsReferenceBaseline flag to FlexNode to match Yoga's isReferenceBaseline_. This flag marks a node as the reference for baseline alignment among siblings.

## Todo List
- [ ] Add `IsReferenceBaseline` property to FlexNode with default false
- [ ] Add setter that marks dirty when changed (like Yoga)
- [ ] Update baseline alignment calculation to respect reference baseline
- [ ] Add XML documentation explaining the flag's purpose
- [ ] Add unit tests for IsReferenceBaseline behavior
- [ ] Verify baseline alignment matches Yoga when flag is set

## Notes
Yoga reference (YGNode.cpp lines 306-316):
```cpp
void YGNodeSetIsReferenceBaseline(YGNodeRef nodeRef, bool isReferenceBaseline) {
  const auto node = resolveRef(nodeRef);
  if (node->isReferenceBaseline() != isReferenceBaseline) {
    node->setIsReferenceBaseline(isReferenceBaseline);
    node->markDirtyAndPropagate();
  }
}

bool YGNodeIsReferenceBaseline(YGNodeConstRef node) {
  return resolveRef(node)->isReferenceBaseline();
}
```

When multiple siblings use baseline alignment, the reference baseline node determines which baseline to align to.
