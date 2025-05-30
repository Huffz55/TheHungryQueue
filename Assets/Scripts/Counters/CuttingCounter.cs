using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

    // Event: Bir kesme iþlemi yapýldýðýnda tetiklenir
    public static event EventHandler OnAnyCut;

    // Statik verileri sýfýrlar
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    // Event: Kesme iþlemi sýrasýnda ilerleme deðiþtiðinde tetiklenir
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    // Event: Kesme iþlemi tamamlandýðýnda tetiklenir
    public event EventHandler OnCut;

    // Kesme tariflerinin dizisi
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    // Kesme iþlemi ilerleme durumu
    private int cuttingProgress;

    // Player'ýn etkileþimde bulunduðu kesme tezgahý
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Tezgah boþsa ve oyuncu taþýyor
            if (player.HasKitchenObject())
            {
                // Oyuncu kesilebilecek bir þey taþýyor
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Oyuncu kesilebilen bir nesne taþýyor, iþlemi baþlat
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    // Tarife göre kesme iþlemi baþlatýlýyor
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    // Kesme ilerlemesi normalleþtirilmiþ olarak event'e gönderilir
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
            else
            {
                // Oyuncu hiçbir þey taþýmýyor
            }
        }
        else
        {
            // Tezgah zaten dolu
            if (player.HasKitchenObject())
            {
                // Oyuncu baþka bir þey taþýyor
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Oyuncu bir tabak taþýyor
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // Tabak içine malzeme ekleniyor
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                // Oyuncu hiçbir þey taþýmýyor
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    // Kesme iþleminin ikinci etkileþimi
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // Eðer tezgahta bir nesne varsa ve kesilebiliyorsa
            cuttingProgress++;

            // Kesme iþlemi tetikleniyor
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            // Kesme iþlemi ilerlemesi güncelleniyor
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            // Kesme iþlemi tamamlandýðýnda nesne üretimi yapýlýr
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                // Ýlk nesne yok ediliyor
                GetKitchenObject().DestroySelf();

                // Kesilen nesne ortaya çýkarýlýyor
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    // Verilen bir mutfak objesi için bir kesme tarifinin olup olmadýðýný kontrol eder
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    // Bir girdi objesi için çýkýþ objesini döndürür
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    // Girdi objesiyle ilgili kesme tarifini bulur
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
