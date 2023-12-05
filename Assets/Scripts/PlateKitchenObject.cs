using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObjects
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO KitchenObjectsSO;
    }

    [SerializeField] List<KitchenObjectsSO> validKitchenObjectOnPlateList;

    private List<KitchenObjectsSO> kitchenObjectsSOList;

    private void Awake()
    {
        kitchenObjectsSOList = new List<KitchenObjectsSO>();
    }

    public bool TryAddIngredient(KitchenObjectsSO kitchenObjectsSO)
    {
        if (!validKitchenObjectOnPlateList.Contains(kitchenObjectsSO))
        {
            //Not a valid Ingredient
            return false;
        }
        
        if(kitchenObjectsSOList.Contains(kitchenObjectsSO))
        {
            //already has this type
            return false;
        }
        else
        {
            kitchenObjectsSOList.Add(kitchenObjectsSO);
            
            OnIngredientAdded?.Invoke(this,new OnIngredientAddedEventArgs
            {
                KitchenObjectsSO = kitchenObjectsSO
            });
            
            return true;
        }
    }

    public List<KitchenObjectsSO> GetkitKitchenObjectsSOList()
    {
        return kitchenObjectsSOList;
    }
}