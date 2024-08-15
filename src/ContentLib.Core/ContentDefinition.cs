using System;
using UnityEngine;

namespace ContentLib.Core;

/// <summary>
/// Base class for custom content that can be registered.
/// </summary>
public abstract class ContentDefinition : ScriptableObject
{
    /// <inheritdoc cref="WarningSeverityLevel"/>
    [field: SerializeField] public WarningSeverityLevel WarningSeverity { get; set; } = WarningSeverityLevel.WarningsAsExceptions;

    /// <summary>
    /// The owner of this content.
    /// </summary>
    /// <remarks>
    /// This member is not null when <see cref="IsRegistered"/> of this instance is <see langword="true"/>.
    /// </remarks>
    public ModDefinition? Mod { get; private set; }

    /// <summary>
    /// Get whether or not this content has been registered.
    /// </summary>
    public bool IsRegistered { get; protected set; }

    /// <summary>
    /// Validates and registers this content.
    /// </summary>
    /// <remarks>
    /// If this method completes without throwing, <see cref="IsRegistered"/> of this instance
    /// is set to <see langword="true"/>.
    /// </remarks>
    public virtual void Register(ModDefinition mod)
    {
        if (mod == null)
            throw new ArgumentNullException(nameof(mod));

        Mod = mod._realModDefinition; 
    }

    /// <summary>
    /// Warn according to <see cref="WarningSeverity"/>.
    /// </summary>
    /// <param name="message">The error or warning message.</param>
    /// <param name="throwDelegate">A delegate that throws an exception with a message.</param>
    /// <exception cref="InvalidOperationException"></exception>
    protected void WarnBySeverity(string message, Action<string> throwDelegate)
    {
        switch (WarningSeverity)
        {
            case WarningSeverityLevel.WarningsAsExceptions:
                throwDelegate($"WarningsAsExceptions: {message}");
                break;

            case WarningSeverityLevel.WarningsAsWarnings:
                Plugin.Log.LogWarning(message);
                break;

            case WarningSeverityLevel.IgnoreWarnings:
                return;
            
            default:
                throw new InvalidOperationException($"{nameof(WarningSeverity)} isn't set to a valid value! Value: {WarningSeverity}");
        }
    }
}
