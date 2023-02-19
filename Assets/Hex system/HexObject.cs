using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexObject : MonoBehaviour
{
    [SerializeField]
    private bool _hasBuilding = false;
    private bool _isClaimed;

    public bool HasBuilding()
    {
        return _hasBuilding;
    } 
    public void SetBuilding()
    {
        _hasBuilding = true;
    }
}
