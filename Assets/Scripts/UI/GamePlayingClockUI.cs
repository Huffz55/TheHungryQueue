using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{

    [SerializeField] private Image timerImage;  // Zamanlay�c�y� g�rsel olarak g�sterecek Image bile�eni

    // Update her frame'de �a�r�l�r, zamanlay�c�y� g�nceller
    private void Update()
    {
        // Oyun zamanlay�c�s�n�n normalize edilmi� de�erini al�r ve timerImage'�n doluluk oran�n� ayarlar
        timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
