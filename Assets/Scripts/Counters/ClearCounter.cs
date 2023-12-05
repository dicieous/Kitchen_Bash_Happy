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
                player.GetKitchenObject().SetKItchenObjectParent(this);
            }
        }
        else
        {
            //counter has a kitchen object
            if (player.HasKitchenObject())
            {
                //player has something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player has plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //player is not carrying plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //counter has plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                //player doesn't have something
                GetKitchenObject().SetKItchenObjectParent(player);
            }
        }
    }
}
