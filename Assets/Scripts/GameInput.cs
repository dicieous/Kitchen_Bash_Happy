using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;

    private PlayerInputActions inputActions;


    private void Awake ()
    {
        inputActions = new PlayerInputActions ();
        inputActions.Player.Enable ();

        inputActions.Player.Interaction.performed += Interaction_performed;
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
