using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

    // StoveCounter bile�enini ba�lamak i�in kullan�lan serialize edilmi� alan
    [SerializeField] private StoveCounter stoveCounter;

    // AudioSource bile�eni, ses �almay� sa�lamak i�in kullan�l�r
    private AudioSource audioSource;
    // Uyar� sesi i�in zamanlay�c�
    private float warningSoundTimer;
    // Uyar� sesinin �almas� gerekti�ini belirten bayrak
    private bool playWarningSound;

    // Awake metodunda, AudioSource bile�eni al�n�r
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start metodunda, StoveCounter bile�eninin olaylar� dinlenmeye ba�lan�r
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged; // Durum de�i�ikli�ini dinler
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged; // �lerleme durumu de�i�imini dinler
    }

    // StoveCounter �zerindeki ilerleme durumu de�i�ti�inde �a�r�l�r
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Yanma s�recinin uyar� sesini �almaya ba�lamas� i�in ilerleme miktar�n�n yar�s�na ula��lmas� gerekti�i belirlenmi�tir
        float burnShowProgressAmount = .5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    // StoveCounter �zerindeki durum de�i�ti�inde �a�r�l�r
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        // K�zartma veya k�zart�lm�� durumda ses �almak gerekir
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play(); // Ses �almaya ba�la
        }
        else
        {
            audioSource.Pause(); // Ses duraklat
        }
    }

    // Her frame'de g�ncellenir, uyar� sesini �alma zaman� kontrol edilir
    private void Update()
    {
        if (playWarningSound)
        {
            // Zamanlay�c�y� g�ncelle
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0f)
            {
                // Zamanlay�c� s�f�rland���nda uyar� sesini �al
                float warningSoundTimerMax = .2f; // Ses aral��� (0.2 saniye)
                warningSoundTimer = warningSoundTimerMax;

                // Uyar� sesini �al
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }

}
