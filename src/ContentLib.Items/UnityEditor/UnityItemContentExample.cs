using System.Collections.Generic;
using ContentLib.Core;
using UnityEngine;

namespace ContentLib.Items.UnityEditor
{
    [CreateAssetMenu(menuName = "ContentLib/Item Content Example")]
    public class ItemContentExampleSO : ScriptableObject, IItemContentExample, IContent
    {
        [Header("Basic Info")]
        [SerializeField]
        string itemName;

        [SerializeField]
        List<string> spawnPositionTypes;

        [SerializeField]
        private string levelType;

        [SerializeField]
        private int rarity;

        [Header("Handling")]
        [SerializeField]
        private bool twoHanded;

        [SerializeField]
        private bool twoHandedAnimation;

        [SerializeField]
        private bool disableHandsOnWall;

        [SerializeField]
        private bool canBeGrabbedBeforeGameStart;

        [Header("Physics")]
        [SerializeField]
        private float weight;

        [SerializeField]
        private bool itemIsTrigger;

        [SerializeField]
        private bool holdButtonUse;

        [SerializeField]
        private bool itemSpawnsOnGround;

        [Header("Scrap Properties")]
        [SerializeField]
        private bool isScrap;

        [SerializeField]
        private int creditsWorth;

        [SerializeField]
        private bool lockedInDemo;

        [SerializeField]
        private int highestSalePercentage;

        [SerializeField]
        private int maxValue;

        [SerializeField]
        private int minValue;

        [Header("Prefab")]
        [SerializeField]
        private GameObject spawnPrefab;

        [Header("Battery Settings")]
        [SerializeField]
        private bool requiresBattery;

        [SerializeField]
        private float batteryUsage;

        [SerializeField]
        private bool automaticallySetUsingPower;

        [Header("UI")]
        [SerializeField]
        private Sprite itemIcon;

        [Header("Animations")]
        [SerializeField]
        private string grabAnim;

        [SerializeField]
        private string useAnim;

        [SerializeField]
        private string pocketAnim;

        [SerializeField]
        private string throwAnim;

        [SerializeField]
        private float grabAnimationTime;

        [Header("Audio")]
        [SerializeField]
        private AudioClip grabSFX;

        [SerializeField]
        private AudioClip dropSFX;

        [SerializeField]
        private AudioClip pocketSFX;

        [SerializeField]
        private AudioClip throwSFX;

        [Header("Networking")]
        [SerializeField]
        private bool syncGrabFunction;

        [SerializeField]
        private bool syncUseFunction;

        [SerializeField]
        private bool syncDiscardFunction;

        [SerializeField]
        private bool syncInteractLRFunction;

        [Header("Save Data")]
        [SerializeField]
        private bool saveItemVariable;

        [Header("Combat")]
        [SerializeField]
        private bool isDefensiveWeapon;

        [Header("Tooltip and Offsets")]
        [SerializeField]
        private string[] toolTips;

        [SerializeField]
        private float verticalOffset;

        [SerializeField]
        private int floorYOffset;

        [SerializeField]
        private bool allowDroppingAheadOfPlayer;

        [SerializeField]
        private Vector3 restingRotation;

        [SerializeField]
        private Vector3 rotationOffset;

        [SerializeField]
        private Vector3 positionOffset;

        [Header("Mesh Variants")]
        [SerializeField]
        private bool meshOffset;

        [SerializeField]
        private Mesh[] meshVariants;

        [SerializeField]
        private Material[] materialVariants;

        [Header("Misc")]
        [SerializeField]
        private bool isConductiveMetal;

        [SerializeField]
        private bool usableInSpecialAnimations;

        [SerializeField]
        private bool canBeInspected;

        // Interface Implementation
        public string GetName() => itemName;
        public List<string> GetSpawnPositionTypes() => spawnPositionTypes;
        public string GetLevelType() => levelType;
        public int GetRarity() => rarity;
        public bool GetTwoHanded() => twoHanded;
        public bool GetTwoHandedAnimation() => twoHandedAnimation;
        public bool GetDisableHandsOnWall() => disableHandsOnWall;
        public bool GetCanBeGrabbedBeforeGameStart() => canBeGrabbedBeforeGameStart;
        public float GetWeight() => weight;
        public bool GetItemIsTrigger() => itemIsTrigger;
        public bool GetHoldButtonUse() => holdButtonUse;
        public bool GetItemSpawnsOnGround() => itemSpawnsOnGround;
        public bool GetIsScrap() => isScrap;
        public int GetCreditsWorth() => creditsWorth;
        public bool GetLockedInDemo() => lockedInDemo;
        public int GetHighestSalePercentage() => highestSalePercentage;
        public int GetMaxValue() => maxValue;
        public int GetMinValue() => minValue;
        public GameObject GetSpawnPrefab() => spawnPrefab;
        public bool GetRequiresBattery() => requiresBattery;
        public float GetBatteryUsage() => batteryUsage;
        public bool GetAutomaticallySetUsingPower() => automaticallySetUsingPower;
        public Sprite GetItemIcon() => itemIcon;
        public string GetGrabAnim() => grabAnim;
        public string GetUseAnim() => useAnim;
        public string GetPocketAnim() => pocketAnim;
        public string GetThrowAnim() => throwAnim;
        public float GetGrabAnimationTime() => grabAnimationTime;
        public AudioClip GetGrabSFX() => grabSFX;
        public AudioClip GetDropSFX() => dropSFX;
        public AudioClip GetPocketSFX() => pocketSFX;
        public AudioClip GetThrowSFX() => throwSFX;
        public bool GetSyncGrabFunction() => syncGrabFunction;
        public bool GetSyncUseFunction() => syncUseFunction;
        public bool GetSyncDiscardFunction() => syncDiscardFunction;
        public bool GetSyncInteractLRFunction() => syncInteractLRFunction;
        public bool GetSaveItemVariable() => saveItemVariable;
        public bool GetIsDefensiveWeapon() => isDefensiveWeapon;
        public string[] GetToolTips() => toolTips;
        public float GetVerticalOffset() => verticalOffset;
        public int GetFloorYOffset() => floorYOffset;
        public bool GetAllowDroppingAheadOfPlayer() => allowDroppingAheadOfPlayer;
        public Vector3 GetRestingRotation() => restingRotation;
        public Vector3 GetRotationOffset() => rotationOffset;
        public Vector3 GetPositionOffset() => positionOffset;
        public bool GetMeshOffset() => meshOffset;
        public Mesh[] GetMeshVariants() => meshVariants;
        public Material[] GetMaterialVariants() => materialVariants;
        public bool GetUsableInSpecialAnimations() => usableInSpecialAnimations;
        public bool GetCanBeInspected() => canBeInspected;
        public bool GetIsConductiveMetal() => isConductiveMetal;
        public string Name { get; }
        
        internal static readonly Dictionary<ItemContentExampleSO, ItemContent> s_UnityToModItem = [];

        public IRegisteredContent Register(ModDefinition owner) => Resolve().Register(owner);
        
        public IContent Resolve()
        {
            ItemCreator itemCreator = new ItemCreator();
            Item item = itemCreator.createItem(this);
            ItemContent itemContent = new ItemContent(item);
            s_UnityToModItem.Add(this, itemContent);
            return this;
        }
    }
}
