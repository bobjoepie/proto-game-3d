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

    public virtual void ConvertData(BG_EntityController entityController)
    {
        entityController.internalName = this.internalName;

        entityController.pickupSprite = this.pickupSprite;
        entityController.pickupGameObject = this.pickupGameObject;
        entityController.pickupAudioClip = this.pickupAudioClip;

        entityController.selectionAudioClip = this.selectionAudioClip;
        entityController.portrait = this.portrait;
        entityController.icon = this.icon;
    }
}
