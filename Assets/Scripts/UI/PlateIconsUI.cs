using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{

    // PlateKitchenObject'a referans, taba�a eklenen malzemeleri y�netir
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    // Ikonlar� g�stermek i�in kullan�lan �ablon
    [SerializeField] private Transform iconTemplate;

    // Ba�lang��ta iconTemplate'in g�r�nmemesini sa�la
    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    // Ba�lang��ta PlateKitchenObject'teki malzeme eklenme olay�n� dinlemeye ba�la
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    // PlateKitchenObject'a malzeme eklendi�inde tetiklenen olay fonksiyonu
    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();  // G�rseli g�ncelle
    }

    // G�rselleri (ikonlar�) g�ncelleyen fonksiyon
    private void UpdateVisual()
    {
        // Mevcut t�m �ocuk nesnelerini sil
        foreach (Transform child in transform)
        {
            // iconTemplate'i hari� tutarak di�er t�m �ocuklar� sil
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // PlateKitchenObject i�indeki t�m KitchenObjectSO'lar i�in ikonlar� olu�tur
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            // Yeni bir ikon nesnesi olu�tur
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);  // �konu aktif hale getir
            // Yeni olu�turulan ikona ilgili KitchenObjectSO'yu ayarla
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }

}
