using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{

    // Animator tetikleyicisini temsil eden sabit deðiþken
    private const string CUT = "Cut";

    // CuttingCounter sýnýfýna referans
    [SerializeField] private CuttingCounter cuttingCounter;

    // Animator bileþeni
    private Animator animator;

    // Awake metodu, animator bileþenini alýr
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start metodu, kesme iþlemi baþlatýldýðýnda tetiklenir
    private void Start()
    {
        // CuttingCounter sýnýfýndaki OnCut event'ine abone olunur
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    // CuttingCounter'da bir kesme iþlemi gerçekleþtiðinde tetiklenir
    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        // Kesme animasyonunu baþlatmak için animator'a "Cut" tetikleyicisi gönderilir
        animator.SetTrigger(CUT);
    }
}
