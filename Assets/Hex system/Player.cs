using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HexCell currentCell;
    public Vector3Int playerPos; 

    private void Start()
    {
        currentCell = HexCellManager.Instance.Cells.Values.ToArray()[50];
        playerPos = currentCell.cubeCoordinates;
        transform.position = currentCell.transform.position + new Vector3(0, 1f, 0);
    }
}
