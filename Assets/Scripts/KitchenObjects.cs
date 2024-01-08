using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObjects : NetworkBehaviour
{

    [SerializeField]
    private KitchenObjectsSO kitchenObjectsSO;

    private IkitchenObjectParent kitchenObjectParent;

    private FollowTransform followTransform;

    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }

    public KitchenObjectsSO GetKitchenObject ()
    {
        return kitchenObjectsSO;
    }

    public void SetKItchenObjectParent(IkitchenObjectParent kitchenObjectParent)
    {
       SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
       SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IkitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IkitchenObjectParent>();
        
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;
        kitchenObjectParent.SetKitchenObject(this);

        followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());
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
        Destroy(gameObject);
    }

    public void ClearKitchenObjectOnParent()
    {
        kitchenObjectParent.ClearKitchenObject();
    }

    public static void SpawnKitchenObject(KitchenObjectsSO kitchenObjectsSo, IkitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectsSo,kitchenObjectParent);
        
    }

    public static void DestroyKitchenObject(KitchenObjects kitchenObjects)
    {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObjects);
    }
    
}
