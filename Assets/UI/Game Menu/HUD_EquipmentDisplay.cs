using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD_EquipmentDisplay : VisualElement
{
    private VisualElement EquippedWeapon;
    private VisualElement EquippedWeaponIcon;
    private Label EquippedWeaponLabel;
    public new class UxmlFactory : UxmlFactory<HUD_EquipmentDisplay, UxmlTraits> { }

    public HUD_EquipmentDisplay()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        EquippedWeapon = this.Q("equipped-weapon-display");
        EquippedWeaponIcon = EquippedWeapon.Q("equipped-weapon-icon");
        EquippedWeaponLabel = (Label)EquippedWeapon.Q("equipped-weapon-label");
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void SetPrimaryWeapon(PickupSO weapon)
    {
        SetPrimaryWeaponAsync(weapon).Forget();
    }

    private async UniTaskVoid SetPrimaryWeaponAsync(PickupSO weapon)
    {
        await UniTask.NextFrame();
        EquippedWeaponIcon.style.backgroundImage = new StyleBackground(weapon.displaySprite);
    }

    public void ClearDisplay()
    {
        EquippedWeaponIcon.style.backgroundImage = new StyleBackground();
    }

    public void Enable()
    {

    }

    public void Disable()
    {

    }
}
