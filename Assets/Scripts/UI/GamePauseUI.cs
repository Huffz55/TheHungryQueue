using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{

    // UI elemanlarý
    [SerializeField] private Button resumeButton;  // Oyunu kaldýðý yerden devam ettirecek buton
    [SerializeField] private Button mainMenuButton;  // Ana menüye yönlendirecek buton
    [SerializeField] private Button optionsButton;  // Seçenekler ekranýný açacak buton

    // Awake metodu, butonlarýn týklama olaylarýný dinler
    private void Awake()
    {
        // Resume butonuna týklama iþlemi ekler, oyun duraklatma iþlemini tersine çevirir
        resumeButton.onClick.AddListener(() => {
            KitchenGameManager.Instance.TogglePauseGame();  // Oyun duraklatmayý tersine çevirir
        });
        // Main Menu butonuna týklama iþlemi ekler, ana menüyü yükler
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);  // Ana menü sahnesini yükler
        });
        // Options butonuna týklama iþlemi ekler, seçenekler ekranýný gösterir
        optionsButton.onClick.AddListener(() => {
            Hide();  // Önce mevcut UI'yi gizler
            OptionsUI.Instance.Show(Show);  // Seçenekler ekranýný gösterir
        });
    }

    // Start metodu, oyun baþladýðýnda gerekli abone iþlemlerini yapar
    private void Start()
    {
        // Oyun durumu deðiþtiðinde (pause/unpause) ilgili metodu çaðýrýr
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide();  // Baþlangýçta UI'yi gizler
    }

    // Oyun duraklatýldýðýnda çalýþacak metot
    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();  // Duraklatýldýðýnda UI'yi gösterir
    }

    // Oyun baþlatýldýðýnda çalýþacak metot
    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();  // Baþlatýldýðýnda UI'yi gizler
    }

    // UI'yi gösterir
    private void Show()
    {
        gameObject.SetActive(true);  // GameObject'i aktif eder

        resumeButton.Select();  // Resume butonuna odaklanýr
    }

    // UI'yi gizler
    private void Hide()
    {
        gameObject.SetActive(false);  // GameObject'i gizler
    }

}
