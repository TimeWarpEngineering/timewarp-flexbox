/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for FlexBasis calculation functions.
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for <see cref="FlexBasis"/> class.
/// </summary>
public sealed class FlexBasisTests : IDisposable
{
    /// <summary>
    /// Mock implementation of calculateLayoutInternal for testing.
    /// Sets measured dimensions based on the sizing modes.
    /// </summary>
    private static bool MockCalculateLayoutInternal(
        FlexNode node,
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
        uint generationCount)
    {
        // Simple mock: use available dimensions if in StretchFit, otherwise use a default size
        float width = widthSizingMode == SizingMode.StretchFit ? availableWidth : 50;
        float height = heightSizingMode == SizingMode.StretchFit ? availableHeight : 50;

        // Handle undefined dimensions
        if (Comparison.IsUndefined(width))
        {
            width = 50;
        }

        if (Comparison.IsUndefined(height))
        {
            height = 50;
        }

        node.SetLayoutMeasuredDimension(width, Dimension.Width);
        node.SetLayoutMeasuredDimension(height, Dimension.Height);

        layoutMarkerData.Measures++;
        return true;
    }

    public FlexBasisTests()
    {
        // Set up the mock delegate before each test
        FlexBasis.CalculateLayoutInternal = MockCalculateLayoutInternal;
    }

    public void Dispose()
    {
        // Clean up the delegate after each test
        FlexBasis.CalculateLayoutInternal = null;
        GC.SuppressFinalize(this);
    }

    #region ComputeFlexBasisForChild Tests

    public void ComputeFlexBasisForChildShouldThrowForNullNode()
    {
        FlexNode child = new();
        LayoutData layoutData = new();

        Should.Throw<ArgumentNullException>(() =>
            FlexBasis.ComputeFlexBasisForChild(
                null!,
                child,
                100,
                SizingMode.StretchFit,
                100,
                100,
                100,
                SizingMode.StretchFit,
                Direction.LTR,
                layoutData,
                0,
                1));
    }

    public void ComputeFlexBasisForChildShouldThrowForNullChild()
    {
        FlexNode node = new();
        LayoutData layoutData = new();

        Should.Throw<ArgumentNullException>(() =>
            FlexBasis.ComputeFlexBasisForChild(
                node,
                null!,
                100,
                SizingMode.StretchFit,
                100,
                100,
                100,
                SizingMode.StretchFit,
                Direction.LTR,
                layoutData,
                0,
                1));
    }

    public void ComputeFlexBasisForChildShouldThrowForNullLayoutData()
    {
        FlexNode node = new();
        FlexNode child = new();

        Should.Throw<ArgumentNullException>(() =>
            FlexBasis.ComputeFlexBasisForChild(
                node,
                child,
                100,
                SizingMode.StretchFit,
                100,
                100,
                100,
                SizingMode.StretchFit,
                Direction.LTR,
                null!,
                0,
                1));
    }

    public void ComputeFlexBasisForChildShouldUseExplicitFlexBasis()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child = new();
        child.Style.FlexBasis = StyleSizeLength.Points(75);
        node.InsertChild(child, 0);
        child.Owner = node;

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChild(
            node,
            child,
            200, // width
            SizingMode.StretchFit,
            100, // height
            200, // ownerWidth
            100, // ownerHeight
            SizingMode.StretchFit,
            Direction.LTR,
            layoutData,
            0,
            1);

        child.Layout.ComputedFlexBasis.IsDefined.ShouldBeTrue();
        child.Layout.ComputedFlexBasis.Unwrap().ShouldBe(75);
    }

    public void ComputeFlexBasisForChildShouldUseDefiniteWidthForRowDirection()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child = new();
        child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80));
        node.InsertChild(child, 0);
        child.Owner = node;
        child.ProcessDimensions();

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChild(
            node,
            child,
            200,
            SizingMode.StretchFit,
            100,
            200,
            100,
            SizingMode.StretchFit,
            Direction.LTR,
            layoutData,
            0,
            1);

        child.Layout.ComputedFlexBasis.IsDefined.ShouldBeTrue();
        child.Layout.ComputedFlexBasis.Unwrap().ShouldBe(80);
    }

    public void ComputeFlexBasisForChildShouldUseDefiniteHeightForColumnDirection()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Column;

        FlexNode child = new();
        child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(60));
        node.InsertChild(child, 0);
        child.Owner = node;
        child.ProcessDimensions();

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChild(
            node,
            child,
            200,
            SizingMode.StretchFit,
            100,
            200,
            100,
            SizingMode.StretchFit,
            Direction.LTR,
            layoutData,
            0,
            1);

        child.Layout.ComputedFlexBasis.IsDefined.ShouldBeTrue();
        child.Layout.ComputedFlexBasis.Unwrap().ShouldBe(60);
    }

    public void ComputeFlexBasisForChildShouldMeasureWhenAutoFlexBasis()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child = new();
        // FlexBasis is Auto by default, no width/height set
        node.InsertChild(child, 0);
        child.Owner = node;
        child.ProcessDimensions();

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChild(
            node,
            child,
            float.NaN, // undefined width
            SizingMode.MaxContent,
            float.NaN, // undefined height
            200,
            100,
            SizingMode.MaxContent,
            Direction.LTR,
            layoutData,
            0,
            1);

        // The mock sets 50 as the default measured dimension
        child.Layout.ComputedFlexBasis.IsDefined.ShouldBeTrue();
        child.Layout.ComputedFlexBasis.Unwrap().ShouldBe(50);
        layoutData.Measures.ShouldBe(1);
    }

    public void ComputeFlexBasisForChildShouldSetGenerationCount()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child = new();
        child.Style.FlexBasis = StyleSizeLength.Points(100);
        node.InsertChild(child, 0);
        child.Owner = node;

        LayoutData layoutData = new();
        uint generationCount = 42;

        FlexBasis.ComputeFlexBasisForChild(
            node,
            child,
            200,
            SizingMode.StretchFit,
            100,
            200,
            100,
            SizingMode.StretchFit,
            Direction.LTR,
            layoutData,
            0,
            generationCount);

        child.Layout.ComputedFlexBasisGeneration.ShouldBe(generationCount);
    }

    public void ComputeFlexBasisForChildShouldHandlePercentageFlexBasis()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child = new();
        child.Style.FlexBasis = StyleSizeLength.Percent(50);
        node.InsertChild(child, 0);
        child.Owner = node;

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChild(
            node,
            child,
            200, // width
            SizingMode.StretchFit,
            100, // height
            200, // ownerWidth (used for percentage calculation)
            100, // ownerHeight
            SizingMode.StretchFit,
            Direction.LTR,
            layoutData,
            0,
            1);

        child.Layout.ComputedFlexBasis.IsDefined.ShouldBeTrue();
        // 50% of 200 = 100
        child.Layout.ComputedFlexBasis.Unwrap().ShouldBe(100);
    }

    public void ComputeFlexBasisForChildShouldRespectMinPaddingAndBorder()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child = new();
        child.Style.FlexBasis = StyleSizeLength.Points(10); // Small flex basis
        child.Style.SetPadding(Edge.Left, StyleLength.Points(15));
        child.Style.SetPadding(Edge.Right, StyleLength.Points(15));
        child.Style.SetBorder(Edge.Left, StyleLength.Points(5));
        child.Style.SetBorder(Edge.Right, StyleLength.Points(5));
        // Total padding + border = 40
        node.InsertChild(child, 0);
        child.Owner = node;

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChild(
            node,
            child,
            200,
            SizingMode.StretchFit,
            100,
            200,
            100,
            SizingMode.StretchFit,
            Direction.LTR,
            layoutData,
            0,
            1);

        // Flex basis should be at least padding + border = 40
        child.Layout.ComputedFlexBasis.IsDefined.ShouldBeTrue();
        child.Layout.ComputedFlexBasis.Unwrap().ShouldBeGreaterThanOrEqualTo(40);
    }

    #endregion

    #region ComputeFlexBasisForChildren Tests

    public void ComputeFlexBasisForChildrenShouldThrowForNullNode()
    {
        LayoutData layoutData = new();

        Should.Throw<ArgumentNullException>(() =>
            FlexBasis.ComputeFlexBasisForChildren(
                null!,
                200,
                100,
                SizingMode.StretchFit,
                SizingMode.StretchFit,
                Direction.LTR,
                FlexDirection.Row,
                layoutData,
                0,
                1));
    }

    public void ComputeFlexBasisForChildrenShouldThrowForNullLayoutData()
    {
        FlexNode node = new();

        Should.Throw<ArgumentNullException>(() =>
            FlexBasis.ComputeFlexBasisForChildren(
                node,
                200,
                100,
                SizingMode.StretchFit,
                SizingMode.StretchFit,
                Direction.LTR,
                FlexDirection.Row,
                null!,
                0,
                1));
    }

    public void ComputeFlexBasisForChildrenShouldReturnZeroForNodeWithNoChildren()
    {
        FlexNode node = new();
        LayoutData layoutData = new();

        float result = FlexBasis.ComputeFlexBasisForChildren(
            node,
            200,
            100,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            Direction.LTR,
            FlexDirection.Row,
            layoutData,
            0,
            1);

        result.ShouldBe(0);
    }

    public void ComputeFlexBasisForChildrenShouldComputeTotalOuterFlexBasis()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        // Add three children with different flex basis values
        FlexNode child1 = new();
        child1.Style.FlexBasis = StyleSizeLength.Points(50);
        node.InsertChild(child1, 0);
        child1.Owner = node;

        FlexNode child2 = new();
        child2.Style.FlexBasis = StyleSizeLength.Points(75);
        node.InsertChild(child2, 1);
        child2.Owner = node;

        FlexNode child3 = new();
        child3.Style.FlexBasis = StyleSizeLength.Points(25);
        node.InsertChild(child3, 2);
        child3.Owner = node;

        LayoutData layoutData = new();

        float result = FlexBasis.ComputeFlexBasisForChildren(
            node,
            300,
            100,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            Direction.LTR,
            FlexDirection.Row,
            layoutData,
            0,
            1);

        // Total should be 50 + 75 + 25 = 150 (plus any margins, which are 0)
        result.ShouldBe(150);
    }

    public void ComputeFlexBasisForChildrenShouldSkipDisplayNoneChildren()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child1 = new();
        child1.Style.FlexBasis = StyleSizeLength.Points(50);
        node.InsertChild(child1, 0);
        child1.Owner = node;

        FlexNode hiddenChild = new();
        hiddenChild.Style.Display = Display.None;
        hiddenChild.Style.FlexBasis = StyleSizeLength.Points(100);
        node.InsertChild(hiddenChild, 1);
        hiddenChild.Owner = node;

        FlexNode child2 = new();
        child2.Style.FlexBasis = StyleSizeLength.Points(50);
        node.InsertChild(child2, 2);
        child2.Owner = node;

        LayoutData layoutData = new();

        float result = FlexBasis.ComputeFlexBasisForChildren(
            node,
            300,
            100,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            Direction.LTR,
            FlexDirection.Row,
            layoutData,
            0,
            1);

        // Hidden child should be skipped: 50 + 50 = 100
        result.ShouldBe(100);
    }

    public void ComputeFlexBasisForChildrenShouldSkipAbsolutePositionedChildren()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child1 = new();
        child1.Style.FlexBasis = StyleSizeLength.Points(50);
        node.InsertChild(child1, 0);
        child1.Owner = node;

        FlexNode absoluteChild = new();
        absoluteChild.Style.PositionType = PositionType.Absolute;
        absoluteChild.Style.FlexBasis = StyleSizeLength.Points(100);
        node.InsertChild(absoluteChild, 1);
        absoluteChild.Owner = node;

        FlexNode child2 = new();
        child2.Style.FlexBasis = StyleSizeLength.Points(50);
        node.InsertChild(child2, 2);
        child2.Owner = node;

        LayoutData layoutData = new();

        float result = FlexBasis.ComputeFlexBasisForChildren(
            node,
            300,
            100,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            Direction.LTR,
            FlexDirection.Row,
            layoutData,
            0,
            1);

        // Absolute child should be skipped: 50 + 50 = 100
        result.ShouldBe(100);
    }

    public void ComputeFlexBasisForChildrenShouldApplySingleFlexChildOptimization()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        // Single flexible child with both grow and shrink
        FlexNode flexChild = new();
        flexChild.Style.FlexGrow = new FloatOptional(1.0f);
        flexChild.Style.FlexShrink = new FloatOptional(1.0f);
        flexChild.Style.FlexBasis = StyleSizeLength.Points(100);
        node.InsertChild(flexChild, 0);
        flexChild.Owner = node;

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChildren(
            node,
            300,
            100,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            Direction.LTR,
            FlexDirection.Row,
            layoutData,
            0,
            1);

        // Single flex child optimization should set flex basis to 0
        flexChild.Layout.ComputedFlexBasis.IsDefined.ShouldBeTrue();
        flexChild.Layout.ComputedFlexBasis.Unwrap().ShouldBe(0);
    }

    public void ComputeFlexBasisForChildrenShouldNotApplySingleFlexChildOptimizationWithMultipleFlexChildren()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        // Two flexible children - optimization should not apply
        FlexNode flexChild1 = new();
        flexChild1.Style.FlexGrow = new FloatOptional(1.0f);
        flexChild1.Style.FlexShrink = new FloatOptional(1.0f);
        flexChild1.Style.FlexBasis = StyleSizeLength.Points(50);
        node.InsertChild(flexChild1, 0);
        flexChild1.Owner = node;

        FlexNode flexChild2 = new();
        flexChild2.Style.FlexGrow = new FloatOptional(1.0f);
        flexChild2.Style.FlexShrink = new FloatOptional(1.0f);
        flexChild2.Style.FlexBasis = StyleSizeLength.Points(50);
        node.InsertChild(flexChild2, 1);
        flexChild2.Owner = node;

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChildren(
            node,
            300,
            100,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            Direction.LTR,
            FlexDirection.Row,
            layoutData,
            0,
            1);

        // Multiple flex children - optimization should NOT apply
        // Each should use its explicit flex basis
        flexChild1.Layout.ComputedFlexBasis.Unwrap().ShouldBe(50);
        flexChild2.Layout.ComputedFlexBasis.Unwrap().ShouldBe(50);
    }

    public void ComputeFlexBasisForChildrenShouldIncludeMargins()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child = new();
        child.Style.FlexBasis = StyleSizeLength.Points(100);
        child.Style.SetMargin(Edge.Left, StyleLength.Points(10));
        child.Style.SetMargin(Edge.Right, StyleLength.Points(10));
        node.InsertChild(child, 0);
        child.Owner = node;

        LayoutData layoutData = new();

        float result = FlexBasis.ComputeFlexBasisForChildren(
            node,
            300,
            100,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            Direction.LTR,
            FlexDirection.Row,
            layoutData,
            0,
            1);

        // FlexBasis (100) + left margin (10) + right margin (10) = 120
        result.ShouldBe(120);
    }

    public void ComputeFlexBasisForChildrenShouldZeroOutDisplayNoneChildLayout()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode hiddenChild = new();
        hiddenChild.Style.Display = Display.None;
        hiddenChild.Style.FlexBasis = StyleSizeLength.Points(100);
        node.InsertChild(hiddenChild, 0);
        hiddenChild.Owner = node;

        // Pre-set some layout values
        hiddenChild.SetLayoutMeasuredDimension(500, Dimension.Width);
        hiddenChild.SetLayoutMeasuredDimension(500, Dimension.Height);

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChildren(
            node,
            300,
            100,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            Direction.LTR,
            FlexDirection.Row,
            layoutData,
            0,
            1);

        // Display:none children should have zeroed-out layout
        hiddenChild.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(0);
        hiddenChild.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(0);
        hiddenChild.HasNewLayout.ShouldBeTrue();
    }

    public void ComputeFlexBasisForChildrenShouldWorkWithColumnDirection()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Column;

        FlexNode child1 = new();
        child1.Style.FlexBasis = StyleSizeLength.Points(40);
        node.InsertChild(child1, 0);
        child1.Owner = node;

        FlexNode child2 = new();
        child2.Style.FlexBasis = StyleSizeLength.Points(60);
        node.InsertChild(child2, 1);
        child2.Owner = node;

        LayoutData layoutData = new();

        float result = FlexBasis.ComputeFlexBasisForChildren(
            node,
            100, // width
            200, // height
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            Direction.LTR,
            FlexDirection.Column, // main axis is vertical
            layoutData,
            0,
            1);

        result.ShouldBe(100); // 40 + 60
    }

    #endregion

    #region Edge Case Tests

    public void ComputeFlexBasisForChildShouldThrowWhenDelegateNotSet()
    {
        // Remove the delegate
        FlexBasis.CalculateLayoutInternal = null;

        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child = new();
        // No explicit flex basis or dimensions - will need to measure
        node.InsertChild(child, 0);
        child.Owner = node;
        child.ProcessDimensions();

        LayoutData layoutData = new();

        // Should throw because delegate is null and measurement is needed
        Should.Throw<YogaAssertException>(() =>
            FlexBasis.ComputeFlexBasisForChild(
                node,
                child,
                float.NaN,
                SizingMode.MaxContent,
                float.NaN,
                200,
                100,
                SizingMode.MaxContent,
                Direction.LTR,
                layoutData,
                0,
                1));
    }

    public void ComputeFlexBasisForChildShouldHandleRTLDirection()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Row;

        FlexNode child = new();
        child.Style.FlexBasis = StyleSizeLength.Points(100);
        node.InsertChild(child, 0);
        child.Owner = node;

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChild(
            node,
            child,
            200,
            SizingMode.StretchFit,
            100,
            200,
            100,
            SizingMode.StretchFit,
            Direction.RTL,
            layoutData,
            0,
            1);

        child.Layout.ComputedFlexBasis.IsDefined.ShouldBeTrue();
        child.Layout.ComputedFlexBasis.Unwrap().ShouldBe(100);
    }

    public void ComputeFlexBasisForChildShouldHandleAspectRatio()
    {
        FlexNode node = new();
        node.Style.FlexDirection = FlexDirection.Column; // Main axis is vertical

        FlexNode child = new();
        child.Style.AspectRatio = new FloatOptional(2.0f); // width = 2 * height
        child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100));
        node.InsertChild(child, 0);
        child.Owner = node;
        child.ProcessDimensions();

        LayoutData layoutData = new();

        FlexBasis.ComputeFlexBasisForChild(
            node,
            child,
            200, // available width
            SizingMode.StretchFit,
            300, // available height
            200,
            300,
            SizingMode.StretchFit,
            Direction.LTR,
            layoutData,
            0,
            1);

        // With column direction and width set, the flex basis should be derived from aspect ratio
        child.Layout.ComputedFlexBasis.IsDefined.ShouldBeTrue();
    }

    #endregion
}
