using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI recipesDeliveredText;  // Teslim edilen tarif say�s�n� g�sterecek metin bile�eni

    // Ba�lang��ta GameManager'a abone olup, UI'yi gizliyoruz
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;  // Oyun durumu de�i�ti�inde tetiklenecek olan event'e abone oluyoruz

        Hide();  // GameOverUI ba�lang��ta gizli olmal�
    }

    // Oyun durumu de�i�ti�inde tetiklenen event handler
    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {  // E�er oyun bitmi�se
            Show();  // Game Over ekran�n� g�ster

            // Teslim edilen tariflerin say�s�n� al�p metne d�n��t�r�p UI'ye ekliyoruz
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
        }
        else
        {
            Hide();  // E�er oyun bitmemi�se UI'yi gizle
        }
    }

    // UI'yi aktif hale getiren metod
    private void Show()
    {
        gameObject.SetActive(true);  // UI ��esini g�r�n�r yap�yoruz
    }

    // UI'yi gizleyen metod
    private void Hide()
    {
        gameObject.SetActive(false);  // UI ��esini gizliyoruz
    }
}
