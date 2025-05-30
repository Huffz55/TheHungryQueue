using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{

    // Singleton �rne�i, OptionsUI'yi tek bir yerde eri�ilebilir k�lar
    public static OptionsUI Instance { get; private set; }

    // UI bile�enlerini temsil eden butonlar ve metinler
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAlternateButton;
    [SerializeField] private Button gamepadPauseButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;
    [SerializeField] private Transform pressToRebindKeyTransform;

    // Buton kapatma i�lemi i�in kullan�lacak aksiyon
    private Action onCloseButtonAction;

    private void Awake()
    {
        // Singleton �rne�ini olu�tur
        Instance = this;

        // Butonlara listener ekle
        soundEffectsButton.onClick.AddListener(() => {
            // Ses efektleri ses seviyesini de�i�tir
            SoundManager.Instance.ChangeVolume();
            UpdateVisual(); // G�rsel g�ncelle
        });
        musicButton.onClick.AddListener(() => {
            // M�zik ses seviyesini de�i�tir
            MusicManager.Instance.ChangeVolume();
            UpdateVisual(); // G�rsel g�ncelle
        });
        closeButton.onClick.AddListener(() => {
            // Kapatma i�lemi ve onCloseButtonAction �a�r�s�
            Hide();
            onCloseButtonAction();
        });

        // Tu� atamalar� i�in rebind i�lemleri
        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        interactAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact); });
        gamepadInteractAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_InteractAlternate); });
        gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Pause); });
    }

    private void Start()
    {
        // Oyunun unpause (oyuna devam) oldu�unda UI'yi gizle
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        // G�rsel g�ncelleme
        UpdateVisual();

        // Rebind i�lemi i�in g�sterilmeyecek tu�u gizle
        HidePressToRebindKey();
        Hide(); // Ba�lang��ta gizle
    }

    // Oyunun unpause oldu�u durumda UI'yi gizler
    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    // Ses efektleri ve m�zik metinlerini g�nceller, tu� atamalar�n� g�sterir
    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        gamepadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    // UI'yi g�sterir ve kapanma aksiyonunu al�r
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);

        soundEffectsButton.Select(); // �lk olarak soundEffectsButton se�ili olsun
    }

    // UI'yi gizler
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    // Tu� ba�lama i�in "Press to rebind key" mesaj�n� g�sterir
    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    // Tu� ba�lama i�in "Press to rebind key" mesaj�n� gizler
    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    // Verilen tu� ba�lamay� rebind eder
    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey(); // Rebind yap�l�rken kullan�c�ya mesaj g�ster
        GameInput.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey(); // Rebind i�lemi tamamland���nda gizle
            UpdateVisual(); // G�rseli g�ncelle
        });
    }

}
