using System;
using Lean.Transition;
using TMPro;
using Undercooked.Managers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    // Bu s�n�f, geri say�m aray�z�n� kontrol eder.
    // Oyun ba�lad���nda zaman geri say�m� ekranda g�sterir ve zaman sona erdi�inde aray�z� gizler.
    [RequireComponent(typeof(CanvasGroup))]
    public class CountdownUI : MonoBehaviour
    {
        // Geri say�m�n zaman�n� g�sterecek olan TextMeshPro nesnesi
        [SerializeField] private TextMeshProUGUI text;

        // CanvasGroup, bu UI elementinin g�r�n�rl���n� kontrol etmek i�in kullan�l�r.
        private CanvasGroup _canvasGroup;

        // Awake metodu, bile�enlerin ba�lat�ld��� yerdir.
        private void Awake()
        {
            // CanvasGroup bile�enini al�yoruz.
            _canvasGroup = GetComponent<CanvasGroup>();

#if UNITY_EDITOR
            // Editor modunda, text ve _canvasGroup bile�enlerinin varl���n� do�ruluyoruz.
            Assert.IsNotNull(text);
            Assert.IsNotNull(_canvasGroup);
#endif

            // Ba�lang��ta CanvasGroup'un g�r�n�rl���n� s�f�rl�yoruz (gizli).
            _canvasGroup.alpha = 0f;
        }

        // Bu metot, bu script aktif oldu�unda �a�r�l�r.
        private void OnEnable()
        {
            // Oyun ba�lad�ktan sonra geri say�m t�klamalar�, zaman�n dolmas� ve seviyenin ba�lamas� gibi olaylar� dinleriz.
            GameManager.OnCountdownTick += HandleCountdownTick;
            GameManager.OnLevelStart += HandleLevelStart;
            GameManager.OnTimeIsOver += HandleTimeOver;
        }

        // Bu metot, bu script devre d��� b�rak�ld���nda �a�r�l�r.
        private void OnDisable()
        {
            // Olay dinleyicilerini kald�r�yoruz.
            GameManager.OnCountdownTick -= HandleCountdownTick;
            GameManager.OnLevelStart -= HandleLevelStart;
            GameManager.OnTimeIsOver -= HandleTimeOver;
        }

        // Seviye ba�lad���nda �a�r�l�r.
        // Geri say�m UI'sini g�sterir (alpha = 1).
        private void HandleLevelStart()
        {
            _canvasGroup.alphaTransition(1f, 1f); // Alpha de�eri ge�i�le 1'e ayarlan�r (g�r�n�r).
        }

        // Zaman sona erdi�inde �a�r�l�r.
        // Geri say�m UI'sini gizler (alpha = 0).
        private void HandleTimeOver()
        {
            _canvasGroup.alphaTransition(0f, 1f); // Alpha de�eri ge�i�le 0'a ayarlan�r (gizlenir).
        }

        // Her geri say�m t�klamas� (zaman azald�k�a) bu metot �a�r�l�r.
        // Kalan zaman� m:ss format�nda g�sterir.
        private void HandleCountdownTick(int timeRemaining)
        {
            // Kalan zaman� TimeSpan objesi olarak al�yoruz.
            var timespan = TimeSpan.FromSeconds(timeRemaining);

            // Text objesine, kalan zaman� m:ss format�nda yaz�yoruz.
            text.text = timespan.ToString(@"m\:ss");
        }
    }
}
