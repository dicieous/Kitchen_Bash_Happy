using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : MonoBehaviour,IkitchenObjectParent
{
    [SerializeField]
    private KitchenObjectsSO kitchenObjectSO;
    [SerializeField]
    private Transform tableTopPoint;

    private KitchenObjects kitchenObject;


    public void Interact (Player player)
    {
        if (kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefabs, tableTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObjects>().SetKItchenObjectParent(this);
        } else
        {
            //Give Object to the player
            kitchenObject.SetKItchenObjectParent(player);
        }
    }

    public Transform GetKitchenObjectFollowTransform ()
    {
        return tableTopPoint;
    }

    public void SetKitchenObject (KitchenObjects kitchenObjects)
    {
        this.kitchenObject = kitchenObjects;
    }

    public KitchenObjects GetKitchenObjects ()
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
