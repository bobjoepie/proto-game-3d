using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "BG/Card Data"), Serializable]
public class BG_CardData : ScriptableObject
{
    [Header("Card Properties")]
    public string cardName;
    public int cardTier;
    public float cardCost;
    public List<BG_CardBehavior> castBehaviors;

    [Header("Internal Properties")]
    public Sprite pickupSprite;
    public GameObject pickupGameObject;
    public AudioClip pickupAudioClip;

    public AudioClip selectionAudioClip;
    public Sprite portrait;
    public Sprite icon;

    public void ConvertData(BG_CardController cardController)
    {
        cardController.cardName = this.cardName;
        cardController.cardTier = this.cardTier;
        cardController.cardCost = this.cardCost;
        cardController.castBehaviors = this.castBehaviors;

        cardController.cardData = this;

        cardController.pickupSprite = this.pickupSprite;
        cardController.pickupGameObject = this.pickupGameObject;
        cardController.pickupAudioClip = this.pickupAudioClip;

        cardController.selectionAudioClip = this.selectionAudioClip;
        cardController.portrait = this.portrait;
        cardController.icon = this.icon;
    }
}
