using System;
using System.Collections.Generic;
using BepInEx;
using ContentLib.Core.Exceptions;
using UnityEngine;

namespace ContentLib.Core;

/// <summary>
/// Contains metadata for a mod so that registered <see cref="ContentDefinition"/> objects can have an owner.<br/>
/// </summary>
/// <remarks>
/// This is mostly intended to be used as ScriptableObjects in Unity, but it can be used programmatically too,<br/>
/// although ContentLib can automatically create a <see cref="ContentDefinition"/> from your 
/// <see cref="BepInEx.BepInPlugin"/> Attribute in your assembly, using reflection.<br/>
/// <br/>
/// If two or more <see cref="ModDefinition"/> instances with the same <see cref="ModGUID"/> are loaded
/// from AssetBundles, the one that was loaded first will get all the Content from the other loaded
/// mods merged, and others will reference the same Content list.<br/>
/// <br/>
/// In this situation, equality comparisons with registered <see cref="ContentDefinition"/> instances'
/// Mod properties with this <see cref="ModDefinition"/> instance might not equal <see langword="true"/>.
/// </remarks>
[CreateAssetMenu(fileName = "ModDefinition", menuName = "ContentLib/Core/ModDefinition", order = 0)]
public class ModDefinition : ScriptableObject
{
    [SerializeField] private string _modGUID = null!;
    [SerializeField] private string _modName = null!;

    /// <summary>
    /// The GUID of this mod. This is an unique identifier for your mod.<br/>
    /// Example:<br/>
    /// <c>com.github.lc-contentlib.core</c>
    /// </summary>
    public string ModGUID => _realModDefinition._modGUID;

    /// <summary>
    /// The name of this mod.<br/>
    /// Example:<br/>
    /// <c>ContentLib.Core</c>
    /// </summary>
    public string ModName => _realModDefinition._modName;

    /// <summary>
    /// The content this mod contains.
    /// </summary>
    [field: SerializeField] public List<ContentDefinition> Content { get; private set; } = [];

    /// <summary>
    /// A Dictionary containing all <see cref="ModDefinition"/> instances.<br/>
    /// Takes a <see cref="ModGUID"/> as the key.
    /// </summary>
    public static IReadOnlyDictionary<string, ModDefinition> AllMods => s_allMods;
    private static readonly Dictionary<string, ModDefinition> s_allMods = [];
    private ModDefinition _realModDefinition = null!;

    /// <param name="pluginInfo">PluginInfo for a plugin from BepInEx.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <inheritdoc cref="Create(string, string)"/>
    public static ModDefinition Create(PluginInfo pluginInfo)
    {
        if (pluginInfo is null)
            throw new ArgumentNullException($"{nameof(pluginInfo)} is null!");

        return Create(pluginInfo.Metadata);
    }

    /// <param name="bepInPlugin">A BepInPlugin Attribute which contains the mod's GUID and name.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <inheritdoc cref="Create(string, string)"/>
    public static ModDefinition Create(BepInPlugin bepInPlugin)
    {
        if (bepInPlugin is null)
            throw new ArgumentNullException($"{nameof(bepInPlugin)} is null!");

        return Create(bepInPlugin.GUID, bepInPlugin.Name);
    }

    /// <summary>
    /// Creates or gets an existing instance of a <see cref="ModDefinition"/> ScriptableObject.
    /// </summary>
    /// <remarks>
    /// Using this method isn't necessary, as ContentLib will create a ModDefinition based on the
    /// <see cref="BepInEx.BepInPlugin"/> Attribute in your plugin assembly when registering a
    /// <see cref="ContentDefinition"/>.
    /// </remarks>
    /// <param name="modGUID">The GUID of the mod. This is an unique identifier for your mod.</param>
    /// <param name="modName">The name of the mod.</param>
    /// <returns>
    /// A new or an existing <see cref="ModDefinition"/> based on the GUID.
    /// If an existing one was found, the modName is ignored and the existing modName is used.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public static ModDefinition Create(string modGUID, string modName)
    {
        if (string.IsNullOrEmpty(modGUID))
            throw new ArgumentException("String must not be null or empty!", nameof(modGUID));

        if (string.IsNullOrEmpty(modName))
            throw new ArgumentException("String must not be null or empty!", nameof(modName));

        if (s_allMods.TryGetValue(modGUID, out ModDefinition existingMod))
            return existingMod;

        ModDefinition mod = CreateInstance<ModDefinition>();
        mod._modGUID = modGUID;
        mod._modName = modName;
        mod.Awake();

        return mod;
    }

    /// <summary>
    /// Get the real instance of this ModDefinition.
    /// </summary>
    /// <remarks>
    /// This is only useful if a second ModDefinition with the same <see cref="ModGUID"/>
    /// is loaded from an AssetBundle, as a loaded instance cannot re-assign its instance.<br/>
    /// ModDefinition instances created programmatically will already point to the real instance.
    /// </remarks>
    /// <returns>This instance or the 'real' instance if this is a duplicate.</returns>
    public ModDefinition GetRealInstance() => _realModDefinition;

    private void Awake()
    {
        _realModDefinition = this;

        // if ScriptableObject was made through CreateInstance,
        // run Awake manually after the properties are initialized.
        // Unity should serialize this object's fields before calling Awake.
        if (ModGUID == null)
            return;

        // These were already checked for creating a ModDefinition programmatically,
        // these checks here are for loading a ModDefinition from an AssetBundle.
        if (string.IsNullOrEmpty(ModGUID))
            throw new InvalidModDefinitionLoadedException($"{nameof(ModGUID)}");

        if (string.IsNullOrEmpty(ModName))
            throw new InvalidModDefinitionLoadedException($"{nameof(ModName)}");

        // If this is a duplicate, merge Content to existing mod's Content and reference that.
        if (s_allMods.TryGetValue(ModGUID, out ModDefinition existingMod))
        {
            existingMod.Content.AddRange(Content);
            Content = existingMod.Content;
            _realModDefinition = existingMod;
            return;
        }

        s_allMods.Add(ModGUID, this);
    }

    /// <summary>
    /// Validates all the content from this mod.
    /// </summary>
    /// <returns>If all the content passed validation, or otherwise a message with each issue.</returns>
    public (bool isValid, string? message) ValidateContent()
    {
        (bool isValid, string? message) result = new(true, null);

        foreach (ContentDefinition contentDefinition in Content)
        {
            (bool isValid, string? message) newResult = contentDefinition.Validate();

            if (!newResult.isValid)
            {
                result.isValid = false;
                result.message += $"\n{newResult.message}";
            }
        }

        return result;
    }


    /// <summary>
    /// Registers all the content from this mod that hasn't been registered.
    /// </summary>
    public void RegisterContent()
    {
        foreach (ContentDefinition contentDefinition in Content)
        {
            if (!contentDefinition.IsRegistered)
                contentDefinition.Register();
        }
    }
}
