using System.Collections.Generic;

namespace ContentLib.EnemyAPI.Internal;

internal static partial class EnemyDefinitionInjector
{
    internal static bool s_lateForRegister = false;
    private static List<EnemyDefinition> s_enemiesToRegister = null!;

    internal static void Register(EnemyDefinition enemyDefinition)
    {
        if (s_enemiesToRegister is null)
        {
            s_enemiesToRegister = [enemyDefinition];
            InitializeHooks();
            return;
        }

        s_enemiesToRegister.Add(enemyDefinition);
    }

    private static void InitializeHooks()
    {
        On.Terminal.Start += Hook_InjectEnemiesToBestiary;
        On.QuickMenuManager.Start += Hook_InjectEnemiesToDebugMenu;
        On.RoundManager.Start += Hook_InjectEnemiesToLevels;
    }
}
