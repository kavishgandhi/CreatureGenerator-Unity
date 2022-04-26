using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuadMeshData
{
    public List<Vector3> vertices;
    public List<Vector4> quads;
    private List<Vector3> triangleVertices;
    private List<int> triangles;
    private MeshFilter meshFilter;

    public QuadMeshData()
    {
        vertices = new List<Vector3>();
        quads = new List<Vector4>();
    }

    // Initializes the QuadMeshData object with the given vertices and quads.
    public QuadMeshData(List<Vector3> vertices, List<Vector4> quads)
    {
        this.vertices = vertices;
        this.quads = quads;
    }


    public Mesh ToUnityMesh()
    {
        GetTriangleFaces();
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = triangleVertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }


    private void GetTriangleFaces()
    {
        List<Vector4> newQuads = new List<Vector4>();
        triangleVertices = new List<Vector3>();
        triangles = new List<int>();

        for (int i = 0; i < quads.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                triangleVertices.Add(vertices[(int)quads[i][j]]);
            }

            newQuads.Add(new Vector4(i * 4, i * 4 + 1, i * 4 + 2, i * 4 + 3));
        }

        foreach (Vector4 quad in newQuads)
        {
            triangles.Add((int)quad[0]);
            triangles.Add((int)quad[1]);
            triangles.Add((int)quad[2]);

            triangles.Add((int)quad[0]);
            triangles.Add((int)quad[2]);
            triangles.Add((int)quad[3]);
        }
    }
}