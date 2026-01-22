/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/config/Config.h, yoga/config/Config.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Delegate for custom clone node callbacks.
/// </summary>
/// <param name="oldNode">The node to clone.</param>
/// <param name="owner">The owner of the node.</param>
/// <param name="childIndex">The index of the child in the owner.</param>
/// <returns>A cloned node, or null to use default cloning.</returns>
public delegate object? CloneNodeFunc(object oldNode, object? owner, int childIndex);

/// <summary>
/// Represents a set of experimental features as a bitset.
/// C# equivalent of std::bitset&lt;ordinalCount&lt;ExperimentalFeature&gt;()&gt;.
/// </summary>
public readonly struct ExperimentalFeatureSet : IEquatable<ExperimentalFeatureSet>
{
  private readonly int _bits;

  /// <summary>
  /// Initializes a new instance of the <see cref="ExperimentalFeatureSet"/> struct.
  /// </summary>
  public ExperimentalFeatureSet()
  {
    _bits = 0;
  }

  private ExperimentalFeatureSet(int bits)
  {
    _bits = bits;
  }

  /// <summary>
  /// Tests whether a feature is enabled.
  /// </summary>
  /// <param name="feature">The feature to test.</param>
  /// <returns>True if the feature is enabled.</returns>
  public bool Test(ExperimentalFeature feature)
  {
    return (_bits & (1 << (int)feature)) != 0;
  }

  /// <summary>
  /// Creates a new set with the specified feature enabled or disabled.
  /// </summary>
  /// <param name="feature">The feature to modify.</param>
  /// <param name="value">True to enable, false to disable.</param>
  /// <returns>A new set with the modification applied.</returns>
  public ExperimentalFeatureSet Set(ExperimentalFeature feature, bool value)
  {
    if (value)
    {
      return new ExperimentalFeatureSet(_bits | (1 << (int)feature));
    }
    else
    {
      return new ExperimentalFeatureSet(_bits & ~(1 << (int)feature));
    }
  }

  /// <inheritdoc />
  public bool Equals(ExperimentalFeatureSet other) => _bits == other._bits;

  /// <inheritdoc />
  public override bool Equals(object? obj) => obj is ExperimentalFeatureSet other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode() => _bits.GetHashCode();

  /// <summary>
  /// Equality operator.
  /// </summary>
  public static bool operator ==(ExperimentalFeatureSet left, ExperimentalFeatureSet right) => left.Equals(right);

  /// <summary>
  /// Inequality operator.
  /// </summary>
  public static bool operator !=(ExperimentalFeatureSet left, ExperimentalFeatureSet right) => !left.Equals(right);
}

/// <summary>
/// Configuration options for Yoga layout calculations.
/// </summary>
/// <remarks>
/// The configuration may be applied to multiple nodes (i.e. a single global config),
/// or can be applied more granularly per-node.
///
/// Design Decision: In C++, Config inherits from YGConfig (an empty tag struct for the public C API).
/// In C#, we don't need this inheritance pattern since we use the same type for both internal and public API.
/// </remarks>
public sealed class Config
{
  private CloneNodeFunc? _cloneNodeCallback;
  private YogaLogHandler? _logger;
  private bool _useWebDefaults;
  private uint _version;
  private ExperimentalFeatureSet _experimentalFeatures;
  private Errata _errata = Errata.None;
  private float _pointScaleFactor = 1.0f;
  private object? _context;

  /// <summary>
  /// Gets the default configuration instance.
  /// </summary>
  public static Config Default { get; } = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="Config"/> class.
  /// </summary>
  public Config()
  {
    _logger = null;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Config"/> class with a custom logger.
  /// </summary>
  /// <param name="logger">The custom log handler.</param>
  public Config(YogaLogHandler? logger)
  {
    _logger = logger;
  }

  #region UseWebDefaults

  /// <summary>
  /// Gets or sets whether to use web defaults.
  /// </summary>
  /// <remarks>
  /// Yoga by default creates new nodes with style defaults different from flexbox
  /// on web (e.g. FlexDirection.Column and PositionType.Relative).
  /// UseWebDefaults instructs Yoga to instead use a default style consistent with the web.
  /// </remarks>
  public bool UseWebDefaults
  {
    get => _useWebDefaults;
    set => _useWebDefaults = value;
  }

  #endregion

  #region Experimental Features

  /// <summary>
  /// Sets whether an experimental feature is enabled.
  /// </summary>
  /// <param name="feature">The feature to enable or disable.</param>
  /// <param name="enabled">True to enable, false to disable.</param>
  public void SetExperimentalFeatureEnabled(ExperimentalFeature feature, bool enabled)
  {
    if (IsExperimentalFeatureEnabled(feature) != enabled)
    {
      _experimentalFeatures = _experimentalFeatures.Set(feature, enabled);
      _version++;
    }
  }

  /// <summary>
  /// Gets whether an experimental feature is enabled.
  /// </summary>
  /// <param name="feature">The feature to check.</param>
  /// <returns>True if the feature is enabled.</returns>
  public bool IsExperimentalFeatureEnabled(ExperimentalFeature feature)
  {
    return _experimentalFeatures.Test(feature);
  }

  /// <summary>
  /// Gets the set of enabled experimental features.
  /// </summary>
  public ExperimentalFeatureSet EnabledExperiments => _experimentalFeatures;

  #endregion

  #region Errata

  /// <summary>
  /// Sets the errata flags, completely replacing any existing flags.
  /// </summary>
  /// <remarks>
  /// Configures how Yoga balances W3C conformance vs compatibility with layouts
  /// created against earlier versions of Yoga.
  ///
  /// By default Yoga will prioritize W3C conformance. Errata may be set to ask
  /// Yoga to produce specific incorrect behaviors. E.g. SetErrata(Errata.StretchFlexBasis).
  ///
  /// Errata is a bitmask, and multiple errata may be set at once.
  /// </remarks>
  /// <param name="errata">The errata flags to set.</param>
  public void SetErrata(Errata errata)
  {
    if (_errata != errata)
    {
      _errata = errata;
      _version++;
    }
  }

  /// <summary>
  /// Adds the specified errata flags to the existing flags.
  /// </summary>
  /// <param name="errata">The errata flags to add.</param>
  public void AddErrata(Errata errata)
  {
    if (!HasErrata(errata))
    {
      _errata |= errata;
      _version++;
    }
  }

  /// <summary>
  /// Removes the specified errata flags from the existing flags.
  /// </summary>
  /// <param name="errata">The errata flags to remove.</param>
  public void RemoveErrata(Errata errata)
  {
    if (HasErrata(errata))
    {
      _errata &= ~errata;
      _version++;
    }
  }

  /// <summary>
  /// Gets the current errata flags.
  /// </summary>
  public Errata Errata => _errata;

  /// <summary>
  /// Gets whether the specified errata flags are set.
  /// </summary>
  /// <param name="errata">The errata flags to check.</param>
  /// <returns>True if the flags are set.</returns>
  public bool HasErrata(Errata errata)
  {
    return (_errata & errata) != Errata.None;
  }

  #endregion

  #region Point Scale Factor

  /// <summary>
  /// Gets or sets the point scale factor.
  /// </summary>
  /// <remarks>
  /// Yoga will by default round final layout positions and dimensions to the
  /// nearest point. PointScaleFactor controls the density of the grid used for
  /// layout rounding (e.g. to round to the closest display pixel).
  ///
  /// May be set to 0.0f to avoid rounding the layout results.
  /// </remarks>
  /// <exception cref="ArgumentOutOfRangeException">Thrown if value is less than zero.</exception>
  public float PointScaleFactor
  {
    get => _pointScaleFactor;
    set
    {
      YogaAssert.Assert(this, value >= 0.0f, "Scale factor should not be less than zero");

      if (!_pointScaleFactor.Equals(value))
      {
        _pointScaleFactor = value;
        _version++;
      }
    }
  }

  #endregion

  #region Context

  /// <summary>
  /// Gets or sets an arbitrary context object on the config which may be read from during callbacks.
  /// </summary>
  public object? Context
  {
    get => _context;
    set => _context = value;
  }

  #endregion

  #region Version

  /// <summary>
  /// Gets the version of this configuration.
  /// </summary>
  /// <remarks>
  /// The version is incremented whenever a configuration property that affects
  /// layout is changed. This is used to determine whether moving a node from
  /// one config to another should dirty previously calculated layout results.
  /// </remarks>
  public uint Version => _version;

  #endregion

  #region Logger

  /// <summary>
  /// Sets a custom log function to use when logging diagnostics or fatal errors.
  /// </summary>
  /// <param name="logger">The custom log handler, or null to use the default.</param>
  public void SetLogger(YogaLogHandler? logger)
  {
    _logger = logger;
  }

  /// <summary>
  /// Logs a message using this configuration's logger.
  /// </summary>
  /// <param name="node">The node context, if any.</param>
  /// <param name="level">The log level.</param>
  /// <param name="message">The message to log.</param>
  public void Log(object? node, LogLevel level, string message)
  {
    if (_logger is not null)
    {
      _logger(node ?? this, level, message);
    }
    else
    {
      YogaLog.DefaultLog(node ?? this, level, message);
    }
  }

  #endregion

  #region Clone Node Callback

  /// <summary>
  /// Sets a callback, called during layout, to create a new mutable Yoga node if
  /// Yoga must write to it and its owner is not its parent observed during layout.
  /// </summary>
  /// <param name="cloneNode">The clone node callback.</param>
  public void SetCloneNodeCallback(CloneNodeFunc? cloneNode)
  {
    _cloneNodeCallback = cloneNode;
  }

  /// <summary>
  /// Clones a node using the registered callback or default cloning.
  /// </summary>
  /// <param name="node">The node to clone.</param>
  /// <param name="owner">The owner of the node.</param>
  /// <param name="childIndex">The index of the child in the owner.</param>
  /// <returns>The cloned node.</returns>
  public object? CloneNode(object node, object? owner, int childIndex)
  {
    object? clone = null;
    if (_cloneNodeCallback is not null)
    {
      clone = _cloneNodeCallback(node, owner, childIndex);
    }
    // Note: Default cloning (YGNodeClone equivalent) will be implemented when Node is ported.
    // For now, we return the callback result or null.
    return clone;
  }

  #endregion

  #region Static Methods

  /// <summary>
  /// Determines whether moving a node from an old to new config should dirty
  /// previously calculated layout results.
  /// </summary>
  /// <param name="oldConfig">The old configuration.</param>
  /// <param name="newConfig">The new configuration.</param>
  /// <returns>True if the layout should be invalidated.</returns>
  /// <exception cref="ArgumentNullException">Thrown if either config is null.</exception>
  public static bool ConfigUpdateInvalidatesLayout(Config oldConfig, Config newConfig)
  {
    ArgumentNullException.ThrowIfNull(oldConfig);
    ArgumentNullException.ThrowIfNull(newConfig);

    return oldConfig.Errata != newConfig.Errata ||
           oldConfig.EnabledExperiments != newConfig.EnabledExperiments ||
           !oldConfig.PointScaleFactor.Equals(newConfig.PointScaleFactor) ||
           oldConfig.UseWebDefaults != newConfig.UseWebDefaults;
  }

  #endregion
}
