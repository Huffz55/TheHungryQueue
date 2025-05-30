using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{

    // Kontrol tuþlarý için TextMeshPro referanslarý
    [SerializeField] private TextMeshProUGUI keyMoveUpText;            // Yukarý hareket tuþu
    [SerializeField] private TextMeshProUGUI keyMoveDownText;          // Aþaðý hareket tuþu
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;          // Sola hareket tuþu
    [SerializeField] private TextMeshProUGUI keyMoveRightText;         // Saða hareket tuþu
    [SerializeField] private TextMeshProUGUI keyInteractText;          // Etkileþim tuþu
    [SerializeField] private TextMeshProUGUI keyInteractAlternateText; // Alternatif etkileþim tuþu
    [SerializeField] private TextMeshProUGUI keyPauseText;             // Duraklatma tuþu
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;   // Gamepad etkileþim tuþu
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAlternateText; // Gamepad alternatif etkileþim tuþu
    [SerializeField] private TextMeshProUGUI keyGamepadPauseText;      // Gamepad duraklatma tuþu

    private void Start()
    {
        // Tuþ baðlama deðiþtirildiðinde güncelleme
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;

        // Oyun durumu deðiþtiðinde güncelleme
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        // Görseli güncelle
        UpdateVisual();

        // Tutorial UI'ý göster
        Show();
    }

    // Oyun durumu deðiþtiðinde çaðrýlýr (Baþlangýç sayacý aktifse tutorial'ý gizle)
    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    // Kullanýcý tuþ baðlamalarýný deðiþtirdiðinde çaðrýlýr
    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        // Görseli güncelle
        UpdateVisual();
    }

    // Tuþ baðlamalarýný alýp UI'ý günceller
    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        keyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        keyGamepadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        keyGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    // Tutorial UI'ý göster
    private void Show()
    {
        gameObject.SetActive(true);
    }

    // Tutorial UI'ý gizle
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
