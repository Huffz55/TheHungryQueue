using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{

    // UI elemanlar�n� tan�ml�yoruz: 
    // - Recipe ad� i�in TextMeshProUGUI
    // - Iconlar� tutacak konteyner Transform
    // - Icon �ablonunu tutacak Transform
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    // Awake metodunda �ablon iconu devre d��� b�rak�yoruz ��nk� bu sadece bir template.
    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    // SetRecipeSO metodu, bir yemek tarifi (RecipeSO) al�r ve UI'deki ilgili bilgileri g�nceller.
    // 1. Recipe ad� g�ncellenir.
    // 2. Mevcut iconlar silinir.
    // 3. Yeni tarife ait iconlar eklenir.
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        // 1. Tarife ait ismi UI'ya yans�t�r�z.
        recipeNameText.text = recipeSO.recipeName;

        // 2. Daha �nce olu�turulmu� iconlar� temizleriz (iconTemplate hari�).
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;  // �ablon iconu ge�iyoruz.
            Destroy(child.gameObject);  // Di�er t�m iconlar� yok ederiz.
        }

        // 3. Yeni tarife ait her KitchenObjectSO i�in icon ekleriz.
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            // �ablonu �o�alt�yoruz ve container'a ekliyoruz.
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);  // Iconu aktif hale getiriyoruz.

            // �lgili iconun sprite'�n� tarife ait KitchenObjectSO'dan al�yoruz.
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
