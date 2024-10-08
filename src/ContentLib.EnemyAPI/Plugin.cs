﻿using BepInEx;
using BepInEx.Logging;
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
        s_log.LogInfo($"Plugin {LCMPluginInfo.PLUGIN_NAME} is loaded!");

        // We might need a project purely for tests. Leaving this as a reminder for later
        // as we could accidentally break this whole system and not realize for a while.
        EnemyDefinition myEnemy = ScriptableObject.CreateInstance<EnemyDefinition>();
        myEnemy.name = "testEnemyDefinition";

        // EnemyDefinition.Callbacks.AddOnBeforeRegister(myMod, "testEnemyDefinition",
        //     (enemy) => s_log.LogInfo("I was called! " + enemy.name));

        myEnemy.Register();
    }
}
