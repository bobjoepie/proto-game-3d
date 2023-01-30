using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "BG/Building Data")]
public class BG_BuildingData : BG_EntityData
{
    public List<BG_EntityData> constructableEntities = new List<BG_EntityData>();

    public override void ConvertData(BG_EntityController entityController)
    {
        BG_BuildingController buildingController = (BG_BuildingController)entityController;
        base.ConvertData(buildingController);

        buildingController.constructableEntities = this.constructableEntities;
    }
}
