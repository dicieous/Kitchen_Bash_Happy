using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;

    private AudioSource _audioSource;

    private float burningWarningSoundTimer;
    private bool playBurningSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        playBurningSound = _stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.State is StoveCounter.State.Frying or StoveCounter.State.Fried;

        if (playSound)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Pause();
        }
    }

    private void Update()
    {
        if (playBurningSound)
        {
            burningWarningSoundTimer -= Time.deltaTime;
            if (burningWarningSoundTimer <= 0)
            {
                float burningWarningSoundTimerMax = .2f;
                burningWarningSoundTimer = burningWarningSoundTimerMax;
                
                SoundManager.Instance.PlayBurningWarningSound(_stoveCounter.transform.position);
            }
        }
    }
}
