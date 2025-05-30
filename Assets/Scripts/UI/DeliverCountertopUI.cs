using Lean.Transition;
using UnityEngine;
using TMPro;
using Undercooked.Appliances;
using Undercooked.Managers;
using Undercooked.Model;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    // Bu sýnýf, teslim edilen sipariþle ilgili UI bildirimlerini yönetir.
    // Örneðin, "Plato Eksik" veya "Tip +X" mesajlarýný ekranda göstermek için kullanýlýr.
    [RequireComponent(typeof(CanvasGroup))]
    public class DeliverCountertopUI : MonoBehaviour
    {
        // CanvasGroup bileþeni, UI elementinin görünürlüðünü kontrol eder.
        private CanvasGroup _canvasGroup;

        // TextMeshProUGUI, UI'de metin görüntülemek için kullanýlýr.
        private static TextMeshProUGUI _text;

        // Bildirim renkleri
        [Header("notification colors")]
        [SerializeField] private Color positiveColorOutline;  // Olumlu mesajlarýn kenarlýk rengi
        [SerializeField] private Color positiveColorBase;     // Olumlu mesajlarýn ana rengi
        [SerializeField] private Color negativeColorOutline;  // Olumsuz mesajlarýn kenarlýk rengi
        [SerializeField] private Color negativeColorBase;     // Olumsuz mesajlarýn ana rengi

        // Awake metodu, bileþenlerin baþlatýldýðý yerdir.
        private void Awake()
        {
            // Text bileþenini çocuklardan alýyoruz.
            _text = GetComponentInChildren<TextMeshProUGUI>();

            // CanvasGroup bileþenini alýyoruz.
            _canvasGroup = GetComponent<CanvasGroup>();

#if UNITY_EDITOR
            // Editor modunda, _text ve _canvasGroup bileþenlerinin varlýðýný doðruluyoruz.
            Assert.IsNotNull(_text);
            Assert.IsNotNull(_canvasGroup);
#endif
        }

        // Bu metod, bu script aktif olduðunda çaðrýlýr.
        private void OnEnable()
        {
            // Sipariþ teslim edildiðinde veya eksik bir plaka olduðunda bildirimler gösterilir.
            OrderManager.OnOrderDelivered += HandleOrderDelivered;
            DeliverCountertop.OnPlateMissing += HandlePlateMissing;
        }

        // Bu metod, bu script devre dýþý býrakýldýðýnda çaðrýlýr.
        private void OnDisable()
        {
            // Olay dinleyicilerini kaldýrýyoruz.
            OrderManager.OnOrderDelivered -= HandleOrderDelivered;
            DeliverCountertop.OnPlateMissing -= HandlePlateMissing;
        }

        // Eðer teslimat sýrasýnda plaka eksikse, ekranda bir mesaj görüntülenir.
        private void HandlePlateMissing()
        {
            // ScrollAndFadeText metodu ile "NEEDS PLATE!" mesajýný görüntüleriz.
            ScrollAndFadeText("NEEDS PLATE!", negativeColorBase, negativeColorOutline, 2f);
        }

        // Sipariþ teslim edildiðinde, tip varsa, ekranda tip miktarý gösterilir.
        private void HandleOrderDelivered(Order order, int tip)
        {
            // Eðer tip 0 ise, mesaj gösterilmez.
            if (tip == 0) return;

            // ScrollAndFadeText metodu ile tip miktarýný görüntüleriz.
            ScrollAndFadeText($"+{tip} TIP!", positiveColorBase, positiveColorOutline, 2f);
        }

        // Metin görüntüleme, kaydýrma ve silme animasyonu yapan yardýmcý metot.
        // Parametre olarak metin, renkler ve görünürlük süresi alýr.
        private void ScrollAndFadeText(string textToDisplay, Color baseColor, Color outlineColor, float timeToDisplayInSeconds = 2f)
        {
            // Metni baþlangýç pozisyonuna getiriyoruz.
            _text.gameObject.transform.localPosition = Vector3.zero;

            // CanvasGroup'u görünür yapýyoruz.
            _canvasGroup.alpha = 1f;

            // Gösterilecek metni, rengini ve outline rengini ayarlýyoruz.
            _text.text = textToDisplay;
            _text.color = baseColor;
            _text.outlineColor = outlineColor;

            // Alpha geçiþi ile metni kaybettiriyoruz.
            _canvasGroup.alphaTransition(0f, timeToDisplayInSeconds, LeanEase.Smooth);

            // Y ekseninde kaydýrma animasyonu ile metni yukarý kaydýrýyoruz.
            _text.rectTransform
                .localPositionTransition_Y(100f, timeToDisplayInSeconds, LeanEase.Smooth);
        }

    }
}
