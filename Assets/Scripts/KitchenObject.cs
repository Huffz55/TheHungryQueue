using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// KitchenObject: Oyun içindeki mutfak nesnelerini temsil eden sýnýf
public class KitchenObject : MonoBehaviour
{

    // Bu nesnenin ScriptableObject verisi (örneðin tipi, prefab'ý vs.)
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Bu nesnenin þu anki sahibi (örneðin tezgah, oyuncu gibi bir nesne)
    private IKitchenObjectParent kitchenObjectParent;

    // Bu KitchenObject’e ait KitchenObjectSO verisini döner
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    // KitchenObject’e yeni bir sahip atar
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        // Önceki sahibe ait nesne referansý temizlenir
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        // Yeni sahibe zaten bir nesne atanmýþsa hata ver
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent already has a KitchenObject!");
        }

        // Yeni sahibin KitchenObject referansý olarak bu nesne atanýr
        kitchenObjectParent.SetKitchenObject(this);

        // KitchenObject, sahibin nesnesine fiziksel olarak yerleþtirilir
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    // Bu KitchenObject’in þu anki sahibini döner
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    // Bu KitchenObject’i oyundan siler ve sahibin referansýný temizler
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    // Bu nesne bir tabak mý? Deðilse null döner, PlateKitchenObject olarak cast eder
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    // Yeni bir KitchenObject üretir ve belirli bir sahibi olur
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        // ScriptableObject’in prefab'ýný sahneye instantiate eder
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        // Üretilen prefab’tan KitchenObject bileþenini al
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        // Yeni KitchenObject’in sahibini belirle
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
}
