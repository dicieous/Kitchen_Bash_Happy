using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IkitchenObjectParent
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float rotSpeed = 18f;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask CounterlayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    public static Player LocalInstance { get; private set; }

    private bool isWalking;

    private Vector3 lastInteraction;

    private BaseCounter SelectedCounter;

    private KitchenObjects kitchenObject;

    public static event EventHandler OnAnyPlayerSpawned;
    public event EventHandler OnPickingSomething;
    public static event EventHandler OnAnyPlayerPickSomething;
    public event EventHandler<OnselectedCounterChangedEventArgs> OnselectedCounterChanged;
    
    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
        OnAnyPlayerPickSomething = null;
    }
    

    public class OnselectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    
    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractActionAlternate;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }
        OnAnyPlayerSpawned?.Invoke(this,EventArgs.Empty);
    }

    private void GameInput_OnInteractActionAlternate(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (SelectedCounter != null)
        {
            SelectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        
        if (SelectedCounter != null)
        {
            SelectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        if(!IsOwner) return;
        HandleInteractions();

        HandleMovements();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 dirVec = new Vector3(inputVector.x, 0, inputVector.y);


        if (dirVec != Vector3.zero)
        {
            lastInteraction = dirVec;
        }

        float interactionDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteraction, out RaycastHit raycasthit, interactionDistance,
                CounterlayerMask))
        {
            if (raycasthit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //Has ClearCounter
                if (baseCounter != SelectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }


    private void HandleMovements()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        //PlayerMovement
        Vector3 dirVec = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, dirVec, moveDistance);

        if (!canMove)
        {
            //Attempt move in X direction
            Vector3 dirVecX = new Vector3(dirVec.x, 0, 0);
            canMove = (dirVec.x < -0.5f || dirVec.x > 0.5f) && !Physics.CapsuleCast(transform.position,
                transform.position + Vector3.up * playerHeight, playerRadius, dirVecX, moveDistance);

            if (canMove)
            {
                //can only move in X
                dirVec = dirVecX;
            }
            else
            {
                //Attempt in Z direction
                Vector3 dirVecZ = new Vector3(0, 0, dirVec.z);
                canMove = (dirVec.z < -0.5f || dirVec.z > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                    playerRadius, dirVecZ, moveDistance);

                if (canMove)
                {
                    //can only move in Z direction
                    dirVec = dirVecZ;
                }
            }
        }


        if (canMove)
        {
            transform.position += dirVec * moveDistance;
        }

        isWalking = dirVec != Vector3.zero;

        //PlayerRotation

        transform.forward = Vector3.Slerp(transform.forward, dirVec, Time.deltaTime * rotSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.SelectedCounter = selectedCounter;
        OnselectedCounterChanged?.Invoke(this, new OnselectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObjects kitchenObjects)
    {
        this.kitchenObject = kitchenObjects;

        if (kitchenObject != null)
        {
            OnPickingSomething?.Invoke(this, EventArgs.Empty);
            OnAnyPlayerPickSomething?.Invoke(this,EventArgs.Empty); 
        }
    }

    public KitchenObjects GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}