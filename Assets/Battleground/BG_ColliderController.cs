using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_ColliderController : MonoBehaviour
{
    private void OnEnable()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            entity.Register(this);
        }
    }

    private void Start()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            gameObject.layer = entity.collisionLayer.ToLayer();
        }
    }
}
