/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/node/Node.h, yoga/node/Node.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Delegate for measuring the size of a node's content.
/// </summary>
/// <param name="node">The node being measured.</param>
/// <param name="width">The available width.</param>
/// <param name="widthMode">The width measurement mode.</param>
/// <param name="height">The available height.</param>
/// <param name="heightMode">The height measurement mode.</param>
/// <returns>The measured size.</returns>
public delegate YGSize MeasureFunc(Node node, float width, MeasureMode widthMode, float height, MeasureMode heightMode);

/// <summary>
/// Delegate for calculating the baseline of a node's content.
/// </summary>
/// <param name="node">The node being measured.</param>
/// <param name="width">The node's width.</param>
/// <param name="height">The node's height.</param>
/// <returns>The baseline offset from the top of the node.</returns>
public delegate float BaselineFunc(Node node, float width, float height);

/// <summary>
/// Delegate called when a node becomes dirty.
/// </summary>
/// <param name="node">The node that became dirty.</param>
public delegate void DirtiedFunc(Node node);

/// <summary>
/// Represents a measured size with width and height.
/// </summary>
public readonly struct YGSize : IEquatable<YGSize>
{
    /// <summary>The width component.</summary>
    public float Width { get; init; }

    /// <summary>The height component.</summary>
    public float Height { get; init; }

    /// <summary>
    /// Creates a new YGSize with the specified dimensions.
    /// </summary>
    public YGSize(float width, float height)
    {
        Width = width;
        Height = height;
    }

    /// <inheritdoc />
    public bool Equals(YGSize other) => Width.Equals(other.Width) && Height.Equals(other.Height);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is YGSize other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Width, Height);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(YGSize left, YGSize right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(YGSize left, YGSize right) => !left.Equals(right);
}

/// <summary>
/// The core layout node containing style, children, and layout results.
/// </summary>
/// <remarks>
/// <para>
/// This is a direct port of C++ yoga/node/Node.h and Node.cpp.
/// The Node class is the fundamental building block of the Yoga layout system.
/// </para>
/// <para>
/// Design Decision: Unlike the C++ version which separates Node (internal) and YGNode (public API),
/// we use a single class since C# doesn't have the same header/implementation separation pattern.
/// </para>
/// </remarks>
public sealed class Node : ILayoutableNode
{
    #region Private Fields

    // State flags
    private bool _hasNewLayout = true;
    private bool _isReferenceBaseline;
    private bool _isDirty = true;
    private bool _alwaysFormsContainingBlock;
    private NodeType _nodeType = NodeType.Default;

    // Context for user data
    private object? _context;

    // Callbacks
    private MeasureFunc? _measureFunc;
    private BaselineFunc? _baselineFunc;
    private DirtiedFunc? _dirtiedFunc;

    // Style and layout
    private readonly Style _style = new();
    private readonly LayoutResults _layout = new();

    // Tree structure
    private int _lineIndex;
    private int _contentsChildrenCount;
    private Node? _owner;
    private readonly List<Node> _children = [];
    private Config _config;

    // Processed dimensions cache
    private readonly StyleSizeLength[] _processedDimensions =
    [
        StyleSizeLength.Undefined,
        StyleSizeLength.Undefined
    ];

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new Node with the default configuration.
    /// </summary>
    public Node() : this(Config.Default)
    {
    }

    /// <summary>
    /// Creates a new Node with the specified configuration.
    /// </summary>
    /// <param name="config">The configuration to use.</param>
    /// <exception cref="YogaAssertException">Thrown if config is null.</exception>
    public Node(Config config)
    {
        YogaAssert.Assert(config is not null, "Attempting to construct Node with null config");
        _config = config!;

        if (_config.UseWebDefaults)
        {
            UseWebDefaults();
        }

        YogaEvent.PublishNodeAllocation(this, _config);
    }

    /// <summary>
    /// Copy constructor - creates a shallow copy of the node.
    /// </summary>
    /// <remarks>
    /// Children are not cloned eagerly. The original C++ comment notes this
    /// doesn't expose true value semantics.
    /// </remarks>
    /// <param name="other">The node to copy from.</param>
    private Node(Node other)
    {
        _hasNewLayout = other._hasNewLayout;
        _isReferenceBaseline = other._isReferenceBaseline;
        _isDirty = other._isDirty;
        _alwaysFormsContainingBlock = other._alwaysFormsContainingBlock;
        _nodeType = other._nodeType;
        _context = other._context;
        _measureFunc = other._measureFunc;
        _baselineFunc = other._baselineFunc;
        _dirtiedFunc = other._dirtiedFunc;
        _lineIndex = other._lineIndex;
        _contentsChildrenCount = other._contentsChildrenCount;
        _owner = other._owner;
        _config = other._config;

        // Copy style properties
        CopyStyleFrom(other._style);

        // Copy layout results
        CopyLayoutFrom(other._layout);

        // Copy processed dimensions
        Array.Copy(other._processedDimensions, _processedDimensions, 2);

        // Shallow copy children list
        _children.AddRange(other._children);

        YogaEvent.PublishNodeAllocation(this, _config);
    }

    #endregion

    #region ILayoutableNode Implementation

    /// <inheritdoc />
    public ILayoutableNode GetChild(int index) => _children[index];

    /// <inheritdoc />
    public int GetChildCount() => _children.Count;

    /// <inheritdoc />
    public Display GetDisplay() => _style.Display;

    #endregion

    #region Properties - Getters

    /// <summary>
    /// Gets or sets the user context object.
    /// </summary>
    public object? Context
    {
        get => _context;
        set => _context = value;
    }

    /// <summary>
    /// Gets or sets whether this node always forms a containing block for
    /// absolutely positioned descendants.
    /// </summary>
    public bool AlwaysFormsContainingBlock
    {
        get => _alwaysFormsContainingBlock;
        set => _alwaysFormsContainingBlock = value;
    }

    /// <summary>
    /// Gets whether this node has new layout results that haven't been read yet.
    /// </summary>
    public bool HasNewLayout
    {
        get => _hasNewLayout;
        set => _hasNewLayout = value;
    }

    /// <summary>
    /// Gets or sets the node type.
    /// </summary>
    public NodeType NodeType
    {
        get => _nodeType;
        set => _nodeType = value;
    }

    /// <summary>
    /// Gets whether this node has a measure function.
    /// </summary>
    public bool HasMeasureFunc => _measureFunc is not null;

    /// <summary>
    /// Gets whether this node has a baseline function.
    /// </summary>
    public bool HasBaselineFunc => _baselineFunc is not null;

    /// <summary>
    /// Gets whether this node has the specified errata enabled.
    /// </summary>
    public bool HasErrata(Errata errata) => _config.HasErrata(errata);

    /// <summary>
    /// Gets whether this node has any display:contents children.
    /// </summary>
    public bool HasContentsChildren => _contentsChildrenCount != 0;

    /// <summary>
    /// Gets or sets the dirtied callback function.
    /// </summary>
    public DirtiedFunc? DirtiedFunc
    {
        get => _dirtiedFunc;
        set => _dirtiedFunc = value;
    }

    /// <summary>
    /// Gets the style for this node.
    /// </summary>
    public Style Style => _style;

    /// <summary>
    /// Gets the layout results for this node.
    /// </summary>
    public LayoutResults Layout => _layout;

    /// <summary>
    /// Gets or sets the line index (for flex wrapping).
    /// </summary>
    public int LineIndex
    {
        get => _lineIndex;
        set => _lineIndex = value;
    }

    /// <summary>
    /// Gets or sets whether this node is the reference baseline for its siblings.
    /// </summary>
    public bool IsReferenceBaseline
    {
        get => _isReferenceBaseline;
        set => _isReferenceBaseline = value;
    }

    /// <summary>
    /// Gets the owner node (the node that owns this one in the tree).
    /// </summary>
    /// <remarks>
    /// An owner is used to identify the YogaTree that a Node belongs to.
    /// This method will return the parent of the Node when a Node only belongs
    /// to one YogaTree or null when the Node is shared between two or more YogaTrees.
    /// </remarks>
    public Node? Owner
    {
        get => _owner;
        set => _owner = value;
    }

    /// <summary>
    /// Gets the read-only list of children.
    /// </summary>
    public IReadOnlyList<Node> Children => _children;

    /// <summary>
    /// Gets the child at the specified index.
    /// </summary>
    public Node GetChildNode(int index) => _children[index];

    // Note: ChildCount property removed to avoid CA1721 conflict with GetChildCount() from ILayoutableNode

    /// <summary>
    /// Gets an enumerable over layoutable children (handling display:contents).
    /// </summary>
    public LayoutableChildren<Node> LayoutChildren => new(this);

    /// <summary>
    /// Gets the count of layoutable children (handling display:contents).
    /// </summary>
    public int LayoutChildCount
    {
        get
        {
            if (_contentsChildrenCount == 0)
            {
                return _children.Count;
            }

            int count = 0;
            foreach (Node _ in LayoutChildren)
            {
                count++;
            }

            return count;
        }
    }

    /// <summary>
    /// Gets the configuration for this node.
    /// </summary>
    public Config Config => _config;

    /// <summary>
    /// Gets whether this node is dirty and needs layout recalculation.
    /// </summary>
    public bool IsDirty => _isDirty;

    /// <summary>
    /// Gets the processed dimension for the specified axis.
    /// </summary>
    public StyleSizeLength GetProcessedDimension(Dimension dimension) =>
        _processedDimensions[YogaEnums.ToUnderlying(dimension)];

    /// <summary>
    /// Gets the resolved dimension, accounting for box-sizing.
    /// </summary>
    public FloatOptional GetResolvedDimension(
        Direction direction,
        Dimension dimension,
        float referenceLength,
        float ownerWidth)
    {
        FloatOptional value = GetProcessedDimension(dimension).Resolve(referenceLength);
        if (_style.BoxSizing == BoxSizing.BorderBox)
        {
            return value;
        }

        FloatOptional dimensionPaddingAndBorder = new(
            _style.ComputePaddingAndBorderForDimension(direction, dimension, ownerWidth));

        return value + (dimensionPaddingAndBorder.IsDefined ? dimensionPaddingAndBorder : new FloatOptional(0.0f));
    }

    #endregion

    #region Measure and Baseline

    /// <summary>
    /// Measures the node's content.
    /// </summary>
    /// <param name="availableWidth">The available width.</param>
    /// <param name="widthMode">The width measurement mode.</param>
    /// <param name="availableHeight">The available height.</param>
    /// <param name="heightMode">The height measurement mode.</param>
    /// <returns>The measured size.</returns>
    public YGSize Measure(
        float availableWidth,
        MeasureMode widthMode,
        float availableHeight,
        MeasureMode heightMode)
    {
        YGSize size = _measureFunc!(this, availableWidth, widthMode, availableHeight, heightMode);

        if (Comparison.IsUndefined(size.Height) || size.Height < 0 ||
            Comparison.IsUndefined(size.Width) || size.Width < 0)
        {
            YogaLog.Log(this, LogLevel.Warn,
                $"Measure function returned an invalid dimension to Yoga: [width={size.Width}, height={size.Height}]");
            return new YGSize
            {
                Width = Comparison.MaxOrDefined(0.0f, size.Width),
                Height = Comparison.MaxOrDefined(0.0f, size.Height)
            };
        }

        return size;
    }

    /// <summary>
    /// Calculates the baseline for this node.
    /// </summary>
    /// <param name="width">The node's width.</param>
    /// <param name="height">The node's height.</param>
    /// <returns>The baseline offset from the top.</returns>
    public float Baseline(float width, float height)
    {
        return _baselineFunc!(this, width, height);
    }

    /// <summary>
    /// Gets the dimension with margin for the specified axis.
    /// </summary>
    public float DimensionWithMargin(FlexDirection axis, float widthSize)
    {
        return _layout.GetMeasuredDimension(axis.GetDimension()) +
               _style.ComputeMarginForAxis(axis, widthSize);
    }

    /// <summary>
    /// Gets whether the layout dimension is defined for the specified axis.
    /// </summary>
    public bool IsLayoutDimensionDefined(FlexDirection axis)
    {
        float value = _layout.GetMeasuredDimension(axis.GetDimension());
        return Comparison.IsDefined(value) && value >= 0.0f;
    }

    /// <summary>
    /// Gets whether this node has a definite length along the specified dimension.
    /// </summary>
    /// <remarks>
    /// See: https://www.w3.org/TR/css-sizing-3/#definite
    /// </remarks>
    public bool HasDefiniteLength(Dimension dimension, float ownerSize)
    {
        FloatOptional usedValue = GetProcessedDimension(dimension).Resolve(ownerSize);
        return usedValue.IsDefined && usedValue.Unwrap() >= 0.0f;
    }

    #endregion

    #region Setters

    /// <summary>
    /// Sets the measure function for this node.
    /// </summary>
    /// <remarks>
    /// Nodes with measure functions cannot have children.
    /// Setting a measure function changes the node type to Text.
    /// </remarks>
    /// <param name="measureFunc">The measure function, or null to clear.</param>
    public void SetMeasureFunc(MeasureFunc? measureFunc)
    {
        if (measureFunc is null)
        {
            _nodeType = NodeType.Default;
        }
        else
        {
            YogaAssert.Assert(this, _children.Count == 0,
                "Cannot set measure function: Nodes with measure functions cannot have children.");
            _nodeType = NodeType.Text;
        }

        _measureFunc = measureFunc;
    }

    /// <summary>
    /// Gets or sets the baseline function.
    /// </summary>
    public BaselineFunc? BaselineFunc
    {
        get => _baselineFunc;
        set => _baselineFunc = value;
    }

    /// <summary>
    /// Sets the style from another style instance.
    /// </summary>
    public void SetStyle(Style style)
    {
        ArgumentNullException.ThrowIfNull(style);
        CopyStyleFrom(style);
    }

    /// <summary>
    /// Sets the layout results from another instance.
    /// </summary>
    public void SetLayout(LayoutResults layout)
    {
        ArgumentNullException.ThrowIfNull(layout);
        CopyLayoutFrom(layout);
    }

    /// <summary>
    /// Sets the configuration for this node.
    /// </summary>
    /// <exception cref="YogaAssertException">Thrown if config is null or UseWebDefaults changed.</exception>
    public void SetConfig(Config config)
    {
        ArgumentNullException.ThrowIfNull(config);
        YogaAssert.Assert(_config, config.UseWebDefaults == _config.UseWebDefaults,
            "UseWebDefaults may not be changed after constructing a Node");

        if (Config.ConfigUpdateInvalidatesLayout(_config, config))
        {
            MarkDirtyAndPropagate();
            _layout.ConfigVersion = 0;
        }
        else
        {
            // If the config is functionally the same, then align the configVersion so
            // that we can reuse the layout cache
            _layout.ConfigVersion = config.Version;
        }

        _config = config;
    }

    /// <summary>
    /// Sets the dirty state of this node.
    /// </summary>
    public void SetDirty(bool isDirty)
    {
        if (isDirty == _isDirty)
        {
            return;
        }

        _isDirty = isDirty;
        if (isDirty && _dirtiedFunc is not null)
        {
            _dirtiedFunc(this);
        }
    }

    /// <summary>
    /// Sets all children, replacing any existing children.
    /// </summary>
    public void SetChildren(IEnumerable<Node> children)
    {
        _children.Clear();
        _children.AddRange(children);

        _contentsChildrenCount = 0;
        foreach (Node child in _children)
        {
            if (child._style.Display == Display.Contents)
            {
                _contentsChildrenCount++;
            }
        }
    }

    /// <summary>
    /// Sets the layout's last owner direction.
    /// </summary>
    public void SetLayoutLastOwnerDirection(Direction direction)
    {
        _layout.LastOwnerDirection = direction;
    }

    /// <summary>
    /// Sets the computed flex basis.
    /// </summary>
    public void SetLayoutComputedFlexBasis(FloatOptional computedFlexBasis)
    {
        _layout.ComputedFlexBasis = computedFlexBasis;
    }

    /// <summary>
    /// Sets the computed flex basis generation counter.
    /// </summary>
    public void SetLayoutComputedFlexBasisGeneration(uint generation)
    {
        _layout.ComputedFlexBasisGeneration = generation;
    }

    /// <summary>
    /// Sets the measured dimension for the specified axis.
    /// </summary>
    public void SetLayoutMeasuredDimension(float measuredDimension, Dimension dimension)
    {
        _layout.SetMeasuredDimension(dimension, measuredDimension);
    }

    /// <summary>
    /// Sets whether the layout had overflow.
    /// </summary>
    public void SetLayoutHadOverflow(bool hadOverflow)
    {
        _layout.SetHadOverflow(hadOverflow);
    }

    /// <summary>
    /// Sets the layout dimension for the specified axis.
    /// </summary>
    public void SetLayoutDimension(float lengthValue, Dimension dimension)
    {
        _layout.SetDimension(dimension, lengthValue);
        _layout.SetRawDimension(dimension, lengthValue);
    }

    /// <summary>
    /// Sets the layout direction.
    /// </summary>
    public void SetLayoutDirection(Direction direction)
    {
        _layout.SetDirection(direction);
    }

    /// <summary>
    /// Sets the layout margin for the specified edge.
    /// </summary>
    public void SetLayoutMargin(float margin, PhysicalEdge edge)
    {
        _layout.SetMargin(edge, margin);
    }

    /// <summary>
    /// Sets the layout border for the specified edge.
    /// </summary>
    public void SetLayoutBorder(float border, PhysicalEdge edge)
    {
        _layout.SetBorder(edge, border);
    }

    /// <summary>
    /// Sets the layout padding for the specified edge.
    /// </summary>
    public void SetLayoutPadding(float padding, PhysicalEdge edge)
    {
        _layout.SetPadding(edge, padding);
    }

    /// <summary>
    /// Sets the layout position for the specified edge.
    /// </summary>
    public void SetLayoutPosition(float position, PhysicalEdge edge)
    {
        _layout.SetPosition(edge, position);
    }

    #endregion

    #region Child Management

    /// <summary>
    /// Replaces the child at the specified index.
    /// </summary>
    public void ReplaceChild(Node child, int index)
    {
        ArgumentNullException.ThrowIfNull(child);
        Node previousChild = _children[index];
        if (previousChild._style.Display == Display.Contents &&
            child._style.Display != Display.Contents)
        {
            _contentsChildrenCount--;
        }
        else if (previousChild._style.Display != Display.Contents &&
                 child._style.Display == Display.Contents)
        {
            _contentsChildrenCount++;
        }

        _children[index] = child;
        child._owner = this;
    }

    /// <summary>
    /// Replaces oldChild with newChild.
    /// </summary>
    public void ReplaceChild(Node oldChild, Node newChild)
    {
        ArgumentNullException.ThrowIfNull(oldChild);
        ArgumentNullException.ThrowIfNull(newChild);
        if (oldChild._style.Display == Display.Contents &&
            newChild._style.Display != Display.Contents)
        {
            _contentsChildrenCount--;
        }
        else if (oldChild._style.Display != Display.Contents &&
                 newChild._style.Display == Display.Contents)
        {
            _contentsChildrenCount++;
        }

        int index = _children.IndexOf(oldChild);
        if (index >= 0)
        {
            _children[index] = newChild;
            newChild._owner = this;
        }
    }

    /// <summary>
    /// Inserts a child at the specified index, taking ownership of it.
    /// </summary>
    /// <remarks>
    /// Corresponds to YGNodeInsertChild in YGNode.cpp: inserts the child,
    /// assigns this node as its owner, and marks the tree dirty.
    /// </remarks>
    public void InsertChild(Node child, int index)
    {
        ArgumentNullException.ThrowIfNull(child);
        YogaAssert.Assert(this, child._owner is null,
            "Child already has a owner, it must be removed first.");
        YogaAssert.Assert(this, !HasMeasureFunc,
            "Cannot add child: Nodes with measure functions cannot have children.");

        if (child._style.Display == Display.Contents)
        {
            _contentsChildrenCount++;
        }

        _children.Insert(index, child);
        child._owner = this;
        MarkDirtyAndPropagate();
    }

    /// <summary>
    /// Removes the first occurrence of the specified child.
    /// </summary>
    /// <returns>True if the child was removed, false if not found.</returns>
    /// <remarks>
    /// Corresponds to YGNodeRemoveChild in YGNode.cpp. Children may be shared
    /// between owners, indicated by a null owner; the child is only fully reset
    /// when it is owned exclusively by this node.
    /// </remarks>
    public bool RemoveChild(Node child)
    {
        ArgumentNullException.ThrowIfNull(child);
        if (_children.Count == 0)
        {
            // This is an empty set. Nothing to remove.
            return false;
        }

        Node? childOwner = child._owner;
        int index = _children.IndexOf(child);
        if (index >= 0)
        {
            if (child._style.Display == Display.Contents)
            {
                _contentsChildrenCount--;
            }

            _children.RemoveAt(index);

            if (childOwner == this)
            {
                child.ResetLayoutResults(); // layout is no longer valid
                child._owner = null;

                // Mark dirty to invalidate cache, but suppress the dirtied callback
                // since the node is being detached from the tree and should not
                // propagate dirty signals through external callback mechanisms.
                DirtiedFunc? dirtiedFunc = child._dirtiedFunc;
                child._dirtiedFunc = null;
                child.SetDirty(true);
                child._dirtiedFunc = dirtiedFunc;
            }

            MarkDirtyAndPropagate();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes the child at the specified index.
    /// </summary>
    public void RemoveChild(int index)
    {
        if (_children[index]._style.Display == Display.Contents)
        {
            _contentsChildrenCount--;
        }

        _children.RemoveAt(index);
    }

    /// <summary>
    /// Removes all children from this node.
    /// </summary>
    /// <remarks>
    /// Corresponds to YGNodeRemoveAllChildren in YGNode.cpp: when the child set
    /// is owned by this node, each child's layout is reset and its owner cleared
    /// before the set is cleared.
    /// </remarks>
    public void ClearChildren()
    {
        if (_children.Count == 0)
        {
            // This is an empty set already. Nothing to do.
            return;
        }

        if (_children[0]._owner == this)
        {
            // If the first child has this node as its owner, we assume that this
            // child set is unique.
            foreach (Node oldChild in _children)
            {
                oldChild.ResetLayoutResults(); // layout is no longer valid
                oldChild._owner = null;

                // Mark dirty to invalidate cache, but suppress the dirtied callback
                // since the node is being detached from the tree and should not
                // propagate dirty signals through external callback mechanisms.
                DirtiedFunc? dirtiedFunc = oldChild._dirtiedFunc;
                oldChild._dirtiedFunc = null;
                oldChild.SetDirty(true);
                oldChild._dirtiedFunc = dirtiedFunc;
            }
        }

        _children.Clear();
        _contentsChildrenCount = 0;
        MarkDirtyAndPropagate();
    }

    #endregion

    #region Position Calculation

    /// <summary>
    /// Sets the position values for all edges based on style and direction.
    /// </summary>
    public void SetPosition(Direction direction, float ownerWidth, float ownerHeight)
    {
        // Root nodes should be always laid out as LTR, so we don't return negative values.
        Direction directionRespectingRoot = _owner is not null ? direction : Direction.LTR;
        FlexDirection mainAxis = _style.FlexDirection.ResolveDirection(directionRespectingRoot);
        FlexDirection crossAxis = mainAxis.ResolveCrossDirection(directionRespectingRoot);

        // In the case of position static these are just 0. See:
        // https://www.w3.org/TR/css-position-3/#valdef-position-static
        float relativePositionMain = RelativePosition(
            mainAxis,
            directionRespectingRoot,
            mainAxis.IsRow() ? ownerWidth : ownerHeight);
        float relativePositionCross = RelativePosition(
            crossAxis,
            directionRespectingRoot,
            mainAxis.IsRow() ? ownerHeight : ownerWidth);

        PhysicalEdge mainAxisLeadingEdge = mainAxis.InlineStartEdge(direction);
        PhysicalEdge mainAxisTrailingEdge = mainAxis.InlineEndEdge(direction);
        PhysicalEdge crossAxisLeadingEdge = crossAxis.InlineStartEdge(direction);
        PhysicalEdge crossAxisTrailingEdge = crossAxis.InlineEndEdge(direction);

        SetLayoutPosition(
            _style.ComputeInlineStartMargin(mainAxis, direction, ownerWidth) + relativePositionMain,
            mainAxisLeadingEdge);
        SetLayoutPosition(
            _style.ComputeInlineEndMargin(mainAxis, direction, ownerWidth) + relativePositionMain,
            mainAxisTrailingEdge);
        SetLayoutPosition(
            _style.ComputeInlineStartMargin(crossAxis, direction, ownerWidth) + relativePositionCross,
            crossAxisLeadingEdge);
        SetLayoutPosition(
            _style.ComputeInlineEndMargin(crossAxis, direction, ownerWidth) + relativePositionCross,
            crossAxisTrailingEdge);
    }

    /// <summary>
    /// Calculates the relative position offset for the specified axis.
    /// </summary>
    private float RelativePosition(FlexDirection axis, Direction direction, float axisSize)
    {
        if (_style.PositionType == PositionType.Static)
        {
            return 0;
        }

        if (_style.IsInlineStartPositionDefined(axis, direction) &&
            !_style.IsInlineStartPositionAuto(axis, direction))
        {
            return _style.ComputeInlineStartPosition(axis, direction, axisSize);
        }

        return -1 * _style.ComputeInlineEndPosition(axis, direction, axisSize);
    }

    #endregion

    #region Flex Basis Processing

    /// <summary>
    /// Processes the flex basis style value.
    /// </summary>
    public StyleSizeLength ProcessFlexBasis()
    {
        StyleSizeLength flexBasis = _style.FlexBasis;
        if (!flexBasis.IsAuto && !flexBasis.IsUndefined)
        {
            return flexBasis;
        }

        if (_style.Flex.IsDefined && _style.Flex.Unwrap() > 0.0f)
        {
            return _config.UseWebDefaults
                ? StyleSizeLength.Auto
                : StyleSizeLength.Points(0);
        }

        return StyleSizeLength.Auto;
    }

    /// <summary>
    /// Resolves the flex basis value.
    /// </summary>
    public FloatOptional ResolveFlexBasis(
        Direction direction,
        FlexDirection flexDirection,
        float referenceLength,
        float ownerWidth)
    {
        FloatOptional value = ProcessFlexBasis().Resolve(referenceLength);
        if (_style.BoxSizing == BoxSizing.BorderBox)
        {
            return value;
        }

        Dimension dim = flexDirection.GetDimension();
        FloatOptional dimensionPaddingAndBorder = new(
            _style.ComputePaddingAndBorderForDimension(direction, dim, ownerWidth));

        return value + (dimensionPaddingAndBorder.IsDefined ? dimensionPaddingAndBorder : new FloatOptional(0.0f));
    }

    /// <summary>
    /// Processes dimensions, caching max == min cases.
    /// </summary>
    public void ProcessDimensions()
    {
        foreach (Dimension dim in new[] { Dimension.Width, Dimension.Height })
        {
            StyleSizeLength maxDim = _style.GetMaxDimension(dim);
            StyleSizeLength minDim = _style.GetMinDimension(dim);

            if (maxDim.IsDefined && minDim.InexactEquals(maxDim))
            {
                _processedDimensions[YogaEnums.ToUnderlying(dim)] = maxDim;
            }
            else
            {
                _processedDimensions[YogaEnums.ToUnderlying(dim)] = _style.GetDimension(dim);
            }
        }
    }

    #endregion

    #region Direction Resolution

    /// <summary>
    /// Resolves the direction for this node based on owner direction.
    /// </summary>
    public Direction ResolveDirection(Direction ownerDirection)
    {
        if (_style.Direction == Direction.Inherit)
        {
            return ownerDirection != Direction.Inherit ? ownerDirection : Direction.LTR;
        }

        return _style.Direction;
    }

    #endregion

    #region Dirty Propagation

    /// <summary>
    /// Marks this node as dirty and propagates to ancestors.
    /// </summary>
    public void MarkDirtyAndPropagate()
    {
        if (!_isDirty)
        {
            SetDirty(true);
            SetLayoutComputedFlexBasis(FloatOptional.Undefined);
            _owner?.MarkDirtyAndPropagate();
        }
    }

    #endregion

    #region Flex Resolution

    /// <summary>
    /// Resolves the effective flex-grow value.
    /// </summary>
    public float ResolveFlexGrow()
    {
        // Root nodes flexGrow should always be 0
        if (_owner is null)
        {
            return 0.0f;
        }

        if (_style.FlexGrow.IsDefined)
        {
            return _style.FlexGrow.Unwrap();
        }

        if (_style.Flex.IsDefined && _style.Flex.Unwrap() > 0.0f)
        {
            return _style.Flex.Unwrap();
        }

        return Style.DefaultFlexGrow;
    }

    /// <summary>
    /// Resolves the effective flex-shrink value.
    /// </summary>
    public float ResolveFlexShrink()
    {
        if (_owner is null)
        {
            return 0.0f;
        }

        if (_style.FlexShrink.IsDefined)
        {
            return _style.FlexShrink.Unwrap();
        }

        if (!_config.UseWebDefaults && _style.Flex.IsDefined && _style.Flex.Unwrap() < 0.0f)
        {
            return -_style.Flex.Unwrap();
        }

        return _config.UseWebDefaults ? Style.WebDefaultFlexShrink : Style.DefaultFlexShrink;
    }

    /// <summary>
    /// Gets whether this node is flexible (can grow or shrink).
    /// </summary>
    public bool IsNodeFlexible()
    {
        return _style.PositionType != PositionType.Absolute &&
               (ResolveFlexGrow() != 0 || ResolveFlexShrink() != 0);
    }

    #endregion

    #region Cloning

    /// <summary>
    /// Clones children if they are not owned by this node.
    /// </summary>
    public void CloneChildrenIfNeeded()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            Node child = _children[i];
            if (child.Owner != this)
            {
                Node? clonedChild = _config.CloneNode(child, this, i) as Node;
                clonedChild ??= child.Clone();
                clonedChild.Owner = this;

                if (clonedChild.HasContentsChildren)
                {
                    clonedChild.CloneContentsChildrenIfNeeded();
                }

                _children[i] = clonedChild;
            }
        }
    }

    /// <summary>
    /// Clones display:contents children if they are not owned by this node.
    /// </summary>
    public void CloneContentsChildrenIfNeeded()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            Node child = _children[i];
            if (child._style.Display == Display.Contents && child.Owner != this)
            {
                Node? clonedChild = _config.CloneNode(child, this, i) as Node;
                clonedChild ??= child.Clone();
                clonedChild.Owner = this;
                clonedChild.CloneChildrenIfNeeded();

                _children[i] = clonedChild;
            }
        }
    }

    /// <summary>
    /// Creates a clone of this node.
    /// </summary>
    public Node Clone()
    {
        return new Node(this);
    }

    /// <summary>
    /// Resets this node to its initial state.
    /// </summary>
    /// <exception cref="YogaAssertException">Thrown if the node has children or an owner.</exception>
    public void Reset()
    {
        YogaAssert.Assert(this, _children.Count == 0,
            "Cannot reset a node which still has children attached");
        YogaAssert.Assert(this, _owner is null,
            "Cannot reset a node still attached to an owner");

        // Reset to default state with current config
        Config currentConfig = _config;

        _hasNewLayout = true;
        _isReferenceBaseline = false;
        _isDirty = true;
        _alwaysFormsContainingBlock = false;
        _nodeType = NodeType.Default;
        _context = null;
        _measureFunc = null;
        _baselineFunc = null;
        _dirtiedFunc = null;
        _lineIndex = 0;
        _contentsChildrenCount = 0;
        _owner = null;
        _children.Clear();
        _config = currentConfig;

        // Reset style to default
        ResetStyleToDefault();

        // Reset layout
        ResetLayoutResults();

        // Reset processed dimensions
        _processedDimensions[0] = StyleSizeLength.Undefined;
        _processedDimensions[1] = StyleSizeLength.Undefined;

        if (_config.UseWebDefaults)
        {
            UseWebDefaults();
        }
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Applies web defaults to the style.
    /// </summary>
    private void UseWebDefaults()
    {
        _style.FlexDirection = FlexDirection.Row;
        _style.AlignContent = Align.Stretch;
    }

    /// <summary>
    /// Copies style properties from another style.
    /// </summary>
    private void CopyStyleFrom(Style other)
    {
        _style.Direction = other.Direction;
        _style.FlexDirection = other.FlexDirection;
        _style.JustifyContent = other.JustifyContent;
        _style.AlignContent = other.AlignContent;
        _style.AlignItems = other.AlignItems;
        _style.AlignSelf = other.AlignSelf;
        _style.PositionType = other.PositionType;
        _style.FlexWrap = other.FlexWrap;
        _style.Overflow = other.Overflow;
        _style.Display = other.Display;
        _style.BoxSizing = other.BoxSizing;
        _style.Flex = other.Flex;
        _style.FlexGrow = other.FlexGrow;
        _style.FlexShrink = other.FlexShrink;
        _style.FlexBasis = other.FlexBasis;
        _style.AspectRatio = other.AspectRatio;

        // Copy edge values
        foreach (Edge edge in YogaEnums.Ordinals<Edge>())
        {
            _style.SetMargin(edge, other.GetMargin(edge));
            _style.SetPosition(edge, other.GetPosition(edge));
            _style.SetPadding(edge, other.GetPadding(edge));
            _style.SetBorder(edge, other.GetBorder(edge));
        }

        // Copy gutter values
        foreach (Gutter gutter in YogaEnums.Ordinals<Gutter>())
        {
            _style.SetGap(gutter, other.GetGap(gutter));
        }

        // Copy dimension values
        foreach (Dimension dim in YogaEnums.Ordinals<Dimension>())
        {
            _style.SetDimension(dim, other.GetDimension(dim));
            _style.SetMinDimension(dim, other.GetMinDimension(dim));
            _style.SetMaxDimension(dim, other.GetMaxDimension(dim));
        }
    }

    /// <summary>
    /// Copies layout results from another instance.
    /// </summary>
    private void CopyLayoutFrom(LayoutResults other)
    {
        _layout.SetDirection(other.Direction);
        _layout.SetHadOverflow(other.HadOverflow);
        _layout.LastOwnerDirection = other.LastOwnerDirection;
        _layout.ConfigVersion = other.ConfigVersion;
        _layout.ComputedFlexBasis = other.ComputedFlexBasis;
        _layout.ComputedFlexBasisGeneration = other.ComputedFlexBasisGeneration;
        _layout.GenerationCount = other.GenerationCount;
        _layout.NextCachedMeasurementsIndex = other.NextCachedMeasurementsIndex;
        _layout.CachedLayout = other.CachedLayout;

        foreach (PhysicalEdge edge in YogaEnums.Ordinals<PhysicalEdge>())
        {
            _layout.SetPosition(edge, other.GetPosition(edge));
            _layout.SetMargin(edge, other.GetMargin(edge));
            _layout.SetBorder(edge, other.GetBorder(edge));
            _layout.SetPadding(edge, other.GetPadding(edge));
        }

        foreach (Dimension dim in YogaEnums.Ordinals<Dimension>())
        {
            _layout.SetDimension(dim, other.GetDimension(dim));
            _layout.SetMeasuredDimension(dim, other.GetMeasuredDimension(dim));
            _layout.SetRawDimension(dim, other.GetRawDimension(dim));
        }

        for (int i = 0; i < LayoutResults.MaxCachedMeasurements; i++)
        {
            _layout.SetCachedMeasurement(i, other.GetCachedMeasurement(i));
        }
    }

    /// <summary>
    /// Resets style to default values.
    /// </summary>
    private void ResetStyleToDefault()
    {
        _style.Direction = Direction.Inherit;
        _style.FlexDirection = FlexDirection.Column;
        _style.JustifyContent = Justify.FlexStart;
        _style.AlignContent = Align.FlexStart;
        _style.AlignItems = Align.Stretch;
        _style.AlignSelf = Align.Auto;
        _style.PositionType = PositionType.Relative;
        _style.FlexWrap = Wrap.NoWrap;
        _style.Overflow = Overflow.Visible;
        _style.Display = Display.Flex;
        _style.BoxSizing = BoxSizing.BorderBox;
        _style.Flex = FloatOptional.Undefined;
        _style.FlexGrow = FloatOptional.Undefined;
        _style.FlexShrink = FloatOptional.Undefined;
        _style.FlexBasis = StyleSizeLength.Auto;
        _style.AspectRatio = FloatOptional.Undefined;

        foreach (Edge edge in YogaEnums.Ordinals<Edge>())
        {
            _style.SetMargin(edge, StyleLength.Undefined);
            _style.SetPosition(edge, StyleLength.Undefined);
            _style.SetPadding(edge, StyleLength.Undefined);
            _style.SetBorder(edge, StyleLength.Undefined);
        }

        foreach (Gutter gutter in YogaEnums.Ordinals<Gutter>())
        {
            _style.SetGap(gutter, StyleLength.Undefined);
        }

        foreach (Dimension dim in YogaEnums.Ordinals<Dimension>())
        {
            _style.SetDimension(dim, StyleSizeLength.Auto);
            _style.SetMinDimension(dim, StyleSizeLength.Undefined);
            _style.SetMaxDimension(dim, StyleSizeLength.Undefined);
        }
    }

    /// <summary>
    /// Resets layout results to default values.
    /// </summary>
    private void ResetLayoutResults()
    {
        _layout.SetDirection(Direction.Inherit);
        _layout.SetHadOverflow(false);
        _layout.LastOwnerDirection = Direction.Inherit;
        _layout.ConfigVersion = 0;
        _layout.ComputedFlexBasis = FloatOptional.Undefined;
        _layout.ComputedFlexBasisGeneration = 0;
        _layout.GenerationCount = 0;
        _layout.NextCachedMeasurementsIndex = 0;
        _layout.CachedLayout = default;

        foreach (PhysicalEdge edge in YogaEnums.Ordinals<PhysicalEdge>())
        {
            _layout.SetPosition(edge, 0);
            _layout.SetMargin(edge, 0);
            _layout.SetBorder(edge, 0);
            _layout.SetPadding(edge, 0);
        }

        foreach (Dimension dim in YogaEnums.Ordinals<Dimension>())
        {
            _layout.SetDimension(dim, float.NaN);
            _layout.SetMeasuredDimension(dim, float.NaN);
            _layout.SetRawDimension(dim, float.NaN);
        }

        for (int i = 0; i < LayoutResults.MaxCachedMeasurements; i++)
        {
            _layout.SetCachedMeasurement(i, default);
        }
    }

    #endregion
}
