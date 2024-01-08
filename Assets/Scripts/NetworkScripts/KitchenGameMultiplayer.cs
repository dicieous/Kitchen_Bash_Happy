using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{
   public static KitchenGameMultiplayer Instance { get; private set; }

   [SerializeField] private KitchenObjectsListSO KitchenObjectsListSO;

   private void Awake()
   {
      Instance = this;
   }
   
   public void SpawnKitchenObject(KitchenObjectsSO kitchenObjectsSo, IkitchenObjectParent kitchenObjectParent)
   {
      SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(kitchenObjectsSo),kitchenObjectParent.GetNetworkObject());
   }

   [ServerRpc(RequireOwnership = false)]
   private void SpawnKitchenObjectServerRpc(int kitchenObjectsIndex, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
   {
      KitchenObjectsSO kitchenObjectsSO = GetKitchenObjectsSOFromIndex(kitchenObjectsIndex);
      
      Transform kitchenObjectTransform = Instantiate(kitchenObjectsSO.prefabs);

      var networkKitchenObject = kitchenObjectTransform.GetComponent<NetworkObject>();
      networkKitchenObject.Spawn(true);
      
      var kitchenObject = kitchenObjectTransform.GetComponent<KitchenObjects>();

      kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
      IkitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IkitchenObjectParent>();
      kitchenObject.SetKItchenObjectParent(kitchenObjectParent);
   }

   private int GetKitchenObjectSOIndex(KitchenObjectsSO kitchenObjectsSO)
   {
      return KitchenObjectsListSO.KitchenObjectsSOList.IndexOf(kitchenObjectsSO);
   }

   private KitchenObjectsSO GetKitchenObjectsSOFromIndex(int kitchenObjectsSOIndex)
   {
      return KitchenObjectsListSO.KitchenObjectsSOList[kitchenObjectsSOIndex];
   }

   public void DestroyKitchenObject(KitchenObjects kitchenObjects)
   {
      DestroyKitchenObjectServerRpc(kitchenObjects.NetworkObject);
   }

   [ServerRpc(RequireOwnership = false)]
   private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
   {
      kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
      var kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObjects>();
      
      ClearKitchenObjectParentClientRpc(kitchenObjectNetworkObjectReference);
      kitchenObject.DestroySelf();
   }

   [ClientRpc]
   private void ClearKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
   {
      kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
      var kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObjects>();
      
      kitchenObject.ClearKitchenObjectOnParent();
   }
}
