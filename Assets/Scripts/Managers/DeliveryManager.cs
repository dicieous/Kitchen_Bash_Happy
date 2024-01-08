using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;

    private float recipeSpawnTimer = 4f;
    private float recipeSpawnTimerMax = 3f;
    private int maxRecipeCount = 4;
    public int successfulRecipeCount { get; private set; }


    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        if(!IsServer) return;
        
        recipeSpawnTimer -= Time.deltaTime;
        if (recipeSpawnTimer <= 0f)
        {
            recipeSpawnTimer = recipeSpawnTimerMax;

            if (GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < maxRecipeCount)
            {
                int waitingRecipeSOIndex = Random.Range(0, recipeListSO.recipeSOList.Count);
                
                SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex);
            }
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[waitingRecipeSOIndex];
        waitingRecipeSOList.Add(waitingRecipeSO);

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        //Debug.Log(waitingRecipeSO.recipeName);
    }
    

    public void DeliverRecipes(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObject.GetkitKitchenObjectsSOList().Count)
            {
                bool plateContentMatchesRecipe = true;

                //Has same number of ingredients
                foreach (var recipeKitchenObjectsSO in waitingRecipeSO.kitchenObjectsSOList)
                {
                    //Cycle through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectsSO platekitchenObjectsSO in plateKitchenObject.GetkitKitchenObjectsSOList())
                    {
                        if (platekitchenObjectsSO == recipeKitchenObjectsSO)
                        {
                            //ingredients match
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        //This recipe Ingredients were not found on the plate
                        plateContentMatchesRecipe = false;
                    }
                }

                if (plateContentMatchesRecipe)
                {
                    //player delivered the correct recipe
                    
                    DeliveredCorrectRecipeServerRpc(i);
                    
                    return;
                }
            }
        }

        //No match found
        //player did not deliver a correct recipe
        
        DeliveredInCorrectRecipeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliveredCorrectRecipeServerRpc(int WaitingRecipeSOListIndex)
    {
        DeliveredCorrectRecipeClientRpc(WaitingRecipeSOListIndex);
    }

    [ClientRpc]
    private void DeliveredCorrectRecipeClientRpc(int WaitingRecipeSOListIndex)
    {
        waitingRecipeSOList.RemoveAt(WaitingRecipeSOListIndex);

        successfulRecipeCount++;
                    
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

    }


    [ServerRpc(RequireOwnership = false)]
    private void DeliveredInCorrectRecipeServerRpc()
    {
        DeliveredInCorrectRecipeClientRpc();
    }

    [ClientRpc]
    private void DeliveredInCorrectRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }
    
    
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
}