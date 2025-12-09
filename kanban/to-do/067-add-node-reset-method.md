# Task 067-add-node-reset-method

## Summary
Add Reset method to FlexNode to match Yoga's YGNodeReset. This resets a node to its default state while keeping the same config.

## Todo List
- [ ] Add `Reset()` method to FlexNode
- [ ] Verify node has no children before reset (throw if it does)
- [ ] Verify node has no parent before reset (throw if it does)
- [ ] Reset all style properties to defaults
- [ ] Reset all spacing properties to defaults
- [ ] Clear layout results
- [ ] Clear cache
- [ ] Clear callbacks (MeasureFunc, BaselineFunc, DirtiedFunc)
- [ ] Keep Config reference
- [ ] Add XML documentation for the method
- [ ] Add unit tests for Reset behavior

## Notes
Yoga reference (YGNode.cpp lines 461-470):
```cpp
void Node::reset() {
  yoga::assertFatalWithNode(
      this,
      children_.empty(),
      "Cannot reset a node which still has children attached");
  yoga::assertFatalWithNode(
      this, owner_ == nullptr, "Cannot reset a node still attached to a owner");

  *this = Node{getConfig()};
}
```

Reset is useful for object pooling scenarios where nodes are reused.
