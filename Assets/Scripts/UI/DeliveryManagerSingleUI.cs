using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{

    // UI elemanlarýný tanýmlýyoruz: 
    // - Recipe adý için TextMeshProUGUI
    // - Iconlarý tutacak konteyner Transform
    // - Icon þablonunu tutacak Transform
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    // Awake metodunda þablon iconu devre dýþý býrakýyoruz çünkü bu sadece bir template.
    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    // SetRecipeSO metodu, bir yemek tarifi (RecipeSO) alýr ve UI'deki ilgili bilgileri günceller.
    // 1. Recipe adý güncellenir.
    // 2. Mevcut iconlar silinir.
    // 3. Yeni tarife ait iconlar eklenir.
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        // 1. Tarife ait ismi UI'ya yansýtýrýz.
        recipeNameText.text = recipeSO.recipeName;

        // 2. Daha önce oluþturulmuþ iconlarý temizleriz (iconTemplate hariç).
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;  // Þablon iconu geçiyoruz.
            Destroy(child.gameObject);  // Diðer tüm iconlarý yok ederiz.
        }

        // 3. Yeni tarife ait her KitchenObjectSO için icon ekleriz.
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            // Þablonu çoðaltýyoruz ve container'a ekliyoruz.
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);  // Iconu aktif hale getiriyoruz.

            // Ýlgili iconun sprite'ýný tarife ait KitchenObjectSO'dan alýyoruz.
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
