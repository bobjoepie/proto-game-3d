using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passive Item", menuName = "Item/Passive Item"), Serializable]
public class PassiveItemSO : ItemSO
{
    public List<PassiveItemPart> itemParts;
}

[Serializable]
public class PassiveItemPart : ItemPart
{
    [Header("Passive Item Properties")]
    public int placeHolderNum;

    public void UpdateValues(PassiveItemController instance, int iterationNum)
    {
        //instance.iterationNum = iterationNum + 1;

        //instance.damage = this.damage;
        //instance.collisionType = this.collisionType;
        //instance.weaponType = this.weaponType;
        //instance.weaponSpawn = this.weaponSpawn;

        //instance.cur_hasCollision = this.cur_hasCollision;
        //instance.cur_lifeTime = this.cur_lifeTime;
        //instance.cur_speed = this.cur_speed;
        //instance.cur_direction = this.cur_direction;
        //instance.cur_spread = this.cur_spread;
        //instance.cur_rotation = this.cur_rotation;
        //instance.cur_rotationSpeed = this.cur_rotationSpeed;
        //instance.cur_targetType = this.cur_targetType;

        //instance.pre_hasCollision = this.pre_hasCollision;
        //instance.pre_lifeTime = this.pre_lifeTime;
        //instance.pre_speed = this.pre_speed;
        //instance.pre_direction = this.pre_direction;
        //instance.pre_spread = this.pre_spread;
        //instance.pre_rotation = this.pre_rotation;
        //instance.pre_rotationSpeed = this.pre_rotationSpeed;
        //instance.pre_targetType = this.pre_targetType;

        //instance.post_subWeapon = this.post_subWeapon;
    }
}