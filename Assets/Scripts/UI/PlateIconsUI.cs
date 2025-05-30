using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{

    // PlateKitchenObject'a referans, tabaða eklenen malzemeleri yönetir
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    // Ikonlarý göstermek için kullanýlan þablon
    [SerializeField] private Transform iconTemplate;

    // Baþlangýçta iconTemplate'in görünmemesini saðla
    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    // Baþlangýçta PlateKitchenObject'teki malzeme eklenme olayýný dinlemeye baþla
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    // PlateKitchenObject'a malzeme eklendiðinde tetiklenen olay fonksiyonu
    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();  // Görseli güncelle
    }

    // Görselleri (ikonlarý) güncelleyen fonksiyon
    private void UpdateVisual()
    {
        // Mevcut tüm çocuk nesnelerini sil
        foreach (Transform child in transform)
        {
            // iconTemplate'i hariç tutarak diðer tüm çocuklarý sil
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // PlateKitchenObject içindeki tüm KitchenObjectSO'lar için ikonlarý oluþtur
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            // Yeni bir ikon nesnesi oluþtur
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);  // Ýkonu aktif hale getir
            // Yeni oluþturulan ikona ilgili KitchenObjectSO'yu ayarla
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }

}
