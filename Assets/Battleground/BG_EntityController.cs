using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_EntityController : MonoBehaviour
{
    public LayerMask layer;
    public Material baseMaterial;
    public Material selectedMaterial;

    private Renderer meshRenderer;

    [Header("Properties")]
    public string internalName;

    public Sprite pickupSprite;
    public GameObject pickupGameObject;
    public AudioClip pickupAudioClip;

    public AudioClip selectionAudioClip;
    public Sprite portrait;
    public Sprite icon;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = layer.ToLayer();
        meshRenderer = gameObject.GetComponent<Renderer>();
        baseMaterial = meshRenderer.material;
    }

    public BG_EntityController Select()
    {
        meshRenderer.material = selectedMaterial;
        return this;
    }

    public BG_EntityController Deselect()
    {
        meshRenderer.material = baseMaterial;
        return null;
    }
}
