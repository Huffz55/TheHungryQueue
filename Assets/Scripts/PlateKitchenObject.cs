using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{

    // Ingredient eklendi�inde tetiklenecek event
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    // Ingredient eklenme olay�n�n parametreleri
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;  // Eklenen ingredient'in bilgisi
    }

    // Ge�erli olan malzeme tiplerinin listesi (taba�a eklenebilecek valid i�erikler)
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    // Taba�a eklenen i�eriklerin listesi
    private List<KitchenObjectSO> kitchenObjectSOList;

    // Ba�lang��ta i�erik listesini ba�lat�yoruz
    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    // Yeni bir ingredient eklemeye �al���r
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        // E�er malzeme ge�erli de�ilse, ekleme i�lemi ba�ar�s�zd�r
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }

        // E�er bu malzeme zaten varsa, ekleme i�lemi ba�ar�s�zd�r
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            // Malzeme ge�erli ve hen�z eklenmemi�se, listeye eklenir
            kitchenObjectSOList.Add(kitchenObjectSO);

            // Ingredient eklenmesi olay� tetiklenir
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });

            return true;  // Ba�ar�l� bir �ekilde ingredient eklenmi�tir
        }
    }

    // Taba�a eklenen t�m i�erikleri d�nd�ren fonksiyon
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
