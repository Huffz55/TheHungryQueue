using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{

    [SerializeField] private Image timerImage;  // Zamanlayýcýyý görsel olarak gösterecek Image bileþeni

    // Update her frame'de çaðrýlýr, zamanlayýcýyý günceller
    private void Update()
    {
        // Oyun zamanlayýcýsýnýn normalize edilmiþ deðerini alýr ve timerImage'ýn doluluk oranýný ayarlar
        timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
