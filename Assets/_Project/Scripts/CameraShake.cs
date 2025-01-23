using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CannonConfigSO config;
    [SerializeField] private float power = 0.1f;

    private void OnEnable()
    {
        config.OnFire += OnFire;
    }

    private void OnDisable()
    {
        config.OnFire -= OnFire;
    }
    
    private void OnFire(Vector3 position, Vector3 velocity)
    {
        this.GetComponent<CinemachineImpulseSource>().GenerateImpulseAtPositionWithVelocity(position, velocity.normalized * power);
    }

    [ContextMenu("Test Shake")]
    private void TestShake()
    {
        this.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(1.0f);
    }
}
