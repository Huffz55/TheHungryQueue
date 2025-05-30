using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{

    // Animator parametresi için animasyon tetikleyicisi adý
    private const string OPEN_CLOSE = "OpenClose";

    // ContainerCounter bileþenini referans olarak alýr
    [SerializeField] private ContainerCounter containerCounter;

    // Animator bileþenini tutacak deðiþken
    private Animator animator;

    // Awake metodunda Animator bileþenini alýr
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start metodunda, ContainerCounter'dan olay dinlemeye baþlar
    private void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    // OnPlayerGrabbedObject eventi tetiklendiðinde çaðrýlýr
    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        // Animasyonun açýlýp kapanmasýný tetikler
        animator.SetTrigger(OPEN_CLOSE);
    }
}
