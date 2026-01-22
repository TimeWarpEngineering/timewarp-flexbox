/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for Config class
 * Original C++ source: tests/YGConfigTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Config;

/// <summary>
/// Tests for Config class functionality.
/// </summary>
public class ConfigTests
{
  #region Default Values Tests

  public void NewConfigShouldHaveDefaultErrataNone()
  {
    TimeWarp.Flexbox.Config config = new();
    config.Errata.ShouldBe(Errata.None);
  }

  public void NewConfigShouldHaveDefaultPointScaleFactorOne()
  {
    TimeWarp.Flexbox.Config config = new();
    config.PointScaleFactor.ShouldBe(1.0f);
  }

  public void NewConfigShouldHaveDefaultUseWebDefaultsFalse()
  {
    TimeWarp.Flexbox.Config config = new();
    config.UseWebDefaults.ShouldBeFalse();
  }

  public void NewConfigShouldHaveVersionZero()
  {
    TimeWarp.Flexbox.Config config = new();
    config.Version.ShouldBe(0u);
  }

  public void NewConfigShouldHaveNullContext()
  {
    TimeWarp.Flexbox.Config config = new();
    config.Context.ShouldBeNull();
  }

  #endregion

  #region UseWebDefaults Tests

  public void SetUseWebDefaultsShouldUpdateValue()
  {
    TimeWarp.Flexbox.Config config = new();
    config.UseWebDefaults = true;
    config.UseWebDefaults.ShouldBeTrue();
  }

  public void SetUseWebDefaultsFalseShouldUpdateValue()
  {
    TimeWarp.Flexbox.Config config = new();
    config.UseWebDefaults = true;
    config.UseWebDefaults = false;
    config.UseWebDefaults.ShouldBeFalse();
  }

  #endregion

  #region Errata Tests

  public void SetErrataShouldUpdateValue()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis);
    config.Errata.ShouldBe(Errata.StretchFlexBasis);
  }

  public void SetErrataShouldIncrementVersion()
  {
    TimeWarp.Flexbox.Config config = new();
    uint initialVersion = config.Version;
    config.SetErrata(Errata.StretchFlexBasis);
    config.Version.ShouldBe(initialVersion + 1);
  }

  public void SetErrataSameValueShouldNotIncrementVersion()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis);
    uint versionAfterFirst = config.Version;
    config.SetErrata(Errata.StretchFlexBasis);
    config.Version.ShouldBe(versionAfterFirst);
  }

  public void AddErrataShouldAddFlags()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis);
    config.AddErrata(Errata.AbsolutePercentAgainstInnerSize);
    config.HasErrata(Errata.StretchFlexBasis).ShouldBeTrue();
    config.HasErrata(Errata.AbsolutePercentAgainstInnerSize).ShouldBeTrue();
  }

  public void AddErrataShouldIncrementVersion()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis);
    uint versionAfterSet = config.Version;
    config.AddErrata(Errata.AbsolutePercentAgainstInnerSize);
    config.Version.ShouldBe(versionAfterSet + 1);
  }

  public void AddErrataSameValueShouldNotIncrementVersion()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis);
    uint versionAfterSet = config.Version;
    config.AddErrata(Errata.StretchFlexBasis);
    config.Version.ShouldBe(versionAfterSet);
  }

  public void RemoveErrataShouldRemoveFlags()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis | Errata.AbsolutePercentAgainstInnerSize);
    config.RemoveErrata(Errata.StretchFlexBasis);
    config.HasErrata(Errata.StretchFlexBasis).ShouldBeFalse();
    config.HasErrata(Errata.AbsolutePercentAgainstInnerSize).ShouldBeTrue();
  }

  public void RemoveErrataShouldIncrementVersion()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis | Errata.AbsolutePercentAgainstInnerSize);
    uint versionAfterSet = config.Version;
    config.RemoveErrata(Errata.StretchFlexBasis);
    config.Version.ShouldBe(versionAfterSet + 1);
  }

  public void RemoveErrataNotPresentShouldNotIncrementVersion()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis);
    uint versionAfterSet = config.Version;
    config.RemoveErrata(Errata.AbsolutePercentAgainstInnerSize);
    config.Version.ShouldBe(versionAfterSet);
  }

  public void HasErrataShouldReturnTrueWhenFlagIsSet()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis);
    config.HasErrata(Errata.StretchFlexBasis).ShouldBeTrue();
  }

  public void HasErrataShouldReturnFalseWhenFlagIsNotSet()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis);
    config.HasErrata(Errata.AbsolutePercentAgainstInnerSize).ShouldBeFalse();
  }

  public void HasErrataShouldWorkWithMultipleFlags()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetErrata(Errata.StretchFlexBasis | Errata.AbsolutePercentAgainstInnerSize);
    // HasErrata checks if ANY of the specified flags are set
    config.HasErrata(Errata.StretchFlexBasis | Errata.AbsolutePercentAgainstInnerSize).ShouldBeTrue();
  }

  #endregion

  #region PointScaleFactor Tests

  public void SetPointScaleFactorShouldUpdateValue()
  {
    TimeWarp.Flexbox.Config config = new();
    config.PointScaleFactor = 2.0f;
    config.PointScaleFactor.ShouldBe(2.0f);
  }

  public void SetPointScaleFactorShouldIncrementVersion()
  {
    TimeWarp.Flexbox.Config config = new();
    uint initialVersion = config.Version;
    config.PointScaleFactor = 2.0f;
    config.Version.ShouldBe(initialVersion + 1);
  }

  public void SetPointScaleFactorSameValueShouldNotIncrementVersion()
  {
    TimeWarp.Flexbox.Config config = new();
    config.PointScaleFactor = 2.0f;
    uint versionAfterFirst = config.Version;
    config.PointScaleFactor = 2.0f;
    config.Version.ShouldBe(versionAfterFirst);
  }

  public void SetPointScaleFactorZeroShouldBeAllowed()
  {
    TimeWarp.Flexbox.Config config = new();
    config.PointScaleFactor = 0.0f;
    config.PointScaleFactor.ShouldBe(0.0f);
  }

  public void SetPointScaleFactorNegativeShouldThrow()
  {
    TimeWarp.Flexbox.Config config = new();
    Should.Throw<YogaAssertException>(() => config.PointScaleFactor = -1.0f);
  }

  #endregion

  #region Experimental Features Tests

  public void ExperimentalFeaturesShouldBeDisabledByDefault()
  {
    TimeWarp.Flexbox.Config config = new();
    config.IsExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis).ShouldBeFalse();
  }

  public void SetExperimentalFeatureEnabledShouldEnableFeature()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis, true);
    config.IsExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis).ShouldBeTrue();
  }

  public void SetExperimentalFeatureDisabledShouldDisableFeature()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis, true);
    config.SetExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis, false);
    config.IsExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis).ShouldBeFalse();
  }

  public void SetExperimentalFeatureShouldIncrementVersion()
  {
    TimeWarp.Flexbox.Config config = new();
    uint initialVersion = config.Version;
    config.SetExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis, true);
    config.Version.ShouldBe(initialVersion + 1);
  }

  public void SetExperimentalFeatureSameValueShouldNotIncrementVersion()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis, true);
    uint versionAfterFirst = config.Version;
    config.SetExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis, true);
    config.Version.ShouldBe(versionAfterFirst);
  }

  public void EnabledExperimentsShouldReturnCorrectSet()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis, true);
    ExperimentalFeatureSet experiments = config.EnabledExperiments;
    experiments.Test(ExperimentalFeature.WebFlexBasis).ShouldBeTrue();
  }

  #endregion

  #region Context Tests

  public void SetContextShouldStoreValue()
  {
    TimeWarp.Flexbox.Config config = new();
    object context = new();
    config.Context = context;
    config.Context.ShouldBeSameAs(context);
  }

  public void SetContextNullShouldClearValue()
  {
    TimeWarp.Flexbox.Config config = new();
    config.Context = new object();
    config.Context = null;
    config.Context.ShouldBeNull();
  }

  #endregion

  #region Clone Node Callback Tests

  public void CloneNodeShouldUseCallbackWhenProvided()
  {
    TimeWarp.Flexbox.Config config = new();
    object clonedNode = new();
    config.SetCloneNodeCallback((_, _, _) => clonedNode);

    object originalNode = new();
    object? result = config.CloneNode(originalNode, null, 0);

    result.ShouldBeSameAs(clonedNode);
  }

  public void CloneNodeShouldReturnNullWhenCallbackReturnsNull()
  {
    TimeWarp.Flexbox.Config config = new();
    config.SetCloneNodeCallback((_, _, _) => null);

    object originalNode = new();
    object? result = config.CloneNode(originalNode, null, 0);

    // When callback returns null, and no default clone exists, result is null
    result.ShouldBeNull();
  }

  public void CloneNodeShouldPassCorrectParameters()
  {
    TimeWarp.Flexbox.Config config = new();
    object? receivedNode = null;
    object? receivedOwner = null;
    int receivedIndex = -1;

    config.SetCloneNodeCallback((node, owner, index) =>
    {
      receivedNode = node;
      receivedOwner = owner;
      receivedIndex = index;
      return new object();
    });

    object originalNode = new();
    object ownerNode = new();
    config.CloneNode(originalNode, ownerNode, 5);

    receivedNode.ShouldBeSameAs(originalNode);
    receivedOwner.ShouldBeSameAs(ownerNode);
    receivedIndex.ShouldBe(5);
  }

  public void CloneNodeWithoutCallbackShouldReturnNull()
  {
    TimeWarp.Flexbox.Config config = new();
    object originalNode = new();
    object? result = config.CloneNode(originalNode, null, 0);

    // Without a callback and without Node implementation, returns null
    result.ShouldBeNull();
  }

  #endregion

  #region ConfigUpdateInvalidatesLayout Tests

  public void ConfigUpdateInvalidatesLayoutShouldReturnFalseForIdenticalConfigs()
  {
    TimeWarp.Flexbox.Config config1 = new();
    TimeWarp.Flexbox.Config config2 = new();

    TimeWarp.Flexbox.Config.ConfigUpdateInvalidatesLayout(config1, config2).ShouldBeFalse();
  }

  public void ConfigUpdateInvalidatesLayoutShouldReturnTrueWhenErrataChanges()
  {
    TimeWarp.Flexbox.Config config1 = new();
    TimeWarp.Flexbox.Config config2 = new();
    config2.SetErrata(Errata.StretchFlexBasis);

    TimeWarp.Flexbox.Config.ConfigUpdateInvalidatesLayout(config1, config2).ShouldBeTrue();
  }

  public void ConfigUpdateInvalidatesLayoutShouldReturnTrueWhenExperimentalFeaturesChange()
  {
    TimeWarp.Flexbox.Config config1 = new();
    TimeWarp.Flexbox.Config config2 = new();
    config2.SetExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis, true);

    TimeWarp.Flexbox.Config.ConfigUpdateInvalidatesLayout(config1, config2).ShouldBeTrue();
  }

  public void ConfigUpdateInvalidatesLayoutShouldReturnTrueWhenPointScaleFactorChanges()
  {
    TimeWarp.Flexbox.Config config1 = new();
    TimeWarp.Flexbox.Config config2 = new();
    config2.PointScaleFactor = 2.0f;

    TimeWarp.Flexbox.Config.ConfigUpdateInvalidatesLayout(config1, config2).ShouldBeTrue();
  }

  public void ConfigUpdateInvalidatesLayoutShouldReturnTrueWhenUseWebDefaultsChanges()
  {
    TimeWarp.Flexbox.Config config1 = new();
    TimeWarp.Flexbox.Config config2 = new();
    config2.UseWebDefaults = true;

    TimeWarp.Flexbox.Config.ConfigUpdateInvalidatesLayout(config1, config2).ShouldBeTrue();
  }

  public void ConfigUpdateInvalidatesLayoutShouldIgnoreContextChanges()
  {
    TimeWarp.Flexbox.Config config1 = new();
    TimeWarp.Flexbox.Config config2 = new();
    config2.Context = new object();

    TimeWarp.Flexbox.Config.ConfigUpdateInvalidatesLayout(config1, config2).ShouldBeFalse();
  }

  #endregion

  #region Default Config Tests

  public void DefaultConfigShouldExist()
  {
    TimeWarp.Flexbox.Config.Default.ShouldNotBeNull();
  }

  public void DefaultConfigShouldBeSingleton()
  {
    TimeWarp.Flexbox.Config config1 = TimeWarp.Flexbox.Config.Default;
    TimeWarp.Flexbox.Config config2 = TimeWarp.Flexbox.Config.Default;
    config1.ShouldBeSameAs(config2);
  }

  #endregion

  #region Logger Tests

  public void LogShouldUseCustomLoggerWhenSet()
  {
    TimeWarp.Flexbox.Config config = new();
    bool loggerCalled = false;
    LogLevel receivedLevel = LogLevel.Debug;
    string receivedMessage = "";

    config.SetLogger((_, level, message) =>
    {
      loggerCalled = true;
      receivedLevel = level;
      receivedMessage = message;
    });

    config.Log(null, LogLevel.Info, "test message");

    loggerCalled.ShouldBeTrue();
    receivedLevel.ShouldBe(LogLevel.Info);
    receivedMessage.ShouldBe("test message");
  }

  public void LogShouldPassNodeAsContext()
  {
    TimeWarp.Flexbox.Config config = new();
    object? receivedContext = null;

    config.SetLogger((context, _, _) =>
    {
      receivedContext = context;
    });

    object node = new();
    config.Log(node, LogLevel.Info, "test");

    receivedContext.ShouldBeSameAs(node);
  }

  public void LogShouldPassConfigAsContextWhenNodeIsNull()
  {
    TimeWarp.Flexbox.Config config = new();
    object? receivedContext = null;

    config.SetLogger((context, _, _) =>
    {
      receivedContext = context;
    });

    config.Log(null, LogLevel.Info, "test");

    receivedContext.ShouldBeSameAs(config);
  }

  #endregion

  #region ExperimentalFeatureSet Tests

  public void ExperimentalFeatureSetShouldBeEmptyByDefault()
  {
    ExperimentalFeatureSet set = new();
    set.Test(ExperimentalFeature.WebFlexBasis).ShouldBeFalse();
  }

  public void ExperimentalFeatureSetSetShouldEnableFeature()
  {
    ExperimentalFeatureSet set = new();
    ExperimentalFeatureSet newSet = set.Set(ExperimentalFeature.WebFlexBasis, true);
    newSet.Test(ExperimentalFeature.WebFlexBasis).ShouldBeTrue();
  }

  public void ExperimentalFeatureSetSetShouldDisableFeature()
  {
    ExperimentalFeatureSet set = new();
    ExperimentalFeatureSet enabledSet = set.Set(ExperimentalFeature.WebFlexBasis, true);
    ExperimentalFeatureSet disabledSet = enabledSet.Set(ExperimentalFeature.WebFlexBasis, false);
    disabledSet.Test(ExperimentalFeature.WebFlexBasis).ShouldBeFalse();
  }

  public void ExperimentalFeatureSetShouldBeImmutable()
  {
    ExperimentalFeatureSet set = new();
    ExperimentalFeatureSet newSet = set.Set(ExperimentalFeature.WebFlexBasis, true);

    // Original set should be unchanged
    set.Test(ExperimentalFeature.WebFlexBasis).ShouldBeFalse();
    newSet.Test(ExperimentalFeature.WebFlexBasis).ShouldBeTrue();
  }

  public void ExperimentalFeatureSetEqualityShouldWorkCorrectly()
  {
    ExperimentalFeatureSet set1 = new();
    ExperimentalFeatureSet set2 = new();

    (set1 == set2).ShouldBeTrue();
    (set1 != set2).ShouldBeFalse();

    ExperimentalFeatureSet set3 = set1.Set(ExperimentalFeature.WebFlexBasis, true);
    (set1 == set3).ShouldBeFalse();
    (set1 != set3).ShouldBeTrue();
  }

  public void ExperimentalFeatureSetGetHashCodeShouldBeConsistent()
  {
    ExperimentalFeatureSet set1 = new();
    ExperimentalFeatureSet set2 = new();

    set1.GetHashCode().ShouldBe(set2.GetHashCode());

    ExperimentalFeatureSet set3 = set1.Set(ExperimentalFeature.WebFlexBasis, true);
    ExperimentalFeatureSet set4 = set2.Set(ExperimentalFeature.WebFlexBasis, true);
    set3.GetHashCode().ShouldBe(set4.GetHashCode());
  }

  #endregion
}
