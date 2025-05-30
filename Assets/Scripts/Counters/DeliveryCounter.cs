using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    // Singleton pattern, DeliveryCounter'ýn tek örneðine eriþimi saðlar
    public static DeliveryCounter Instance { get; private set; }

    // Awake metodu, DeliveryCounter'ýn tek örneðini baþlatýr
    private void Awake()
    {
        Instance = this;
    }

    // Player ile etkileþimde bulunmak için override edilen Interact metodu
    public override void Interact(Player player)
    {
        // Eðer Player bir mutfak nesnesi taþýyorsa
        if (player.HasKitchenObject())
        {
            // Eðer Player, bir Tabak taþýyorsa
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // Yalnýzca Tabak kabul edilir

                // Teslimatý yapar ve tabak nesnesini teslim edilir
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                // Teslim edilen tabak nesnesini yok eder
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
