using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    // Singleton pattern, DeliveryCounter'�n tek �rne�ine eri�imi sa�lar
    public static DeliveryCounter Instance { get; private set; }

    // Awake metodu, DeliveryCounter'�n tek �rne�ini ba�lat�r
    private void Awake()
    {
        Instance = this;
    }

    // Player ile etkile�imde bulunmak i�in override edilen Interact metodu
    public override void Interact(Player player)
    {
        // E�er Player bir mutfak nesnesi ta��yorsa
        if (player.HasKitchenObject())
        {
            // E�er Player, bir Tabak ta��yorsa
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // Yaln�zca Tabak kabul edilir

                // Teslimat� yapar ve tabak nesnesini teslim edilir
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                // Teslim edilen tabak nesnesini yok eder
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
