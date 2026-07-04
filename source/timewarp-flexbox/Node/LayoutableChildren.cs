/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/node/LayoutableChildren.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides iteration over children that participate in layout, handling
/// display:contents nodes specially by traversing into them and returning
/// their children instead.
/// </summary>
/// <typeparam name="T">The node type that implements <see cref="ILayoutableNode"/>.</typeparam>
/// <remarks>
/// <para>
/// This iterator is used during layout calculations to get the effective
/// children of a node. When a child has <c>display: contents</c>, that child
/// is not included in the iteration, but its children are included as if
/// they were direct children of the parent.
/// </para>
/// <para>
/// Note: This iterator does NOT filter out <c>display: none</c> nodes.
/// That filtering is done separately in the layout algorithm.
/// </para>
/// <para>
/// Design Decision: Uses <c>IEnumerable&lt;T&gt;</c> pattern for C# idiomatic
/// iteration. The enumerator maintains a backtrack stack to handle nested
/// <c>display: contents</c> nodes, similar to the C++ implementation's
/// <c>std::forward_list</c>.
/// </para>
/// </remarks>
public readonly struct LayoutableChildren<T> : IEnumerable<T>, IEquatable<LayoutableChildren<T>>
    where T : class, ILayoutableNode
{
  private readonly T? Parent;

  /// <summary>
  /// Creates a new <see cref="LayoutableChildren{T}"/> for the specified parent node.
  /// </summary>
  /// <param name="parent">The parent node whose layoutable children to iterate.</param>
  public LayoutableChildren(T? parent)
  {
    Parent = parent;
  }

  /// <summary>
  /// Returns an enumerator that iterates through the layoutable children.
  /// </summary>
  /// <returns>An enumerator for the layoutable children.</returns>
  public Enumerator GetEnumerator() => new(Parent);

  /// <inheritdoc />
  IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

  /// <inheritdoc />
  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

  #region Equality

  /// <inheritdoc />
  public bool Equals(LayoutableChildren<T> other) => ReferenceEquals(Parent, other.Parent);

  /// <inheritdoc />
  public override bool Equals(object? obj) => obj is LayoutableChildren<T> other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode() => Parent?.GetHashCode() ?? 0;

  /// <summary>
  /// Equality operator.
  /// </summary>
  public static bool operator ==(LayoutableChildren<T> left, LayoutableChildren<T> right) => left.Equals(right);

  /// <summary>
  /// Inequality operator.
  /// </summary>
  public static bool operator !=(LayoutableChildren<T> left, LayoutableChildren<T> right) => !left.Equals(right);

  #endregion

  /// <summary>
  /// Enumerator for layoutable children that handles display:contents traversal.
  /// </summary>
  /// <remarks>
  /// The enumerator skips over display:contents nodes by descending into them
  /// and returning their children instead. It maintains a backtrack stack to
  /// handle nested display:contents nodes and to continue iteration after
  /// exhausting a display:contents node's children.
  /// </remarks>
  public struct Enumerator : IEnumerator<T>
  {
    private T? Node;
    private int ChildIndex;
    private Stack<(T Node, int ChildIndex)>? Backtrack;
    private T? CurrentInternal;
    private bool Started;

    /// <summary>
    /// Creates a new enumerator for the specified parent node.
    /// </summary>
    /// <param name="parent">The parent node to enumerate children from.</param>
    internal Enumerator(T? parent)
    {
      Node = parent;
      ChildIndex = 0;
      Backtrack = null;
      CurrentInternal = null;
      Started = false;
    }

    /// <inheritdoc />
    public readonly T Current => CurrentInternal!;

    /// <inheritdoc />
    readonly object System.Collections.IEnumerator.Current => Current;

    /// <inheritdoc />
    public bool MoveNext()
    {
      if (Node is null || Node.GetChildCount() == 0)
      {
        CurrentInternal = null;
        return false;
      }

      if (!Started)
      {
        // First call - position at first child
        Started = true;
        ChildIndex = 0;

        // Skip display:contents nodes at position 0
        T firstChild = (T)Node.GetChild(0);
        if (firstChild.GetDisplay() == Display.Contents)
        {
          SkipContentsNodes();
        }

        // If we exhausted all nodes during skip, return false
        if (Node is null)
        {
          CurrentInternal = null;
          return false;
        }

        CurrentInternal = (T)Node.GetChild(ChildIndex);
        return true;
      }

      // Subsequent calls - advance to next child
      Next();

      if (Node is null)
      {
        CurrentInternal = null;
        return false;
      }

      CurrentInternal = (T)Node.GetChild(ChildIndex);
      return true;
    }

    /// <summary>
    /// Advances to the next layoutable child.
    /// </summary>
    private void Next()
    {
      if (ChildIndex + 1 >= Node!.GetChildCount())
      {
        // Current node has no more children, try to backtrack
        if (Backtrack is null || Backtrack.Count == 0)
        {
          // No nodes to backtrack to, iteration complete
          Node = null;
          return;
        }

        // Pop and restore the latest backtrack entry
        (T backNode, int backIndex) = Backtrack.Pop();
        Node = backNode;
        ChildIndex = backIndex;

        // Recursively advance from the restored position
        Next();
      }
      else
      {
        // Move to next child
        ChildIndex++;

        // Skip display:contents nodes
        T child = (T)Node.GetChild(ChildIndex);
        if (child.GetDisplay() == Display.Contents)
        {
          SkipContentsNodes();
        }
      }
    }

    /// <summary>
    /// Skips over display:contents nodes by descending into them.
    /// </summary>
    /// <remarks>
    /// When encountering a display:contents node, this method:
    /// 1. Pushes the current position onto the backtrack stack
    /// 2. Descends into the display:contents node
    /// 3. Repeats until finding a non-contents node or reaching a leaf
    /// 4. If only contents nodes found (no non-contents children), backtracks
    /// </remarks>
    private void SkipContentsNodes()
    {
      T currentNode = (T)Node!.GetChild(ChildIndex);

      while (currentNode.GetDisplay() == Display.Contents &&
             currentNode.GetChildCount() > 0)
      {
        // Push current state for backtracking
        Backtrack ??= new Stack<(T, int)>();
        Backtrack.Push((Node!, ChildIndex));

        // Descend into the contents node
        Node = currentNode;
        ChildIndex = 0;

        // Get the first child of the contents node
        currentNode = (T)currentNode.GetChild(0);
      }

      // If we ended on a display:contents node with no children,
      // we need to backtrack and continue
      if (currentNode.GetDisplay() == Display.Contents)
      {
        Next();
      }
    }

    /// <inheritdoc />
    public void Reset()
    {
      throw new NotSupportedException("Reset is not supported for LayoutableChildren enumerator.");
    }

    /// <inheritdoc />
    public readonly void Dispose()
    {
      // No unmanaged resources to dispose
    }
  }
}
