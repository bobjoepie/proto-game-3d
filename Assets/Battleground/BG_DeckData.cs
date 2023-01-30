using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "BG/Deck Data"), Serializable]
public class BG_DeckData : ScriptableObject
{
    public string deckName;
    public List<BG_CardData> deck = new List<BG_CardData>();
}
