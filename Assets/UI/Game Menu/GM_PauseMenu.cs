using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GM_PauseMenu : VisualElement
{
    private Button saveGameButton;
    private Button loadGameButton;
    private Button settingsButton;
    private Button exitToDesktopButton;
    private Button exitToMainMenuButton;

    public new class UxmlFactory : UxmlFactory<GM_PauseMenu, UxmlTraits> { }

    public GM_PauseMenu()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        InitPauseMenu();
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void InitPauseMenu()
    {
        saveGameButton = this.Q<Button>("save-game-button");
        loadGameButton = this.Q<Button>("load-game-button");
        settingsButton = this.Q<Button>("settings-button");
        exitToDesktopButton = this.Q<Button>("exit-to-desktop-button");
        exitToMainMenuButton = this.Q<Button>("exit-to-main-menu-button");

        saveGameButton.clicked += () =>
        {
            Debug.Log("saved");
        };

        loadGameButton.clicked += () =>
        {
            Debug.Log("loaded");
        };

        settingsButton.clicked += () =>
        {
            Debug.Log("open settings");
        };

        exitToDesktopButton.clicked += () =>
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        };

        exitToMainMenuButton.clicked += () =>
        {
            Time.timeScale = 1;
            SceneManager.LoadSceneAsync("MainMenuScene");
        };
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
