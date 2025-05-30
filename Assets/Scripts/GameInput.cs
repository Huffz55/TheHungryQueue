using System;
using UnityEngine;
using UnityEngine.InputSystem; // Yeni Unity Input System'ýný kullanmak için gerekli

public class GameInput : MonoBehaviour
{

    // Girdi ayarlarýnýn PlayerPrefs'te saklanacaðý anahtar
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    // Singleton eriþimi (tek bir GameInput nesnesi)
    public static GameInput Instance { get; private set; }

    // Girdi olaylarý
    public event EventHandler OnInteractAction;              // Etkileþim tuþuna basýldýðýnda
    public event EventHandler OnInteractAlternateAction;     // Alternatif etkileþim tuþuna basýldýðýnda
    public event EventHandler OnPauseAction;                 // Duraklatma tuþuna basýldýðýnda
    public event EventHandler OnBindingRebind;               // Tuþ atamalarý deðiþtirildiðinde

    // Kullanýcý tarafýndan atanabilecek tüm eylemleri temsil eden enum
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

    // InputAction'larý içeren otomatik oluþturulmuþ sýnýf (Input System'dan)
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        // Input sisteminden gelen sýnýfýn örneðini oluþtur
        playerInputActions = new PlayerInputActions();

        // Eðer kullanýcý daha önce özel tuþ atamalarý yaptýysa onlarý yükle
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        // Girdi sistemi aktif hale getirilir
        playerInputActions.Player.Enable();

        // Girdi olaylarýna dinleyici ekle
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        // Oyun nesnesi yok edildiðinde olaylardan çýk ve belleði temizle
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    // Duraklatma tuþuna basýldýðýnda çaðrýlýr
    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    // Alternatif etkileþim tuþuna basýldýðýnda çaðrýlýr
    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    // Etkileþim tuþuna basýldýðýnda çaðrýlýr
    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // Kullanýcýnýn hareket girdisini alýr ve normalize eder
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized; // Yön vektörünü normalize eder
        return inputVector;
    }

    // Belirli bir tuþ atamasýnýn ekranda görünen metnini döndürür
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

    // Kullanýcýnýn belirli bir tuþ atamasýný deðiþtirmesine olanak tanýr
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        // Geçici olarak input'u devre dýþý býrak
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        // Hangi input action ve binding index'ine göre yeniden atama yapýlacaðýný belirle
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

        // Yeni tuþ atamasýný baþlat
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose(); // Bellek temizliði
                playerInputActions.Player.Enable(); // Girdiyi tekrar aktif et
                onActionRebound(); // Dýþarýdan gelen callback'i çalýþtýr

                // Yeni tuþ atamasýný kaydet
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                // Olayý tetikle
                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start(); // Rebinding'i baþlat
    }

}
