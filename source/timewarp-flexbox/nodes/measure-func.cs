namespace TimeWarp.Flexbox;

/// <summary>
/// Delegate for measuring the intrinsic size of a leaf node's content.
/// Called by the layout algorithm when it needs to determine content size.
/// </summary>
/// <param name="node">The node being measured.</param>
/// <param name="width">The width constraint value.</param>
/// <param name="widthMode">How the width constraint should be interpreted.</param>
/// <param name="height">The height constraint value.</param>
/// <param name="heightMode">How the height constraint should be interpreted.</param>
/// <returns>The measured size of the content.</returns>
/// <remarks>
/// The measure function should return the size that the content would like to be,
/// respecting the constraints provided:
/// - Undefined: Return the natural/intrinsic size
/// - Exactly: Return exactly the specified size
/// - AtMost: Return at most the specified size, but may be smaller
/// </remarks>
public delegate Size MeasureFunc
(
  FlexNode node,
  float width,
  MeasureMode widthMode,
  float height,
  MeasureMode heightMode
);
