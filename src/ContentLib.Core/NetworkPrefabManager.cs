using System;
using System.Collections.Generic;
using UnityEngine;

namespace ContentLib.Core;

/// <summary>
/// Handles registering network prefabs.
/// </summary>
public static class NetworkPrefabManager
{
    /// <summary>
    /// Used by <see cref="Hooks.GameNetworkManagerHooks"/>.
    /// </summary>
    internal static HashSet<GameObject> s_networkPrefabs = [];

    /// <summary>
    /// Registers a prefab to be added to the network manager.
    /// </summary>
    /// <remarks>
    /// ContentLib already automatically registers relevant prefabs
    /// in <see cref="IContent"/> as network prefabs.
    /// </remarks>
    /// <exception cref="ArgumentNullException"></exception>
    public static void RegisterNetworkPrefab(GameObject prefab)
    {
        ThrowHelper.ThrowIfArgumentNull(prefab);

        if (!s_networkPrefabs.Contains(prefab))
            s_networkPrefabs.Add(prefab);
    }
}
