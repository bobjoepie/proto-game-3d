using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_ColliderController : MonoBehaviour
{
    public BG_EntityController entityController;

    private void Awake()
    {
        entityController = transform.parent.GetComponent<BG_EntityController>();
        gameObject.layer = entityController.collisionLayer.ToLayer();
    }

    private void OnEnable()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            entity.Register(this);
        }
    }
}
