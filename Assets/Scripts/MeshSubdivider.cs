using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshSubdivider : MonoBehaviour
{
    private MeshFilter meshFilter;
    private QuadMeshData meshData = new QuadMeshData();
    public Transform selectedGameObject;
    public string nameOfAsset;
    // public Mesh viewedModel;
    public void ShowCube()
    {
        meshData.vertices = new List<Vector3>{
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0),
            new Vector3(1,0,0),
            new Vector3(0,0,1),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,0,1)
        };
        meshData.quads = new List<Vector4>{
            new Vector4(0,1,2,3),
            new Vector4(0,4,5,1),
            new Vector4(4,7,6,5),
            new Vector4(3,2,6,7),
            new Vector4(0,3,7,4),
            new Vector4(1,5,6,2)
        };
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = meshData.ToUnityMesh();
    }

    public void Subdivide()
    {
        meshData = CatmullClark.Subdivide(meshData);
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = meshData.ToUnityMesh();
    }

    public void Clear()
    {
        meshFilter.sharedMesh.Clear();
    }
    public void SaveAsPrefab()
    {
        MeshFilter mf = selectedGameObject.GetComponent<MeshFilter>();
        if ((mf) && (nameOfAsset!=null))
        {
            var savePath = "Assets/Meshes/"+nameOfAsset+".asset";
            AssetDatabase.CreateAsset(mf.sharedMesh, savePath);
            Debug.Log("Saved Mesh to:" + savePath);
        }
    }
}

[CustomEditor(typeof(MeshSubdivider))]
class MeshSubdividerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Show Cube"))
        {
            var meshSubdivider = target as MeshSubdivider;
            meshSubdivider.ShowCube();
        }

        if (GUILayout.Button("Subdivide"))
        {
            var meshSubdivider = target as MeshSubdivider;
            meshSubdivider.Subdivide();
        }

        if(GUILayout.Button("Delete & Restart"))
        {
             var meshSubdivider = target as MeshSubdivider;
             meshSubdivider.Clear();
        }
        if(GUILayout.Button("Save"))
        {
             var meshSubdivider = target as MeshSubdivider;
             meshSubdivider.SaveAsPrefab();
        }
    }
}