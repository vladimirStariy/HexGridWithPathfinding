using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public PathNode parent;
    public HexCell target;
    public HexCell destination;
    public HexCell origin;

    public int baseCost;
    public int costFromOrigin;
    public int costToDestination;
    public int pathCost;

    public PathNode(HexCell current, HexCell origin, HexCell destination, int pathCost)
    {
        parent = null;
        this.target = current;
        this.origin = origin;
        this.destination = destination;
        baseCost = 1;
        costFromOrigin = (int)Vector3Int.Distance(current.cubeCoordinates, origin.cubeCoordinates);
        costToDestination = (int)Vector3Int.Distance(current.cubeCoordinates, destination.cubeCoordinates);
        this.pathCost = pathCost;
    }

    public int GetCost()
    {
        return pathCost + baseCost + costFromOrigin + costToDestination;
    }

    public void SetParent(PathNode node)
    {
        this.parent = node;
    }
}
