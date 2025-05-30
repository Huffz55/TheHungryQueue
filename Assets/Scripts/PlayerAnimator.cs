using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    // Animator parametresi için kullanýlan sabit deðer
    private const string IS_WALKING = "IsWalking";

    // Player bileþeni, animatörün kontrol edeceði hareket durumunu almak için
    [SerializeField] private Player player;

    // Animator bileþeni, animasyonlarý yönetmek için
    private Animator animator;

    // Awake, script yüklendiðinde ilk olarak çalýþýr
    private void Awake()
    {
        // Animator bileþenini alýyoruz
        animator = GetComponent<Animator>();
    }

    // Update, her frame'de bir kez çalýþýr
    private void Update()
    {
        // Player'ýn yürüyüp yürümediðini kontrol edip animatöre bildiriyoruz
        animator.SetBool(IS_WALKING, player.IsWalking());
    }

}
