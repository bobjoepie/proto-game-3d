using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_MeleeRadiusController : MonoBehaviour
{
    public SphereCollider meleeRadius;

    private void OnEnable()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            entity.Register(this);
        }
    }

    private void Awake()
    {
        meleeRadius = GetComponent<SphereCollider>();
    }

    public bool IsWithinMeleeRange(Vector3 target)
    {
        var distance = Vector3.Distance(transform.root.position, target);
        if (distance > meleeRadius.radius)
        {
            return false;
        }
        return true;
    }

    private void OnDisable()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            entity.Unregister(this);
        }
    }
}
