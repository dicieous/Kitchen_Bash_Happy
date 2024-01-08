using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    public static event EventHandler OnTrashed;
    
    public new static void ResetStaticData()
    {
        OnTrashed = null;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObjects.DestroyKitchenObject(player.GetKitchenObject());
            
            InteractLogicServerRpc();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnTrashed?.Invoke(this, EventArgs.Empty);
    }
    
}
