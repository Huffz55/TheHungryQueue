using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{

    // Bu sýnýf, bir "kesme" tarifinin tanýmýný yapar.
    // Oyun içindeki belirli nesnelerin, belirli bir ilerleme ile kesilerek ne elde edileceðini belirler.

    // Girdi olarak kullanýlacak mutfak objesi (kesilecek nesne)
    public KitchenObjectSO input;

    // Kesme iþlemi sonrasý elde edilecek mutfak objesi (kesilmiþ nesne)
    public KitchenObjectSO output;

    // Kesme iþlemi için gereken maksimum ilerleme adedi
    // Bu, nesnenin tamamen kesilmesi için tamamlanmasý gereken adým sayýsýný belirtir
    public int cuttingProgressMax;

}
