using UnityEngine;

[CreateAssetMenu(fileName = "CannonConfigSO", menuName = "Scriptable Objects/CannonConfigSO")]
public class CannonConfigSO : ScriptableObject
{
    [Range(0f, 2f)]
    public float powerMultiplier = 1.0f;

    public float bulletEnergyLossFactor = 0.4f;
    
    [System.NonSerialized]
    public readonly Observer<float> power = new Observer<float>(50);

    [System.NonSerialized]
    public Vector3 currentVelocity;
    
    public event System.Action<Vector3, Vector3> OnFire;
    public void TriggerFire(Vector3 position, Vector3 velocity) => OnFire?.Invoke(position, velocity);
}
