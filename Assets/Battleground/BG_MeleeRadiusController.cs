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
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public bool IsWithinMeleeRange(Transform target)
    {
        var distance = Vector3.Distance(transform.root.position, target.position);
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
