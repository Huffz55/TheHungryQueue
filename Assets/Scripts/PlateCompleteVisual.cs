using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{

    // Ýç içe bir struct, bir KitchenObjectSO ile ona karþýlýk gelen GameObject arasýnda iliþki kurar
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;  // Bu, yemeðin içeriðini temsil eder
        public GameObject gameObject;            // Bu, içeriði temsil eden 3D model veya görsel objedir
    }

    // PlateKitchenObject, tabaða eklenen içerikleri tutar
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    // Her KitchenObjectSO için iliþkili GameObject'leri tutan liste
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    // Baþlangýçta, PlateKitchenObject'a ekleme yapýlmasýný dinlemek için event abonesi oluruz
    private void Start()
    {
        // PlateKitchenObject'a ingredient eklendiðinde çalýþacak event handler
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        // Tüm GameObject'ler baþlangýçta devre dýþý býrakýlýr (görünmez yapýlýr)
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    // PlateKitchenObject'a yeni bir ingredient eklendiðinde çaðrýlýr
    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        // Eklenecek ingredient'e karþýlýk gelen GameObject'i aktif hale getirir
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }

}
