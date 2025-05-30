using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    // StoveCounter (ocak sayac�) bile�eni
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        // StoveCounter'dan gelen ilerleme de�i�ikliklerine abone ol
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        // Ba�lang��ta uyar� panelini gizle
        Hide();
    }

    // StoveCounter'daki ilerleme de�i�ti�inde �a�r�l�r
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Yanm�� g�stergeyi g�stermek i�in gereken ilerleme miktar�
        float burnShowProgressAmount = .5f;

        // E�er ocak k�zarm��sa ve ilerleme belirli bir seviyeye ula�m��sa, yanma uyar�s�n� g�ster
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        // Yanma durumu g�sterilmesi gerekiyorsa uyar�y� g�ster, aksi takdirde gizle
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    // Uyar� panelini g�ster
    private void Show()
    {
        gameObject.SetActive(true);
    }

    // Uyar� panelini gizle
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
