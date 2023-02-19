using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class HexRenderer : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    private List<Face> _faces;

    public bool isFlatTopped;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();

        _mesh = new Mesh();
        _mesh.name = "hex";

        _meshFilter.mesh = _mesh;
        _meshCollider.sharedMesh = _mesh;
        _meshCollider.convex = true;
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }

    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    private void CombineFaces()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for(int i = 0; i < _faces.Count; i++)
        {
            vertices.AddRange(_faces[i].vertices);
            uvs.AddRange(_faces[i].uvs);

            int offset = (4*i);
            foreach (var triangle in _faces[i].triangles)
            {
                triangles.Add(triangle + offset);
            }
        }

        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = triangles.ToArray();
        _mesh.uv = uvs.ToArray();
        _mesh.RecalculateNormals();
    }

    private void DrawFaces()
    {
        _faces = new List<Face>();

        //top
        for(int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(HexMetrics.innerRadius, HexMetrics.outerRadius, HexMetrics.height / 2f, HexMetrics.height / 2f, point));
        }
        //bottom
        for(int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(HexMetrics.innerRadius, HexMetrics.outerRadius, -HexMetrics.height / 2f, -HexMetrics.height / 2f, point, true));
        }
        //outer
        for(int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(HexMetrics.outerRadius, HexMetrics.outerRadius, HexMetrics.height / 2f, -HexMetrics.height / 2f, point, true));
        }
        //inner
        for(int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(HexMetrics.innerRadius, HexMetrics.innerRadius, HexMetrics.height / 2f, -HexMetrics.height / 2f, point, false));
        }
    }

    private Face CreateFace(float innerRad, float outRad, float heightA, float heightB, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRad, heightB, point);
        Vector3 pointB = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
        Vector3 pointC = GetPoint(outRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outRad, heightA, point);

        List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
        List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new List<Vector2>() 
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1)
        };

        if(reverse)
        {
            vertices.Reverse();
        }

        return new Face(vertices, triangles, uvs);
    }

    private Vector3 GetPoint(float size, float height, int index)
    {
        float angle_deg = 60*index-30;
        float angle_rad = Mathf.PI / 180f * angle_deg;
        return new Vector3((size * Mathf.Cos(angle_rad)), height, size * Mathf.Sin(angle_rad));
    }
}