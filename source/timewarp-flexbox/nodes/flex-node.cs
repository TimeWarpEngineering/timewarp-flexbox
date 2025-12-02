namespace TimeWarp.Flexbox;

/// <summary>
/// Represents a node in the flexbox layout tree.
/// This is a partial class - additional properties added in subsequent tasks.
/// </summary>
public partial class FlexNode
{
  private readonly List<FlexNode> ChildrenInternal = [];

  /// <summary>
  /// Gets the parent node, or null if this is a root node.
  /// </summary>
  public FlexNode? Parent { get; private set; }

  /// <summary>
  /// Gets the read-only list of child nodes.
  /// </summary>
  public IReadOnlyList<FlexNode> Children => ChildrenInternal;

  /// <summary>
  /// Gets the number of child nodes.
  /// </summary>
  public int ChildCount => ChildrenInternal.Count;

  /// <summary>
  /// Gets whether the node's layout needs to be recalculated.
  /// </summary>
  public bool IsDirty { get; private set; } = true;

  /// <summary>
  /// Adds a child node to the end of the children list.
  /// If the child already has a parent, it is first removed from that parent.
  /// </summary>
  /// <param name="child">The child node to add.</param>
  /// <exception cref="ArgumentNullException">Thrown when child is null.</exception>
  public void AddChild(FlexNode child)
  {
    ArgumentNullException.ThrowIfNull(child);

    // Remove from existing parent if any
    child.Parent?.RemoveChild(child);

    child.Parent = this;
    ChildrenInternal.Add(child);
    MarkDirty();
  }

  /// <summary>
  /// Inserts a child node at the specified index.
  /// If the child already has a parent, it is first removed from that parent.
  /// </summary>
  /// <param name="child">The child node to insert.</param>
  /// <param name="index">The zero-based index at which to insert the child.</param>
  /// <exception cref="ArgumentNullException">Thrown when child is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
  public void InsertChild(FlexNode child, int index)
  {
    ArgumentNullException.ThrowIfNull(child);
    ArgumentOutOfRangeException.ThrowIfNegative(index);
    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, ChildrenInternal.Count);

    // Remove from existing parent if any
    child.Parent?.RemoveChild(child);

    child.Parent = this;
    ChildrenInternal.Insert(index, child);
    MarkDirty();
  }

  /// <summary>
  /// Removes a child node from this node's children.
  /// </summary>
  /// <param name="child">The child node to remove.</param>
  /// <returns>True if the child was found and removed; otherwise, false.</returns>
  public bool RemoveChild(FlexNode child)
  {
    ArgumentNullException.ThrowIfNull(child);

    if (!ChildrenInternal.Remove(child))
      return false;

    child.Parent = null;
    MarkDirty();
    return true;
  }

  /// <summary>
  /// Removes all child nodes from this node.
  /// </summary>
  public void RemoveAllChildren()
  {
    if (ChildrenInternal.Count == 0)
      return;

    foreach (FlexNode child in ChildrenInternal)
    {
      child.Parent = null;
    }

    ChildrenInternal.Clear();
    MarkDirty();
  }

  /// <summary>
  /// Gets the child node at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the child to get.</param>
  /// <returns>The child node at the specified index.</returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
  public FlexNode GetChild(int index)
  {
    ArgumentOutOfRangeException.ThrowIfNegative(index);
    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, ChildrenInternal.Count);

    return ChildrenInternal[index];
  }

  /// <summary>
  /// Marks this node's layout as needing recalculation.
  /// Also marks all ancestor nodes as dirty.
  /// </summary>
  public void MarkDirty()
  {
    if (IsDirty)
      return;

    IsDirty = true;
    Parent?.MarkDirty();
  }

  /// <summary>
  /// Clears the dirty flag. Called by the layout algorithm after layout is computed.
  /// </summary>
  internal void ClearDirty()
  {
    IsDirty = false;
  }
}
