using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

    // Event: Bir kesme i�lemi yap�ld���nda tetiklenir
    public static event EventHandler OnAnyCut;

    // Statik verileri s�f�rlar
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    // Event: Kesme i�lemi s�ras�nda ilerleme de�i�ti�inde tetiklenir
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    // Event: Kesme i�lemi tamamland���nda tetiklenir
    public event EventHandler OnCut;

    // Kesme tariflerinin dizisi
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    // Kesme i�lemi ilerleme durumu
    private int cuttingProgress;

    // Player'�n etkile�imde bulundu�u kesme tezgah�
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Tezgah bo�sa ve oyuncu ta��yor
            if (player.HasKitchenObject())
            {
                // Oyuncu kesilebilecek bir �ey ta��yor
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Oyuncu kesilebilen bir nesne ta��yor, i�lemi ba�lat
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    // Tarife g�re kesme i�lemi ba�lat�l�yor
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    // Kesme ilerlemesi normalle�tirilmi� olarak event'e g�nderilir
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
            else
            {
                // Oyuncu hi�bir �ey ta��m�yor
            }
        }
        else
        {
            // Tezgah zaten dolu
            if (player.HasKitchenObject())
            {
                // Oyuncu ba�ka bir �ey ta��yor
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Oyuncu bir tabak ta��yor
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // Tabak i�ine malzeme ekleniyor
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                // Oyuncu hi�bir �ey ta��m�yor
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    // Kesme i�leminin ikinci etkile�imi
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // E�er tezgahta bir nesne varsa ve kesilebiliyorsa
            cuttingProgress++;

            // Kesme i�lemi tetikleniyor
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            // Kesme i�lemi ilerlemesi g�ncelleniyor
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            // Kesme i�lemi tamamland���nda nesne �retimi yap�l�r
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                // �lk nesne yok ediliyor
                GetKitchenObject().DestroySelf();

                // Kesilen nesne ortaya ��kar�l�yor
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    // Verilen bir mutfak objesi i�in bir kesme tarifinin olup olmad���n� kontrol eder
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    // Bir girdi objesi i�in ��k�� objesini d�nd�r�r
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
