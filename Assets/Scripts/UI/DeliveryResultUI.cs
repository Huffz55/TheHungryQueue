using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{

    // POPUP animasyonunun adý
    private const string POPUP = "Popup";

    // UI bileþenleri
    [SerializeField] private Image backgroundImage;      // Arka plan rengi için image
    [SerializeField] private Image iconImage;            // Baþarý ve baþarýsýzlýk durumlarý için simge
    [SerializeField] private TextMeshProUGUI messageText; // Mesajý göstermek için TextMeshPro
    [SerializeField] private Color successColor;         // Baþarý durumunda kullanýlacak renk
    [SerializeField] private Color failedColor;          // Baþarýsýzlýk durumunda kullanýlacak renk
    [SerializeField] private Sprite successSprite;       // Baþarý durumunda kullanýlacak simge
    [SerializeField] private Sprite failedSprite;        // Baþarýsýzlýk durumunda kullanýlacak simge

    private Animator animator; // Animasyon kontrolü için animator

    // Awake metodunda animator bileþeni alýnýyor.
    private void Awake()
    {
        animator = GetComponent<Animator>(); // Animator bileþenini alýyoruz.
    }

    // Start metodunda DeliveryManager'dan gelen olaylara abone oluyoruz
    // ve baþlangýçta UI'yi gizli tutuyoruz.
    private void Start()
    {
        // DeliveryManager'ýn olaylarýna abone oluyoruz
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        // UI baþlangýçta gizleniyor
        gameObject.SetActive(false);
    }

    // Tarif baþarýsýz olduðunda tetiklenen metod
    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true); // UI'yi görünür yapýyoruz
        animator.SetTrigger(POPUP);  // Animasyonu baþlatýyoruz

        // Baþarýsýzlýk için UI öðelerini ayarlýyoruz
        backgroundImage.color = failedColor; // Arka plan rengini deðiþtiriyoruz
        iconImage.sprite = failedSprite;     // Simgeyi baþarý simgesine deðiþtiriyoruz
        messageText.text = "DELIVERY\nFAILED"; // Mesajý "FAILED" olarak ayarlýyoruz
    }

    // Tarif baþarýlý olduðunda tetiklenen metod
    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true); // UI'yi görünür yapýyoruz
        animator.SetTrigger(POPUP);  // Animasyonu baþlatýyoruz

        // Baþarý için UI öðelerini ayarlýyoruz
        backgroundImage.color = successColor; // Arka plan rengini deðiþtiriyoruz
        iconImage.sprite = successSprite;     // Simgeyi baþarý simgesine deðiþtiriyoruz
        messageText.text = "DELIVERY\nSUCCESS"; // Mesajý "SUCCESS" olarak ayarlýyoruz
    }
}
