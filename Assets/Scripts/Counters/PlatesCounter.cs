using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    // Yeni bir tabak oluþturulduðunda tetiklenen event
    public event EventHandler OnPlateSpawned;
    // Bir tabak alýndýðýnda tetiklenen event
    public event EventHandler OnPlateRemoved;

    // Kullanýlacak tabak nesnesi
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    // Tabak oluþturma zamanlayýcýlarý
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f; // Tabak baþýna geçen süre (4 saniye)

    // Oluþturulan tabak sayýsý
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4; // Maksimum tabak sayýsý

    // Update metodunda zamanla tabak spawn iþlemi yapýlýr
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime; // Zamanlayýcýyý arttýr

        // Zamanlayýcý belirtilen maksimum süreyi aþarsa
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f; // Zamanlayýcýyý sýfýrla

            // Eðer oyun oynanýyorsa ve oluþturulan tabak sayýsý maksimumdan azsa
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++; // Oluþturulan tabak sayýsýný bir arttýr

                // Yeni bir tabak oluþturulduðunda event tetiklenir
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Oyuncunun etkileþimde bulunduðu metod
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Eðer oyuncunun elinde hiçbir þey yoksa
            if (platesSpawnedAmount > 0)
            {
                // Eðer burada en az bir tabak varsa
                platesSpawnedAmount--; // Tabak sayýsýný bir azalt

                // Yeni bir tabak, oyuncunun eline verilir
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                // Tabak alýndýðýnda event tetiklenir
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
