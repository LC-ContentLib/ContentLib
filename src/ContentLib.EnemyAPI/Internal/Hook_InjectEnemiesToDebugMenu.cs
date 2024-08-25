using UnityEngine;

namespace ContentLib.EnemyAPI.Internal;

internal static partial class EnemyDefinitionInjector
{
    /// <summary>
    /// Adds enemies to the Debug/Test Menu that can be accessed by forcing
    /// <see cref="Application.isEditor"/> to be true and opening the menu in-game as the host.
    /// </summary>
    private static void Hook_InjectEnemiesToDebugMenu(On.QuickMenuManager.orig_Start orig, QuickMenuManager self)
    {
        orig(self);

        SelectableLevel testLevel = self.testAllEnemiesLevel;

        foreach (EnemyDefinition enemyDefinition in s_enemiesToRegister)
        {
            SpawnableEnemyWithRarity enemyDefWithRarity = new()
            {
                enemyType = enemyDefinition.EnemyType,
                rarity = 0,
            };

            if (enemyDefinition.InsideLevelMatchingTags is not null)
                testLevel.Enemies.Add(enemyDefWithRarity);

            if (enemyDefinition.OutsideLevelMatchingTags is not null)
                testLevel.OutsideEnemies.Add(enemyDefWithRarity);

            if (enemyDefinition.DaytimeLevelMatchingTags is not null)
                testLevel.DaytimeEnemies.Add(enemyDefWithRarity);
        }
    }
}
