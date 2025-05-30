using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    // Tarif üretildiðinde tetiklenecek olay
    public event EventHandler OnRecipeSpawned;
    // Tarif tamamlandýðýnda tetiklenecek olay
    public event EventHandler OnRecipeCompleted;
    // Tarif baþarýyla teslim edildiðinde tetiklenecek olay
    public event EventHandler OnRecipeSuccess;
    // Tarif yanlýþ teslim edildiðinde tetiklenecek olay
    public event EventHandler OnRecipeFailed;

    // Singleton eriþimi (tek bir DeliveryManager olmasýný saðlar)
    public static DeliveryManager Instance { get; private set; }

    // Tarif listesini tutan ScriptableObject
    [SerializeField] private RecipeListSO recipeListSO;

    // Bekleyen tariflerin listesi
    private List<RecipeSO> waitingRecipeSOList;
    // Yeni tarifin üretileceði zamana kadar sayaç
    private float spawnRecipeTimer;
    // Yeni bir tarifin üretileceði maksimum süre
    private float spawnRecipeTimerMax = 4f;
    // Ayný anda bekleyebilecek maksimum tarif sayýsý
    private int waitingRecipesMax = 4;
    // Oyuncunun baþarýyla teslim ettiði tarif sayýsý
    private int successfulRecipesAmount;

    // Oyun baþýnda singleton ayarlanýr ve liste oluþturulur
    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    // Her frame çaðrýlýr, zamanlayýcýyý günceller ve tarif üretir
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            // Zamanlayýcý sýfýrlandýysa yeni tarif üret
            spawnRecipeTimer = spawnRecipeTimerMax;

            // Oyun oynanýyorsa ve maksimum tarif sýnýrýna ulaþýlmadýysa
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
            {
                // Rastgele bir tarif seç ve listeye ekle
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                // Tarif üretildiðini bildir
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Oyuncunun teslim ettiði tarifi kontrol eder
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        // Bekleyen tarifleri kontrol et
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            // Tarif ile tabaktaki malzeme sayýsý eþitse
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;

                // Tarifin her malzemesi için kontrol et
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;

                    // Tabaktaki malzemelerde bu malzeme var mý kontrol et
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    // Eðer tarifteki bu malzeme tabakta yoksa, tarif uyuþmuyor
                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }

                // Tüm malzemeler eþleþtiyse
                if (plateContentsMatchesRecipe)
                {
                    // Doðru tarif teslim edildi
                    successfulRecipesAmount++; // Skoru artýr
                    waitingRecipeSOList.RemoveAt(i); // Tarifi listeden çýkar

                    // Baþarý olaylarýný tetikle
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        // Eðer hiçbir tarif eþleþmediyse baþarýsýzlýk olayýný tetikle
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    // Bekleyen tariflerin listesini döndürür
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    // Baþarýyla tamamlanan tarif sayýsýný döndürür
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
