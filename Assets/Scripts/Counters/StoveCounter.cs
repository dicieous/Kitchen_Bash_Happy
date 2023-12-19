using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State State;
    }


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private State _state;

    private float fryingTimer;
    private float burnedTimer;

    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOArray;
    private FryingRecipeSO _fryingRecipeSo;

    [SerializeField] private BurnedRecipeSO[] _burnedRecipeSOArray;
    private BurnedRecipeSO _burnedRecipeSo;


    private void Start()
    {
        _state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / _fryingRecipeSo.fryingTimerMax
                    });

                    fryingTimer += Time.deltaTime;
                    if (fryingTimer > _fryingRecipeSo.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObjects.SpawnKitchenObject(_fryingRecipeSo.output, this);

                        _burnedRecipeSo = GetBurnedRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        _state = State.Fried;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            State = _state
                        });

                        burnedTimer = 0f;
                    }

                    break;
                case State.Fried:
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burnedTimer / _burnedRecipeSo.burningTimerMax
                    });

                    burnedTimer += Time.deltaTime;
                    if (burnedTimer > _burnedRecipeSo.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObjects.SpawnKitchenObject(_burnedRecipeSo.output, this);


                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });

                        _state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            State = _state
                        });
                    }

                    break;
                case State.Burned:
                    break;
            }

        {
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //counter doesn't have a Object on it
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                //player has kitchen object and it can be Fried
                player.GetKitchenObject().SetKItchenObjectParent(this);

                _fryingRecipeSo = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());


                _state = State.Frying;
                fryingTimer = 0f;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    State = _state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = fryingTimer / _fryingRecipeSo.fryingTimerMax
                });
            }
        }
        else
        {
            //counter has a kitchen object
            if (player.HasKitchenObject())
            {
                //player has something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player has plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }

                _state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    State = _state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
            else
            {
                //player doesn't have something
                GetKitchenObject().SetKItchenObjectParent(player);
                _state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    State = _state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private KitchenObjectsSO OutputForInput(KitchenObjectsSO inputKitchenObjectSO)
    {
        var fryingRecipeSo = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        if (fryingRecipeSo != null) return fryingRecipeSo.output;

        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectsSO kitchenObjectsSo)
    {
        var fryingRecipeSo = GetFryingRecipeSOWithInput(kitchenObjectsSo);

        return fryingRecipeSo != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectsSO inputKitchenObjectSO)
    {
        foreach (var fryingRecipeSo in _fryingRecipeSOArray)
        {
            if (fryingRecipeSo.input == inputKitchenObjectSO)
            {
                return fryingRecipeSo;
            }
        }

        return null;
    }

    private BurnedRecipeSO GetBurnedRecipeSOWithInput(KitchenObjectsSO inputKitchenObjectSO)
    {
        foreach (var burnedRecipeSo in _burnedRecipeSOArray)
        {
            if (burnedRecipeSo.input == inputKitchenObjectSO)
            {
                return burnedRecipeSo;
            }
        }

        return null;
    }

    public bool IsFried()
    {
        return _state == State.Fried;
    }
}