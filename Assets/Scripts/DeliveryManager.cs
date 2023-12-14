using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;

    private float recipeSpawnTimer;
    private float recipeSpawnTimerMax = 4f;
    private int maxRecipeCount = 4;


    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        recipeSpawnTimer -= Time.deltaTime;
        if (recipeSpawnTimer <= 0f)
        {
            recipeSpawnTimer = recipeSpawnTimerMax;

            if (waitingRecipeSOList.Count < maxRecipeCount)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                
                waitingRecipeSOList.Add(waitingRecipeSO);
                
                OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipes(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            bool plateContentMatchesRecipe = false;
            
            if (waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObject.GetkitKitchenObjectsSOList().Count)
            {
                //Has same number of ingredients
                foreach (var recipeKitchenObjectsSO in waitingRecipeSO.kitchenObjectsSOList)
                {
                    plateContentMatchesRecipe =
                        plateKitchenObject.GetkitKitchenObjectsSOList().Contains(recipeKitchenObjectsSO);
                    if (!plateContentMatchesRecipe)
                    {
                        //doesn't have the same Ingredient
                        Debug.Log("Wrong Ingredient Found");
                        break;
                    }
                }
            }

            if (plateContentMatchesRecipe)
            {
                //player provided correct Recipe
                Debug.Log("Player Provided correct Recipe!!");
                
                waitingRecipeSOList.RemoveAt(i);
                
                OnRecipeCompleted?.Invoke(this,EventArgs.Empty);
                return;
            }

            //player didn't deliver the correct recipe
            Debug.Log("Player did not give the correct Recipe");
        }
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    
}