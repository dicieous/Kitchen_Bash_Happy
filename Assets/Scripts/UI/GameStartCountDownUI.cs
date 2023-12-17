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
      countDownText.text = Mathf.Ceil(GameManager.Instance.GetCountDownToStartTimer()).ToString();
   }
}
