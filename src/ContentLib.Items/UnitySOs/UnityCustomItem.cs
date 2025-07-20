using ContentLib.Core.Model;
using KBCore.Refs;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.Items.UnitySOs;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(NetworkObject))]
public class UnityCustomItem: ValidatedMonoBehaviour, IItemPrefab
{
    [SerializeField, Self] 
    InterfaceRef<IObjectContent> _itemObjectContent;

    [SerializeField, Child]
    InterfaceRef<IScanNode> _scanNode;


    public IObjectContent GetContent()
    {
        return _itemObjectContent.Value;
    }

    public IScanNode GetScanNode()
    {
        return _scanNode.Value;
    }

    public GameObject GetPrefabObject => gameObject;
}
