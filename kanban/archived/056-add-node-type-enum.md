# Task 056-add-node-type-enum

## Summary
Add NodeType enum (Default, Text) to match Yoga's YGNodeType. This distinguishes between regular nodes and text nodes, affecting layout rounding behavior.

## Todo List
- [ ] Create NodeType enum with Default, Text values
- [ ] Add XML documentation for each NodeType value
- [ ] Add `NodeType` property to FlexNode with default value Default
- [ ] Update FlexNode.MeasureFunc setter to set NodeType to Text when function is set
- [ ] Update FlexNode.MeasureFunc setter to set NodeType to Default when function is cleared
- [ ] Update pixel grid rounding to handle Text nodes differently (allow truncation)
- [ ] Add unit tests for NodeType behavior
- [ ] Verify rounding behavior matches Yoga for Text vs Default nodes

## Notes
Yoga reference (YGEnums.h lines 112-115):
```cpp
YG_ENUM_DECL(
    YGNodeType,
    YGNodeTypeDefault,
    YGNodeTypeText)
```

Yoga Node.cpp setMeasureFunc():
```cpp
void Node::setMeasureFunc(YGMeasureFunc measureFunc) {
  if (measureFunc == nullptr) {
    setNodeType(NodeType::Default);
  } else {
    setNodeType(NodeType::Text);
  }
  measureFunc_ = measureFunc;
}
```

Text nodes may have their layout results truncated during rounding, while Default nodes should not.
