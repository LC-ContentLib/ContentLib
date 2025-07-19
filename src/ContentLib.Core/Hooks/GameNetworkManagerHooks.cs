#if !UNITY_EDITOR
using MonoDetour;
using MonoDetour.HookGen;
using On.GameNetworkManager;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.Core.Hooks;

[MonoDetourTargets(typeof(GameNetworkManager))]
static class GameNetworkManagerHooks
{
    [MonoDetourHookInitialize]
    static void Init()
    {
        Start.Postfix(Postfix_RegisterNetworkPrefabs);
    }

    private static void Postfix_RegisterNetworkPrefabs(GameNetworkManager self)
    {
        foreach (GameObject gameObject in NetworkPrefabManager.s_networkPrefabs)
        {
            if (!NetworkManager.Singleton.NetworkConfig.Prefabs.Contains(gameObject))
                NetworkManager.Singleton.AddNetworkPrefab(gameObject);
        }
    }
}
#endif
