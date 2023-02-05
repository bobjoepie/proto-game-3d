using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BG_EntityController : MonoBehaviour
{
    public LayerMask layer;
    public Material baseMaterial;
    public Material selectedMaterial;

    public Renderer meshRenderer;
    public BG_AnimatorController animator;

    [Header("Properties")]
    public string internalName;

    public Sprite pickupSprite;
    public GameObject pickupGameObject;
    public AudioClip pickupAudioClip;

    public AudioClip selectionAudioClip;
    public Sprite portrait;
    public Sprite icon;

    public BG_EntityAttributes attributes;

    // Start is called before the first frame update
    public virtual void Start()
    {
        gameObject.layer = layer.ToLayer();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.GetComponentInChildren<Renderer>();
        }
        baseMaterial = meshRenderer.material;
    }

    public void Register<T>(T component) where T : MonoBehaviour
    {
        switch (component)
        {
            case BG_AnimatorController ac:
                animator = ac;
                break;
        }
    }

    public void Unregister<T>(T component) where T : MonoBehaviour
    {
        switch (component)
        {
            case BG_AnimatorController ac:
                animator = null;
                break;
        }
    }

    public virtual BG_EntityController Select()
    {
        meshRenderer.material = selectedMaterial;
        return this;
    }

    public virtual BG_EntityController Deselect()
    {
        meshRenderer.material = baseMaterial;
        return null;
    }
}
