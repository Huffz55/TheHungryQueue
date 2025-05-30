using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI recipesDeliveredText;  // Teslim edilen tarif sayýsýný gösterecek metin bileþeni

    // Baþlangýçta GameManager'a abone olup, UI'yi gizliyoruz
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;  // Oyun durumu deðiþtiðinde tetiklenecek olan event'e abone oluyoruz

        Hide();  // GameOverUI baþlangýçta gizli olmalý
    }

    // Oyun durumu deðiþtiðinde tetiklenen event handler
    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {  // Eðer oyun bitmiþse
            Show();  // Game Over ekranýný göster

            // Teslim edilen tariflerin sayýsýný alýp metne dönüþtürüp UI'ye ekliyoruz
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
        }
        else
        {
            Hide();  // Eðer oyun bitmemiþse UI'yi gizle
        }
    }

    // UI'yi aktif hale getiren metod
    private void Show()
    {
        gameObject.SetActive(true);  // UI öðesini görünür yapýyoruz
    }

    // UI'yi gizleyen metod
    private void Hide()
    {
        gameObject.SetActive(false);  // UI öðesini gizliyoruz
    }
}
