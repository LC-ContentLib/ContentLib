using ContentLib.Core.Model;
using ContentLib.Items;
using ContentLib.Items.UnitySOs;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    ItemContentExampleSO _unityItemContent;

    void Start()
    {
        var itemCreator = new ItemCreator();
        itemCreator.createItem(_unityItemContent);
    }
}
