/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGAutoMinSizeTest.cpp
 *
 * Only the default-config test is ported. The remaining tests in the C++ file
 * exercise the CSS auto-min-size feature (clearing the
 * YGErrataMinSizeUndefinedInsteadOfAuto errata bit) and the min-content
 * measure APIs (YGNodeSetMinContentMeasureFunc, YGNodeSetMinContentWidth/
 * Height, YGNodeHasMinContentMeasureFunc), none of which exist in the C# port.
 * Omitted tests: auto_min_floors_text_at_min_content_width,
 * auto_min_includes_leaf_padding_and_border_width,
 * auto_min_includes_leaf_padding_height, flex_basis_zero_floors_at_min_content,
 * content_smaller_than_specified_shrinks_to_content, auto_min_capped_by_max_size,
 * explicit_min_width_zero_opts_out, aspect_ratio_transferred_size_floors_main,
 * nested_flexbox_recurses_into_min_content, overflow_hidden_disables_auto_min,
 * min_content_measure_func_preferred_during_probe,
 * has_min_content_measure_func_tracks_setter,
 * static_min_content_width_short_circuits_probe,
 * static_min_content_short_circuits_container_recursion,
 * static_min_content_getter_setter_round_trip, errata_bit_round_trips.
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for automatic minimum sizing, ported from YGAutoMinSizeTest.cpp.
/// </summary>
public class AutoMinSizeTests
{
  // Simulates a min-content-aware text measure: the longest "word" is
  // WordWidth. Asked to be smaller than that on the main axis, the measure
  // function returns the longest-word width — that's how a real text engine
  // reports its min-content width.
  private const float WordWidth = 30.0f;
  private const float NaturalWidth = 90.0f;
  private const float LineHeight = 16.0f;

  private static YGSize MeasureWordWrappingText(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
  {
    if (widthMode == MeasureMode.AtMost)
    {
      if (width < WordWidth)
      {
        return new YGSize(WordWidth, LineHeight * 3);
      }

      if (width < NaturalWidth)
      {
        return new YGSize(width, LineHeight * 2);
      }

      return new YGSize(NaturalWidth, LineHeight);
    }

    if (widthMode == MeasureMode.Exactly)
    {
      return new YGSize(width, LineHeight);
    }

    return new YGSize(NaturalWidth, LineHeight);
  }

  // Default config (auto-min off): shrink path takes the text below its
  // content size — legacy Yoga behavior preserved.
  public void default_config_preserves_existing_shrink()
  {
    FlexConfig config = new() { UseWebDefaults = true };

    // Builds a 2-child row where the first child is shrinkable text and the
    // second is a fixed-size spacer that doesn't shrink. This forces the text
    // to absorb all the shrink when free space is negative.
    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(20f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));

    FlexNode text = new(config);
    text.SetMeasureFunc(MeasureWordWrappingText);
    text.Style.FlexBasis = StyleSizeLength.Points(NaturalWidth);
    text.Style.FlexGrow = 0f;
    text.Style.FlexShrink = 1f;
    root.InsertChild(text, 0);

    FlexNode spacer = new(config);
    spacer.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(10f));
    spacer.Style.FlexShrink = 0f;
    root.InsertChild(spacer, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Container 20 - spacer 10 = 10 for text. Without auto-min, text shrinks
    // freely below WordWidth (30).
    text.Layout.GetDimension(Dimension.Width).ShouldBe(10.0f);
  }
}
