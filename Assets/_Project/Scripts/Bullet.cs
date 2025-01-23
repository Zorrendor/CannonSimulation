using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float destructionTime = 10f;

    public bool ShouldSpawnParticles => ricochetCount == 2;
    
    private Vector3 velocity;
    private int ricochetCount = 0;
    private float timer = 0f;
    private float energyLossFactor = 0.4f;
    
    Transform cachedTransform;

    private System.Action<Bullet> onDestroyBullet;
    
    const float Gravity = 9.81f;

    public void Init(Vector3 position, Vector3 initialVelocity, float energyLossFactor, System.Action<Bullet> onDestroyBullet = null)
    {
        this.energyLossFactor = energyLossFactor;
        this.transform.position = position;
        velocity = initialVelocity;
        timer = 0;
        ricochetCount = 0;
        this.onDestroyBullet = onDestroyBullet;
    }

    private void Update()
    {
        velocity += Vector3.down * (Gravity * Time.deltaTime);

        transform.position += velocity * Time.deltaTime;
        
        timer += Time.deltaTime;
        if (timer > destructionTime)
        {
            onDestroyBullet?.Invoke(this);
        }
    }

    public void OnCollide(BoxCollider collider)
    {
        ricochetCount++;
        if (ricochetCount >= 2)
        {
            onDestroyBullet?.Invoke(this);
            return;
        }
        
        Vector3 closestPoint = collider.ClosestPoint(transform.position);
        Vector3 normal = (transform.position - closestPoint).normalized;
        
        velocity = Vector3.Reflect(velocity, normal) * energyLossFactor;
    }
}
