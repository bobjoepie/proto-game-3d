using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Weapons/Custom Projectile"), Serializable]
public class ProjectileWeaponSO : WeaponSO
{
    public List<ProjectilePart> weaponParts;
}

[Serializable]
public class ProjectilePart : WeaponPart
{
    [Header("Projectile Properties")]
    [SerializeField] public bool cur_hasCollision;
    [SerializeField, Range(0, 10)] public float cur_lifeTime;
    [SerializeField, Range(0, 100)] public float cur_speed;
    public TargetType cur_targetType;
    public AudioClip cur_attackSound;

    [Header("Pre-Attack")]
    public bool pre_hasCollision;
    [SerializeField, Range(0, 10)] public float pre_lifeTime;
    [SerializeField, Range(0, 100)] public float pre_speed;
    public TargetType pre_targetType;
    public AudioClip pre_attackSound;

    [Header("Post-Attack")]
    public WeaponSO post_subWeapon;

    public void UpdateValues(ProjectileController instance, int iterationNum)
    {
        instance.iterationNum = iterationNum + 1;

        instance.damage = this.damage;
        instance.collisionType = this.collisionType;
        instance.weaponType = this.weaponType;
        instance.weaponSpawn = this.weaponSpawn;

        instance.cur_hasCollision = this.cur_hasCollision;
        instance.cur_lifeTime = this.cur_lifeTime;
        instance.cur_speed = this.cur_speed;
        instance.cur_targetType = this.cur_targetType;
        instance.cur_attackSound = this.cur_attackSound;

        instance.pre_hasCollision = this.pre_hasCollision;
        instance.pre_lifeTime = this.pre_lifeTime;
        instance.pre_speed = this.pre_speed;
        instance.pre_targetType = this.pre_targetType;
        instance.pre_attackSound = this.pre_attackSound;

        instance.post_subWeapon = this.post_subWeapon;
    }
}
