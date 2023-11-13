using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CuttingCounter : BaseCounter
{
    public event EventHandler <OnProgressChangedEventArgs> OnCuttingProgressChanged;
    
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    public event EventHandler OnCut;
    
    [SerializeField] private CuttingRecipeSO[] cutKitchenObjectsSOArray;

    private int _cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //counter doesn't have a Object on it
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObjects().GetKitchenObjectSO()))
            {
                //player has kitchen object and it can be cut
                player.GetKitchenObjects().SetKItchenObjectParent(this);
                _cuttingProgress = 0;
                
                var cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObjects().GetKitchenObjectSO());

                OnCuttingProgressChanged?.Invoke(this,new OnProgressChangedEventArgs
                {
                    progressNormalized = (float)_cuttingProgress/cuttingRecipeSO.cuttingProgressMax
                });
            }
        }
        else
        {
            //counter has a kitchen object
            if (player.HasKitchenObject())
            {
                //player has something
            }
            else
            {
                //player doesn't have something
                GetKitchenObjects().SetKItchenObjectParent(player);
            }
        }
    }


    //To cut the Kitchen Objects
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObjects().GetKitchenObjectSO()))
        {
            _cuttingProgress++;
            //this has a kitchen gameObject and it can be cut
            var cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObjects().GetKitchenObjectSO());
            
            OnCuttingProgressChanged?.Invoke(this,new OnProgressChangedEventArgs
            {
                progressNormalized = (float)_cuttingProgress/cuttingRecipeSO.cuttingProgressMax
            });
            
            OnCut?.Invoke(this,EventArgs.Empty);
            
            if (_cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectsSO outputKitchenObjectsSO = OutputForInput(GetKitchenObjects().GetKitchenObjectSO());

                GetKitchenObjects().DestroySelf();

                KitchenObjects.SpawnKitchenObject(outputKitchenObjectsSO, this);
            }
        }
    }


    //To return the sliced KitchenObjects
    private KitchenObjectsSO OutputForInput(KitchenObjectsSO inputKitchenObjectSO)
    {
        var cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null) return cuttingRecipeSO.output;

        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectsSO kitchenObjectsSo)
    {
        var cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectsSo);

        return cuttingRecipeSO != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectsSO inputKitchenObjectSO)
    {
        foreach (var cuttingRecipeSo in cutKitchenObjectsSOArray)
        {
            if (cuttingRecipeSo.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSo;
            }
        }

        return null;
    }
}