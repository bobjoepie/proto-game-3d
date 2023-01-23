using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIDocManager : MonoBehaviour
{
    public static UIDocManager Instance { get; private set; }

    public UIDocument document;
    private VisualElement root;
    public HUD_BossHealthBar bossHealthBar;
    public HUD_EquipmentDisplay equipmentDisplay;
    public HUD_VitalDisplay vitalDisplay;
    public GM_PauseMenu pauseMenu;

    public UIDocManager()
    {
        Instance = this;
    }

    private void Awake()
    {
        root = document.rootVisualElement;
        bossHealthBar = root.Q<HUD_BossHealthBar>();
        equipmentDisplay = root.Q<HUD_EquipmentDisplay>();
        vitalDisplay = root.Q<HUD_VitalDisplay>();
        pauseMenu = root.Q<GM_PauseMenu>();
    }
}
