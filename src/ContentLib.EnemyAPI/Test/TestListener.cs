using ContentLib.Core.Model.Event;
using ContentLib.Core.Model.Event.Listener;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;
using UnityEngine;

namespace ContentLib.EnemyAPI.Test;

public class TestListener : IListener
{
    
    [EventDelegate]
    private void OnMonsterColision(MonsterCollideWithPlayerEvent collideEvent)
    {
        Debug.Log("OnMonsterColision Check");
        IEnemy enemy = collideEvent.Enemy;
        Debug.Log($"The Enemy Type: {enemy.EnemyProperties.EnemyClassType}");
        if(enemy is IBracken)
            Debug.Log("It worked and is works");
        if (EnemyManager.Instance().IsEnemyTypeOf(enemy,typeof(IBracken)))
        {
            Debug.Log("[LC-ContentLib] The player has been killed by a Braken!");
        }
    }
}