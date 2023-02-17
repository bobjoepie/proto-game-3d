using System.Collections;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.TaskParents.Composites;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;

public static class BG_BehaviorRepository
{
    #region Custom Actions

    public static BehaviorTreeBuilder CustomAction(this BehaviorTreeBuilder builder, string name = "My Action")
    {
        return builder.AddNode(new CustomAction
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder AgentDestination(this BehaviorTreeBuilder builder, string name, Transform target)
    {
        return builder.AddNode(new AgentDestination
        {
            Name = name,
            target = target,
        });
    }

    public static BehaviorTreeBuilder SetValidTarget(this BehaviorTreeBuilder builder,
        string name = "Set Valid Target")
    {
        return builder.AddNode(new SetValidTarget
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder AttackValidEntitiesInRange(this BehaviorTreeBuilder builder,
        string name = "Attack Valid Entities In Range")
    {
        return builder.AddNode(new AttackValidEntitiesInRange
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder NavigateToObjective(this BehaviorTreeBuilder builder,
        string name = "Navigate To Objective")
    {
        return builder.AddNode(new NavigateToObjective
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder StopMovingToObjective(this BehaviorTreeBuilder builder,
        string name = "Stop Moving To Objective")
    {
        return builder.AddNode(new StopMovingToObjective
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder PerformObjectiveTask(this BehaviorTreeBuilder builder,
        string name = "Perform Objective Task")
    {
        return builder.AddNode(new PerformObjectiveTask
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder AttackCurrentTarget(this BehaviorTreeBuilder builder,
        string name = "Attack Current Target")
    {
        return builder.AddNode(new AttackCurrentTarget
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder BecomeIdle(this BehaviorTreeBuilder builder,
        string name = "BecomeIdle")
    {
        return builder.AddNode(new BecomeIdle
        {
            Name = name,
        });
    }

    #endregion

    #region Custom Conditions

    public static BehaviorTreeBuilder CustomCondition(this BehaviorTreeBuilder builder, string name = "My Condition")
    {
        return builder.AddNode(new CustomCondition
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder HasValidObjectives(this BehaviorTreeBuilder builder,
        string name = "Has Valid Objectives")
    {
        return builder.AddNode(new HasValidObjectives
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder HasEntitiesInRange(this BehaviorTreeBuilder builder,
        string name = "Has Entities In Range")
    {
        return builder.AddNode(new HasEntitiesInRange()
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder HasCurrentTarget(this BehaviorTreeBuilder builder,
        string name = "Has Current Target")
    {
        return builder.AddNode(new HasCurrentTarget()
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder IsAtObjective(this BehaviorTreeBuilder builder,
        string name = "Is At Objective")
    {
        return builder.AddNode(new IsAtObjective()
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder IsMovingToObjective(this BehaviorTreeBuilder builder,
        string name = "Is Moving To Objective")
    {
        return builder.AddNode(new IsMovingToObjective()
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder IsCurrentTargetInRange(this BehaviorTreeBuilder builder,
        string name = "Is Current Target In Range")
    {
        return builder.AddNode(new IsCurrentTargetInRange()
        {
            Name = name,
        });
    }

    #endregion

    #region Custom Sequences

    public static BehaviorTreeBuilder CustomSequence(this BehaviorTreeBuilder builder, string name = "My Sequence")
    {
        return builder.ParentTask<CustomSequence>(name);
    }

    #endregion

    #region Custom Decorators

    public static BehaviorTreeBuilder CustomInverter(this BehaviorTreeBuilder builder, string name = "My Inverter")
    {
        // See BehaviorTreeBuilder.AddNodeWithPointer() if you need to set custom composite data from arguments
        return builder.ParentTask<CustomInverter>(name);
    }

    #endregion
}

public class AgentDestination : ActionBase
{
    private NavMeshAgent _agent;
    public Transform target;

    protected override void OnInit()
    {
        _agent = Owner.GetComponent<NavMeshAgent>();
    }

    protected override TaskStatus OnUpdate()
    {
        _agent.SetDestination(target.position);
        return TaskStatus.Success;
    }
}

public class CustomAction : ActionBase
{
    // Triggers only the first time this node is run (great for caching data)
    protected override void OnInit()
    {
    }

    // Triggers every time this node starts running. Does not trigger if TaskStatus.Continue was last returned by this node
    protected override void OnStart()
    {
    }

    // Triggers every time `Tick()` is called on the tree and this node is run
    protected override TaskStatus OnUpdate()
    {
        // Points to the GameObject of whoever owns the behavior tree
        Debug.Log(Owner.name);
        return TaskStatus.Success;
    }

    // Triggers whenever this node exits after running
    protected override void OnExit()
    {
    }
}

public class CustomCondition : ConditionBase
{
    // Triggers only the first time this node is run (great for caching data)
    protected override void OnInit()
    {
    }

    // Triggers every time this node starts running. Does not trigger if TaskStatus.Continue was last returned by this node
    protected override void OnStart()
    {
    }

    // Triggers every time `Tick()` is called on the tree and this node is run
    protected override bool OnUpdate()
    {
        // Points to the GameObject of whoever owns the behavior tree
        Debug.Log(Owner.name);
        return true;
    }

    // Triggers whenever this node exits after running
    protected override void OnExit()
    {
    }
}

public class CustomSequence : CompositeBase
{
    protected override TaskStatus OnUpdate()
    {
        for (var i = ChildIndex; i < Children.Count; i++)
        {
            var child = Children[ChildIndex];

            var status = child.Update();
            if (status != TaskStatus.Success)
            {
                return status;
            }

            ChildIndex++;
        }

        return TaskStatus.Success;
    }
}

public class CustomInverter : DecoratorBase
{
    protected override TaskStatus OnUpdate()
    {
        if (Child == null)
        {
            return TaskStatus.Success;
        }

        var childStatus = Child.Update();
        var status = childStatus;

        switch (childStatus)
        {
            case TaskStatus.Success:
                status = TaskStatus.Failure;
                break;
            case TaskStatus.Failure:
                status = TaskStatus.Success;
                break;
        }

        return status;
    }
}

public class HasValidObjectives : ConditionBase
{
    BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }
    
    protected override bool OnUpdate()
    {
        if (BG_EntityManager.Instance.GetValidOpposingObjectivesCount(unit) > 0)
        {
            return true;
        }
        return false;
    }
}

public class HasEntitiesInRange : ConditionBase
{
    BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }
    
    protected override bool OnUpdate()
    {
        if (unit.actionRadiusController.storedTargets.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class IsAtObjective : ConditionBase
{
    BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }
    
    protected override bool OnUpdate()
    {
        var tempGoal = BG_EntityManager.Instance.GetClosestOpposingObjective(unit);
        if (tempGoal != null && Vector3.Distance(unit.transform.position, tempGoal.position) <= unit.agent.stoppingDistance + 1.25f)
        {
            return true;
        }
        return false;
    }
}

public class IsMovingToObjective : ConditionBase
{
    BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }

    protected override bool OnUpdate()
    {
        if (unit.agent.hasPath)
        {
            return true;
        }
        return false;
    }
}

public class HasCurrentTarget : ConditionBase
{
    BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }

    protected override bool OnUpdate()
    {
        return unit.target != null;
    }
}

public class IsCurrentTargetInRange : ConditionBase
{
    BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }

    protected override bool OnUpdate()
    {
        if (unit.target != null && unit.actionRadiusController.storedTargets.Contains(unit.target))
        {
            return true;
        }
        else
        {
            unit.target = null;
            return false;
        }
    }
}

public class SetValidTarget : ActionBase
{
    BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }

    protected override TaskStatus OnUpdate()
    {
        unit.target = unit.actionRadiusController.GetClosestTarget();
        return TaskStatus.Success;
    }
}

public class AttackValidEntitiesInRange : ActionBase
{
    BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }

    protected override TaskStatus OnUpdate()
    {
        //unit.PerformAttack();
        return TaskStatus.Success;
    }
}

public class NavigateToObjective : ActionBase
{
    BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }

    protected override TaskStatus OnUpdate()
    {
        unit.goal = BG_EntityManager.Instance.GetClosestOpposingObjective(unit);
        unit.agent.SetDestination(unit.goal.position);
        unit.animator.ChangeAnimationState("Walk");
        return TaskStatus.Success;
    }

}

public class StopMovingToObjective : ActionBase
{
    BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }

    protected override TaskStatus OnUpdate()
    {
        unit.agent.isStopped = true;
        unit.agent.ResetPath();
        unit.animator.ChangeAnimationState("Idle");
        return TaskStatus.Success;
    }
}

public class PerformObjectiveTask : ActionBase
{
    private BG_UnitController unit;
    private BG_EntityController objective;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
        objective = unit.goal.GetComponent<BG_EntityController>();
    }

    protected override TaskStatus OnUpdate()
    {
        var tags = objective.attributes.tags;

        if (tags.HasTag(BG_EntityTags.ObjectiveDestroy))
        {
            unit.target = objective.transform;
            return TaskStatus.Success;
        }
        else if (tags.HasTag(BG_EntityTags.ObjectiveCapture))
        {
            return TaskStatus.Success;
        }
        else if (tags.HasTag(BG_EntityTags.ObjectiveHold))
        {
            return TaskStatus.Success;
        }
        else if (tags.HasTag(BG_EntityTags.ObjectiveTouch))
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}

public class AttackCurrentTarget : ActionBase
{
    private BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }

    protected override TaskStatus OnUpdate()
    {
        Debug.Log($"{unit.name} attacked {unit.target.name}");
        if (unit.target.TryGetComponent<BG_UnitController>(out var targetUnit))
        {
            targetUnit.TakeDamage(1);
            unit.animator.PlayAnimationStateOneShot("Attack");
            return TaskStatus.Success;
        }
        else if (unit.target.TryGetComponent<BG_BuildingController>(out var targetBuilding))
        {
            targetBuilding.TakeDamage(1);
            unit.animator.PlayAnimationStateOneShot("Attack");
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}

public class BecomeIdle : ActionBase
{
    private BG_UnitController unit;
    protected override void OnInit()
    {
        unit = Owner.GetComponent<BG_UnitController>();
    }

    protected override TaskStatus OnUpdate()
    {
        unit.animator.ChangeAnimationState("Idle");
        return TaskStatus.Success;
    }
}