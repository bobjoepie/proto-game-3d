using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BG_BuildingController : BG_EntityController
{
    public List<BG_EntityData> constructableEntities = new List<BG_EntityData>();

    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        BG_EntityManager.Instance.Register(this);
    }

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

    public override BG_EntityController Select()
    {
        base.Select();
        Destroy(this.gameObject);
        return null;
    }

    private void OnDisable()
    {
        BG_EntityManager.Instance.Unregister(this);
    }
}
