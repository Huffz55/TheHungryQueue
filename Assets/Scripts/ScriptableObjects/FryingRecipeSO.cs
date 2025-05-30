using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{

    // Bu s�n�f, bir "k�zartma" tarifinin tan�m�n� yapar.
    // Belirli bir malzemenin, belirli bir s�re boyunca k�zart�lmak �zere nas�l i�lenece�ini belirler.

    // K�zartma i�lemine ba�lanacak mutfak objesi (�rne�in, et, sebze vb.)
    public KitchenObjectSO input;

    // K�zartma i�lemi sonras�nda elde edilecek mutfak objesi (�rne�in, k�zarm�� et, sebze)
    public KitchenObjectSO output;

    // K�zartma i�lemi i�in gereken maksimum zaman
    // Bu, malzemenin tamamen k�zarmas� i�in ge�mesi gereken s�redir (saniye cinsinden)
    public float fryingTimerMax;

}
