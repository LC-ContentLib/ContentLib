# Coding Our Custom AI

>[!IMPORTANT]
>You should use [ILSpy](https://github.com/icsharpcode/ILSpy/releases) or [DnSpyEx](https://github.com/dnSpyEx/dnSpy/releases) to examine how the enemy AI scripts work in the game. All the game code is contained inside the `Lethal Company/Lethal Company_Data/Managed/Assembly-CSharp.dll` file, so you should open that file in your decompiler. To open the file, there is usually a button at the top left. (For DnSpy, it is `File`) From there, press "Open..." and select the specified DLL mentioned above.  
>Keep in mind that you are **not** allowed to distribute this file **anywhere**. Ensure it is removed from your GitHub or any other place where you might have uploaded it. `Assembly_CSharp.dll` contains all the game code, making it illegal to distribute, so be cautious!  
>[!tip]
>See our tips for optimizing your mod testing workflow on [Mod Testing Tips](/dev/mod-testing-tips.md)! These will be particularly helpful when tweaking your AI.  

## Table of Contents

- [Coding Our Custom AI](#coding-our-custom-ai)
  - [Table of Contents](#table-of-contents)
  - [Overview of EnemyAI](#overview-of-enemyai)
    - [Start Method](#start-method)
    - [Update Method](#update-method)
    - [DoAIInterval Method](#doaiinterval-method)
    - [EnemyAI Methods](#enemyai-methods)
      - [`public bool SetDestinationToPosition(Vector3 position, [bool checkForPath = false])`](#public-bool-setdestinationtopositionvector3-position-bool-checkforpath--false)
      - [`public void KillEnemyOnOwnerClient([bool overrideDestroy = false])`](#public-void-killenemyonownerclientbool-overridedestroy--false)
  - [Behavior Examples](#behavior-examples)
    - [Enemy Movement](#enemy-movement)
    - [Using Random Without Desync](#using-random-without-desync)
  - [Making More Complex AI](#making-more-complex-ai)
    - [Utility](#utility)
    - [MyComplexAI](#mycomplexai)
  - [Common Mistakes](#common-mistakes)
  - [External Resources](#external-resources)
    - [C# Reference](#c-reference)
    - [Randomness](#randomness)
    - [Unity Docs](#unity-docs)
  - [Contributors](#contributors)

## Overview of EnemyAI

This guide will walk you through the process of creating a custom enemy AI in Lethal Company, using the `EnemyAI` base class as a foundation.
Every enemy in Lethal Company inherits from `abstract class EnemyAI`, so we do the same.  

```csharp
 // namespaces are used to declare a scope that contains a set of related objects, see "C# References" for more information.
namespace CustomEnemy;
class CustomEnemyAI : EnemyAI
{
    // Empty for now.
}
```

We will now go over some of the relevant methods:  

### Start Method

The `Start()` method is part of Unity's lifecycle and is called before the first frame update. This is where you should initialize variables or game state before the game starts. It is a good place to set up the initial conditions for your enemy AI, such as defining starting positions, setting up custom random seeds, or initializing variables that don't need to be updated constantly.

### Update Method

The `Update()` method is called once per frame, and it's where the core logic of your AI's behavior should be placed. However, since this method runs every frame, it's crucial to avoid heavy computations here.  
This is also where the enemy position gets updated for clients other than the host:  

```csharp
// ... in EnemyAI.Update()
if (!inSpecialAnimation)
{
    base.transform.position = Vector3.SmoothDamp(base.transform.position, serverPosition, ref tempVelocity, syncMovementSpeed);
    base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Mathf.LerpAngle(base.transform.eulerAngles.y, targetYRotation, 15f * Time.deltaTime), base.transform.eulerAngles.z);
}
```

This also means that if `syncMovementSpeed` is zero, or a very big number, the enemy movement will appear janky on clients other than the host, see [Adding Network Transforms](./unity-project.md#adding-advanced-unity-tools) and [Network Transform Documentation](#unity-docs) for a smoother more optimised method for syncing movement in-between clients.  
If `movingTowardsTargetPlayer` is `true` and `targetPlayer` is not `null`, the `EnemyAI`'s [NavMeshAgent](#unity-docs) will automatically set `destination` to the `targetPlayer`'s Vector3 position.  
Both `base.Update()` and `base.DoAIInterval()` Methods work together to set the position/`destination` of the enemy:

```csharp
// ... in EnemyAI.Update
if (this.movingTowardsTargetPlayer && this.targetPlayer != null)
{
    if (this.setDestinationToPlayerInterval <= 0f)
    {
        // Sets the timer to reset where the enemy is going (so every 0.25 seconds the player's precise NAVMESH position is updated for the enemy).
        this.setDestinationToPlayerInterval = 0.25f;
        // Sets a destination that it uses in `base.DoAIInterval()`.
        this.destination = RoundManager.Instance.GetNavMeshPosition(this.targetPlayer.transform.position, RoundManager.Instance.navHit, 2.7f, -1);
    }
    else
    {
        // Sets the destination for the enemy when the interval is not up (The only change is that the destination isn't set to the NavMesh directly unlike in the if statement above, this is likely because GetNavMeshPosition is performance intensive.)
        this.destination = new Vector3(this.targetPlayer.transform.position.x, this.destination.y, this.targetPlayer.transform.position.z);
        // Decreases the timer for setting the enemy to the player's exact NavMesh position.
        this.setDestinationToPlayerInterval -= Time.deltaTime;
    }
    if (this.addPlayerVelocityToDestination > 0f)
    {
        // Checks to overshoot the destination depending on the direction and speed of the player moving.
        // Uses addPlayerVelocityToDestination set from EnemyAI script in Unity to multiply the destination's Vector3 position.
        if (this.targetPlayer == GameNetworkManager.Instance.localPlayerController)
        {
            this.destination += Vector3.Normalize(this.targetPlayer.thisController.velocity * 100f) * this.addPlayerVelocityToDestination;
        }
        else if (this.targetPlayer.timeSincePlayerMoving < 0.25f)
        {
            this.destination += Vector3.Normalize((this.targetPlayer.serverPlayerPosition - this.targetPlayer.oldPlayerPosition) * 100f) * this.addPlayerVelocityToDestination;
        }
    }
}
```

### DoAIInterval Method

The `DoAIInterval()` method runs in an interval we've set in Unity on our `CustomAI` script (which inherits `EnemyAI`) on the enemy's prefab.  
By default this is set to 0.2 (seconds).

```csharp
// ... in EnemyAI.DoAIInterval
if (moveTowardsDestination) {
    // agent is the NavMeshAgent attached to our prefab
    agent.SetDestination(destination);
}

// Updates serverPosition to the current position of the enemy by checking the distance
// Using the updatePositionThreshold value set in Unity.
SyncPositionToClients();  
```

As shown above, the enemy updates its destination every `base.DoAIInterval()` call if `moveTowardsDestination` is `true`. It is `true` both by default, and set to `true` through the [SetDestinationToPosition()](#public-bool-setdestinationtopositionvector3-position-bool-checkforpath--false) method.  

`OnCollideWithPlayer()` and `OnCollideWithEnemy()` are methods that runs once an object with both an **isTrigger** [`Collider`](#unity-docs) and the `EnemyAICollisionDetect` Script attached to the same `GamObject` collide with a player/enemy.  
This is also the [`Collider`](#unity-docs) that is hittable with the `Shovel`.  
`HitEnemy()` still needs to be implemented for the enemy to be able to take damage and die like so:

```csharp
// ... in our CustomAI implementation.
// Method to override (HitEnemy).
public override void HitEnemy(int force = 1, PlayerControllerB? playerWhoHit = null, bool playHitSFX = false, int hitID = -1)
{
    // Call base to play some sounds set in the enemyType such as hitEnemyVoiceSFX in enemyType.
    base.HitEnemy(force, playerWhoHit, playHitSFX, hitID);
    // No need to keep on calling it if the enemy is already dead.
    if (isEnemyDead) return;
    // Decrease the HP by force.
    enemyHP = enemyHP - force;
    
    // Base game calls to declare the enemy as dead on all clients.
    if (IsOwner && enemyHP <= 0 && !isEnemyDead) {
        KillEnemyOnOwnerClient();
        return;
    }
    // Optional Logging, will error out unless you have an ExtendedLogging method defined in your base Plugin class in Plugin.cs file.
    Plugin.ExtendedLogging($"Player who hit: {playerWhoHit}");
    Plugin.ExtendedLogging($"Enemy HP: {enemyHP}");
    Plugin.ExtendedLogging($"Hit with force {force}");
}
```

See implementation: [ExtendedLogging Utility](#utility)

`SetEnemyStunned` is the base-game method called when an `EnemyAI` is nearby an exploded `StunGrenade` (Mods could also cause this method to be called on your enemy too).  

```csharp
// ... inside EnemyAI.
public virtual void SetEnemyStunned(bool setToStunned, float setToStunTime = 1f, PlayerControllerB setStunnedByPlayer = null)
{
    // Checks to ensure that the enemy is dead and can be stunned before applying any stun.
    if (this.isEnemyDead)
    {
        return;
    }
    if (!this.enemyType.canBeStunned)
    {
        return;
    }
    // if setTuStunned is false, resets the enemy's stun timer.
    if (setToStunned)
    {
        // if currently invincible from a previous stun, don't stun again immediately after.
        if (this.postStunInvincibilityTimer >= 0f)
        {
            return;
        }
        // Play's a stun sound from enemyType if creatureVoice is not null and the enemy is NOT already stunned.
        if (this.stunNormalizedTimer <= 0f && this.creatureVoice != null)
        {
            this.creatureVoice.PlayOneShot(this.enemyType.stunSFX);
        }
        // Sets player that stunned the enemy, an invincibility timer of 0.5 seconds, and how long they'll be stunned for.
        this.stunnedByPlayer = setStunnedByPlayer;
        this.postStunInvincibilityTimer = 0.5f;
        this.stunNormalizedTimer = setToStunTime;
        return;
    }
    else
    {
        this.stunnedByPlayer = null;
        if (this.stunNormalizedTimer > 0f)
        {
            this.stunNormalizedTimer = 0f;
            return;
        }
        return;
    }
}
```

One last important method that you'd need to keep in mind when creating a custom enemy is a method called by `base.Update()` exclusively for daytime enemies, `CheckTimeOfDayToLeave()`.  

```csharp
// ... in EnemyAI
private void CheckTimeOfDayToLeave()
{
    // Null checking whether time exists in the level.
    if (TimeOfDay.Instance == null)
    {
        return;
    }
    // Checks whether this daytime enemy is an outside enemy (enemyType.isOutside needs to be enabled for daytime enemies too!!).
    // Checks whether it's been enough time in the day to "leave" (enemyType has a curve for normalisedTimeInDayToLeave set in Unity).
    if (TimeOfDay.Instance.timeHasStarted && TimeOfDay.Instance.normalizedTimeOfDay > this.enemyType.normalizedTimeInDayToLeave && this.isOutside)
    {
        // Sets leaving to true and calls the leave function, by default, the leave function does NOTHING, implementation is dependent on the enemy instead.
        this.daytimeEnemyLeaving = true;
        this.DaytimeEnemyLeave();
    }
}

public override void DaytimeEnemyLeave()
{
    base.DaytimeEnemyLeave();
    // Custom implementation, possibly despawning the enemy as vanilla enemies do and as seen below.
}

// ... in DocileLocustBeesAI (a vanilla daytime EnemyAI)
public override void DaytimeEnemyLeave()
{
    // Calls base (which only includes debug logs).
    base.DaytimeEnemyLeave();
    // ...Miscellanous code that includes playing sound effects once leaving.
    
    // Starts a Coroutine to determine when the bugs would "leave", implementation seen below.
    StartCoroutine(this.bugsLeave());
}

private IEnumerator bugsLeave()
{
    // Waits 6 seconds before executing the next pieces of code.
    yield return new WaitForSeconds(6f);
    // Kills the enemy as it is time for it to "leave".
    base.KillEnemyOnOwnerClient(true);
}
```

### EnemyAI Methods

#### `public bool SetDestinationToPosition(Vector3 position, [bool checkForPath = false])`

| **Parameter**                    | **Description**                                                                                                     |
|----------------------------------|---------------------------------------------------------------------------------------------------------------------|
| `Vector3 position`               | The `destination` of the enemy.                                                                                     |
| `bool checkForPath` *(optional)* | If `true`, checks if there's a `NavMeshPath` from the current position to the given `position`. Default is `false`.  |

| **Function Description**                                                                                                      |
|-------------------------------------------------------------------------------------------------------------------------------|
| The method `SetDestinationToPosition` sets the enemy's destination to a specified position. It optionally checks if a valid path exists before moving. |

| **Return Value**                                                                                                               |
|-------------------------------------------------------------------------------------------------------------------------------|
| Returns `true` if a valid path is found and the destination is set, or `false` otherwise, preventing movement.                 |

---

#### `public void KillEnemyOnOwnerClient([bool overrideDestroy = false])`

| **Parameter**                    | **Description**                                                                                                      |
|----------------------------------|----------------------------------------------------------------------------------------------------------------------|
| `bool overrideDestroy` *(optional)* | If `true`, the enemy will be completely destroyed when killed. Default is `false`.                                  |

| **Function Description**                                                                                                      |
|-------------------------------------------------------------------------------------------------------------------------------|
| The method `KillEnemyOnOwnerClient` is used to kill the enemy on the owner's client. If `overrideDestroy` is set to `true`, the enemy will be completely destroyed when killed. |

| **Return Value**                                                                                                               |
|-------------------------------------------------------------------------------------------------------------------------------|
| This method does not return a value (`void`).                                                                                  |

>[!TIP]
>When we want to implement these methods from `EnemyAI` in our AI script, we will have to use the `override` modifier on the method to override the [virtual](#c-reference) or [abstract](#c-reference) base method.  
>We will also want to call the original virtual method inside our override method like this:  

```csharp
public override void DoAIInterval()
{
   // Run original virtual method
    base.DoAIInterval();
    // ... custom logic
}
```

## Behavior Examples

### Enemy Movement

The `TargetClosestPlayer()` method can be used to make the enemy change its targetPlayer to the closest player.
Then, we can for example do `SetDestinationToPosition(targetPlayer.transform.position)` to make our enemy pathfind to where targetPlayer stands.

### Using Random Without Desync

We can implement our own random variable which we initialize with a set seed in our `Start()` method, and use it like this:

```csharp
System.Random enemyRandom;

public override void Start()
{
    base.Start();
    enemyRandom = new System.Random(StartOfRound.Instance.randomMapSeed + thisEnemyIndex);
}
```

We should be careful about using random, as it is still possible that, as an example, some `if` statement might have a different outcome due to some small desync, and then our randomly generated numbers become desynced across players.

One way to ensure we don't get desync is to use [ClientRpc](#unity-docs)'s and [ServerRpc](#unity-docs)'s methods, as those are networked. To be able to use these methods like in Unity, we can use [Unity Netcode Patcher](https://github.com/EvaisaDev/UnityNetcodePatcher). It is already set up in the example-enemy template project.

## Making More Complex AI

### Utility  

Just to start off as a super simple base, we should create an `ExtendedLogging` method that we would log everything inside of for our testing, this is so that we can hide these logs behind configs so users can disable them when they are not needed, otherwise, please use the approach Debug Levels when creating a log.

```csharp
// ... in Plugin class.

internal static void ExtendedLogging(object text) {
    // Example of gating log over a config value, usually true by default.
    if (ModConfig.ConfigEnableExtendedLogging.Value) {
        Logger.LogInfo(text);
    }
}
```

### MyComplexAI

In order to properly structure our AI when it gets more complex is to use enums. Enums can be used to more explicitly define the "state" that our AI is in. Do note however that the game uses `currentBehaviourStateIndex` for the state of the enemy's behavior, and this can be changed with `SwitchToBehaviourClientRpc()`. For example:

```csharp
class MyComplexAI : EnemyAI {
    // Note: also add your behavior states in your enemy script in Unity,
    // where you can configure things like a voice/sfx clip or an animation
    // to play when changing to a specific behavior state.
    // If we don't do that, we will get an index out of range exception.
    enum State
    {
        WANDERING,
        CHASING,
        // ... and many more states
    }
    // ...
    private void SomeExampleMethod()
    {
        // The owner of the enemy (the host by default) needs to call the 
        // method below. The method is a ClientRpc, meaning it will run for
        // all clients. This will update currentBehaviourStateIndex.
        SwitchToBehaviourClientRpc((int)State.CHASING);
    }
}
```

Now we have two states in this example, the `WANDERING` state and the `CHASING` state. What's great about enums is that we can very easily add a new state to our AI. In order to use our new states we need to modify our `DoAIInterval()` method.

```csharp
class MyComplexAI : EnemyAI
{
    enum State
    {
        WANDERING,
        CHASING,
        // ... and many more states
    }
    // ...
    public override void DoAIInterval()
    {
        // Run the original virtual method to sync enemy position
        // and update the enemy's pathing target.
        base.DoAIInterval();

        // The switch is a more advanced if statement that allows us to
        // more easily define what should happen in each state vs using
        // just a bunch of ifs.
        switch(currentBehaviourStateIndex)
        { 
            case State.WANDERING:
                // ... handle logic for looking for a player or
                // just wandering around.

                // This break is VERY IMPORTANT, it means that it won't 
                // just continue onto our next bit of code for the
                // different states.
                break; 
            case State.CHASING:
                // ... other logic to handle when we are chasing a player.
                break;
        }
    }
}
```

Now all we need to do is instruct *when* the AI should change state:

```csharp
// ... in our DoAIInterval() method
switch(currentBehaviourStateIndex)
{ 
    case State.WANDERING:
        // ... logic for looking for a player or just wandering around.
        if (foundPlayer)
        {
            SwitchToBehaviourClientRpc((int)State.CHASING);
            // The game will also print out some debug statements when the
            // above method is called.
        }
        break;
    case State.CHASING:
        // ... other logic to handle when we are chasing at a player.
        if (lostPlayer)
        {
            SwitchToBehaviourClientRpc((int)State.WANDERING);
        }
        break;
}
```

We've now converted our AI into a state machine by using an enum! This helps you organize larger AI systems into chunks that can't interfere with each other so you'll encounter less bugs. It's also a lot easier for you to now add more states to your AI without having to use a bunch of `Conditional Operators` such as "C# if-else" statements.

## Common Mistakes

- **Forgetting to Call `base` Methods**: When overriding methods, always remember to call the base method (e.g., `base.Start()`, `base.DoAIInterval()`) unless you have a specific reason not to. This ensures that the original functionality is preserved.
  
- **Overloading the `Update()` Method**: Placing too much logic in `Update()` can lead to performance issues, especially in networked environments. Offload logic to `DoAIInterval()` or use [Coroutines](#unity-docs) for actions that don't need to run every frame.

- **Desync Issues with Randomness**: When using [randomness](#randomness), ensure that all clients are synchronized by using the same seed or leveraging networked methods like [`ClientRpc`](#unity-docs) or [`ServerRpc`](#unity-docs).

## External Resources

### C# Reference

- [C# Programming Guide](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/)
- [namespace](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/namespace)  
- [Virtual modifier](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual)  
- [Override modifier](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override)  
- [Abstract modifier](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract)  
- [Enums](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum)  
- [Switch statement](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#the-switch-statement)  

### Randomness

- [UnityEngine Random](https://docs.unity3d.com/ScriptReference/Random.Range.html)  
- [System Random](https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-8.0)  
- [Randomness](https://developers.dusk.gg/docs/advanced/randomness/)  

### Unity Docs

- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/)
- [Collider](https://docs.unity3d.com/ScriptReference/Collider.html)
- [NavMeshAgent](https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent.html)
- [Coroutines](https://docs.unity3d.com/Manual/Coroutines.html)
- [ClientRpc](https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/clientrpc/)  
- [ServerRpc](https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/serverrpc/)  
- [Network Transform](https://docs-multiplayer.unity3d.com/netcode/current/components/networktransform/)  
- [Network Animator](https://docs-multiplayer.unity3d.com/netcode/current/components/networkanimator/)  

>[!IMPORTANT]
>We are using [Unity Netcode Patcher](https://github.com/EvaisaDev/UnityNetcodePatcher) to make Rpc methods work.

## Contributors

This documentation is a community effort! If you have suggestions for improvements or additional examples, feel free to contribute. Every bit of help is appreciated!  

- Hamunii (Suni)
- Xu Xiaolan (XuuXiao)
- Cosmo Brain (cosmobrain0)
