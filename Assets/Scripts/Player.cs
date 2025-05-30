using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    // Oyuncunun singleton �rne�i
    public static Player Instance { get; private set; }

    // Etkile�im ve se�ilen tezgah de�i�im olaylar�
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    // Se�ilen tezgah� olayla birlikte iletmek i�in event arg�man�
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    // Oyuncunun hareket ayarlar�
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput; // Oyuncu girdilerini i�lemek i�in
    [SerializeField] private LayerMask countersLayerMask; // Tezgahlar i�in layer mask
    [SerializeField] private Transform kitchenObjectHoldPoint; // Mutfak objesinin tutuldu�u nokta

    // Dahili hareket ve etkile�im takip de�i�kenleri
    private bool isWalking; // Oyuncunun y�r�y�p y�r�medi�i
    private Vector3 lastInteractDir; // Son etkile�im y�n�
    private BaseCounter selectedCounter; // Se�ilen tezgah
    private KitchenObject kitchenObject; // Oyuncunun tuttu�u mutfak objesi

    // Script y�klendi�inde �a�r�l�r
    private void Awake()
    {
        // Singleton kontrol�
        if (Instance != null)
        {
            Debug.LogError("Birden fazla Player nesnesi var!");
        }
        Instance = this;
    }

    // Oyun ba�lad���nda �a�r�l�r
    private void Start()
    {
        // Giri� olaylar�na abone ol
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    // Ana etkile�im (�rne�in: e�ya alma/b�rakma) ger�ekle�ti�inde �al���r
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        // Se�ilen tezgah varsa onunla etkile�ime ge�
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    // Alternatif etkile�im ger�ekle�ti�inde �al���r (�rne�in: kar��t�rma)
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        // Se�ilen tezgah varsa alternatif etkile�ime ge�
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    // Her frame'de �al���r
    private void Update()
    {
        HandleMovement();     // Hareket i�lemleri
        HandleInteractions(); // Etkile�im i�lemleri
    }

    // Oyuncu y�r�yor mu?
    public bool IsWalking()
    {
        return isWalking;
    }

    // Oyuncunun hareketlerini i�ler
    private void HandleMovement()
    {
        // Giri�ten hareket y�n�n� al
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Hareket mesafesi ve �arp��ma kontrol�
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Tam hareket olmuyorsa X y�n�nde dene
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) &&
                      !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX; // Sadece X y�n�nde hareket et
            }
            else
            {
                // X de olmuyorsa Z y�n�nde dene
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) &&
                          !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ; // Sadece Z y�n�nde hareket et
                }
            }
        }

        // Hareket edilebiliyorsa pozisyonu g�ncelle
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        // Y�r�me durumu g�ncelle
        isWalking = moveDir != Vector3.zero;

        // Hareket y�n�ne do�ru yumu�ak d�n��
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    // Oyuncunun tezgahlarla etkile�imini kontrol eder
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir; // Son etkile�im y�n�n� g�ncelle
        }

        // Belirli mesafedeki tezgah� kontrol et
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Tezgah varsa onu se�
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null); // Tezgah bulunamad�ysa se�imi kald�r
            }
        }
        else
        {
            SetSelectedCounter(null); // I��n �arpmad�ysa se�imi kald�r
        }
    }

    // Se�ilen tezgah� belirler ve olay� tetikler
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    // Mutfak objesinin takip edece�i Transform
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    // Oyuncuya mutfak objesi ver
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        // Bir obje al�nd���nda olay tetikle
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    // Oyuncunun tuttu�u mutfak objesini al
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // Mutfak objesini temizle
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // Oyuncunun elinde mutfak objesi var m�?
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
