using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BG_EntityManager : MonoBehaviour
{
    public static BG_EntityManager Instance { get; private set; }

    public List<BG_EntityController> entities = new List<BG_EntityController>();
    public List<BG_UnitController> units = new List<BG_UnitController>();
    public List<BG_BuildingController> buildings = new List<BG_BuildingController>();

    private UIDocManager uiDocManager;
    private CancellationTokenSource cancellationToken;
    public float behaviorFrequency = 1f;

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

    private void Start()
    {
        StartBehaviorLoop();
    }

    public void StartBehaviorLoop()
    {
        if (cancellationToken != null)
        {
            CancelBehaviorLoop();
        }
        cancellationToken = new CancellationTokenSource();
        PerformAllUnitBehaviorsLoop(cancellationToken.Token).Forget();
    }

    private async UniTask PerformAllUnitBehaviorsLoop(CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.NextFrame(token);
            foreach (var unit in units.ToList())
            {
                unit.behaviorTree.Tick();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(behaviorFrequency), cancellationToken: token);
        }
    }

    public void CancelBehaviorLoop()
    {
        cancellationToken.Cancel();
        cancellationToken.Dispose();
        cancellationToken = null;
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
