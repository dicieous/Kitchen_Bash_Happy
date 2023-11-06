using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjects : MonoBehaviour
{

    [SerializeField]
    private KitchenObjectsSO kitchenObjectsSO;

    private IkitchenObjectParent kitchenObjectParent;
   
    public KitchenObjectsSO GetKitchenObject ()
    {
        return kitchenObjectsSO;
    }

    public void SetKItchenObjectParent(IkitchenObjectParent kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IkitchenObjectParent GetKitchenObjectParent ()
    {
        return kitchenObjectParent;
    }
}
