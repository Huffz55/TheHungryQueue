using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{

    // StoveCounter bileþenini baðlamak için serialize edilen alan
    [SerializeField] private StoveCounter stoveCounter;
    // Ocak yandýðýnda gösterilecek olan GameObject (örneðin, ocaðýn ýþýðý)
    [SerializeField] private GameObject stoveOnGameObject;
    // Ocakta çalýþan bir görsel efekt (örneðin, duman veya ateþ partikülleri)
    [SerializeField] private GameObject particlesGameObject;

    // Start metodunda, StoveCounter bileþeninin olaylarý dinlemeye baþlanýr
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged; // Ocak durumu deðiþtiðinde tetiklenen olay
    }

    // StoveCounter üzerinde durum deðiþikliði olduðunda çaðrýlýr
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        // Ocak, kýzartma veya kýzarmýþ durumda olduðunda görsel efektlerin aktif olmasý gerektiðini belirler
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        // Ocak yanýyorsa (kýzartma veya kýzarmýþ durumda), görsel öðeler aktifleþtirilir
        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual); // Partiküller (duman veya ateþ) aktif edilir
    }
}
