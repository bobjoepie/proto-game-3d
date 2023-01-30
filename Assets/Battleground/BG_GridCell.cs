using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_GridCell : MonoBehaviour
{
    public BG_EntityController entity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsFree()
    {
        return entity == null;
    }
}
