using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BossController : EntityController
{
    [Range(0f, 20f)] public float minLeashRange = 0f;
    [Range(0f, 20f)] public float maxLeashRange = 20;
    [Range(0f, 1f)] public float actionSpeed = 0.5f;
    [Range(0f, 30f)] public float rotationSpeed = 5f;

    private bool isRotating = false;
    private Sequence movementSequence;
    private HUD_BossHealthBar bossHealthBarUI;

    private void Start()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.Register(this);
        }

        if (UIDocManager.Instance != null)
        {
            bossHealthBarUI = UIDocManager.Instance.bossHealthBar;
            bossHealthBarUI.Show();
            bossHealthBarUI.SetName(attributes.name);
        }
    }

    private void Update()
    {
        if (actionRadiusController.storedTargets.Any())
        {
            if (!isRotating)
            {
                //RotateTowardsTarget();
            }

            //MoveTowardsTarget();
        }
        else
        {
            // handle idle movement, actions, etc.
        }
    }

    private void RotateTowardsTarget()
    {
        isRotating = true;
        movementSequence = DOTween.Sequence();
        movementSequence.Append(transform.DORotateQuaternion(transform.position.AngleTowards2D(actionRadiusController.GetClosestTarget().position), actionSpeed));
        movementSequence.Play().OnComplete(() => { isRotating = false; });
    }

    private void MoveTowardsTarget()
    {
        var target = actionRadiusController.GetClosestTarget();
        var curLeashDistance = Vector2.Distance(target.position, transform.position);
        if (curLeashDistance < minLeashRange)
        {
            var dir = target.position - transform.position;
            var dist = minLeashRange - curLeashDistance;
            // 1.0f add for edge case where it can't move enough to reach the RotateAround, but stuck here
            var targetPos = gameObject.transform.position - (dir.normalized * (dist + 1.0f));
            transform.position = Vector2.MoveTowards(transform.position, targetPos, attributes.moveSpeed * Time.deltaTime);
        }
        else if (curLeashDistance > maxLeashRange)
        {
            var dir = transform.position - target.position;
            var dist = curLeashDistance - maxLeashRange;
            var targetPos = gameObject.transform.position - (dir.normalized * (dist + 1.0f));
            transform.position = Vector2.MoveTowards(transform.position, targetPos, attributes.moveSpeed * Time.deltaTime);
        }
        else
        {
            switch (Time.time % 10)
            {
                case > 5:
                    transform.RotateAround(target.position, new Vector3(0, 0, 1), Time.deltaTime * rotationSpeed);
                    break;
                default:
                    transform.RotateAround(target.position, new Vector3(0, 0, -1), Time.deltaTime * rotationSpeed);
                    break;
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        bossHealthBarUI.SetHealthBar(attributes.curHealth, attributes.maxHealth);
    }

    public override void DestroyAll()
    {
        actionRadiusController.CancelAutoSortTargets();
        base.DestroyAll();
    }

    private void OnDisable()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.Unregister(this);
        }
        bossHealthBarUI.Hide();
    }
}
