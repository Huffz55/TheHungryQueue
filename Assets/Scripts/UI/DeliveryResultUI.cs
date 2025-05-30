using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{

    // POPUP animasyonunun ad�
    private const string POPUP = "Popup";

    // UI bile�enleri
    [SerializeField] private Image backgroundImage;      // Arka plan rengi i�in image
    [SerializeField] private Image iconImage;            // Ba�ar� ve ba�ar�s�zl�k durumlar� i�in simge
    [SerializeField] private TextMeshProUGUI messageText; // Mesaj� g�stermek i�in TextMeshPro
    [SerializeField] private Color successColor;         // Ba�ar� durumunda kullan�lacak renk
    [SerializeField] private Color failedColor;          // Ba�ar�s�zl�k durumunda kullan�lacak renk
    [SerializeField] private Sprite successSprite;       // Ba�ar� durumunda kullan�lacak simge
    [SerializeField] private Sprite failedSprite;        // Ba�ar�s�zl�k durumunda kullan�lacak simge

    private Animator animator; // Animasyon kontrol� i�in animator

    // Awake metodunda animator bile�eni al�n�yor.
    private void Awake()
    {
        animator = GetComponent<Animator>(); // Animator bile�enini al�yoruz.
    }

    // Start metodunda DeliveryManager'dan gelen olaylara abone oluyoruz
    // ve ba�lang��ta UI'yi gizli tutuyoruz.
    private void Start()
    {
        // DeliveryManager'�n olaylar�na abone oluyoruz
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        // UI ba�lang��ta gizleniyor
        gameObject.SetActive(false);
    }

    // Tarif ba�ar�s�z oldu�unda tetiklenen metod
    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true); // UI'yi g�r�n�r yap�yoruz
        animator.SetTrigger(POPUP);  // Animasyonu ba�lat�yoruz

        // Ba�ar�s�zl�k i�in UI ��elerini ayarl�yoruz
        backgroundImage.color = failedColor; // Arka plan rengini de�i�tiriyoruz
        iconImage.sprite = failedSprite;     // Simgeyi ba�ar� simgesine de�i�tiriyoruz
        messageText.text = "DELIVERY\nFAILED"; // Mesaj� "FAILED" olarak ayarl�yoruz
    }

    // Tarif ba�ar�l� oldu�unda tetiklenen metod
    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true); // UI'yi g�r�n�r yap�yoruz
        animator.SetTrigger(POPUP);  // Animasyonu ba�lat�yoruz

        // Ba�ar� i�in UI ��elerini ayarl�yoruz
        backgroundImage.color = successColor; // Arka plan rengini de�i�tiriyoruz
        iconImage.sprite = successSprite;     // Simgeyi ba�ar� simgesine de�i�tiriyoruz
        messageText.text = "DELIVERY\nSUCCESS"; // Mesaj� "SUCCESS" olarak ayarl�yoruz
    }
}
