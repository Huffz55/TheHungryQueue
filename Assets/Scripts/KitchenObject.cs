using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// KitchenObject: Oyun i�indeki mutfak nesnelerini temsil eden s�n�f
public class KitchenObject : MonoBehaviour
{

    // Bu nesnenin ScriptableObject verisi (�rne�in tipi, prefab'� vs.)
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Bu nesnenin �u anki sahibi (�rne�in tezgah, oyuncu gibi bir nesne)
    private IKitchenObjectParent kitchenObjectParent;

    // Bu KitchenObject�e ait KitchenObjectSO verisini d�ner
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    // KitchenObject�e yeni bir sahip atar
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        // �nceki sahibe ait nesne referans� temizlenir
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        // Yeni sahibe zaten bir nesne atanm��sa hata ver
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent already has a KitchenObject!");
        }

        // Yeni sahibin KitchenObject referans� olarak bu nesne atan�r
        kitchenObjectParent.SetKitchenObject(this);

        // KitchenObject, sahibin nesnesine fiziksel olarak yerle�tirilir
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    // Bu KitchenObject�in �u anki sahibini d�ner
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    // Bu KitchenObject�i oyundan siler ve sahibin referans�n� temizler
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    // Bu nesne bir tabak m�? De�ilse null d�ner, PlateKitchenObject olarak cast eder
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

    // Yeni bir KitchenObject �retir ve belirli bir sahibi olur
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        // ScriptableObject�in prefab'�n� sahneye instantiate eder
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        // �retilen prefab�tan KitchenObject bile�enini al
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        // Yeni KitchenObject�in sahibini belirle
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
}
