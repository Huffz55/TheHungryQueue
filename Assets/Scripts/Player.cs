using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    // Oyuncunun singleton örneði
    public static Player Instance { get; private set; }

    // Etkileþim ve seçilen tezgah deðiþim olaylarý
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    // Seçilen tezgahý olayla birlikte iletmek için event argümaný
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    // Oyuncunun hareket ayarlarý
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput; // Oyuncu girdilerini iþlemek için
    [SerializeField] private LayerMask countersLayerMask; // Tezgahlar için layer mask
    [SerializeField] private Transform kitchenObjectHoldPoint; // Mutfak objesinin tutulduðu nokta

    // Dahili hareket ve etkileþim takip deðiþkenleri
    private bool isWalking; // Oyuncunun yürüyüp yürümediði
    private Vector3 lastInteractDir; // Son etkileþim yönü
    private BaseCounter selectedCounter; // Seçilen tezgah
    private KitchenObject kitchenObject; // Oyuncunun tuttuðu mutfak objesi

    // Script yüklendiðinde çaðrýlýr
    private void Awake()
    {
        // Singleton kontrolü
        if (Instance != null)
        {
            Debug.LogError("Birden fazla Player nesnesi var!");
        }
        Instance = this;
    }

    // Oyun baþladýðýnda çaðrýlýr
    private void Start()
    {
        // Giriþ olaylarýna abone ol
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    // Ana etkileþim (örneðin: eþya alma/býrakma) gerçekleþtiðinde çalýþýr
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        // Seçilen tezgah varsa onunla etkileþime geç
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    // Alternatif etkileþim gerçekleþtiðinde çalýþýr (örneðin: karýþtýrma)
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        // Seçilen tezgah varsa alternatif etkileþime geç
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    // Her frame'de çalýþýr
    private void Update()
    {
        HandleMovement();     // Hareket iþlemleri
        HandleInteractions(); // Etkileþim iþlemleri
    }

    // Oyuncu yürüyor mu?
    public bool IsWalking()
    {
        return isWalking;
    }

    // Oyuncunun hareketlerini iþler
    private void HandleMovement()
    {
        // Giriþten hareket yönünü al
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Hareket mesafesi ve çarpýþma kontrolü
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Tam hareket olmuyorsa X yönünde dene
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) &&
                      !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX; // Sadece X yönünde hareket et
            }
            else
            {
                // X de olmuyorsa Z yönünde dene
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) &&
                          !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ; // Sadece Z yönünde hareket et
                }
            }
        }

        // Hareket edilebiliyorsa pozisyonu güncelle
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        // Yürüme durumu güncelle
        isWalking = moveDir != Vector3.zero;

        // Hareket yönüne doðru yumuþak dönüþ
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    // Oyuncunun tezgahlarla etkileþimini kontrol eder
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir; // Son etkileþim yönünü güncelle
        }

        // Belirli mesafedeki tezgahý kontrol et
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Tezgah varsa onu seç
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null); // Tezgah bulunamadýysa seçimi kaldýr
            }
        }
        else
        {
            SetSelectedCounter(null); // Iþýn çarpmadýysa seçimi kaldýr
        }
    }

    // Seçilen tezgahý belirler ve olayý tetikler
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    // Mutfak objesinin takip edeceði Transform
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    // Oyuncuya mutfak objesi ver
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        // Bir obje alýndýðýnda olay tetikle
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    // Oyuncunun tuttuðu mutfak objesini al
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // Mutfak objesini temizle
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // Oyuncunun elinde mutfak objesi var mý?
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
