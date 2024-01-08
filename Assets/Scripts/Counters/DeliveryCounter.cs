using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            //player has something
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //player has plate
                DeliveryManager.Instance.DeliverRecipes(plateKitchenObject);

                KitchenObjects.DestroyKitchenObject(player.GetKitchenObject());
            }
        }
    }
}