using System.Collections.Generic;
using ContentLib.Core.Model;
using UnityEngine;

namespace ContentLib.Core
{
    public interface IItemContentExample
    {
        string GetName();
        List<string> GetSpawnPositionTypes();

        int GetRarity();
        string GetLevelType();
        bool GetTwoHanded();
        bool GetTwoHandedAnimation();

        bool GetDisableHandsOnWall();
        bool GetCanBeGrabbedBeforeGameStart();

        float GetWeight();
        bool GetItemIsTrigger();
        bool GetHoldButtonUse();
        bool GetItemSpawnsOnGround();

        bool GetIsConductiveMetal();

        bool GetIsScrap();
        int GetCreditsWorth();
        bool GetLockedInDemo();
        int GetHighestSalePercentage();
        int GetMaxValue();
        int GetMinValue();

        
        IItemPrefab GetSpawnPrefab();

        bool GetRequiresBattery();
        float GetBatteryUsage();
        bool GetAutomaticallySetUsingPower();

        Sprite GetItemIcon();

        string GetGrabAnim();
        string GetUseAnim();
        string GetPocketAnim();
        string GetThrowAnim();

        float GetGrabAnimationTime();

        AudioClip GetGrabSFX();
        AudioClip GetDropSFX();
        AudioClip GetPocketSFX();
        AudioClip GetThrowSFX();

        bool GetSyncGrabFunction();
        bool GetSyncUseFunction();
        bool GetSyncDiscardFunction();
        bool GetSyncInteractLRFunction();

        bool GetSaveItemVariable();

        bool GetIsDefensiveWeapon();

        string[] GetToolTips();
        float GetVerticalOffset();
        int GetFloorYOffset();
        bool GetAllowDroppingAheadOfPlayer();

        Vector3 GetRestingRotation();
        Vector3 GetRotationOffset();
        Vector3 GetPositionOffset();

        bool GetMeshOffset();
        Mesh[] GetMeshVariants();
        Material[] GetMaterialVariants();

        bool GetUsableInSpecialAnimations();
        bool GetCanBeInspected();
    }
}
