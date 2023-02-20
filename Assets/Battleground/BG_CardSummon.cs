using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "CardSummon", menuName = "BG/Card Behavior/Card Summon"), Serializable]
public class BG_CardSummon : BG_CardBehavior
{
    [Header("Properties")]
    public BG_UnitData unitData;

    public void Summon(Vector3 summonPos, BG_EntityTags tags, string collisionLayer, string actionRadiusLayer)
    {
        var entity = unitData.entityGameObject;
        var isValidNavMeshPos = NavMesh.SamplePosition(summonPos, out var hit, Mathf.Infinity, NavMesh.AllAreas);
        var instance = Instantiate(entity, hit.position, Quaternion.identity);
        unitData.ConvertData(instance);
        instance.attributes.tags |= tags;
        instance.collisionLayer = LayerMask.GetMask(collisionLayer);
        instance.actionRadiusLayer = LayerMask.GetMask(actionRadiusLayer);
    }
}
