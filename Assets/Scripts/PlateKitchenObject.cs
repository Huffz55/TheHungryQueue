using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{

    // Ingredient eklendiðinde tetiklenecek event
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    // Ingredient eklenme olayýnýn parametreleri
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;  // Eklenen ingredient'in bilgisi
    }

    // Geçerli olan malzeme tiplerinin listesi (tabaða eklenebilecek valid içerikler)
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    // Tabaða eklenen içeriklerin listesi
    private List<KitchenObjectSO> kitchenObjectSOList;

    // Baþlangýçta içerik listesini baþlatýyoruz
    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    // Yeni bir ingredient eklemeye çalýþýr
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        // Eðer malzeme geçerli deðilse, ekleme iþlemi baþarýsýzdýr
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }

        // Eðer bu malzeme zaten varsa, ekleme iþlemi baþarýsýzdýr
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            // Malzeme geçerli ve henüz eklenmemiþse, listeye eklenir
            kitchenObjectSOList.Add(kitchenObjectSO);

            // Ingredient eklenmesi olayý tetiklenir
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });

            return true;  // Baþarýlý bir þekilde ingredient eklenmiþtir
        }
    }

    // Tabaða eklenen tüm içerikleri döndüren fonksiyon
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
