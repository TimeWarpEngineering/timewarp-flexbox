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

  private readonly CachedMeasurement[] CachedMeasurements = new CachedMeasurement[MaxCachedMeasurements];

  /// <summary>
  /// Gets the cached measurement at the specified index.
  /// </summary>
  /// <param name="index">The index of the cached measurement.</param>
  /// <returns>The cached measurement at the specified index.</returns>
  internal CachedMeasurement GetCachedMeasurement(int index) => CachedMeasurements[index];

  /// <summary>
  /// Sets the cached measurement at the specified index.
  /// </summary>
  /// <param name="index">The index of the cached measurement.</param>
  /// <param name="measurement">The cached measurement to set.</param>
  internal void SetCachedMeasurement(int index, CachedMeasurement measurement) => CachedMeasurements[index] = measurement;

  /// <summary>
  /// Cached layout result for the node.
  /// </summary>
  internal CachedMeasurement CachedLayout { get; set; }

  #endregion

  #region Layout Direction

  /// <summary>
  /// Gets the computed layout direction.
  /// </summary>
  public Direction Direction { get; private set; } = Direction.Inherit;

  /// <summary>
  /// Sets the computed layout direction.
  /// </summary>
  /// <param name="direction">The direction to set.</param>
  public void SetDirection(Direction direction) => Direction = direction;

  #endregion

  #region Overflow Flag

  /// <summary>
  /// Gets whether the node had overflow during layout.
  /// </summary>
  public bool HadOverflow { get; private set; }

  /// <summary>
  /// Sets whether the node had overflow during layout.
  /// </summary>
  /// <param name="hadOverflow">True if overflow occurred.</param>
  public void SetHadOverflow(bool hadOverflow) => HadOverflow = hadOverflow;

  #endregion

  #region Dimensions

  private readonly float[] Dimensions = [float.NaN, float.NaN];
  private readonly float[] MeasuredDimensions = [float.NaN, float.NaN];
  private readonly float[] RawDimensions = [float.NaN, float.NaN];

  /// <summary>
  /// Gets the computed dimension for the specified axis.
  /// </summary>
  /// <param name="axis">The dimension axis (Width or Height).</param>
  /// <returns>The computed dimension value.</returns>
  public float GetDimension(Dimension axis) => Dimensions[YogaEnums.ToUnderlying(axis)];

  /// <summary>
  /// Sets the computed dimension for the specified axis.
  /// </summary>
  /// <param name="axis">The dimension axis (Width or Height).</param>
  /// <param name="dimension">The dimension value to set.</param>
  public void SetDimension(Dimension axis, float dimension) => Dimensions[YogaEnums.ToUnderlying(axis)] = dimension;

  /// <summary>
  /// Gets the measured dimension for the specified axis.
  /// </summary>
  /// <param name="axis">The dimension axis (Width or Height).</param>
  /// <returns>The measured dimension value.</returns>
  public float GetMeasuredDimension(Dimension axis) => MeasuredDimensions[YogaEnums.ToUnderlying(axis)];

  /// <summary>
  /// Sets the measured dimension for the specified axis.
  /// </summary>
  /// <param name="axis">The dimension axis (Width or Height).</param>
  /// <param name="dimension">The dimension value to set.</param>
  public void SetMeasuredDimension(Dimension axis, float dimension) => MeasuredDimensions[YogaEnums.ToUnderlying(axis)] = dimension;

  /// <summary>
  /// Gets the raw (pre-rounding) dimension for the specified axis.
  /// </summary>
  /// <param name="axis">The dimension axis (Width or Height).</param>
  /// <returns>The raw dimension value.</returns>
  public float GetRawDimension(Dimension axis) => RawDimensions[YogaEnums.ToUnderlying(axis)];

  /// <summary>
  /// Sets the raw (pre-rounding) dimension for the specified axis.
  /// </summary>
  /// <param name="axis">The dimension axis (Width or Height).</param>
  /// <param name="dimension">The dimension value to set.</param>
  public void SetRawDimension(Dimension axis, float dimension) => RawDimensions[YogaEnums.ToUnderlying(axis)] = dimension;

  #endregion

  #region Position

  private readonly float[] Position = new float[4];

  /// <summary>
  /// Gets the position for the specified physical edge.
  /// </summary>
  /// <param name="physicalEdge">The physical edge.</param>
  /// <returns>The position value.</returns>
  public float GetPosition(PhysicalEdge physicalEdge) => Position[YogaEnums.ToUnderlying(physicalEdge)];

  /// <summary>
  /// Sets the position for the specified physical edge.
  /// </summary>
  /// <param name="physicalEdge">The physical edge.</param>
  /// <param name="dimension">The position value to set.</param>
  public void SetPosition(PhysicalEdge physicalEdge, float dimension) => Position[YogaEnums.ToUnderlying(physicalEdge)] = dimension;

  #endregion

  #region Margin

  private readonly float[] Margin = new float[4];

  /// <summary>
  /// Gets the computed margin for the specified physical edge.
  /// </summary>
  /// <param name="physicalEdge">The physical edge.</param>
  /// <returns>The margin value.</returns>
  public float GetMargin(PhysicalEdge physicalEdge) => Margin[YogaEnums.ToUnderlying(physicalEdge)];

  /// <summary>
  /// Sets the computed margin for the specified physical edge.
  /// </summary>
  /// <param name="physicalEdge">The physical edge.</param>
  /// <param name="dimension">The margin value to set.</param>
  public void SetMargin(PhysicalEdge physicalEdge, float dimension) => Margin[YogaEnums.ToUnderlying(physicalEdge)] = dimension;

  #endregion

  #region Border

  private readonly float[] Border = new float[4];

  /// <summary>
  /// Gets the computed border for the specified physical edge.
  /// </summary>
  /// <param name="physicalEdge">The physical edge.</param>
  /// <returns>The border value.</returns>
  public float GetBorder(PhysicalEdge physicalEdge) => Border[YogaEnums.ToUnderlying(physicalEdge)];

  /// <summary>
  /// Sets the computed border for the specified physical edge.
  /// </summary>
  /// <param name="physicalEdge">The physical edge.</param>
  /// <param name="dimension">The border value to set.</param>
  public void SetBorder(PhysicalEdge physicalEdge, float dimension) => Border[YogaEnums.ToUnderlying(physicalEdge)] = dimension;

  #endregion

  #region Padding

  private readonly float[] Padding = new float[4];

  /// <summary>
  /// Gets the computed padding for the specified physical edge.
  /// </summary>
  /// <param name="physicalEdge">The physical edge.</param>
  /// <returns>The padding value.</returns>
  public float GetPadding(PhysicalEdge physicalEdge) => Padding[YogaEnums.ToUnderlying(physicalEdge)];

  /// <summary>
  /// Sets the computed padding for the specified physical edge.
  /// </summary>
  /// <param name="physicalEdge">The physical edge.</param>
  /// <param name="dimension">The padding value to set.</param>
  public void SetPadding(PhysicalEdge physicalEdge, float dimension) => Padding[YogaEnums.ToUnderlying(physicalEdge)] = dimension;

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

    bool isEqual = Comparison.InexactEquals(Position, other.Position) &&
                   Comparison.InexactEquals(Dimensions, other.Dimensions) &&
                   Comparison.InexactEquals(Margin, other.Margin) &&
                   Comparison.InexactEquals(Border, other.Border) &&
                   Comparison.InexactEquals(Padding, other.Padding) &&
                   Direction == other.Direction &&
                   HadOverflow == other.HadOverflow &&
                   LastOwnerDirection == other.LastOwnerDirection &&
                   ConfigVersion == other.ConfigVersion &&
                   NextCachedMeasurementsIndex == other.NextCachedMeasurementsIndex &&
                   CachedLayout == other.CachedLayout &&
                   ComputedFlexBasis == other.ComputedFlexBasis;

    for (int i = 0; i < MaxCachedMeasurements && isEqual; i++)
    {
      isEqual = isEqual && CachedMeasurements[i] == other.CachedMeasurements[i];
    }

    if (!Comparison.IsUndefined(MeasuredDimensions[0]) ||
        !Comparison.IsUndefined(other.MeasuredDimensions[0]))
    {
      isEqual = isEqual && (MeasuredDimensions[0] == other.MeasuredDimensions[0]);
    }

    if (!Comparison.IsUndefined(MeasuredDimensions[1]) ||
        !Comparison.IsUndefined(other.MeasuredDimensions[1]))
    {
      isEqual = isEqual && (MeasuredDimensions[1] == other.MeasuredDimensions[1]);
    }

    return isEqual;
  }

  /// <inheritdoc />
  public override bool Equals(object? obj) => obj is LayoutResults other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode()
  {
    HashCode hash = new();
    hash.Add(Direction);
    hash.Add(HadOverflow);
    hash.Add(LastOwnerDirection);
    hash.Add(ConfigVersion);
    hash.Add(NextCachedMeasurementsIndex);
    hash.Add(CachedLayout);
    hash.Add(ComputedFlexBasis);

    foreach (float value in Position)
    {
      hash.Add(value);
    }

    foreach (float value in Dimensions)
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
