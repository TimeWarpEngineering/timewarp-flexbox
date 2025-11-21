# Task 009-implement-flexnode-class-structure

## Summary
Implement the FlexNode class tree structure with parent-child relationships and child manipulation methods.

## Todo List
- [ ] Create Nodes/FlexNode.cs class
- [ ] Add Parent property (FlexNode?, internal setter)
- [ ] Add Children property (IReadOnlyList<FlexNode>)
- [ ] Implement AddChild(FlexNode child) method
- [ ] Implement InsertChild(FlexNode child, int index) method
- [ ] Implement RemoveChild(FlexNode child) method
- [ ] Implement RemoveAllChildren() method
- [ ] Add ChildCount property
- [ ] Implement GetChild(int index) method
- [ ] Add IsDirty property and MarkDirty() method for layout invalidation
- [ ] Verify parent-child consistency (child.Parent updated on add/remove)
- [ ] Verify code follows csharp-coding.md standards

## Notes
FlexNode is the central class of the layout engine. This task focuses only on the tree structure:

```csharp
public class FlexNode
{
  // Tree structure
  public FlexNode? Parent { get; internal set; }
  public IReadOnlyList<FlexNode> Children { get; }
  public int ChildCount { get; }
  
  // Child manipulation
  public void AddChild(FlexNode child);
  public void InsertChild(FlexNode child, int index);
  public void RemoveChild(FlexNode child);
  public void RemoveAllChildren();
  public FlexNode GetChild(int index);
  
  // Layout invalidation
  public bool IsDirty { get; private set; }
  public void MarkDirty();
}
```

Important behaviors:
- Adding a child that already has a parent should remove it from old parent first
- Adding a child marks the node dirty
- Children list should be internally mutable but exposed as IReadOnlyList
- Validate index bounds on InsertChild/GetChild

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
