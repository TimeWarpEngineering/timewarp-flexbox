namespace TimeWarp.Flexbox;

/// <summary>
/// Represents a single line of flex items during layout calculation.
/// Used when flex-wrap is enabled to track items that flow onto each line.
/// </summary>
public sealed class FlexLine
{
  private readonly List<FlexNode> ItemsInternal = [];

  /// <summary>
  /// Gets the read-only list of flex nodes in this line.
  /// </summary>
  public IReadOnlyList<FlexNode> Items => ItemsInternal;

  /// <summary>
  /// Gets or sets the total size of items along the main axis.
  /// </summary>
  public float MainSize { get; set; }

  /// <summary>
  /// Gets or sets the size of the line along the cross axis.
  /// This is typically the maximum cross size of all items in the line.
  /// </summary>
  public float CrossSize { get; set; }

  /// <summary>
  /// Gets or sets the remaining free space on the main axis after items are placed.
  /// Positive value means extra space; negative means overflow.
  /// </summary>
  public float RemainingFreeSpace { get; set; }

  /// <summary>
  /// Gets or sets the sum of flex-grow factors for all items in this line.
  /// </summary>
  public float TotalFlexGrow { get; set; }

  /// <summary>
  /// Gets or sets the sum of flex-shrink factors for all items in this line.
  /// </summary>
  public float TotalFlexShrink { get; set; }

  /// <summary>
  /// Gets or sets the sum of (flex-shrink * flex-basis) for all items.
  /// Used for proportional shrinking calculation.
  /// </summary>
  public float TotalWeightedFlexShrink { get; set; }

  /// <summary>
  /// Gets the number of items in this line.
  /// </summary>
  public int ItemCount => Items.Count;

  /// <summary>
  /// Adds an item to this line.
  /// </summary>
  /// <param name="item">The flex node to add.</param>
  /// <exception cref="ArgumentNullException">Thrown when item is null.</exception>
  public void AddItem(FlexNode item)
  {
    ArgumentNullException.ThrowIfNull(item);
    ItemsInternal.Add(item);
  }

  /// <summary>
  /// Resets all line properties to their default values.
  /// </summary>
  public void Reset()
  {
    ItemsInternal.Clear();
    MainSize = 0;
    CrossSize = 0;
    RemainingFreeSpace = 0;
    TotalFlexGrow = 0;
    TotalFlexShrink = 0;
    TotalWeightedFlexShrink = 0;
  }
}
