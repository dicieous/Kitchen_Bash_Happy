using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    
    private PlayerInputActions inputActions;


    private void Awake ()
    {
        inputActions = new PlayerInputActions ();
        inputActions.Player.Enable ();

        inputActions.Player.Interaction.performed += Interaction_performed;
        inputActions.Player.InteractionAlternate.performed += InteractionAlternate_performed;
        
    }

    private void InteractionAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this,EventArgs.Empty);
    }

    private void Interaction_performed (UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized ()
    {
        //PlayerDirection
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
