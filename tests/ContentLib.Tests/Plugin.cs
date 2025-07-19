using BepInEx;
using BepInEx.Logging;
using ContentLib.Core;

namespace ContentLib.Tests;

/// <summary>
/// BepInEx plugin of ContentLib.Items.
/// Depend on this with <c>[BepInDependency(ItemsPlugin.Id)]</c>.
/// </summary>
[BepInAutoPlugin]
public partial class TestsPlugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; } = BepInEx.Logging.Logger.CreateLogSource(Name);

    private void Awake()
    {
        this.LoadBundleAndContentsWithName("testscrap.contentbundle");

        Log.LogInfo($"Plugin {Name} is loaded!");
    }
}
