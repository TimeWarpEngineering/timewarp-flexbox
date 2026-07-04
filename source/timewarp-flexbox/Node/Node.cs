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

  // Measure callback (no public property; exposed via HasMeasureFunc/SetMeasureFunc)
  private MeasureFunc? MeasureFunc;

  // Tree structure
  private int ContentsChildrenCount;
  private readonly List<Node> ChildrenInternal = [];

  // Processed dimensions cache
  private readonly StyleSizeLength[] ProcessedDimensions =
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
    Config = config;

    if (Config.UseWebDefaults)
    {
      UseWebDefaults();
    }

    // Attach after initialization so constructing a node never dirties it.
    Style.OwnerNode = this;

    YogaEvent.PublishNodeAllocation(this, Config);
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
    HasNewLayout = other.HasNewLayout;
    IsReferenceBaseline = other.IsReferenceBaseline;
    IsDirty = other.IsDirty;
    AlwaysFormsContainingBlock = other.AlwaysFormsContainingBlock;
    NodeType = other.NodeType;
    Context = other.Context;
    MeasureFunc = other.MeasureFunc;
    BaselineFunc = other.BaselineFunc;
    DirtiedFunc = other.DirtiedFunc;
    LineIndex = other.LineIndex;
    ContentsChildrenCount = other.ContentsChildrenCount;
    Owner = other.Owner;
    Config = other.Config;

    // Copy style properties
    CopyStyleFrom(other.Style);

    // Copy layout results
    CopyLayoutFrom(other.Layout);

    // Copy processed dimensions
    Array.Copy(other.ProcessedDimensions, ProcessedDimensions, 2);

    // Shallow copy children list
    ChildrenInternal.AddRange(other.ChildrenInternal);

    // Attach after copying so cloning never dirties the clone.
    Style.OwnerNode = this;

    YogaEvent.PublishNodeAllocation(this, Config);
  }

  #endregion

  #region ILayoutableNode Implementation

  /// <inheritdoc />
  ILayoutableNode ILayoutableNode.GetChild(int index) => ChildrenInternal[index];

  /// <inheritdoc />
  public int GetChildCount() => ChildrenInternal.Count;

  /// <inheritdoc />
  Display ILayoutableNode.GetDisplay() => Style.Display;

  /// <summary>
  /// Computes the layout for this node tree. Convenience for
  /// <see cref="TimeWarp.Flexbox.CalculateLayout.Calculate"/>.
  /// </summary>
  /// <param name="availableWidth">Available width, or <see cref="float.NaN"/> for unconstrained.</param>
  /// <param name="availableHeight">Available height, or <see cref="float.NaN"/> for unconstrained.</param>
  /// <param name="ownerDirection">The layout direction (LTR or RTL).</param>
  public void CalculateLayout(
      float availableWidth = float.NaN,
      float availableHeight = float.NaN,
      Direction ownerDirection = Direction.LTR) =>
    TimeWarp.Flexbox.CalculateLayout.Calculate(this, availableWidth, availableHeight, ownerDirection);

  #endregion

  #region Properties - Getters

  /// <summary>
  /// Gets or sets the user context object.
  /// </summary>
  public object? Context { get; set; }

  /// <summary>
  /// Gets or sets whether this node always forms a containing block for
  /// absolutely positioned descendants.
  /// </summary>
  public bool AlwaysFormsContainingBlock { get; set; }

  /// <summary>
  /// Gets whether this node has new layout results that haven't been read yet.
  /// </summary>
  public bool HasNewLayout { get; set; } = true;

  /// <summary>
  /// Gets or sets the node type.
  /// </summary>
  public NodeType NodeType { get; set; } = NodeType.Default;

  /// <summary>
  /// Gets whether this node has a measure function.
  /// </summary>
  public bool HasMeasureFunc => MeasureFunc is not null;

  /// <summary>
  /// Gets whether this node has a baseline function.
  /// </summary>
  public bool HasBaselineFunc => BaselineFunc is not null;

  /// <summary>
  /// Gets whether this node has the specified errata enabled.
  /// </summary>
  public bool HasErrata(Errata errata) => Config.HasErrata(errata);

  /// <summary>
  /// Gets whether this node has any display:contents children.
  /// </summary>
  public bool HasContentsChildren => ContentsChildrenCount != 0;

  /// <summary>
  /// Gets or sets the dirtied callback function.
  /// </summary>
  public DirtiedFunc? DirtiedFunc { get; set; }

  /// <summary>
  /// Gets the style for this node.
  /// </summary>
  public Style Style { get; } = new();

  /// <summary>
  /// Gets the layout results for this node.
  /// </summary>
  public LayoutResults Layout { get; } = new();

  /// <summary>
  /// Gets or sets the line index (for flex wrapping).
  /// </summary>
  public int LineIndex { get; set; }

  /// <summary>
  /// Gets or sets whether this node is the reference baseline for its siblings.
  /// </summary>
  public bool IsReferenceBaseline { get; set; }

  /// <summary>
  /// Gets the owner node (the node that owns this one in the tree).
  /// </summary>
  /// <remarks>
  /// An owner is used to identify the YogaTree that a Node belongs to.
  /// This method will return the parent of the Node when a Node only belongs
  /// to one YogaTree or null when the Node is shared between two or more YogaTrees.
  /// </remarks>
  public Node? Owner { get; set; }

  /// <summary>
  /// Gets the read-only list of children.
  /// </summary>
  public IReadOnlyList<Node> Children => ChildrenInternal;

  /// <summary>
  /// Gets the child at the specified index.
  /// </summary>
  public Node GetChild(int index) => ChildrenInternal[index];

  // Note: ChildCount property removed to avoid CA1721 conflict with GetChildCount() from ILayoutableNode

  /// <summary>
  /// Gets an enumerable over layoutable children (handling display:contents).
  /// </summary>
  internal LayoutableChildren<Node> LayoutChildren => new(this);

  /// <summary>
  /// Gets the count of layoutable children (handling display:contents).
  /// </summary>
  public int LayoutChildCount
  {
    get
    {
      if (ContentsChildrenCount == 0)
      {
        return ChildrenInternal.Count;
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
  public Config Config { get; private set; }

  /// <summary>
  /// Gets whether this node is dirty and needs layout recalculation.
  /// </summary>
  public bool IsDirty { get; private set; } = true;

  /// <summary>
  /// Gets the processed dimension for the specified axis.
  /// </summary>
  public StyleSizeLength GetProcessedDimension(Dimension dimension) =>
      ProcessedDimensions[YogaEnums.ToUnderlying(dimension)];

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
    if (Style.BoxSizing == BoxSizing.BorderBox)
    {
      return value;
    }

    FloatOptional dimensionPaddingAndBorder = new(
        Style.ComputePaddingAndBorderForDimension(direction, dimension, ownerWidth));

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
    YGSize size = MeasureFunc!(this, availableWidth, widthMode, availableHeight, heightMode);

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
    return BaselineFunc!(this, width, height);
  }

  /// <summary>
  /// Gets the dimension with margin for the specified axis.
  /// </summary>
  public float DimensionWithMargin(FlexDirection axis, float widthSize)
  {
    return Layout.GetMeasuredDimension(axis.GetDimension()) +
           Style.ComputeMarginForAxis(axis, widthSize);
  }

  /// <summary>
  /// Gets whether the layout dimension is defined for the specified axis.
  /// </summary>
  public bool IsLayoutDimensionDefined(FlexDirection axis)
  {
    float value = Layout.GetMeasuredDimension(axis.GetDimension());
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
      NodeType = NodeType.Default;
    }
    else
    {
      YogaAssert.Assert(this, ChildrenInternal.Count == 0,
          "Cannot set measure function: Nodes with measure functions cannot have children.");
      NodeType = NodeType.Text;
    }

    MeasureFunc = measureFunc;
  }

  /// <summary>
  /// Gets or sets the baseline function.
  /// </summary>
  public BaselineFunc? BaselineFunc { get; set; }

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
    YogaAssert.Assert(Config, config.UseWebDefaults == Config.UseWebDefaults,
        "UseWebDefaults may not be changed after constructing a Node");

    if (Config.ConfigUpdateInvalidatesLayout(Config, config))
    {
      MarkDirtyAndPropagate();
      Layout.ConfigVersion = 0;
    }
    else
    {
      // If the config is functionally the same, then align the configVersion so
      // that we can reuse the layout cache
      Layout.ConfigVersion = config.Version;
    }

    Config = config;
  }

  /// <summary>
  /// Sets the dirty state of this node.
  /// </summary>
  public void SetDirty(bool isDirty)
  {
    if (isDirty == IsDirty)
    {
      return;
    }

    IsDirty = isDirty;
    if (isDirty && DirtiedFunc is not null)
    {
      DirtiedFunc(this);
    }
  }

  /// <summary>
  /// Sets all children, replacing any existing children.
  /// </summary>
  public void SetChildren(IEnumerable<Node> children)
  {
    ChildrenInternal.Clear();
    ChildrenInternal.AddRange(children);

    ContentsChildrenCount = 0;
    foreach (Node child in ChildrenInternal)
    {
      if (child.Style.Display == Display.Contents)
      {
        ContentsChildrenCount++;
      }
    }
  }

  /// <summary>
  /// Sets the layout's last owner direction.
  /// </summary>
  public void SetLayoutLastOwnerDirection(Direction direction)
  {
    Layout.LastOwnerDirection = direction;
  }

  /// <summary>
  /// Sets the computed flex basis.
  /// </summary>
  public void SetLayoutComputedFlexBasis(FloatOptional computedFlexBasis)
  {
    Layout.ComputedFlexBasis = computedFlexBasis;
  }

  /// <summary>
  /// Sets the computed flex basis generation counter.
  /// </summary>
  public void SetLayoutComputedFlexBasisGeneration(uint generation)
  {
    Layout.ComputedFlexBasisGeneration = generation;
  }

  /// <summary>
  /// Sets the measured dimension for the specified axis.
  /// </summary>
  public void SetLayoutMeasuredDimension(float measuredDimension, Dimension dimension)
  {
    Layout.SetMeasuredDimension(dimension, measuredDimension);
  }

  /// <summary>
  /// Sets whether the layout had overflow.
  /// </summary>
  public void SetLayoutHadOverflow(bool hadOverflow)
  {
    Layout.SetHadOverflow(hadOverflow);
  }

  /// <summary>
  /// Sets the layout dimension for the specified axis.
  /// </summary>
  public void SetLayoutDimension(float lengthValue, Dimension dimension)
  {
    Layout.SetDimension(dimension, lengthValue);
    Layout.SetRawDimension(dimension, lengthValue);
  }

  /// <summary>
  /// Sets the layout direction.
  /// </summary>
  public void SetLayoutDirection(Direction direction)
  {
    Layout.SetDirection(direction);
  }

  /// <summary>
  /// Sets the layout margin for the specified edge.
  /// </summary>
  public void SetLayoutMargin(float margin, PhysicalEdge edge)
  {
    Layout.SetMargin(edge, margin);
  }

  /// <summary>
  /// Sets the layout border for the specified edge.
  /// </summary>
  public void SetLayoutBorder(float border, PhysicalEdge edge)
  {
    Layout.SetBorder(edge, border);
  }

  /// <summary>
  /// Sets the layout padding for the specified edge.
  /// </summary>
  public void SetLayoutPadding(float padding, PhysicalEdge edge)
  {
    Layout.SetPadding(edge, padding);
  }

  /// <summary>
  /// Sets the layout position for the specified edge.
  /// </summary>
  public void SetLayoutPosition(float position, PhysicalEdge edge)
  {
    Layout.SetPosition(edge, position);
  }

  #endregion

  #region Child Management

  /// <summary>
  /// Replaces the child at the specified index.
  /// </summary>
  public void ReplaceChild(Node child, int index)
  {
    ArgumentNullException.ThrowIfNull(child);
    Node previousChild = ChildrenInternal[index];
    if (previousChild.Style.Display == Display.Contents &&
        child.Style.Display != Display.Contents)
    {
      ContentsChildrenCount--;
    }
    else if (previousChild.Style.Display != Display.Contents &&
             child.Style.Display == Display.Contents)
    {
      ContentsChildrenCount++;
    }

    ChildrenInternal[index] = child;
    child.Owner = this;
  }

  /// <summary>
  /// Replaces oldChild with newChild.
  /// </summary>
  public void ReplaceChild(Node oldChild, Node newChild)
  {
    ArgumentNullException.ThrowIfNull(oldChild);
    ArgumentNullException.ThrowIfNull(newChild);
    if (oldChild.Style.Display == Display.Contents &&
        newChild.Style.Display != Display.Contents)
    {
      ContentsChildrenCount--;
    }
    else if (oldChild.Style.Display != Display.Contents &&
             newChild.Style.Display == Display.Contents)
    {
      ContentsChildrenCount++;
    }

    int index = ChildrenInternal.IndexOf(oldChild);
    if (index >= 0)
    {
      ChildrenInternal[index] = newChild;
      newChild.Owner = this;
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
    YogaAssert.Assert(this, child.Owner is null,
        "Child already has a owner, it must be removed first.");
    YogaAssert.Assert(this, !HasMeasureFunc,
        "Cannot add child: Nodes with measure functions cannot have children.");

    if (child.Style.Display == Display.Contents)
    {
      ContentsChildrenCount++;
    }

    ChildrenInternal.Insert(index, child);
    child.Owner = this;
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
    if (ChildrenInternal.Count == 0)
    {
      // This is an empty set. Nothing to remove.
      return false;
    }

    Node? childOwner = child.Owner;
    int index = ChildrenInternal.IndexOf(child);
    if (index >= 0)
    {
      if (child.Style.Display == Display.Contents)
      {
        ContentsChildrenCount--;
      }

      ChildrenInternal.RemoveAt(index);

      if (childOwner == this)
      {
        child.ResetLayoutResults(); // layout is no longer valid
        child.Owner = null;

        // Mark dirty to invalidate cache, but suppress the dirtied callback
        // since the node is being detached from the tree and should not
        // propagate dirty signals through external callback mechanisms.
        DirtiedFunc? dirtiedFunc = child.DirtiedFunc;
        child.DirtiedFunc = null;
        child.SetDirty(true);
        child.DirtiedFunc = dirtiedFunc;
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
    if (ChildrenInternal[index].Style.Display == Display.Contents)
    {
      ContentsChildrenCount--;
    }

    ChildrenInternal.RemoveAt(index);
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
    if (ChildrenInternal.Count == 0)
    {
      // This is an empty set already. Nothing to do.
      return;
    }

    if (ChildrenInternal[0].Owner == this)
    {
      // If the first child has this node as its owner, we assume that this
      // child set is unique.
      foreach (Node oldChild in ChildrenInternal)
      {
        oldChild.ResetLayoutResults(); // layout is no longer valid
        oldChild.Owner = null;

        // Mark dirty to invalidate cache, but suppress the dirtied callback
        // since the node is being detached from the tree and should not
        // propagate dirty signals through external callback mechanisms.
        DirtiedFunc? dirtiedFunc = oldChild.DirtiedFunc;
        oldChild.DirtiedFunc = null;
        oldChild.SetDirty(true);
        oldChild.DirtiedFunc = dirtiedFunc;
      }
    }

    ChildrenInternal.Clear();
    ContentsChildrenCount = 0;
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
    Direction directionRespectingRoot = Owner is not null ? direction : Direction.LTR;
    FlexDirection mainAxis = Style.FlexDirection.ResolveDirection(directionRespectingRoot);
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
        Style.ComputeInlineStartMargin(mainAxis, direction, ownerWidth) + relativePositionMain,
        mainAxisLeadingEdge);
    SetLayoutPosition(
        Style.ComputeInlineEndMargin(mainAxis, direction, ownerWidth) + relativePositionMain,
        mainAxisTrailingEdge);
    SetLayoutPosition(
        Style.ComputeInlineStartMargin(crossAxis, direction, ownerWidth) + relativePositionCross,
        crossAxisLeadingEdge);
    SetLayoutPosition(
        Style.ComputeInlineEndMargin(crossAxis, direction, ownerWidth) + relativePositionCross,
        crossAxisTrailingEdge);
  }

  /// <summary>
  /// Calculates the relative position offset for the specified axis.
  /// </summary>
  private float RelativePosition(FlexDirection axis, Direction direction, float axisSize)
  {
    if (Style.PositionType == PositionType.Static)
    {
      return 0;
    }

    if (Style.IsInlineStartPositionDefined(axis, direction) &&
        !Style.IsInlineStartPositionAuto(axis, direction))
    {
      return Style.ComputeInlineStartPosition(axis, direction, axisSize);
    }

    return -1 * Style.ComputeInlineEndPosition(axis, direction, axisSize);
  }

  #endregion

  #region Flex Basis Processing

  /// <summary>
  /// Processes the flex basis style value.
  /// </summary>
  public StyleSizeLength ProcessFlexBasis()
  {
    StyleSizeLength flexBasis = Style.FlexBasis;
    if (!flexBasis.IsAuto && !flexBasis.IsUndefined)
    {
      return flexBasis;
    }

    if (Style.Flex.IsDefined && Style.Flex.Unwrap() > 0.0f)
    {
      return Config.UseWebDefaults
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
    if (Style.BoxSizing == BoxSizing.BorderBox)
    {
      return value;
    }

    Dimension dim = flexDirection.GetDimension();
    FloatOptional dimensionPaddingAndBorder = new(
        Style.ComputePaddingAndBorderForDimension(direction, dim, ownerWidth));

    return value + (dimensionPaddingAndBorder.IsDefined ? dimensionPaddingAndBorder : new FloatOptional(0.0f));
  }

  /// <summary>
  /// Processes dimensions, caching max == min cases.
  /// </summary>
  public void ProcessDimensions()
  {
    foreach (Dimension dim in new[] { Dimension.Width, Dimension.Height })
    {
      StyleSizeLength maxDim = Style.GetMaxDimension(dim);
      StyleSizeLength minDim = Style.GetMinDimension(dim);

      if (maxDim.IsDefined && minDim.InexactEquals(maxDim))
      {
        ProcessedDimensions[YogaEnums.ToUnderlying(dim)] = maxDim;
      }
      else
      {
        ProcessedDimensions[YogaEnums.ToUnderlying(dim)] = Style.GetDimension(dim);
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
    if (Style.Direction == Direction.Inherit)
    {
      return ownerDirection != Direction.Inherit ? ownerDirection : Direction.LTR;
    }

    return Style.Direction;
  }

  #endregion

  #region Dirty Propagation

  /// <summary>
  /// Marks this node as dirty and propagates to ancestors.
  /// </summary>
  public void MarkDirtyAndPropagate()
  {
    if (!IsDirty)
    {
      SetDirty(true);
      SetLayoutComputedFlexBasis(FloatOptional.Undefined);
      Owner?.MarkDirtyAndPropagate();
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
    if (Owner is null)
    {
      return 0.0f;
    }

    if (Style.FlexGrow.IsDefined)
    {
      return Style.FlexGrow.Unwrap();
    }

    if (Style.Flex.IsDefined && Style.Flex.Unwrap() > 0.0f)
    {
      return Style.Flex.Unwrap();
    }

    return Style.DefaultFlexGrow;
  }

  /// <summary>
  /// Resolves the effective flex-shrink value.
  /// </summary>
  public float ResolveFlexShrink()
  {
    if (Owner is null)
    {
      return 0.0f;
    }

    if (Style.FlexShrink.IsDefined)
    {
      return Style.FlexShrink.Unwrap();
    }

    if (!Config.UseWebDefaults && Style.Flex.IsDefined && Style.Flex.Unwrap() < 0.0f)
    {
      return -Style.Flex.Unwrap();
    }

    return Config.UseWebDefaults ? Style.WebDefaultFlexShrink : Style.DefaultFlexShrink;
  }

  /// <summary>
  /// Gets whether this node is flexible (can grow or shrink).
  /// </summary>
  public bool IsNodeFlexible()
  {
    return Style.PositionType != PositionType.Absolute &&
           (ResolveFlexGrow() != 0 || ResolveFlexShrink() != 0);
  }

  #endregion

  #region Cloning

  /// <summary>
  /// Clones children if they are not owned by this node.
  /// </summary>
  public void CloneChildrenIfNeeded()
  {
    for (int i = 0; i < ChildrenInternal.Count; i++)
    {
      Node child = ChildrenInternal[i];
      if (child.Owner != this)
      {
        Node? clonedChild = Config.CloneNode(child, this, i) as Node;
        clonedChild ??= child.Clone();
        clonedChild.Owner = this;

        if (clonedChild.Style.Display == Display.Contents)
        {
          // The contents node's children are treated as children of the
          // contents node's parent for layout purposes, so they need
          // to be cloned as well.
          clonedChild.CloneChildrenIfNeeded();
        }
        else if (clonedChild.HasContentsChildren)
        {
          clonedChild.CloneContentsChildrenIfNeeded();
        }

        ChildrenInternal[i] = clonedChild;
      }
    }
  }

  /// <summary>
  /// Clones display:contents children if they are not owned by this node.
  /// </summary>
  public void CloneContentsChildrenIfNeeded()
  {
    for (int i = 0; i < ChildrenInternal.Count; i++)
    {
      Node child = ChildrenInternal[i];
      if (child.Style.Display == Display.Contents && child.Owner != this)
      {
        Node? clonedChild = Config.CloneNode(child, this, i) as Node;
        clonedChild ??= child.Clone();
        clonedChild.Owner = this;
        clonedChild.CloneChildrenIfNeeded();

        ChildrenInternal[i] = clonedChild;
      }
    }
  }

  /// <summary>
  /// Creates a clone of this node.
  /// </summary>
  public Node Clone()
  {
    // C++ YGNodeClone clears the owner: a clone starts detached; callers
    // (including CloneChildrenIfNeeded) re-parent it explicitly.
    Node clone = new(this);
    clone.Owner = null;
    return clone;
  }

  /// <summary>
  /// Resets this node to its initial state.
  /// </summary>
  /// <exception cref="YogaAssertException">Thrown if the node has children or an owner.</exception>
  public void Reset()
  {
    YogaAssert.Assert(this, ChildrenInternal.Count == 0,
        "Cannot reset a node which still has children attached");
    YogaAssert.Assert(this, Owner is null,
        "Cannot reset a node still attached to an owner");

    // Reset to default state with current config
    Config currentConfig = Config;

    HasNewLayout = true;
    IsReferenceBaseline = false;
    IsDirty = true;
    AlwaysFormsContainingBlock = false;
    NodeType = NodeType.Default;
    Context = null;
    MeasureFunc = null;
    BaselineFunc = null;
    DirtiedFunc = null;
    LineIndex = 0;
    ContentsChildrenCount = 0;
    Owner = null;
    ChildrenInternal.Clear();
    Config = currentConfig;

    // Reset style to default
    ResetStyleToDefault();

    // Reset layout
    ResetLayoutResults();

    // Reset processed dimensions
    ProcessedDimensions[0] = StyleSizeLength.Undefined;
    ProcessedDimensions[1] = StyleSizeLength.Undefined;

    if (Config.UseWebDefaults)
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
    Style.FlexDirection = FlexDirection.Row;
    Style.AlignContent = Align.Stretch;
  }

  /// <summary>
  /// Copies style properties from another style.
  /// </summary>
  private void CopyStyleFrom(Style other)
  {
    Style.Direction = other.Direction;
    Style.FlexDirection = other.FlexDirection;
    Style.JustifyContent = other.JustifyContent;
    Style.AlignContent = other.AlignContent;
    Style.AlignItems = other.AlignItems;
    Style.AlignSelf = other.AlignSelf;
    Style.PositionType = other.PositionType;
    Style.FlexWrap = other.FlexWrap;
    Style.Overflow = other.Overflow;
    Style.Display = other.Display;
    Style.BoxSizing = other.BoxSizing;
    Style.Flex = other.Flex;
    Style.FlexGrow = other.FlexGrow;
    Style.FlexShrink = other.FlexShrink;
    Style.FlexBasis = other.FlexBasis;
    Style.AspectRatio = other.AspectRatio;

    // Copy edge values
    foreach (Edge edge in YogaEnums.Ordinals<Edge>())
    {
      Style.SetMargin(edge, other.GetMargin(edge));
      Style.SetPosition(edge, other.GetPosition(edge));
      Style.SetPadding(edge, other.GetPadding(edge));
      Style.SetBorder(edge, other.GetBorder(edge));
    }

    // Copy gutter values
    foreach (Gutter gutter in YogaEnums.Ordinals<Gutter>())
    {
      Style.SetGap(gutter, other.GetGap(gutter));
    }

    // Copy dimension values
    foreach (Dimension dim in YogaEnums.Ordinals<Dimension>())
    {
      Style.SetDimension(dim, other.GetDimension(dim));
      Style.SetMinDimension(dim, other.GetMinDimension(dim));
      Style.SetMaxDimension(dim, other.GetMaxDimension(dim));
    }
  }

  /// <summary>
  /// Copies layout results from another instance.
  /// </summary>
  private void CopyLayoutFrom(LayoutResults other)
  {
    Layout.SetDirection(other.Direction);
    Layout.SetHadOverflow(other.HadOverflow);
    Layout.LastOwnerDirection = other.LastOwnerDirection;
    Layout.ConfigVersion = other.ConfigVersion;
    Layout.ComputedFlexBasis = other.ComputedFlexBasis;
    Layout.ComputedFlexBasisGeneration = other.ComputedFlexBasisGeneration;
    Layout.GenerationCount = other.GenerationCount;
    Layout.NextCachedMeasurementsIndex = other.NextCachedMeasurementsIndex;
    Layout.CachedLayout = other.CachedLayout;

    foreach (PhysicalEdge edge in YogaEnums.Ordinals<PhysicalEdge>())
    {
      Layout.SetPosition(edge, other.GetPosition(edge));
      Layout.SetMargin(edge, other.GetMargin(edge));
      Layout.SetBorder(edge, other.GetBorder(edge));
      Layout.SetPadding(edge, other.GetPadding(edge));
    }

    foreach (Dimension dim in YogaEnums.Ordinals<Dimension>())
    {
      Layout.SetDimension(dim, other.GetDimension(dim));
      Layout.SetMeasuredDimension(dim, other.GetMeasuredDimension(dim));
      Layout.SetRawDimension(dim, other.GetRawDimension(dim));
    }

    for (int i = 0; i < LayoutResults.MaxCachedMeasurements; i++)
    {
      Layout.SetCachedMeasurement(i, other.GetCachedMeasurement(i));
    }
  }

  /// <summary>
  /// Resets style to default values.
  /// </summary>
  private void ResetStyleToDefault()
  {
    Style.Direction = Direction.Inherit;
    Style.FlexDirection = FlexDirection.Column;
    Style.JustifyContent = Justify.FlexStart;
    Style.AlignContent = Align.FlexStart;
    Style.AlignItems = Align.Stretch;
    Style.AlignSelf = Align.Auto;
    Style.PositionType = PositionType.Relative;
    Style.FlexWrap = Wrap.NoWrap;
    Style.Overflow = Overflow.Visible;
    Style.Display = Display.Flex;
    Style.BoxSizing = BoxSizing.BorderBox;
    Style.Flex = FloatOptional.Undefined;
    Style.FlexGrow = FloatOptional.Undefined;
    Style.FlexShrink = FloatOptional.Undefined;
    Style.FlexBasis = StyleSizeLength.Auto;
    Style.AspectRatio = FloatOptional.Undefined;

    foreach (Edge edge in YogaEnums.Ordinals<Edge>())
    {
      Style.SetMargin(edge, StyleLength.Undefined);
      Style.SetPosition(edge, StyleLength.Undefined);
      Style.SetPadding(edge, StyleLength.Undefined);
      Style.SetBorder(edge, StyleLength.Undefined);
    }

    foreach (Gutter gutter in YogaEnums.Ordinals<Gutter>())
    {
      Style.SetGap(gutter, StyleLength.Undefined);
    }

    foreach (Dimension dim in YogaEnums.Ordinals<Dimension>())
    {
      Style.SetDimension(dim, StyleSizeLength.Auto);
      Style.SetMinDimension(dim, StyleSizeLength.Undefined);
      Style.SetMaxDimension(dim, StyleSizeLength.Undefined);
    }
  }

  /// <summary>
  /// Resets layout results to default values.
  /// </summary>
  internal void ResetLayoutResults()
  {
    Layout.SetDirection(Direction.Inherit);
    Layout.SetHadOverflow(false);
    Layout.LastOwnerDirection = Direction.Inherit;
    Layout.ConfigVersion = 0;
    Layout.ComputedFlexBasis = FloatOptional.Undefined;
    Layout.ComputedFlexBasisGeneration = 0;
    Layout.GenerationCount = 0;
    Layout.NextCachedMeasurementsIndex = 0;
    Layout.CachedLayout = default;

    foreach (PhysicalEdge edge in YogaEnums.Ordinals<PhysicalEdge>())
    {
      Layout.SetPosition(edge, 0);
      Layout.SetMargin(edge, 0);
      Layout.SetBorder(edge, 0);
      Layout.SetPadding(edge, 0);
    }

    foreach (Dimension dim in YogaEnums.Ordinals<Dimension>())
    {
      Layout.SetDimension(dim, float.NaN);
      Layout.SetMeasuredDimension(dim, float.NaN);
      Layout.SetRawDimension(dim, float.NaN);
    }

    for (int i = 0; i < LayoutResults.MaxCachedMeasurements; i++)
    {
      Layout.SetCachedMeasurement(i, default);
    }
  }

  #endregion
}
