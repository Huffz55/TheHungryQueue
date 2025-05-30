using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    // StoveCounter (ocak sayacý) bileþeni
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        // StoveCounter'dan gelen ilerleme deðiþikliklerine abone ol
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        // Baþlangýçta uyarý panelini gizle
        Hide();
    }

    // StoveCounter'daki ilerleme deðiþtiðinde çaðrýlýr
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Yanmýþ göstergeyi göstermek için gereken ilerleme miktarý
        float burnShowProgressAmount = .5f;

        // Eðer ocak kýzarmýþsa ve ilerleme belirli bir seviyeye ulaþmýþsa, yanma uyarýsýný göster
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        // Yanma durumu gösterilmesi gerekiyorsa uyarýyý göster, aksi takdirde gizle
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    // Uyarý panelini göster
    private void Show()
    {
        gameObject.SetActive(true);
    }

    // Uyarý panelini gizle
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
