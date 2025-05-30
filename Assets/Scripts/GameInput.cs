using System;
using UnityEngine;
using UnityEngine.InputSystem; // Yeni Unity Input System'�n� kullanmak i�in gerekli

public class GameInput : MonoBehaviour
{

    // Girdi ayarlar�n�n PlayerPrefs'te saklanaca�� anahtar
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    // Singleton eri�imi (tek bir GameInput nesnesi)
    public static GameInput Instance { get; private set; }

    // Girdi olaylar�
    public event EventHandler OnInteractAction;              // Etkile�im tu�una bas�ld���nda
    public event EventHandler OnInteractAlternateAction;     // Alternatif etkile�im tu�una bas�ld���nda
    public event EventHandler OnPauseAction;                 // Duraklatma tu�una bas�ld���nda
    public event EventHandler OnBindingRebind;               // Tu� atamalar� de�i�tirildi�inde

    // Kullan�c� taraf�ndan atanabilecek t�m eylemleri temsil eden enum
    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause
    }

    // InputAction'lar� i�eren otomatik olu�turulmu� s�n�f (Input System'dan)
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        // Input sisteminden gelen s�n�f�n �rne�ini olu�tur
        playerInputActions = new PlayerInputActions();

        // E�er kullan�c� daha �nce �zel tu� atamalar� yapt�ysa onlar� y�kle
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        // Girdi sistemi aktif hale getirilir
        playerInputActions.Player.Enable();

        // Girdi olaylar�na dinleyici ekle
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        // Oyun nesnesi yok edildi�inde olaylardan ��k ve belle�i temizle
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    // Duraklatma tu�una bas�ld���nda �a�r�l�r
    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    // Alternatif etkile�im tu�una bas�ld���nda �a�r�l�r
    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    // Etkile�im tu�una bas�ld���nda �a�r�l�r
    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // Kullan�c�n�n hareket girdisini al�r ve normalize eder
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized; // Y�n vekt�r�n� normalize eder
        return inputVector;
    }

    // Belirli bir tu� atamas�n�n ekranda g�r�nen metnini d�nd�r�r
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    // Kullan�c�n�n belirli bir tu� atamas�n� de�i�tirmesine olanak tan�r
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        // Ge�ici olarak input'u devre d��� b�rak
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        // Hangi input action ve binding index'ine g�re yeniden atama yap�laca��n� belirle
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        // Yeni tu� atamas�n� ba�lat
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose(); // Bellek temizli�i
                playerInputActions.Player.Enable(); // Girdiyi tekrar aktif et
                onActionRebound(); // D��ar�dan gelen callback'i �al��t�r

                // Yeni tu� atamas�n� kaydet
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                // Olay� tetikle
                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start(); // Rebinding'i ba�lat
    }

}
