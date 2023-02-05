using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSummon", menuName = "BG/Card Behavior/Card Summon"), Serializable]
public class BG_CardSummon : BG_CardBehavior
{
    [Header("Properties")]
    public BG_UnitData unitData;
}
