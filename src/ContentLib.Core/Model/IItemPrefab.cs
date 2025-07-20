using UnityEngine;

namespace ContentLib.Core.Model;

public interface IItemPrefab
{
    IObjectContent GetContent();
    IScanNode GetScanNode();
    GameObject GetPrefabObject { get; }
}
