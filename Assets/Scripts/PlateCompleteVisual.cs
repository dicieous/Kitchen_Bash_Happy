using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
   [Serializable]
   public struct KitchenObjectSO_Gameobject
   {
      public KitchenObjectsSO kitchenObjectsSO;
      public GameObject gameObject;
   }
   
   [SerializeField] private PlateKitchenObject _plateKitchenObject;

   [SerializeField] private List<KitchenObjectSO_Gameobject> KitchenObjectSOGameobjectsLIst;

   private void Start()
   {
      _plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnIngredientAdded;
      foreach (var kitchenObjectSOGameobject in KitchenObjectSOGameobjectsLIst)
      {
         kitchenObjectSOGameobject.gameObject.SetActive(false);
      }
   }

   private void PlateKitchenObjectOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
   {
      foreach (var kitchenObjSOGameobject in KitchenObjectSOGameobjectsLIst)
      {
         if (kitchenObjSOGameobject.kitchenObjectsSO == e.KitchenObjectsSO)
         {
            kitchenObjSOGameobject.gameObject.SetActive(true);
         }
      }
   }
}
