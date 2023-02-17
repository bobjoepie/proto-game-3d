using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_GridCell : MonoBehaviour
{
    public BG_EntityController entity;

    public bool IsFree()
    {
        return entity == null;
    }
}
