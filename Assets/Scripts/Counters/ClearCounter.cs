using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ClearCounter : BaseCounter
{
    [SerializeField]
    private KitchenObjectsSO kitchenObjectSO;

    public override void Interact (Player player)
    {
        if (!HasKitchenObject())
        {
            //counter doesn't have a Object on it
            if (player.HasKitchenObject())
            {
                //player has kitchen object
                player.GetKitchenObjects().SetKItchenObjectParent(this);
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
}
