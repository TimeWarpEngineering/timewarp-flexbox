/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/CalculateLayout.cpp (lines 2149-2434)
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides the public entry point and caching wrapper for the layout algorithm.
/// </summary>
public static class CalculateLayout
{
  /// <summary>
  /// Global generation counter used to detect when layout needs recalculation.
  /// Each layout pass increments this counter, forcing dirty nodes to be revisited.
  /// </summary>
  private static int s_currentGenerationCount;

  /// <summary>
  /// Gets the current generation count (for testing purposes).
  /// </summary>
  internal static int CurrentGenerationCount => s_currentGenerationCount;

  /// <summary>
  /// The public entry point for calculating layout on a node tree.
  /// </summary>
  /// <param name="node">The root node of the tree to lay out.</param>
  /// <param name="ownerWidth">The available width from the owner (may be NaN for undefined).</param>
  /// <param name="ownerHeight">The available height from the owner (may be NaN for undefined).</param>
  /// <param name="ownerDirection">The direction of the owner.</param>
  public static void Calculate(
      Node node,
      float ownerWidth,
      float ownerHeight,
      Direction ownerDirection)
  {
    ArgumentNullException.ThrowIfNull(node);

    // Initialize the internal layout callbacks to break circular dependencies.
    // These delegates allow FlexBasis and AbsoluteLayout to call back into CalculateLayout.
    FlexBasis.CalculateLayoutInternal ??= (node, availableWidth, availableHeight, ownerDirection,
        widthSizingMode, heightSizingMode, ownerWidth, ownerHeight, performLayout, reason,
        layoutMarkerData, depth, generationCount) =>
        CalculateLayoutInternal(node, availableWidth, availableHeight, ownerDirection,
            widthSizingMode, heightSizingMode, ownerWidth, ownerHeight, performLayout, reason,
            layoutMarkerData, depth, (int)generationCount);
    AbsoluteLayout.CalculateLayoutInternal ??= CalculateLayoutInternal;

    YogaEvent.PublishLayoutPassStart(node);
    LayoutData markerData = new();

    // Increment the generation count. This will force the recursive routine to
    // visit all dirty nodes at least once. Subsequent visits will be skipped if
    // the input parameters don't change.
    int generationCount = Interlocked.Increment(ref s_currentGenerationCount);

    node.ProcessDimensions();
    Direction direction = node.ResolveDirection(ownerDirection);

    float width = float.NaN;
    SizingMode widthSizingMode = SizingMode.MaxContent;
    Style style = node.Style;

    if (node.HasDefiniteLength(Dimension.Width, ownerWidth))
    {
      width = node.GetResolvedDimension(
                  direction,
                  FlexDirection.Row.GetDimension(),
                  ownerWidth,
                  ownerWidth).Unwrap() +
              style.ComputeMarginForAxis(FlexDirection.Row, ownerWidth);
      widthSizingMode = SizingMode.StretchFit;
    }
    else if (style.ResolvedMaxDimension(direction, Dimension.Width, ownerWidth, ownerWidth).IsDefined)
    {
      width = style.ResolvedMaxDimension(direction, Dimension.Width, ownerWidth, ownerWidth).Unwrap();
      widthSizingMode = SizingMode.FitContent;
    }
    else
    {
      width = ownerWidth;
      widthSizingMode = Comparison.IsUndefined(width) ? SizingMode.MaxContent : SizingMode.StretchFit;
    }

    float height = float.NaN;
    SizingMode heightSizingMode = SizingMode.MaxContent;

    if (node.HasDefiniteLength(Dimension.Height, ownerHeight))
    {
      height = node.GetResolvedDimension(
                   direction,
                   FlexDirection.Column.GetDimension(),
                   ownerHeight,
                   ownerWidth).Unwrap() +
               style.ComputeMarginForAxis(FlexDirection.Column, ownerWidth);
      heightSizingMode = SizingMode.StretchFit;
    }
    else if (style.ResolvedMaxDimension(direction, Dimension.Height, ownerHeight, ownerWidth).IsDefined)
    {
      height = style.ResolvedMaxDimension(direction, Dimension.Height, ownerHeight, ownerWidth).Unwrap();
      heightSizingMode = SizingMode.FitContent;
    }
    else
    {
      height = ownerHeight;
      heightSizingMode = Comparison.IsUndefined(height) ? SizingMode.MaxContent : SizingMode.StretchFit;
    }

    if (CalculateLayoutInternal(
            node,
            width,
            height,
            ownerDirection,
            widthSizingMode,
            heightSizingMode,
            ownerWidth,
            ownerHeight,
            performLayout: true,
            LayoutPassReason.Initial,
            markerData,
            depth: 0,
            generationCount))
    {
      node.SetPosition(node.Layout.Direction, ownerWidth, ownerHeight);
      PixelGrid.RoundLayoutResultsToPixelGrid(node, 0.0f, 0.0f);
    }

    YogaEvent.PublishLayoutPassEnd(node, markerData);
  }

  /// <summary>
  /// Cache wrapper that checks if layout can be skipped based on cached results.
  /// </summary>
  /// <param name="node">The node to lay out.</param>
  /// <param name="availableWidth">The available width.</param>
  /// <param name="availableHeight">The available height.</param>
  /// <param name="ownerDirection">The direction of the owner.</param>
  /// <param name="widthSizingMode">The width sizing mode.</param>
  /// <param name="heightSizingMode">The height sizing mode.</param>
  /// <param name="ownerWidth">The owner's width.</param>
  /// <param name="ownerHeight">The owner's height.</param>
  /// <param name="performLayout">True to perform layout, false for measurement only.</param>
  /// <param name="reason">The reason for this layout pass.</param>
  /// <param name="layoutMarkerData">Statistics about the layout pass.</param>
  /// <param name="depth">The current depth in the tree.</param>
  /// <param name="generationCount">The current generation count.</param>
  /// <returns>True if layout was performed, false if skipped.</returns>
  internal static bool CalculateLayoutInternal(
      Node node,
      float availableWidth,
      float availableHeight,
      Direction ownerDirection,
      SizingMode widthSizingMode,
      SizingMode heightSizingMode,
      float ownerWidth,
      float ownerHeight,
      bool performLayout,
      LayoutPassReason reason,
      LayoutData layoutMarkerData,
      int depth,
      int generationCount)
  {
    LayoutResults layout = node.Layout;

    depth++;

    bool needToVisitNode =
        (node.IsDirty && layout.GenerationCount != generationCount) ||
        layout.ConfigVersion != node.Config.Version ||
        layout.LastOwnerDirection != ownerDirection;

    if (needToVisitNode)
    {
      // Invalidate the cached results.
      layout.NextCachedMeasurementsIndex = 0;
      layout.CachedLayout = new CachedMeasurement
      {
        AvailableWidth = -1,
        AvailableHeight = -1,
        WidthSizingMode = SizingMode.MaxContent,
        HeightSizingMode = SizingMode.MaxContent,
        ComputedWidth = -1,
        ComputedHeight = -1
      };
    }

    CachedMeasurement? cachedResults = null;

    // Determine whether the results are already cached. We maintain a separate
    // cache for layouts and measurements. A layout operation modifies the
    // positions and dimensions for nodes in the subtree. The algorithm assumes
    // that each node gets laid out a maximum of one time per tree layout, but
    // multiple measurements may be required to resolve all of the flex
    // dimensions. We handle nodes with measure functions specially here because
    // they are the most expensive to measure, so it's worth avoiding redundant
    // measurements if at all possible.
    if (node.HasMeasureFunc)
    {
      float marginAxisRow = node.Style.ComputeMarginForAxis(FlexDirection.Row, ownerWidth);
      float marginAxisColumn = node.Style.ComputeMarginForAxis(FlexDirection.Column, ownerWidth);

      // First, try to use the layout cache.
      if (Cache.CanUseCachedMeasurement(
              widthSizingMode,
              availableWidth,
              heightSizingMode,
              availableHeight,
              layout.CachedLayout.WidthSizingMode,
              layout.CachedLayout.AvailableWidth,
              layout.CachedLayout.HeightSizingMode,
              layout.CachedLayout.AvailableHeight,
              layout.CachedLayout.ComputedWidth,
              layout.CachedLayout.ComputedHeight,
              marginAxisRow,
              marginAxisColumn,
              node.Config))
      {
        cachedResults = layout.CachedLayout;
      }
      else
      {
        // Try to use the measurement cache.
        for (int i = 0; i < layout.NextCachedMeasurementsIndex; i++)
        {
          CachedMeasurement cached = layout.GetCachedMeasurement(i);
          if (Cache.CanUseCachedMeasurement(
                  widthSizingMode,
                  availableWidth,
                  heightSizingMode,
                  availableHeight,
                  cached.WidthSizingMode,
                  cached.AvailableWidth,
                  cached.HeightSizingMode,
                  cached.AvailableHeight,
                  cached.ComputedWidth,
                  cached.ComputedHeight,
                  marginAxisRow,
                  marginAxisColumn,
                  node.Config))
          {
            cachedResults = cached;
            break;
          }
        }
      }
    }
    else if (performLayout)
    {
      if (Comparison.InexactEquals(layout.CachedLayout.AvailableWidth, availableWidth) &&
          Comparison.InexactEquals(layout.CachedLayout.AvailableHeight, availableHeight) &&
          layout.CachedLayout.WidthSizingMode == widthSizingMode &&
          layout.CachedLayout.HeightSizingMode == heightSizingMode)
      {
        cachedResults = layout.CachedLayout;
      }
    }
    else
    {
      for (int i = 0; i < layout.NextCachedMeasurementsIndex; i++)
      {
        CachedMeasurement cached = layout.GetCachedMeasurement(i);
        if (Comparison.InexactEquals(cached.AvailableWidth, availableWidth) &&
            Comparison.InexactEquals(cached.AvailableHeight, availableHeight) &&
            cached.WidthSizingMode == widthSizingMode &&
            cached.HeightSizingMode == heightSizingMode)
        {
          cachedResults = cached;
          break;
        }
      }
    }

    if (!needToVisitNode && cachedResults is not null)
    {
      layout.SetMeasuredDimension(Dimension.Width, cachedResults.Value.ComputedWidth);
      layout.SetMeasuredDimension(Dimension.Height, cachedResults.Value.ComputedHeight);

      if (performLayout)
      {
        layoutMarkerData.CachedLayouts++;
      }
      else
      {
        layoutMarkerData.CachedMeasures++;
      }
    }
    else
    {
      CalculateLayoutCore.Calculate(
          node,
          availableWidth,
          availableHeight,
          ownerDirection,
          widthSizingMode,
          heightSizingMode,
          ownerWidth,
          ownerHeight,
          performLayout,
          reason,
          layoutMarkerData,
          depth,
          generationCount);

      layout.LastOwnerDirection = ownerDirection;
      layout.ConfigVersion = node.Config.Version;

      if (cachedResults is null)
      {
        layoutMarkerData.MaxMeasureCache = Math.Max(
            layoutMarkerData.MaxMeasureCache,
            layout.NextCachedMeasurementsIndex + 1);

        if (layout.NextCachedMeasurementsIndex == LayoutResults.MaxCachedMeasurements)
        {
          layout.NextCachedMeasurementsIndex = 0;
        }

        CachedMeasurement newCacheEntry = new()
        {
          AvailableWidth = availableWidth,
          AvailableHeight = availableHeight,
          WidthSizingMode = widthSizingMode,
          HeightSizingMode = heightSizingMode,
          ComputedWidth = layout.GetMeasuredDimension(Dimension.Width),
          ComputedHeight = layout.GetMeasuredDimension(Dimension.Height)
        };

        if (performLayout)
        {
          // Use the single layout cache entry.
          layout.CachedLayout = newCacheEntry;
        }
        else
        {
          // Allocate a new measurement cache entry.
          int index = (int)layout.NextCachedMeasurementsIndex;
          layout.SetCachedMeasurement(index, newCacheEntry);
          layout.NextCachedMeasurementsIndex++;
        }
      }
    }

    if (performLayout)
    {
      node.SetLayoutDimension(
          node.Layout.GetMeasuredDimension(Dimension.Width),
          Dimension.Width);
      node.SetLayoutDimension(
          node.Layout.GetMeasuredDimension(Dimension.Height),
          Dimension.Height);

      node.HasNewLayout = true;
      node.SetDirty(false);
    }

    layout.GenerationCount = (uint)generationCount;

    LayoutType layoutType;
    if (performLayout)
    {
      layoutType = !needToVisitNode && cachedResults?.Equals(layout.CachedLayout) == true
          ? LayoutType.CachedLayout
          : LayoutType.Layout;
    }
    else
    {
      layoutType = cachedResults is not null
          ? LayoutType.CachedMeasure
          : LayoutType.Measure;
    }

    YogaEvent.PublishNodeLayout(node, layoutType);

    return needToVisitNode || cachedResults is null;
  }
}
