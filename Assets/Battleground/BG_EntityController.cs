using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

public class BG_EntityController : MonoBehaviour
{
    public Material baseMaterial;
    public Material selectedMaterial;

    public Renderer meshRenderer;
    public BG_AnimatorController animator;
    public BG_ActionRadiusController actionRadiusController;
    public BG_ColliderController colliderController;
    public BG_MeleeRadiusController meleeRadiusController;
    public BG_RangedRadiusController rangedRadiusController;

    [Header("Properties")]
    public string internalName;
    public LayerMask collisionLayer;
    public LayerMask actionRadiusLayer;

    public Sprite pickupSprite;
    public GameObject pickupGameObject;
    public AudioClip pickupAudioClip;

    public AudioClip selectionAudioClip;
    public Sprite portrait;
    public Sprite icon;

    public BG_EntityAttributes attributes;

    [SerializeField] public BehaviorTree behaviorTree;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.GetComponentInChildren<Renderer>();
        }
        baseMaterial = meshRenderer.material;

        attributes.curHealth = attributes.maxHealth;
    }

    public void Register<T>(T component) where T : MonoBehaviour
    {
        switch (component)
        {
            case BG_AnimatorController ac:
                animator = ac;
                break;
            case BG_ActionRadiusController ar:
                actionRadiusController = ar;
                break;
            case BG_ColliderController co:
                colliderController = co;
                break;
            case BG_MeleeRadiusController mr:
                meleeRadiusController = mr;
                break;
            case BG_RangedRadiusController rr:
                rangedRadiusController = rr;
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
            case BG_ActionRadiusController ar:
                actionRadiusController = null;
                break;
            case BG_ColliderController co:
                colliderController = null;
                break;
            case BG_MeleeRadiusController mr:
                meleeRadiusController = null;
                break;
            case BG_RangedRadiusController rr:
                rangedRadiusController = null;
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

    public virtual void TakeDamage(int damage)
    {
        attributes.curHealth -= damage;
        if (attributes.curHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
