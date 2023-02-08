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

    #endregion

    #region Custom Conditions

    public static BehaviorTreeBuilder CustomCondition(this BehaviorTreeBuilder builder, string name = "My Condition")
    {
        return builder.AddNode(new CustomCondition
        {
            Name = name,
        });
    }

    public static BehaviorTreeBuilder CheckForObjectives(this BehaviorTreeBuilder builder,
        string name = "Check For Objectives")
    {
        return builder.AddNode(new CheckForObjectives
        {
            Name = name,
            
        });
    }

    public static BehaviorTreeBuilder CheckEntitiesInRange(this BehaviorTreeBuilder builder,
        string name = "Check Entities In Range")
    {
        return builder.AddNode(new CheckEntitiesInRange()
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

public class CheckForObjectives : ConditionBase
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
        if (Owner.TryGetComponent<BG_UnitController>(out var unit) && unit.goal != null)
        {
            return true;
        }
        return false;
    }

    // Triggers whenever this node exits after running
    protected override void OnExit()
    {
    }
}

public class CheckEntitiesInRange : ConditionBase
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
        if (Owner.TryGetComponent<BG_UnitController>(out var unit))
        {
            if (unit.actionRadiusController.storedTargets.Count > 0)
            {
                return true;
            }
            else
            {
                unit.target = null;
            }
        }
        return false;
    }

    // Triggers whenever this node exits after running
    protected override void OnExit()
    {
    }
}

public class AttackValidEntitiesInRange : ActionBase
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
        if (Owner.TryGetComponent<BG_UnitController>(out var unit))
        {
            unit.AttackTarget();
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    // Triggers whenever this node exits after running
    protected override void OnExit()
    {
    }
}

public class NavigateToObjective : ActionBase
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
        if (Owner.TryGetComponent<BG_UnitController>(out var unit))
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    // Triggers whenever this node exits after running
    protected override void OnExit()
    {
    }
}