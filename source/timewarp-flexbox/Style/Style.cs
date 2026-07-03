/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/style/Style.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Style represents all the styling properties for a flex node.
/// This is a direct port of C++ yoga/style/Style.h.
/// </summary>
/// <remarks>
/// Design Decision: In C++, Style uses bit-fields for enum storage to minimize memory.
/// In C#, we use regular properties since bit-field optimization would require unsafe code.
/// The StyleValuePool is used for efficient storage of length/number values.
/// </remarks>
public sealed class Style : IEquatable<Style>
{
  /// <summary>Default flex-grow value (0.0f).</summary>
  public const float DefaultFlexGrow = 0.0f;

  /// <summary>Default flex-shrink value (0.0f).</summary>
  public const float DefaultFlexShrink = 0.0f;

  /// <summary>Web-compatible default flex-shrink value (1.0f).</summary>
  public const float WebDefaultFlexShrink = 1.0f;

  #region Private Fields

  // StyleValueHandle fields for pooled values
  private StyleValueHandle FlexHandle;
  private StyleValueHandle FlexGrowHandle;
  private StyleValueHandle FlexShrinkHandle;
  private StyleValueHandle FlexBasisHandle = StyleValueHandle.Auto;

  // Edge arrays (margin, position, padding, border)
  private readonly StyleValueHandle[] MarginHandles = new StyleValueHandle[YogaEnums.OrdinalCount<Edge>()];
  private readonly StyleValueHandle[] PositionHandles = new StyleValueHandle[YogaEnums.OrdinalCount<Edge>()];
  private readonly StyleValueHandle[] PaddingHandles = new StyleValueHandle[YogaEnums.OrdinalCount<Edge>()];
  private readonly StyleValueHandle[] BorderHandles = new StyleValueHandle[YogaEnums.OrdinalCount<Edge>()];

  // Gutter array (gap)
  private readonly StyleValueHandle[] GapHandles = new StyleValueHandle[YogaEnums.OrdinalCount<Gutter>()];

  // Dimension arrays
  private readonly StyleValueHandle[] DimensionHandles =
  [
      StyleValueHandle.Auto, // Width
        StyleValueHandle.Auto  // Height
  ];
  private readonly StyleValueHandle[] MinDimensionHandles = new StyleValueHandle[YogaEnums.OrdinalCount<Dimension>()];
  private readonly StyleValueHandle[] MaxDimensionHandles = new StyleValueHandle[YogaEnums.OrdinalCount<Dimension>()];

  // Aspect ratio
  private StyleValueHandle AspectRatioHandle;

  // The value pool for storing non-inlinable values
  private readonly StyleValuePool Pool = new();

  // The node owning this style, marked dirty when a style value changes.
  // In C++ Yoga, mutation goes through YGNodeStyleSet* -> updateStyle, which
  // compares the new value to the old one and calls markDirtyAndPropagate
  // only on change. The C# port exposes Style directly, so the same
  // compare-and-dirty behavior lives in the setters themselves.
  internal Node? OwnerNode;

  #endregion

  #region Enum Properties

  /// <summary>Gets or sets the text direction.</summary>
  public Direction Direction
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = Direction.Inherit;

  /// <summary>Gets or sets the flex direction.</summary>
  public FlexDirection FlexDirection
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = FlexDirection.Column;

  /// <summary>Gets or sets how content is justified along the main axis.</summary>
  public Justify JustifyContent
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = Justify.FlexStart;

  /// <summary>Gets or sets how lines are aligned along the cross axis.</summary>
  public Align AlignContent
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = Align.FlexStart;

  /// <summary>Gets or sets how items are aligned along the cross axis.</summary>
  public Align AlignItems
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = Align.Stretch;

  /// <summary>Gets or sets how this item is aligned (overrides parent's align-items).</summary>
  public Align AlignSelf
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = Align.Auto;

  /// <summary>Gets or sets the position type (static, relative, absolute).</summary>
  public PositionType PositionType
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = PositionType.Relative;

  /// <summary>Gets or sets the flex wrap behavior.</summary>
  public Wrap FlexWrap
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = Wrap.NoWrap;

  /// <summary>Gets or sets the overflow behavior.</summary>
  public Overflow Overflow
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = Overflow.Visible;

  /// <summary>Gets or sets the display type.</summary>
  public Display Display
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = Display.Flex;

  /// <summary>Gets or sets the box-sizing mode.</summary>
  public BoxSizing BoxSizing
  {
    get;
    set
    {
      if (field != value)
      {
        field = value;
        OwnerNode?.MarkDirtyAndPropagate();
      }
    }
  } = BoxSizing.BorderBox;

  #endregion

  #region Flex Properties

  /// <summary>Gets or sets the flex shorthand value.</summary>
  public FloatOptional Flex
  {
    get => Pool.GetNumber(FlexHandle);
    set => StoreNumber(ref FlexHandle, value);
  }

  /// <summary>Gets or sets the flex-grow value.</summary>
  public FloatOptional FlexGrow
  {
    get => Pool.GetNumber(FlexGrowHandle);
    set => StoreNumber(ref FlexGrowHandle, value);
  }

  /// <summary>Gets or sets the flex-shrink value.</summary>
  public FloatOptional FlexShrink
  {
    get => Pool.GetNumber(FlexShrinkHandle);
    set => StoreNumber(ref FlexShrinkHandle, value);
  }

  /// <summary>Gets or sets the flex-basis value.</summary>
  public StyleSizeLength FlexBasis
  {
    get => Pool.GetSize(FlexBasisHandle);
    set => StoreSize(ref FlexBasisHandle, value);
  }

  #endregion

  #region Edge Properties (Margin, Position, Padding, Border)

  /// <summary>Gets the margin for the specified edge.</summary>
  public StyleLength GetMargin(Edge edge) => Pool.GetLength(MarginHandles[YogaEnums.ToUnderlying(edge)]);

  /// <summary>Sets the margin for the specified edge.</summary>
  public void SetMargin(Edge edge, StyleLength value) => StoreLength(ref MarginHandles[YogaEnums.ToUnderlying(edge)], value);

  /// <summary>Gets the position for the specified edge.</summary>
  public StyleLength GetPosition(Edge edge) => Pool.GetLength(PositionHandles[YogaEnums.ToUnderlying(edge)]);

  /// <summary>Sets the position for the specified edge.</summary>
  public void SetPosition(Edge edge, StyleLength value) => StoreLength(ref PositionHandles[YogaEnums.ToUnderlying(edge)], value);

  /// <summary>Gets the padding for the specified edge.</summary>
  public StyleLength GetPadding(Edge edge) => Pool.GetLength(PaddingHandles[YogaEnums.ToUnderlying(edge)]);

  /// <summary>Sets the padding for the specified edge.</summary>
  public void SetPadding(Edge edge, StyleLength value) => StoreLength(ref PaddingHandles[YogaEnums.ToUnderlying(edge)], value);

  /// <summary>Gets the border for the specified edge.</summary>
  public StyleLength GetBorder(Edge edge) => Pool.GetLength(BorderHandles[YogaEnums.ToUnderlying(edge)]);

  /// <summary>Sets the border for the specified edge.</summary>
  public void SetBorder(Edge edge, StyleLength value) => StoreLength(ref BorderHandles[YogaEnums.ToUnderlying(edge)], value);

  /// <summary>Gets the gap for the specified gutter.</summary>
  public StyleLength GetGap(Gutter gutter) => Pool.GetLength(GapHandles[YogaEnums.ToUnderlying(gutter)]);

  /// <summary>Sets the gap for the specified gutter.</summary>
  public void SetGap(Gutter gutter, StyleLength value) => StoreLength(ref GapHandles[YogaEnums.ToUnderlying(gutter)], value);

  #endregion

  #region Dimension Properties

  /// <summary>Gets the dimension (width or height) for the specified axis.</summary>
  public StyleSizeLength GetDimension(Dimension axis) => Pool.GetSize(DimensionHandles[YogaEnums.ToUnderlying(axis)]);

  /// <summary>Sets the dimension (width or height) for the specified axis.</summary>
  public void SetDimension(Dimension axis, StyleSizeLength value) => StoreSize(ref DimensionHandles[YogaEnums.ToUnderlying(axis)], value);

  /// <summary>Gets the minimum dimension for the specified axis.</summary>
  public StyleSizeLength GetMinDimension(Dimension axis) => Pool.GetSize(MinDimensionHandles[YogaEnums.ToUnderlying(axis)]);

  /// <summary>Sets the minimum dimension for the specified axis.</summary>
  public void SetMinDimension(Dimension axis, StyleSizeLength value) => StoreSize(ref MinDimensionHandles[YogaEnums.ToUnderlying(axis)], value);

  /// <summary>Gets the maximum dimension for the specified axis.</summary>
  public StyleSizeLength GetMaxDimension(Dimension axis) => Pool.GetSize(MaxDimensionHandles[YogaEnums.ToUnderlying(axis)]);

  /// <summary>Sets the maximum dimension for the specified axis.</summary>
  public void SetMaxDimension(Dimension axis, StyleSizeLength value) => StoreSize(ref MaxDimensionHandles[YogaEnums.ToUnderlying(axis)], value);

  /// <summary>
  /// Gets the resolved minimum dimension, accounting for box-sizing.
  /// </summary>
  public FloatOptional ResolvedMinDimension(Direction direction, Dimension axis, float referenceLength, float ownerWidth)
  {
    FloatOptional value = GetMinDimension(axis).Resolve(referenceLength);
    if (BoxSizing == BoxSizing.BorderBox)
    {
      return value;
    }

    FloatOptional dimensionPaddingAndBorder = new(ComputePaddingAndBorderForDimension(direction, axis, ownerWidth));
    return value + (dimensionPaddingAndBorder.IsDefined ? dimensionPaddingAndBorder : new FloatOptional(0.0f));
  }

  /// <summary>
  /// Gets the resolved maximum dimension, accounting for box-sizing.
  /// </summary>
  public FloatOptional ResolvedMaxDimension(Direction direction, Dimension axis, float referenceLength, float ownerWidth)
  {
    FloatOptional value = GetMaxDimension(axis).Resolve(referenceLength);
    if (BoxSizing == BoxSizing.BorderBox)
    {
      return value;
    }

    FloatOptional dimensionPaddingAndBorder = new(ComputePaddingAndBorderForDimension(direction, axis, ownerWidth));
    return value + (dimensionPaddingAndBorder.IsDefined ? dimensionPaddingAndBorder : new FloatOptional(0.0f));
  }

  /// <summary>Gets or sets the aspect ratio.</summary>
  public FloatOptional AspectRatio
  {
    get => Pool.GetNumber(AspectRatioHandle);
    set
    {
      // Degenerate aspect ratios act as auto.
      // See https://drafts.csswg.org/css-sizing-4/#valdef-aspect-ratio-ratio
      FloatOptional normalized = value == 0.0f || float.IsInfinity(value.Unwrap())
          ? FloatOptional.Undefined
          : value;
      StoreNumber(ref AspectRatioHandle, normalized);
    }
  }

  #endregion

  #region Owner Dirtying

  private void StoreNumber(ref StyleValueHandle handle, FloatOptional value)
  {
    if (Pool.GetNumber(handle) != value)
    {
      Pool.Store(ref handle, value);
      OwnerNode?.MarkDirtyAndPropagate();
    }
  }

  private void StoreLength(ref StyleValueHandle handle, StyleLength value)
  {
    if (Pool.GetLength(handle) != value)
    {
      Pool.Store(ref handle, value);
      OwnerNode?.MarkDirtyAndPropagate();
    }
  }

  private void StoreSize(ref StyleValueHandle handle, StyleSizeLength value)
  {
    if (Pool.GetSize(handle) != value)
    {
      Pool.Store(ref handle, value);
      OwnerNode?.MarkDirtyAndPropagate();
    }
  }

  #endregion

  #region Inset Query Methods

  /// <summary>Returns true if any horizontal position inset is defined.</summary>
  public bool HorizontalInsetsDefined()
  {
    return PositionHandles[YogaEnums.ToUnderlying(Edge.Left)].IsDefined ||
           PositionHandles[YogaEnums.ToUnderlying(Edge.Right)].IsDefined ||
           PositionHandles[YogaEnums.ToUnderlying(Edge.All)].IsDefined ||
           PositionHandles[YogaEnums.ToUnderlying(Edge.Horizontal)].IsDefined ||
           PositionHandles[YogaEnums.ToUnderlying(Edge.Start)].IsDefined ||
           PositionHandles[YogaEnums.ToUnderlying(Edge.End)].IsDefined;
  }

  /// <summary>Returns true if any vertical position inset is defined.</summary>
  public bool VerticalInsetsDefined()
  {
    return PositionHandles[YogaEnums.ToUnderlying(Edge.Top)].IsDefined ||
           PositionHandles[YogaEnums.ToUnderlying(Edge.Bottom)].IsDefined ||
           PositionHandles[YogaEnums.ToUnderlying(Edge.All)].IsDefined ||
           PositionHandles[YogaEnums.ToUnderlying(Edge.Vertical)].IsDefined;
  }

  /// <summary>Returns true if flex-start position is defined.</summary>
  public bool IsFlexStartPositionDefined(FlexDirection axis, Direction direction)
  {
    return ComputePosition(axis.FlexStartEdge(), direction).IsDefined;
  }

  /// <summary>Returns true if flex-start position is auto.</summary>
  public bool IsFlexStartPositionAuto(FlexDirection axis, Direction direction)
  {
    return ComputePosition(axis.FlexStartEdge(), direction).IsAuto;
  }

  /// <summary>Returns true if inline-start position is defined.</summary>
  public bool IsInlineStartPositionDefined(FlexDirection axis, Direction direction)
  {
    return ComputePosition(axis.InlineStartEdge(direction), direction).IsDefined;
  }

  /// <summary>Returns true if inline-start position is auto.</summary>
  public bool IsInlineStartPositionAuto(FlexDirection axis, Direction direction)
  {
    return ComputePosition(axis.InlineStartEdge(direction), direction).IsAuto;
  }

  /// <summary>Returns true if flex-end position is defined.</summary>
  public bool IsFlexEndPositionDefined(FlexDirection axis, Direction direction)
  {
    return ComputePosition(axis.FlexEndEdge(), direction).IsDefined;
  }

  /// <summary>Returns true if flex-end position is auto.</summary>
  public bool IsFlexEndPositionAuto(FlexDirection axis, Direction direction)
  {
    return ComputePosition(axis.FlexEndEdge(), direction).IsAuto;
  }

  /// <summary>Returns true if inline-end position is defined.</summary>
  public bool IsInlineEndPositionDefined(FlexDirection axis, Direction direction)
  {
    return ComputePosition(axis.InlineEndEdge(direction), direction).IsDefined;
  }

  /// <summary>Returns true if inline-end position is auto.</summary>
  public bool IsInlineEndPositionAuto(FlexDirection axis, Direction direction)
  {
    return ComputePosition(axis.InlineEndEdge(direction), direction).IsAuto;
  }

  #endregion

  #region Compute Position Methods

  /// <summary>Computes the flex-start position value.</summary>
  public float ComputeFlexStartPosition(FlexDirection axis, Direction direction, float axisSize)
  {
    return ComputePosition(axis.FlexStartEdge(), direction)
        .Resolve(axisSize)
        .UnwrapOrDefault(0.0f);
  }

  /// <summary>Computes the inline-start position value.</summary>
  public float ComputeInlineStartPosition(FlexDirection axis, Direction direction, float axisSize)
  {
    return ComputePosition(axis.InlineStartEdge(direction), direction)
        .Resolve(axisSize)
        .UnwrapOrDefault(0.0f);
  }

  /// <summary>Computes the flex-end position value.</summary>
  public float ComputeFlexEndPosition(FlexDirection axis, Direction direction, float axisSize)
  {
    return ComputePosition(axis.FlexEndEdge(), direction)
        .Resolve(axisSize)
        .UnwrapOrDefault(0.0f);
  }

  /// <summary>Computes the inline-end position value.</summary>
  public float ComputeInlineEndPosition(FlexDirection axis, Direction direction, float axisSize)
  {
    return ComputePosition(axis.InlineEndEdge(direction), direction)
        .Resolve(axisSize)
        .UnwrapOrDefault(0.0f);
  }

  #endregion

  #region Compute Margin Methods

  /// <summary>Computes the flex-start margin value.</summary>
  public float ComputeFlexStartMargin(FlexDirection axis, Direction direction, float widthSize)
  {
    return ComputeMargin(axis.FlexStartEdge(), direction)
        .Resolve(widthSize)
        .UnwrapOrDefault(0.0f);
  }

  /// <summary>Computes the inline-start margin value.</summary>
  public float ComputeInlineStartMargin(FlexDirection axis, Direction direction, float widthSize)
  {
    return ComputeMargin(axis.InlineStartEdge(direction), direction)
        .Resolve(widthSize)
        .UnwrapOrDefault(0.0f);
  }

  /// <summary>Computes the flex-end margin value.</summary>
  public float ComputeFlexEndMargin(FlexDirection axis, Direction direction, float widthSize)
  {
    return ComputeMargin(axis.FlexEndEdge(), direction)
        .Resolve(widthSize)
        .UnwrapOrDefault(0.0f);
  }

  /// <summary>Computes the inline-end margin value.</summary>
  public float ComputeInlineEndMargin(FlexDirection axis, Direction direction, float widthSize)
  {
    return ComputeMargin(axis.InlineEndEdge(direction), direction)
        .Resolve(widthSize)
        .UnwrapOrDefault(0.0f);
  }

  /// <summary>Returns true if flex-start margin is auto.</summary>
  public bool FlexStartMarginIsAuto(FlexDirection axis, Direction direction)
  {
    return ComputeMargin(axis.FlexStartEdge(), direction).IsAuto;
  }

  /// <summary>Returns true if flex-end margin is auto.</summary>
  public bool FlexEndMarginIsAuto(FlexDirection axis, Direction direction)
  {
    return ComputeMargin(axis.FlexEndEdge(), direction).IsAuto;
  }

  #endregion

  #region Compute Border Methods

  /// <summary>Computes the flex-start border value.</summary>
  public float ComputeFlexStartBorder(FlexDirection axis, Direction direction)
  {
    return Comparison.MaxOrDefined(
        ComputeBorder(axis.FlexStartEdge(), direction).Resolve(0.0f).Unwrap(),
        0.0f);
  }

  /// <summary>Computes the inline-start border value.</summary>
  public float ComputeInlineStartBorder(FlexDirection axis, Direction direction)
  {
    return Comparison.MaxOrDefined(
        ComputeBorder(axis.InlineStartEdge(direction), direction).Resolve(0.0f).Unwrap(),
        0.0f);
  }

  /// <summary>Computes the flex-end border value.</summary>
  public float ComputeFlexEndBorder(FlexDirection axis, Direction direction)
  {
    return Comparison.MaxOrDefined(
        ComputeBorder(axis.FlexEndEdge(), direction).Resolve(0.0f).Unwrap(),
        0.0f);
  }

  /// <summary>Computes the inline-end border value.</summary>
  public float ComputeInlineEndBorder(FlexDirection axis, Direction direction)
  {
    return Comparison.MaxOrDefined(
        ComputeBorder(axis.InlineEndEdge(direction), direction).Resolve(0.0f).Unwrap(),
        0.0f);
  }

  #endregion

  #region Compute Padding Methods

  /// <summary>Computes the flex-start padding value.</summary>
  public float ComputeFlexStartPadding(FlexDirection axis, Direction direction, float widthSize)
  {
    return Comparison.MaxOrDefined(
        ComputePadding(axis.FlexStartEdge(), direction).Resolve(widthSize).Unwrap(),
        0.0f);
  }

  /// <summary>Computes the inline-start padding value.</summary>
  public float ComputeInlineStartPadding(FlexDirection axis, Direction direction, float widthSize)
  {
    return Comparison.MaxOrDefined(
        ComputePadding(axis.InlineStartEdge(direction), direction).Resolve(widthSize).Unwrap(),
        0.0f);
  }

  /// <summary>Computes the flex-end padding value.</summary>
  public float ComputeFlexEndPadding(FlexDirection axis, Direction direction, float widthSize)
  {
    return Comparison.MaxOrDefined(
        ComputePadding(axis.FlexEndEdge(), direction).Resolve(widthSize).Unwrap(),
        0.0f);
  }

  /// <summary>Computes the inline-end padding value.</summary>
  public float ComputeInlineEndPadding(FlexDirection axis, Direction direction, float widthSize)
  {
    return Comparison.MaxOrDefined(
        ComputePadding(axis.InlineEndEdge(direction), direction).Resolve(widthSize).Unwrap(),
        0.0f);
  }

  #endregion

  #region Compute Padding+Border Methods

  /// <summary>Computes inline-start padding plus border.</summary>
  public float ComputeInlineStartPaddingAndBorder(FlexDirection axis, Direction direction, float widthSize)
  {
    return ComputeInlineStartPadding(axis, direction, widthSize) +
           ComputeInlineStartBorder(axis, direction);
  }

  /// <summary>Computes flex-start padding plus border.</summary>
  public float ComputeFlexStartPaddingAndBorder(FlexDirection axis, Direction direction, float widthSize)
  {
    return ComputeFlexStartPadding(axis, direction, widthSize) +
           ComputeFlexStartBorder(axis, direction);
  }

  /// <summary>Computes inline-end padding plus border.</summary>
  public float ComputeInlineEndPaddingAndBorder(FlexDirection axis, Direction direction, float widthSize)
  {
    return ComputeInlineEndPadding(axis, direction, widthSize) +
           ComputeInlineEndBorder(axis, direction);
  }

  /// <summary>Computes flex-end padding plus border.</summary>
  public float ComputeFlexEndPaddingAndBorder(FlexDirection axis, Direction direction, float widthSize)
  {
    return ComputeFlexEndPadding(axis, direction, widthSize) +
           ComputeFlexEndBorder(axis, direction);
  }

  /// <summary>Computes padding and border for a dimension.</summary>
  public float ComputePaddingAndBorderForDimension(Direction direction, Dimension dimension, float widthSize)
  {
    FlexDirection flexDirectionForDimension = dimension == Dimension.Width
        ? FlexDirection.Row
        : FlexDirection.Column;

    return ComputeFlexStartPaddingAndBorder(flexDirectionForDimension, direction, widthSize) +
           ComputeFlexEndPaddingAndBorder(flexDirectionForDimension, direction, widthSize);
  }

  /// <summary>Computes the total border for an axis.</summary>
  public float ComputeBorderForAxis(FlexDirection axis)
  {
    return ComputeInlineStartBorder(axis, Direction.LTR) +
           ComputeInlineEndBorder(axis, Direction.LTR);
  }

  /// <summary>Computes the total margin for an axis.</summary>
  public float ComputeMarginForAxis(FlexDirection axis, float widthSize)
  {
    // The total margin for a given axis does not depend on the direction
    // so hardcoding LTR here to avoid piping direction to this function
    return ComputeInlineStartMargin(axis, Direction.LTR, widthSize) +
           ComputeInlineEndMargin(axis, Direction.LTR, widthSize);
  }

  /// <summary>Computes the gap for an axis.</summary>
  public float ComputeGapForAxis(FlexDirection axis, float ownerSize)
  {
    StyleLength gap = axis.IsRow() ? ComputeColumnGap() : ComputeRowGap();
    return Comparison.MaxOrDefined(gap.Resolve(ownerSize).Unwrap(), 0.0f);
  }

  #endregion

  #region Private Edge Computation Methods

  private StyleLength ComputeColumnGap()
  {
    if (GapHandles[YogaEnums.ToUnderlying(Gutter.Column)].IsDefined)
    {
      return Pool.GetLength(GapHandles[YogaEnums.ToUnderlying(Gutter.Column)]);
    }

    return Pool.GetLength(GapHandles[YogaEnums.ToUnderlying(Gutter.All)]);
  }

  private StyleLength ComputeRowGap()
  {
    if (GapHandles[YogaEnums.ToUnderlying(Gutter.Row)].IsDefined)
    {
      return Pool.GetLength(GapHandles[YogaEnums.ToUnderlying(Gutter.Row)]);
    }

    return Pool.GetLength(GapHandles[YogaEnums.ToUnderlying(Gutter.All)]);
  }

  private StyleLength ComputeLeftEdge(StyleValueHandle[] edges, Direction layoutDirection)
  {
    if (layoutDirection == Direction.LTR && edges[YogaEnums.ToUnderlying(Edge.Start)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Start)]);
    }
    else if (layoutDirection == Direction.RTL && edges[YogaEnums.ToUnderlying(Edge.End)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.End)]);
    }
    else if (edges[YogaEnums.ToUnderlying(Edge.Left)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Left)]);
    }
    else if (edges[YogaEnums.ToUnderlying(Edge.Horizontal)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Horizontal)]);
    }

    return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.All)]);
  }

  private StyleLength ComputeTopEdge(StyleValueHandle[] edges)
  {
    if (edges[YogaEnums.ToUnderlying(Edge.Top)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Top)]);
    }
    else if (edges[YogaEnums.ToUnderlying(Edge.Vertical)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Vertical)]);
    }

    return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.All)]);
  }

  private StyleLength ComputeRightEdge(StyleValueHandle[] edges, Direction layoutDirection)
  {
    if (layoutDirection == Direction.LTR && edges[YogaEnums.ToUnderlying(Edge.End)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.End)]);
    }
    else if (layoutDirection == Direction.RTL && edges[YogaEnums.ToUnderlying(Edge.Start)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Start)]);
    }
    else if (edges[YogaEnums.ToUnderlying(Edge.Right)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Right)]);
    }
    else if (edges[YogaEnums.ToUnderlying(Edge.Horizontal)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Horizontal)]);
    }

    return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.All)]);
  }

  private StyleLength ComputeBottomEdge(StyleValueHandle[] edges)
  {
    if (edges[YogaEnums.ToUnderlying(Edge.Bottom)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Bottom)]);
    }
    else if (edges[YogaEnums.ToUnderlying(Edge.Vertical)].IsDefined)
    {
      return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Vertical)]);
    }

    return Pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.All)]);
  }

  private StyleLength ComputePosition(PhysicalEdge edge, Direction direction) => edge switch
  {
    PhysicalEdge.Left => ComputeLeftEdge(PositionHandles, direction),
    PhysicalEdge.Top => ComputeTopEdge(PositionHandles),
    PhysicalEdge.Right => ComputeRightEdge(PositionHandles, direction),
    PhysicalEdge.Bottom => ComputeBottomEdge(PositionHandles),
    _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, "Invalid physical edge")
  };

  private StyleLength ComputeMargin(PhysicalEdge edge, Direction direction) => edge switch
  {
    PhysicalEdge.Left => ComputeLeftEdge(MarginHandles, direction),
    PhysicalEdge.Top => ComputeTopEdge(MarginHandles),
    PhysicalEdge.Right => ComputeRightEdge(MarginHandles, direction),
    PhysicalEdge.Bottom => ComputeBottomEdge(MarginHandles),
    _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, "Invalid physical edge")
  };

  private StyleLength ComputePadding(PhysicalEdge edge, Direction direction) => edge switch
  {
    PhysicalEdge.Left => ComputeLeftEdge(PaddingHandles, direction),
    PhysicalEdge.Top => ComputeTopEdge(PaddingHandles),
    PhysicalEdge.Right => ComputeRightEdge(PaddingHandles, direction),
    PhysicalEdge.Bottom => ComputeBottomEdge(PaddingHandles),
    _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, "Invalid physical edge")
  };

  private StyleLength ComputeBorder(PhysicalEdge edge, Direction direction) => edge switch
  {
    PhysicalEdge.Left => ComputeLeftEdge(BorderHandles, direction),
    PhysicalEdge.Top => ComputeTopEdge(BorderHandles),
    PhysicalEdge.Right => ComputeRightEdge(BorderHandles, direction),
    PhysicalEdge.Bottom => ComputeBottomEdge(BorderHandles),
    _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, "Invalid physical edge")
  };

  #endregion

  #region Equality

  /// <summary>
  /// Checks equality with another Style.
  /// </summary>
  public bool Equals(Style? other)
  {
    if (other is null)
    {
      return false;
    }

    if (ReferenceEquals(this, other))
    {
      return true;
    }

    return Direction == other.Direction &&
           FlexDirection == other.FlexDirection &&
           JustifyContent == other.JustifyContent &&
           AlignContent == other.AlignContent &&
           AlignItems == other.AlignItems &&
           AlignSelf == other.AlignSelf &&
           PositionType == other.PositionType &&
           FlexWrap == other.FlexWrap &&
           Overflow == other.Overflow &&
           Display == other.Display &&
           BoxSizing == other.BoxSizing &&
           NumbersEqual(FlexHandle, Pool, other.FlexHandle, other.Pool) &&
           NumbersEqual(FlexGrowHandle, Pool, other.FlexGrowHandle, other.Pool) &&
           NumbersEqual(FlexShrinkHandle, Pool, other.FlexShrinkHandle, other.Pool) &&
           LengthsEqual(FlexBasisHandle, Pool, other.FlexBasisHandle, other.Pool) &&
           LengthsEqual(MarginHandles, Pool, other.MarginHandles, other.Pool) &&
           LengthsEqual(PositionHandles, Pool, other.PositionHandles, other.Pool) &&
           LengthsEqual(PaddingHandles, Pool, other.PaddingHandles, other.Pool) &&
           LengthsEqual(BorderHandles, Pool, other.BorderHandles, other.Pool) &&
           LengthsEqual(GapHandles, Pool, other.GapHandles, other.Pool) &&
           LengthsEqual(DimensionHandles, Pool, other.DimensionHandles, other.Pool) &&
           LengthsEqual(MinDimensionHandles, Pool, other.MinDimensionHandles, other.Pool) &&
           LengthsEqual(MaxDimensionHandles, Pool, other.MaxDimensionHandles, other.Pool) &&
           NumbersEqual(AspectRatioHandle, Pool, other.AspectRatioHandle, other.Pool);
  }

  /// <inheritdoc />
  public override bool Equals(object? obj) => Equals(obj as Style);

  /// <inheritdoc />
  public override int GetHashCode()
  {
    HashCode hash = new();
    hash.Add(Direction);
    hash.Add(FlexDirection);
    hash.Add(JustifyContent);
    hash.Add(AlignContent);
    hash.Add(AlignItems);
    hash.Add(AlignSelf);
    hash.Add(PositionType);
    hash.Add(FlexWrap);
    hash.Add(Overflow);
    hash.Add(Display);
    hash.Add(BoxSizing);
    return hash.ToHashCode();
  }

  /// <summary>Equality operator.</summary>
  public static bool operator ==(Style? left, Style? right) =>
      left is null ? right is null : left.Equals(right);

  /// <summary>Inequality operator.</summary>
  public static bool operator !=(Style? left, Style? right) => !(left == right);

  private static bool NumbersEqual(
      StyleValueHandle lhsHandle, StyleValuePool lhsPool,
      StyleValueHandle rhsHandle, StyleValuePool rhsPool)
  {
    return (lhsHandle.IsUndefined && rhsHandle.IsUndefined) ||
           (lhsPool.GetNumber(lhsHandle) == rhsPool.GetNumber(rhsHandle));
  }

  private static bool LengthsEqual(
      StyleValueHandle lhsHandle, StyleValuePool lhsPool,
      StyleValueHandle rhsHandle, StyleValuePool rhsPool)
  {
    return (lhsHandle.IsUndefined && rhsHandle.IsUndefined) ||
           (lhsPool.GetLength(lhsHandle) == rhsPool.GetLength(rhsHandle));
  }

  private static bool LengthsEqual(
      StyleValueHandle[] lhs, StyleValuePool lhsPool,
      StyleValueHandle[] rhs, StyleValuePool rhsPool)
  {
    if (lhs.Length != rhs.Length)
    {
      return false;
    }

    for (int i = 0; i < lhs.Length; i++)
    {
      if (!LengthsEqual(lhs[i], lhsPool, rhs[i], rhsPool))
      {
        return false;
      }
    }

    return true;
  }

  #endregion
}
