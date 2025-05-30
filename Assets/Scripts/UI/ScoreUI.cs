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
        // Temel puan metni bile�eni
        [SerializeField] private TextMeshProUGUI scoreBaseText;

        // Animasyonlu puan metni bile�eni (delta de�eri i�in)
        [SerializeField] private TextMeshProUGUI scoreAnimatedText;

        // UI ��esinin g�r�n�rl���n� y�netmek i�in CanvasGroup
        private CanvasGroup _canvasGroup;

        // Animasyonlu puan metni i�in CanvasGroup
        [SerializeField] private CanvasGroup scoreAnimatedCanvasGroup;

        [Header("Bildirim Renkleri")]
        // Pozitif puan de�i�iklikleri i�in renkler
        [SerializeField] private Color positiveColorOutline;
        [SerializeField] private Color positiveColorBase;

        // Negatif puan de�i�iklikleri i�in renkler
        [SerializeField] private Color negativeColorOutline;
        [SerializeField] private Color negativeColorBase;

        // Animasyonlu puan metninin ba�lang�� pozisyonu (s�f�rlamak i�in)
        private Vector3 _initialAnimatedTextLocalPosition;

        private void Awake()
        {
            // Animasyonlu puan metninin ba�lang�� pozisyonunu sakla
            _initialAnimatedTextLocalPosition = scoreAnimatedText.transform.localPosition;

            // Bu GameObject'e ba�l� CanvasGroup bile�enini al
            _canvasGroup = GetComponent<CanvasGroup>();

#if UNITY_EDITOR
            // Gerekli bile�enlerin bo� olmad���ndan emin ol
            Assert.IsNotNull(scoreBaseText);
            Assert.IsNotNull(scoreAnimatedText);
            Assert.IsNotNull(scoreAnimatedCanvasGroup);
            Assert.IsNotNull(_canvasGroup);
#endif

            // Ba�lang��ta canvas group'unu g�r�nmez yap
            _canvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            // Oyun olaylar�na abone ol: puan g�ncelleme, seviye ba�latma, zaman bitme
            GameManager.OnScoreUpdate += HandleScoreUpdate;
            GameManager.OnLevelStart += HandleLevelStart;
            GameManager.OnTimeIsOver += HandleTimeOver;
        }

        private void OnDisable()
        {
            // Oyun olaylar�ndan abone olmay� b�rak
            GameManager.OnScoreUpdate -= HandleScoreUpdate;
            GameManager.OnLevelStart -= HandleLevelStart;
            GameManager.OnTimeIsOver -= HandleTimeOver;
        }

        private void HandleLevelStart()
        {
            // Seviye ba�lat�ld���nda puan UI'sini g�ster
            _canvasGroup.alphaTransition(1f, 1f);
        }

        private void HandleTimeOver()
        {
            // Zaman bitti�inde puan UI'sini gizle
            _canvasGroup.alphaTransition(0f, 1f);
        }

        private void HandleScoreUpdate(int score, int delta)
        {
            // E�er puan de�i�memi�se, hi�bir �ey yapma
            if (delta == 0) return;

            // Temel puan metnini g�ncelle
            scoreBaseText.text = score.ToString();

            // E�er puan azalm��sa, negatif de�i�ikli�i g�ster, yoksa pozitif de�i�ikli�i g�ster
            if (delta < 0)
            {
                ScrollAndFadeText(delta.ToString(), negativeColorOutline, negativeColorBase);
                return;
            }
            ScrollAndFadeText(delta.ToString(), positiveColorOutline, positiveColorBase);
        }

        // Animasyonlu puan metnini kayd�r ve solukla�t�r
        private void ScrollAndFadeText(string textToDisplay, Color baseColor, Color outlineColor, float timeToDisplayInSeconds = 2f)
        {
            // Animasyonlu metnin pozisyonunu s�f�rla
            scoreAnimatedText.transform.localPosition = _initialAnimatedTextLocalPosition;
            scoreAnimatedCanvasGroup.alpha = 1f;

            // Animasyonlu puan metnini ve renklerini ayarla
            scoreAnimatedText.text = textToDisplay;
            scoreAnimatedText.color = baseColor;
            scoreAnimatedText.outlineColor = outlineColor;

            // Animasyonlu metnin boyutunu de�i�tir
            scoreAnimatedText.rectTransform
                .localScaleTransition(new Vector3(1.2f, 1.2f, 1.2f), 0.2f, LeanEase.Decelerate)
                .JoinTransition()
                .localScaleTransition(Vector3.one, .2f, LeanEase.Smooth);

            // Metnin dikey hareketini animasyonla g�ster
            scoreAnimatedText.rectTransform
                 .localPositionTransition_Y(200f, timeToDisplayInSeconds, LeanEase.Smooth);

            // Animasyonlu metni solukla�t�r
            scoreAnimatedCanvasGroup.alphaTransition(0f, timeToDisplayInSeconds, LeanEase.Smooth);
        }
    }
}
