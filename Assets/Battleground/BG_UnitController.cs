using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BG_UnitController : BG_EntityController
{
    public List<BG_UnitBehavior> unitBehaviors = new List<BG_UnitBehavior>();
    public NavMeshAgent agent;
    public Transform goal;

    public override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        BG_EntityManager.Instance.Register(this);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        foreach (var behavior in unitBehaviors)
        {
            HandleBehaviors(behavior);
        }
    }

    private void HandleBehaviors(BG_UnitBehavior behavior)
    {
        switch (behavior)
        {
            case BG_UnitBehaviorMoveTowardsGoal b:
                HandleBehaviorMoveTowardGoal();
                break;
            case BG_UnitBehaviorAggressive b:
                HandleBehaviorAggressive();
                break;

        }
    }

    private void HandleBehaviorMoveTowardGoal()
    {

    }

    public void SetGoal(Transform transform)
    {
        goal = transform;
        agent.destination = goal.position;
    }

    public void ClearGoal()
    {
        goal = null;
        agent.ResetPath();
    }

    private void HandleBehaviorAggressive()
    {

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
