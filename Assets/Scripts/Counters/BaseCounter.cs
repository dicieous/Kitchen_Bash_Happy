using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IkitchenObjectParent
{
    [SerializeField] private Transform tableTopPoint;

    private KitchenObjects kitchenObject;

    
    public virtual void Interact(Player player){
        Debug.LogError("BaseCounter = Interact()");
    }
    
    public virtual void InteractAlternate(Player player){
        Debug.LogError("BaseCounter = InteractAlternate()");
    }
    
    public Transform GetKitchenObjectFollowTransform ()
    {
        return tableTopPoint;
    }

    public void SetKitchenObject (KitchenObjects kitchenObjects)
    {
        this.kitchenObject = kitchenObjects;
    }

    public KitchenObjects GetKitchenObject ()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject ()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject ()
    {
        return kitchenObject != null;
    }
}
