using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponController : MonoBehaviour
{
    public bool CanAttack = true;
    public EntityController entityController;
    [Range(0f, 2f)] public float timeBetweenAttacks = 0.2f;
    [Range(0f, 1f)] public float volleyDelay = 0f;

    private void Start()
    {
        entityController = transform.root.GetComponent<EntityController>();
    }

    private void Update()
    {
        HandleAttacks();
    }

    private void HandleAttacks()
    {
        if (CanAttack && entityController.actionRadiusController.storedTargets.Any())
        {
            CanAttack = false;
            TryAttack(entityController.actionRadiusController, entityController.appendages).Forget();
        }
    }

    private async UniTaskVoid TryAttack(ActionRadiusController actionRadiusController, List<AppendageController> appendages)
    {
        foreach (AppendageController appendage in appendages.ToList())
        {
            if (!actionRadiusController.storedTargets.Any() || appendage == null) continue;
            appendage.PerformAttacks(actionRadiusController.GetClosestTarget()?.position);
            await UniTask.Delay(TimeSpan.FromSeconds(volleyDelay));
        }

        await UniTask.Delay(TimeSpan.FromSeconds(timeBetweenAttacks));
        CanAttack = true;
    }
}