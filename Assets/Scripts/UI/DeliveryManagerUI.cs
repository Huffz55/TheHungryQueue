using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{

    // UI elemanlar�:
    // - container: T�m teslimat tariflerinin yerle�tirilece�i ana konteyner
    // - recipeTemplate: Her bir tarifi temsil eden �ablon (prefab)
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    // Awake metodunda recipeTemplate �ablonunun g�r�nmesini engelliyoruz,
    // ��nk� sadece prefab olarak kullan�lacak.
    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    // Start metodunda DeliveryManager'dan gelen olaylara abone oluyoruz
    // ve UI'y� g�ncellemek i�in UpdateVisual() metodunu �a��r�yoruz.
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;   // Yeni bir tarif olu�turuldu�unda
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted; // Tarif tamamland���nda

        // Ba�lang��ta UI'yi g�ncelliyoruz.
        UpdateVisual();
    }

    // Tarif tamamland���nda UI'yi g�ncelleyen metod
    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    // Yeni bir tarif olu�turuldu�unda UI'yi g�ncelleyen metod
    private void DeliveryManager_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    // UI elemanlar�n� g�ncelleyen metod
    // 1. �ncelikle mevcut t�m ��eleri (�ablon hari�) temizliyoruz.
    // 2. DeliveryManager'dan gelen tarifleri al�p UI'ye ekliyoruz.
    private void UpdateVisual()
    {
        // 1. �nceden olu�turulmu� t�m ��eleri temizliyoruz.
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue; // �ablonu ge�iyoruz
            Destroy(child.gameObject);  // Di�er t�m ��eleri yok ediyoruz.
        }

        // 2. DeliveryManager'dan gelen tarifleri UI'ye ekliyoruz.
        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            // �ablon tarifin kopyas�n� olu�turuyoruz
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true); // ��eyi g�r�n�r yap�yoruz.

            // DeliveryManagerSingleUI bile�enine tarif verilerini g�nderiyoruz.
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
