using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BG_BuildingController : BG_EntityController
{
    public List<BG_EntityData> constructableEntities = new List<BG_EntityData>();

    public List<Action> LoadButtons()
    {
        List<Action> actions = new List<Action>();
        foreach (var entity in constructableEntities.Where(e => e != null))
        {
            actions.Add(delegate
            {
                Construct(entity); 
            });
        }

        return actions;
    }

    public void Construct(BG_EntityData entity)
    {
        Debug.Log($"Constructing {entity.name}...");
    }
}
