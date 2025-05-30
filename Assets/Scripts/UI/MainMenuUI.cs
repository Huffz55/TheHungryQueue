using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    // UI butonlarý
    [SerializeField] private Button playButton;  // Oyun baþlatma butonu
    [SerializeField] private Button quitButton;  // Uygulamayý kapatma butonu

    // Awake metodu, sahne yüklendiðinde çalýþýr
    private void Awake()
    {
        // Play butonuna týklama iþlemi ekler, oyunu baþlatýr
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);  // GameScene sahnesini yükler
        });

        // Quit butonuna týklama iþlemi ekler, uygulamayý kapatýr
        quitButton.onClick.AddListener(() => {
            Application.Quit();  // Uygulamayý kapatýr
        });

        Time.timeScale = 1f;  // Oyun hýzýný (zaman akýþýný) normal seviyeye ayarlar
    }

}
