# Task 075-add-clone-node-callback

## Summary
Add CloneNodeCallback to FlexConfig to match Yoga's clone node functionality. This callback is invoked during layout when a shared node needs to be cloned.

## Todo List
- [ ] Define CloneNodeFunc delegate type
- [ ] Add `CloneNodeCallback` property to FlexConfig
- [ ] Update node cloning logic to use callback when set
- [ ] Implement default cloning behavior when callback is null
- [ ] Add XML documentation for CloneNodeCallback
- [ ] Add unit tests for custom clone callback
- [ ] Document shared node scenarios

## Notes
Yoga reference (YGConfig.h lines 127-156):
```cpp
typedef YGNodeRef (*YGCloneNodeFunc)(
    YGNodeConstRef oldNode,
    YGNodeConstRef owner,
    size_t childIndex);

/**
 * Sets a callback, called during layout, to create a new mutable Yoga node if
 * Yoga must write to it and its owner is not its parent observed during layout.
 */
YG_EXPORT void YGConfigSetCloneNodeFunc(
    YGConfigRef config,
    YGCloneNodeFunc callback);
```

Yoga Node.cpp cloneChildrenIfNeeded:
```cpp
void Node::cloneChildrenIfNeeded() {
  for (Node*& child : children_) {
    if (child->getOwner() != this) {
      child = resolveRef(config_->cloneNode(child, this, i));
      child->setOwner(this);
    }
  }
}
```

This enables copy-on-write semantics for shared node trees.
