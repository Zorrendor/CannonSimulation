using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletTrajectory : MonoBehaviour
{
    [SerializeField] private int pointCount = 100;
    [SerializeField] private float maxTime = 10f;
    [SerializeField] private CannonConfigSO cannonConfig;
    
    private LineRenderer lineRenderer;
    private NativeArray<Vector3> points;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = this.transform;
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = pointCount;
        points = new NativeArray<Vector3>(pointCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
    }

    private void OnDestroy()
    {
        points.Dispose();
    }

    private void Update()
    {
        float gravity = 9.81f;
        Vector3 initialVelocity = cameraTransform.forward * (cannonConfig.power * cannonConfig.powerMultiplier);
        for (int i = 0; i < pointCount; i++)
        {
            float t = maxTime * i / (pointCount - 1);
            Vector3 displacement = initialVelocity * t;
            displacement.y -= 0.5f * gravity * t * t;
        
            points[i] = cameraTransform.position + displacement;
        }
        lineRenderer.SetPositions(points);
        
        // lineRenderer.sharedMaterial.SetVector("_InitialVelocity", initialVelocity);
        // lineRenderer.sharedMaterial.SetVector("_InitialPosition", transform.position);
        // lineRenderer.sharedMaterial.SetFloat("_MaxTime", maxTime);
    }

    #if UNITY_EDITOR
    [ContextMenu("Save mesh")]
    public void SaveMesh()
    {
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component is missing!");
            return;
        }
        
        Mesh bakedMesh = new Mesh();
        lineRenderer.BakeMesh(bakedMesh, true);

        string path = "Assets/_Project/LineRendererMesh.asset";
        UnityEditor.AssetDatabase.CreateAsset(bakedMesh, path);
        UnityEditor.AssetDatabase.SaveAssets();

        Debug.Log($"Mesh saved at {path}");
    }
    #endif
}
