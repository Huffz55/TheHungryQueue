using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{

    // Bu sýnýf, bir "kýzartma" tarifinin tanýmýný yapar.
    // Belirli bir malzemenin, belirli bir süre boyunca kýzartýlmak üzere nasýl iþleneceðini belirler.

    // Kýzartma iþlemine baþlanacak mutfak objesi (örneðin, et, sebze vb.)
    public KitchenObjectSO input;

    // Kýzartma iþlemi sonrasýnda elde edilecek mutfak objesi (örneðin, kýzarmýþ et, sebze)
    public KitchenObjectSO output;

    // Kýzartma iþlemi için gereken maksimum zaman
    // Bu, malzemenin tamamen kýzarmasý için geçmesi gereken süredir (saniye cinsinden)
    public float fryingTimerMax;

}
