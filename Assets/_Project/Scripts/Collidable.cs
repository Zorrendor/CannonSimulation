using System;
using UnityEngine;
using UnityEngine.Events;

public class Collidable : MonoBehaviour
{
    public UnityEvent<BoxCollider> onCollide;

    public bool isKinematic = false;
    
    public BoxCollider BoxCollider { get; private set;}
    
    private float lastCollisionTime;
    
    private const float CollisionCooldown = 0.1f;
    
    private void Awake()
    {
        BoxCollider = this.GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        lastCollisionTime = 0;
        CollisionManager.Instance.RegisterCollidable(this);
    }

    private void OnDisable()
    {
        CollisionManager.Instance.UnregisterCollidable(this);
    }

    public void OnCollide(BoxCollider collider)
    {
        if (Time.time - lastCollisionTime < CollisionCooldown) return;
        
        lastCollisionTime = Time.time;
        onCollide?.Invoke(collider);
    }


}