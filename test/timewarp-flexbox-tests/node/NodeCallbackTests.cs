/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ tests: tests/YGNodeCallbackTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Node_;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for measure and baseline callbacks.
/// Ported from C++ YGNodeCallbackTest.cpp.
/// </summary>
public class NodeCallbackTests
{
  public void hasMeasureFunc_initial()
  {
    FlexNode n = new();
    n.HasMeasureFunc.ShouldBeFalse();
  }

  public void hasMeasureFunc_with_measure_fn()
  {
    FlexNode n = new();
    n.SetMeasureFunc((_, _, _, _, _) => new YGSize());
    n.HasMeasureFunc.ShouldBeTrue();
  }

  public void measure_with_measure_fn()
  {
    FlexNode n = new();

    n.SetMeasureFunc((_, w, wm, h, hm) =>
        new YGSize(w * (int)wm, h / (int)hm));

    n.Measure(23f, MeasureMode.Exactly, 24f, MeasureMode.AtMost)
        .ShouldBe(new YGSize(23f, 12f));
  }

  public void hasMeasureFunc_after_unset()
  {
    FlexNode n = new();
    n.SetMeasureFunc((_, _, _, _, _) => new YGSize());

    n.SetMeasureFunc(null);
    n.HasMeasureFunc.ShouldBeFalse();
  }

  public void hasBaselineFunc_initial()
  {
    FlexNode n = new();
    n.HasBaselineFunc.ShouldBeFalse();
  }

  public void hasBaselineFunc_with_baseline_fn()
  {
    FlexNode n = new();
    n.BaselineFunc = (_, _, _) => 0.0f;
    n.HasBaselineFunc.ShouldBeTrue();
  }

  public void baseline_with_baseline_fn()
  {
    FlexNode n = new();
    n.BaselineFunc = (_, w, h) => w + h;

    n.Baseline(1.25f, 2.5f).ShouldBe(3.75f);
  }

  public void hasBaselineFunc_after_unset()
  {
    FlexNode n = new();
    n.BaselineFunc = (_, _, _) => 0.0f;

    n.BaselineFunc = null;
    n.HasBaselineFunc.ShouldBeFalse();
  }
}
