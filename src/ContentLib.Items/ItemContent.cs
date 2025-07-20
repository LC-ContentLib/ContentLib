using System.Collections.Generic;
using ContentLib.Core;

namespace ContentLib.Items;

/// <summary>
/// A ContentLib <see cref="ItemContent"/>.
/// </summary>
public class ItemContent(Item item) : IContent<ItemContent>
{
    /// <inheritdoc/>
    public string Name => Item.name;

    /// <inheritdoc/>
    public Item Item { get; } = ThrowHelper.ThrowIfArgumentNull(item);
    internal static List<RegisteredContent<ItemContent>> s_RegisteredItems = [];

    /// <inheritdoc/>
    public RegisteredContent<ItemContent> Register(ModDefinition owner)
    {
        var registered = ContentRegistry.Register(this, owner);

        // TODO: actual logic to register item.
#if !UNITY_EDITOR_ASSEMBLY
        NetworkPrefabManager.RegisterNetworkPrefab(Item.spawnPrefab);
        s_RegisteredItems.Add(registered);
        LethalLib.Modules.Items.RegisterScrap(Item);
#endif
        return registered;
    }

    IRegisteredContent IContent.Register(ModDefinition owner) => Register(owner);

    /// <inheritdoc/>
    public IContent Resolve() => this;
}
