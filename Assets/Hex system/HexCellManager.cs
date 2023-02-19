using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexCellManager : MonoBehaviour
{
    public static HexCellManager Instance;
    public Dictionary<Vector3Int, HexCell> Cells;
    public List<HexCell> path; 
    public GameObject settlerUI;
    public Unit selectedUnit;

    public bool isWalking;

    public GameObject city;

    private void Awake()
    {
        Instance = this;
        Cells = new Dictionary<Vector3Int, HexCell>();
    }

    public void RegisterCell(HexCell cell)
    {
        Cells.Add(cell.cubeCoordinates, cell);
    }

    public List<HexCell> GetNeighbours(HexCell cell)
    {
        List<HexCell> neighbours = new List<HexCell>();

        Vector3Int[] neighbourCoordinates = new Vector3Int[]
        {
            new Vector3Int(1,-1,0),
            new Vector3Int(1,0,-1),
            new Vector3Int(0,1,-1),
            new Vector3Int(-1,1,0),
            new Vector3Int(-1,0,1),
            new Vector3Int(0,-1,1)
        };

        foreach(var neighbourCoordinate in neighbourCoordinates)
        {
            Vector3Int cellCoordinate = cell.cubeCoordinates;
            if(Cells.TryGetValue(cellCoordinate + neighbourCoordinate, out HexCell neighbour))
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            foreach (HexCell cell in path)
            {
                Gizmos.DrawCube(cell.transform.position + new Vector3(0f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f));
            }
        }
    }

    public IEnumerator Movement()
    {
        isWalking = true;
        for(int i = 0; i < path.Count; i++)
        {
            selectedUnit.Move(path[i]);
            yield return new WaitForSeconds(1f);
        }
        isWalking = false;
        path.Clear();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
			HandleInput();
		}
        if (Input.GetMouseButton(0))
        {
            SelectUnit();
        }
    }

    void SelectUnit()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) 
        {
            Transform objectHit = hit.transform;
            if(hit.transform.tag == "selectable")
            {
                selectedUnit = objectHit.GetComponent<Unit>();
                settlerUI.SetActive(true);
            }
		}
    }

    public void PlaceCity()
    {
        var a = Instantiate(city, selectedUnit.currentCell.transform);
        a.transform.position = selectedUnit.currentCell.transform.position + new Vector3(0,1f,0);
        Destroy(selectedUnit.gameObject);
        selectedUnit = null;
    }

    void HandleInput () 
    {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) 
        {
            Transform objectHit = hit.transform;
            if(selectedUnit != null && !isWalking)
            {
                path = Pathfinder.FindPath(selectedUnit.currentCell, objectHit.GetComponent<HexCell>());
                StartCoroutine(Movement());
            }
		}
	}
        
}
