using System;
using System.Collections.Generic;
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
    /// <summary>
    /// The author of this mod.
    /// </summary>
    [field: SerializeField] public string AuthorName { get; private set; } = null!;

    /// <summary>
    /// The name of this mod.
    /// </summary>
    [field: SerializeField] public string ModName { get; private set; } = null!;

    /// <summary>
    /// The content this mod contains.
    /// </summary>
    [field: SerializeField] public List<ContentDefinition> Content { get; private set; } = [];

    /// <summary>
    /// A Dictionary containing all <see cref="ModDefinition"/> instances.
    /// </summary>
    public static IReadOnlyDictionary<(string AuthorName, string ModName), ModDefinition> AllMods => s_allMods;
    private static readonly Dictionary<(string AuthorName, string ModName), ModDefinition> s_allMods = [];
    internal ModDefinition _realModDefinition = null!;

    /// <summary>
    /// Creates or gets an existing an instance of a <see cref="ModDefinition"/> ScriptableObject.
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
        mod.AuthorName = authorName;
        mod.ModName = modName;
        mod.Awake();

        return mod;
    }

    private void Awake()
    {
        // if ScriptableObject was made through CreateInstance,
        // run Awake manually after the properties are initialized.
        // I don't know if this actually happens for loading from AssetBundles too, needs to be tested!!
        if (AuthorName == null)
            return;

        // If this is a duplicate, merge Content to existing mod's Content and reference that.
        if (s_allMods.TryGetValue((AuthorName, ModName), out ModDefinition existingMod))
        {
            existingMod.Content.AddRange(Content);
            Content = existingMod.Content;
            _realModDefinition = existingMod;
            return;
        }

        s_allMods.Add((AuthorName, ModName), this);
        _realModDefinition = this;
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
