/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for FlexDistribution
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for the FlexDistribution class.
/// </summary>
public class FlexDistributionTests
{
    #region DistributeFreeSpaceFirstPass - Null Argument Handling

    public void DistributeFreeSpaceFirstPassShouldThrowOnNullFlexLine()
    {
        // Arrange & Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            FlexDistribution.DistributeFreeSpaceFirstPass(
                null!,
                Direction.LTR,
                FlexDirection.Row,
                100f,
                100f,
                100f,
                100f));
    }

    #endregion

    #region DistributeFreeSpaceFirstPass - Basic Functionality

    public void DistributeFreeSpaceFirstPassShouldNotModifyEmptyFlexLine()
    {
        // Arrange
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 50f,
                TotalFlexGrowFactors = 0f,
                TotalFlexShrinkScaledFactors = 0f
            }
        };

        // Act
        FlexDistribution.DistributeFreeSpaceFirstPass(
            flexLine,
            Direction.LTR,
            FlexDirection.Row,
            100f,
            100f,
            100f,
            100f);

        // Assert
        flexLine.Layout.RemainingFreeSpace.ShouldBe(50f);
    }

    #endregion

    #region DistributeFreeSpaceSecondPass - Null Argument Handling

    public void DistributeFreeSpaceSecondPassShouldThrowOnNullFlexLine()
    {
        // Arrange
        FlexNode node = new();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            FlexDistribution.DistributeFreeSpaceSecondPass(
                null!,
                node,
                FlexDirection.Row,
                FlexDirection.Column,
                Direction.LTR,
                100f,
                100f,
                100f,
                100f,
                100f,
                100f,
                false,
                SizingMode.StretchFit,
                false,
                new LayoutData(),
                0,
                1));
    }

    public void DistributeFreeSpaceSecondPassShouldThrowOnNullNode()
    {
        // Arrange
        FlexLine flexLine = new();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            FlexDistribution.DistributeFreeSpaceSecondPass(
                flexLine,
                null!,
                FlexDirection.Row,
                FlexDirection.Column,
                Direction.LTR,
                100f,
                100f,
                100f,
                100f,
                100f,
                100f,
                false,
                SizingMode.StretchFit,
                false,
                new LayoutData(),
                0,
                1));
    }

    #endregion

    #region DistributeFreeSpaceSecondPass - Basic Functionality

    public void DistributeFreeSpaceSecondPassShouldReturnZeroDeltaForEmptyFlexLine()
    {
        // Arrange
        FlexNode node = new();
        FlexLine flexLine = new();

        // Act
        float deltaFreeSpace = FlexDistribution.DistributeFreeSpaceSecondPass(
            flexLine,
            node,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            100f,
            100f,
            100f,
            100f,
            100f,
            100f,
            false,
            SizingMode.StretchFit,
            false,
            new LayoutData(),
            0,
            1);

        // Assert
        deltaFreeSpace.ShouldBe(0f);
    }

    #endregion

    #region ResolveFlexibleLength - Null Argument Handling

    public void ResolveFlexibleLengthShouldThrowOnNullNode()
    {
        // Arrange
        FlexLine flexLine = new();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            FlexDistribution.ResolveFlexibleLength(
                null!,
                flexLine,
                FlexDirection.Row,
                FlexDirection.Column,
                Direction.LTR,
                100f,
                100f,
                100f,
                100f,
                100f,
                100f,
                false,
                SizingMode.StretchFit,
                false,
                new LayoutData(),
                0,
                1));
    }

    public void ResolveFlexibleLengthShouldThrowOnNullFlexLine()
    {
        // Arrange
        FlexNode node = new();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            FlexDistribution.ResolveFlexibleLength(
                node,
                null!,
                FlexDirection.Row,
                FlexDirection.Column,
                Direction.LTR,
                100f,
                100f,
                100f,
                100f,
                100f,
                100f,
                false,
                SizingMode.StretchFit,
                false,
                new LayoutData(),
                0,
                1));
    }

    #endregion

    #region ResolveFlexibleLength - Basic Functionality

    public void ResolveFlexibleLengthShouldNotModifyEmptyFlexLine()
    {
        // Arrange
        FlexNode node = new();
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 50f,
                TotalFlexGrowFactors = 0f,
                TotalFlexShrinkScaledFactors = 0f
            }
        };

        // Act
        FlexDistribution.ResolveFlexibleLength(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            100f,
            100f,
            100f,
            100f,
            100f,
            100f,
            false,
            SizingMode.StretchFit,
            false,
            new LayoutData(),
            0,
            1);

        // Assert - remaining free space should be preserved when no items
        flexLine.Layout.RemainingFreeSpace.ShouldBe(50f);
    }

    #endregion

    #region Direction Tests

    public void DistributeFreeSpaceFirstPassShouldWorkWithRTLDirection()
    {
        // Arrange
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 100f,
                TotalFlexGrowFactors = 0f,
                TotalFlexShrinkScaledFactors = 0f
            }
        };

        // Act - should not throw
        FlexDistribution.DistributeFreeSpaceFirstPass(
            flexLine,
            Direction.RTL,
            FlexDirection.Row,
            100f,
            100f,
            100f,
            100f);

        // Assert
        flexLine.Layout.RemainingFreeSpace.ShouldBe(100f);
    }

    public void DistributeFreeSpaceFirstPassShouldWorkWithColumnDirection()
    {
        // Arrange
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 100f,
                TotalFlexGrowFactors = 0f,
                TotalFlexShrinkScaledFactors = 0f
            }
        };

        // Act - should not throw
        FlexDistribution.DistributeFreeSpaceFirstPass(
            flexLine,
            Direction.LTR,
            FlexDirection.Column,
            100f,
            100f,
            100f,
            100f);

        // Assert
        flexLine.Layout.RemainingFreeSpace.ShouldBe(100f);
    }

    public void DistributeFreeSpaceFirstPassShouldWorkWithColumnReverseDirection()
    {
        // Arrange
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 100f,
                TotalFlexGrowFactors = 0f,
                TotalFlexShrinkScaledFactors = 0f
            }
        };

        // Act - should not throw
        FlexDistribution.DistributeFreeSpaceFirstPass(
            flexLine,
            Direction.LTR,
            FlexDirection.ColumnReverse,
            100f,
            100f,
            100f,
            100f);

        // Assert
        flexLine.Layout.RemainingFreeSpace.ShouldBe(100f);
    }

    #endregion

    #region Negative Free Space (Shrink Case)

    public void DistributeFreeSpaceFirstPassShouldHandleNegativeRemainingFreeSpace()
    {
        // Arrange - negative free space means we need to shrink
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = -50f,
                TotalFlexGrowFactors = 0f,
                TotalFlexShrinkScaledFactors = 0f
            }
        };

        // Act - should not throw
        FlexDistribution.DistributeFreeSpaceFirstPass(
            flexLine,
            Direction.LTR,
            FlexDirection.Row,
            100f,
            100f,
            100f,
            100f);

        // Assert - no items, so space unchanged
        flexLine.Layout.RemainingFreeSpace.ShouldBe(-50f);
    }

    #endregion

    #region Positive Free Space (Grow Case)

    public void DistributeFreeSpaceFirstPassShouldHandlePositiveRemainingFreeSpace()
    {
        // Arrange - positive free space means we can grow
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 100f,
                TotalFlexGrowFactors = 0f,
                TotalFlexShrinkScaledFactors = 0f
            }
        };

        // Act - should not throw
        FlexDistribution.DistributeFreeSpaceFirstPass(
            flexLine,
            Direction.LTR,
            FlexDirection.Row,
            100f,
            100f,
            100f,
            100f);

        // Assert - no items with flex grow, so space unchanged
        flexLine.Layout.RemainingFreeSpace.ShouldBe(100f);
    }

    #endregion

    #region Zero Free Space

    public void DistributeFreeSpaceFirstPassShouldHandleZeroRemainingFreeSpace()
    {
        // Arrange - zero free space means perfect fit
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 0f,
                TotalFlexGrowFactors = 0f,
                TotalFlexShrinkScaledFactors = 0f
            }
        };

        // Act - should not throw
        FlexDistribution.DistributeFreeSpaceFirstPass(
            flexLine,
            Direction.LTR,
            FlexDirection.Row,
            100f,
            100f,
            100f,
            100f);

        // Assert
        flexLine.Layout.RemainingFreeSpace.ShouldBe(0f);
    }

    #endregion

    #region ResolveFlexibleLength - Orchestration

    public void ResolveFlexibleLengthShouldPreserveOriginalFreeSpaceCalculation()
    {
        // Arrange
        FlexNode node = new();
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 100f,
                TotalFlexGrowFactors = 0f,
                TotalFlexShrinkScaledFactors = 0f
            }
        };

        // Act
        FlexDistribution.ResolveFlexibleLength(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            100f,
            100f,
            100f,
            100f,
            100f,
            100f,
            false,
            SizingMode.StretchFit,
            false,
            new LayoutData(),
            0,
            1);

        // Assert - no items, so originalFreeSpace - 0 = originalFreeSpace
        flexLine.Layout.RemainingFreeSpace.ShouldBe(100f);
    }

    #endregion

    #region SizingMode Tests

    public void DistributeFreeSpaceSecondPassShouldWorkWithFitContentMode()
    {
        // Arrange
        FlexNode node = new();
        FlexLine flexLine = new();

        // Act
        float deltaFreeSpace = FlexDistribution.DistributeFreeSpaceSecondPass(
            flexLine,
            node,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            100f,
            100f,
            100f,
            100f,
            100f,
            100f,
            false,
            SizingMode.FitContent,
            false,
            new LayoutData(),
            0,
            1);

        // Assert
        deltaFreeSpace.ShouldBe(0f);
    }

    public void DistributeFreeSpaceSecondPassShouldWorkWithMaxContentMode()
    {
        // Arrange
        FlexNode node = new();
        FlexLine flexLine = new();

        // Act
        float deltaFreeSpace = FlexDistribution.DistributeFreeSpaceSecondPass(
            flexLine,
            node,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            100f,
            100f,
            100f,
            100f,
            100f,
            100f,
            false,
            SizingMode.MaxContent,
            false,
            new LayoutData(),
            0,
            1);

        // Assert
        deltaFreeSpace.ShouldBe(0f);
    }

    #endregion

    #region Overflow Tests

    public void DistributeFreeSpaceSecondPassShouldHandleMainAxisOverflow()
    {
        // Arrange
        FlexNode node = new();
        FlexLine flexLine = new();

        // Act
        float deltaFreeSpace = FlexDistribution.DistributeFreeSpaceSecondPass(
            flexLine,
            node,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            100f,
            100f,
            100f,
            100f,
            100f,
            100f,
            true, // mainAxisOverflows = true
            SizingMode.StretchFit,
            false,
            new LayoutData(),
            0,
            1);

        // Assert
        deltaFreeSpace.ShouldBe(0f);
    }

    #endregion

    #region Perform Layout Tests

    public void DistributeFreeSpaceSecondPassShouldHandlePerformLayoutTrue()
    {
        // Arrange
        FlexNode node = new();
        FlexLine flexLine = new();

        // Act
        float deltaFreeSpace = FlexDistribution.DistributeFreeSpaceSecondPass(
            flexLine,
            node,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            100f,
            100f,
            100f,
            100f,
            100f,
            100f,
            false,
            SizingMode.StretchFit,
            true, // performLayout = true
            new LayoutData(),
            0,
            1);

        // Assert
        deltaFreeSpace.ShouldBe(0f);
    }

    #endregion
}
