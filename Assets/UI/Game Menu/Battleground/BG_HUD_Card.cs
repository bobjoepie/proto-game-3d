using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BG_HUD_Card : Button
{
    public BG_CardController cardController;
    public new class UxmlFactory : UxmlFactory<BG_HUD_Card, UxmlTraits> { }

    public BG_HUD_Card()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void InitCard(BG_CardController card, Action action)
    {
        card.cardHud = this;
        this.cardController = card;
        this.text = card.cardName;
        this.focusable = false;
        this.clickable.clicked += () =>
        {
            action.Invoke();
        };
    }

    public void DestroyCard()
    {
        this.parent.RemoveFromHierarchy();
    }
}
