using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public static List<HexCell> FindPath(HexCell startCell, HexCell targetCell)
    {
        List<HexCell> openPathTiles = new List<HexCell>();
        List<HexCell> closedPathTiles = new List<HexCell>();

        HexCell currentCell = startCell;
        currentCell.G = 0;
        currentCell.H = GetEstimatedPathCost(startCell.cubeCoordinates, targetCell.cubeCoordinates);

        openPathTiles.Add(currentCell);

        while (openPathTiles.Count != 0)
        {
            // Sorting the open list to get the tile with the lowest F.
            openPathTiles = openPathTiles.OrderBy(x => x.F).ThenByDescending(x => x.H).ToList();
            currentCell = openPathTiles[0];

            // Removing the current tile from the open list and adding it to the closed list.
            openPathTiles.Remove(currentCell);
            closedPathTiles.Add(currentCell);

            int g = currentCell.G + 1;

            // If there is a target tile in the closed list, we have found a path.
            if (closedPathTiles.Contains(targetCell))
            {
                break;
            }

            // Investigating each adjacent tile of the current tile.
            foreach (HexCell neighbour in currentCell.neighbours)
            {
                // Ignore not walkable adjacent tiles.
                //if (adjacentTile.isObstacle)
                //{
                //    continue;
                //}

                // Ignore the tile if it's already in the closed list.
                if (closedPathTiles.Contains(neighbour))
                {
                    continue;
                }

                // If it's not in the open list - add it and compute G and H.
                if (!(openPathTiles.Contains(neighbour)))
                {
                    neighbour.G = g;
                    neighbour.H = GetEstimatedPathCost(neighbour.cubeCoordinates, targetCell.cubeCoordinates);
                    openPathTiles.Add(neighbour);
                }
                // Otherwise check if using current G we can get a lower value of F, if so update it's value.
                else if (neighbour.F > g + neighbour.H)
                {
                    neighbour.G = g;
                }
            }
        }

        List<HexCell> finalPathCells = new List<HexCell>();

        // Backtracking - setting the final path.
        if (closedPathTiles.Contains(targetCell))
        {
            currentCell = targetCell;
            finalPathCells.Add(currentCell);

            for (int i = targetCell.G - 1; i >= 0; i--)
            {
                currentCell = closedPathTiles.Find(x => x.G == i && currentCell.neighbours.Contains(x));
                finalPathCells.Add(currentCell);
            }

            finalPathCells.Reverse();
        }

        return finalPathCells;
    }

    protected static int GetEstimatedPathCost(Vector3Int startPosition, Vector3Int targetPosition)
    {
        return Mathf.Max
        (
            Mathf.Abs(startPosition.z - targetPosition.z), 
            Mathf.Max(Mathf.Abs(startPosition.x - targetPosition.x), 
            Mathf.Abs(startPosition.y - targetPosition.y))
        );
    }
}
