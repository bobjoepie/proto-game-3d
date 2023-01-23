using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public WeaponSO weaponSO;
    public List<WeaponPart> weaponParts = new List<WeaponPart>();
    public LayerMask mask;

    private PlayerController playerController;
    public bool CanAttack = true;

    private void Awake()
    {
        playerController = transform.root.GetComponent<PlayerController>();
        if (weaponParts.Count == 0 && weaponSO != null)
        {
            weaponParts = WeaponSO.ConvertWeaponToParts(weaponSO);
        }
    }

    private void OnEnable()
    {
        GetComponent<AppendageController>()?.Register(this);
    }

    public async UniTaskVoid Attack(Vector3? targetPos = null)
    {
        if (!CanAttack) return;
        CanAttack = false;

        if (playerController != null && playerController.equippedWeapon != weaponSO)
        {
            weaponSO = playerController.equippedWeapon;
            weaponParts = WeaponSO.ConvertWeaponToParts(playerController.equippedWeapon);
        }

        for (int i = 0; i < weaponSO.amount; i++)
        {
            var rot = targetPos != null ? Quaternion.LookRotation((Vector3)targetPos - transform.position) : transform.rotation;
            WeaponSO.InstantiateWeaponParts(weaponParts, transform.position, rot, mask.ToLayer());
            if (weaponSO.amountBurstTime > 0f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(weaponSO.amountBurstTime));
            }
        }

        await UniTask.Delay(TimeSpan.FromSeconds(weaponSO.cooldown));
        CanAttack = true;
    }
}