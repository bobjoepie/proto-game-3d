using System;
using System.Collections;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;

public class BG_UnitController : BG_EntityController
{
    public List<BG_UnitBehavior> unitBehaviors = new List<BG_UnitBehavior>();
    public NavMeshAgent agent;
    public Transform goal;
    public Transform target;

    public float attackCooldown;
    public float lastAttackedTime;

    private void OnEnable()
    {
        behaviorTree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .UnitHandleAttacks(gameObject)
                .UnitHandleTargeting(gameObject)
                .UnitHandleObjectives(gameObject)
                .UnitHandleIdle(gameObject)
            .End()
            .Build();

        BG_EntityManager.Instance.Register(this);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override BG_EntityController Select()
    {
        //meshRenderer.material = selectedMaterial;
        animator.ChangeAnimationState("Walk");
        return this;
    }

    public override BG_EntityController Deselect()
    {
        //meshRenderer.material = baseMaterial;
        animator.ChangeAnimationState("Idle");
        return null;
    }

    private void OnDisable()
    {
        BG_EntityManager.Instance.Unregister(this);
    }
}
