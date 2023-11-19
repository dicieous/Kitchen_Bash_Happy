using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particleGameObject;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounterOnStateChanged;
    }

    private void StoveCounterOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisuals = e.State is StoveCounter.State.Frying or StoveCounter.State.Fried;
        
        stoveOnGameObject.SetActive(showVisuals);
        particleGameObject.SetActive(showVisuals);
    }
}