using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_EntityController : MonoBehaviour
{
    public LayerMask layer;
    public Material baseMaterial;
    public Material selectedMaterial;

    private Renderer meshRenderer;

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
