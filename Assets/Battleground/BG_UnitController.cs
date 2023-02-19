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
    public float behaviorFrequency = 1f;

    private CancellationTokenSource cancellationToken;

    private void OnEnable()
    {
        BG_EntityManager.Instance.Register(this);
    }

    private void Awake()
    {
        behaviorTree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .UnitHandleAttacks(gameObject)
                .UnitHandleTargeting(gameObject)
                .UnitHandleObjectives(gameObject)
                .UnitHandleIdle(gameObject)
            .End()
            .Build();

        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        //StartBehaviorLoop();
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
        //CancelBehaviorLoop();
        BG_EntityManager.Instance.Unregister(this);
    }
}
