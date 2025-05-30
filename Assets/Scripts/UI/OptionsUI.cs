using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{

    // Singleton örneði, OptionsUI'yi tek bir yerde eriþilebilir kýlar
    public static OptionsUI Instance { get; private set; }

    // UI bileþenlerini temsil eden butonlar ve metinler
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

    // Buton kapatma iþlemi için kullanýlacak aksiyon
    private Action onCloseButtonAction;

    private void Awake()
    {
        // Singleton örneðini oluþtur
        Instance = this;

        // Butonlara listener ekle
        soundEffectsButton.onClick.AddListener(() => {
            // Ses efektleri ses seviyesini deðiþtir
            SoundManager.Instance.ChangeVolume();
            UpdateVisual(); // Görsel güncelle
        });
        musicButton.onClick.AddListener(() => {
            // Müzik ses seviyesini deðiþtir
            MusicManager.Instance.ChangeVolume();
            UpdateVisual(); // Görsel güncelle
        });
        closeButton.onClick.AddListener(() => {
            // Kapatma iþlemi ve onCloseButtonAction çaðrýsý
            Hide();
            onCloseButtonAction();
        });

        // Tuþ atamalarý için rebind iþlemleri
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
        // Oyunun unpause (oyuna devam) olduðunda UI'yi gizle
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        // Görsel güncelleme
        UpdateVisual();

        // Rebind iþlemi için gösterilmeyecek tuþu gizle
        HidePressToRebindKey();
        Hide(); // Baþlangýçta gizle
    }

    // Oyunun unpause olduðu durumda UI'yi gizler
    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    // Ses efektleri ve müzik metinlerini günceller, tuþ atamalarýný gösterir
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

    // UI'yi gösterir ve kapanma aksiyonunu alýr
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);

        soundEffectsButton.Select(); // Ýlk olarak soundEffectsButton seçili olsun
    }

    // UI'yi gizler
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    // Tuþ baðlama için "Press to rebind key" mesajýný gösterir
    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    // Tuþ baðlama için "Press to rebind key" mesajýný gizler
    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    // Verilen tuþ baðlamayý rebind eder
    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey(); // Rebind yapýlýrken kullanýcýya mesaj göster
        GameInput.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey(); // Rebind iþlemi tamamlandýðýnda gizle
            UpdateVisual(); // Görseli güncelle
        });
    }

}
