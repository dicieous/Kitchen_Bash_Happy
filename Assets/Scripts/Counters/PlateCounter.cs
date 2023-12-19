using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectsSO plateKitchenObjectSO;

    private float plateSpawnTimer;
    private float plateSpawnTimerMax = 4f;

    private int spawnedPlatesCount;
    private int spawwnedPlatesCountMax = 4;

    private void Update()
    {
        plateSpawnTimer += Time.deltaTime;

        if (plateSpawnTimer > plateSpawnTimerMax)
        {
            plateSpawnTimer = 0f;
            if (GameManager.Instance.IsGamePlaying() && spawnedPlatesCount < spawwnedPlatesCountMax)
            {
                spawnedPlatesCount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //player is empty Handed
            if (spawnedPlatesCount > 0)
            {
                //There is plate on the counter
                spawnedPlatesCount--;

                KitchenObjects.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}