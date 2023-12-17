
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;


    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });
        
        mainMenuButton.onClick.AddListener(() =>
        {
            ScenesLoader.Load(ScenesLoader.Scene.MainMenuScene);
        });
        
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManger_OnGameUnPaused;
        
        Hide();
    }

    
    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }
    private void GameManger_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
}
