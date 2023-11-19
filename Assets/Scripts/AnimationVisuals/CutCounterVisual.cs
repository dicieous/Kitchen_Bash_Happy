using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutCounterVisual : MonoBehaviour
{
   private const string CUT = "Cut";

   [SerializeField] private CuttingCounter _cuttingCounter;
   
   private Animator _animator;
   
   private void Awake()
   {
      _animator = GetComponent<Animator>();
   }

   private void Start()
   {
      _cuttingCounter.OnCut += CuttingCounterOnCuttingProgressChanged;
   }

   private void CuttingCounterOnCuttingProgressChanged(object sender, EventArgs e)
   {
      _animator.SetTrigger(CUT);
   }
}
