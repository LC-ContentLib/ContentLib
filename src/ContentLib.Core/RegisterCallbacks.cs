using System;
using System.Collections.Generic;

namespace ContentLib.Core;

/// <summary>
/// Callback events for ContentDefinition registration.
/// </summary>
public class RegisterCallbacks<T> where T : ContentDefinition
{
    // We can disable this warning because of <inheritdoc/>
    #pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
    /// <summary>
    /// Delegate method to feed into the constructor <see cref="RegisterCallbacks(ref CallbackInvoker)"/>
    /// that when called, will try to invoke a callback event for the specified ContentDefinition.
    /// </summary>
    /// <inheritdoc cref="AddOnBeforeRegister(ModDefinition, string, Action{T})"/>
    /// <param name="contentDefinitionName">The name of the ContentDefinition ScriptableObject that is being registered.</param>
    /// <param name="isBefore">Is this event before the ContentDefinition has been registered or after?</param>
    /// <param name="contentDefinition">The ContentDefinition instance. Will be fed to whoever subscribed to this callback.</param>
    public delegate void CallbackInvoker(ModDefinition modDefinition, string contentDefinitionName, bool isBefore, T contentDefinition);
    #pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

    /// <summary>
    /// Constructor for setting up an eventInvoker for the callback events.
    /// </summary>
    /// <param name="callbackInvoker">A <see cref="CallbackInvoker"/> that is invoked before and after a ContentDefinition is registered.</param>
    public RegisterCallbacks(ref CallbackInvoker? callbackInvoker)
    {
        callbackInvoker = (
            ModDefinition mod,
            string contentDefinitionName,
            bool isBefore,
            T contentDefinition
        ) =>
        {
            if (_callbacks.TryGetValue((mod, contentDefinitionName, isBefore),
                out List<Action<T>> eventHandlers))
            {
                foreach (Action<T> eventHandler in eventHandlers)
                    eventHandler(contentDefinition);
            }
        };
    }

    private readonly Dictionary<(ModDefinition modDefinition, string contentDefinitionName, bool isBefore), List<Action<T>>> _callbacks = [];

    /// <summary>
    /// Subscribe to a callback immediately before a ContentDefinition is validated and registered.
    /// </summary>
    /// <param name="authorName">The name of the author whose mod owns this ContentDefinition.</param>
    /// <param name="modName">The name of the mod that owns this ContentDefinition.</param>
    /// <param name="contentDefinitionName">The name of the ContentDefinition ScriptableObject whose register method we are listening to.</param>
    /// <param name="eventHandler">The delegate that runs when the ContentDefinition's register method is called.</param>
    public void AddOnBeforeRegister(
        string authorName,
        string modName,
        string contentDefinitionName,
        Action<T> eventHandler
    )
    {
        ModDefinition mod = ModDefinition.Create(authorName, modName);
        SubscribeTo(mod, contentDefinitionName, eventHandler, isBefore: true);
    }

    // We can disable this warning because of <inheritdoc/>
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

    /// <param name="modDefinition">The mod that owns this ContentDefinition.</param>
    /// <inheritdoc cref="AddOnBeforeRegister(string, string, string, Action{T})"/>
    public void AddOnBeforeRegister(
        ModDefinition modDefinition,
        string contentDefinitionName,
        Action<T> eventHandler
    )
    {
        ModDefinition realMod = modDefinition.GetRealInstance();
        SubscribeTo(realMod, contentDefinitionName, eventHandler, isBefore: true);
    }
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

    /// <summary>
    /// Subscribe to a callback immediately after a ContentDefinition has been validated and registered.
    /// </summary>
    /// <inheritdoc cref="AddOnBeforeRegister(string, string, string, Action{T})"/>
    public void AddOnAfterRegister(
        string authorName,
        string modName,
        string contentDefinitionName,
        Action<T> eventHandler
    )
    {
        ModDefinition mod = ModDefinition.Create(authorName, modName);
        SubscribeTo(mod, contentDefinitionName, eventHandler, isBefore: false);
    }

    /// <inheritdoc cref="AddOnAfterRegister(string, string, string, Action{T})"/>
    public void AddOnAfterRegister(
        ModDefinition modDefinition,
        string contentDefinitionName,
        Action<T> eventHandler
    )
    {
        ModDefinition realMod = modDefinition.GetRealInstance();
        SubscribeTo(realMod, contentDefinitionName, eventHandler, isBefore: false);
    }

    private void SubscribeTo(
        ModDefinition modDefinition,
        string contentDefinitionName,
        Action<T> eventHandler,
        bool isBefore
    )
    {
        if (_callbacks.TryGetValue((modDefinition, contentDefinitionName, isBefore),
            out List<Action<T>> eventHandlers))
        {
            eventHandlers.Add(eventHandler);
        }
        else
            _callbacks.Add((modDefinition, contentDefinitionName, isBefore), [eventHandler]);
    }
}
