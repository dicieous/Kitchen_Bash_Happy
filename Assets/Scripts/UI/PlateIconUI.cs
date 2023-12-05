using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
   [SerializeField] private PlateKitchenObject _plateKitchenObject;

   [SerializeField] private Transform _iconTemplate;


   private void Awake()
   {
      _iconTemplate.gameObject.SetActive(false);
   }

   private void Start()
   {
      _plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnIngredientAdded;
   }

   private void PlateKitchenObjectOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
   {
      UpdateVisual();
   }

   private void UpdateVisual()
   {
      foreach (Transform child in transform)
      {
         if(child == _iconTemplate) continue;
         Destroy(child.gameObject);
      }
      
      foreach (KitchenObjectsSO kitchenObjectsSO in _plateKitchenObject.GetkitKitchenObjectsSOList())
      {
         Transform iconTransform = Instantiate(_iconTemplate, transform);
         
         iconTransform.gameObject.SetActive(true);
         iconTransform.GetComponent<PlateIconTemplateUI>().SetKItchenObjectSO(kitchenObjectsSO);
      }
   }
}
