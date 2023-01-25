using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class BG_UIDocManager : MonoBehaviour
{
    public static BG_UIDocManager Instance { get; private set; }
    public UIDocument document;
    private static VisualElement root;
    public BG_HUD_Overlay hudOverlay;
    private List<VisualElement> rayCastBlockers = new List<VisualElement>();

    public BG_UIDocManager()
    {
        Instance = this;
    }

    private void Awake()
    {
        root = document.rootVisualElement;
        hudOverlay = root.Q<BG_HUD_Overlay>();
    }

    public void AddRaycastBlocker(VisualElement element)
    {
        UniTask.Action(async () =>
        {
            await UniTask.NextFrame();
            rayCastBlockers.Add(element);

        }).Invoke();
    }

    public bool IsHoveringOverUI()
    {
        foreach (var blocker in rayCastBlockers)
        {
            if (IsMouseOverBlocker(blocker))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsMouseOverBlocker(VisualElement element)
    {
        var mousePosition = Input.mousePosition;
        var scaledMousePosition = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        
        var mousePosPanel = scaledMousePosition * root.panel.visualTree.layout.size;

        Rect layout = element.layout;
        Vector3 pos = element.transform.position;
        Rect blockingArea = new Rect(pos.x, pos.y, layout.width, layout.height);

        if (mousePosPanel.x <= blockingArea.xMax && mousePosPanel.x >= blockingArea.xMin && mousePosPanel.y <= blockingArea.yMax && mousePosPanel.y >= blockingArea.yMin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
