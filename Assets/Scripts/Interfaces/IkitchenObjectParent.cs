using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IkitchenObjectParent
{
    public Transform GetKitchenObjectFollowTransform ();



    public void SetKitchenObject (KitchenObjects kitchenObjects);



    public KitchenObjects GetKitchenObject ();



    public void ClearKitchenObject ();


    public bool HasKitchenObject ();

    public NetworkObject GetNetworkObject();

}
