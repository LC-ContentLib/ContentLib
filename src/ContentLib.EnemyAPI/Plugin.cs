using BepInEx;
using BepInEx.Logging;
using ContentLib.Core;
using UnityEngine;

namespace ContentLib.EnemyAPI;

/// <summary>
/// The Plugin instance of ContentLib.EnemyAPI.
/// </summary>
[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = null!;

    private void Awake()
    {
        Log = Logger;
        Log.LogInfo($"Plugin {LCMPluginInfo.PLUGIN_NAME} is loaded!");

        // We might need a project purely for tests. Leaving this as a reminder for later
        // as we could accidentally break this whole system and not realize for a while.
        var myMod = ModDefinition.Create("test", "test");
        EnemyDefinition myEnemy = ScriptableObject.CreateInstance<EnemyDefinition>();
        myEnemy.name = "testEnemyDefinition";

        EnemyDefinition.Callbacks.AddOnBeforeRegister(myMod, "testEnemyDefinition",
            (enemy) => Log.LogInfo("I was called! " + enemy.name));

        myEnemy.Register(myMod);
    }
}
