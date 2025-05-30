using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    // UI butonlar�
    [SerializeField] private Button playButton;  // Oyun ba�latma butonu
    [SerializeField] private Button quitButton;  // Uygulamay� kapatma butonu

    // Awake metodu, sahne y�klendi�inde �al���r
    private void Awake()
    {
        // Play butonuna t�klama i�lemi ekler, oyunu ba�lat�r
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);  // GameScene sahnesini y�kler
        });

        // Quit butonuna t�klama i�lemi ekler, uygulamay� kapat�r
        quitButton.onClick.AddListener(() => {
            Application.Quit();  // Uygulamay� kapat�r
        });

        Time.timeScale = 1f;  // Oyun h�z�n� (zaman ak���n�) normal seviyeye ayarlar
    }

}
