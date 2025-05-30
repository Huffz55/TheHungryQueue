using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    // Progres barýn gösterileceði oyun nesnesi
    [SerializeField] private GameObject hasProgressGameObject;

    // Progres barýn görseli (Image component)
    [SerializeField] private Image barImage;

    // IHasProgress arayüzünü implement eden nesne
    private IHasProgress hasProgress;

    // Baþlangýçta IHasProgress arayüzünü implement eden bileþeni al
    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        // IHasProgress implementasyonu yapýlmamýþsa hata mesajý göster
        if (hasProgress == null)
        {
            Debug.LogError("Game Object " + hasProgressGameObject + " does not have a component that implements IHasProgress!");
        }

        // Progres deðiþtiðinde tetiklenecek olayý dinlemeye baþla
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        // Barý baþlangýçta sýfýrla
        barImage.fillAmount = 0f;

        // Baþlangýçta progres barýný gizle
        Hide();
    }

    // Progres deðiþtiðinde tetiklenen olay fonksiyonu
    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Progress'in normalized deðerini barýn doluluk oranýna ata
        barImage.fillAmount = e.progressNormalized;

        // Eðer progres tam dolmuþsa veya hiç baþlamamýþsa, barý gizle
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            // Progress devam ediyorsa barý göster
            Show();
        }
    }

    // Progres barýný göster
    private void Show()
    {
        gameObject.SetActive(true);
    }

    // Progres barýný gizle
    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
