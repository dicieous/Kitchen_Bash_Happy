using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{

   private const string PLAYERPREFS_SOUND_EFFFECT_VOLUME = "SoundEffectVolume";
   
   public static SoundManager Instance { get; private set; }
   
   [SerializeField] private AudioClipRefSO _audioClipRefSO;

   private float volume = 1f;

   private void Awake()
   {
      Instance = this;

      volume  = PlayerPrefs.GetFloat(PLAYERPREFS_SOUND_EFFFECT_VOLUME, 1f);
   }

   private void Start()
   {
      DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
      DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
      CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
      Player.OnAnyPlayerPickSomething += Player_OnPickingSomething;
      BaseCounter.OnAnythingPlacedHere += BaseCounter_OnAnythingPlacedHere;
      TrashCounter.OnTrashed += TrashCounter_OnTrashed;
   }

   private void TrashCounter_OnTrashed(object sender, EventArgs e)
   {
      var trashCounter = sender as TrashCounter;
      PlaySound(_audioClipRefSO.trash, trashCounter.transform.position);
   }

   private void BaseCounter_OnAnythingPlacedHere(object sender, EventArgs e)
   {
      BaseCounter baseCounter = sender as BaseCounter;
      PlaySound(_audioClipRefSO.objectDrop, baseCounter.transform.position);
   }

   private void Player_OnPickingSomething(object sender, EventArgs e)
   {
      Player player = sender as Player;
      PlaySound(_audioClipRefSO.objectPickup,player.transform.position);
   }

   private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
   {
      var cuttingCounter = sender as CuttingCounter;
      PlaySound(_audioClipRefSO.chop, cuttingCounter.transform.position);
   }

   private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
   {
      DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
      if (Camera.main != null) PlaySound(_audioClipRefSO.deliverySuccess, deliveryCounter.transform.position);
   }

   private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
   {
      DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
      if (Camera.main != null) PlaySound(_audioClipRefSO.deliveryFail, deliveryCounter.transform.position);
   }

   private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
   {
      AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0, audioClipArray.Length)], position,
         volumeMultiplier * volume);
   }
   
   private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
   {
      AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
   }

   public void PlayFootStepsSound(Vector3 position)
   {
      PlaySound(_audioClipRefSO.footStep, position);
   }
   
   public void PlayNumberPopUpSound()
   {
      PlaySound(_audioClipRefSO.warning, Vector3.zero);
   }
   
   public void PlayBurningWarningSound(Vector3 position)
   {
      PlaySound(_audioClipRefSO.warning, position);
   }

   public void ChangeVolume()
   {
      volume += 0.1f;

      if (volume > 1f)
      {
         volume = 0f;
      }
      PlayerPrefs.SetFloat(PLAYERPREFS_SOUND_EFFFECT_VOLUME,volume);
      PlayerPrefs.Save();
   }

   public float GetVolume()
   {
      return volume;
   }
}
