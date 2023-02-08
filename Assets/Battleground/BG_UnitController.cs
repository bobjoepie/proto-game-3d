using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class BG_UnitController : BG_EntityController
{
    public List<BG_UnitBehavior> unitBehaviors = new List<BG_UnitBehavior>();
    public NavMeshAgent agent;
    public Transform goal;
    public Transform target;
    public int behaviorFrequency = 1;

    private CancellationTokenSource cancellationToken;

    public override void Start()
    {
        base.Start();
        StartBehaviorLoop();
    }

    private void OnEnable()
    {
        BG_EntityManager.Instance.Register(this);
    }

    private void Awake()
    {
        behaviorTree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .CheckEntitiesInRange()
                    .AttackValidEntitiesInRange()
                .End()
                .Sequence()
                    .CheckForObjectives()
                    .NavigateToObjective()
                .End()
            .End()
            .Build();
        agent = GetComponent<NavMeshAgent>();
    }

    public void AttackTarget()
    {
        target = actionRadiusController.GetClosestTarget();
    }

    private async UniTask BehaviorLoop(CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            behaviorTree.Tick();
            await UniTask.Delay(TimeSpan.FromSeconds(behaviorFrequency), cancellationToken: token);
        }
    }

    public void StartBehaviorLoop()
    {
        if (cancellationToken != null)
        {
            CancelBehaviorLoop();
        }
        cancellationToken = new CancellationTokenSource();
        BehaviorLoop(cancellationToken.Token).Forget();
    }

    public void CancelBehaviorLoop()
    {
        cancellationToken.Cancel();
        cancellationToken.Dispose();
        cancellationToken = null;
    }

    private void HandleBehaviorMoveTowardGoal()
    {
        if (!agent.hasPath && goal != null)
        {
            agent.SetDestination(goal.position);
        }

        if ((!agent.hasPath && !agent.pathPending && goal != null) ||
            (goal != null && Vector3.Distance(transform.position, goal.position) <= agent.stoppingDistance + 1.25f))
        {
            goal = null;
            agent.isStopped = true;
            agent.ResetPath();
            Debug.Log("complete");
        }
    }

    public void SetGoal(Transform transform = null)
    {
        goal = transform;
        if (goal != null)
        {
            agent.SetDestination(goal.position);
        }
    }

    public void ClearGoal()
    {
        goal = null;
    }

    private void HandleBehaviorAggressive()
    {
        var target = actionRadiusController.GetClosestTarget();
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
