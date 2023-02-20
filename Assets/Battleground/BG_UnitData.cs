using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "BG/Unit Data")]
public class BG_UnitData : BG_EntityData
{
    public List<BG_UnitBehavior> unitBehaviors = new List<BG_UnitBehavior>();

    public override void ConvertData(BG_EntityController entityController)
    {
        BG_UnitController unitController = (BG_UnitController)entityController;
        base.ConvertData(unitController);

        unitController.lastAttackedTime = 0f;
        if (attributes.meleeAttacksPerSecond > 0)
        {
            unitController.attackCooldown = 1f / attributes.meleeAttacksPerSecond;
        }
        else if (attributes.rangedAttacksPerSecond > 0)
        {
            unitController.attackCooldown = 1f / attributes.rangedAttacksPerSecond;
        }
        else
        {
            unitController.lastAttackedTime = Mathf.Infinity;
        }

        unitController.unitBehaviors = this.unitBehaviors;
    }
}
