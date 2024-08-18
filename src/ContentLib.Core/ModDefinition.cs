using System;
using System.Collections.Generic;
using ContentLib.Core.Exceptions;
using UnityEngine;

namespace ContentLib.Core;

/// <summary>
/// Contains metadata for a mod so that registered <see cref="ContentDefinition"/> objects can have an owner.
/// </summary>
/// <remarks>
/// If two or more <see cref="ModDefinition"/> instances with the same author name and mod name are loaded
/// from AssetBundles, the one that was loaded first will get all the Content from the other loaded
/// mods merged, and others will reference the same Content list.<br/>
/// <br/>
/// In this situation, equality comparisons with registered <see cref="ContentDefinition"/> instances'
/// Mod properties with this <see cref="ModDefinition"/> instance might not equal <see langword="true"/>.
/// </remarks>
[CreateAssetMenu(fileName = "ModDefinition", menuName = "ContentLib/Core/ModDefinition", order = 0)]
public class ModDefinition : ScriptableObject
{
    [SerializeField] private string _authorName = null!;
    [SerializeField] private string _modName = null!;

    /// <summary>
    /// The author of this mod.
    /// </summary>
    public string AuthorName => _realModDefinition._authorName;

    /// <summary>
    /// The name of this mod.
    /// </summary>
    public string ModName => _realModDefinition._modName;

    /// <summary>
    /// The content this mod contains.
    /// </summary>
    [field: SerializeField] public List<ContentDefinition> Content { get; private set; } = [];

    /// <summary>
    /// A Dictionary containing all <see cref="ModDefinition"/> instances.
    /// </summary>
    public static IReadOnlyDictionary<(string authorName, string modName), ModDefinition> AllMods => s_allMods;
    private static readonly Dictionary<(string authorName, string modName), ModDefinition> s_allMods = [];
    private ModDefinition _realModDefinition = null!;

    /// <summary>
    /// Creates or gets an existing instance of a <see cref="ModDefinition"/> ScriptableObject.
    /// </summary>
    /// <param name="authorName"></param>
    /// <param name="modName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ModDefinition Create(string authorName, string modName)
    {
        if (string.IsNullOrEmpty(authorName))
            throw new ArgumentException("String must not be null or empty!", nameof(authorName));

        if (string.IsNullOrEmpty(modName))
            throw new ArgumentException("String must not be null or empty!", nameof(modName));

        if (s_allMods.TryGetValue((authorName, modName), out ModDefinition existingMod))
            return existingMod;

        var mod = CreateInstance<ModDefinition>();
        mod._authorName = authorName;
        mod._modName = modName;
        mod.Awake();

        return mod;
    }

    /// <summary>
    /// Get the real instance of this ModDefinition.
    /// </summary>
    /// <remarks>
    /// This is only useful if a second ModDefinition with the same <see cref="AuthorName"/>
    /// and <see cref="ModName"/> are loaded from an AssetBundle, as a loaded instance cannot
    /// re-assign its instance.
    /// </remarks>
    /// <returns>This instance or the 'real' instance if this is a duplicate.</returns>
    public ModDefinition GetRealInstance() => _realModDefinition;

    private void Awake()
    {
        _realModDefinition = this;

        // if ScriptableObject was made through CreateInstance,
        // run Awake manually after the properties are initialized.
        // Unity should serialize this object's fields before calling Awake.
        if (AuthorName == null)
            return;

        // These were already checked for creating a ModDefinition programmatically,
        // these checks here are for loading a ModDefinition from an AssetBundle.
        if (string.IsNullOrEmpty(AuthorName))
            throw new InvalidModDefinitionLoadedException($"{nameof(AuthorName)}");

        if (string.IsNullOrEmpty(ModName))
            throw new InvalidModDefinitionLoadedException($"{nameof(ModName)}");

        // If this is a duplicate, merge Content to existing mod's Content and reference that.
        if (s_allMods.TryGetValue((AuthorName, ModName), out ModDefinition existingMod))
        {
            existingMod.Content.AddRange(Content);
            Content = existingMod.Content;
            _realModDefinition = existingMod;
            return;
        }

        s_allMods.Add((AuthorName, ModName), this);
    }

    /// <summary>
    /// Registers all the content from this mod that hasn't been registered.
    /// </summary>
    public void RegisterContent()
    {
        foreach (ContentDefinition contentDefinition in Content)
        {
            if (!contentDefinition.IsRegistered)
                contentDefinition.Register(this);
        }
    }
}
