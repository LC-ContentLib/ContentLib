using Unity.Netcode;

namespace ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;

/// <summary>
/// Interface representing the general functionality of a Bracken.
/// </summary>
public interface IBracken : IEnemy
{
    void AddToAngerMeter(float amountToAdd);
    
    float Anger { get; }
    
    bool IsAngry { get; }

    [ServerRpc]
    void EnterAngerModeServerRpc(float angerTime);

    [ClientRpc]
    void EnterAngerModeClientRpc(float angerTime);
    
    void AvoidClosestPlayer();
    
    void LookAtTrigger(int playerId);
    
    [ServerRpc]
    void ResetStealthTimerServerRpc(int playerObj);

    [ClientRpc]
    void ResetStealthTimerClientRpc(int playerObj);
    
}