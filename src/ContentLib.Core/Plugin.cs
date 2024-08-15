using BepInEx;
using BepInEx.Logging;

namespace ContentLib.Core;

/// <summary>
/// The Plugin instance of ContentLib.Core.
/// </summary>
[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = null!;

    private void Awake()
    {
        Log = Logger;
        Log.LogInfo($"Plugin {LCMPluginInfo.PLUGIN_NAME} is loaded!");

        // TODO: Test behavior with loading an "identical" ModDefinition from an AssetBundle
        ModDefinition.Create("Author", "Test");
        ModDefinition.Create("Author", "Test");
    }
}
