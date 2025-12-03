namespace TimeWarp.Flexbox.Debug;

using System.Globalization;
using System.Text;
using System.Text.Json;

/// <summary>
/// Provides debugging utilities for visualizing FlexNode trees and layout results.
/// </summary>
public static class FlexNodePrinter
{
  private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

  private static readonly JsonSerializerOptions JsonOptionsIndented = new()
  {
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  private static readonly JsonSerializerOptions JsonOptionsCompact = new()
  {
    WriteIndented = false,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  /// <summary>
  /// Prints a FlexNode tree with style properties and optional layout results.
  /// </summary>
  /// <param name="node">The root node to print.</param>
  /// <param name="includeLayout">Whether to include layout results in output.</param>
  /// <param name="verbose">Whether to include all properties (even defaults).</param>
  /// <returns>A string representation of the node tree.</returns>
  public static string Print(FlexNode node, bool includeLayout = true, bool verbose = false)
  {
    ArgumentNullException.ThrowIfNull(node);

    StringBuilder sb = new();
    PrintNode(sb, node, 0, includeLayout, verbose);
    return sb.ToString();
  }

  /// <summary>
  /// Prints only the layout results for a FlexNode tree.
  /// </summary>
  /// <param name="node">The root node to print.</param>
  /// <returns>A string representation of the layout results.</returns>
  public static string PrintLayout(FlexNode node)
  {
    ArgumentNullException.ThrowIfNull(node);

    StringBuilder sb = new();
    PrintLayoutNode(sb, node, 0);
    return sb.ToString();
  }

  /// <summary>
  /// Serializes the layout results of a FlexNode tree to JSON.
  /// </summary>
  /// <param name="node">The root node to serialize.</param>
  /// <param name="indented">Whether to format with indentation.</param>
  /// <returns>A JSON string of layout results.</returns>
  public static string ToLayoutJson(FlexNode node, bool indented = true)
  {
    ArgumentNullException.ThrowIfNull(node);

    object layoutTree = BuildLayoutTree(node);
    JsonSerializerOptions options = indented ? JsonOptionsIndented : JsonOptionsCompact;

    return JsonSerializer.Serialize(layoutTree, options);
  }

  private static void PrintNode(StringBuilder sb, FlexNode node, int depth, bool includeLayout, bool verbose)
  {
    string indent = new(' ', depth * 2);

    sb.Append(indent).AppendLine("<FlexNode");

    // Direction and wrapping
    if (verbose || node.FlexDirection != FlexDirection.Row)
      sb.Append(indent).Append("  FlexDirection=").AppendLine(node.FlexDirection.ToString());
    if (verbose || node.FlexWrap != FlexWrap.NoWrap)
      sb.Append(indent).Append("  FlexWrap=").AppendLine(node.FlexWrap.ToString());
    if (verbose || node.Direction != Direction.Inherit)
      sb.Append(indent).Append("  Direction=").AppendLine(node.Direction.ToString());

    // Alignment
    if (verbose || node.JustifyContent != JustifyContent.FlexStart)
      sb.Append(indent).Append("  JustifyContent=").AppendLine(node.JustifyContent.ToString());
    if (verbose || node.AlignItems != AlignItems.Stretch)
      sb.Append(indent).Append("  AlignItems=").AppendLine(node.AlignItems.ToString());
    if (verbose || node.AlignContent != AlignContent.FlexStart)
      sb.Append(indent).Append("  AlignContent=").AppendLine(node.AlignContent.ToString());
    if (verbose || node.AlignSelf != AlignSelf.Auto)
      sb.Append(indent).Append("  AlignSelf=").AppendLine(node.AlignSelf.ToString());

    // Flex factors
    if (verbose || node.FlexGrow != 0)
      sb.Append(indent).Append("  FlexGrow=").AppendLine(node.FlexGrow.ToString(Invariant));
    if (verbose || node.FlexShrink != 1)
      sb.Append(indent).Append("  FlexShrink=").AppendLine(node.FlexShrink.ToString(Invariant));
    if (verbose || node.FlexBasis.Unit != Unit.Auto)
      sb.Append(indent).Append("  FlexBasis=").AppendLine(node.FlexBasis.ToString());

    // Dimensions
    if (verbose || node.Width.Unit != Unit.Undefined)
      sb.Append(indent).Append("  Width=").AppendLine(node.Width.ToString());
    if (verbose || node.Height.Unit != Unit.Undefined)
      sb.Append(indent).Append("  Height=").AppendLine(node.Height.ToString());
    if (verbose || node.MinWidth.Unit != Unit.Undefined)
      sb.Append(indent).Append("  MinWidth=").AppendLine(node.MinWidth.ToString());
    if (verbose || node.MinHeight.Unit != Unit.Undefined)
      sb.Append(indent).Append("  MinHeight=").AppendLine(node.MinHeight.ToString());
    if (verbose || node.MaxWidth.Unit != Unit.Undefined)
      sb.Append(indent).Append("  MaxWidth=").AppendLine(node.MaxWidth.ToString());
    if (verbose || node.MaxHeight.Unit != Unit.Undefined)
      sb.Append(indent).Append("  MaxHeight=").AppendLine(node.MaxHeight.ToString());

    // Gap
    if (verbose || node.RowGap != 0)
      sb.Append(indent).Append("  RowGap=").AppendLine(node.RowGap.ToString(Invariant));
    if (verbose || node.ColumnGap != 0)
      sb.Append(indent).Append("  ColumnGap=").AppendLine(node.ColumnGap.ToString(Invariant));

    // Position
    if (verbose || node.PositionType != PositionType.Relative)
      sb.Append(indent).Append("  PositionType=").AppendLine(node.PositionType.ToString());

    // Display and overflow
    if (verbose || node.Display != Display.Flex)
      sb.Append(indent).Append("  Display=").AppendLine(node.Display.ToString());
    if (verbose || node.Overflow != Overflow.Visible)
      sb.Append(indent).Append("  Overflow=").AppendLine(node.Overflow.ToString());

    // Aspect ratio
    if (node.AspectRatio.HasValue)
      sb.Append(indent).Append("  AspectRatio=").AppendLine(node.AspectRatio.Value.ToString(Invariant));

    // Layout results
    if (includeLayout)
    {
      sb.Append(indent).AppendLine("  Layout={");
      sb.Append(indent).Append("    Left=").AppendLine(node.Layout.Left.ToString("F2", Invariant));
      sb.Append(indent).Append("    Top=").AppendLine(node.Layout.Top.ToString("F2", Invariant));
      sb.Append(indent).Append("    Width=").AppendLine(node.Layout.Width.ToString("F2", Invariant));
      sb.Append(indent).Append("    Height=").AppendLine(node.Layout.Height.ToString("F2", Invariant));
      sb.Append(indent).AppendLine("  }");
    }

    sb.Append(indent).AppendLine(">");

    // Children
    foreach (FlexNode child in node.Children)
    {
      PrintNode(sb, child, depth + 1, includeLayout, verbose);
    }

    sb.Append(indent).AppendLine("</FlexNode>");
  }

  private static void PrintLayoutNode(StringBuilder sb, FlexNode node, int depth)
  {
    string indent = new(' ', depth * 2);

    sb.Append(indent)
      .Append('[')
      .Append(node.Layout.Left.ToString("F2", Invariant))
      .Append(", ")
      .Append(node.Layout.Top.ToString("F2", Invariant))
      .Append(", ")
      .Append(node.Layout.Width.ToString("F2", Invariant))
      .Append(", ")
      .Append(node.Layout.Height.ToString("F2", Invariant))
      .AppendLine("]");

    foreach (FlexNode child in node.Children)
    {
      PrintLayoutNode(sb, child, depth + 1);
    }
  }

  private static object BuildLayoutTree(FlexNode node)
  {
    List<object> children = [];

    foreach (FlexNode child in node.Children)
    {
      children.Add(BuildLayoutTree(child));
    }

    return new
    {
      left = node.Layout.Left,
      top = node.Layout.Top,
      width = node.Layout.Width,
      height = node.Layout.Height,
      children = children.Count > 0 ? children : null
    };
  }
}
