using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance => instance;
    private static CollisionManager instance;

    private List<AABBCollider> colliders = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Register(AABBCollider collider)
    {
        if (colliders.Contains(collider)) return;
        colliders.Add(collider);
    }

    public void Unregister(AABBCollider collider)
    {
        colliders.Remove(collider);
    }

    private void LateUpdate()
    {
        foreach (var collider in colliders)
        {
            if (collider != null)
                collider.UpdateBounds();
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            var colliderA = colliders[i];

            if (colliderA == null) continue;

            for (int j = i + 1; j < colliders.Count; j++)
            {
                var colliderB = colliders[j];

                if (colliderB == null) continue;

                if (Overlaps(colliderA, colliderB))
                {
                    colliderA.OnCollision(colliderB);
                    colliderB.OnCollision(colliderA);
                }
            }
        }
    }

    bool Overlaps(AABBCollider a, AABBCollider b)
    {
        return a.min.x <= b.max.x && b.min.x <= a.max.x &&
               a.min.y <= b.max.y && b.min.y <= a.max.y;
    }

}