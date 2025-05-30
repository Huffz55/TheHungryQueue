using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{

    private const string NUMBER_POPUP = "NumberPopup";  // Animator trigger'ý için kullanýlan sabit isim

    [SerializeField] private TextMeshProUGUI countdownText;  // Geri sayým numarasýný gösterecek TextMeshPro bileþeni

    private Animator animator;  // Animator bileþeni
    private int previousCountdownNumber;  // Önceki geri sayým numarasýný tutar

    private void Awake()
    {
        animator = GetComponent<Animator>();  // Animator bileþenini alýr
    }

    private void Start()
    {
        // Oyun durumu deðiþtiðinde geri sayýmý kontrol eden metodu çaðýrýr
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();  // Baþlangýçta geri sayým UI'sýný gizler
    }

    // Oyun durumu deðiþtiðinde çalýþacak metod
    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();  // Geri sayým aktifse UI'yý göster
        }
        else
        {
            Hide();  // Geri sayým aktif deðilse UI'yý gizle
        }
    }

    private void Update()
    {
        // Geri sayým sayýsýný alýr ve ekranda görüntüler
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        // Eðer önceki sayýya göre farklýysa animasyonu baþlatýr ve ses çalar
        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);  // Sayý deðiþtiðinde animasyonu tetikler
            SoundManager.Instance.PlayCountdownSound();  // Geri sayým sesi çalar
        }
    }

    // UI'yi gösterir
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
