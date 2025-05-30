using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    // Yeni bir tabak olu�turuldu�unda tetiklenen event
    public event EventHandler OnPlateSpawned;
    // Bir tabak al�nd���nda tetiklenen event
    public event EventHandler OnPlateRemoved;

    // Kullan�lacak tabak nesnesi
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    // Tabak olu�turma zamanlay�c�lar�
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f; // Tabak ba��na ge�en s�re (4 saniye)

    // Olu�turulan tabak say�s�
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4; // Maksimum tabak say�s�

    // Update metodunda zamanla tabak spawn i�lemi yap�l�r
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime; // Zamanlay�c�y� artt�r

        // Zamanlay�c� belirtilen maksimum s�reyi a�arsa
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f; // Zamanlay�c�y� s�f�rla

            // E�er oyun oynan�yorsa ve olu�turulan tabak say�s� maksimumdan azsa
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++; // Olu�turulan tabak say�s�n� bir artt�r

                // Yeni bir tabak olu�turuldu�unda event tetiklenir
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Oyuncunun etkile�imde bulundu�u metod
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // E�er oyuncunun elinde hi�bir �ey yoksa
            if (platesSpawnedAmount > 0)
            {
                // E�er burada en az bir tabak varsa
                platesSpawnedAmount--; // Tabak say�s�n� bir azalt

                // Yeni bir tabak, oyuncunun eline verilir
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                // Tabak al�nd���nda event tetiklenir
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
