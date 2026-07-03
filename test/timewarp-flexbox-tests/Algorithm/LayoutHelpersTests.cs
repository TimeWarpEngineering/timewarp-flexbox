/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for LayoutHelpers utilities
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for LayoutHelpers utilities.
/// </summary>
public class LayoutHelpersTests
{
  #region IsFixedSize Tests

  public void IsFixedSizeShouldReturnTrueForStretchFitMode()
  {
    // Act & Assert - StretchFit is always fixed regardless of dimension
    LayoutHelpers.IsFixedSize(100f, SizingMode.StretchFit).ShouldBeTrue();
    LayoutHelpers.IsFixedSize(0f, SizingMode.StretchFit).ShouldBeTrue();
    LayoutHelpers.IsFixedSize(float.NaN, SizingMode.StretchFit).ShouldBeTrue();
  }

  public void IsFixedSizeShouldReturnFalseForMaxContentMode()
  {
    // Act & Assert - MaxContent requires measurement
    LayoutHelpers.IsFixedSize(100f, SizingMode.MaxContent).ShouldBeFalse();
    LayoutHelpers.IsFixedSize(0f, SizingMode.MaxContent).ShouldBeFalse();
    LayoutHelpers.IsFixedSize(float.NaN, SizingMode.MaxContent).ShouldBeFalse();
  }

  public void IsFixedSizeShouldReturnTrueForFitContentWithZeroOrNegativeDimension()
  {
    // Act & Assert - FitContent is fixed when dimension is 0 or negative
    LayoutHelpers.IsFixedSize(0f, SizingMode.FitContent).ShouldBeTrue();
    LayoutHelpers.IsFixedSize(-10f, SizingMode.FitContent).ShouldBeTrue();
  }

  public void IsFixedSizeShouldReturnFalseForFitContentWithPositiveDimension()
  {
    // Act & Assert - FitContent with positive dimension requires measurement
    LayoutHelpers.IsFixedSize(100f, SizingMode.FitContent).ShouldBeFalse();
    LayoutHelpers.IsFixedSize(0.001f, SizingMode.FitContent).ShouldBeFalse();
  }

  public void IsFixedSizeShouldReturnFalseForFitContentWithUndefinedDimension()
  {
    // Act & Assert - FitContent with undefined dimension requires measurement
    LayoutHelpers.IsFixedSize(float.NaN, SizingMode.FitContent).ShouldBeFalse();
  }

  #endregion

  #region ConstrainMaxSizeForMode Tests

  public void ConstrainMaxSizeForModeShouldNotModifyWhenNoMaxSet()
  {
    // Arrange
    FlexNode node = new();
    SizingMode mode = SizingMode.StretchFit;
    float size = 100f;

    // Act
    LayoutHelpers.ConstrainMaxSizeForMode(
        node,
        Direction.LTR,
        FlexDirection.Row,
        ownerAxisSize: 200f,
        ownerWidth: 200f,
        ref mode,
        ref size);

    // Assert - No max set, so values unchanged
    mode.ShouldBe(SizingMode.StretchFit);
    size.ShouldBe(100f);
  }

  public void ConstrainMaxSizeForModeShouldClampSizeToMax()
  {
    // Arrange
    FlexNode node = new();
    node.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(50f));
    SizingMode mode = SizingMode.StretchFit;
    float size = 100f;

    // Act
    LayoutHelpers.ConstrainMaxSizeForMode(
        node,
        Direction.LTR,
        FlexDirection.Row,
        ownerAxisSize: 200f,
        ownerWidth: 200f,
        ref mode,
        ref size);

    // Assert - Size clamped to max (50); StretchFit mode is preserved
    mode.ShouldBe(SizingMode.StretchFit);
    size.ShouldBe(50f);
  }

  public void ConstrainMaxSizeForModeShouldNotClampWhenSizeBelowMax()
  {
    // Arrange
    FlexNode node = new();
    node.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(150f));
    SizingMode mode = SizingMode.StretchFit;
    float size = 100f;

    // Act
    LayoutHelpers.ConstrainMaxSizeForMode(
        node,
        Direction.LTR,
        FlexDirection.Row,
        ownerAxisSize: 200f,
        ownerWidth: 200f,
        ref mode,
        ref size);

    // Assert - Size is below max, so size and mode are unchanged
    mode.ShouldBe(SizingMode.StretchFit);
    size.ShouldBe(100f);
  }

  public void ConstrainMaxSizeForModeShouldConvertMaxContentToFitContent()
  {
    // Arrange
    FlexNode node = new();
    node.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(50f));
    SizingMode mode = SizingMode.MaxContent;
    float size = 100f;

    // Act
    LayoutHelpers.ConstrainMaxSizeForMode(
        node,
        Direction.LTR,
        FlexDirection.Row,
        ownerAxisSize: 200f,
        ownerWidth: 200f,
        ref mode,
        ref size);

    // Assert - MaxContent with a defined max becomes FitContent at the max size
    mode.ShouldBe(SizingMode.FitContent);
    size.ShouldBe(50f);
  }

  public void ConstrainMaxSizeForModeShouldWorkForColumnAxis()
  {
    // Arrange
    FlexNode node = new();
    node.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(50f));
    SizingMode mode = SizingMode.StretchFit;
    float size = 100f;

    // Act
    LayoutHelpers.ConstrainMaxSizeForMode(
        node,
        Direction.LTR,
        FlexDirection.Column,
        ownerAxisSize: 200f,
        ownerWidth: 200f,
        ref mode,
        ref size);

    // Assert - Size clamped to max height; StretchFit mode is preserved
    mode.ShouldBe(SizingMode.StretchFit);
    size.ShouldBe(50f);
  }

  public void ConstrainMaxSizeForModeShouldHandleUndefinedSize()
  {
    // Arrange
    FlexNode node = new();
    node.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(50f));
    SizingMode mode = SizingMode.StretchFit;
    float size = float.NaN;

    // Act
    LayoutHelpers.ConstrainMaxSizeForMode(
        node,
        Direction.LTR,
        FlexDirection.Row,
        ownerAxisSize: 200f,
        ownerWidth: 200f,
        ref mode,
        ref size);

    // Assert - Undefined size, so use max; StretchFit mode is preserved
    mode.ShouldBe(SizingMode.StretchFit);
    size.ShouldBe(50f);
  }

  #endregion

  #region CalculateAvailableInnerDimension Tests

  public void CalculateAvailableInnerDimensionShouldReturnUndefinedForUndefinedInput()
  {
    // Arrange
    FlexNode node = new();

    // Act
    float result = LayoutHelpers.CalculateAvailableInnerDimension(
        node,
        Direction.LTR,
        Dimension.Width,
        availableDim: float.NaN,
        paddingAndBorder: 10f,
        ownerDim: 100f,
        ownerWidth: 100f);

    // Assert
    Comparison.IsUndefined(result).ShouldBeTrue();
  }

  public void CalculateAvailableInnerDimensionShouldSubtractPaddingAndBorder()
  {
    // Arrange
    FlexNode node = new();

    // Act
    float result = LayoutHelpers.CalculateAvailableInnerDimension(
        node,
        Direction.LTR,
        Dimension.Width,
        availableDim: 100f,
        paddingAndBorder: 20f,
        ownerDim: 100f,
        ownerWidth: 100f);

    // Assert - 100 - 20 = 80
    result.ShouldBe(80f);
  }

  public void CalculateAvailableInnerDimensionShouldRespectMinConstraint()
  {
    // Arrange
    FlexNode node = new();
    node.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(90f));

    // Act
    float result = LayoutHelpers.CalculateAvailableInnerDimension(
        node,
        Direction.LTR,
        Dimension.Width,
        availableDim: 100f,
        paddingAndBorder: 20f,
        ownerDim: 100f,
        ownerWidth: 100f);

    // Assert - min (90 - 20 = 70) > available inner (80), so use 80
    // Wait, 80 > 70 so available inner (80) is used
    result.ShouldBe(80f);
  }

  public void CalculateAvailableInnerDimensionShouldRespectMinConstraintWhenLarger()
  {
    // Arrange
    FlexNode node = new();
    node.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(110f));

    // Act
    float result = LayoutHelpers.CalculateAvailableInnerDimension(
        node,
        Direction.LTR,
        Dimension.Width,
        availableDim: 100f,
        paddingAndBorder: 20f,
        ownerDim: 100f,
        ownerWidth: 100f);

    // Assert - min inner (110 - 20 = 90) > available inner (80), so use 90
    result.ShouldBe(90f);
  }

  public void CalculateAvailableInnerDimensionShouldRespectMaxConstraint()
  {
    // Arrange
    FlexNode node = new();
    node.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(50f));

    // Act
    float result = LayoutHelpers.CalculateAvailableInnerDimension(
        node,
        Direction.LTR,
        Dimension.Width,
        availableDim: 100f,
        paddingAndBorder: 20f,
        ownerDim: 100f,
        ownerWidth: 100f);

    // Assert - max inner (50 - 20 = 30) < available inner (80), so use 30
    result.ShouldBe(30f);
  }

  public void CalculateAvailableInnerDimensionShouldNotGoNegative()
  {
    // Arrange
    FlexNode node = new();

    // Act
    float result = LayoutHelpers.CalculateAvailableInnerDimension(
        node,
        Direction.LTR,
        Dimension.Width,
        availableDim: 10f,
        paddingAndBorder: 50f,
        ownerDim: 100f,
        ownerWidth: 100f);

    // Assert - 10 - 50 = -40, but should be floored at 0
    result.ShouldBe(0f);
  }

  public void CalculateAvailableInnerDimensionShouldWorkForHeightDimension()
  {
    // Arrange
    FlexNode node = new();
    node.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(60f));

    // Act
    float result = LayoutHelpers.CalculateAvailableInnerDimension(
        node,
        Direction.LTR,
        Dimension.Height,
        availableDim: 100f,
        paddingAndBorder: 10f,
        ownerDim: 100f,
        ownerWidth: 100f);

    // Assert - max inner (60 - 10 = 50) < available inner (90), so use 50
    result.ShouldBe(50f);
  }

  #endregion

  #region ZeroOutLayoutRecursively Tests

  public void ZeroOutLayoutRecursivelyShouldZeroDimensions()
  {
    // Arrange
    FlexNode node = new();
    node.SetLayoutDimension(100f, Dimension.Width);
    node.SetLayoutDimension(200f, Dimension.Height);
    node.SetLayoutMeasuredDimension(100f, Dimension.Width);
    node.SetLayoutMeasuredDimension(200f, Dimension.Height);
    node.HasNewLayout = false;

    // Act
    LayoutHelpers.ZeroOutLayoutRecursively(node);

    // Assert
    node.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    node.Layout.GetDimension(Dimension.Height).ShouldBe(0f);
    node.Layout.GetMeasuredDimension(Dimension.Width).ShouldBe(0f);
    node.Layout.GetMeasuredDimension(Dimension.Height).ShouldBe(0f);
    node.HasNewLayout.ShouldBeTrue();
  }

  public void ZeroOutLayoutRecursivelyShouldZeroChildNodes()
  {
    // Arrange
    FlexNode parent = new();
    FlexNode child = new();
    child.SetLayoutDimension(50f, Dimension.Width);
    child.SetLayoutDimension(50f, Dimension.Height);
    child.HasNewLayout = false;
    parent.InsertChild(child, 0);
    child.Owner = parent;

    // Act
    LayoutHelpers.ZeroOutLayoutRecursively(parent);

    // Assert - Child is also zeroed
    child.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    child.Layout.GetDimension(Dimension.Height).ShouldBe(0f);
    child.HasNewLayout.ShouldBeTrue();
  }

  public void ZeroOutLayoutRecursivelyShouldZeroDeeplyNestedNodes()
  {
    // Arrange
    FlexNode root = new();
    FlexNode child = new();
    FlexNode grandchild = new();

    grandchild.SetLayoutDimension(25f, Dimension.Width);
    grandchild.SetLayoutDimension(25f, Dimension.Height);
    grandchild.HasNewLayout = false;

    child.InsertChild(grandchild, 0);
    grandchild.Owner = child;
    root.InsertChild(child, 0);
    child.Owner = root;

    // Act
    LayoutHelpers.ZeroOutLayoutRecursively(root);

    // Assert - Grandchild is also zeroed
    grandchild.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    grandchild.Layout.GetDimension(Dimension.Height).ShouldBe(0f);
    grandchild.HasNewLayout.ShouldBeTrue();
  }

  #endregion

  #region CleanupContentsNodesRecursively Tests

  public void CleanupContentsNodesRecursivelyShouldZeroContentsNodes()
  {
    // Arrange
    FlexNode parent = new();
    FlexNode contentsChild = new();
    contentsChild.Style.Display = Display.Contents;
    contentsChild.SetLayoutDimension(100f, Dimension.Width);
    contentsChild.SetLayoutDimension(100f, Dimension.Height);
    contentsChild.HasNewLayout = false;
    parent.InsertChild(contentsChild, 0);
    contentsChild.Owner = parent;

    // Act
    LayoutHelpers.CleanupContentsNodesRecursively(parent);

    // Assert - Contents node is zeroed
    contentsChild.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    contentsChild.Layout.GetDimension(Dimension.Height).ShouldBe(0f);
    contentsChild.HasNewLayout.ShouldBeTrue();
  }

  public void CleanupContentsNodesRecursivelyShouldNotZeroRegularNodes()
  {
    // Arrange
    FlexNode parent = new();
    FlexNode regularChild = new();
    regularChild.Style.Display = Display.Flex;
    regularChild.SetLayoutDimension(100f, Dimension.Width);
    regularChild.SetLayoutDimension(100f, Dimension.Height);
    regularChild.HasNewLayout = false;
    parent.InsertChild(regularChild, 0);
    regularChild.Owner = parent;

    // Act
    LayoutHelpers.CleanupContentsNodesRecursively(parent);

    // Assert - Regular node is NOT zeroed
    regularChild.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    regularChild.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
    regularChild.HasNewLayout.ShouldBeFalse();
  }

  public void CleanupContentsNodesRecursivelyShouldProcessNestedContentsNodes()
  {
    // Arrange
    FlexNode root = new();
    FlexNode contentsParent = new();
    FlexNode contentsChild = new();

    contentsParent.Style.Display = Display.Contents;
    contentsChild.Style.Display = Display.Contents;

    contentsParent.SetLayoutDimension(100f, Dimension.Width);
    contentsChild.SetLayoutDimension(50f, Dimension.Width);

    contentsChild.HasNewLayout = false;
    contentsParent.HasNewLayout = false;

    contentsParent.InsertChild(contentsChild, 0);
    contentsChild.Owner = contentsParent;
    root.InsertChild(contentsParent, 0);
    contentsParent.Owner = root;

    // Act
    LayoutHelpers.CleanupContentsNodesRecursively(root);

    // Assert - Both contents nodes are zeroed
    contentsParent.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    contentsChild.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    contentsParent.HasNewLayout.ShouldBeTrue();
    contentsChild.HasNewLayout.ShouldBeTrue();
  }

  public void CleanupContentsNodesRecursivelyShouldProcessContentsChildrenOfRegularNodes()
  {
    // Arrange
    FlexNode root = new();
    FlexNode regularChild = new();
    FlexNode contentsGrandchild = new();

    regularChild.Style.Display = Display.Flex;
    contentsGrandchild.Style.Display = Display.Contents;

    contentsGrandchild.SetLayoutDimension(50f, Dimension.Width);
    contentsGrandchild.HasNewLayout = false;

    regularChild.InsertChild(contentsGrandchild, 0);
    contentsGrandchild.Owner = regularChild;
    root.InsertChild(regularChild, 0);
    regularChild.Owner = root;

    // Act
    LayoutHelpers.CleanupContentsNodesRecursively(root);

    // Assert - Contents grandchild is zeroed even though parent is regular
    contentsGrandchild.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    contentsGrandchild.HasNewLayout.ShouldBeTrue();
  }

  #endregion

  #region Edge Cases

  public void ConstrainMaxSizeForModeShouldHandleNegativeMax()
  {
    // Arrange - Negative max is applied as-is (matches C++ constrainMaxSizeForMode)
    FlexNode node = new();
    node.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(-10f));
    SizingMode mode = SizingMode.StretchFit;
    float size = 100f;

    // Act
    LayoutHelpers.ConstrainMaxSizeForMode(
        node,
        Direction.LTR,
        FlexDirection.Row,
        ownerAxisSize: 200f,
        ownerWidth: 200f,
        ref mode,
        ref size);

    // Assert - Size is clamped to the negative max, mode unchanged
    mode.ShouldBe(SizingMode.StretchFit);
    size.ShouldBe(-10f);
  }

  public void ConstrainMaxSizeForModeShouldThrowForNullNode()
  {
    // Arrange
    SizingMode mode = SizingMode.StretchFit;
    float size = 100f;

    // Act & Assert
    Should.Throw<ArgumentNullException>(() =>
        LayoutHelpers.ConstrainMaxSizeForMode(
            null!,
            Direction.LTR,
            FlexDirection.Row,
            ownerAxisSize: 200f,
            ownerWidth: 200f,
            ref mode,
            ref size));
  }

  public void CalculateAvailableInnerDimensionShouldThrowForNullNode()
  {
    // Act & Assert
    Should.Throw<ArgumentNullException>(() =>
        LayoutHelpers.CalculateAvailableInnerDimension(
            null!,
            Direction.LTR,
            Dimension.Width,
            availableDim: 100f,
            paddingAndBorder: 10f,
            ownerDim: 100f,
            ownerWidth: 100f));
  }

  public void ZeroOutLayoutRecursivelyShouldThrowForNullNode()
  {
    // Act & Assert
    Should.Throw<ArgumentNullException>(() =>
        LayoutHelpers.ZeroOutLayoutRecursively(null!));
  }

  public void CleanupContentsNodesRecursivelyShouldThrowForNullNode()
  {
    // Act & Assert
    Should.Throw<ArgumentNullException>(() =>
        LayoutHelpers.CleanupContentsNodesRecursively(null!));
  }

  #endregion
}
