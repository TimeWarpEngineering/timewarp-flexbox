namespace TimeWarp.Flexbox;

/// <summary>
/// FlexNode partial class containing CSS flexbox style properties.
/// </summary>
public partial class FlexNode
{
  // Backing fields for properties that need dirty tracking
  private FlexDirection StyleFlexDirection = FlexDirection.Row;
  private FlexWrap StyleFlexWrap = FlexWrap.NoWrap;
  private JustifyContent StyleJustifyContent = JustifyContent.FlexStart;
  private AlignItems StyleAlignItems = AlignItems.Stretch;
  private AlignContent StyleAlignContent = AlignContent.FlexStart;
  private AlignSelf StyleAlignSelf = AlignSelf.Auto;
  private float StyleFlexGrow;
  private float StyleFlexShrink = 1f;
  private FlexValue StyleFlexBasis = FlexValue.Auto;
  private FlexValue StyleWidth = FlexValue.Undefined;
  private FlexValue StyleHeight = FlexValue.Undefined;
  private FlexValue StyleMinWidth = FlexValue.Undefined;
  private FlexValue StyleMinHeight = FlexValue.Undefined;
  private FlexValue StyleMaxWidth = FlexValue.Undefined;
  private FlexValue StyleMaxHeight = FlexValue.Undefined;
  private Display StyleDisplay = Display.Flex;
  private PositionType StylePositionType = PositionType.Relative;
  private Overflow StyleOverflow = Overflow.Visible;
  private float? StyleAspectRatio;

  #region Direction & Wrapping

  /// <summary>
  /// Gets or sets the flex direction (main axis orientation).
  /// Default: Row
  /// </summary>
  public FlexDirection FlexDirection
  {
    get => StyleFlexDirection;
    set => SetStyleProperty(ref StyleFlexDirection, value);
  }

  /// <summary>
  /// Gets or sets the flex wrap behavior.
  /// Default: NoWrap
  /// </summary>
  public FlexWrap FlexWrap
  {
    get => StyleFlexWrap;
    set => SetStyleProperty(ref StyleFlexWrap, value);
  }

  #endregion

  #region Alignment

  /// <summary>
  /// Gets or sets how flex items are distributed along the main axis.
  /// Default: FlexStart
  /// </summary>
  public JustifyContent JustifyContent
  {
    get => StyleJustifyContent;
    set => SetStyleProperty(ref StyleJustifyContent, value);
  }

  /// <summary>
  /// Gets or sets the default alignment for items along the cross axis.
  /// Default: Stretch
  /// </summary>
  public AlignItems AlignItems
  {
    get => StyleAlignItems;
    set => SetStyleProperty(ref StyleAlignItems, value);
  }

  /// <summary>
  /// Gets or sets how flex lines are distributed along the cross axis.
  /// Only applies when FlexWrap is enabled.
  /// Default: FlexStart
  /// </summary>
  public AlignContent AlignContent
  {
    get => StyleAlignContent;
    set => SetStyleProperty(ref StyleAlignContent, value);
  }

  /// <summary>
  /// Gets or sets the alignment for this specific item, overriding parent's AlignItems.
  /// Default: Auto (inherit from parent)
  /// </summary>
  public AlignSelf AlignSelf
  {
    get => StyleAlignSelf;
    set => SetStyleProperty(ref StyleAlignSelf, value);
  }

  #endregion

  #region Flex Factors

  /// <summary>
  /// Gets or sets the flex grow factor.
  /// Determines how much the item grows relative to siblings when extra space is available.
  /// Default: 0
  /// </summary>
  public float FlexGrow
  {
    get => StyleFlexGrow;
    set => SetStyleProperty(ref StyleFlexGrow, value);
  }

  /// <summary>
  /// Gets or sets the flex shrink factor.
  /// Determines how much the item shrinks relative to siblings when space is insufficient.
  /// Default: 1
  /// </summary>
  public float FlexShrink
  {
    get => StyleFlexShrink;
    set => SetStyleProperty(ref StyleFlexShrink, value);
  }

  /// <summary>
  /// Gets or sets the flex basis (initial main size before growing/shrinking).
  /// Default: Auto
  /// </summary>
  public FlexValue FlexBasis
  {
    get => StyleFlexBasis;
    set => SetStyleProperty(ref StyleFlexBasis, value);
  }

  #endregion

  #region Dimensions

  /// <summary>
  /// Gets or sets the width of the node.
  /// Default: Undefined
  /// </summary>
  public FlexValue Width
  {
    get => StyleWidth;
    set => SetStyleProperty(ref StyleWidth, value);
  }

  /// <summary>
  /// Gets or sets the height of the node.
  /// Default: Undefined
  /// </summary>
  public FlexValue Height
  {
    get => StyleHeight;
    set => SetStyleProperty(ref StyleHeight, value);
  }

  /// <summary>
  /// Gets or sets the minimum width of the node.
  /// Default: Undefined
  /// </summary>
  public FlexValue MinWidth
  {
    get => StyleMinWidth;
    set => SetStyleProperty(ref StyleMinWidth, value);
  }

  /// <summary>
  /// Gets or sets the minimum height of the node.
  /// Default: Undefined
  /// </summary>
  public FlexValue MinHeight
  {
    get => StyleMinHeight;
    set => SetStyleProperty(ref StyleMinHeight, value);
  }

  /// <summary>
  /// Gets or sets the maximum width of the node.
  /// Default: Undefined
  /// </summary>
  public FlexValue MaxWidth
  {
    get => StyleMaxWidth;
    set => SetStyleProperty(ref StyleMaxWidth, value);
  }

  /// <summary>
  /// Gets or sets the maximum height of the node.
  /// Default: Undefined
  /// </summary>
  public FlexValue MaxHeight
  {
    get => StyleMaxHeight;
    set => SetStyleProperty(ref StyleMaxHeight, value);
  }

  #endregion

  #region Other Layout Properties

  /// <summary>
  /// Gets or sets the display mode.
  /// Default: Flex
  /// </summary>
  public Display Display
  {
    get => StyleDisplay;
    set => SetStyleProperty(ref StyleDisplay, value);
  }

  /// <summary>
  /// Gets or sets the position type.
  /// Default: Relative
  /// </summary>
  public PositionType PositionType
  {
    get => StylePositionType;
    set => SetStyleProperty(ref StylePositionType, value);
  }

  /// <summary>
  /// Gets or sets the overflow behavior.
  /// Default: Visible
  /// </summary>
  public Overflow Overflow
  {
    get => StyleOverflow;
    set => SetStyleProperty(ref StyleOverflow, value);
  }

  /// <summary>
  /// Gets or sets the aspect ratio (width / height).
  /// When set, constrains the node's dimensions to maintain this ratio.
  /// Default: null (no constraint)
  /// </summary>
  public float? AspectRatio
  {
    get => StyleAspectRatio;
    set => SetStyleProperty(ref StyleAspectRatio, value);
  }

  #endregion

  #region Helper Methods

  private void SetStyleProperty<T>(ref T field, T value)
  {
    if (EqualityComparer<T>.Default.Equals(field, value))
      return;

    field = value;
    MarkDirty();
  }

  #endregion
}
