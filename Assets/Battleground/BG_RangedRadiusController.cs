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
        rangedRadius.isTrigger = true;
    }

    private void Start()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            rangedRadius.radius = entity.attributes.rangedRange;
        }
    }

    public bool IsWithinRangedRange(Transform target)
    {
        var distance = Vector3.Distance(transform.root.position, target.position);
        if (distance > rangedRadius.radius)
        {
            return false;
        }
        return true;
    }

    public bool IsWithinLineOfSight(Transform target)
    {
        var hasObstacleInLineOfSight = Physics.Linecast(transform.position, target.position, out var hits, LayerUtility.Only("Default"));
        return !hasObstacleInLineOfSight;
    }

    private void OnDisable()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            entity.Unregister(this);
        }
    }
}
