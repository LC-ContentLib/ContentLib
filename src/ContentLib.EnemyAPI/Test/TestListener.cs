using ContentLib.Core.Model.Event;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;
using UnityEngine;

namespace Lethal_Promotions.Model.Events.Listeners;

public class TestListener 
{
    public TestListener()
    {
        GameEventManager.Instance.Subscribe<MonsterCollideWithPlayerEvent>(GameEventType.MonsterPlayerCollisionEvent,
            OnMonsterColision);
        Debug.Log("TestListener has been registered");
    }
    
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