using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bir ilerleme gösterebilen nesneler için kullanýlan arayüz
public interface IHasProgress
{

    // Bu event, ilerleme durumu deðiþtiðinde tetiklenir
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    // Event'e gönderilecek verileri içeren sýnýf
    public class OnProgressChangedEventArgs : EventArgs
    {
        // 0 ile 1 arasýnda normalize edilmiþ ilerleme deðeri (örnek: %75 ilerleme için 0.75)
        public float progressNormalized;
    }
}
