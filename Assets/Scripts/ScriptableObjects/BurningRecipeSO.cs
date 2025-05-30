using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{

    // Bu sýnýf, bir "yakma" tarifinin tanýmýný yapar. 
    // Oyun içindeki belirli nesnelerin, belirli bir süre boyunca yakýlmasýyla ne elde edileceðini belirler.

    // Girdi olarak kullanýlacak mutfak objesi (yakýlacak nesne)
    public KitchenObjectSO input;

    // Çýktý olarak elde edilecek mutfak objesi (yakma iþlemi sonrasýnda elde edilen nesne)
    public KitchenObjectSO output;

    // Yanmaya baþlama süresi (yakma iþlemi için gereken süre)
    public float burningTimerMax;

}
