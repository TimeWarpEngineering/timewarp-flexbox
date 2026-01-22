# Task 066-add-set-children-method

## Summary
Add SetChildren method to FlexNode to match Yoga's YGNodeSetChildren. This allows bulk setting of children in a single operation, which is more efficient than individual add/remove calls.

## Todo List
- [ ] Add `SetChildren(IEnumerable<FlexNode> children)` method to FlexNode
- [ ] Clear existing children and set their Parent to null
- [ ] Set new children and update their Parent references
- [ ] Handle shared children (children that exist in both old and new sets)
- [ ] Mark node as dirty after children change
- [ ] Add XML documentation for the method
- [ ] Add unit tests for SetChildren behavior
- [ ] Verify behavior matches Yoga's YGNodeSetChildren

## Notes
Yoga reference (YGNode.cpp lines 212-251):
```cpp
void YGNodeSetChildren(
    const YGNodeRef ownerRef,
    const YGNodeRef* childrenRefs,
    const size_t count) {
  auto owner = resolveRef(ownerRef);
  // ...
  if (childrenVector.empty()) {
    // Clear all children
  } else {
    // Set new children, handling shared nodes
    for (auto* oldChild : owner->getChildren()) {
      if (std::find(...) == childrenVector.end()) {
        oldChild->setLayout({});
        oldChild->setOwner(nullptr);
      }
    }
    owner->setChildren(childrenVector);
    for (yoga::Node* child : childrenVector) {
      child->setOwner(owner);
    }
  }
  owner->markDirtyAndPropagate();
}
```

This is more efficient than multiple add/remove operations when replacing all children.
