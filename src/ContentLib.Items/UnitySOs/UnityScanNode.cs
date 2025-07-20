using ContentLib.Core.Model;
using UnityEngine;

namespace ContentLib.Items.UnitySOs;

[RequireComponent(typeof(BoxCollider))]
public class UnityScanNode : MonoBehaviour, IScanNode
{
    [SerializeField]
    int maxRange;
    
    [SerializeField]
    int minRange;

    [SerializeField]
    bool requiesLineOfSight;

    [SerializeField]
    string headerText;

    public int GetMaxRange()
    {
        return maxRange;
    }

    public int GetMinRange()
    {
        return minRange;
    }

    public string GetHeaderText()
    {
        return headerText;
    }

    public GameObject GetScanNodeObject()
    {
        return gameObject;
    }

}
