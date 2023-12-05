using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class PlateIconTemplateUI : MonoBehaviour
{
   [SerializeField] private Image image;

   public void SetKItchenObjectSO(KitchenObjectsSO kitchenObjectsSO)
   {
      image.sprite = kitchenObjectsSO.objectSprite;
   }
}
