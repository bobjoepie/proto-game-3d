using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_RangedRadiusController : MonoBehaviour
{
    public SphereCollider rangedRadius;

    private void OnEnable()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            entity.Register(this);
        }
    }

    private void Awake()
    {
        rangedRadius = GetComponent<SphereCollider>();
    }

    public bool IsWithinRangedRange(Vector3 target)
    {
        var distance = Vector3.Distance(transform.root.position, target);
        if (distance > rangedRadius.radius)
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
