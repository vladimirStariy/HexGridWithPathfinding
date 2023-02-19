using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public Vector2Int offsetCoordinates;
    public Vector3Int cubeCoordinates;

    public int H;
    public int G;
    public int F => G + H;

    public List<HexCell> neighbours;

    public void CacheNeighbours()
    {
        neighbours = HexCellManager.Instance.GetNeighbours(this);
    }

    public void OnDrawGizmosSelected()
    {
        foreach(HexCell neighbour in neighbours)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.1f);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, neighbour.transform.position);
        }
    }
}