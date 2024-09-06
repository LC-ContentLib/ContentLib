using BepInEx;
using BepInEx.Logging;
using ContentLib.Core.Model.Event;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Patches;
using ContentLib.EnemyAPI.Test;
using UnityEngine;

namespace ContentLib.EnemyAPI;

/// <summary>
/// The Plugin instance of ContentLib.EnemyAPI.
/// </summary>
/// <exclude />
[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource s_log = null!;
    private bool _isTesting = true;

    private void Awake()
    {
        s_log = Logger;
        CLLogger.Instance.Log("Enemy Module is loading!");
        InitPatches();
        CLLogger.Instance.Log("Enemy Module is loaded!");
        
        if (!_isTesting) return;
        
        CLLogger.Instance.DebugLog("Test mode is enabled!");
        RegisterTestListeners();
    }

    private void InitPatches()
    {
        CLLogger.Instance.Log("Enemy Module Patches Initializing!");
        BrackenPatches.Init();
        CLLogger.Instance.Log("Enemy Module Patches Initialized!");

    }

    private void RegisterTestListeners()
    {
        CLLogger.Instance.DebugLog("Registering test listeners...",DebugLevel.EnemyEvent);
        GameEventManager.Instance.RegisterListener(new TestListener());
    }
}
