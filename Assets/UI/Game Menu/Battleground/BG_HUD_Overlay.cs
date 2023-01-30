using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class BG_HUD_Overlay : VisualElement
{
    //private VisualElement HealthBar;
    //private Label BossName;
    public List<VisualElement> panels = new List<VisualElement>();
    public VisualElement buttonPanel;
    public List<Button> buttons = new List<Button>();
    public VisualElement cardPanel;
    public List<BG_HUD_Card> cards = new List<BG_HUD_Card>();
    public VisualElement statePanel;
    public Button endTurnButton;

    public VisualTreeAsset cardTemplate;

    public new class UxmlFactory : UxmlFactory<BG_HUD_Overlay, UxmlTraits> { }

    public BG_HUD_Overlay()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        var toolbar = this.Q("bar");
        BG_UIDocManager.Instance.AddRaycastBlocker(toolbar);
        panels = toolbar.Children().ToList();
        InitButtonPanel();
        InitCardPanel();
        InitStatePanel();

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void InitButtonPanel()
    {
        buttonPanel = panels.First(p => p.name=="button-panel");
        var rows = buttonPanel.Children().Where(c => c.name == "row");
        foreach (var row in rows)
        {
            foreach (var element in row.Children())
            {
                var button = (Button)element.Children().First();
                button.clickable = null;
                buttons.Add(button);
            }
        }
    }

    private void InitCardPanel()
    {
        cardPanel = panels.First(p => p.name == "card-panel");
    }

    private void InitStatePanel()
    {
        statePanel = panels.First(p => p.name == "state-panel");
        endTurnButton = statePanel.Q<Button>("end-turn-button");
    }

    public void InitButtons(List<Action> actions)
    {
        for (var i = 0; i < buttons.Count; i++)
        {
            var index = i;
            if (index < actions.Count)
            {
                UniTask.Action(async () =>
                {
                    await UniTask.NextFrame();
                    buttons[index].clickable.clicked += actions[index].Invoke;
                }).Invoke();
            }
            else
            {
                UniTask.Action(async () =>
                {
                    await UniTask.NextFrame();
                    buttons[index].clickable = null;
                }).Invoke();
            }
        }
    }

    public void ClearButtonActions()
    {
        foreach (var button in buttons)
        {
            UniTask.Action(async () =>
            {
                await UniTask.NextFrame();
                button.clickable = null;

            }).Invoke();
        }
    }

    public void InitCard(BG_CardController cardController, Action action)
    {
        UniTask.Action(async () =>
        {
            await UniTask.NextFrame();
            var cardContainer = cardTemplate.Instantiate().Children().First();
            var card = cardContainer.Q<BG_HUD_Card>();
            card.InitCard(cardController, action);

            cardPanel.Add(cardContainer);

        }).Invoke();
    }

    public void RemoveCard(BG_CardController cardController)
    {
        UniTask.Action(async () =>
        {
            await UniTask.NextFrame();
            cardController.RemoveFromHud();

        }).Invoke();
    }

    public void InitEndTurnButton(Action action)
    {
        UniTask.Action(async () =>
        {
            await UniTask.NextFrame();
            endTurnButton.clickable.clicked += action;

        }).Invoke();
    }

    public void Show()
    {
        UniTask.Action(async () =>
        {
            await UniTask.NextFrame();
            this.style.visibility = Visibility.Visible;

        }).Invoke();
    }

    public void Hide()
    {
        UniTask.Action(async () =>
        {
            await UniTask.NextFrame();
            this.style.visibility = Visibility.Hidden;

        }).Invoke();
    }

    public void ToggleView()
    {
        if (this.style.visibility == Visibility.Visible)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public void ShowPanel(PanelType panel)
    {
        UniTask.Action(async () =>
        {
            await UniTask.NextFrame();
            switch (panel)
            {
                case PanelType.Button:
                    buttonPanel.style.visibility = Visibility.Visible;
                    break;
                case PanelType.Card:
                    cardPanel.style.visibility = Visibility.Visible;
                    break;
            }

        }).Invoke();
    }

    public void HidePanel(PanelType panel)
    {
        UniTask.Action(async () =>
        {
            await UniTask.NextFrame();
            switch (panel)
            {
                case PanelType.Button:
                    buttonPanel.style.visibility = Visibility.Hidden;
                    break;
                case PanelType.Card:
                    cardPanel.style.visibility = Visibility.Hidden;
                    break;
            }

        }).Invoke();
    }
}

public enum PanelType
{
    Button,
    Card
}