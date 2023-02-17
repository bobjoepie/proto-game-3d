using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BG_EntityManager : MonoBehaviour
{
    public static BG_EntityManager Instance { get; private set; }

    public List<BG_EntityController> entities = new List<BG_EntityController>();
    public List<BG_UnitController> units = new List<BG_UnitController>();
    public List<BG_BuildingController> buildings = new List<BG_BuildingController>();

    private UIDocManager uiDocManager;

    private BG_EntityManager()
    {
        Instance = this;
    }

    private void Awake()
    {
        if (UIDocManager.Instance != null)
        {
            uiDocManager = UIDocManager.Instance;
        }
    }

    public void Register(BG_EntityController entity)
    {
        switch (entity)
        {
            case BG_UnitController u:
                units.Add(u);
                break;
            case BG_BuildingController b:
                buildings.Add(b);
                break;
            case BG_EntityController e:
                entities.Add(e);
                break;
        }
        
        //var tags = entity.attributes.tags;
        //if (tags.HasTag(BG_EntityTags.Objective))
        //{
        //    NotifySubscribeObjective(entity);
        //}
        //if (tags.HasTag(BG_EntityTags.ObjectiveSeeker))
        //{
        //    SubscribeObjective(entity);
        //}
    }

    public void Unregister(BG_EntityController entity)
    {
        switch (entity)
        {
            case BG_UnitController u:
                units.Remove(u);
                break;
            case BG_BuildingController b:
                buildings.Remove(b);
                break;
            case BG_EntityController e:
                entities.Remove(e);
                break;
        }

        //var tags = entity.attributes.tags;
        //if (tags.HasTag(BG_EntityTags.Objective))
        //{
        //    NotifyUnsubscribeObjective(entity);
        //}
    }

    private void SubscribeObjective(BG_EntityController entity)
    {
        //IEnumerable<BG_EntityController> unitObjectives = units.Where(u =>
        //    u.HasTag(BG_EntityTags.Objective) &&
        //    BG_EntityTags.Faction.IsOpposingTag(u, entity));

        //IEnumerable<BG_EntityController> buildingObjectives = buildings.Where(b => 
        //    b.HasTag(BG_EntityTags.Objective) &&
        //    BG_EntityTags.Faction.IsOpposingTag(b, entity));

        //var objectives = unitObjectives.Concat(buildingObjectives).ToList();

        //foreach (var objective in objectives)
        //{
        //    ((BG_UnitController)entity).SetGoal(objective.transform);
        //}
    }

    private void NotifySubscribeObjective(BG_EntityController objective)
    {
        //var objectiveSeekers = units.Where(u =>
        //    u.HasTag(BG_EntityTags.ObjectiveSeeker) &&
        //    BG_EntityTags.Faction.IsOpposingTag(u, objective));

        //foreach (var objectiveSeeker in objectiveSeekers)
        //{
        //    objectiveSeeker.SetGoal(objective.transform);
        //}
    }

    private void NotifyUnsubscribeObjective(BG_EntityController objective)
    {
        //var objectiveSeekers = units.Where(u => 
        //    u.HasTag(BG_EntityTags.ObjectiveSeeker) && 
        //    BG_EntityTags.Faction.IsOpposingTag(u, objective) &&
        //    u.goal == objective.transform);

        //foreach (var objectiveSeeker in objectiveSeekers)
        //{
        //    objectiveSeeker.goal = null;
        //}
    }

    public int GetValidOpposingObjectivesCount(BG_EntityController entity)
    {
        IEnumerable<BG_EntityController> unitObjectives = units.Where(u =>
            u.HasTag(BG_EntityTags.Objective) &&
            BG_EntityTags.Faction.IsOpposingTag(u, entity));

        IEnumerable<BG_EntityController> buildingObjectives = buildings.Where(b =>
            b.HasTag(BG_EntityTags.Objective) &&
            BG_EntityTags.Faction.IsOpposingTag(b, entity));

        var objectives = unitObjectives.Concat(buildingObjectives);

        return objectives.Count();
    }

    public Transform GetClosestOpposingObjective(BG_EntityController entity)
    {
        IEnumerable<BG_EntityController> unitObjectives = units.Where(u =>
            u.HasTag(BG_EntityTags.Objective) &&
            BG_EntityTags.Faction.IsOpposingTag(u, entity));

        IEnumerable<BG_EntityController> buildingObjectives = buildings.Where(b =>
            b.HasTag(BG_EntityTags.Objective) &&
            BG_EntityTags.Faction.IsOpposingTag(b, entity));

        var objectives = unitObjectives.Concat(buildingObjectives).ToList();

        return objectives.FirstOrDefault()?.transform;
    }

    public Transform GetClosestOwnObjective(BG_EntityController entity)
    {
        IEnumerable<BG_EntityController> unitObjectives = units.Where(u =>
            u.HasTag(BG_EntityTags.Objective) &&
            BG_EntityTags.Faction.IsSameTag(u, entity));

        IEnumerable<BG_EntityController> buildingObjectives = buildings.Where(b =>
            b.HasTag(BG_EntityTags.Objective) &&
            BG_EntityTags.Faction.IsSameTag(b, entity));

        var objectives = unitObjectives.Concat(buildingObjectives).ToList();

        return objectives.FirstOrDefault()?.transform;
    }

    public List<BG_EntityController> GetAllOwnObjectives(BG_EntityTags faction)
    {
        IEnumerable<BG_EntityController> unitObjectives = units.Where(u =>
            u.HasTag(BG_EntityTags.Objective) &&
            BG_EntityTags.Faction.IsSameTag(u.attributes.tags, faction));

        IEnumerable<BG_EntityController> buildingObjectives = buildings.Where(b =>
            b.HasTag(BG_EntityTags.Objective) &&
            BG_EntityTags.Faction.IsSameTag(b.attributes.tags, faction));

        var objectives = unitObjectives.Concat(buildingObjectives).ToList();

        return objectives;
    }
}
