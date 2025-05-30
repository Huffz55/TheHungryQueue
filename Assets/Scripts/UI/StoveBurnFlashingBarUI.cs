using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    // "IsFlashing" animasyon parametresinin ad�
    private const string IS_FLASHING = "IsFlashing";

    // StoveCounter (ocak sayac�) bile�eni
    [SerializeField] private StoveCounter stoveCounter;

    // Animator bile�eni (animasyonlar� kontrol etmek i�in)
    private Animator animator;

    private void Awake()
    {
        // Animator bile�enini al
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // StoveCounter'dan gelen ilerleme de�i�ikliklerine abone ol
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        // Ba�lang��ta animasyonda "flashing" �zelli�ini devre d��� b�rak
        animator.SetBool(IS_FLASHING, false);
    }

    // StoveCounter'daki ilerleme de�i�ti�inde �a�r�l�r
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Yanm�� g�stergeyi g�stermek i�in gereken ilerleme miktar�
        float burnShowProgressAmount = .5f;

        // E�er ocak k�zarm��sa ve ilerleme belirli bir seviyeye ula�m��sa, yanma g�stergesini g�ster
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        // Animator parametresini g�ncelle, g�stergeyi yanmaya ba�lat
        animator.SetBool(IS_FLASHING, show);
    }
}
