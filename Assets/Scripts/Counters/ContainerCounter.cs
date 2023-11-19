using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ContainerCounter : BaseCounter ,IkitchenObjectParent
{

    public event EventHandler OnPlayerGrabObject;
    
    [SerializeField] private KitchenObjectsSO kitchenObjectSO;


    public override void Interact (Player player)
    {
        if (!player.HasKitchenObject())
        {
            KitchenObjects.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
