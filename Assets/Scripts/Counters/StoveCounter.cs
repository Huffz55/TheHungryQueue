using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{

    // Olaylar
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged; // Ýlerleme durumu deðiþtiðinde tetiklenir
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged; // Durum deðiþtiðinde tetiklenir

    // State deðiþikliklerini temsil eden sýnýf
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state; // Durum (Idle, Frying, Fried, Burned)
    }

    // Fritözün durumlarýný temsil eden enum
    public enum State
    {
        Idle,       // Beklemede
        Frying,     // Kýzartýlýyor
        Fried,      // Kýzartýlmýþ
        Burned,     // Yanmýþ
    }

    // Fritözün kullanabileceði tarifler
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray; // Kýzartma tarifleri
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray; // Yanma tarifleri

    // Durum, zamanlayýcý ve tarifler
    private State state;
    private float fryingTimer; // Kýzartma süresi
    private FryingRecipeSO fryingRecipeSO; // Kýzartma tarifine ait bilgiler
    private float burningTimer; // Yanma süresi
    private BurningRecipeSO burningRecipeSO; // Yanma tarifine ait bilgiler

    // Baþlangýçta durum "Idle" olarak ayarlanýr
    private void Start()
    {
        state = State.Idle;
    }

    // Her frame'de güncellenen metod
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break; // Beklemede olduðunda hiçbir iþlem yapýlmaz
                case State.Frying:
                    fryingTimer += Time.deltaTime; // Zamanlayýcýyý artýr

                    // Ýlerleme durumu olayýný tetikle
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    // Kýzartma süresi bittiðinde
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        // Kýzartýlmýþ nesne yaratýlýr
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried; // Durumu Fried olarak deðiþtir
                        burningTimer = 0f; // Yanma zamanlayýcýsýný sýfýrla
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()); // Yanma tarifini al

                        // Durum deðiþikliði olayýný tetikle
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime; // Yanma zamanlayýcýsýný artýr

                    // Ýlerleme durumu olayýný tetikle
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    // Yanma süresi bittiðinde
                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        // Yanmýþ nesne yaratýlýr
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned; // Durumu Burned olarak deðiþtir

                        // Durum deðiþikliði olayýný tetikle
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        // Ýlerleme durumunu sýfýrla
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break; // Yanmýþ durumda hiçbir iþlem yapýlmaz
            }
        }
    }

    // Player ile etkileþime girmeyi saðlayan metod
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Tezgah boþsa
            if (player.HasKitchenObject())
            {
                // Player bir mutfak objesi taþýyor
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player kýzartýlabilir bir þey taþýyor
                    player.GetKitchenObject().SetKitchenObjectParent(this); // Nesneyi bu tezgaha yerleþtir

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()); // Kýzartma tarifini al

                    state = State.Frying; // Durumu Frying olarak deðiþtir
                    fryingTimer = 0f; // Kýzartma zamanlayýcýsýný sýfýrla

                    // Durum deðiþikliði olayýný tetikle
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    // Ýlerleme durumu olayýný tetikle
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
            else
            {
                // Player hiçbir þey taþýmýyor
            }
        }
        else
        {
            // Tezgahýn üzerinde mutfak objesi var
            if (player.HasKitchenObject())
            {
                // Player bir mutfak objesi taþýyor
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player bir tabak taþýyor
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf(); // Tezgahýn üzerindeki objeyi sil

                        state = State.Idle; // Durumu Idle olarak deðiþtir

                        // Durum deðiþikliði olayýný tetikle
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        // Ýlerleme durumu olayýný sýfýrla
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // Player hiçbir þey taþýmýyor
                GetKitchenObject().SetKitchenObjectParent(player); // Nesneyi Player'a ver

                state = State.Idle; // Durumu Idle olarak deðiþtir

                // Durum deðiþikliði olayýný tetikle
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                // Ýlerleme durumu olayýný sýfýrla
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    // Giriþ için bir tarifin olup olmadýðýný kontrol eder
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    // Giriþ için çýkan mutfak objesini alýr
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    // Giriþ için uygun kýzartma tarifini bulur
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    // Giriþ için uygun yanma tarifini bulur
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    // Kýzarmýþ olup olmadýðýný kontrol eder
    public bool IsFried()
    {
        return state == State.Fried;
    }
}
