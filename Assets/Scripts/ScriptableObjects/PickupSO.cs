using UnityEngine;

public abstract class PickupSO : ScriptableObject
{
    public string internalName;

    public Sprite pickupSprite;
    public GameObject pickupGameObject;
    public AudioClip pickupAudioClip;

    public Sprite displaySprite;
    public GameObject displayGameObject;
    public AudioClip displayAudioClip;
}