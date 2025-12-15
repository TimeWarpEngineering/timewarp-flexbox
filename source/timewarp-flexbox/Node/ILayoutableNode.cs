/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/node/LayoutableChildren.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Interface defining the minimal contract required for a node to participate
/// in layoutable children iteration.
/// </summary>
/// <remarks>
/// This interface is used by <see cref="LayoutableChildren{T}"/> to traverse
/// the node tree while handling display:contents nodes specially.
/// The full Node implementation (Task 122) will implement this interface.
/// </remarks>
public interface ILayoutableNode
{
    /// <summary>
    /// Gets the child at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the child.</param>
    /// <returns>The child node at the specified index.</returns>
    ILayoutableNode GetChild(int index);

    /// <summary>
    /// Gets the number of children this node has.
    /// </summary>
    /// <returns>The count of child nodes.</returns>
    int GetChildCount();

    /// <summary>
    /// Gets the display style property for this node.
    /// </summary>
    /// <returns>The Display enum value.</returns>
    Display GetDisplay();
}
