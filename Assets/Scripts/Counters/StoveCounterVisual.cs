using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{

    // StoveCounter bile�enini ba�lamak i�in serialize edilen alan
    [SerializeField] private StoveCounter stoveCounter;
    // Ocak yand���nda g�sterilecek olan GameObject (�rne�in, oca��n �����)
    [SerializeField] private GameObject stoveOnGameObject;
    // Ocakta �al��an bir g�rsel efekt (�rne�in, duman veya ate� partik�lleri)
    [SerializeField] private GameObject particlesGameObject;

    // Start metodunda, StoveCounter bile�eninin olaylar� dinlemeye ba�lan�r
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged; // Ocak durumu de�i�ti�inde tetiklenen olay
    }

    // StoveCounter �zerinde durum de�i�ikli�i oldu�unda �a�r�l�r
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        // Ocak, k�zartma veya k�zarm�� durumda oldu�unda g�rsel efektlerin aktif olmas� gerekti�ini belirler
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        // Ocak yan�yorsa (k�zartma veya k�zarm�� durumda), g�rsel ��eler aktifle�tirilir
        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual); // Partik�ller (duman veya ate�) aktif edilir
    }
}
