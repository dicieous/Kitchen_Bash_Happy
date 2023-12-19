using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameStartCountDownUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI countDownText;

   private const string NUMBERPOPUP_TRIGGER_STRING = "NumberPopUp";

   private Animator _animator;

   private int previousCountdown;

   private void Awake()
   {
      _animator = GetComponent<Animator>();
   }

   private void Start()
   {
      GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
      Hide();
   }

   private void GameManager_OnStateChanged(object sender, EventArgs e)
   {
      if (GameManager.Instance.IsCountDownToStartActive())
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
      gameObject.SetActive(true);
   }

   private void Hide()
   { 
      gameObject.SetActive(false);
   }

   private void Update()
   { 
      int countDownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountDownToStartTimer());
      countDownText.text = countDownNumber.ToString();

      if (countDownNumber != previousCountdown)
      {
         previousCountdown = countDownNumber;
         _animator.SetTrigger(NUMBERPOPUP_TRIGGER_STRING);
         SoundManager.Instance.PlayNumberPopUpSound();
      }
   }
}
