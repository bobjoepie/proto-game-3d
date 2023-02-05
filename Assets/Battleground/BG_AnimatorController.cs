using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BG_AnimatorController : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(string state)
    {
        animator.CrossFade(state, 0.05f);
    }

    private void OnEnable()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            entity.Register(this);
        }
    }

    private void OnDisable()
    {
        if (transform.root.TryGetComponent<EntityController>(out var entity))
        {
            entity.Unregister(this);
        }
    }
}
