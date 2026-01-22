/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/node/LayoutResults.h, yoga/node/LayoutResults.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Stores the computed layout output for a node.
/// </summary>
/// <remarks>
/// This struct contains all computed layout values including position, dimensions,
/// margins, borders, padding, and cached measurements for layout optimization.
/// </remarks>
public sealed class LayoutResults : IEquatable<LayoutResults>
{
    /// <summary>
    /// Maximum number of cached measurements per node.
    /// This value was chosen based on empirical data:
    /// 98% of analyzed layouts require less than 8 entries.
    /// </summary>
    public const int MaxCachedMeasurements = 8;

    #region Cache Fields

    /// <summary>
    /// Generation counter for computed flex basis.
    /// </summary>
    public uint ComputedFlexBasisGeneration { get; set; }

    /// <summary>
    /// The computed flex basis value.
    /// </summary>
    public FloatOptional ComputedFlexBasis { get; set; } = FloatOptional.Undefined;

    /// <summary>
    /// Generation counter used to detect when layout needs recalculation.
    /// Instead of recomputing the entire layout every single time, we cache some
    /// information to break early when nothing changed.
    /// </summary>
    public uint GenerationCount { get; set; }

    /// <summary>
    /// The config version used when this layout was computed.
    /// </summary>
    public uint ConfigVersion { get; set; }

    /// <summary>
    /// The direction of the owner when this layout was computed.
    /// </summary>
    public Direction LastOwnerDirection { get; set; } = Direction.Inherit;

    /// <summary>
    /// Index of the next cached measurement slot to use.
    /// </summary>
    public uint NextCachedMeasurementsIndex { get; set; }

    private readonly CachedMeasurement[] _cachedMeasurements = new CachedMeasurement[MaxCachedMeasurements];

    /// <summary>
    /// Gets the cached measurement at the specified index.
    /// </summary>
    /// <param name="index">The index of the cached measurement.</param>
    /// <returns>The cached measurement at the specified index.</returns>
    public CachedMeasurement GetCachedMeasurement(int index) => _cachedMeasurements[index];

    /// <summary>
    /// Sets the cached measurement at the specified index.
    /// </summary>
    /// <param name="index">The index of the cached measurement.</param>
    /// <param name="measurement">The cached measurement to set.</param>
    public void SetCachedMeasurement(int index, CachedMeasurement measurement) => _cachedMeasurements[index] = measurement;

    /// <summary>
    /// Cached layout result for the node.
    /// </summary>
    public CachedMeasurement CachedLayout { get; set; }

    #endregion

    #region Layout Direction

    private Direction _direction = Direction.Inherit;

    /// <summary>
    /// Gets the computed layout direction.
    /// </summary>
    public Direction Direction => _direction;

    /// <summary>
    /// Sets the computed layout direction.
    /// </summary>
    /// <param name="direction">The direction to set.</param>
    public void SetDirection(Direction direction) => _direction = direction;

    #endregion

    #region Overflow Flag

    private bool _hadOverflow;

    /// <summary>
    /// Gets whether the node had overflow during layout.
    /// </summary>
    public bool HadOverflow => _hadOverflow;

    /// <summary>
    /// Sets whether the node had overflow during layout.
    /// </summary>
    /// <param name="hadOverflow">True if overflow occurred.</param>
    public void SetHadOverflow(bool hadOverflow) => _hadOverflow = hadOverflow;

    #endregion

    #region Dimensions

    private readonly float[] _dimensions = [float.NaN, float.NaN];
    private readonly float[] _measuredDimensions = [float.NaN, float.NaN];
    private readonly float[] _rawDimensions = [float.NaN, float.NaN];

    /// <summary>
    /// Gets the computed dimension for the specified axis.
    /// </summary>
    /// <param name="axis">The dimension axis (Width or Height).</param>
    /// <returns>The computed dimension value.</returns>
    public float GetDimension(Dimension axis) => _dimensions[YogaEnums.ToUnderlying(axis)];

    /// <summary>
    /// Sets the computed dimension for the specified axis.
    /// </summary>
    /// <param name="axis">The dimension axis (Width or Height).</param>
    /// <param name="dimension">The dimension value to set.</param>
    public void SetDimension(Dimension axis, float dimension) => _dimensions[YogaEnums.ToUnderlying(axis)] = dimension;

    /// <summary>
    /// Gets the measured dimension for the specified axis.
    /// </summary>
    /// <param name="axis">The dimension axis (Width or Height).</param>
    /// <returns>The measured dimension value.</returns>
    public float GetMeasuredDimension(Dimension axis) => _measuredDimensions[YogaEnums.ToUnderlying(axis)];

    /// <summary>
    /// Sets the measured dimension for the specified axis.
    /// </summary>
    /// <param name="axis">The dimension axis (Width or Height).</param>
    /// <param name="dimension">The dimension value to set.</param>
    public void SetMeasuredDimension(Dimension axis, float dimension) => _measuredDimensions[YogaEnums.ToUnderlying(axis)] = dimension;

    /// <summary>
    /// Gets the raw (pre-rounding) dimension for the specified axis.
    /// </summary>
    /// <param name="axis">The dimension axis (Width or Height).</param>
    /// <returns>The raw dimension value.</returns>
    public float GetRawDimension(Dimension axis) => _rawDimensions[YogaEnums.ToUnderlying(axis)];

    /// <summary>
    /// Sets the raw (pre-rounding) dimension for the specified axis.
    /// </summary>
    /// <param name="axis">The dimension axis (Width or Height).</param>
    /// <param name="dimension">The dimension value to set.</param>
    public void SetRawDimension(Dimension axis, float dimension) => _rawDimensions[YogaEnums.ToUnderlying(axis)] = dimension;

    #endregion

    #region Position

    private readonly float[] _position = new float[4];

    /// <summary>
    /// Gets the position for the specified physical edge.
    /// </summary>
    /// <param name="physicalEdge">The physical edge.</param>
    /// <returns>The position value.</returns>
    public float GetPosition(PhysicalEdge physicalEdge) => _position[YogaEnums.ToUnderlying(physicalEdge)];

    /// <summary>
    /// Sets the position for the specified physical edge.
    /// </summary>
    /// <param name="physicalEdge">The physical edge.</param>
    /// <param name="dimension">The position value to set.</param>
    public void SetPosition(PhysicalEdge physicalEdge, float dimension) => _position[YogaEnums.ToUnderlying(physicalEdge)] = dimension;

    #endregion

    #region Margin

    private readonly float[] _margin = new float[4];

    /// <summary>
    /// Gets the computed margin for the specified physical edge.
    /// </summary>
    /// <param name="physicalEdge">The physical edge.</param>
    /// <returns>The margin value.</returns>
    public float GetMargin(PhysicalEdge physicalEdge) => _margin[YogaEnums.ToUnderlying(physicalEdge)];

    /// <summary>
    /// Sets the computed margin for the specified physical edge.
    /// </summary>
    /// <param name="physicalEdge">The physical edge.</param>
    /// <param name="dimension">The margin value to set.</param>
    public void SetMargin(PhysicalEdge physicalEdge, float dimension) => _margin[YogaEnums.ToUnderlying(physicalEdge)] = dimension;

    #endregion

    #region Border

    private readonly float[] _border = new float[4];

    /// <summary>
    /// Gets the computed border for the specified physical edge.
    /// </summary>
    /// <param name="physicalEdge">The physical edge.</param>
    /// <returns>The border value.</returns>
    public float GetBorder(PhysicalEdge physicalEdge) => _border[YogaEnums.ToUnderlying(physicalEdge)];

    /// <summary>
    /// Sets the computed border for the specified physical edge.
    /// </summary>
    /// <param name="physicalEdge">The physical edge.</param>
    /// <param name="dimension">The border value to set.</param>
    public void SetBorder(PhysicalEdge physicalEdge, float dimension) => _border[YogaEnums.ToUnderlying(physicalEdge)] = dimension;

    #endregion

    #region Padding

    private readonly float[] _padding = new float[4];

    /// <summary>
    /// Gets the computed padding for the specified physical edge.
    /// </summary>
    /// <param name="physicalEdge">The physical edge.</param>
    /// <returns>The padding value.</returns>
    public float GetPadding(PhysicalEdge physicalEdge) => _padding[YogaEnums.ToUnderlying(physicalEdge)];

    /// <summary>
    /// Sets the computed padding for the specified physical edge.
    /// </summary>
    /// <param name="physicalEdge">The physical edge.</param>
    /// <param name="dimension">The padding value to set.</param>
    public void SetPadding(PhysicalEdge physicalEdge, float dimension) => _padding[YogaEnums.ToUnderlying(physicalEdge)] = dimension;

    #endregion

    #region Equality

    /// <inheritdoc />
    public bool Equals(LayoutResults? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        bool isEqual = Comparison.InexactEquals(_position, other._position) &&
                       Comparison.InexactEquals(_dimensions, other._dimensions) &&
                       Comparison.InexactEquals(_margin, other._margin) &&
                       Comparison.InexactEquals(_border, other._border) &&
                       Comparison.InexactEquals(_padding, other._padding) &&
                       Direction == other.Direction &&
                       HadOverflow == other.HadOverflow &&
                       LastOwnerDirection == other.LastOwnerDirection &&
                       ConfigVersion == other.ConfigVersion &&
                       NextCachedMeasurementsIndex == other.NextCachedMeasurementsIndex &&
                       CachedLayout == other.CachedLayout &&
                       ComputedFlexBasis == other.ComputedFlexBasis;

        for (int i = 0; i < MaxCachedMeasurements && isEqual; i++)
        {
            isEqual = isEqual && _cachedMeasurements[i] == other._cachedMeasurements[i];
        }

        if (!Comparison.IsUndefined(_measuredDimensions[0]) ||
            !Comparison.IsUndefined(other._measuredDimensions[0]))
        {
            isEqual = isEqual && (_measuredDimensions[0] == other._measuredDimensions[0]);
        }

        if (!Comparison.IsUndefined(_measuredDimensions[1]) ||
            !Comparison.IsUndefined(other._measuredDimensions[1]))
        {
            isEqual = isEqual && (_measuredDimensions[1] == other._measuredDimensions[1]);
        }

        return isEqual;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is LayoutResults other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(_direction);
        hash.Add(_hadOverflow);
        hash.Add(LastOwnerDirection);
        hash.Add(ConfigVersion);
        hash.Add(NextCachedMeasurementsIndex);
        hash.Add(CachedLayout);
        hash.Add(ComputedFlexBasis);

        foreach (float value in _position)
        {
            hash.Add(value);
        }

        foreach (float value in _dimensions)
        {
            hash.Add(value);
        }

        return hash.ToHashCode();
    }

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(LayoutResults? left, LayoutResults? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(LayoutResults? left, LayoutResults? right) => !(left == right);

    #endregion
}
