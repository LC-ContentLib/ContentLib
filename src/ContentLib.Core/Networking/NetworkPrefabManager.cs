
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.Core.Networking;

/// <summary>
/// Handles registering network prefabs.
/// </summary>
public static class NetworkPrefabManager
{
    private static HashSet<GameObject>? s_networkPrefabs;

    /// <summary>
    /// Registers a prefab to be added to the network manager.
    /// </summary>
    /// <remarks>
    /// ContentLib already automatically registers relevant prefabs in <see cref="ContentDefinition"/> as network prefabs.
    /// </remarks>
    /// <exception cref="ArgumentNullException"></exception>
    public static void RegisterNetworkPrefab(GameObject prefab)
    {
        if (prefab is null)
            throw new ArgumentNullException(nameof(prefab), $"The given argument for {nameof(RegisterNetworkPrefab)} is null!");

        if (s_networkPrefabs is null)
        {
            s_networkPrefabs = [];
            On.GameNetworkManager.Start += Hook_RegisterNetworkPrefabs;
        }

        if (!s_networkPrefabs.Contains(prefab))
            s_networkPrefabs.Add(prefab);
    }

    private static void Hook_RegisterNetworkPrefabs(On.GameNetworkManager.orig_Start orig, GameNetworkManager self)
    {
        orig(self);

        foreach (GameObject gameObject in s_networkPrefabs!)
        {
            if (!NetworkManager.Singleton.NetworkConfig.Prefabs.Contains(gameObject))
                NetworkManager.Singleton.AddNetworkPrefab(gameObject);
        }
    }
}
