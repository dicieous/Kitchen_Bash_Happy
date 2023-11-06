using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisuals : MonoBehaviour
{
   private const string OPEN_CLOSE = "OpenClose";

   [SerializeField] private ContainerCounter _containerCounter;
   
   private Animator _animator;
   
   private void Awake()
   {
      _animator = GetComponent<Animator>();
   }

   private void Start()
   {
      _containerCounter.OnPlayerGrabObject += ContainerCounterOnOnPlayerGrabObject;
   }

   private void ContainerCounterOnOnPlayerGrabObject(object sender, EventArgs e)
   {
      _animator.SetTrigger(OPEN_CLOSE);
   }
}
