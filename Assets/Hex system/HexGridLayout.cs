using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("Grid size")]
    public Vector2Int gridSize;

    [Header("Hex tile parameters")]
    public Material material;
    public Unit unit;
    public bool isFlatTopped;

    HexCell[] cells;
    GameObject[] Hexagons;

    private void Awake()
    {
        cells = new HexCell[gridSize.x * gridSize.y];
        Hexagons = new GameObject[gridSize.x * gridSize.y];
    }

    private void Start()
    {
        LayoutGrid();
        unit.Spawn(HexCellManager.Instance.Cells.Values.ToArray()[50]);
    }

    private void LayoutGrid()
    {
        for(int y = 0, i = 0; y < gridSize.y; y++)
        {
            for(int x = 0; x < gridSize.x; x++)
            {
                CreateHexCell(y,x,i++);
            }
        }
        foreach(var item in gameObject.GetComponentsInChildren<HexCell>())
        {
            item.CacheNeighbours();
        }
    }

    private void CreateHexCell(int y, int x, int i)
    {
        GameObject hexTile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));
        hexTile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x,y));
        hexTile.AddComponent<HexCell>();

        HexRenderer hexRenderer = hexTile.GetComponent<HexRenderer>();
        hexRenderer.isFlatTopped = isFlatTopped;
        hexRenderer.SetMaterial(material);
        hexRenderer.DrawMesh();

        hexTile.transform.SetParent(transform, true);

        hexTile.GetComponent<HexCell>().offsetCoordinates = new Vector2Int(x, y);
        hexTile.GetComponent<HexCell>().cubeCoordinates = HexCoordinate.OffsetToCude(new Vector2Int(x, y));

        cells[i] = hexTile.GetComponent<HexCell>();
        HexCellManager.Instance.RegisterCell(hexTile.GetComponent<HexCell>());
        Hexagons[i] = hexTile;
    }

    private Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;

        float width;
        float height;
        float xPosition;
        float yPosition;
        bool shouldOffset;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = HexMetrics.outerRadius;

        if(!isFlatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f/4f);

            offset = (shouldOffset) ? width / 2 : 0;

            xPosition = (column * (horizontalDistance)) + offset;
            yPosition = (row * verticalDistance);
        }
        else
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width * (3f/4f);
            verticalDistance = height;

            offset = (shouldOffset) ? width / 2 : 0;

            xPosition = (column * (horizontalDistance));
            yPosition = (row * verticalDistance) - offset;
        }

        return new Vector3(xPosition, 0, -yPosition);
    }
}
