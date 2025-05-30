using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{

    // Animator parametresi i�in animasyon tetikleyicisi ad�
    private const string OPEN_CLOSE = "OpenClose";

    // ContainerCounter bile�enini referans olarak al�r
    [SerializeField] private ContainerCounter containerCounter;

    // Animator bile�enini tutacak de�i�ken
    private Animator animator;

    // Awake metodunda Animator bile�enini al�r
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start metodunda, ContainerCounter'dan olay dinlemeye ba�lar
    private void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    // OnPlayerGrabbedObject eventi tetiklendi�inde �a�r�l�r
    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        // Animasyonun a��l�p kapanmas�n� tetikler
        animator.SetTrigger(OPEN_CLOSE);
    }
}
