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

    // Enum properties (stored directly - no bit-packing in C#)
    private Direction _direction = Direction.Inherit;
    private FlexDirection _flexDirection = FlexDirection.Column;
    private Justify _justifyContent = Justify.FlexStart;
    private Align _alignContent = Align.FlexStart;
    private Align _alignItems = Align.Stretch;
    private Align _alignSelf = Align.Auto;
    private PositionType _positionType = PositionType.Relative;
    private Wrap _flexWrap = Wrap.NoWrap;
    private Overflow _overflow = Overflow.Visible;
    private Display _display = Display.Flex;
    private BoxSizing _boxSizing = BoxSizing.BorderBox;

    // StyleValueHandle fields for pooled values
    private StyleValueHandle _flex;
    private StyleValueHandle _flexGrow;
    private StyleValueHandle _flexShrink;
    private StyleValueHandle _flexBasis = StyleValueHandle.Auto;

    // Edge arrays (margin, position, padding, border)
    private readonly StyleValueHandle[] _margin = new StyleValueHandle[YogaEnums.OrdinalCount<Edge>()];
    private readonly StyleValueHandle[] _position = new StyleValueHandle[YogaEnums.OrdinalCount<Edge>()];
    private readonly StyleValueHandle[] _padding = new StyleValueHandle[YogaEnums.OrdinalCount<Edge>()];
    private readonly StyleValueHandle[] _border = new StyleValueHandle[YogaEnums.OrdinalCount<Edge>()];

    // Gutter array (gap)
    private readonly StyleValueHandle[] _gap = new StyleValueHandle[YogaEnums.OrdinalCount<Gutter>()];

    // Dimension arrays
    private readonly StyleValueHandle[] _dimensions =
    [
        StyleValueHandle.Auto, // Width
        StyleValueHandle.Auto  // Height
    ];
    private readonly StyleValueHandle[] _minDimensions = new StyleValueHandle[YogaEnums.OrdinalCount<Dimension>()];
    private readonly StyleValueHandle[] _maxDimensions = new StyleValueHandle[YogaEnums.OrdinalCount<Dimension>()];

    // Aspect ratio
    private StyleValueHandle _aspectRatio;

    // The value pool for storing non-inlinable values
    private readonly StyleValuePool _pool = new();

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
        get => _direction;
        set => SetField(ref _direction, value);
    }

    /// <summary>Gets or sets the flex direction.</summary>
    public FlexDirection FlexDirection
    {
        get => _flexDirection;
        set => SetField(ref _flexDirection, value);
    }

    /// <summary>Gets or sets how content is justified along the main axis.</summary>
    public Justify JustifyContent
    {
        get => _justifyContent;
        set => SetField(ref _justifyContent, value);
    }

    /// <summary>Gets or sets how lines are aligned along the cross axis.</summary>
    public Align AlignContent
    {
        get => _alignContent;
        set => SetField(ref _alignContent, value);
    }

    /// <summary>Gets or sets how items are aligned along the cross axis.</summary>
    public Align AlignItems
    {
        get => _alignItems;
        set => SetField(ref _alignItems, value);
    }

    /// <summary>Gets or sets how this item is aligned (overrides parent's align-items).</summary>
    public Align AlignSelf
    {
        get => _alignSelf;
        set => SetField(ref _alignSelf, value);
    }

    /// <summary>Gets or sets the position type (static, relative, absolute).</summary>
    public PositionType PositionType
    {
        get => _positionType;
        set => SetField(ref _positionType, value);
    }

    /// <summary>Gets or sets the flex wrap behavior.</summary>
    public Wrap FlexWrap
    {
        get => _flexWrap;
        set => SetField(ref _flexWrap, value);
    }

    /// <summary>Gets or sets the overflow behavior.</summary>
    public Overflow Overflow
    {
        get => _overflow;
        set => SetField(ref _overflow, value);
    }

    /// <summary>Gets or sets the display type.</summary>
    public Display Display
    {
        get => _display;
        set => SetField(ref _display, value);
    }

    /// <summary>Gets or sets the box-sizing mode.</summary>
    public BoxSizing BoxSizing
    {
        get => _boxSizing;
        set => SetField(ref _boxSizing, value);
    }

    #endregion

    #region Flex Properties

    /// <summary>Gets or sets the flex shorthand value.</summary>
    public FloatOptional Flex
    {
        get => _pool.GetNumber(_flex);
        set => StoreNumber(ref _flex, value);
    }

    /// <summary>Gets or sets the flex-grow value.</summary>
    public FloatOptional FlexGrow
    {
        get => _pool.GetNumber(_flexGrow);
        set => StoreNumber(ref _flexGrow, value);
    }

    /// <summary>Gets or sets the flex-shrink value.</summary>
    public FloatOptional FlexShrink
    {
        get => _pool.GetNumber(_flexShrink);
        set => StoreNumber(ref _flexShrink, value);
    }

    /// <summary>Gets or sets the flex-basis value.</summary>
    public StyleSizeLength FlexBasis
    {
        get => _pool.GetSize(_flexBasis);
        set => StoreSize(ref _flexBasis, value);
    }

    #endregion

    #region Edge Properties (Margin, Position, Padding, Border)

    /// <summary>Gets the margin for the specified edge.</summary>
    public StyleLength GetMargin(Edge edge) => _pool.GetLength(_margin[YogaEnums.ToUnderlying(edge)]);

    /// <summary>Sets the margin for the specified edge.</summary>
    public void SetMargin(Edge edge, StyleLength value) => StoreLength(ref _margin[YogaEnums.ToUnderlying(edge)], value);

    /// <summary>Gets the position for the specified edge.</summary>
    public StyleLength GetPosition(Edge edge) => _pool.GetLength(_position[YogaEnums.ToUnderlying(edge)]);

    /// <summary>Sets the position for the specified edge.</summary>
    public void SetPosition(Edge edge, StyleLength value) => StoreLength(ref _position[YogaEnums.ToUnderlying(edge)], value);

    /// <summary>Gets the padding for the specified edge.</summary>
    public StyleLength GetPadding(Edge edge) => _pool.GetLength(_padding[YogaEnums.ToUnderlying(edge)]);

    /// <summary>Sets the padding for the specified edge.</summary>
    public void SetPadding(Edge edge, StyleLength value) => StoreLength(ref _padding[YogaEnums.ToUnderlying(edge)], value);

    /// <summary>Gets the border for the specified edge.</summary>
    public StyleLength GetBorder(Edge edge) => _pool.GetLength(_border[YogaEnums.ToUnderlying(edge)]);

    /// <summary>Sets the border for the specified edge.</summary>
    public void SetBorder(Edge edge, StyleLength value) => StoreLength(ref _border[YogaEnums.ToUnderlying(edge)], value);

    /// <summary>Gets the gap for the specified gutter.</summary>
    public StyleLength GetGap(Gutter gutter) => _pool.GetLength(_gap[YogaEnums.ToUnderlying(gutter)]);

    /// <summary>Sets the gap for the specified gutter.</summary>
    public void SetGap(Gutter gutter, StyleLength value) => StoreLength(ref _gap[YogaEnums.ToUnderlying(gutter)], value);

    #endregion

    #region Dimension Properties

    /// <summary>Gets the dimension (width or height) for the specified axis.</summary>
    public StyleSizeLength GetDimension(Dimension axis) => _pool.GetSize(_dimensions[YogaEnums.ToUnderlying(axis)]);

    /// <summary>Sets the dimension (width or height) for the specified axis.</summary>
    public void SetDimension(Dimension axis, StyleSizeLength value) => StoreSize(ref _dimensions[YogaEnums.ToUnderlying(axis)], value);

    /// <summary>Gets the minimum dimension for the specified axis.</summary>
    public StyleSizeLength GetMinDimension(Dimension axis) => _pool.GetSize(_minDimensions[YogaEnums.ToUnderlying(axis)]);

    /// <summary>Sets the minimum dimension for the specified axis.</summary>
    public void SetMinDimension(Dimension axis, StyleSizeLength value) => StoreSize(ref _minDimensions[YogaEnums.ToUnderlying(axis)], value);

    /// <summary>Gets the maximum dimension for the specified axis.</summary>
    public StyleSizeLength GetMaxDimension(Dimension axis) => _pool.GetSize(_maxDimensions[YogaEnums.ToUnderlying(axis)]);

    /// <summary>Sets the maximum dimension for the specified axis.</summary>
    public void SetMaxDimension(Dimension axis, StyleSizeLength value) => StoreSize(ref _maxDimensions[YogaEnums.ToUnderlying(axis)], value);

    /// <summary>
    /// Gets the resolved minimum dimension, accounting for box-sizing.
    /// </summary>
    public FloatOptional ResolvedMinDimension(Direction direction, Dimension axis, float referenceLength, float ownerWidth)
    {
        FloatOptional value = GetMinDimension(axis).Resolve(referenceLength);
        if (_boxSizing == BoxSizing.BorderBox)
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
        if (_boxSizing == BoxSizing.BorderBox)
        {
            return value;
        }

        FloatOptional dimensionPaddingAndBorder = new(ComputePaddingAndBorderForDimension(direction, axis, ownerWidth));
        return value + (dimensionPaddingAndBorder.IsDefined ? dimensionPaddingAndBorder : new FloatOptional(0.0f));
    }

    /// <summary>Gets or sets the aspect ratio.</summary>
    public FloatOptional AspectRatio
    {
        get => _pool.GetNumber(_aspectRatio);
        set
        {
            // Degenerate aspect ratios act as auto.
            // See https://drafts.csswg.org/css-sizing-4/#valdef-aspect-ratio-ratio
            FloatOptional normalized = value == 0.0f || float.IsInfinity(value.Unwrap())
                ? FloatOptional.Undefined
                : value;
            StoreNumber(ref _aspectRatio, normalized);
        }
    }

    #endregion

    #region Owner Dirtying

    private void SetField<T>(ref T field, T value) where T : struct
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            OwnerNode?.MarkDirtyAndPropagate();
        }
    }

    private void StoreNumber(ref StyleValueHandle handle, FloatOptional value)
    {
        if (_pool.GetNumber(handle) != value)
        {
            _pool.Store(ref handle, value);
            OwnerNode?.MarkDirtyAndPropagate();
        }
    }

    private void StoreLength(ref StyleValueHandle handle, StyleLength value)
    {
        if (_pool.GetLength(handle) != value)
        {
            _pool.Store(ref handle, value);
            OwnerNode?.MarkDirtyAndPropagate();
        }
    }

    private void StoreSize(ref StyleValueHandle handle, StyleSizeLength value)
    {
        if (_pool.GetSize(handle) != value)
        {
            _pool.Store(ref handle, value);
            OwnerNode?.MarkDirtyAndPropagate();
        }
    }

    #endregion

    #region Inset Query Methods

    /// <summary>Returns true if any horizontal position inset is defined.</summary>
    public bool HorizontalInsetsDefined()
    {
        return _position[YogaEnums.ToUnderlying(Edge.Left)].IsDefined ||
               _position[YogaEnums.ToUnderlying(Edge.Right)].IsDefined ||
               _position[YogaEnums.ToUnderlying(Edge.All)].IsDefined ||
               _position[YogaEnums.ToUnderlying(Edge.Horizontal)].IsDefined ||
               _position[YogaEnums.ToUnderlying(Edge.Start)].IsDefined ||
               _position[YogaEnums.ToUnderlying(Edge.End)].IsDefined;
    }

    /// <summary>Returns true if any vertical position inset is defined.</summary>
    public bool VerticalInsetsDefined()
    {
        return _position[YogaEnums.ToUnderlying(Edge.Top)].IsDefined ||
               _position[YogaEnums.ToUnderlying(Edge.Bottom)].IsDefined ||
               _position[YogaEnums.ToUnderlying(Edge.All)].IsDefined ||
               _position[YogaEnums.ToUnderlying(Edge.Vertical)].IsDefined;
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
        if (_gap[YogaEnums.ToUnderlying(Gutter.Column)].IsDefined)
        {
            return _pool.GetLength(_gap[YogaEnums.ToUnderlying(Gutter.Column)]);
        }

        return _pool.GetLength(_gap[YogaEnums.ToUnderlying(Gutter.All)]);
    }

    private StyleLength ComputeRowGap()
    {
        if (_gap[YogaEnums.ToUnderlying(Gutter.Row)].IsDefined)
        {
            return _pool.GetLength(_gap[YogaEnums.ToUnderlying(Gutter.Row)]);
        }

        return _pool.GetLength(_gap[YogaEnums.ToUnderlying(Gutter.All)]);
    }

    private StyleLength ComputeLeftEdge(StyleValueHandle[] edges, Direction layoutDirection)
    {
        if (layoutDirection == Direction.LTR && edges[YogaEnums.ToUnderlying(Edge.Start)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Start)]);
        }
        else if (layoutDirection == Direction.RTL && edges[YogaEnums.ToUnderlying(Edge.End)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.End)]);
        }
        else if (edges[YogaEnums.ToUnderlying(Edge.Left)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Left)]);
        }
        else if (edges[YogaEnums.ToUnderlying(Edge.Horizontal)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Horizontal)]);
        }

        return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.All)]);
    }

    private StyleLength ComputeTopEdge(StyleValueHandle[] edges)
    {
        if (edges[YogaEnums.ToUnderlying(Edge.Top)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Top)]);
        }
        else if (edges[YogaEnums.ToUnderlying(Edge.Vertical)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Vertical)]);
        }

        return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.All)]);
    }

    private StyleLength ComputeRightEdge(StyleValueHandle[] edges, Direction layoutDirection)
    {
        if (layoutDirection == Direction.LTR && edges[YogaEnums.ToUnderlying(Edge.End)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.End)]);
        }
        else if (layoutDirection == Direction.RTL && edges[YogaEnums.ToUnderlying(Edge.Start)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Start)]);
        }
        else if (edges[YogaEnums.ToUnderlying(Edge.Right)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Right)]);
        }
        else if (edges[YogaEnums.ToUnderlying(Edge.Horizontal)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Horizontal)]);
        }

        return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.All)]);
    }

    private StyleLength ComputeBottomEdge(StyleValueHandle[] edges)
    {
        if (edges[YogaEnums.ToUnderlying(Edge.Bottom)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Bottom)]);
        }
        else if (edges[YogaEnums.ToUnderlying(Edge.Vertical)].IsDefined)
        {
            return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.Vertical)]);
        }

        return _pool.GetLength(edges[YogaEnums.ToUnderlying(Edge.All)]);
    }

    private StyleLength ComputePosition(PhysicalEdge edge, Direction direction) => edge switch
    {
        PhysicalEdge.Left => ComputeLeftEdge(_position, direction),
        PhysicalEdge.Top => ComputeTopEdge(_position),
        PhysicalEdge.Right => ComputeRightEdge(_position, direction),
        PhysicalEdge.Bottom => ComputeBottomEdge(_position),
        _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, "Invalid physical edge")
    };

    private StyleLength ComputeMargin(PhysicalEdge edge, Direction direction) => edge switch
    {
        PhysicalEdge.Left => ComputeLeftEdge(_margin, direction),
        PhysicalEdge.Top => ComputeTopEdge(_margin),
        PhysicalEdge.Right => ComputeRightEdge(_margin, direction),
        PhysicalEdge.Bottom => ComputeBottomEdge(_margin),
        _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, "Invalid physical edge")
    };

    private StyleLength ComputePadding(PhysicalEdge edge, Direction direction) => edge switch
    {
        PhysicalEdge.Left => ComputeLeftEdge(_padding, direction),
        PhysicalEdge.Top => ComputeTopEdge(_padding),
        PhysicalEdge.Right => ComputeRightEdge(_padding, direction),
        PhysicalEdge.Bottom => ComputeBottomEdge(_padding),
        _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, "Invalid physical edge")
    };

    private StyleLength ComputeBorder(PhysicalEdge edge, Direction direction) => edge switch
    {
        PhysicalEdge.Left => ComputeLeftEdge(_border, direction),
        PhysicalEdge.Top => ComputeTopEdge(_border),
        PhysicalEdge.Right => ComputeRightEdge(_border, direction),
        PhysicalEdge.Bottom => ComputeBottomEdge(_border),
        _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, "Invalid physical edge")
    };

    #endregion

    #region Equality

    /// <summary>
    /// Checks equality with another Style.
    /// </summary>
    public bool Equals(Style? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return _direction == other._direction &&
               _flexDirection == other._flexDirection &&
               _justifyContent == other._justifyContent &&
               _alignContent == other._alignContent &&
               _alignItems == other._alignItems &&
               _alignSelf == other._alignSelf &&
               _positionType == other._positionType &&
               _flexWrap == other._flexWrap &&
               _overflow == other._overflow &&
               _display == other._display &&
               _boxSizing == other._boxSizing &&
               NumbersEqual(_flex, _pool, other._flex, other._pool) &&
               NumbersEqual(_flexGrow, _pool, other._flexGrow, other._pool) &&
               NumbersEqual(_flexShrink, _pool, other._flexShrink, other._pool) &&
               LengthsEqual(_flexBasis, _pool, other._flexBasis, other._pool) &&
               LengthsEqual(_margin, _pool, other._margin, other._pool) &&
               LengthsEqual(_position, _pool, other._position, other._pool) &&
               LengthsEqual(_padding, _pool, other._padding, other._pool) &&
               LengthsEqual(_border, _pool, other._border, other._pool) &&
               LengthsEqual(_gap, _pool, other._gap, other._pool) &&
               LengthsEqual(_dimensions, _pool, other._dimensions, other._pool) &&
               LengthsEqual(_minDimensions, _pool, other._minDimensions, other._pool) &&
               LengthsEqual(_maxDimensions, _pool, other._maxDimensions, other._pool) &&
               NumbersEqual(_aspectRatio, _pool, other._aspectRatio, other._pool);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as Style);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(_direction);
        hash.Add(_flexDirection);
        hash.Add(_justifyContent);
        hash.Add(_alignContent);
        hash.Add(_alignItems);
        hash.Add(_alignSelf);
        hash.Add(_positionType);
        hash.Add(_flexWrap);
        hash.Add(_overflow);
        hash.Add(_display);
        hash.Add(_boxSizing);
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
        if (lhs.Length != rhs.Length) return false;

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
