using Lean.Transition;
using UnityEngine;
using TMPro;
using Undercooked.Appliances;
using Undercooked.Managers;
using Undercooked.Model;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    // Bu s�n�f, teslim edilen sipari�le ilgili UI bildirimlerini y�netir.
    // �rne�in, "Plato Eksik" veya "Tip +X" mesajlar�n� ekranda g�stermek i�in kullan�l�r.
    [RequireComponent(typeof(CanvasGroup))]
    public class DeliverCountertopUI : MonoBehaviour
    {
        // CanvasGroup bile�eni, UI elementinin g�r�n�rl���n� kontrol eder.
        private CanvasGroup _canvasGroup;

        // TextMeshProUGUI, UI'de metin g�r�nt�lemek i�in kullan�l�r.
        private static TextMeshProUGUI _text;

        // Bildirim renkleri
        [Header("notification colors")]
        [SerializeField] private Color positiveColorOutline;  // Olumlu mesajlar�n kenarl�k rengi
        [SerializeField] private Color positiveColorBase;     // Olumlu mesajlar�n ana rengi
        [SerializeField] private Color negativeColorOutline;  // Olumsuz mesajlar�n kenarl�k rengi
        [SerializeField] private Color negativeColorBase;     // Olumsuz mesajlar�n ana rengi

        // Awake metodu, bile�enlerin ba�lat�ld��� yerdir.
        private void Awake()
        {
            // Text bile�enini �ocuklardan al�yoruz.
            _text = GetComponentInChildren<TextMeshProUGUI>();

            // CanvasGroup bile�enini al�yoruz.
            _canvasGroup = GetComponent<CanvasGroup>();

#if UNITY_EDITOR
            // Editor modunda, _text ve _canvasGroup bile�enlerinin varl���n� do�ruluyoruz.
            Assert.IsNotNull(_text);
            Assert.IsNotNull(_canvasGroup);
#endif
        }

        // Bu metod, bu script aktif oldu�unda �a�r�l�r.
        private void OnEnable()
        {
            // Sipari� teslim edildi�inde veya eksik bir plaka oldu�unda bildirimler g�sterilir.
            OrderManager.OnOrderDelivered += HandleOrderDelivered;
            DeliverCountertop.OnPlateMissing += HandlePlateMissing;
        }

        // Bu metod, bu script devre d��� b�rak�ld���nda �a�r�l�r.
        private void OnDisable()
        {
            // Olay dinleyicilerini kald�r�yoruz.
            OrderManager.OnOrderDelivered -= HandleOrderDelivered;
            DeliverCountertop.OnPlateMissing -= HandlePlateMissing;
        }

        // E�er teslimat s�ras�nda plaka eksikse, ekranda bir mesaj g�r�nt�lenir.
        private void HandlePlateMissing()
        {
            // ScrollAndFadeText metodu ile "NEEDS PLATE!" mesaj�n� g�r�nt�leriz.
            ScrollAndFadeText("NEEDS PLATE!", negativeColorBase, negativeColorOutline, 2f);
        }

        // Sipari� teslim edildi�inde, tip varsa, ekranda tip miktar� g�sterilir.
        private void HandleOrderDelivered(Order order, int tip)
        {
            // E�er tip 0 ise, mesaj g�sterilmez.
            if (tip == 0) return;

            // ScrollAndFadeText metodu ile tip miktar�n� g�r�nt�leriz.
            ScrollAndFadeText($"+{tip} TIP!", positiveColorBase, positiveColorOutline, 2f);
        }

        // Metin g�r�nt�leme, kayd�rma ve silme animasyonu yapan yard�mc� metot.
        // Parametre olarak metin, renkler ve g�r�n�rl�k s�resi al�r.
        private void ScrollAndFadeText(string textToDisplay, Color baseColor, Color outlineColor, float timeToDisplayInSeconds = 2f)
        {
            // Metni ba�lang�� pozisyonuna getiriyoruz.
            _text.gameObject.transform.localPosition = Vector3.zero;

            // CanvasGroup'u g�r�n�r yap�yoruz.
            _canvasGroup.alpha = 1f;

            // G�sterilecek metni, rengini ve outline rengini ayarl�yoruz.
            _text.text = textToDisplay;
            _text.color = baseColor;
            _text.outlineColor = outlineColor;

            // Alpha ge�i�i ile metni kaybettiriyoruz.
            _canvasGroup.alphaTransition(0f, timeToDisplayInSeconds, LeanEase.Smooth);

            // Y ekseninde kayd�rma animasyonu ile metni yukar� kayd�r�yoruz.
            _text.rectTransform
                .localPositionTransition_Y(100f, timeToDisplayInSeconds, LeanEase.Smooth);
        }

    }
}
