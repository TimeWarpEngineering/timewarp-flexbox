/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for LayoutResults class
 */

namespace TimeWarp.Flexbox.Tests.Node;

/// <summary>
/// Tests for LayoutResults class.
/// </summary>
public class LayoutResultsTests
{
    #region Default Values

    public void DefaultValuesShouldBeCorrect()
    {
        // Arrange & Act
        LayoutResults layout = new();

        // Assert - Cache fields
        layout.ComputedFlexBasisGeneration.ShouldBe(0u);
        layout.ComputedFlexBasis.IsUndefined.ShouldBeTrue();
        layout.GenerationCount.ShouldBe(0u);
        layout.ConfigVersion.ShouldBe(0u);
        layout.LastOwnerDirection.ShouldBe(Direction.Inherit);
        layout.NextCachedMeasurementsIndex.ShouldBe(0u);

        // Assert - Direction and overflow
        layout.Direction.ShouldBe(Direction.Inherit);
        layout.HadOverflow.ShouldBeFalse();

        // Assert - Dimensions should be NaN (undefined)
        Comparison.IsUndefined(layout.GetDimension(Dimension.Width)).ShouldBeTrue();
        Comparison.IsUndefined(layout.GetDimension(Dimension.Height)).ShouldBeTrue();
        Comparison.IsUndefined(layout.GetMeasuredDimension(Dimension.Width)).ShouldBeTrue();
        Comparison.IsUndefined(layout.GetMeasuredDimension(Dimension.Height)).ShouldBeTrue();
        Comparison.IsUndefined(layout.GetRawDimension(Dimension.Width)).ShouldBeTrue();
        Comparison.IsUndefined(layout.GetRawDimension(Dimension.Height)).ShouldBeTrue();

        // Assert - Position, margin, border, padding should be 0
        layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        layout.GetPosition(PhysicalEdge.Right).ShouldBe(0f);
        layout.GetPosition(PhysicalEdge.Bottom).ShouldBe(0f);

        layout.GetMargin(PhysicalEdge.Left).ShouldBe(0f);
        layout.GetMargin(PhysicalEdge.Top).ShouldBe(0f);
        layout.GetMargin(PhysicalEdge.Right).ShouldBe(0f);
        layout.GetMargin(PhysicalEdge.Bottom).ShouldBe(0f);

        layout.GetBorder(PhysicalEdge.Left).ShouldBe(0f);
        layout.GetBorder(PhysicalEdge.Top).ShouldBe(0f);
        layout.GetBorder(PhysicalEdge.Right).ShouldBe(0f);
        layout.GetBorder(PhysicalEdge.Bottom).ShouldBe(0f);

        layout.GetPadding(PhysicalEdge.Left).ShouldBe(0f);
        layout.GetPadding(PhysicalEdge.Top).ShouldBe(0f);
        layout.GetPadding(PhysicalEdge.Right).ShouldBe(0f);
        layout.GetPadding(PhysicalEdge.Bottom).ShouldBe(0f);
    }

    #endregion

    #region Cache Field Get/Set

    public void ComputedFlexBasisGenerationShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.ComputedFlexBasisGeneration = 42;

        // Assert
        layout.ComputedFlexBasisGeneration.ShouldBe(42u);
    }

    public void ComputedFlexBasisShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.ComputedFlexBasis = new FloatOptional(100.5f);

        // Assert
        layout.ComputedFlexBasis.IsDefined.ShouldBeTrue();
        layout.ComputedFlexBasis.Unwrap().ShouldBe(100.5f);
    }

    public void GenerationCountShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.GenerationCount = 123;

        // Assert
        layout.GenerationCount.ShouldBe(123u);
    }

    public void ConfigVersionShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.ConfigVersion = 456;

        // Assert
        layout.ConfigVersion.ShouldBe(456u);
    }

    public void LastOwnerDirectionShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.LastOwnerDirection = Direction.LTR;

        // Assert
        layout.LastOwnerDirection.ShouldBe(Direction.LTR);
    }

    public void NextCachedMeasurementsIndexShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.NextCachedMeasurementsIndex = 5;

        // Assert
        layout.NextCachedMeasurementsIndex.ShouldBe(5u);
    }

    public void CachedMeasurementsShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();
        CachedMeasurement measurement = new()
        {
            AvailableWidth = 100,
            AvailableHeight = 200,
            WidthSizingMode = SizingMode.StretchFit,
            HeightSizingMode = SizingMode.MaxContent,
            ComputedWidth = 100,
            ComputedHeight = 200
        };

        // Act
        layout.SetCachedMeasurement(3, measurement);

        // Assert
        CachedMeasurement retrieved = layout.GetCachedMeasurement(3);
        retrieved.AvailableWidth.ShouldBe(100f);
        retrieved.AvailableHeight.ShouldBe(200f);
        retrieved.WidthSizingMode.ShouldBe(SizingMode.StretchFit);
        retrieved.HeightSizingMode.ShouldBe(SizingMode.MaxContent);
        retrieved.ComputedWidth.ShouldBe(100f);
        retrieved.ComputedHeight.ShouldBe(200f);
    }

    public void CachedLayoutShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();
        CachedMeasurement cachedLayout = new()
        {
            AvailableWidth = 500,
            AvailableHeight = 600,
            ComputedWidth = 500,
            ComputedHeight = 600
        };

        // Act
        layout.CachedLayout = cachedLayout;

        // Assert
        layout.CachedLayout.AvailableWidth.ShouldBe(500f);
        layout.CachedLayout.AvailableHeight.ShouldBe(600f);
    }

    #endregion

    #region Direction Get/Set

    public void DirectionShouldGetSetViaMethod()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetDirection(Direction.RTL);

        // Assert
        layout.Direction.ShouldBe(Direction.RTL);
    }

    #endregion

    #region HadOverflow Get/Set

    public void HadOverflowShouldGetSetViaMethod()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetHadOverflow(true);

        // Assert
        layout.HadOverflow.ShouldBeTrue();
    }

    #endregion

    #region Dimension Get/Set

    public void DimensionWidthShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetDimension(Dimension.Width, 200f);

        // Assert
        layout.GetDimension(Dimension.Width).ShouldBe(200f);
    }

    public void DimensionHeightShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetDimension(Dimension.Height, 300f);

        // Assert
        layout.GetDimension(Dimension.Height).ShouldBe(300f);
    }

    public void MeasuredDimensionWidthShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetMeasuredDimension(Dimension.Width, 150f);

        // Assert
        layout.GetMeasuredDimension(Dimension.Width).ShouldBe(150f);
    }

    public void MeasuredDimensionHeightShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetMeasuredDimension(Dimension.Height, 250f);

        // Assert
        layout.GetMeasuredDimension(Dimension.Height).ShouldBe(250f);
    }

    public void RawDimensionWidthShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetRawDimension(Dimension.Width, 175.5f);

        // Assert
        layout.GetRawDimension(Dimension.Width).ShouldBe(175.5f);
    }

    public void RawDimensionHeightShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetRawDimension(Dimension.Height, 275.5f);

        // Assert
        layout.GetRawDimension(Dimension.Height).ShouldBe(275.5f);
    }

    #endregion

    #region Position Get/Set

    public void PositionLeftShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetPosition(PhysicalEdge.Left, 10f);

        // Assert
        layout.GetPosition(PhysicalEdge.Left).ShouldBe(10f);
    }

    public void PositionTopShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetPosition(PhysicalEdge.Top, 20f);

        // Assert
        layout.GetPosition(PhysicalEdge.Top).ShouldBe(20f);
    }

    public void PositionRightShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetPosition(PhysicalEdge.Right, 30f);

        // Assert
        layout.GetPosition(PhysicalEdge.Right).ShouldBe(30f);
    }

    public void PositionBottomShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetPosition(PhysicalEdge.Bottom, 40f);

        // Assert
        layout.GetPosition(PhysicalEdge.Bottom).ShouldBe(40f);
    }

    #endregion

    #region Margin Get/Set

    public void MarginAllEdgesShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetMargin(PhysicalEdge.Left, 5f);
        layout.SetMargin(PhysicalEdge.Top, 10f);
        layout.SetMargin(PhysicalEdge.Right, 15f);
        layout.SetMargin(PhysicalEdge.Bottom, 20f);

        // Assert
        layout.GetMargin(PhysicalEdge.Left).ShouldBe(5f);
        layout.GetMargin(PhysicalEdge.Top).ShouldBe(10f);
        layout.GetMargin(PhysicalEdge.Right).ShouldBe(15f);
        layout.GetMargin(PhysicalEdge.Bottom).ShouldBe(20f);
    }

    #endregion

    #region Border Get/Set

    public void BorderAllEdgesShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetBorder(PhysicalEdge.Left, 1f);
        layout.SetBorder(PhysicalEdge.Top, 2f);
        layout.SetBorder(PhysicalEdge.Right, 3f);
        layout.SetBorder(PhysicalEdge.Bottom, 4f);

        // Assert
        layout.GetBorder(PhysicalEdge.Left).ShouldBe(1f);
        layout.GetBorder(PhysicalEdge.Top).ShouldBe(2f);
        layout.GetBorder(PhysicalEdge.Right).ShouldBe(3f);
        layout.GetBorder(PhysicalEdge.Bottom).ShouldBe(4f);
    }

    #endregion

    #region Padding Get/Set

    public void PaddingAllEdgesShouldGetSet()
    {
        // Arrange
        LayoutResults layout = new();

        // Act
        layout.SetPadding(PhysicalEdge.Left, 8f);
        layout.SetPadding(PhysicalEdge.Top, 16f);
        layout.SetPadding(PhysicalEdge.Right, 24f);
        layout.SetPadding(PhysicalEdge.Bottom, 32f);

        // Assert
        layout.GetPadding(PhysicalEdge.Left).ShouldBe(8f);
        layout.GetPadding(PhysicalEdge.Top).ShouldBe(16f);
        layout.GetPadding(PhysicalEdge.Right).ShouldBe(24f);
        layout.GetPadding(PhysicalEdge.Bottom).ShouldBe(32f);
    }

    #endregion

    #region Equality

    public void EqualitySameValuesShouldBeEqual()
    {
        // Arrange
        LayoutResults layout1 = new();
        layout1.SetDimension(Dimension.Width, 100f);
        layout1.SetDimension(Dimension.Height, 200f);
        layout1.SetPosition(PhysicalEdge.Left, 10f);
        layout1.SetPosition(PhysicalEdge.Top, 20f);
        layout1.SetDirection(Direction.LTR);

        LayoutResults layout2 = new();
        layout2.SetDimension(Dimension.Width, 100f);
        layout2.SetDimension(Dimension.Height, 200f);
        layout2.SetPosition(PhysicalEdge.Left, 10f);
        layout2.SetPosition(PhysicalEdge.Top, 20f);
        layout2.SetDirection(Direction.LTR);

        // Assert
        layout1.Equals(layout2).ShouldBeTrue();
        (layout1 == layout2).ShouldBeTrue();
        (layout1 != layout2).ShouldBeFalse();
    }

    public void EqualityDifferentDimensionsShouldNotBeEqual()
    {
        // Arrange
        LayoutResults layout1 = new();
        layout1.SetDimension(Dimension.Width, 100f);

        LayoutResults layout2 = new();
        layout2.SetDimension(Dimension.Width, 150f);

        // Assert
        layout1.Equals(layout2).ShouldBeFalse();
        (layout1 == layout2).ShouldBeFalse();
        (layout1 != layout2).ShouldBeTrue();
    }

    public void EqualityDifferentDirectionShouldNotBeEqual()
    {
        // Arrange
        LayoutResults layout1 = new();
        layout1.SetDirection(Direction.LTR);

        LayoutResults layout2 = new();
        layout2.SetDirection(Direction.RTL);

        // Assert
        layout1.Equals(layout2).ShouldBeFalse();
    }

    public void EqualityDifferentOverflowShouldNotBeEqual()
    {
        // Arrange
        LayoutResults layout1 = new();
        layout1.SetHadOverflow(false);

        LayoutResults layout2 = new();
        layout2.SetHadOverflow(true);

        // Assert
        layout1.Equals(layout2).ShouldBeFalse();
    }

    public void EqualityNullShouldNotBeEqual()
    {
        // Arrange
        LayoutResults layout = new();
        LayoutResults? nullLayout = GetNullLayout();

        // Assert
        layout.Equals(nullLayout).ShouldBeFalse();
        (layout == nullLayout).ShouldBeFalse();
        (nullLayout == layout).ShouldBeFalse();
    }

    public void EqualitySameReferenceShouldBeEqual()
    {
        // Arrange
        LayoutResults layout = new();

        // Assert
        layout.Equals(layout).ShouldBeTrue();
#pragma warning disable CS1718 // Comparison made to same variable
        (layout == layout).ShouldBeTrue();
#pragma warning restore CS1718
    }

    public void EqualityBothNullShouldBeEqual()
    {
        // Arrange
        LayoutResults? layout1 = GetNullLayout();
        LayoutResults? layout2 = GetNullLayout();

        // Assert
        (layout1 == layout2).ShouldBeTrue();
    }

    // Helper to avoid dead code analysis warnings
    private static LayoutResults? GetNullLayout() => null;

    public void EqualityInexactFloatsShouldBeEqual()
    {
        // Arrange - Values within tolerance (0.0001f)
        LayoutResults layout1 = new();
        layout1.SetDimension(Dimension.Width, 100.00001f);

        LayoutResults layout2 = new();
        layout2.SetDimension(Dimension.Width, 100.00002f);

        // Assert - Should be equal due to inexact comparison
        layout1.Equals(layout2).ShouldBeTrue();
    }

    public void EqualityMeasuredDimensionsUndefinedShouldBeEqual()
    {
        // Arrange - Both have undefined measured dimensions (default)
        LayoutResults layout1 = new();
        LayoutResults layout2 = new();

        // Assert
        layout1.Equals(layout2).ShouldBeTrue();
    }

    public void EqualityMeasuredDimensionsDifferentShouldNotBeEqual()
    {
        // Arrange
        LayoutResults layout1 = new();
        layout1.SetMeasuredDimension(Dimension.Width, 100f);

        LayoutResults layout2 = new();
        layout2.SetMeasuredDimension(Dimension.Width, 200f);

        // Assert
        layout1.Equals(layout2).ShouldBeFalse();
    }

    public void EqualityOneUndefinedMeasuredDimensionShouldNotBeEqual()
    {
        // Arrange
        LayoutResults layout1 = new();
        layout1.SetMeasuredDimension(Dimension.Width, 100f);

        LayoutResults layout2 = new();
        // layout2 measured dimension is undefined (NaN by default)

        // Assert
        layout1.Equals(layout2).ShouldBeFalse();
    }

    #endregion

    #region HashCode

    public void HashCodeSameValuesShouldBeSame()
    {
        // Arrange
        LayoutResults layout1 = new();
        layout1.SetDimension(Dimension.Width, 100f);
        layout1.SetDirection(Direction.LTR);

        LayoutResults layout2 = new();
        layout2.SetDimension(Dimension.Width, 100f);
        layout2.SetDirection(Direction.LTR);

        // Assert
        layout1.GetHashCode().ShouldBe(layout2.GetHashCode());
    }

    public void HashCodeDifferentValuesShouldGenerallyBeDifferent()
    {
        // Arrange
        LayoutResults layout1 = new();
        layout1.SetDimension(Dimension.Width, 100f);

        LayoutResults layout2 = new();
        layout2.SetDimension(Dimension.Width, 200f);

        // Assert - Different values should generally produce different hash codes
        // Note: This is not guaranteed but is highly likely for different numeric values
        layout1.GetHashCode().ShouldNotBe(layout2.GetHashCode());
    }

    #endregion

    #region MaxCachedMeasurements Constant

    public void MaxCachedMeasurementsShouldBe8()
    {
        // Assert
        LayoutResults.MaxCachedMeasurements.ShouldBe(8);
    }

    #endregion
}
