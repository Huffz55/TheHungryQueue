using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{

    // �� i�e bir struct, bir KitchenObjectSO ile ona kar��l�k gelen GameObject aras�nda ili�ki kurar
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;  // Bu, yeme�in i�eri�ini temsil eder
        public GameObject gameObject;            // Bu, i�eri�i temsil eden 3D model veya g�rsel objedir
    }

    // PlateKitchenObject, taba�a eklenen i�erikleri tutar
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    // Her KitchenObjectSO i�in ili�kili GameObject'leri tutan liste
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    // Ba�lang��ta, PlateKitchenObject'a ekleme yap�lmas�n� dinlemek i�in event abonesi oluruz
    private void Start()
    {
        // PlateKitchenObject'a ingredient eklendi�inde �al��acak event handler
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        // T�m GameObject'ler ba�lang��ta devre d��� b�rak�l�r (g�r�nmez yap�l�r)
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    // PlateKitchenObject'a yeni bir ingredient eklendi�inde �a�r�l�r
    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        // Eklenecek ingredient'e kar��l�k gelen GameObject'i aktif hale getirir
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }

}
