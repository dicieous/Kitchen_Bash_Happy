using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start()
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnselectedCounterChanged += Player_OnselectedCounterChanged;
        }
        else
        {
            Player.OnAnyPlayerSpawned += OnAnyPlayerSpawned;
        }
    }

    private void OnAnyPlayerSpawned(object sender, EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnselectedCounterChanged -= Player_OnselectedCounterChanged;
            Player.LocalInstance.OnselectedCounterChanged += Player_OnselectedCounterChanged;
        }
    }

    private void Player_OnselectedCounterChanged(object sender, Player.OnselectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (var visualObj in visualGameObjectArray)
        {
            visualObj.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (var visualObj in visualGameObjectArray)
        {
            visualObj.SetActive(false);
        }
    }
}