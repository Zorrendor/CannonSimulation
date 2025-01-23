using System;
using UnityEngine;
using UnityEngine.Events;

public class Collidable : MonoBehaviour
{
    public UnityEvent<BoxCollider> onCollide;

    public bool isKinematic = false;
    
    public BoxCollider BoxCollider { get; private set;}
    
    private float lastCollisionTime;
    private float collisionCooldown = 0.1f;

    private int frameCounter = 0;
    
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
    
    void Update()
    {
        frameCounter++;
    }

    public void OnCollide(BoxCollider collider)
    {
        if (Time.time - lastCollisionTime < collisionCooldown) return;
        
        lastCollisionTime = Time.time;
        onCollide?.Invoke(collider);
        frameCounter = 0;
    }


}