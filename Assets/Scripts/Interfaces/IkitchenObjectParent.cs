using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IkitchenObjectParent
{
    public Transform GetKitchenObjectFollowTransform ();



    public void SetKitchenObject (KitchenObjects kitchenObjects);



    public KitchenObjects GetKitchenObject ();



    public void ClearKitchenObject ();


    public bool HasKitchenObject ();
    
}
