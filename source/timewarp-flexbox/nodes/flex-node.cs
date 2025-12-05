namespace TimeWarp.Flexbox;

/// <summary>
/// Represents a node in the flexbox layout tree.
/// This is a partial class - additional properties added in subsequent tasks.
/// </summary>
public partial class FlexNode
{
  private readonly List<FlexNode> ChildrenInternal = [];

  /// <summary>
  /// Creates a new FlexNode instance.
  /// </summary>
  public FlexNode()
  {
    Layout = new LayoutResult();
  }

  #region Layout and Callbacks

  /// <summary>
  /// Gets the computed layout result for this node.
  /// Populated by the layout algorithm.
  /// </summary>
  public LayoutResult Layout { get; }

  /// <summary>
  /// Gets or sets the measurement function for leaf nodes with intrinsic size.
  /// Required for nodes that need to measure their content (e.g., text).
  /// </summary>
  public MeasureFunc? MeasureFunc { get; set; }

  /// <summary>
  /// Gets or sets the baseline function for text baseline alignment.
  /// Optional - used when aligning items by baseline.
  /// </summary>
  public BaselineFunc? BaselineFunc { get; set; }

  /// <summary>
  /// Gets or sets the callback invoked when this node becomes dirty.
  /// The callback is only invoked on the first transition from clean to dirty,
  /// not on subsequent changes while already dirty.
  /// </summary>
  public Action<FlexNode>? DirtiedFunc { get; set; }

  /// <summary>
  /// Gets or sets the configuration for this node.
  /// If null, uses FlexConfig.Default.
  /// </summary>
  public FlexConfig? Config { get; set; }

  /// <summary>
  /// Gets or sets arbitrary user data associated with this node.
  /// </summary>
  public object? Context { get; set; }

  /// <summary>
  /// Gets whether this node has a measure function.
  /// </summary>
  public bool HasMeasureFunc => MeasureFunc is not null;

  /// <summary>
  /// Gets whether this node is a leaf node.
  /// A node is a leaf if it has no children or has a measure function.
  /// </summary>
  public bool IsLeaf => ChildrenInternal.Count == 0 || HasMeasureFunc;

  /// <summary>
  /// Gets the effective configuration for this node.
  /// Returns Config if set, otherwise FlexConfig.Default.
  /// </summary>
  public FlexConfig EffectiveConfig => Config ?? FlexConfig.Default;

  /// <summary>
  /// Calculates the layout for this node and all descendants.
  /// </summary>
  /// <param name="availableWidth">The available width for layout.</param>
  /// <param name="availableHeight">The available height for layout.</param>
  /// <remarks>
  /// This is the entry point for layout calculation.
  /// Full implementation in a later task.
  /// </remarks>
  public void CalculateLayout(float availableWidth, float availableHeight)
  {
    // TODO: Implement layout algorithm in task 019
    // For now, just reset the layout and clear dirty flags
    Layout.Reset();
    Layout.Width = availableWidth;
    Layout.Height = availableHeight;
    ClearDirtyRecursive();
  }

  private void ClearDirtyRecursive()
  {
    ClearDirty();
    foreach (FlexNode child in ChildrenInternal)
    {
      child.ClearDirtyRecursive();
    }
  }

  #endregion

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
  /// Replaces an existing child node with a new child node at the same index position.
  /// If the new child already has a parent, it is first removed from that parent.
  /// </summary>
  /// <param name="oldChild">The existing child node to replace.</param>
  /// <param name="newChild">The new child node to insert in its place.</param>
  /// <returns>True if the old child was found and replaced; otherwise, false.</returns>
  /// <exception cref="ArgumentNullException">Thrown when oldChild or newChild is null.</exception>
  public bool ReplaceChild(FlexNode oldChild, FlexNode newChild)
  {
    ArgumentNullException.ThrowIfNull(oldChild);
    ArgumentNullException.ThrowIfNull(newChild);

    int index = ChildrenInternal.IndexOf(oldChild);
    if (index < 0)
      return false;

    // Remove new child from its existing parent if any
    newChild.Parent?.RemoveChild(newChild);

    // Update parent references
    oldChild.Parent = null;
    newChild.Parent = this;

    // Replace in the list
    ChildrenInternal[index] = newChild;
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
  /// Also marks all ancestor nodes as dirty, invalidates the cache,
  /// and invokes the DirtiedFunc callback if set.
  /// </summary>
  public void MarkDirty()
  {
    if (IsDirty)
      return;

    IsDirty = true;
    InvalidateCache();
    DirtiedFunc?.Invoke(this);
    Parent?.MarkDirty();
  }

  /// <summary>
  /// Clears the dirty flag. Called by the layout algorithm after layout is computed.
  /// </summary>
  internal void ClearDirty()
  {
    IsDirty = false;
  }

  /// <summary>
  /// Creates a deep clone of this node and all its descendants.
  /// The cloned node is detached (has no parent).
  /// </summary>
  /// <returns>A new FlexNode that is a deep copy of this node.</returns>
  /// <remarks>
  /// The clone includes:
  /// - All style properties (Direction, FlexDirection, dimensions, etc.)
  /// - All spacing properties (Margin, Padding, Border, Position, Gap)
  /// - All children (recursively cloned)
  /// - References to MeasureFunc, BaselineFunc, DirtiedFunc, Config, and Context
  /// 
  /// The clone does NOT include:
  /// - Parent reference (cloned node is detached)
  /// - Layout results (the clone needs recalculation)
  /// - Cache entries
  /// </remarks>
  public FlexNode Clone()
  {
    FlexNode clone = new();

    // Copy style properties
    clone.Direction = Direction;
    clone.FlexDirection = FlexDirection;
    clone.FlexWrap = FlexWrap;
    clone.JustifyContent = JustifyContent;
    clone.AlignItems = AlignItems;
    clone.AlignContent = AlignContent;
    clone.AlignSelf = AlignSelf;
    clone.FlexGrow = FlexGrow;
    clone.FlexShrink = FlexShrink;
    clone.FlexBasis = FlexBasis;
    clone.Width = Width;
    clone.Height = Height;
    clone.MinWidth = MinWidth;
    clone.MinHeight = MinHeight;
    clone.MaxWidth = MaxWidth;
    clone.MaxHeight = MaxHeight;
    clone.Display = Display;
    clone.PositionType = PositionType;
    clone.Overflow = Overflow;
    clone.AspectRatio = AspectRatio;
    clone.BoxSizing = BoxSizing;

    // Copy spacing properties (structs are copied by value)
    clone.Margin = Margin;
    clone.Padding = Padding;
    clone.Border = Border;
    clone.Position = Position;
    clone.Gap = Gap;
    clone.RowGap = RowGap;
    clone.ColumnGap = ColumnGap;

    // Copy callbacks and config references
    clone.MeasureFunc = MeasureFunc;
    clone.BaselineFunc = BaselineFunc;
    clone.DirtiedFunc = DirtiedFunc;
    clone.Config = Config;
    clone.Context = Context;

    // Recursively clone children
    foreach (FlexNode child in ChildrenInternal)
    {
      FlexNode childClone = child.Clone();
      clone.AddChild(childClone);
    }

    return clone;
  }
}
