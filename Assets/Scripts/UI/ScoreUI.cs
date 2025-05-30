using Lean.Transition;
using TMPro;
using Undercooked.Managers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ScoreUI : MonoBehaviour
    {
        // Temel puan metni bileþeni
        [SerializeField] private TextMeshProUGUI scoreBaseText;

        // Animasyonlu puan metni bileþeni (delta deðeri için)
        [SerializeField] private TextMeshProUGUI scoreAnimatedText;

        // UI öðesinin görünürlüðünü yönetmek için CanvasGroup
        private CanvasGroup _canvasGroup;

        // Animasyonlu puan metni için CanvasGroup
        [SerializeField] private CanvasGroup scoreAnimatedCanvasGroup;

        [Header("Bildirim Renkleri")]
        // Pozitif puan deðiþiklikleri için renkler
        [SerializeField] private Color positiveColorOutline;
        [SerializeField] private Color positiveColorBase;

        // Negatif puan deðiþiklikleri için renkler
        [SerializeField] private Color negativeColorOutline;
        [SerializeField] private Color negativeColorBase;

        // Animasyonlu puan metninin baþlangýç pozisyonu (sýfýrlamak için)
        private Vector3 _initialAnimatedTextLocalPosition;

        private void Awake()
        {
            // Animasyonlu puan metninin baþlangýç pozisyonunu sakla
            _initialAnimatedTextLocalPosition = scoreAnimatedText.transform.localPosition;

            // Bu GameObject'e baðlý CanvasGroup bileþenini al
            _canvasGroup = GetComponent<CanvasGroup>();

#if UNITY_EDITOR
            // Gerekli bileþenlerin boþ olmadýðýndan emin ol
            Assert.IsNotNull(scoreBaseText);
            Assert.IsNotNull(scoreAnimatedText);
            Assert.IsNotNull(scoreAnimatedCanvasGroup);
            Assert.IsNotNull(_canvasGroup);
#endif

            // Baþlangýçta canvas group'unu görünmez yap
            _canvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            // Oyun olaylarýna abone ol: puan güncelleme, seviye baþlatma, zaman bitme
            GameManager.OnScoreUpdate += HandleScoreUpdate;
            GameManager.OnLevelStart += HandleLevelStart;
            GameManager.OnTimeIsOver += HandleTimeOver;
        }

        private void OnDisable()
        {
            // Oyun olaylarýndan abone olmayý býrak
            GameManager.OnScoreUpdate -= HandleScoreUpdate;
            GameManager.OnLevelStart -= HandleLevelStart;
            GameManager.OnTimeIsOver -= HandleTimeOver;
        }

        private void HandleLevelStart()
        {
            // Seviye baþlatýldýðýnda puan UI'sini göster
            _canvasGroup.alphaTransition(1f, 1f);
        }

        private void HandleTimeOver()
        {
            // Zaman bittiðinde puan UI'sini gizle
            _canvasGroup.alphaTransition(0f, 1f);
        }

        private void HandleScoreUpdate(int score, int delta)
        {
            // Eðer puan deðiþmemiþse, hiçbir þey yapma
            if (delta == 0) return;

            // Temel puan metnini güncelle
            scoreBaseText.text = score.ToString();

            // Eðer puan azalmýþsa, negatif deðiþikliði göster, yoksa pozitif deðiþikliði göster
            if (delta < 0)
            {
                ScrollAndFadeText(delta.ToString(), negativeColorOutline, negativeColorBase);
                return;
            }
            ScrollAndFadeText(delta.ToString(), positiveColorOutline, positiveColorBase);
        }

        // Animasyonlu puan metnini kaydýr ve soluklaþtýr
        private void ScrollAndFadeText(string textToDisplay, Color baseColor, Color outlineColor, float timeToDisplayInSeconds = 2f)
        {
            // Animasyonlu metnin pozisyonunu sýfýrla
            scoreAnimatedText.transform.localPosition = _initialAnimatedTextLocalPosition;
            scoreAnimatedCanvasGroup.alpha = 1f;

            // Animasyonlu puan metnini ve renklerini ayarla
            scoreAnimatedText.text = textToDisplay;
            scoreAnimatedText.color = baseColor;
            scoreAnimatedText.outlineColor = outlineColor;

            // Animasyonlu metnin boyutunu deðiþtir
            scoreAnimatedText.rectTransform
                .localScaleTransition(new Vector3(1.2f, 1.2f, 1.2f), 0.2f, LeanEase.Decelerate)
                .JoinTransition()
                .localScaleTransition(Vector3.one, .2f, LeanEase.Smooth);

            // Metnin dikey hareketini animasyonla göster
            scoreAnimatedText.rectTransform
                 .localPositionTransition_Y(200f, timeToDisplayInSeconds, LeanEase.Smooth);

            // Animasyonlu metni soluklaþtýr
            scoreAnimatedCanvasGroup.alphaTransition(0f, timeToDisplayInSeconds, LeanEase.Smooth);
        }
    }
}
