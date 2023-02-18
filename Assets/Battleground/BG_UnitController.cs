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
    #region Properties
    
    public List<BG_UnitBehavior> unitBehaviors = new List<BG_UnitBehavior>();
    public NavMeshAgent agent;
    public Transform goal;
    public Transform target;
    public float behaviorFrequency = 1f;

    private CancellationTokenSource cancellationToken;

    #endregion

    #region System Methods

    private void OnEnable()
    {
        BG_EntityManager.Instance.Register(this);
    }

    private void Awake()
    {
        behaviorTree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Selector("Handle Attacks")
                    .Sequence("Handle Melee Attacks")
                        .IsMelee()
                        .HasCurrentTarget()
                        .IsCurrentTargetInActionRadius()
                        .IsCurrentTargetInMeleeRange()
                        .AttackCurrentTarget()
                    .End()
                    .Sequence("Move To Melee Range")
                        .IsMelee()
                        .HasCurrentTarget()
                        .Inverter().IsCurrentTargetInMeleeRange().End()
                        .NavigateToTarget()
                    .End()
                    .Sequence("Handle Ranged Attacks")
                        .IsRanged()
                        .HasCurrentTarget()
                        .IsCurrentTargetInActionRadius()
                        .IsCurrentTargetInRangedRange()
                        .StopMoving()
                        .AttackCurrentTarget()
                    .End()
                    .Sequence("Move To Ranged Range")
                        .IsRanged()
                        .HasCurrentTarget()
                        .Inverter().IsCurrentTargetInRangedRange().End()
                        .NavigateToTarget()
                    .End()
                    .Sequence()
                        .HasEntitiesInRange()
                        .SetValidTarget()
                        .AttackValidEntitiesInRange()
                    .End()
                .End()

                .Selector("Handle Objectives")
                    .Sequence("Handle Objective Tasks")
                        .IsAtObjective()
                        .PerformObjectiveTask()
                    .End()

                    .Sequence("Move To Objectives")
                        .HasValidObjectives()
                        .Inverter().IsAtObjective().End()
                        .NavigateToObjective()
                    .End()

                    //.Sequence("Stop Moving To Objectives")
                    //    .IsMovingToObjective()
                    //    .StopMovingToObjective()
                    //.End()
                .End()

                .Sequence("Handle Idle")
                    .StopMoving()
                    .BecomeIdle() 
                .End()
            .End()
            .Build();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        StartBehaviorLoop();
    }

    private async UniTask BehaviorLoop(CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.NextFrame(token);
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
        CancelBehaviorLoop();
        BG_EntityManager.Instance.Unregister(this);
    }

    #endregion
}
