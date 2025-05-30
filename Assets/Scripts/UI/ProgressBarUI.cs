using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    // Progres bar�n g�sterilece�i oyun nesnesi
    [SerializeField] private GameObject hasProgressGameObject;

    // Progres bar�n g�rseli (Image component)
    [SerializeField] private Image barImage;

    // IHasProgress aray�z�n� implement eden nesne
    private IHasProgress hasProgress;

    // Ba�lang��ta IHasProgress aray�z�n� implement eden bile�eni al
    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        // IHasProgress implementasyonu yap�lmam��sa hata mesaj� g�ster
        if (hasProgress == null)
        {
            Debug.LogError("Game Object " + hasProgressGameObject + " does not have a component that implements IHasProgress!");
        }

        // Progres de�i�ti�inde tetiklenecek olay� dinlemeye ba�la
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        // Bar� ba�lang��ta s�f�rla
        barImage.fillAmount = 0f;

        // Ba�lang��ta progres bar�n� gizle
        Hide();
    }

    // Progres de�i�ti�inde tetiklenen olay fonksiyonu
    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Progress'in normalized de�erini bar�n doluluk oran�na ata
        barImage.fillAmount = e.progressNormalized;

        // E�er progres tam dolmu�sa veya hi� ba�lamam��sa, bar� gizle
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            // Progress devam ediyorsa bar� g�ster
            Show();
        }
    }

    // Progres bar�n� g�ster
    private void Show()
    {
        gameObject.SetActive(true);
    }

    // Progres bar�n� gizle
    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
