using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    
    [SerializeField] private Button hideButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button soundEffectButton;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI soundEffectText;
    
    
    [Space(10)]
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamePadPauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamePadInteractAlternateText;
    
    [Space(10)]
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAlternateButton;
    [SerializeField] private Button gamepadPauseButton;
    
    [Space(10)]
    [SerializeField] private Transform pressToRebindKeyUI;

    private Action onCloseButtonAction;


    private void Awake()
    {
        Instance = this;
        
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisuals();
        });

        soundEffectButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisuals();
        });
        
        hideButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });
        
        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveUp); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveDown); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveLeft); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveRight); });
        
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        interactAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
        
        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePadInteract); });
        gamepadInteractAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePadInteractAlternate); });
        gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePadPause); });

    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        
        UpdateVisuals();
        
        HidePressRebindKey();
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisuals()
    {
        soundEffectText.text = "Sound Effect : " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = " Music : " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePadInteract);
        gamePadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePadInteractAlternate);
        gamePadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePadPause);

    }
    
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        
        gameObject.SetActive(true);
        soundEffectButton.Select();
    }
    
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressRebindKey()
    {
        pressToRebindKeyUI.gameObject.SetActive(true);
    }
    
    private void HidePressRebindKey()
    {
        pressToRebindKeyUI.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressRebindKey();
            UpdateVisuals();
        });
    }
}