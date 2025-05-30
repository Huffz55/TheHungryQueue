using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    // Çöp kutusuna herhangi bir nesne atýldýðýnda tetiklenen olay
    public static event EventHandler OnAnyObjectTrashed;

    // Statik veriyi sýfýrlamak için kullanýlýr
    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null; // Olayý sýfýrlamak, abone olan tüm dinleyicileri temizler
    }

    // Player, çöp kutusuyla etkileþimde bulunduðunda çalýþýr
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            // Player'ýn taþýdýðý bir nesne varsa
            player.GetKitchenObject().DestroySelf(); // Nesneyi yok eder

            // Nesne çöp kutusuna atýldýðýnda olay tetiklenir
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
