using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    // Singleton instance of the Player
    public static Player Instance { get; private set; }

    // Event declarations for interaction and selected counter changes
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    // Event argument to pass the selected counter
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    // Player movement settings
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;  // To handle player inputs
    [SerializeField] private LayerMask countersLayerMask;  // Layer mask to check for counters
    [SerializeField] private Transform kitchenObjectHoldPoint;  // Position where kitchen object is held

    // Internal variables to track movement and interactions
    private bool isWalking;  // To track if the player is walking
    private Vector3 lastInteractDir;  // Direction of last interaction attempt
    private BaseCounter selectedCounter;  // The currently selected counter
    private KitchenObject kitchenObject;  // The kitchen object currently held by the player

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Singleton pattern check
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Subscribe to input events for interaction
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    // Handle interaction with counters on primary action (e.g., picking or placing objects)
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        // If there's a selected counter, interact with it
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    // Handle alternate interaction (e.g., using or alternating an object)
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        // If there's a selected counter, perform alternate interaction
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();  // Update movement based on player input
        HandleInteractions();  // Check for interactions with counters
    }

    // Check if the player is walking
    public bool IsWalking()
    {
        return isWalking;
    }

    // Handle movement logic
    private void HandleMovement()
    {
        // Get movement direction from input
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Calculate movement distance and check if the player can move
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;  // Player's radius for collision detection
        float playerHeight = 2f;  // Player's height for collision detection
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Try moving along only the X axis if full movement isn't possible
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;  // Can move along X axis only
            }
            else
            {
                // Attempt to move along only the Z axis if X axis movement fails
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;  // Can move along Z axis only
                }
            }
        }

        // Perform the movement if possible
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        // Update walking status
        isWalking = moveDir != Vector3.zero;

        // Smoothly rotate the player to face the movement direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    // Handle interaction with counters based on player input
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;  // Update last interaction direction
        }

        // Check if the player can interact with a counter in the last interaction direction
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // If the player is close to a counter, set it as the selected counter
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);  // No counter found, deselect counter
            }
        }
        else
        {
            SetSelectedCounter(null);  // No hit, deselect counter
        }
    }

    // Set the selected counter and trigger the event
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    // Get the transform where the kitchen object should follow
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    // Set the kitchen object for the player
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        // If a kitchen object is set, invoke the pick event
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    // Get the kitchen object the player is holding
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // Clear the kitchen object from the player
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // Check if the player is holding a kitchen object
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
