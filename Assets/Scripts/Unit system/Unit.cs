using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Civilization_ID;
    public string UnitName;
    public Vector3 WorldPosition;
    public Vector3Int CubePosition;
    public HexCell currentCell;
    public void SetUnitName(string name)
    {
        this.UnitName = name;
    }

    public void Move(HexCell destination)
    {
        this.CubePosition = destination.cubeCoordinates;
        transform.position = destination.transform.position + new Vector3(0, 1f, 0);
        this.currentCell = destination;
    }

    public void Spawn(HexCell spawnPoint)
    {
        this.currentCell = spawnPoint;
        this.CubePosition = spawnPoint.cubeCoordinates;
        transform.position = spawnPoint.transform.position + new Vector3(0, 1f, 0);
    }
}
