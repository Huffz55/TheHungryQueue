using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    // Tarif �retildi�inde tetiklenecek olay
    public event EventHandler OnRecipeSpawned;
    // Tarif tamamland���nda tetiklenecek olay
    public event EventHandler OnRecipeCompleted;
    // Tarif ba�ar�yla teslim edildi�inde tetiklenecek olay
    public event EventHandler OnRecipeSuccess;
    // Tarif yanl�� teslim edildi�inde tetiklenecek olay
    public event EventHandler OnRecipeFailed;

    // Singleton eri�imi (tek bir DeliveryManager olmas�n� sa�lar)
    public static DeliveryManager Instance { get; private set; }

    // Tarif listesini tutan ScriptableObject
    [SerializeField] private RecipeListSO recipeListSO;

    // Bekleyen tariflerin listesi
    private List<RecipeSO> waitingRecipeSOList;
    // Yeni tarifin �retilece�i zamana kadar saya�
    private float spawnRecipeTimer;
    // Yeni bir tarifin �retilece�i maksimum s�re
    private float spawnRecipeTimerMax = 4f;
    // Ayn� anda bekleyebilecek maksimum tarif say�s�
    private int waitingRecipesMax = 4;
    // Oyuncunun ba�ar�yla teslim etti�i tarif say�s�
    private int successfulRecipesAmount;

    // Oyun ba��nda singleton ayarlan�r ve liste olu�turulur
    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    // Her frame �a�r�l�r, zamanlay�c�y� g�nceller ve tarif �retir
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            // Zamanlay�c� s�f�rland�ysa yeni tarif �ret
            spawnRecipeTimer = spawnRecipeTimerMax;

            // Oyun oynan�yorsa ve maksimum tarif s�n�r�na ula��lmad�ysa
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
            {
                // Rastgele bir tarif se� ve listeye ekle
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                // Tarif �retildi�ini bildir
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Oyuncunun teslim etti�i tarifi kontrol eder
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        // Bekleyen tarifleri kontrol et
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            // Tarif ile tabaktaki malzeme say�s� e�itse
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;

                // Tarifin her malzemesi i�in kontrol et
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;

                    // Tabaktaki malzemelerde bu malzeme var m� kontrol et
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    // E�er tarifteki bu malzeme tabakta yoksa, tarif uyu�muyor
                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }

                // T�m malzemeler e�le�tiyse
                if (plateContentsMatchesRecipe)
                {
                    // Do�ru tarif teslim edildi
                    successfulRecipesAmount++; // Skoru art�r
                    waitingRecipeSOList.RemoveAt(i); // Tarifi listeden ��kar

                    // Ba�ar� olaylar�n� tetikle
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        // E�er hi�bir tarif e�le�mediyse ba�ar�s�zl�k olay�n� tetikle
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    // Bekleyen tariflerin listesini d�nd�r�r
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    // Ba�ar�yla tamamlanan tarif say�s�n� d�nd�r�r
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
