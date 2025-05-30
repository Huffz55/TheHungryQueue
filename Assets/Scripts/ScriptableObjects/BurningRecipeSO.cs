using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{

    // Bu s�n�f, bir "yakma" tarifinin tan�m�n� yapar. 
    // Oyun i�indeki belirli nesnelerin, belirli bir s�re boyunca yak�lmas�yla ne elde edilece�ini belirler.

    // Girdi olarak kullan�lacak mutfak objesi (yak�lacak nesne)
    public KitchenObjectSO input;

    // ��kt� olarak elde edilecek mutfak objesi (yakma i�lemi sonras�nda elde edilen nesne)
    public KitchenObjectSO output;

    // Yanmaya ba�lama s�resi (yakma i�lemi i�in gereken s�re)
    public float burningTimerMax;

}
