namespace TimeWarp.Flexbox;

/// <summary>
/// Delegate for determining the baseline offset of a node's content.
/// Used for baseline alignment of flex items.
/// </summary>
/// <param name="node">The node whose baseline is being calculated.</param>
/// <param name="width">The computed width of the node.</param>
/// <param name="height">The computed height of the node.</param>
/// <returns>The baseline offset from the top of the node.</returns>
/// <remarks>
/// The baseline is typically the line upon which text sits.
/// For text content, this is usually the bottom of most lowercase letters.
/// Return the distance from the top of the node to the baseline.
/// </remarks>
public delegate float BaselineFunc
(
  FlexNode node,
  float width,
  float height
);
