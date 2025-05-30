using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{

    // Olaylar
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged; // �lerleme durumu de�i�ti�inde tetiklenir
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged; // Durum de�i�ti�inde tetiklenir

    // State de�i�ikliklerini temsil eden s�n�f
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state; // Durum (Idle, Frying, Fried, Burned)
    }

    // Frit�z�n durumlar�n� temsil eden enum
    public enum State
    {
        Idle,       // Beklemede
        Frying,     // K�zart�l�yor
        Fried,      // K�zart�lm��
        Burned,     // Yanm��
    }

    // Frit�z�n kullanabilece�i tarifler
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray; // K�zartma tarifleri
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray; // Yanma tarifleri

    // Durum, zamanlay�c� ve tarifler
    private State state;
    private float fryingTimer; // K�zartma s�resi
    private FryingRecipeSO fryingRecipeSO; // K�zartma tarifine ait bilgiler
    private float burningTimer; // Yanma s�resi
    private BurningRecipeSO burningRecipeSO; // Yanma tarifine ait bilgiler

    // Ba�lang��ta durum "Idle" olarak ayarlan�r
    private void Start()
    {
        state = State.Idle;
    }

    // Her frame'de g�ncellenen metod
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break; // Beklemede oldu�unda hi�bir i�lem yap�lmaz
                case State.Frying:
                    fryingTimer += Time.deltaTime; // Zamanlay�c�y� art�r

                    // �lerleme durumu olay�n� tetikle
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    // K�zartma s�resi bitti�inde
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        // K�zart�lm�� nesne yarat�l�r
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried; // Durumu Fried olarak de�i�tir
                        burningTimer = 0f; // Yanma zamanlay�c�s�n� s�f�rla
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()); // Yanma tarifini al

                        // Durum de�i�ikli�i olay�n� tetikle
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime; // Yanma zamanlay�c�s�n� art�r

                    // �lerleme durumu olay�n� tetikle
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    // Yanma s�resi bitti�inde
                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        // Yanm�� nesne yarat�l�r
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned; // Durumu Burned olarak de�i�tir

                        // Durum de�i�ikli�i olay�n� tetikle
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        // �lerleme durumunu s�f�rla
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break; // Yanm�� durumda hi�bir i�lem yap�lmaz
            }
        }
    }

    // Player ile etkile�ime girmeyi sa�layan metod
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Tezgah bo�sa
            if (player.HasKitchenObject())
            {
                // Player bir mutfak objesi ta��yor
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player k�zart�labilir bir �ey ta��yor
                    player.GetKitchenObject().SetKitchenObjectParent(this); // Nesneyi bu tezgaha yerle�tir

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()); // K�zartma tarifini al

                    state = State.Frying; // Durumu Frying olarak de�i�tir
                    fryingTimer = 0f; // K�zartma zamanlay�c�s�n� s�f�rla

                    // Durum de�i�ikli�i olay�n� tetikle
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    // �lerleme durumu olay�n� tetikle
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
            else
            {
                // Player hi�bir �ey ta��m�yor
            }
        }
        else
        {
            // Tezgah�n �zerinde mutfak objesi var
            if (player.HasKitchenObject())
            {
                // Player bir mutfak objesi ta��yor
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player bir tabak ta��yor
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf(); // Tezgah�n �zerindeki objeyi sil

                        state = State.Idle; // Durumu Idle olarak de�i�tir

                        // Durum de�i�ikli�i olay�n� tetikle
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        // �lerleme durumu olay�n� s�f�rla
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // Player hi�bir �ey ta��m�yor
                GetKitchenObject().SetKitchenObjectParent(player); // Nesneyi Player'a ver

                state = State.Idle; // Durumu Idle olarak de�i�tir

                // Durum de�i�ikli�i olay�n� tetikle
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                // �lerleme durumu olay�n� s�f�rla
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    // Giri� i�in bir tarifin olup olmad���n� kontrol eder
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    // Giri� i�in ��kan mutfak objesini al�r
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

    // Giri� i�in uygun k�zartma tarifini bulur
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

    // Giri� i�in uygun yanma tarifini bulur
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

    // K�zarm�� olup olmad���n� kontrol eder
    public bool IsFried()
    {
        return state == State.Fried;
    }
}
