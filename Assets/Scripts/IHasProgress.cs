using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler <OnProgressChangedEventArgs> OnCuttingProgressChanged;
    
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

}
