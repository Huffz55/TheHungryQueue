using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    // Bu, tezgaha yerleþtirilebilecek mutfak nesnesinin tipini tanýmlar
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Bu metod, oyuncunun bu tezgah ile etkileþimde bulunmasýný saðlar
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Eðer tezgah üzerinde mutfak nesnesi yoksa
            if (player.HasKitchenObject())
            {
                // Eðer oyuncu bir nesne taþýyorsa
                player.GetKitchenObject().SetKitchenObjectParent(this); // Nesneyi bu tezgaha yerleþtir
            }
            else
            {
                // Eðer oyuncu hiçbir nesne taþýmýyorsa
                // Yapýlacak bir þey yok, çünkü hiçbir þey yerleþtirilemez
            }
        }
        else
        {
            // Eðer tezgah üzerinde mutfak nesnesi varsa
            if (player.HasKitchenObject())
            {
                // Eðer oyuncu bir nesne taþýyorsa
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Eðer oyuncu bir tabak taþýyorsa
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // Tabak, bu tezgahýn üzerindeki malzemeyi ekleyebiliyorsa
                        GetKitchenObject().DestroySelf(); // Tezgahýn üzerindeki nesneyi yok et
                    }
                }
                else
                {
                    // Eðer oyuncu tabak taþýmýyorsa ama baþka bir þey taþýyorsa
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Eðer tezgahýn üzerinde bir tabak varsa
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            // Tabak, oyuncunun taþýdýðý malzemeyi ekleyebiliyorsa
                            player.GetKitchenObject().DestroySelf(); // Oyuncunun taþýdýðý nesneyi yok et
                        }
                    }
                }
            }
            else
            {
                // Eðer oyuncu hiçbir þey taþýmýyorsa
                GetKitchenObject().SetKitchenObjectParent(player); // Tezgahýn üzerindeki nesneyi oyuncuya yerleþtir
            }
        }
    }

}
