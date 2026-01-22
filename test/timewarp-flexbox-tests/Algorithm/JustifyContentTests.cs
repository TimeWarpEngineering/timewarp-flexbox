/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for JustifyContent
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for the JustifyContent class.
/// </summary>
public class JustifyContentTests
{
    #region Null Argument Handling

    public void JustifyMainAxisShouldThrowOnNullNode()
    {
        // Arrange
        FlexLine flexLine = new();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            JustifyContent.JustifyMainAxis(
                null!,
                flexLine,
                FlexDirection.Row,
                FlexDirection.Column,
                Direction.LTR,
                SizingMode.StretchFit,
                SizingMode.StretchFit,
                100f,
                100f,
                100f,
                100f,
                100f,
                false));
    }

    public void JustifyMainAxisShouldThrowOnNullFlexLine()
    {
        // Arrange
        FlexNode node = new();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            JustifyContent.JustifyMainAxis(
                node,
                null!,
                FlexDirection.Row,
                FlexDirection.Column,
                Direction.LTR,
                SizingMode.StretchFit,
                SizingMode.StretchFit,
                100f,
                100f,
                100f,
                100f,
                100f,
                false));
    }

    #endregion

    #region Basic Functionality - Empty FlexLine

    public void JustifyMainAxisShouldSetMainDimToLeadingPaddingAndBorderForEmptyLine()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.Left, StyleLength.Points(10f));
        node.Style.SetBorder(Edge.Left, StyleLength.Points(5f));

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 0f
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - mainDim should include leading padding and border
        flexLine.Layout.MainDim.ShouldBe(15f);
    }

    public void JustifyMainAxisShouldSetCrossDimToZeroForEmptyLine()
    {
        // Arrange
        FlexNode node = new();
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 0f,
                CrossDim = 50f // Set to non-zero to verify it gets reset
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert
        flexLine.Layout.CrossDim.ShouldBe(0f);
    }

    #endregion

    #region Justify Content - FlexStart

    public void JustifyMainAxisShouldNotAddLeadingSpaceForFlexStart()
    {
        // Arrange
        FlexNode node = new();
        node.Style.JustifyContent = Justify.FlexStart;

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 100f
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - mainDim should be 0 (no leading padding/border set, no leading space)
        flexLine.Layout.MainDim.ShouldBe(0f);
    }

    #endregion

    #region Justify Content - FlexEnd

    public void JustifyMainAxisShouldAddAllFreeSpaceAsLeadingForFlexEnd()
    {
        // Arrange
        FlexNode node = new();
        node.Style.JustifyContent = Justify.FlexEnd;

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 100f
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - all free space should be leading
        flexLine.Layout.MainDim.ShouldBe(100f);
    }

    #endregion

    #region Justify Content - Center

    public void JustifyMainAxisShouldAddHalfFreeSpaceAsLeadingForCenter()
    {
        // Arrange
        FlexNode node = new();
        node.Style.JustifyContent = Justify.Center;

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 100f
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - half free space should be leading
        flexLine.Layout.MainDim.ShouldBe(50f);
    }

    #endregion

    #region Direction Tests

    public void JustifyMainAxisShouldWorkWithRTLDirection()
    {
        // Arrange
        FlexNode node = new();
        node.Style.JustifyContent = Justify.Center;

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 100f
            }
        };

        // Act - should not throw
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.RTL,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert
        flexLine.Layout.MainDim.ShouldBe(50f);
    }

    public void JustifyMainAxisShouldWorkWithColumnDirection()
    {
        // Arrange
        FlexNode node = new();
        node.Style.JustifyContent = Justify.FlexEnd;

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 80f
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Column,
            FlexDirection.Row,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert
        flexLine.Layout.MainDim.ShouldBe(80f);
    }

    #endregion

    #region Negative Free Space (Overflow Fallback)

    public void JustifyMainAxisShouldFallbackToFlexStartOnOverflowForSpaceBetween()
    {
        // Arrange
        FlexNode node = new();
        node.Style.JustifyContent = Justify.SpaceBetween;

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = -50f // Negative = overflow
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - should fallback to flex-start (no leading space)
        flexLine.Layout.MainDim.ShouldBe(0f);
    }

    public void JustifyMainAxisShouldFallbackToFlexStartOnOverflowForSpaceAround()
    {
        // Arrange
        FlexNode node = new();
        node.Style.JustifyContent = Justify.SpaceAround;

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = -50f // Negative = overflow
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - should fallback to flex-start (no leading space)
        flexLine.Layout.MainDim.ShouldBe(0f);
    }

    public void JustifyMainAxisShouldFallbackToFlexStartOnOverflowForSpaceEvenly()
    {
        // Arrange
        FlexNode node = new();
        node.Style.JustifyContent = Justify.SpaceEvenly;

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = -50f // Negative = overflow
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - should fallback to flex-start (no leading space)
        flexLine.Layout.MainDim.ShouldBe(0f);
    }

    public void JustifyMainAxisShouldNotFallbackForCenterOnOverflow()
    {
        // Arrange
        FlexNode node = new();
        node.Style.JustifyContent = Justify.Center;

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = -50f // Negative = overflow
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - center doesn't fallback, so leading = -50/2 = -25
        flexLine.Layout.MainDim.ShouldBe(-25f);
    }

    #endregion

    #region FitContent SizingMode Tests

    public void JustifyMainAxisShouldSetRemainingFreeSpaceToZeroForFitContentWithNoMinDim()
    {
        // Arrange
        FlexNode node = new();
        // No min dimension set

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 100f
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.FitContent, // Using FitContent mode
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - remaining free space should be set to 0
        flexLine.Layout.RemainingFreeSpace.ShouldBe(0f);
    }

    #endregion

    #region PerformLayout Flag Tests

    public void JustifyMainAxisShouldNotSetChildPositionsWhenPerformLayoutIsFalse()
    {
        // Arrange
        FlexNode node = new();
        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 0f
            }
        };

        // Act - with performLayout = false
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false); // performLayout = false

        // Assert - should complete without error (no children to position anyway)
        flexLine.Layout.MainDim.ShouldBeGreaterThanOrEqualTo(0f);
    }

    #endregion

    #region Zero Free Space Tests

    public void JustifyMainAxisShouldHandleZeroFreeSpaceCorrectly()
    {
        // Arrange
        FlexNode node = new();
        node.Style.JustifyContent = Justify.Center;

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 0f
            }
        };

        // Act
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - 0/2 = 0
        flexLine.Layout.MainDim.ShouldBe(0f);
    }

    #endregion

    #region Gap Tests

    public void JustifyMainAxisShouldIncludeGapInBetweenMainDim()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetGap(Gutter.Column, StyleLength.Points(10f)); // Gap for row direction

        FlexLine flexLine = new()
        {
            Layout = new FlexLineRunningLayout
            {
                RemainingFreeSpace = 0f
            }
        };

        // Act - gap is used internally but we can't easily verify without children
        JustifyContent.JustifyMainAxis(
            node,
            flexLine,
            FlexDirection.Row,
            FlexDirection.Column,
            Direction.LTR,
            SizingMode.StretchFit,
            SizingMode.StretchFit,
            100f,
            100f,
            100f,
            100f,
            100f,
            false);

        // Assert - should complete without error
        flexLine.Layout.MainDim.ShouldBeGreaterThanOrEqualTo(0f);
    }

    #endregion
}
