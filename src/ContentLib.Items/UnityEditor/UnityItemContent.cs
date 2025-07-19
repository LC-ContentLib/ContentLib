using System;
using System.Collections.Generic;
using ContentLib.Core;
using UnityEngine;

namespace ContentLib.Items.UnityEditor;

/// <summary>
/// A <see cref="ScriptableObject"/> representation of <see cref="ItemContent"/>.
/// </summary>
[CreateAssetMenu(fileName = "ItemContent", menuName = "ContentLib/ItemContent", order = 1)]
public class UnityItemContent : ScriptableObject, IContent
{
    /// <inheritdoc/>
    public string Name => Item is not null ? Item.name : name;

    /// <summary>
    /// The prefab that contains an <see cref="global::Item"/> component.
    /// </summary>
    [field: SerializeField]
    public Item Item { get; private set; } = null!;

    internal static readonly Dictionary<UnityItemContent, ItemContent> s_UnityToModItem = [];

    /// <inheritdoc/>
    public IRegisteredContent Register(ModDefinition owner) => Resolve().Register(owner);

    /// <inheritdoc/>
    public ItemContent Resolve()
    {
        if (s_UnityToModItem.TryGetValue(this, out var modItem))
        {
            return modItem;
        }

        if (Item == null)
            throw new NullReferenceException($"{nameof(Item)} component is null!");

        modItem = new(Item);
        s_UnityToModItem.Add(this, modItem);

        return modItem;
    }

    IContent IContent.Resolve() => Resolve();
}
