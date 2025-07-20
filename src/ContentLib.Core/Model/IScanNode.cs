using UnityEngine;

namespace ContentLib.Core.Model;

public interface IScanNode
{
    int GetMaxRange();
    int GetMinRange();
    string GetHeaderText();
    GameObject GetScanNodeObject();
}
