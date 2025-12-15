/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for YGEnums enum definitions and string conversions
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Enums;

/// <summary>
/// Tests for Yoga enum ordinal counts and string conversions.
/// </summary>
public class YGEnumsTests
{
    #region Ordinal Count Tests

    public void AlignShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Align>().ShouldBe(10);
    }

    public void BoxSizingShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<BoxSizing>().ShouldBe(2);
    }

    public void DimensionShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Dimension>().ShouldBe(2);
    }

    public void DirectionShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Direction>().ShouldBe(3);
    }

    public void DisplayShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Display>().ShouldBe(3);
    }

    public void EdgeShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Edge>().ShouldBe(9);
    }

    public void ErrataShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Errata>().ShouldBe(6);
    }

    public void ExperimentalFeatureShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<ExperimentalFeature>().ShouldBe(1);
    }

    public void FlexDirectionShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<FlexDirection>().ShouldBe(4);
    }

    public void GutterShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Gutter>().ShouldBe(3);
    }

    public void JustifyShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Justify>().ShouldBe(6);
    }

    public void LogLevelShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<LogLevel>().ShouldBe(6);
    }

    public void MeasureModeShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<MeasureMode>().ShouldBe(3);
    }

    public void NodeTypeShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<NodeType>().ShouldBe(2);
    }

    public void OverflowShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Overflow>().ShouldBe(3);
    }

    public void PositionTypeShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<PositionType>().ShouldBe(3);
    }

    public void UnitShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Unit>().ShouldBe(7);
    }

    public void WrapShouldHaveCorrectOrdinalCount()
    {
        YogaEnums.OrdinalCount<Wrap>().ShouldBe(3);
    }

    #endregion

    #region Align String Conversion Tests

    public void AlignAutoToCssStringShouldReturnAuto()
    {
        Align.Auto.ToCssString().ShouldBe("auto");
    }

    public void AlignFlexStartToCssStringShouldReturnFlexStart()
    {
        Align.FlexStart.ToCssString().ShouldBe("flex-start");
    }

    public void AlignCenterToCssStringShouldReturnCenter()
    {
        Align.Center.ToCssString().ShouldBe("center");
    }

    public void AlignFlexEndToCssStringShouldReturnFlexEnd()
    {
        Align.FlexEnd.ToCssString().ShouldBe("flex-end");
    }

    public void AlignStretchToCssStringShouldReturnStretch()
    {
        Align.Stretch.ToCssString().ShouldBe("stretch");
    }

    public void AlignBaselineToCssStringShouldReturnBaseline()
    {
        Align.Baseline.ToCssString().ShouldBe("baseline");
    }

    public void AlignSpaceBetweenToCssStringShouldReturnSpaceBetween()
    {
        Align.SpaceBetween.ToCssString().ShouldBe("space-between");
    }

    public void AlignSpaceAroundToCssStringShouldReturnSpaceAround()
    {
        Align.SpaceAround.ToCssString().ShouldBe("space-around");
    }

    public void AlignSpaceEvenlyToCssStringShouldReturnSpaceEvenly()
    {
        Align.SpaceEvenly.ToCssString().ShouldBe("space-evenly");
    }

    #endregion

    #region BoxSizing String Conversion Tests

    public void BoxSizingBorderBoxToCssStringShouldReturnBorderBox()
    {
        BoxSizing.BorderBox.ToCssString().ShouldBe("border-box");
    }

    public void BoxSizingContentBoxToCssStringShouldReturnContentBox()
    {
        BoxSizing.ContentBox.ToCssString().ShouldBe("content-box");
    }

    #endregion

    #region Direction String Conversion Tests

    public void DirectionInheritToCssStringShouldReturnInherit()
    {
        Direction.Inherit.ToCssString().ShouldBe("inherit");
    }

    public void DirectionLTRToCssStringShouldReturnLtr()
    {
        Direction.LTR.ToCssString().ShouldBe("ltr");
    }

    public void DirectionRTLToCssStringShouldReturnRtl()
    {
        Direction.RTL.ToCssString().ShouldBe("rtl");
    }

    #endregion

    #region Display String Conversion Tests

    public void DisplayFlexToCssStringShouldReturnFlex()
    {
        Display.Flex.ToCssString().ShouldBe("flex");
    }

    public void DisplayNoneToCssStringShouldReturnNone()
    {
        Display.None.ToCssString().ShouldBe("none");
    }

    public void DisplayContentsToCssStringShouldReturnContents()
    {
        Display.Contents.ToCssString().ShouldBe("contents");
    }

    #endregion

    #region Edge String Conversion Tests

    public void EdgeLeftToCssStringShouldReturnLeft()
    {
        Edge.Left.ToCssString().ShouldBe("left");
    }

    public void EdgeTopToCssStringShouldReturnTop()
    {
        Edge.Top.ToCssString().ShouldBe("top");
    }

    public void EdgeRightToCssStringShouldReturnRight()
    {
        Edge.Right.ToCssString().ShouldBe("right");
    }

    public void EdgeBottomToCssStringShouldReturnBottom()
    {
        Edge.Bottom.ToCssString().ShouldBe("bottom");
    }

    public void EdgeStartToCssStringShouldReturnStart()
    {
        Edge.Start.ToCssString().ShouldBe("start");
    }

    public void EdgeEndToCssStringShouldReturnEnd()
    {
        Edge.End.ToCssString().ShouldBe("end");
    }

    public void EdgeHorizontalToCssStringShouldReturnHorizontal()
    {
        Edge.Horizontal.ToCssString().ShouldBe("horizontal");
    }

    public void EdgeVerticalToCssStringShouldReturnVertical()
    {
        Edge.Vertical.ToCssString().ShouldBe("vertical");
    }

    public void EdgeAllToCssStringShouldReturnAll()
    {
        Edge.All.ToCssString().ShouldBe("all");
    }

    #endregion

    #region FlexDirection String Conversion Tests

    public void FlexDirectionColumnToCssStringShouldReturnColumn()
    {
        FlexDirection.Column.ToCssString().ShouldBe("column");
    }

    public void FlexDirectionColumnReverseToCssStringShouldReturnColumnReverse()
    {
        FlexDirection.ColumnReverse.ToCssString().ShouldBe("column-reverse");
    }

    public void FlexDirectionRowToCssStringShouldReturnRow()
    {
        FlexDirection.Row.ToCssString().ShouldBe("row");
    }

    public void FlexDirectionRowReverseToCssStringShouldReturnRowReverse()
    {
        FlexDirection.RowReverse.ToCssString().ShouldBe("row-reverse");
    }

    #endregion

    #region Justify String Conversion Tests

    public void JustifyFlexStartToCssStringShouldReturnFlexStart()
    {
        Justify.FlexStart.ToCssString().ShouldBe("flex-start");
    }

    public void JustifyCenterToCssStringShouldReturnCenter()
    {
        Justify.Center.ToCssString().ShouldBe("center");
    }

    public void JustifyFlexEndToCssStringShouldReturnFlexEnd()
    {
        Justify.FlexEnd.ToCssString().ShouldBe("flex-end");
    }

    public void JustifySpaceBetweenToCssStringShouldReturnSpaceBetween()
    {
        Justify.SpaceBetween.ToCssString().ShouldBe("space-between");
    }

    public void JustifySpaceAroundToCssStringShouldReturnSpaceAround()
    {
        Justify.SpaceAround.ToCssString().ShouldBe("space-around");
    }

    public void JustifySpaceEvenlyToCssStringShouldReturnSpaceEvenly()
    {
        Justify.SpaceEvenly.ToCssString().ShouldBe("space-evenly");
    }

    #endregion

    #region Overflow String Conversion Tests

    public void OverflowVisibleToCssStringShouldReturnVisible()
    {
        Overflow.Visible.ToCssString().ShouldBe("visible");
    }

    public void OverflowHiddenToCssStringShouldReturnHidden()
    {
        Overflow.Hidden.ToCssString().ShouldBe("hidden");
    }

    public void OverflowScrollToCssStringShouldReturnScroll()
    {
        Overflow.Scroll.ToCssString().ShouldBe("scroll");
    }

    #endregion

    #region PositionType String Conversion Tests

    public void PositionTypeStaticToCssStringShouldReturnStatic()
    {
        PositionType.Static.ToCssString().ShouldBe("static");
    }

    public void PositionTypeRelativeToCssStringShouldReturnRelative()
    {
        PositionType.Relative.ToCssString().ShouldBe("relative");
    }

    public void PositionTypeAbsoluteToCssStringShouldReturnAbsolute()
    {
        PositionType.Absolute.ToCssString().ShouldBe("absolute");
    }

    #endregion

    #region Unit String Conversion Tests

    public void UnitUndefinedToCssStringShouldReturnUndefined()
    {
        Unit.Undefined.ToCssString().ShouldBe("undefined");
    }

    public void UnitPointToCssStringShouldReturnPoint()
    {
        Unit.Point.ToCssString().ShouldBe("point");
    }

    public void UnitPercentToCssStringShouldReturnPercent()
    {
        Unit.Percent.ToCssString().ShouldBe("percent");
    }

    public void UnitAutoToCssStringShouldReturnAuto()
    {
        Unit.Auto.ToCssString().ShouldBe("auto");
    }

    public void UnitMaxContentToCssStringShouldReturnMaxContent()
    {
        Unit.MaxContent.ToCssString().ShouldBe("max-content");
    }

    public void UnitFitContentToCssStringShouldReturnFitContent()
    {
        Unit.FitContent.ToCssString().ShouldBe("fit-content");
    }

    public void UnitStretchToCssStringShouldReturnStretch()
    {
        Unit.Stretch.ToCssString().ShouldBe("stretch");
    }

    #endregion

    #region Wrap String Conversion Tests

    public void WrapNoWrapToCssStringShouldReturnNoWrap()
    {
        Wrap.NoWrap.ToCssString().ShouldBe("no-wrap");
    }

    public void WrapWrapToCssStringShouldReturnWrap()
    {
        Wrap.Wrap.ToCssString().ShouldBe("wrap");
    }

    public void WrapWrapReverseToCssStringShouldReturnWrapReverse()
    {
        Wrap.WrapReverse.ToCssString().ShouldBe("wrap-reverse");
    }

    #endregion

    #region Errata Flags Tests

    public void ErrataNoneShouldHaveValueZero()
    {
        ((int)Errata.None).ShouldBe(0);
    }

    public void ErrataStretchFlexBasisShouldHaveValueOne()
    {
        ((int)Errata.StretchFlexBasis).ShouldBe(1);
    }

    public void ErrataAllShouldHaveValueMaxInt()
    {
        ((int)Errata.All).ShouldBe(2147483647);
    }

    public void ErrataClassicShouldHaveValueMaxIntMinusOne()
    {
        ((int)Errata.Classic).ShouldBe(2147483646);
    }

    public void ErrataShouldSupportBitwiseOr()
    {
        Errata combined = Errata.StretchFlexBasis | Errata.AbsolutePercentAgainstInnerSize;
        ((int)combined).ShouldBe(5); // 1 | 4 = 5
    }

    public void ErrataShouldSupportHasFlag()
    {
        Errata combined = Errata.StretchFlexBasis | Errata.AbsolutePercentAgainstInnerSize;
        combined.HasFlag(Errata.StretchFlexBasis).ShouldBeTrue();
        combined.HasFlag(Errata.AbsolutePercentAgainstInnerSize).ShouldBeTrue();
        combined.HasFlag(Errata.AbsolutePositionWithoutInsetsExcludesPadding).ShouldBeFalse();
    }

    #endregion

    #region Ordinals Iteration Tests

    public void FlexDirectionOrdinalsShouldIterateAllValues()
    {
        List<FlexDirection> values = [.. YogaEnums.Ordinals<FlexDirection>()];
        values.Count.ShouldBe(4);
        values[0].ShouldBe(FlexDirection.Column);
        values[1].ShouldBe(FlexDirection.ColumnReverse);
        values[2].ShouldBe(FlexDirection.Row);
        values[3].ShouldBe(FlexDirection.RowReverse);
    }

    public void DisplayOrdinalsShouldIterateAllValues()
    {
        List<Display> values = [.. YogaEnums.Ordinals<Display>()];
        values.Count.ShouldBe(3);
        values[0].ShouldBe(Display.Flex);
        values[1].ShouldBe(Display.None);
        values[2].ShouldBe(Display.Contents);
    }

    #endregion

    #region ToUnderlying Tests

    public void ToUnderlyingFlexDirectionColumnShouldReturnZero()
    {
        YogaEnums.ToUnderlying(FlexDirection.Column).ShouldBe(0);
    }

    public void ToUnderlyingFlexDirectionRowReverseShouldReturnThree()
    {
        YogaEnums.ToUnderlying(FlexDirection.RowReverse).ShouldBe(3);
    }

    public void ToUnderlyingEdgeAllShouldReturnEight()
    {
        YogaEnums.ToUnderlying(Edge.All).ShouldBe(8);
    }

    #endregion

    #region BitCount Tests

    public void BitCountBoxSizingShouldReturnOne()
    {
        // 2 values, max ordinal = 1, bit_width(1) = 1
        YogaEnums.BitCount<BoxSizing>().ShouldBe(1);
    }

    public void BitCountFlexDirectionShouldReturnTwo()
    {
        // 4 values, max ordinal = 3, bit_width(3) = 2
        YogaEnums.BitCount<FlexDirection>().ShouldBe(2);
    }

    public void BitCountEdgeShouldReturnFour()
    {
        // 9 values, max ordinal = 8, bit_width(8) = 4
        YogaEnums.BitCount<Edge>().ShouldBe(4);
    }

    public void BitCountAlignShouldReturnFour()
    {
        // 10 values, max ordinal = 9, bit_width(9) = 4
        YogaEnums.BitCount<Align>().ShouldBe(4);
    }

    #endregion
}
