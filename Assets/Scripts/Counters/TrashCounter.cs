using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    // ��p kutusuna herhangi bir nesne at�ld���nda tetiklenen olay
    public static event EventHandler OnAnyObjectTrashed;

    // Statik veriyi s�f�rlamak i�in kullan�l�r
    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null; // Olay� s�f�rlamak, abone olan t�m dinleyicileri temizler
    }

    // Player, ��p kutusuyla etkile�imde bulundu�unda �al���r
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            // Player'�n ta��d��� bir nesne varsa
            player.GetKitchenObject().DestroySelf(); // Nesneyi yok eder

            // Nesne ��p kutusuna at�ld���nda olay tetiklenir
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
