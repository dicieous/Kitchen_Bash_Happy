using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   
   public static GameManager Instance { get; private set; }

   public event EventHandler OnStateChanged;
   public event EventHandler OnGamePaused;
   public event EventHandler OnGameUnpaused;
   
   private enum State
   {
      WaitingToStart,
      CountdownToStart,
      GamePlaying,
      GameOver,
   }

   private State _state;
   
   private float countdownToStartTimer = 1f;
   private float gamePlayingTimer;
   [SerializeField] private float gamePlayingTimerMax = 300f;

   private bool isGamePaused = false;

   private void Awake()
   {
      _state = State.WaitingToStart;
      Instance = this;
   }

   private void Start()
   {
      GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
      GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
      
      //DEBUG TRIGGER GAME START AUTOMATICALLY
      _state = State.CountdownToStart;
      OnStateChanged?.Invoke(this,EventArgs.Empty);
   }

   private void GameInput_OnInteractAction(object sender, EventArgs e)
   {
      if (_state == State.WaitingToStart)
      {
         _state = State.CountdownToStart;
         OnStateChanged?.Invoke(this,EventArgs.Empty);
      }
   }

   private void GameInput_OnPauseAction(object sender, EventArgs e)
   {
      TogglePauseGame();
   }
   
   private void Update()
   {
      switch (_state)
      {
         case State.WaitingToStart:
            
            break;
         case State.CountdownToStart:
            
            countdownToStartTimer -= Time.deltaTime;
            if (countdownToStartTimer < 0)
            {
               gamePlayingTimer = gamePlayingTimerMax;
               _state = State.GamePlaying;
               OnStateChanged?.Invoke(this,EventArgs.Empty);

            }
            
            break;
         case State.GamePlaying:
            
            gamePlayingTimer -= Time.deltaTime;
            if (gamePlayingTimer < 0)
            {
               _state = State.GameOver;
               OnStateChanged?.Invoke(this,EventArgs.Empty);

            }
            
            break;
         case State.GameOver:
            break;
      }
      
      //Debug.Log(_state);
   }

   public bool IsGamePlaying()
   {
      return _state == State.GamePlaying;
   }

   public bool IsCountDownToStartActive()
   {
      return _state == State.CountdownToStart;
   }

   public float GetCountDownToStartTimer()
   {
      return countdownToStartTimer;
   }
   
   public bool IsGameOverActive()
   {
      return _state == State.GameOver;
   }

   public float GetPlayingTimerNormalized()
   {
      return (1 - gamePlayingTimer / gamePlayingTimerMax);
   }
   
   public void TogglePauseGame()
   {
      isGamePaused = !isGamePaused;
      if (isGamePaused)
      {
         Time.timeScale = 0f;
         OnGamePaused?.Invoke(this, EventArgs.Empty);
      }
      else
      {
         Time.timeScale = 1f;
         OnGameUnpaused?.Invoke(this, EventArgs.Empty);

      }
   }

}
