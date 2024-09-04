using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyHordeType", order = 2)]
public class EnemyHordeTypeSO : ScriptableObject, IEnemyHordeProperties
{
    // IEnemyHordeProperties implementation
    [SerializeField] private AnimationCurve numberSpawnedFalloff;
    public AnimationCurve? NumberSpawnedFalloff { get => numberSpawnedFalloff; set => numberSpawnedFalloff = value; }

    [SerializeField] private bool useNumberSpawnedFalloff;
    public bool UseNumberSpawnedFalloff { get => useNumberSpawnedFalloff; set => useNumberSpawnedFalloff = value; }

    [SerializeField] private int spawnInGroupsOf;
    public int SpawnInGroupsOf { get => spawnInGroupsOf; set => spawnInGroupsOf = value; }

    [SerializeField] private bool requireNestObjectsToSpawn;
    public bool RequireNestObjectsToSpawn { get => requireNestObjectsToSpawn; set => requireNestObjectsToSpawn = value; }

    [SerializeField] private float normalizedTimeInDayToLeave;
    public float NormalizedTimeInDayToLeave { get => normalizedTimeInDayToLeave; set => normalizedTimeInDayToLeave = value; }

    [SerializeField] private NavSizeLimit sizeLimit;
    public NavSizeLimit SizeLimit { get => sizeLimit; set => sizeLimit = value; }

    [SerializeField] private GameObject nestSpawnPrefab;
    public GameObject NestSpawnPrefab { get => nestSpawnPrefab; set => nestSpawnPrefab = value; }

    [SerializeField] private float nestSpawnPrefabWidth;
    public float NestSpawnPrefabWidth { get => nestSpawnPrefabWidth; set => nestSpawnPrefabWidth = value; }

    [SerializeField] private bool useMinEnemyThresholdForNest;
    public bool UseMinEnemyThresholdForNest { get => useMinEnemyThresholdForNest; set => useMinEnemyThresholdForNest = value; }

    [SerializeField] private int minEnemiesToSpawnNest;
    public int MinEnemiesToSpawnNest { get => minEnemiesToSpawnNest; set => minEnemiesToSpawnNest = value; }
}