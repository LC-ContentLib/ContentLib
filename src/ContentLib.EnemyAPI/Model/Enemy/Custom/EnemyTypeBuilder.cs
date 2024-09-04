using UnityEngine;

public class EnemyTypeBuilder
{
    private string _enemyName;
    private AnimationCurve _probabilityCurve;
    private bool _spawningDisabled;
    private bool _spawnFromWeeds;
    private AnimationCurve? _numberSpawnedFalloff;
    private bool _useNumberSpawnedFalloff;
    private int _spawnInGroupsOf;
    private bool _requireNestObjectsToSpawn;
    private GameObject _enemyPrefab;
    private float _powerLevel;
    private int _maxCount;
    private bool _isOutsideEnemy;
    private bool _isDaytimeEnemy;
    private float _normalizedTimeInDayToLeave;
    private float _stunTimeMultiplier;
    private float _doorSpeedMultiplier;
    private float _stunGameDifficultyMultiplier;
    private bool _canBeStunned;
    private bool _canDie;
    private bool _destroyOnDeath;
    private bool _canSeeThroughFog;
    private float _pushPlayerForce;
    private float _pushPlayerDistance;
    private NavSizeLimit _sizeLimit;
    private float _timeToPlayAudio;
    private float _loudnessMultiplier;
    private AudioClip _overrideVentSFX;
    private GameObject _nestSpawnPrefab;
    private float _nestSpawnPrefabWidth;
    private bool _useMinEnemyThresholdForNest;
    private int _minEnemiesToSpawnNest;
    private AudioClip _hitBodySFX;
    private AudioClip _hitEnemyVoiceSFX;
    private AudioClip _deathSFX;
    private AudioClip _stunSFX;
    private MiscAnimation[] _miscAnimations;
    private AudioClip[] _audioClips;

    public EnemyTypeBuilder()
    {
        // Default initialization can be set here if necessary
    }

    public EnemyTypeBuilder SetEnemyName(string enemyName)
    {
        _enemyName = enemyName;
        return this;
    }

    public EnemyTypeBuilder SetProbabilityCurve(AnimationCurve curve)
    {
        _probabilityCurve = curve;
        return this;
    }

    public EnemyTypeBuilder SetSpawningDisabled(bool spawningDisabled)
    {
        _spawningDisabled = spawningDisabled;
        return this;
    }

    public EnemyTypeBuilder SetSpawnFromWeeds(bool spawnFromWeeds)
    {
        _spawnFromWeeds = spawnFromWeeds;
        return this;
    }

    public EnemyTypeBuilder SetNumberSpawnedFalloff(AnimationCurve? numberSpawnedFalloff, bool useNumberSpawnedFalloff = true)
    {
        _numberSpawnedFalloff = numberSpawnedFalloff;
        _useNumberSpawnedFalloff = useNumberSpawnedFalloff;
        return this;
    }

    public EnemyTypeBuilder SetSpawnInGroupsOf(int spawnInGroupsOf)
    {
        _spawnInGroupsOf = spawnInGroupsOf;
        return this;
    }

    public EnemyTypeBuilder SetRequireNestObjectsToSpawn(bool requireNestObjectsToSpawn)
    {
        _requireNestObjectsToSpawn = requireNestObjectsToSpawn;
        return this;
    }

    public EnemyTypeBuilder SetEnemyPrefab(GameObject enemyPrefab)
    {
        _enemyPrefab = enemyPrefab;
        return this;
    }

    public EnemyTypeBuilder SetPowerLevel(float powerLevel)
    {
        _powerLevel = powerLevel;
        return this;
    }

    public EnemyTypeBuilder SetMaxCount(int maxCount)
    {
        _maxCount = maxCount;
        return this;
    }

    public EnemyTypeBuilder SetIsOutsideEnemy(bool isOutsideEnemy)
    {
        _isOutsideEnemy = isOutsideEnemy;
        return this;
    }

    public EnemyTypeBuilder SetIsDaytimeEnemy(bool isDaytimeEnemy)
    {
        _isDaytimeEnemy = isDaytimeEnemy;
        return this;
    }

    public EnemyTypeBuilder SetNormalizedTimeInDayToLeave(float normalizedTimeInDayToLeave)
    {
        _normalizedTimeInDayToLeave = normalizedTimeInDayToLeave;
        return this;
    }

    public EnemyTypeBuilder SetStunTimeMultiplier(float stunTimeMultiplier)
    {
        _stunTimeMultiplier = stunTimeMultiplier;
        return this;
    }

    public EnemyTypeBuilder SetDoorSpeedMultiplier(float doorSpeedMultiplier)
    {
        _doorSpeedMultiplier = doorSpeedMultiplier;
        return this;
    }

    public EnemyTypeBuilder SetStunGameDifficultyMultiplier(float stunGameDifficultyMultiplier)
    {
        _stunGameDifficultyMultiplier = stunGameDifficultyMultiplier;
        return this;
    }

    public EnemyTypeBuilder SetCanBeStunned(bool canBeStunned)
    {
        _canBeStunned = canBeStunned;
        return this;
    }

    public EnemyTypeBuilder SetCanDie(bool canDie)
    {
        _canDie = canDie;
        return this;
    }

    public EnemyTypeBuilder SetDestroyOnDeath(bool destroyOnDeath)
    {
        _destroyOnDeath = destroyOnDeath;
        return this;
    }

    public EnemyTypeBuilder SetCanSeeThroughFog(bool canSeeThroughFog)
    {
        _canSeeThroughFog = canSeeThroughFog;
        return this;
    }

    public EnemyTypeBuilder SetPushPlayerForce(float pushPlayerForce)
    {
        _pushPlayerForce = pushPlayerForce;
        return this;
    }

    public EnemyTypeBuilder SetPushPlayerDistance(float pushPlayerDistance)
    {
        _pushPlayerDistance = pushPlayerDistance;
        return this;
    }

    public EnemyTypeBuilder SetSizeLimit(NavSizeLimit sizeLimit)
    {
        _sizeLimit = sizeLimit;
        return this;
    }

    public EnemyTypeBuilder SetTimeToPlayAudio(float timeToPlayAudio)
    {
        _timeToPlayAudio = timeToPlayAudio;
        return this;
    }

    public EnemyTypeBuilder SetLoudnessMultiplier(float loudnessMultiplier)
    {
        _loudnessMultiplier = loudnessMultiplier;
        return this;
    }

    public EnemyTypeBuilder SetOverrideVentSFX(AudioClip overrideVentSFX)
    {
        _overrideVentSFX = overrideVentSFX;
        return this;
    }

    public EnemyTypeBuilder SetNestSpawnPrefab(GameObject nestSpawnPrefab)
    {
        _nestSpawnPrefab = nestSpawnPrefab;
        return this;
    }

    public EnemyTypeBuilder SetNestSpawnPrefabWidth(float nestSpawnPrefabWidth)
    {
        _nestSpawnPrefabWidth = nestSpawnPrefabWidth;
        return this;
    }

    public EnemyTypeBuilder SetUseMinEnemyThresholdForNest(bool useMinEnemyThresholdForNest)
    {
        _useMinEnemyThresholdForNest = useMinEnemyThresholdForNest;
        return this;
    }

    public EnemyTypeBuilder SetMinEnemiesToSpawnNest(int minEnemiesToSpawnNest)
    {
        _minEnemiesToSpawnNest = minEnemiesToSpawnNest;
        return this;
    }

    public EnemyTypeBuilder SetHitBodySFX(AudioClip hitBodySFX)
    {
        _hitBodySFX = hitBodySFX;
        return this;
    }

    public EnemyTypeBuilder SetHitEnemyVoiceSFX(AudioClip hitEnemyVoiceSFX)
    {
        _hitEnemyVoiceSFX = hitEnemyVoiceSFX;
        return this;
    }

    public EnemyTypeBuilder SetDeathSFX(AudioClip deathSFX)
    {
        _deathSFX = deathSFX;
        return this;
    }

    public EnemyTypeBuilder SetStunSFX(AudioClip stunSFX)
    {
        _stunSFX = stunSFX;
        return this;
    }

    public EnemyTypeBuilder SetMiscAnimations(MiscAnimation[] miscAnimations)
    {
        _miscAnimations = miscAnimations;
        return this;
    }

    public EnemyTypeBuilder SetAudioClips(AudioClip[] audioClips)
    {
        _audioClips = audioClips;
        return this;
    }

    public EnemyType Build()
    {
        EnemyType enemyType = ScriptableObject.CreateInstance<EnemyType>();
        enemyType.enemyName = _enemyName;
        enemyType.probabilityCurve = _probabilityCurve;
        enemyType.spawningDisabled = _spawningDisabled;
        enemyType.spawnFromWeeds = _spawnFromWeeds;
        enemyType.numberSpawnedFalloff = _numberSpawnedFalloff;
        enemyType.useNumberSpawnedFalloff = _useNumberSpawnedFalloff;
        enemyType.spawnInGroupsOf = _spawnInGroupsOf;
        enemyType.requireNestObjectsToSpawn = _requireNestObjectsToSpawn;
        enemyType.enemyPrefab = _enemyPrefab;
        enemyType.PowerLevel = _powerLevel;
        enemyType.MaxCount = _maxCount;
        enemyType.isOutsideEnemy = _isOutsideEnemy;
        enemyType.isDaytimeEnemy = _isDaytimeEnemy;
        enemyType.normalizedTimeInDayToLeave = _normalizedTimeInDayToLeave;
        enemyType.stunTimeMultiplier = _stunTimeMultiplier;
        enemyType.doorSpeedMultiplier = _doorSpeedMultiplier;
        enemyType.stunGameDifficultyMultiplier = _stunGameDifficultyMultiplier;
        enemyType.canBeStunned = _canBeStunned;
        enemyType.canDie = _canDie;
        enemyType.destroyOnDeath = _destroyOnDeath;
        enemyType.canSeeThroughFog = _canSeeThroughFog;
        enemyType.pushPlayerForce = _pushPlayerForce;
        enemyType.pushPlayerDistance = _pushPlayerDistance;
        enemyType.SizeLimit = _sizeLimit;
        enemyType.timeToPlayAudio = _timeToPlayAudio;
        enemyType.loudnessMultiplier = _loudnessMultiplier;
        enemyType.overrideVentSFX = _overrideVentSFX;
        enemyType.nestSpawnPrefab = _nestSpawnPrefab;
        enemyType.nestSpawnPrefabWidth = _nestSpawnPrefabWidth;
        enemyType.useMinEnemyThresholdForNest = _useMinEnemyThresholdForNest;
        enemyType.minEnemiesToSpawnNest = _minEnemiesToSpawnNest;
        enemyType.hitBodySFX = _hitBodySFX;
        enemyType.hitEnemyVoiceSFX = _hitEnemyVoiceSFX;
        enemyType.deathSFX = _deathSFX;
        enemyType.stunSFX = _stunSFX;
        enemyType.miscAnimations = _miscAnimations;
        enemyType.audioClips = _audioClips;

        return enemyType;
    }
}
