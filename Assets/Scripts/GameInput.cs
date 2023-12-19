using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDING = "InputBindings";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause,
        GamePadInteract,
        GamePadInteractAlternate,
        GamePadPause
    }

    private PlayerInputActions inputActions;


    private void Awake()
    {
        Instance = this;

        inputActions = new PlayerInputActions();
        
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING))
        {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDING));
        }
        
        inputActions.Player.Enable();

        inputActions.Player.Interaction.performed += Interaction_performed;
        inputActions.Player.InteractionAlternate.performed += InteractionAlternate_performed;
        inputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        inputActions.Player.Interaction.performed -= Interaction_performed;
        inputActions.Player.InteractionAlternate.performed -= InteractionAlternate_performed;
        inputActions.Player.Pause.performed -= Pause_performed;

        inputActions.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractionAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interaction_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        //PlayerDirection
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                return inputActions.Player.Move.bindings[1].ToDisplayString();

            case Binding.MoveDown:
                return inputActions.Player.Move.bindings[2].ToDisplayString();

            case Binding.MoveLeft:
                return inputActions.Player.Move.bindings[3].ToDisplayString();

            case Binding.MoveRight:
                return inputActions.Player.Move.bindings[4].ToDisplayString();

            case Binding.Interact:
                return inputActions.Player.Interaction.bindings[0].ToDisplayString();

            case Binding.InteractAlternate:
                return inputActions.Player.InteractionAlternate.bindings[0].ToDisplayString();

            case Binding.Pause:
                return inputActions.Player.Pause.bindings[0].ToDisplayString();
            
            case Binding.GamePadInteract:
                return inputActions.Player.Interaction.bindings[1].ToDisplayString();
            
            case Binding.GamePadInteractAlternate:
                return inputActions.Player.InteractionAlternate.bindings[1].ToDisplayString();
            
            case Binding.GamePadPause:
                return inputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        inputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.MoveUp:
                inputAction = inputActions.Player.Move;
                bindingIndex = 1;

                break;
            case Binding.MoveDown:
                inputAction = inputActions.Player.Move;
                bindingIndex = 2;

                break;
            case Binding.MoveLeft:
                inputAction = inputActions.Player.Move;
                bindingIndex = 3;

                break;
            case Binding.MoveRight:
                inputAction = inputActions.Player.Move;
                bindingIndex = 4;

                break;
            case Binding.Interact:
                inputAction = inputActions.Player.Interaction;
                bindingIndex = 0;

                break;
            case Binding.InteractAlternate:
                inputAction = inputActions.Player.InteractionAlternate;
                bindingIndex = 0;

                break;
            case Binding.Pause:
                inputAction = inputActions.Player.Pause;
                bindingIndex = 0;

                break;
            case Binding.GamePadInteract:
                inputAction = inputActions.Player.Interaction;
                bindingIndex = 1;

                break;
            case Binding.GamePadInteractAlternate:
                inputAction = inputActions.Player.InteractionAlternate;
                bindingIndex = 1;

                break;
            case Binding.GamePadPause:
                inputAction = inputActions.Player.Pause;
                bindingIndex = 1;

                break;
        }   

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                inputActions.Player.Enable();
                onActionRebound();
                
                PlayerPrefs.SetString(PLAYER_PREFS_BINDING,inputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }
}