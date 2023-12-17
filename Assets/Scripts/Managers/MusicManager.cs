using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
   public const string PLAYERPREFS_MUSIC_VOLUME = "MusicVolume";
   public static MusicManager Instance { get; private set; }
   
   private AudioSource audioSource;

   private float volume = 0.3f;

   private void Awake()
   {
      Instance = this;
      
      audioSource = GetComponent<AudioSource>();

      volume = PlayerPrefs.GetFloat(PLAYERPREFS_MUSIC_VOLUME, .3f);
      audioSource.volume = volume;
   }


   public void ChangeVolume()
   {
      volume += 0.1f;

      if (volume > 1f)
      {
         volume = 0f;
      }
      
      audioSource.volume = volume;
      PlayerPrefs.SetFloat(PLAYERPREFS_MUSIC_VOLUME, volume);
   }

   public float GetVolume()
   {
      return volume;
   }
}
