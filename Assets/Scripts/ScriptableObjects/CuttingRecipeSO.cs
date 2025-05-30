using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{

    // Bu s�n�f, bir "kesme" tarifinin tan�m�n� yapar.
    // Oyun i�indeki belirli nesnelerin, belirli bir ilerleme ile kesilerek ne elde edilece�ini belirler.

    // Girdi olarak kullan�lacak mutfak objesi (kesilecek nesne)
    public KitchenObjectSO input;

    // Kesme i�lemi sonras� elde edilecek mutfak objesi (kesilmi� nesne)
    public KitchenObjectSO output;

    // Kesme i�lemi i�in gereken maksimum ilerleme adedi
    // Bu, nesnenin tamamen kesilmesi i�in tamamlanmas� gereken ad�m say�s�n� belirtir
    public int cuttingProgressMax;

}
