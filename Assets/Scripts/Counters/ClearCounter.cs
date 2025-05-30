using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    // Bu, tezgaha yerle�tirilebilecek mutfak nesnesinin tipini tan�mlar
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Bu metod, oyuncunun bu tezgah ile etkile�imde bulunmas�n� sa�lar
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // E�er tezgah �zerinde mutfak nesnesi yoksa
            if (player.HasKitchenObject())
            {
                // E�er oyuncu bir nesne ta��yorsa
                player.GetKitchenObject().SetKitchenObjectParent(this); // Nesneyi bu tezgaha yerle�tir
            }
            else
            {
                // E�er oyuncu hi�bir nesne ta��m�yorsa
                // Yap�lacak bir �ey yok, ��nk� hi�bir �ey yerle�tirilemez
            }
        }
        else
        {
            // E�er tezgah �zerinde mutfak nesnesi varsa
            if (player.HasKitchenObject())
            {
                // E�er oyuncu bir nesne ta��yorsa
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // E�er oyuncu bir tabak ta��yorsa
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // Tabak, bu tezgah�n �zerindeki malzemeyi ekleyebiliyorsa
                        GetKitchenObject().DestroySelf(); // Tezgah�n �zerindeki nesneyi yok et
                    }
                }
                else
                {
                    // E�er oyuncu tabak ta��m�yorsa ama ba�ka bir �ey ta��yorsa
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // E�er tezgah�n �zerinde bir tabak varsa
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            // Tabak, oyuncunun ta��d��� malzemeyi ekleyebiliyorsa
                            player.GetKitchenObject().DestroySelf(); // Oyuncunun ta��d��� nesneyi yok et
                        }
                    }
                }
            }
            else
            {
                // E�er oyuncu hi�bir �ey ta��m�yorsa
                GetKitchenObject().SetKitchenObjectParent(player); // Tezgah�n �zerindeki nesneyi oyuncuya yerle�tir
            }
        }
    }

}
