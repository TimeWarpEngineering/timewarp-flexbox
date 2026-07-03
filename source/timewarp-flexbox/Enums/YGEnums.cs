/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/YGEnums.h, yoga/YGEnums.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

#region Enums

/// <summary>
/// Specifies how a node's children are aligned along the cross axis.
/// </summary>
[OrdinalCount(10)]
public enum Align
{
  /// <summary>Defer to parent's align-items (only valid for align-self).</summary>
  Auto = 0,

  /// <summary>Align to the start of the cross axis.</summary>
  FlexStart = 1,

  /// <summary>Center along the cross axis.</summary>
  Center = 2,

  /// <summary>Align to the end of the cross axis.</summary>
  FlexEnd = 3,

  /// <summary>Stretch to fill the cross axis (default).</summary>
  Stretch = 4,

  /// <summary>Align baselines of first text line.</summary>
  Baseline = 5,

  /// <summary>Distribute with equal space between items.</summary>
  SpaceBetween = 6,

  /// <summary>Distribute with equal space around items.</summary>
  SpaceAround = 7,

  /// <summary>Distribute with equal space between and around items.</summary>
  SpaceEvenly = 8
}

/// <summary>
/// Specifies how node dimensions are calculated.
/// </summary>
[OrdinalCount(2)]
public enum BoxSizing
{
  /// <summary>Dimensions include padding and border (default CSS behavior).</summary>
  BorderBox = 0,

  /// <summary>Dimensions are for content only, padding/border added.</summary>
  ContentBox = 1
}

/// <summary>
/// Specifies a dimension (width or height).
/// </summary>
[OrdinalCount(2)]
public enum Dimension
{
  /// <summary>Width dimension.</summary>
  Width = 0,

  /// <summary>Height dimension.</summary>
  Height = 1
}

/// <summary>
/// Specifies the text direction for layout.
/// </summary>
[OrdinalCount(3)]
public enum Direction
{
  /// <summary>Inherit direction from parent.</summary>
  Inherit = 0,

  /// <summary>Left-to-right direction.</summary>
  LTR = 1,

  /// <summary>Right-to-left direction.</summary>
  RTL = 2
}

/// <summary>
/// Specifies the display type of a node.
/// </summary>
[OrdinalCount(3)]
public enum Display
{
  /// <summary>Node is a flex container.</summary>
  Flex = 0,

  /// <summary>Node is hidden and doesn't participate in layout.</summary>
  None = 1,

  /// <summary>Node's children are treated as direct children of the node's parent.</summary>
  Contents = 2
}

/// <summary>
/// Specifies which edge(s) of a node a property applies to.
/// </summary>
[OrdinalCount(9)]
public enum Edge
{
  /// <summary>Left edge (in LTR context).</summary>
  Left = 0,

  /// <summary>Top edge.</summary>
  Top = 1,

  /// <summary>Right edge (in LTR context).</summary>
  Right = 2,

  /// <summary>Bottom edge.</summary>
  Bottom = 3,

  /// <summary>Start edge (left in LTR, right in RTL).</summary>
  Start = 4,

  /// <summary>End edge (right in LTR, left in RTL).</summary>
  End = 5,

  /// <summary>Both horizontal edges (left and right).</summary>
  Horizontal = 6,

  /// <summary>Both vertical edges (top and bottom).</summary>
  Vertical = 7,

  /// <summary>All four edges.</summary>
  All = 8
}

/// <summary>
/// Flags for enabling legacy/errata behavior modes.
/// </summary>
/// <remarks>
/// Design Decision: CA2217 is suppressed because this enum intentionally uses
/// special values (All, Classic) that are not powers of two, matching the C++ Yoga implementation.
/// </remarks>
[Flags]
[OrdinalCount(6)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2217:Do not mark enums with FlagsAttribute", Justification = "Intentionally uses All/Classic values matching C++ Yoga")]
public enum Errata
{
  /// <summary>No errata, use spec-compliant behavior.</summary>
  None = 0,

  /// <summary>Legacy stretch behavior for flex-basis.</summary>
  StretchFlexBasis = 1,

  /// <summary>Absolute positioning excludes padding without insets.</summary>
  AbsolutePositionWithoutInsetsExcludesPadding = 2,

  /// <summary>Absolute percent values against inner size.</summary>
  AbsolutePercentAgainstInnerSize = 4,

  /// <summary>Enable all errata flags.</summary>
  All = 2147483647,

  /// <summary>Classic pre-1.0 behavior (all except None).</summary>
  Classic = 2147483646
}

/// <summary>
/// Experimental features that may change or be removed.
/// </summary>
[OrdinalCount(1)]
public enum ExperimentalFeature
{
  /// <summary>Use web-compatible flex-basis behavior.</summary>
  WebFlexBasis = 0
}

/// <summary>
/// Specifies the direction of the main axis for flex layout.
/// </summary>
[OrdinalCount(4)]
public enum FlexDirection
{
  /// <summary>Main axis is vertical, top to bottom.</summary>
  Column = 0,

  /// <summary>Main axis is vertical, bottom to top.</summary>
  ColumnReverse = 1,

  /// <summary>Main axis is horizontal, left to right (or RTL).</summary>
  Row = 2,

  /// <summary>Main axis is horizontal, right to left (or RTL reversed).</summary>
  RowReverse = 3
}

/// <summary>
/// Specifies which gutter(s) a gap property applies to.
/// </summary>
[OrdinalCount(3)]
public enum Gutter
{
  /// <summary>Gap between columns.</summary>
  Column = 0,

  /// <summary>Gap between rows.</summary>
  Row = 1,

  /// <summary>Gap between both columns and rows.</summary>
  All = 2
}

/// <summary>
/// Specifies how flex items are distributed along the main axis.
/// </summary>
[OrdinalCount(6)]
public enum Justify
{
  /// <summary>Pack items at the start of the main axis.</summary>
  FlexStart = 0,

  /// <summary>Center items along the main axis.</summary>
  Center = 1,

  /// <summary>Pack items at the end of the main axis.</summary>
  FlexEnd = 2,

  /// <summary>Distribute with equal space between items.</summary>
  SpaceBetween = 3,

  /// <summary>Distribute with equal space around items.</summary>
  SpaceAround = 4,

  /// <summary>Distribute with equal space between and around items.</summary>
  SpaceEvenly = 5
}

/// <summary>
/// Log levels for Yoga debug output.
/// </summary>
[OrdinalCount(6)]
public enum LogLevel
{
  /// <summary>Error messages only.</summary>
  Error = 0,

  /// <summary>Warning messages and above.</summary>
  Warn = 1,

  /// <summary>Informational messages and above.</summary>
  Info = 2,

  /// <summary>Debug messages and above.</summary>
  Debug = 3,

  /// <summary>Verbose messages and above.</summary>
  Verbose = 4,

  /// <summary>Fatal errors that terminate execution.</summary>
  Fatal = 5
}

/// <summary>
/// Specifies how a dimension should be measured.
/// </summary>
[OrdinalCount(3)]
public enum MeasureMode
{
  /// <summary>No constraint, measure freely.</summary>
  Undefined = 0,

  /// <summary>Measure to exactly the given size.</summary>
  Exactly = 1,

  /// <summary>Measure up to but not exceeding the given size.</summary>
  AtMost = 2
}

/// <summary>
/// Specifies the type of node for layout purposes.
/// </summary>
[OrdinalCount(2)]
public enum NodeType
{
  /// <summary>Default container node.</summary>
  Default = 0,

  /// <summary>Text node with baseline support.</summary>
  Text = 1
}

/// <summary>
/// Specifies how content that overflows the node is handled.
/// </summary>
[OrdinalCount(3)]
public enum Overflow
{
  /// <summary>Content is not clipped and may overflow.</summary>
  Visible = 0,

  /// <summary>Content is clipped to the node's bounds.</summary>
  Hidden = 1,

  /// <summary>Content is clipped and scrollable.</summary>
  Scroll = 2
}

/// <summary>
/// Specifies how a node is positioned within its parent.
/// </summary>
[OrdinalCount(3)]
public enum PositionType
{
  /// <summary>Node is positioned statically (not offset).</summary>
  Static = 0,

  /// <summary>Node is offset relative to its normal position.</summary>
  Relative = 1,

  /// <summary>Node is positioned absolutely within containing block.</summary>
  Absolute = 2
}

/// <summary>
/// Specifies the unit type for a dimension value.
/// </summary>
[OrdinalCount(7)]
public enum Unit
{
  /// <summary>Value is undefined/unset.</summary>
  Undefined = 0,

  /// <summary>Value is in points (absolute units).</summary>
  Point = 1,

  /// <summary>Value is a percentage of parent.</summary>
  Percent = 2,

  /// <summary>Value is automatically calculated.</summary>
  Auto = 3,

  /// <summary>Value is max-content intrinsic size.</summary>
  MaxContent = 4,

  /// <summary>Value is fit-content intrinsic size.</summary>
  FitContent = 5,

  /// <summary>Value stretches to fill available space.</summary>
  Stretch = 6
}

/// <summary>
/// Specifies whether flex items wrap to multiple lines.
/// </summary>
[OrdinalCount(3)]
public enum Wrap
{
  /// <summary>Items do not wrap, single line only.</summary>
  NoWrap = 0,

  /// <summary>Items wrap to additional lines.</summary>
  Wrap = 1,

  /// <summary>Items wrap in reverse order.</summary>
  WrapReverse = 2
}

/// <summary>
/// Represents the four physical edges (not logical start/end).
/// Used for layout calculations where physical position matters.
/// </summary>
[OrdinalCount(4)]
public enum PhysicalEdge
{
  /// <summary>Left physical edge.</summary>
  Left = 0,

  /// <summary>Top physical edge.</summary>
  Top = 1,

  /// <summary>Right physical edge.</summary>
  Right = 2,

  /// <summary>Bottom physical edge.</summary>
  Bottom = 3
}

#endregion

#region YogaEnums Utilities

/// <summary>
/// Provides utility methods for working with Yoga enum types.
/// C# equivalent of C++ YogaEnums.h template utilities.
/// </summary>
/// <remarks>
/// Design Decision: In C++, these are compile-time template functions using concepts.
/// In C#, we use generic methods with constraints and attributes to achieve similar goals.
/// The ordinal count is provided via the <see cref="OrdinalCountAttribute"/> on each enum.
/// </remarks>
public static class YogaEnums
{
  /// <summary>
  /// Gets the count of ordinals in a Yoga enum (must have OrdinalCountAttribute).
  /// </summary>
  /// <typeparam name="TEnum">The enum type.</typeparam>
  /// <returns>The number of ordinal values in the enum.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown if the enum does not have an OrdinalCountAttribute.
  /// </exception>
  public static int OrdinalCount<TEnum>() where TEnum : struct, Enum
  {
    OrdinalCountAttribute? attr = typeof(TEnum).GetCustomAttributes(typeof(OrdinalCountAttribute), false)
        .FirstOrDefault() as OrdinalCountAttribute;

    return attr?.Count ?? throw new InvalidOperationException(
        $"Enum {typeof(TEnum).Name} does not have an OrdinalCountAttribute. " +
        "Only Yoga enums with defined ordinal counts can use this method.");
  }

  /// <summary>
  /// Gets the number of bits needed to represent all ordinals of an enum.
  /// </summary>
  /// <typeparam name="TEnum">The enum type.</typeparam>
  /// <returns>The minimum number of bits needed.</returns>
  public static int BitCount<TEnum>() where TEnum : struct, Enum
  {
    int count = OrdinalCount<TEnum>();
    if (count <= 1) return 0;
    return BitWidth((uint)(count - 1));
  }

  /// <summary>
  /// Converts an enum value to its underlying integer type.
  /// C# equivalent of C++23 to_underlying().
  /// </summary>
  /// <typeparam name="TEnum">The enum type.</typeparam>
  /// <param name="value">The enum value.</param>
  /// <returns>The underlying integer value.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int ToUnderlying<TEnum>(TEnum value) where TEnum : struct, Enum
  {
    // Using Unsafe.As for zero-cost conversion (assuming int underlying type)
    return Unsafe.As<TEnum, int>(ref value);
  }

  /// <summary>
  /// Returns an enumerable that iterates through all ordinal values of a Yoga enum.
  /// </summary>
  /// <typeparam name="TEnum">The enum type.</typeparam>
  /// <returns>An enumerable of all enum values from 0 to OrdinalCount-1.</returns>
  public static IEnumerable<TEnum> Ordinals<TEnum>() where TEnum : struct, Enum
  {
    int count = OrdinalCount<TEnum>();
    for (int i = 0; i < count; i++)
    {
      yield return Unsafe.As<int, TEnum>(ref i);
    }
  }

  /// <summary>
  /// Calculates the bit width (position of highest set bit + 1) of a value.
  /// C# equivalent of std::bit_width from C++20.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static int BitWidth(uint value)
  {
    if (value == 0) return 0;
    return 32 - System.Numerics.BitOperations.LeadingZeroCount(value);
  }
}

/// <summary>
/// Attribute to specify the ordinal count for Yoga enums.
/// This enables compile-time-like behavior for enum utilities.
/// </summary>
/// <remarks>
/// Design Decision: C++ uses template specialization to define ordinalCount for each enum.
/// In C#, we use an attribute since we can't specialize generic methods at compile time.
/// This attribute must be applied to all Yoga enums that participate in the ordinals system.
/// </remarks>
[AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
public sealed class OrdinalCountAttribute : Attribute
{
  /// <summary>
  /// Gets the number of ordinal values in the enum.
  /// </summary>
  public int Count { get; }

  /// <summary>
  /// Initializes a new instance with the specified ordinal count.
  /// </summary>
  /// <param name="count">The number of sequential ordinal values starting from 0.</param>
  public OrdinalCountAttribute(int count)
  {
    Count = count;
  }
}

#endregion

#region String Conversion Extensions

/// <summary>
/// Extension methods for converting Yoga enums to their CSS-compatible string representations.
/// </summary>
public static class YogaEnumExtensions
{
  /// <summary>Converts Align to its CSS string representation.</summary>
  public static string ToCssString(this Align value) => value switch
  {
    Align.Auto => "auto",
    Align.FlexStart => "flex-start",
    Align.Center => "center",
    Align.FlexEnd => "flex-end",
    Align.Stretch => "stretch",
    Align.Baseline => "baseline",
    Align.SpaceBetween => "space-between",
    Align.SpaceAround => "space-around",
    Align.SpaceEvenly => "space-evenly",
    _ => "unknown"
  };

  /// <summary>Converts BoxSizing to its CSS string representation.</summary>
  public static string ToCssString(this BoxSizing value) => value switch
  {
    BoxSizing.BorderBox => "border-box",
    BoxSizing.ContentBox => "content-box",
    _ => "unknown"
  };

  /// <summary>Converts Dimension to its CSS string representation.</summary>
  public static string ToCssString(this Dimension value) => value switch
  {
    Dimension.Width => "width",
    Dimension.Height => "height",
    _ => "unknown"
  };

  /// <summary>Converts Direction to its CSS string representation.</summary>
  public static string ToCssString(this Direction value) => value switch
  {
    Direction.Inherit => "inherit",
    Direction.LTR => "ltr",
    Direction.RTL => "rtl",
    _ => "unknown"
  };

  /// <summary>Converts Display to its CSS string representation.</summary>
  public static string ToCssString(this Display value) => value switch
  {
    Display.Flex => "flex",
    Display.None => "none",
    Display.Contents => "contents",
    _ => "unknown"
  };

  /// <summary>Converts Edge to its CSS string representation.</summary>
  public static string ToCssString(this Edge value) => value switch
  {
    Edge.Left => "left",
    Edge.Top => "top",
    Edge.Right => "right",
    Edge.Bottom => "bottom",
    Edge.Start => "start",
    Edge.End => "end",
    Edge.Horizontal => "horizontal",
    Edge.Vertical => "vertical",
    Edge.All => "all",
    _ => "unknown"
  };

  /// <summary>Converts Errata to its string representation.</summary>
  public static string ToCssString(this Errata value) => value switch
  {
    Errata.None => "none",
    Errata.StretchFlexBasis => "stretch-flex-basis",
    Errata.AbsolutePositionWithoutInsetsExcludesPadding => "absolute-position-without-insets-excludes-padding",
    Errata.AbsolutePercentAgainstInnerSize => "absolute-percent-against-inner-size",
    Errata.All => "all",
    Errata.Classic => "classic",
    _ => "unknown"
  };

  /// <summary>Converts ExperimentalFeature to its string representation.</summary>
  public static string ToCssString(this ExperimentalFeature value) => value switch
  {
    ExperimentalFeature.WebFlexBasis => "web-flex-basis",
    _ => "unknown"
  };

  /// <summary>Converts FlexDirection to its CSS string representation.</summary>
  public static string ToCssString(this FlexDirection value) => value switch
  {
    FlexDirection.Column => "column",
    FlexDirection.ColumnReverse => "column-reverse",
    FlexDirection.Row => "row",
    FlexDirection.RowReverse => "row-reverse",
    _ => "unknown"
  };

  /// <summary>Converts Gutter to its CSS string representation.</summary>
  public static string ToCssString(this Gutter value) => value switch
  {
    Gutter.Column => "column",
    Gutter.Row => "row",
    Gutter.All => "all",
    _ => "unknown"
  };

  /// <summary>Converts Justify to its CSS string representation.</summary>
  public static string ToCssString(this Justify value) => value switch
  {
    Justify.FlexStart => "flex-start",
    Justify.Center => "center",
    Justify.FlexEnd => "flex-end",
    Justify.SpaceBetween => "space-between",
    Justify.SpaceAround => "space-around",
    Justify.SpaceEvenly => "space-evenly",
    _ => "unknown"
  };

  /// <summary>Converts LogLevel to its string representation.</summary>
  public static string ToCssString(this LogLevel value) => value switch
  {
    LogLevel.Error => "error",
    LogLevel.Warn => "warn",
    LogLevel.Info => "info",
    LogLevel.Debug => "debug",
    LogLevel.Verbose => "verbose",
    LogLevel.Fatal => "fatal",
    _ => "unknown"
  };

  /// <summary>Converts MeasureMode to its string representation.</summary>
  public static string ToCssString(this MeasureMode value) => value switch
  {
    MeasureMode.Undefined => "undefined",
    MeasureMode.Exactly => "exactly",
    MeasureMode.AtMost => "at-most",
    _ => "unknown"
  };

  /// <summary>Converts NodeType to its string representation.</summary>
  public static string ToCssString(this NodeType value) => value switch
  {
    NodeType.Default => "default",
    NodeType.Text => "text",
    _ => "unknown"
  };

  /// <summary>Converts Overflow to its CSS string representation.</summary>
  public static string ToCssString(this Overflow value) => value switch
  {
    Overflow.Visible => "visible",
    Overflow.Hidden => "hidden",
    Overflow.Scroll => "scroll",
    _ => "unknown"
  };

  /// <summary>Converts PositionType to its CSS string representation.</summary>
  public static string ToCssString(this PositionType value) => value switch
  {
    PositionType.Static => "static",
    PositionType.Relative => "relative",
    PositionType.Absolute => "absolute",
    _ => "unknown"
  };

  /// <summary>Converts Unit to its string representation.</summary>
  public static string ToCssString(this Unit value) => value switch
  {
    Unit.Undefined => "undefined",
    Unit.Point => "point",
    Unit.Percent => "percent",
    Unit.Auto => "auto",
    Unit.MaxContent => "max-content",
    Unit.FitContent => "fit-content",
    Unit.Stretch => "stretch",
    _ => "unknown"
  };

  /// <summary>Converts Wrap to its CSS string representation.</summary>
  public static string ToCssString(this Wrap value) => value switch
  {
    Wrap.NoWrap => "no-wrap",
    Wrap.Wrap => "wrap",
    Wrap.WrapReverse => "wrap-reverse",
    _ => "unknown"
  };

  /// <summary>Converts PhysicalEdge to its string representation.</summary>
  public static string ToCssString(this PhysicalEdge value) => value switch
  {
    PhysicalEdge.Left => "left",
    PhysicalEdge.Top => "top",
    PhysicalEdge.Right => "right",
    PhysicalEdge.Bottom => "bottom",
    _ => "unknown"
  };
}

#endregion
