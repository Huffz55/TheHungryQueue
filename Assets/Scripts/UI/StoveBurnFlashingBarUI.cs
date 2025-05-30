using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    // "IsFlashing" animasyon parametresinin adý
    private const string IS_FLASHING = "IsFlashing";

    // StoveCounter (ocak sayacý) bileþeni
    [SerializeField] private StoveCounter stoveCounter;

    // Animator bileþeni (animasyonlarý kontrol etmek için)
    private Animator animator;

    private void Awake()
    {
        // Animator bileþenini al
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // StoveCounter'dan gelen ilerleme deðiþikliklerine abone ol
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        // Baþlangýçta animasyonda "flashing" özelliðini devre dýþý býrak
        animator.SetBool(IS_FLASHING, false);
    }

    // StoveCounter'daki ilerleme deðiþtiðinde çaðrýlýr
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Yanmýþ göstergeyi göstermek için gereken ilerleme miktarý
        float burnShowProgressAmount = .5f;

        // Eðer ocak kýzarmýþsa ve ilerleme belirli bir seviyeye ulaþmýþsa, yanma göstergesini göster
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        // Animator parametresini güncelle, göstergeyi yanmaya baþlat
        animator.SetBool(IS_FLASHING, show);
    }
}
