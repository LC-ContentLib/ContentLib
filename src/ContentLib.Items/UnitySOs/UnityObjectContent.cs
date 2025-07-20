using ContentLib.Core.Model;
using UnityEngine;

public class UnityObjectContent : MonoBehaviour, IObjectContent
{
    public bool GetGrabbable() => true;

    public bool GetGrabbableToEnemies() => true;

    public bool GetIsInFactory() => true;
}
