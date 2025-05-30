using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{

    // Bu event, oyuncunun tezgah üzerinden bir nesneyi almasý durumunda tetiklenir
    public event EventHandler OnPlayerGrabbedObject;

    // Bu, bu tezgahýn vereceði mutfak nesnesinin tipini belirtir
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Bu metod, oyuncunun tezgah ile etkileþime girmesini saðlar
    public override void Interact(Player player)
    {
        // Eðer oyuncu hiç nesne taþýmýyorsa
        if (!player.HasKitchenObject())
        {
            // Oyuncu bir þey taþýmýyorsa, bu tezgahýn mutfak nesnesini alabilir
            // Mutfak nesnesini oyuncuya spawn eder
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            // OnPlayerGrabbedObject event'ini tetikler
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
