using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_CardController : MonoBehaviour
{
    [Header("Card Properties")]
    public string cardName;
    public int cardTier;
    public float cardCost;
    public BG_EntityData entityData;

    [Header("Internal Properties")] 
    public BG_CardData cardData;
    public BG_HUD_Card cardHud;

    public Sprite pickupSprite;
    public GameObject pickupGameObject;
    public AudioClip pickupAudioClip;

    public AudioClip selectionAudioClip;
    public Sprite portrait;
    public Sprite icon;

    public void RemoveFromHud()
    {
        cardHud.DestroyCard();
    }
}
