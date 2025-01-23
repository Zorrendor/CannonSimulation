using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance { get; private set; }
    
    private readonly List<Collidable> kinematicCollidables = new List<Collidable>();
    private readonly List<Collidable> collidables = new List<Collidable>();
    
    private readonly Collider[] hitColliders = new Collider[8];

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterCollidable(Collidable collidable)
    {
        if (collidable.isKinematic)
        {
            if (!kinematicCollidables.Contains(collidable)) 
                kinematicCollidables.Add(collidable);
            
        }
        else if (!collidables.Contains(collidable))
            collidables.Add(collidable);
    }

    public void UnregisterCollidable(Collidable collidable)
    {
        if (collidable.isKinematic)
        {
            kinematicCollidables.Remove(collidable);
        }
        else
        {
            collidables.Remove(collidable);
        }
    }

    private void LateUpdate()
    {
        this.CheckCollisions();
    }

    private void CheckCollisions()
    {
        foreach (var collidable in collidables)
        {
            var size = Physics.OverlapBoxNonAlloc(collidable.transform.position, collidable.BoxCollider.bounds.extents, hitColliders, collidable.transform.rotation);
            for (int i = 0; i < size; i++)
            {
                Collidable collidable2 = hitColliders[i].GetComponent<Collidable>();
                if (!collidable2 || !collidable2.isKinematic) continue;
                
                collidable2.OnCollide(collidable.BoxCollider);
                collidable.OnCollide(collidable2.BoxCollider);
            }
        }
    }
}