using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{

    // Animator tetikleyicisini temsil eden sabit de�i�ken
    private const string CUT = "Cut";

    // CuttingCounter s�n�f�na referans
    [SerializeField] private CuttingCounter cuttingCounter;

    // Animator bile�eni
    private Animator animator;

    // Awake metodu, animator bile�enini al�r
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start metodu, kesme i�lemi ba�lat�ld���nda tetiklenir
    private void Start()
    {
        // CuttingCounter s�n�f�ndaki OnCut event'ine abone olunur
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    // CuttingCounter'da bir kesme i�lemi ger�ekle�ti�inde tetiklenir
    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        // Kesme animasyonunu ba�latmak i�in animator'a "Cut" tetikleyicisi g�nderilir
        animator.SetTrigger(CUT);
    }
}
