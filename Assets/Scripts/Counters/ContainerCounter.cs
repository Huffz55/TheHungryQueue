using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{

    // Bu event, oyuncunun tezgah �zerinden bir nesneyi almas� durumunda tetiklenir
    public event EventHandler OnPlayerGrabbedObject;

    // Bu, bu tezgah�n verece�i mutfak nesnesinin tipini belirtir
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Bu metod, oyuncunun tezgah ile etkile�ime girmesini sa�lar
    public override void Interact(Player player)
    {
        // E�er oyuncu hi� nesne ta��m�yorsa
        if (!player.HasKitchenObject())
        {
            // Oyuncu bir �ey ta��m�yorsa, bu tezgah�n mutfak nesnesini alabilir
            // Mutfak nesnesini oyuncuya spawn eder
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            // OnPlayerGrabbedObject event'ini tetikler
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
