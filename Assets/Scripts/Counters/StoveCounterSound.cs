using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

    // StoveCounter bileþenini baðlamak için kullanýlan serialize edilmiþ alan
    [SerializeField] private StoveCounter stoveCounter;

    // AudioSource bileþeni, ses çalmayý saðlamak için kullanýlýr
    private AudioSource audioSource;
    // Uyarý sesi için zamanlayýcý
    private float warningSoundTimer;
    // Uyarý sesinin çalmasý gerektiðini belirten bayrak
    private bool playWarningSound;

    // Awake metodunda, AudioSource bileþeni alýnýr
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start metodunda, StoveCounter bileþeninin olaylarý dinlenmeye baþlanýr
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged; // Durum deðiþikliðini dinler
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged; // Ýlerleme durumu deðiþimini dinler
    }

    // StoveCounter üzerindeki ilerleme durumu deðiþtiðinde çaðrýlýr
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Yanma sürecinin uyarý sesini çalmaya baþlamasý için ilerleme miktarýnýn yarýsýna ulaþýlmasý gerektiði belirlenmiþtir
        float burnShowProgressAmount = .5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    // StoveCounter üzerindeki durum deðiþtiðinde çaðrýlýr
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        // Kýzartma veya kýzartýlmýþ durumda ses çalmak gerekir
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play(); // Ses çalmaya baþla
        }
        else
        {
            audioSource.Pause(); // Ses duraklat
        }
    }

    // Her frame'de güncellenir, uyarý sesini çalma zamaný kontrol edilir
    private void Update()
    {
        if (playWarningSound)
        {
            // Zamanlayýcýyý güncelle
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0f)
            {
                // Zamanlayýcý sýfýrlandýðýnda uyarý sesini çal
                float warningSoundTimerMax = .2f; // Ses aralýðý (0.2 saniye)
                warningSoundTimer = warningSoundTimerMax;

                // Uyarý sesini çal
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }

}
