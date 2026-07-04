/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/CalculateLayout.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides helper utility functions for the layout algorithm.
/// These are leaf-level functions with minimal dependencies.
/// </summary>
public static class LayoutHelpers
{
  /// <summary>
  /// Applies max-size constraints to sizing mode and dimension.
  /// Modifies both mode and size by reference.
  /// </summary>
  /// <param name="node">The node being measured.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="axis">The flex direction axis.</param>
  /// <param name="ownerAxisSize">The size of the owner along the axis.</param>
  /// <param name="ownerWidth">The owner width for percentage calculations.</param>
  /// <param name="mode">The sizing mode (modified by reference).</param>
  /// <param name="size">The size dimension (modified by reference).</param>
  /// <remarks>
  /// This corresponds to constrainMaxSizeForMode in CalculateLayout.cpp (lines 38-64).
  /// When a max dimension is defined, StretchFit/FitContent sizes are clamped to the
  /// max (including margin), and MaxContent mode becomes FitContent at the max size.
  /// </remarks>
  public static void ConstrainMaxSizeForMode(
      Node node,
      Direction direction,
      FlexDirection axis,
      float ownerAxisSize,
      float ownerWidth,
      ref SizingMode mode,
      ref float size)
  {
    ArgumentNullException.ThrowIfNull(node);

    FloatOptional maxSize =
        node.Style.ResolvedMaxDimension(direction, axis.GetDimension(), ownerAxisSize, ownerWidth) +
        new FloatOptional(node.Style.ComputeMarginForAxis(axis, ownerWidth));

    switch (mode)
    {
      case SizingMode.StretchFit:
      case SizingMode.FitContent:
        size = (maxSize.IsUndefined || size < maxSize.Unwrap())
            ? size
            : maxSize.Unwrap();
        break;
      case SizingMode.MaxContent:
        if (maxSize.IsDefined)
        {
          mode = SizingMode.FitContent;
          size = maxSize.Unwrap();
        }

        break;
      default:
        break;
    }
  }

  /// <summary>
  /// Checks if a dimension is already fixed (no measurement needed).
  /// </summary>
  /// <param name="dim">The dimension value.</param>
  /// <param name="sizingMode">The sizing mode.</param>
  /// <returns>True if the dimension is fixed and doesn't need measurement.</returns>
  /// <remarks>
  /// This corresponds to isFixedSize in CalculateLayout.cpp (lines 421-425).
  /// A dimension is considered fixed when:
  /// 1. The sizing mode is StretchFit (exact size is known), OR
  /// 2. The dimension is defined, the mode is FitContent, and the dimension is zero or negative
  /// </remarks>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsFixedSize(float dim, SizingMode sizingMode)
  {
    return sizingMode == SizingMode.StretchFit ||
           (Comparison.IsDefined(dim) && sizingMode == SizingMode.FitContent && dim <= 0.0f);
  }

  /// <summary>
  /// Computes the available inner dimension respecting min/max constraints.
  /// </summary>
  /// <param name="node">The node being measured.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="dimension">The dimension (Width or Height).</param>
  /// <param name="availableDim">The available dimension.</param>
  /// <param name="paddingAndBorder">The total padding and border for the axis.</param>
  /// <param name="ownerDim">The owner dimension.</param>
  /// <param name="ownerWidth">The owner width for percentage calculations.</param>
  /// <returns>The computed available inner dimension.</returns>
  /// <remarks>
  /// This corresponds to calculateAvailableInnerDimension in CalculateLayout.cpp (lines 501-534).
  /// The algorithm:
  /// 1. If available dimension is undefined, return undefined
  /// 2. Calculate the available inner space (available - padding/border)
  /// 3. Apply min/max constraints
  /// 4. Ensure result is not negative
  /// </remarks>
  public static float CalculateAvailableInnerDimension(
      Node node,
      Direction direction,
      Dimension dimension,
      float availableDim,
      float paddingAndBorder,
      float ownerDim,
      float ownerWidth)
  {
    ArgumentNullException.ThrowIfNull(node);

    // If available dimension is undefined, return undefined
    if (Comparison.IsUndefined(availableDim))
    {
      return float.NaN;
    }

    // Calculate the available inner dimension (subtracting padding and border)
    float availableInnerDim = Comparison.MaxOrDefined(availableDim - paddingAndBorder, 0.0f);

    // Get min and max constraints
    FloatOptional minDim = node.Style.ResolvedMinDimension(direction, dimension, ownerDim, ownerWidth);
    FloatOptional maxDim = node.Style.ResolvedMaxDimension(direction, dimension, ownerDim, ownerWidth);

    // Apply min constraint (ensure we're at least as large as min, minus padding/border)
    if (minDim.IsDefined && minDim.Unwrap() >= 0)
    {
      float minInner = Comparison.MaxOrDefined(minDim.Unwrap() - paddingAndBorder, 0.0f);
      availableInnerDim = Comparison.MaxOrDefined(availableInnerDim, minInner);
    }

    // Apply max constraint (ensure we're no larger than max, minus padding/border)
    if (maxDim.IsDefined && maxDim.Unwrap() >= 0)
    {
      float maxInner = Comparison.MaxOrDefined(maxDim.Unwrap() - paddingAndBorder, 0.0f);
      availableInnerDim = Comparison.MinOrDefined(availableInnerDim, maxInner);
    }

    return availableInnerDim;
  }

  /// <summary>
  /// Resets layout for display:none nodes and their descendants.
  /// </summary>
  /// <param name="node">The node to zero out.</param>
  /// <remarks>
  /// This corresponds to zeroOutLayoutRecursively in CalculateLayout.cpp (lines 471-481).
  /// Nodes with display:none should not participate in layout, so their
  /// layout dimensions are set to zero.
  /// </remarks>
  public static void ZeroOutLayoutRecursively(Node node)
  {
    ArgumentNullException.ThrowIfNull(node);

    // Zero out the layout dimensions
    node.SetLayoutMeasuredDimension(0, Dimension.Width);
    node.SetLayoutMeasuredDimension(0, Dimension.Height);
    node.SetLayoutDimension(0, Dimension.Width);
    node.SetLayoutDimension(0, Dimension.Height);

    // Mark as having new layout
    node.HasNewLayout = true;

    // Recursively zero out children
    foreach (Node child in node.Children)
    {
      ZeroOutLayoutRecursively(child);
    }
  }

  /// <summary>
  /// Handles display:contents nodes by zeroing out their layout while
  /// preserving their children's participation in the parent's layout.
  /// </summary>
  /// <param name="node">The node to process.</param>
  /// <remarks>
  /// This corresponds to cleanupContentsNodesRecursively in CalculateLayout.cpp (lines 483-499).
  /// Nodes with display:contents are "invisible" containers - they don't generate
  /// any boxes themselves, but their children are treated as if they were direct
  /// children of the grandparent node for layout purposes.
  /// </remarks>
  public static void CleanupContentsNodesRecursively(Node node)
  {
    ArgumentNullException.ThrowIfNull(node);

    foreach (Node child in node.Children)
    {
      if (child.Style.Display == Display.Contents)
      {
        // Zero out the contents node's own layout
        child.SetLayoutMeasuredDimension(0, Dimension.Width);
        child.SetLayoutMeasuredDimension(0, Dimension.Height);
        child.SetLayoutDimension(0, Dimension.Width);
        child.SetLayoutDimension(0, Dimension.Height);

        // Mark as having new layout
        child.HasNewLayout = true;

        // Recursively process the contents node's children
        CleanupContentsNodesRecursively(child);
      }
      else
      {
        // For non-contents children, still check their descendants
        CleanupContentsNodesRecursively(child);
      }
    }
  }
}
