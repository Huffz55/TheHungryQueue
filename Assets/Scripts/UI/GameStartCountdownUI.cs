using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{

    private const string NUMBER_POPUP = "NumberPopup";  // Animator trigger'� i�in kullan�lan sabit isim

    [SerializeField] private TextMeshProUGUI countdownText;  // Geri say�m numaras�n� g�sterecek TextMeshPro bile�eni

    private Animator animator;  // Animator bile�eni
    private int previousCountdownNumber;  // �nceki geri say�m numaras�n� tutar

    private void Awake()
    {
        animator = GetComponent<Animator>();  // Animator bile�enini al�r
    }

    private void Start()
    {
        // Oyun durumu de�i�ti�inde geri say�m� kontrol eden metodu �a��r�r
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();  // Ba�lang��ta geri say�m UI's�n� gizler
    }

    // Oyun durumu de�i�ti�inde �al��acak metod
    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();  // Geri say�m aktifse UI'y� g�ster
        }
        else
        {
            Hide();  // Geri say�m aktif de�ilse UI'y� gizle
        }
    }

    private void Update()
    {
        // Geri say�m say�s�n� al�r ve ekranda g�r�nt�ler
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        // E�er �nceki say�ya g�re farkl�ysa animasyonu ba�lat�r ve ses �alar
        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);  // Say� de�i�ti�inde animasyonu tetikler
            SoundManager.Instance.PlayCountdownSound();  // Geri say�m sesi �alar
        }
    }

    // UI'yi g�sterir
    private void Show()
    {
        gameObject.SetActive(true);
    }

    // UI'yi gizler
    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
