using BepInEx;
using BepInEx.Logging;
using ContentLib.API.Model.Event;
using ContentLib.Core.Loader;
using ContentLib.EnemyAPI.Model.Enemy.Custom;
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

    private void Awake()
    {
        s_log = Logger;
        RoundPatches.Init();
        EnemyAIPatches.Init();
        BrackenPatches.Init();
        s_log.LogInfo($"Plugin {LCMPluginInfo.PLUGIN_NAME} is loaded!");
        
        //------------------------------------------------------------

        #region TestMethods
        TestListener testListener = new();
        GameEventManager.Instance.RegisterListener(testListener);
        #endregion
        
    }
}
