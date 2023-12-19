using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
   [SerializeField] private Image backgroundImage;
   [SerializeField] private Image iconImage;
   
   [Space(10)]
   [SerializeField] private TextMeshProUGUI messageText;
   
   [Space(10)]
   [SerializeField] private Color successColor;
   [SerializeField] private Color failedColor;
   
   [Space(10)]
   [SerializeField] private Sprite successSprite;
   [SerializeField] private Sprite failedSprite;

   private Animator _animator;
   private const string POP_UP = "PopUp";

   private void Awake()
   {
      _animator = GetComponent<Animator>();
   }


   private void Start()
   {
      DeliveryManager.Instance.OnRecipeSuccess += DeliverManager_OnRecipeSuccess;
      DeliveryManager.Instance.OnRecipeFailed += DeliverManager_OnRecipeFailed;
      
      gameObject.SetActive(false);
   }

   private void DeliverManager_OnRecipeFailed(object sender, EventArgs e)
   {
      _animator.SetTrigger(POP_UP);
      gameObject.SetActive(true);
      
      backgroundImage.color = failedColor;
      iconImage.sprite = failedSprite;
      messageText.text = "DELIVERY\nFAILED";
   }

   private void DeliverManager_OnRecipeSuccess(object sender, EventArgs e)
   {
      _animator.SetTrigger(POP_UP);
      gameObject.SetActive(true);
      
      backgroundImage.color = successColor;
      iconImage.sprite = successSprite;
      messageText.text = "DELIVERY\nSUCCESS";
   }
}
