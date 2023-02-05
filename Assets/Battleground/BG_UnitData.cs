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
        
        unitController.unitBehaviors = this.unitBehaviors;
    }
}
