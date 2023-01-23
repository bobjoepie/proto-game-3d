using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD_BossHealthBar : VisualElement
{
    private VisualElement HealthBar;
    private Label BossName;
    public new class UxmlFactory : UxmlFactory<HUD_BossHealthBar, UxmlTraits> { }

    public HUD_BossHealthBar()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        HealthBar = this.Q("boss-health-bar-foreground");
        BossName = (Label)this.Q("boss-name-label");
        SetHealthBarAsync(100, 100).Forget();
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void SetName(string bossName)
    {
        SetNameAsync(bossName).Forget();
    }

    private async UniTaskVoid SetNameAsync(string bossName)
    {
        await UniTask.NextFrame();
        BossName.text = bossName;
    }

    public void SetHealthBar(int curHealth, int maxHealth)
    {
        SetHealthBarAsync(curHealth, maxHealth).Forget();
    }

    private async UniTaskVoid SetHealthBarAsync(int curHealth, int maxHealth)
    {
        await UniTask.NextFrame();
        HealthBar.style.width = Length.Percent(100f * curHealth / maxHealth);
    }

    public void Show()
    {
        ShowPauseMenuAsync().Forget();
    }

    private async UniTaskVoid ShowPauseMenuAsync()
    {
        await UniTask.NextFrame();
        this.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
    }

    public void Hide()
    {
        HidePauseMenuAsync().Forget();
    }

    private async UniTaskVoid HidePauseMenuAsync()
    {
        await UniTask.NextFrame();
        this.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
    }

    public void ToggleView()
    {
        if (this.style.visibility == Visibility.Visible)
        {
            HidePauseMenuAsync().Forget();
        }
        else
        {
            ShowPauseMenuAsync().Forget();
        }
    }
}
