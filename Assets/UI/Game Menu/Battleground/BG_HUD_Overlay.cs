using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class BG_HUD_Overlay : VisualElement
{
    //private VisualElement HealthBar;
    //private Label BossName;
    public List<VisualElement> panels = new List<VisualElement>();
    public List<Button> buttons = new List<Button>();

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
        var panel = panels[2];
        var rows = panel.Children().Where(c => c.name == "row");
        foreach (var row in rows)
        {
            foreach (var element in row.Children())
            {
                var button = (Button)element.Children().First();
                buttons.Add(button);
            }
        }
        ClearButtonActions();

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
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
                    buttons[index].clicked += actions[index].Invoke;
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

        foreach (var row in panels[2].Children())
        {
            UniTask.Action(async () =>
            {
                await UniTask.NextFrame();
                row.style.visibility = Visibility.Visible;

            }).Invoke();
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
        
        foreach (var row in panels[2].Children())
        {
            UniTask.Action(async () =>
            {
                await UniTask.NextFrame();
                row.style.visibility = Visibility.Hidden;

            }).Invoke();
        }
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
}
