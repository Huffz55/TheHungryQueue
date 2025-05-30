using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{

    // UI elemanlar�
    [SerializeField] private Button resumeButton;  // Oyunu kald��� yerden devam ettirecek buton
    [SerializeField] private Button mainMenuButton;  // Ana men�ye y�nlendirecek buton
    [SerializeField] private Button optionsButton;  // Se�enekler ekran�n� a�acak buton

    // Awake metodu, butonlar�n t�klama olaylar�n� dinler
    private void Awake()
    {
        // Resume butonuna t�klama i�lemi ekler, oyun duraklatma i�lemini tersine �evirir
        resumeButton.onClick.AddListener(() => {
            KitchenGameManager.Instance.TogglePauseGame();  // Oyun duraklatmay� tersine �evirir
        });
        // Main Menu butonuna t�klama i�lemi ekler, ana men�y� y�kler
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);  // Ana men� sahnesini y�kler
        });
        // Options butonuna t�klama i�lemi ekler, se�enekler ekran�n� g�sterir
        optionsButton.onClick.AddListener(() => {
            Hide();  // �nce mevcut UI'yi gizler
            OptionsUI.Instance.Show(Show);  // Se�enekler ekran�n� g�sterir
        });
    }

    // Start metodu, oyun ba�lad���nda gerekli abone i�lemlerini yapar
    private void Start()
    {
        // Oyun durumu de�i�ti�inde (pause/unpause) ilgili metodu �a��r�r
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide();  // Ba�lang��ta UI'yi gizler
    }

    // Oyun duraklat�ld���nda �al��acak metot
    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();  // Duraklat�ld���nda UI'yi g�sterir
    }

    // Oyun ba�lat�ld���nda �al��acak metot
    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();  // Ba�lat�ld���nda UI'yi gizler
    }

    // UI'yi g�sterir
    private void Show()
    {
        gameObject.SetActive(true);  // GameObject'i aktif eder

        resumeButton.Select();  // Resume butonuna odaklan�r
    }

    // UI'yi gizler
    private void Hide()
    {
        gameObject.SetActive(false);  // GameObject'i gizler
    }

}
