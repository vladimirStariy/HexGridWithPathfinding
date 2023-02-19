using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public int Civilization_ID;
    public int Production;
    public int Food;
    public int Population;

    public void PlaceCity(HexCell cell)
    {
        transform.position = cell.transform.position + new Vector3(0,1f,0);
    }
}
