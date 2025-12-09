# Task 052-add-display-contents

## Summary
Add `Contents` value to the Display enum and implement display:contents behavior. In Yoga, this is `YGDisplayContents`. When a node has display:contents, it doesn't generate a box but its children are laid out as if they were children of the node's parent.

## Todo List
- [ ] Add `Contents` to Display enum with XML documentation
- [ ] Add `contentsChildrenCount` tracking to FlexNode (like Yoga's `contentsChildrenCount_`)
- [ ] Update child insertion to track contents children count
- [ ] Update child removal to track contents children count
- [ ] Update ReplaceChild to track contents children count
- [ ] Implement `GetLayoutChildren()` that flattens contents nodes
- [ ] Update FlexLayoutEngine to use layout children instead of direct children
- [ ] Add `HasContentsChildren` property to FlexNode
- [ ] Add unit tests for display:contents behavior
- [ ] Test nested display:contents scenarios

## Notes
Yoga reference (YGEnums.h lines 43-47):
```cpp
YG_ENUM_DECL(
    YGDisplay,
    YGDisplayFlex,
    YGDisplayNone,
    YGDisplayContents)  // <-- Missing in TimeWarp
```

Yoga Node.cpp shows how contents children are tracked:
```cpp
void Node::insertChild(Node* child, size_t index) {
  if (child->style().display() == Display::Contents) {
    contentsChildrenCount_++;
  }
  children_.insert(...);
}
```

display:contents makes an element's children behave as direct children of the element's parent, effectively "unwrapping" the element.
