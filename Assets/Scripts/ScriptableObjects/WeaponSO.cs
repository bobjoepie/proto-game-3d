using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public abstract class WeaponSO : PickupSO
{
    [TextArea(3, 5)]
    public string description;
    [Range(0, 1)]
    public float cooldown;
    [Range(1, 10)] public int amount;
    [Range(0, 1)] public float amountBurstTime;

    private void Reset()
    {
        amount = 1;
    }

    public static List<WeaponPart> ConvertWeaponToParts(WeaponSO weaponSO)
    {
        List<WeaponPart> weaponParts = new List<WeaponPart>();

        switch (weaponSO)
        {
            case ProjectileWeaponSO projObj:
                weaponParts = projObj.weaponParts.Cast<WeaponPart>().ToList();
                break;
        }
        return weaponParts;
    }

    public static void InstantiateWeaponParts(List<WeaponPart> weaponParts, Vector3 position, Quaternion rotation, int? layer, int iterationNum = 0)
    {
        if (iterationNum >= 5)
        {
            return;
        }
        foreach (WeaponPart part in weaponParts)
        {
            switch (part.weaponSpawn)
            {
                case SpawnLocation.ClosestEnemy:
                    break;
                case SpawnLocation.ClosestMouse:
                    break;
                case SpawnLocation.Single:
                    break;
                default:
                    var wepInstance = Instantiate(part.weaponGameObject);
                    wepInstance.transform.position = position;
                    wepInstance.transform.rotation = rotation;
                    if (layer != null)
                    {
                        wepInstance.gameObject.layer = (int)layer;
                        foreach (Transform child in wepInstance.transform)
                        {
                            child.gameObject.layer = (int)layer;
                        }
                    }

                    part.UpdatePartValues(wepInstance, iterationNum);
                    break;
            }
        }
    }
}

public enum WeaponType
{
    Projectile
}

public enum TargetType
{
    None,
    TowardsMouse,
    Self, //TODO
    TowardsNearestEnemy, //TODO
    TowardsNearestMouse, //TODO
    TowardsInitialMouse,
    TowardsInitialEnemy, //TODO
}

public enum SpawnLocation
{
    None,
    Self,
    MousePoint,
    ClosestEnemy, //TODO
    ClosestMouse, //TODO
    Single, //TODO
    CustomPosition, //TODO
}

public enum CollisionType //TODO
{
    Enemy,
    All,
    Player,
    None
}

[Serializable]
public class WeaponPart
{
    public string label;

    [Header("Properties")]
    public WeaponType weaponType;
    public CollisionType collisionType;
    public WeaponBase weaponGameObject;
    public int damage;
    public SpawnLocation weaponSpawn;

    public void UpdatePartValues(WeaponBase wepInstance, int iterationNum)
    {
        switch (this)
        {
            case ProjectilePart proj:
                proj.UpdateValues(wepInstance as ProjectileController, iterationNum);
                break;
        }
    }
}
