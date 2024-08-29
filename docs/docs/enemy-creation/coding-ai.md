# Coding Our Custom AI

>[!IMPORTANT]
>You should use a [ILSpy](FILL THIS IN) or [DnSpy](FILL THIS IN) to look how the enemy AI scripts work in the game.  
>All the game code is contained inside `Lethal Company/Lethal Company_Data/Managed/Assembly-CSharp.dll`, so you should open that file in your decompiler!  
>To open the file, there will usually be a button at the top left, for `DnSpy` it is "File", from there, press "Open..." and select the specified dll from above.  
>Keep in mind that you are NOT allowed to distribute this file ANYWHERE, make sure it is out of your github if you place the dll there or anywhere.  
>`Assembly_CSharp.dll` contains all the game mode, which makes it illegal to distribute, so beware!  
>[!tip]
>See our tips for optimizing your mod testing workflow on [Mod Testing Tips](/dev/mod-testing-tips.md)! These will be particularly helpful when tweaking your AI.  

## Overview of EnemyAI

This guide will walk you through the process of creating a custom enemy AI in Lethal Company, using the `EnemyAI` base class as a foundation.
Every enemy in Lethal Company inherits from the `abstract class EnemyAI`, so we do the same. We will now go over some of the relevant methods:

The `Start()` method will run when the enemy spawns in a level. We can initialize our variables here.

The `Update()` method will run every frame, and we should try to avoiding intensive calculations here.  
This is also where the enemy position gets updated for clients other than the host:

```cs
// ... in EnemyAI, Update()
if (!inSpecialAnimation)
{
    base.transform.position = Vector3.SmoothDamp(base.transform.position, serverPosition, ref tempVelocity, syncMovementSpeed);
    base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Mathf.LerpAngle(base.transform.eulerAngles.y, targetYRotation, 15f * Time.deltaTime), base.transform.eulerAngles.z);
}
```

This also means that if `syncMovementSpeed` is zero, or a very big number, the enemy movement will appear janky on clients other than the host.

The `DoAIInterval()` method runs in an interval we've set in Unity on our `CustomAI` script (which inherits `EnemyAI`) on the enemy's prefab.  
By default this is set to 0.2 seconds, which is also used in the game by for example the BaboonHawk enemy and probably other enemies too.

If `movingTowardsTargetPlayer` is set to true, the `EnemyAI`'s `NavMeshAgent` will automatically set `destination` to the `targetPlayer` IF `targetPlayer` is not null.  
Both `base.Update()` and `base.DoAIInterval()` Methods work together to set the position/`destination` of the enemy:

```cs
// Update method of EnemyAI.
public virtual void Update()
{
    // ... deep in EnemyAI.Update
    if (this.movingTowardsTargetPlayer && this.targetPlayer != null)
    {
        if (this.setDestinationToPlayerInterval <= 0f)
        {
            // Sets the timer to reset where the enemy is going (so every 0.25 seconds the player's precise NAVMESH position is updated for the enemy).
            this.setDestinationToPlayerInterval = 0.25f;
            // Sets a destination that it uses in `Base.DoAIInterval()`.
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
}
```

```cs
// ... in EnemyAI.DoAIInterval
public virtual void DoAIInterval()
{
    if (moveTowardsDestination) {
        // agent is the Nav Mesh Agent attached to our prefab
        agent.SetDestination(destination);
    }
    // Updates serverPosition to current enemy position on server if
    // distance from serverPosition to current position is above
    // updatePositionThreshold, which we set in our custom AI script
    // in Unity.
    SyncPositionToClients();
}
```

As shown above, the enemy updates its destination every `Base.DoAIInterval()` call if `moveTowardsDestination` is `true`. It is true by default, and also gets set `true` if you run the `SetDestinationToPosition()` method.  

### `public bool SetDestinationToPosition(Vector3 position, [bool checkForPath = false])`

#### Parameters

- `Vector3 position`: the `destination` of the enemy.
- `bool checkForPath` *(optional, default is `false`)*: if `true`, it should check if there's a `NavMeshPath` from where it currently is to the given `position`.

##### Return Value

Returns `true` if it was able to find a valid path and setting a destination, or `false` otherwise, preventing movement.

Running `SetDestinationToPosition()` sets `movingTowardsTargetPlayer` to false, and updates the `destination` variable for use in DoAIInterval.

`OnCollideWithPlayer()` and `OnCollideWithEnemy()` are methods that runs once an object with both an __isTrigger__ [`Collider`](https://docs.unity3d.com/ScriptReference/Collider.html) and the `EnemyAICollisionDetect` Script attached to the same `GamObject` collide with a player/enemy.  
This is also the [`Collider`](https://docs.unity3d.com/ScriptReference/Collider.html) that is hittable with the `Shovel`.  
`HitEnemy()` still needs to be implemented for the enemy to be able to take damage and die like so:

```cs
// ... inside our CustomAI implementation.
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

`SetEnemyStunned` is the base-game method called when an `EnemyAI` is nearby an exploded `StunGrenade` (Mods could also cause this method to be called on your enemy too).  

```cs
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

One last important method that you'd need to keep in mind when creating a custom enemy is a method ran on `Base.Update()` exclusively for daytime enemies, `CheckTimeOfDayToLeave()`.  

```cs
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

// ... in DocileLocustBeesAI
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

### `public void KillEnemyOnOwnerClient([bool overrideDestroy = false])`

#### Parameters

- `bool overrideDestroy` *(optional, default is `false`)*: if `true`, it should completely destroy the enemy when killing it.

>[!TIP]
>When we want to implement these methods from `EnemyAI` into our AI script, we will have to use the `override` modifier on the method to override the virtual base method.  
>We will also want to call the original virtual method inside our override method like this:  

```cs
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

```cs
System.Random enemyRandom;

public override void Start()
{
    base.Start();
    enemyRandom = new System.Random(StartOfRound.Instance.randomMapSeed + thisEnemyIndex);
}
```

And we can use it for example this: `enemyRandom.Next(0, 5)`. This will choose the next random integer in our range.

We should still be careful about using random, as it is still possible that for example some `if` statement might have a different outcome due to some small desync, and then our random numbers also get desynced.

One way to ensure we don't get desync is to use ClientRpc methods, as those are networked. See the Unity Docs on [ClientRpcs](https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/clientrpc/) for more information. To be able to use these methods like in Unity, we can use [Unity Netcode Patcher](https://github.com/EvaisaDev/UnityNetcodePatcher). It is already set up in our example enemy project.

## Making More Complex AI

In order to properly structure our AI when it gets more complex is to use enums. Enums can be used to more explicitly define the "state" that our AI is in. Do note however that the game uses `currentBehaviourStateIndex` for the state of the enemy's behavior, and this can be changed with `SwitchToBehaviourClientRpc()`. For example:

```cs
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

```cs
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

```cs
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

We've now converted our AI into a state machine by using an enum! This helps you organize larger AI systems into chunks that can't interfere with each other so you'll encounter less bugs. It's also a lot easier for you to now add more states to your AI without having to use a bunch of `if` checks.

## Common Mistakes

## External Resources

### C# Reference

[Virtual modifier](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual)  
[Override modifier](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override)  
[Abstract modifier](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract)  
[Enums](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum)  
[Switch statement](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#the-switch-statement)

### Networking - Unity Docs

::: warning IMPORTANT
We are using [Unity Netcode Patcher](https://github.com/EvaisaDev/UnityNetcodePatcher) to make our custom Rpc methods work.
:::

[ClientRpc](https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/clientrpc/)  
[ServerRpc](https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/serverrpc/)
