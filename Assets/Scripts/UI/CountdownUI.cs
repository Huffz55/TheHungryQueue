using System;
using Lean.Transition;
using TMPro;
using Undercooked.Managers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    // Bu sýnýf, geri sayým arayüzünü kontrol eder.
    // Oyun baþladýðýnda zaman geri sayýmý ekranda gösterir ve zaman sona erdiðinde arayüzü gizler.
    [RequireComponent(typeof(CanvasGroup))]
    public class CountdownUI : MonoBehaviour
    {
        // Geri sayýmýn zamanýný gösterecek olan TextMeshPro nesnesi
        [SerializeField] private TextMeshProUGUI text;

        // CanvasGroup, bu UI elementinin görünürlüðünü kontrol etmek için kullanýlýr.
        private CanvasGroup _canvasGroup;

        // Awake metodu, bileþenlerin baþlatýldýðý yerdir.
        private void Awake()
        {
            // CanvasGroup bileþenini alýyoruz.
            _canvasGroup = GetComponent<CanvasGroup>();

#if UNITY_EDITOR
            // Editor modunda, text ve _canvasGroup bileþenlerinin varlýðýný doðruluyoruz.
            Assert.IsNotNull(text);
            Assert.IsNotNull(_canvasGroup);
#endif

            // Baþlangýçta CanvasGroup'un görünürlüðünü sýfýrlýyoruz (gizli).
            _canvasGroup.alpha = 0f;
        }

        // Bu metot, bu script aktif olduðunda çaðrýlýr.
        private void OnEnable()
        {
            // Oyun baþladýktan sonra geri sayým týklamalarý, zamanýn dolmasý ve seviyenin baþlamasý gibi olaylarý dinleriz.
            GameManager.OnCountdownTick += HandleCountdownTick;
            GameManager.OnLevelStart += HandleLevelStart;
            GameManager.OnTimeIsOver += HandleTimeOver;
        }

        // Bu metot, bu script devre dýþý býrakýldýðýnda çaðrýlýr.
        private void OnDisable()
        {
            // Olay dinleyicilerini kaldýrýyoruz.
            GameManager.OnCountdownTick -= HandleCountdownTick;
            GameManager.OnLevelStart -= HandleLevelStart;
            GameManager.OnTimeIsOver -= HandleTimeOver;
        }

        // Seviye baþladýðýnda çaðrýlýr.
        // Geri sayým UI'sini gösterir (alpha = 1).
        private void HandleLevelStart()
        {
            _canvasGroup.alphaTransition(1f, 1f); // Alpha deðeri geçiþle 1'e ayarlanýr (görünür).
        }

        // Zaman sona erdiðinde çaðrýlýr.
        // Geri sayým UI'sini gizler (alpha = 0).
        private void HandleTimeOver()
        {
            _canvasGroup.alphaTransition(0f, 1f); // Alpha deðeri geçiþle 0'a ayarlanýr (gizlenir).
        }

        // Her geri sayým týklamasý (zaman azaldýkça) bu metot çaðrýlýr.
        // Kalan zamaný m:ss formatýnda gösterir.
        private void HandleCountdownTick(int timeRemaining)
        {
            // Kalan zamaný TimeSpan objesi olarak alýyoruz.
            var timespan = TimeSpan.FromSeconds(timeRemaining);

            // Text objesine, kalan zamaný m:ss formatýnda yazýyoruz.
            text.text = timespan.ToString(@"m\:ss");
        }
    }
}
