using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mutfak nesnelerini ta��yabilen (veya tutabilen) nesneler i�in kullan�lan aray�z
public interface IKitchenObjectParent
{

    // Mutfak nesnesinin bu nesneye g�re hizalanaca�� (takip edece�i) transform'u d�nd�r�r
    public Transform GetKitchenObjectFollowTransform();

    // Bu nesneye bir KitchenObject atar
    public void SetKitchenObject(KitchenObject kitchenObject);

    // Bu nesnenin sahip oldu�u KitchenObject'i d�nd�r�r
    public KitchenObject GetKitchenObject();

    // Bu nesnedeki KitchenObject'i temizler (kald�r�r)
    public void ClearKitchenObject();

    // Bu nesne �u anda bir KitchenObject ta��yor mu? (true/false)
    public bool HasKitchenObject();
}
