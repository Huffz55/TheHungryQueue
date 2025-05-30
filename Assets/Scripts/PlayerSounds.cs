using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    // Player bile�enine referans, oyuncunun hareketini kontrol etmek i�in
    private Player player;

    // Ayak sesi �alma zamanlay�c�s�
    private float footstepTimer;

    // Ayak sesi i�in maksimum zaman aral��� (her ad�m i�in �alma s�kl���)
    private float footstepTimerMax = .1f;

    // Awake, script y�klendi�inde ilk olarak �al���r
    private void Awake()
    {
        // Player bile�enini al�yoruz
        player = GetComponent<Player>();
    }

    // Update, her frame'de bir kez �al���r
    private void Update()
    {
        // Zamanlay�c�y� azalt�yoruz
        footstepTimer -= Time.deltaTime;

        // E�er zamanlay�c� s�f�rsa, bir ad�m sesi �almak i�in s�f�rl�yoruz
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax; // Zamanlay�c�y� s�f�rl�yoruz

            // E�er oyuncu y�r�yor ise
            if (player.IsWalking())
            {
                // Sesin volume seviyesini belirliyoruz
                float volume = 1f;

                // SoundManager'dan ayak sesi �almas�n� sa�l�yoruz
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, volume);
            }
        }
    }
}
