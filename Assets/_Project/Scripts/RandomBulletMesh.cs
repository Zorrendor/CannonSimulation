using System;
using UnityEngine;

public class RandomBulletMesh : MonoBehaviour
{
    private static readonly int RandomSeed = Shader.PropertyToID("_RandomSeed");
    private Material material;
    
    private void Awake()
    {
        material = this.GetComponent<MeshRenderer>().material;
    }

    private void OnEnable()
    {
        material.SetInt(RandomSeed, UnityEngine.Random.Range(0, 100000));
    }
    
    #if UNITY_EDITOR
    [ContextMenu("Save cube")]
    public void SaveCubeWithConnectedEdges()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f)
        };

        int[] triangles = new int[]
        {
            0, 1, 2, 0, 2, 3, // Bottom
            1, 5, 6, 1, 6, 2, // Right
            5, 4, 7, 5, 7, 6, // Top
            4, 0, 3, 4, 3, 7, // Left
            3, 2, 6, 3, 6, 7, // Front
            4, 5, 1, 4, 1, 0  // Back
        };
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.UploadMeshData(true);
        
        string path = "Assets/_Project/Models/Cube.asset";
        UnityEditor.AssetDatabase.CreateAsset(mesh, path);
        UnityEditor.AssetDatabase.SaveAssets();
    }
    #endif
}
