using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BepInEx;
using UnityEngine;

namespace ContentLib.Core;

/// <summary>
/// Base class for custom content that can be registered.
/// </summary>
public abstract class ContentDefinition : ScriptableObject
{
    /// <summary>
    /// The owner of this content.
    /// </summary>
    /// <remarks>
    /// This is guaranteed not null when <see cref="IsRegistered"/> of this instance is <see langword="true"/>.<br/>
    /// This is set in <see cref="Validate"/> if ModDefinition is successfully gotten via reflection
    /// based on the <see cref="BepInPlugin"/> Attribute. If this fails, validation fails, and the content can't be registered.
    /// </remarks>
    public ModDefinition? Mod { get; private set; }

    /// <summary>
    /// Get whether or not this content has been registered.
    /// </summary>
    [MemberNotNullWhen(returnValue: true, nameof(Mod))]
    public bool IsRegistered { get; protected set; }

    /// <summary>
    /// Validates this content.
    /// </summary>
    /// <returns>If this content passed validation, or otherwise a message with each issue.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Aggressive inlining is for GetCallingAssembly!
    public virtual (bool isValid, string? message) Validate()
    {
        (bool isValid, string? message) result = new(true, null);

        if (Mod is not null)
        {
            // The ModDefinition should always be in the the dictionary, so we validate it was created properly.
            if (!ModDefinition.AllMods.ContainsKey(Mod.ModName))
            {
                MarkAsInvalid(ref result,
                    $"The `{nameof(ModDefinition)} {nameof(Mod)}` of this ContentDefinition exists but isn't registered! " +
                    "Please create your ModDefinition through intended methods.");
            }
        }
        else if (TryGetModDefinitionFromCallingAssembly(out ModDefinition? modDefinition))
            Mod = modDefinition;
        else
        {
            MarkAsInvalid(ref result,
                $"Tried getting {nameof(BepInPlugin)} Attribute from CallingAssembly, but the attribute was not found! " +
                "Make sure to call ContentLib from a valid BepInEx Plugin assembly.");

            return result;
        }

        if (!Mod.Content.Contains(this))
            Mod.Content.Add(this);

        return result;
    }

    /// <summary>
    /// Validates and registers this content.
    /// </summary>
    /// <remarks>
    /// If this method completes without throwing, <see cref="IsRegistered"/> of this instance
    /// is set to <see langword="true"/>.
    /// </remarks>
    public abstract void Register();

    /// <summary>
    /// Used for marking a <see cref="Validate"/> return value as invalid.
    /// </summary>
    /// <param name="result">The object the <see cref="Validate"/> method will return.</param>
    /// <param name="message">A message that explains why the <see cref="ContentDefinition"/> was marked as invalid.</param>
    protected void MarkAsInvalid(ref (bool isValid, string? message) result, string message)
    {
        result.isValid = false;

        result.message ??= "Content failed validation for the following reasons:";
        result.message += $"\n{message}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Aggressive inlining is for GetCallingAssembly!
    private bool TryGetModDefinitionFromCallingAssembly([NotNullWhen(returnValue: true)] out ModDefinition? modDefinition)
    {
        modDefinition = null;
        var callingAssembly = Assembly.GetCallingAssembly();

        Type[] types;
        try
        {
			types = callingAssembly.GetTypes();
		}
        catch(ReflectionTypeLoadException ex)
        {
			types = ex.Types.Where(t => t != null).ToArray();
		}

        foreach (Type type in types)
        {
            if (type.IsSubclassOf(typeof(BaseUnityPlugin)))
            {
                var bepInPluginAttribute = (BepInPlugin)Attribute.GetCustomAttribute(type, typeof(BepInPlugin));

                if (bepInPluginAttribute is null)
                    continue;

                modDefinition = ModDefinition.Create(bepInPluginAttribute);
                return true;
            }
        }

        return false;
    }
}
