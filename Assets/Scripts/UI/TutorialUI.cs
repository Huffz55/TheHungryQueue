using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{

    // Kontrol tu�lar� i�in TextMeshPro referanslar�
    [SerializeField] private TextMeshProUGUI keyMoveUpText;            // Yukar� hareket tu�u
    [SerializeField] private TextMeshProUGUI keyMoveDownText;          // A�a�� hareket tu�u
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;          // Sola hareket tu�u
    [SerializeField] private TextMeshProUGUI keyMoveRightText;         // Sa�a hareket tu�u
    [SerializeField] private TextMeshProUGUI keyInteractText;          // Etkile�im tu�u
    [SerializeField] private TextMeshProUGUI keyInteractAlternateText; // Alternatif etkile�im tu�u
    [SerializeField] private TextMeshProUGUI keyPauseText;             // Duraklatma tu�u
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;   // Gamepad etkile�im tu�u
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAlternateText; // Gamepad alternatif etkile�im tu�u
    [SerializeField] private TextMeshProUGUI keyGamepadPauseText;      // Gamepad duraklatma tu�u

    private void Start()
    {
        // Tu� ba�lama de�i�tirildi�inde g�ncelleme
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;

        // Oyun durumu de�i�ti�inde g�ncelleme
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        // G�rseli g�ncelle
        UpdateVisual();

        // Tutorial UI'� g�ster
        Show();
    }

    // Oyun durumu de�i�ti�inde �a�r�l�r (Ba�lang�� sayac� aktifse tutorial'� gizle)
    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    // Kullan�c� tu� ba�lamalar�n� de�i�tirdi�inde �a�r�l�r
    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        // G�rseli g�ncelle
        UpdateVisual();
    }

    // Tu� ba�lamalar�n� al�p UI'� g�nceller
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

    // Tutorial UI'� g�ster
    private void Show()
    {
        gameObject.SetActive(true);
    }

    // Tutorial UI'� gizle
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
