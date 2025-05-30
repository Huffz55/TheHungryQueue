using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mutfak nesnelerini taþýyabilen (veya tutabilen) nesneler için kullanýlan arayüz
public interface IKitchenObjectParent
{

    // Mutfak nesnesinin bu nesneye göre hizalanacaðý (takip edeceði) transform'u döndürür
    public Transform GetKitchenObjectFollowTransform();

    // Bu nesneye bir KitchenObject atar
    public void SetKitchenObject(KitchenObject kitchenObject);

    // Bu nesnenin sahip olduðu KitchenObject'i döndürür
    public KitchenObject GetKitchenObject();

    // Bu nesnedeki KitchenObject'i temizler (kaldýrýr)
    public void ClearKitchenObject();

    // Bu nesne þu anda bir KitchenObject taþýyor mu? (true/false)
    public bool HasKitchenObject();
}
