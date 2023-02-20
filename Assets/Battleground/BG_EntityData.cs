using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BG_EntityData : ScriptableObject
{
    public string internalName;
    public BG_EntityController entityGameObject;

    public Sprite pickupSprite;
    public GameObject pickupGameObject;
    public AudioClip pickupAudioClip;

    public AudioClip selectionAudioClip;
    public Sprite portrait;
    public Sprite icon;

    public BG_EntityAttributes attributes;

    public virtual void ConvertData(BG_EntityController entityController)
    {
        entityController.internalName = this.internalName;

        entityController.pickupSprite = this.pickupSprite;
        entityController.pickupGameObject = this.pickupGameObject;
        entityController.pickupAudioClip = this.pickupAudioClip;

        entityController.selectionAudioClip = this.selectionAudioClip;
        entityController.portrait = this.portrait;
        entityController.icon = this.icon;
        
        entityController.attributes.name = this.attributes.name;
        entityController.attributes.tags = this.attributes.tags;
        entityController.attributes.curLevel = this.attributes.curLevel;
        entityController.attributes.curExperience = this.attributes.curExperience;
        entityController.attributes.expToLevel = this.attributes.expToLevel;
        entityController.attributes.curHealth = this.attributes.curHealth;
        entityController.attributes.maxHealth = this.attributes.maxHealth;
        entityController.attributes.actionSpeed = this.attributes.actionSpeed;
        entityController.attributes.moveSpeed = this.attributes.moveSpeed;
        
        entityController.attributes.meleeAttacksPerSecond = this.attributes.meleeAttacksPerSecond;
        entityController.attributes.rangedAttacksPerSecond = this.attributes.rangedAttacksPerSecond;

        entityController.attributes.actionRange = this.attributes.actionRange;
        entityController.attributes.meleeRange = this.attributes.meleeRange;
        entityController.attributes.rangedRange = this.attributes.rangedRange;
        
        entityController.attributes.strength = this.attributes.strength;
        entityController.attributes.dexterity = this.attributes.dexterity;
        entityController.attributes.intelligence = this.attributes.intelligence;
        entityController.attributes.luck = this.attributes.luck;

        entityController.attributes.physRes = this.attributes.physRes;
        entityController.attributes.elemRes = this.attributes.elemRes;
    }
}

[Serializable]
public class BG_EntityAttributes
{
    [Header("Properties")]
    public string name;
    public BG_EntityTags tags;
    public int curLevel;
    public int curExperience;
    public int expToLevel;
    public int curHealth;
    public int maxHealth;
    public int actionSpeed;
    public int moveSpeed;

    [Header("Attack Speed")]
    public float meleeAttacksPerSecond;
    public float rangedAttacksPerSecond;

    [Header("Range")]
    public float actionRange;
    public float meleeRange;
    public float rangedRange;

    [Header("Attributes")]
    public int strength;
    public int dexterity;
    public int intelligence;
    public int luck;

    [Header("Resistances")]
    public int physRes;
    public int elemRes;
}

[Flags]
public enum BG_EntityTags : uint
{
    None = 0,
    ObjectiveSeeker = 1 << 0,

    ObjectiveDestroy = 1 << 1,
    ObjectiveCapture = 1 << 2,
    ObjectiveHold = 1 << 3,
    ObjectiveTouch = 1 << 4,
    Objective = ObjectiveDestroy | ObjectiveCapture | ObjectiveHold | ObjectiveTouch,

    FactionPlayer = 1 << 5,
    FactionEnemy = 1 << 6,
    FactionNeutral = 1 << 7,
    Faction = FactionPlayer | FactionEnemy | FactionNeutral,

    AttackMelee = 1 << 8,
    AttackRanged = 1 << 9,
    AttackCaster = 1 << 10,

    MovementGround = 1 << 11,
    MovementAir = 1 << 12,
    MovementWater = 1 << 13,

    All = uint.MaxValue
}