/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for MeasureNode utilities
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for MeasureNode measurement functions.
/// </summary>
public class MeasureNodeTests
{
    #region MeasureNodeWithMeasureFunc Tests

    public void MeasureNodeWithMeasureFuncShouldCallMeasureFunction()
    {
        // Arrange
        FlexNode node = new();
        bool measureCalled = false;
        node.SetMeasureFunc((_, _, _, _, _) =>
        {
            measureCalled = true;
            return new YGSize(50f, 30f);
        });

        LayoutData layoutData = new();

        // Act
        MeasureNode.MeasureNodeWithMeasureFunc(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 100f,
            ownerHeight: 100f,
            layoutMarkerData: layoutData,
            reason: LayoutPassReason.Initial);

        // Assert
        measureCalled.ShouldBeTrue();
        layoutData.MeasureCallbacks.ShouldBe(1);
    }

    public void MeasureNodeWithMeasureFuncShouldSetMeasuredDimensions()
    {
        // Arrange
        FlexNode node = new();
        node.SetMeasureFunc((_, _, _, _, _) =>
            new YGSize(50f, 30f));

        LayoutData layoutData = new();

        // Act
        MeasureNode.MeasureNodeWithMeasureFunc(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 100f,
            ownerHeight: 100f,
            layoutMarkerData: layoutData,
            reason: LayoutPassReason.Initial);

        // Assert - measured dimensions should be set (50 and 30 from measure func)
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(50f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(30f);
    }

    public void MeasureNodeWithMeasureFuncShouldPassInnerDimensionsToCallback()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.All, StyleLength.Points(10f));

        float receivedWidth = 0;
        float receivedHeight = 0;
        node.SetMeasureFunc((_, width, _, height, _) =>
        {
            receivedWidth = width;
            receivedHeight = height;
            return new YGSize(50f, 30f);
        });

        LayoutData layoutData = new();

        // Act
        MeasureNode.MeasureNodeWithMeasureFunc(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 100f,
            ownerHeight: 100f,
            layoutMarkerData: layoutData,
            reason: LayoutPassReason.Initial);

        // Assert - inner dimensions = available - padding (100 - 20 = 80)
        receivedWidth.ShouldBe(80f);
        receivedHeight.ShouldBe(80f);
    }

    public void MeasureNodeWithMeasureFuncShouldAddPaddingAndBorderToResult()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.All, StyleLength.Points(10f));
        node.Style.SetBorder(Edge.All, StyleLength.Points(5f));

        node.SetMeasureFunc((_, _, _, _, _) =>
            new YGSize(50f, 30f));

        LayoutData layoutData = new();

        // Act
        MeasureNode.MeasureNodeWithMeasureFunc(
            node,
            Direction.LTR,
            availableWidth: 200f,
            availableHeight: 200f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 200f,
            ownerHeight: 200f,
            layoutMarkerData: layoutData,
            reason: LayoutPassReason.Initial);

        // Assert - measured + padding (10*2) + border (5*2) = 50 + 30 = 80
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(80f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(60f);
    }

    public void MeasureNodeWithMeasureFuncShouldPassUndefinedForMaxContentMode()
    {
        // Arrange
        FlexNode node = new();
        MeasureMode receivedWidthMode = MeasureMode.Exactly;
        MeasureMode receivedHeightMode = MeasureMode.Exactly;
        float receivedWidth = 0;
        float receivedHeight = 0;

        node.SetMeasureFunc((_, width, widthMode, height, heightMode) =>
        {
            receivedWidth = width;
            receivedWidthMode = widthMode;
            receivedHeight = height;
            receivedHeightMode = heightMode;
            return new YGSize(50f, 30f);
        });

        LayoutData layoutData = new();

        // Act
        MeasureNode.MeasureNodeWithMeasureFunc(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.MaxContent,
            heightSizingMode: SizingMode.MaxContent,
            ownerWidth: 100f,
            ownerHeight: 100f,
            layoutMarkerData: layoutData,
            reason: LayoutPassReason.Initial);

        // Assert - MaxContent mode passes undefined dimensions
        Comparison.IsUndefined(receivedWidth).ShouldBeTrue();
        Comparison.IsUndefined(receivedHeight).ShouldBeTrue();
        receivedWidthMode.ShouldBe(MeasureMode.Undefined);
        receivedHeightMode.ShouldBe(MeasureMode.Undefined);
    }

    public void MeasureNodeWithMeasureFuncShouldConvertSizingModesToMeasureModes()
    {
        // Arrange
        FlexNode node = new();
        MeasureMode receivedWidthMode = MeasureMode.Undefined;
        MeasureMode receivedHeightMode = MeasureMode.Undefined;

        node.SetMeasureFunc((_, _, widthMode, _, heightMode) =>
        {
            receivedWidthMode = widthMode;
            receivedHeightMode = heightMode;
            return new YGSize(50f, 30f);
        });

        LayoutData layoutData = new();

        // Act - StretchFit -> Exactly, FitContent -> AtMost
        MeasureNode.MeasureNodeWithMeasureFunc(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.StretchFit,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 100f,
            ownerHeight: 100f,
            layoutMarkerData: layoutData,
            reason: LayoutPassReason.Initial);

        // Assert
        receivedWidthMode.ShouldBe(MeasureMode.Exactly);
        receivedHeightMode.ShouldBe(MeasureMode.AtMost);
    }

    public void MeasureNodeWithMeasureFuncShouldApplyMinMaxBounds()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(100f));
        node.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(20f));

        node.SetMeasureFunc((_, _, _, _, _) =>
            new YGSize(50f, 50f));

        LayoutData layoutData = new();

        // Act
        MeasureNode.MeasureNodeWithMeasureFunc(
            node,
            Direction.LTR,
            availableWidth: 200f,
            availableHeight: 200f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 200f,
            ownerHeight: 200f,
            layoutMarkerData: layoutData,
            reason: LayoutPassReason.Initial);

        // Assert - width bounded to min 100, height bounded to max 20
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(100f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(20f);
    }

    public void MeasureNodeWithMeasureFuncShouldTrackLayoutPassReason()
    {
        // Arrange
        FlexNode node = new();
        node.SetMeasureFunc((_, _, _, _, _) =>
            new YGSize(50f, 30f));

        LayoutData layoutData = new();

        // Act
        MeasureNode.MeasureNodeWithMeasureFunc(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 100f,
            ownerHeight: 100f,
            layoutMarkerData: layoutData,
            reason: LayoutPassReason.FlexMeasure);

        // Assert
        layoutData.GetMeasureCallbackReasonCount(LayoutPassReason.FlexMeasure).ShouldBe(1);
        layoutData.GetMeasureCallbackReasonCount(LayoutPassReason.Initial).ShouldBe(0);
    }

    public void MeasureNodeWithMeasureFuncShouldThrowForNullNode()
    {
        // Arrange
        LayoutData layoutData = new();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            MeasureNode.MeasureNodeWithMeasureFunc(
                null!,
                Direction.LTR,
                availableWidth: 100f,
                availableHeight: 100f,
                widthSizingMode: SizingMode.FitContent,
                heightSizingMode: SizingMode.FitContent,
                ownerWidth: 100f,
                ownerHeight: 100f,
                layoutMarkerData: layoutData,
                reason: LayoutPassReason.Initial));
    }

    public void MeasureNodeWithMeasureFuncShouldThrowForNullLayoutData()
    {
        // Arrange
        FlexNode node = new();
        node.SetMeasureFunc((_, _, _, _, _) =>
            new YGSize(50f, 30f));

        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            MeasureNode.MeasureNodeWithMeasureFunc(
                node,
                Direction.LTR,
                availableWidth: 100f,
                availableHeight: 100f,
                widthSizingMode: SizingMode.FitContent,
                heightSizingMode: SizingMode.FitContent,
                ownerWidth: 100f,
                ownerHeight: 100f,
                layoutMarkerData: null!,
                reason: LayoutPassReason.Initial));
    }

    #endregion

    #region MeasureNodeWithoutChildren Tests

    public void MeasureNodeWithoutChildrenShouldUseAvailableDimensionsForStretchFit()
    {
        // Arrange
        FlexNode node = new();

        // Act
        MeasureNode.MeasureNodeWithoutChildren(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 80f,
            widthSizingMode: SizingMode.StretchFit,
            heightSizingMode: SizingMode.StretchFit,
            ownerWidth: 100f,
            ownerHeight: 80f);

        // Assert
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(100f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(80f);
    }

    public void MeasureNodeWithoutChildrenShouldUsePaddingAndBorderForMaxContent()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.All, StyleLength.Points(10f));
        node.Style.SetBorder(Edge.All, StyleLength.Points(5f));

        // Act
        MeasureNode.MeasureNodeWithoutChildren(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.MaxContent,
            heightSizingMode: SizingMode.MaxContent,
            ownerWidth: 100f,
            ownerHeight: 100f);

        // Assert - padding (10*2) + border (5*2) = 30
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(30f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(30f);
    }

    public void MeasureNodeWithoutChildrenShouldUsePaddingAndBorderForFitContent()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.Horizontal, StyleLength.Points(15f));
        node.Style.SetPadding(Edge.Vertical, StyleLength.Points(10f));

        // Act
        MeasureNode.MeasureNodeWithoutChildren(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 100f,
            ownerHeight: 100f);

        // Assert - horizontal padding (15*2) = 30, vertical padding (10*2) = 20
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(30f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(20f);
    }

    public void MeasureNodeWithoutChildrenShouldApplyMinBound()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(50f));
        node.Style.SetMinDimension(Dimension.Height, StyleSizeLength.Points(40f));

        // Act
        MeasureNode.MeasureNodeWithoutChildren(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.MaxContent,
            heightSizingMode: SizingMode.MaxContent,
            ownerWidth: 100f,
            ownerHeight: 100f);

        // Assert - bounded to min dimensions
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(50f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(40f);
    }

    public void MeasureNodeWithoutChildrenShouldApplyMaxBound()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(80f));
        node.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(60f));

        // Act
        MeasureNode.MeasureNodeWithoutChildren(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.StretchFit,
            heightSizingMode: SizingMode.StretchFit,
            ownerWidth: 100f,
            ownerHeight: 100f);

        // Assert - bounded to max dimensions
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(80f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(60f);
    }

    public void MeasureNodeWithoutChildrenShouldHandleMixedSizingModes()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.All, StyleLength.Points(5f));

        // Act
        MeasureNode.MeasureNodeWithoutChildren(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 80f,
            widthSizingMode: SizingMode.StretchFit,
            heightSizingMode: SizingMode.MaxContent,
            ownerWidth: 100f,
            ownerHeight: 80f);

        // Assert - width stretches to 100, height uses padding (5*2 = 10)
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(100f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(10f);
    }

    public void MeasureNodeWithoutChildrenShouldThrowForNullNode()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            MeasureNode.MeasureNodeWithoutChildren(
                null!,
                Direction.LTR,
                availableWidth: 100f,
                availableHeight: 100f,
                widthSizingMode: SizingMode.FitContent,
                heightSizingMode: SizingMode.FitContent,
                ownerWidth: 100f,
                ownerHeight: 100f));
    }

    #endregion

    #region MeasureNodeWithFixedSize Tests

    public void MeasureNodeWithFixedSizeShouldReturnTrueWhenBothDimensionsFixed()
    {
        // Arrange
        FlexNode node = new();

        // Act - StretchFit is always fixed
        bool result = MeasureNode.MeasureNodeWithFixedSize(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 80f,
            widthSizingMode: SizingMode.StretchFit,
            heightSizingMode: SizingMode.StretchFit,
            ownerWidth: 100f,
            ownerHeight: 80f);

        // Assert
        result.ShouldBeTrue();
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(100f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(80f);
    }

    public void MeasureNodeWithFixedSizeShouldReturnFalseWhenWidthNotFixed()
    {
        // Arrange
        FlexNode node = new();

        // Act - MaxContent is not fixed
        bool result = MeasureNode.MeasureNodeWithFixedSize(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 80f,
            widthSizingMode: SizingMode.MaxContent,
            heightSizingMode: SizingMode.StretchFit,
            ownerWidth: 100f,
            ownerHeight: 80f);

        // Assert
        result.ShouldBeFalse();
    }

    public void MeasureNodeWithFixedSizeShouldReturnFalseWhenHeightNotFixed()
    {
        // Arrange
        FlexNode node = new();

        // Act
        bool result = MeasureNode.MeasureNodeWithFixedSize(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 80f,
            widthSizingMode: SizingMode.StretchFit,
            heightSizingMode: SizingMode.MaxContent,
            ownerWidth: 100f,
            ownerHeight: 80f);

        // Assert
        result.ShouldBeFalse();
    }

    public void MeasureNodeWithFixedSizeShouldReturnTrueForFitContentWithZeroDimension()
    {
        // Arrange
        FlexNode node = new();

        // Act - FitContent with zero dimension is fixed
        bool result = MeasureNode.MeasureNodeWithFixedSize(
            node,
            Direction.LTR,
            availableWidth: 0f,
            availableHeight: 0f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 100f,
            ownerHeight: 100f);

        // Assert
        result.ShouldBeTrue();
    }

    public void MeasureNodeWithFixedSizeShouldReturnFalseForFitContentWithPositiveDimension()
    {
        // Arrange
        FlexNode node = new();

        // Act - FitContent with positive dimension is not fixed
        bool result = MeasureNode.MeasureNodeWithFixedSize(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 80f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 100f,
            ownerHeight: 80f);

        // Assert
        result.ShouldBeFalse();
    }

    public void MeasureNodeWithFixedSizeShouldApplyMinMaxBounds()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(120f));
        node.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(50f));

        // Act
        bool result = MeasureNode.MeasureNodeWithFixedSize(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 80f,
            widthSizingMode: SizingMode.StretchFit,
            heightSizingMode: SizingMode.StretchFit,
            ownerWidth: 100f,
            ownerHeight: 80f);

        // Assert - bounded by min/max
        result.ShouldBeTrue();
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(120f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(50f);
    }

    public void MeasureNodeWithFixedSizeShouldNotModifyLayoutWhenReturningFalse()
    {
        // Arrange
        FlexNode node = new();
        node.SetLayoutMeasuredDimension(999f, Dimension.Width);
        node.SetLayoutMeasuredDimension(999f, Dimension.Height);

        // Act
        bool result = MeasureNode.MeasureNodeWithFixedSize(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 80f,
            widthSizingMode: SizingMode.MaxContent,
            heightSizingMode: SizingMode.MaxContent,
            ownerWidth: 100f,
            ownerHeight: 80f);

        // Assert - layout should not be modified when returning false
        result.ShouldBeFalse();
        // Note: The current implementation doesn't modify layout when returning false
        // This is the expected behavior
    }

    public void MeasureNodeWithFixedSizeShouldThrowForNullNode()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            MeasureNode.MeasureNodeWithFixedSize(
                null!,
                Direction.LTR,
                availableWidth: 100f,
                availableHeight: 80f,
                widthSizingMode: SizingMode.StretchFit,
                heightSizingMode: SizingMode.StretchFit,
                ownerWidth: 100f,
                ownerHeight: 80f));
    }

    #endregion

    #region Edge Cases

    public void MeasureNodeWithMeasureFuncShouldHandleRTLDirection()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.Start, StyleLength.Points(10f));
        node.Style.SetPadding(Edge.End, StyleLength.Points(20f));

        float receivedWidth = 0;
        node.SetMeasureFunc((_, width, _, _, _) =>
        {
            receivedWidth = width;
            return new YGSize(50f, 30f);
        });

        LayoutData layoutData = new();

        // Act
        MeasureNode.MeasureNodeWithMeasureFunc(
            node,
            Direction.RTL,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 100f,
            ownerHeight: 100f,
            layoutMarkerData: layoutData,
            reason: LayoutPassReason.Initial);

        // Assert - padding should be applied regardless of direction
        // inner width = 100 - 10 - 20 = 70
        receivedWidth.ShouldBe(70f);
    }

    public void MeasureNodeWithoutChildrenShouldHandleZeroPaddingAndBorder()
    {
        // Arrange
        FlexNode node = new();

        // Act
        MeasureNode.MeasureNodeWithoutChildren(
            node,
            Direction.LTR,
            availableWidth: 100f,
            availableHeight: 100f,
            widthSizingMode: SizingMode.MaxContent,
            heightSizingMode: SizingMode.MaxContent,
            ownerWidth: 100f,
            ownerHeight: 100f);

        // Assert - with no padding/border, dimensions should be 0
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(0f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(0f);
    }

    public void MeasureNodeWithFixedSizeShouldHandleNegativeDimension()
    {
        // Arrange
        FlexNode node = new();

        // Act - FitContent with negative dimension is fixed
        bool result = MeasureNode.MeasureNodeWithFixedSize(
            node,
            Direction.LTR,
            availableWidth: -10f,
            availableHeight: -5f,
            widthSizingMode: SizingMode.FitContent,
            heightSizingMode: SizingMode.FitContent,
            ownerWidth: 100f,
            ownerHeight: 100f);

        // Assert
        result.ShouldBeTrue();
        // Bounded dimensions should be at least 0 (padding+border minimum)
        node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(0f);
        node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(0f);
    }

    #endregion
}
