using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{

    // UI elemanlarý:
    // - container: Tüm teslimat tariflerinin yerleþtirileceði ana konteyner
    // - recipeTemplate: Her bir tarifi temsil eden þablon (prefab)
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    // Awake metodunda recipeTemplate þablonunun görünmesini engelliyoruz,
    // çünkü sadece prefab olarak kullanýlacak.
    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    // Start metodunda DeliveryManager'dan gelen olaylara abone oluyoruz
    // ve UI'yý güncellemek için UpdateVisual() metodunu çaðýrýyoruz.
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;   // Yeni bir tarif oluþturulduðunda
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted; // Tarif tamamlandýðýnda

        // Baþlangýçta UI'yi güncelliyoruz.
        UpdateVisual();
    }

    // Tarif tamamlandýðýnda UI'yi güncelleyen metod
    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    // Yeni bir tarif oluþturulduðunda UI'yi güncelleyen metod
    private void DeliveryManager_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    // UI elemanlarýný güncelleyen metod
    // 1. Öncelikle mevcut tüm öðeleri (þablon hariç) temizliyoruz.
    // 2. DeliveryManager'dan gelen tarifleri alýp UI'ye ekliyoruz.
    private void UpdateVisual()
    {
        // 1. Önceden oluþturulmuþ tüm öðeleri temizliyoruz.
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue; // Þablonu geçiyoruz
            Destroy(child.gameObject);  // Diðer tüm öðeleri yok ediyoruz.
        }

        // 2. DeliveryManager'dan gelen tarifleri UI'ye ekliyoruz.
        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            // Þablon tarifin kopyasýný oluþturuyoruz
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true); // Öðeyi görünür yapýyoruz.

            // DeliveryManagerSingleUI bileþenine tarif verilerini gönderiyoruz.
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
