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
    
    public KitchenObjectsSO GetKitchenObjectSO ()
    {
        return kitchenObjectsSO;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
      
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public void DestroySelf()
    {
        //Destroy this gameobject
        
        kitchenObjectParent.ClearKitchenObject();
        
        Destroy(gameObject);
    }

    public static KitchenObjects SpawnKitchenObject(KitchenObjectsSO kitchenObjectsSo, IkitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectsSo.prefabs);
        var kitchenObject = kitchenObjectTransform.GetComponent<KitchenObjects>();
        kitchenObject.SetKItchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
}
