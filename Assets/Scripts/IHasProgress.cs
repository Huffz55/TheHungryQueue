using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bir ilerleme g�sterebilen nesneler i�in kullan�lan aray�z
public interface IHasProgress
{

    // Bu event, ilerleme durumu de�i�ti�inde tetiklenir
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    // Event'e g�nderilecek verileri i�eren s�n�f
    public class OnProgressChangedEventArgs : EventArgs
    {
        // 0 ile 1 aras�nda normalize edilmi� ilerleme de�eri (�rnek: %75 ilerleme i�in 0.75)
        public float progressNormalized;
    }
}
