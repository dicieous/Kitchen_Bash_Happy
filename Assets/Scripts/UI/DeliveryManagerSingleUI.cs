using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    
    [SerializeField] private Transform ingredientContainer;
    [SerializeField] private Transform ingredientTemplate;

    private void Awake()
    {
        ingredientTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeName(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;

        foreach (Transform child in ingredientContainer)
        {
            if(child == ingredientTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (var kitchenObjectsSO in recipeSO.kitchenObjectsSOList)
        {
            Transform ingredientTransform = Instantiate(ingredientTemplate, ingredientContainer);
            ingredientTransform.gameObject.SetActive(true);
            ingredientTransform.GetComponent<Image>().sprite = kitchenObjectsSO.objectSprite;
        }
    }
}
