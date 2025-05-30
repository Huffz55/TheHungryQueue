using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    // Player bileþenine referans, oyuncunun hareketini kontrol etmek için
    private Player player;

    // Ayak sesi çalma zamanlayýcýsý
    private float footstepTimer;

    // Ayak sesi için maksimum zaman aralýðý (her adým için çalma sýklýðý)
    private float footstepTimerMax = .1f;

    // Awake, script yüklendiðinde ilk olarak çalýþýr
    private void Awake()
    {
        // Player bileþenini alýyoruz
        player = GetComponent<Player>();
    }

    // Update, her frame'de bir kez çalýþýr
    private void Update()
    {
        // Zamanlayýcýyý azaltýyoruz
        footstepTimer -= Time.deltaTime;

        // Eðer zamanlayýcý sýfýrsa, bir adým sesi çalmak için sýfýrlýyoruz
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax; // Zamanlayýcýyý sýfýrlýyoruz

            // Eðer oyuncu yürüyor ise
            if (player.IsWalking())
            {
                // Sesin volume seviyesini belirliyoruz
                float volume = 1f;

                // SoundManager'dan ayak sesi çalmasýný saðlýyoruz
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, volume);
            }
        }
    }
}
