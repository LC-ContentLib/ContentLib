using System.Collections.Generic;
using UnityEngine;

namespace ContentLib.Core;

public class ItemCreator
{

    
    public Item createItem(IItemContentExample itemContent)
    {
        Item item = ScriptableObject.CreateInstance<Item>();
        item.itemName = "Item";
        item.spawnPositionTypes = CreatePositionTypes(itemContent.GetSpawnPositionTypes());
        item.allowDroppingAheadOfPlayer = itemContent.GetAllowDroppingAheadOfPlayer();
        item.twoHandedAnimation = itemContent.GetTwoHandedAnimation();
        item.itemIcon = itemContent.GetItemIcon();
        item.weight = itemContent.GetWeight();
        item.twoHanded = itemContent.GetTwoHanded();
        item.disableHandsOnWall = itemContent.GetDisableHandsOnWall();
        item.canBeGrabbedBeforeGameStart = itemContent.GetCanBeGrabbedBeforeGameStart();
        item.itemIsTrigger = itemContent.GetItemIsTrigger();
        item.holdButtonUse = itemContent.GetHoldButtonUse();
        item.itemSpawnsOnGround = itemContent.GetItemSpawnsOnGround();
        item.isConductiveMetal = itemContent.GetIsConductiveMetal();
        item.isScrap = itemContent.GetIsScrap();
        item.creditsWorth = itemContent.GetCreditsWorth();
        item.lockedInDemo = itemContent.GetLockedInDemo();
        item.highestSalePercentage = itemContent.GetHighestSalePercentage();
        item.maxValue = itemContent.GetMaxValue();
        item.minValue = itemContent.GetMinValue();
        item.spawnPrefab = itemContent.GetSpawnPrefab();
        item.requiresBattery = itemContent.GetRequiresBattery();
        item.batteryUsage = itemContent.GetBatteryUsage();
        item.automaticallySetUsingPower  = itemContent.GetAutomaticallySetUsingPower();
        item.grabAnim = itemContent.GetGrabAnim();
        item.useAnim = itemContent.GetUseAnim();
        item.pocketAnim = itemContent.GetPocketAnim();
        item.throwAnim = itemContent.GetThrowAnim();
        item.grabAnimationTime = itemContent.GetGrabAnimationTime();
        item.grabSFX = itemContent.GetGrabSFX();
        item.dropSFX = itemContent.GetDropSFX();
        item.pocketSFX = itemContent.GetPocketSFX();
        item.throwSFX = itemContent.GetThrowSFX();
        item.syncGrabFunction = itemContent.GetSyncGrabFunction();
        item.syncUseFunction = itemContent.GetSyncUseFunction();
        item.syncDiscardFunction = itemContent.GetSyncDiscardFunction();
        item.syncInteractLRFunction = itemContent.GetSyncInteractLRFunction();
        item.saveItemVariable = itemContent.GetSaveItemVariable();
        item.isDefensiveWeapon = itemContent.GetIsDefensiveWeapon();
        item.toolTips = itemContent.GetToolTips();
        item.verticalOffset = itemContent.GetVerticalOffset();
        item.floorYOffset = itemContent.GetFloorYOffset();
        item.allowDroppingAheadOfPlayer = itemContent.GetAllowDroppingAheadOfPlayer();
        item.restingRotation = itemContent.GetRestingRotation();
        item.rotationOffset = itemContent.GetRotationOffset();
        item.positionOffset = itemContent.GetPositionOffset();
        item.meshOffset = itemContent.GetMeshOffset();
        item.meshVariants = itemContent.GetMeshVariants();
        item.materialVariants = itemContent.GetMaterialVariants();
        item.usableInSpecialAnimations = itemContent.GetUsableInSpecialAnimations();
        item.canBeInspected = itemContent.GetCanBeInspected();
        return item;
    }


    List<ItemGroup> CreatePositionTypes(List<string> groupNames)
    {
        List<ItemGroup> groups = new List<ItemGroup>();

        foreach (var groupName in groupNames)
        {
            ItemGroup group = ScriptableObject.CreateInstance<ItemGroup>();
            group.itemSpawnTypeName = groupName;
            groups.Add(group);
        }
        return groups;
        
    }
}
