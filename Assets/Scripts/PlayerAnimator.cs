using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    // Animator parametresi i�in kullan�lan sabit de�er
    private const string IS_WALKING = "IsWalking";

    // Player bile�eni, animat�r�n kontrol edece�i hareket durumunu almak i�in
    [SerializeField] private Player player;

    // Animator bile�eni, animasyonlar� y�netmek i�in
    private Animator animator;

    // Awake, script y�klendi�inde ilk olarak �al���r
    private void Awake()
    {
        // Animator bile�enini al�yoruz
        animator = GetComponent<Animator>();
    }

    // Update, her frame'de bir kez �al���r
    private void Update()
    {
        // Player'�n y�r�y�p y�r�medi�ini kontrol edip animat�re bildiriyoruz
        animator.SetBool(IS_WALKING, player.IsWalking());
    }

}
