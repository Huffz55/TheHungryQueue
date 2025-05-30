using System.Collections.Generic;
using System.Threading.Tasks;
using Lean.Transition;
using Undercooked.Model;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Undercooked.UI
{
    public class OrderUI : MonoBehaviour
    {
        // UI ��eleri
        [SerializeField] private RectTransform rootRectTransform;
        [SerializeField] private RectTransform basePanel;
        [SerializeField] private RectTransform bottomPanel;
        [SerializeField] private Image orderImage;
        [SerializeField] private Slider slider;
        [SerializeField] private List<Image> ingredientImages = new List<Image>();

        [Header("Audio")]
        [SerializeField] private AudioClip popAudio; // Sipari� g�rseli ekran�n i�ine kayarken �alan ses
        [SerializeField] private AudioClip notificationAudio; // Sipari� olu�turulurken �alan ses
        [SerializeField] private AudioClip buzzerAudio; // Sipari� zaman a��m�na u�rad���nda �alan ses

        [SerializeField] private Image[] images; // UI ��elerindeki g�rseller
        [SerializeField] private Gradient sliderGradient; // Slider�n renk ge�i�i

        // Sabitler ve �zel de�i�kenler
        private const float UIWidth = 190f; // UI ��esinin geni�li�i
        private const int ShakeIntervalTimeMs = 600; // Titre�im i�in aral�k s�resi (milisaniye cinsinden)
        private Image _sliderFillImage; // Slider'�n dolum k�sm�n�n g�rseli
        private bool _shake; // Titre�im durumunu kontrol etmek i�in bayrak
        private float _initialRemainingTime; // Sipari�in ba�lang��ta kalan s�resi
        private Material _uiMaterial; // UI ��elerinin materyali (titre�imde kullan�lacak)
        private Vector2 _bottomPanelInitialAnchoredPosition; // Alt panelin ba�lang��taki konumu

        public float CurrentAnchorX { get; private set; } // UI ��esinin X koordinat�
        public float SizeDeltaX => rootRectTransform.sizeDelta.x; // UI ��esinin geni�li�i
        public Order Order { get; private set; } // Bu UI'nin temsil etti�i sipari� nesnesi

        private void Awake()
        {
            // Gerekli bile�enlere referanslar� al
            rootRectTransform = GetComponent<RectTransform>();
            _sliderFillImage = slider.fillRect.GetComponent<Image>();
            _bottomPanelInitialAnchoredPosition = bottomPanel.anchoredPosition;
            DuplicateMaterial(); // Materyali �o�alt (titre�im i�in)

#if UNITY_EDITOR
            // Unity Editor'de ��elerin atan�p atanmad���n� kontrol et
            Assert.IsNotNull(rootRectTransform);
            Assert.IsNotNull(basePanel);
            Assert.IsNotNull(bottomPanel);
            Assert.IsNotNull(orderImage);
            Assert.IsNotNull(slider);
            Assert.IsNotNull(popAudio);
            Assert.IsNotNull(notificationAudio);
            Assert.IsNotNull(buzzerAudio);
#endif
        }

        // Sipari�in s�resi bitti�inde yap�lacak i�lemler
        private async void HandleExpired(Order order)
        {
            StopShake(); // Titre�imi durdur
            await RotateAlertBasePanelAsync(Color.red, buzzerAudio); // K�rm�z� uyar� efekti ve ses �al
        }

        // Sipari� alarm s�resine yakla�t���nda yap�lacak i�lemler
        private void HandleAlertTime(Order order)
        {
            StartShake(); // Titre�imi ba�lat
        }

        // UI materyalini �o�alt (titre�im efekti i�in)
        private void DuplicateMaterial()
        {
            images = GetComponentsInChildren<Image>(); // Alt bile�enlerdeki t�m Image ��elerini al

            if (images.Length <= 0) return;
            _uiMaterial = Instantiate(images[0].material); // �lk g�rselin materyalini kopyala
            foreach (var image in images)
            {
                image.material = _uiMaterial; // T�m g�rsellerin materyalini ayarla
            }
        }

        // Sipari� UI ��esini ba�lat ve d�zenle
        public void Setup(Order order)
        {
            Order = order; // Sipari�i ata
            rootRectTransform.anchoredPosition = new Vector2(Screen.width + 300f, 0f); // Ba�lang��ta UI ��esini ekran d���na yerle�tir
            var sizeDelta = rootRectTransform.sizeDelta;
            rootRectTransform.sizeDelta = new Vector2(UIWidth, sizeDelta.y); // UI ��esinin geni�li�ini ayarla
            var randomRotation = Random.Range(-45f, +45f); // Sipari� panelini rastgele d�nd�r
            basePanel.localRotation = Quaternion.Euler(0f, 0f, randomRotation); // D�nd�rmeyi uygula

            basePanel.localPosition = new Vector3(basePanel.localPosition.x, 0f, 0f); // BasePanel pozisyonunu s�f�rla

            orderImage.sprite = Order.OrderData.sprite; // Sipari� resmini ayarla
            _initialRemainingTime = Order.InitialRemainingTime; // Ba�lang�� zaman�n� kaydet

            // Sipari�in i�eriklerini UI'ye yerle�tir
            for (var i = 0; i < Order.OrderData.ingredients.Count; i++)
            {
                ingredientImages[i].sprite = Order.OrderData.ingredients[i].sprite;
            }
            SubscribeEvents(); // Olaylar� abone et
        }

        // Sipari�in event'lerine abone ol
        private void SubscribeEvents()
        {
            Order.OnDelivered += HandleDelivered;
            Order.OnAlertTime += HandleAlertTime;
            Order.OnExpired += HandleExpired;
            Order.OnUpdatedCountdown += HandleUpdatedCountdown;
        }

        // Sipari� event'lerinden abonelikleri kald�r
        private void UnsubscribeEvents()
        {
            Order.OnDelivered -= HandleDelivered;
            Order.OnAlertTime -= HandleAlertTime;
            Order.OnExpired -= HandleExpired;
            Order.OnUpdatedCountdown -= HandleUpdatedCountdown;
        }

        // Sayac�n g�ncellenmesi durumunda slider'� ve renk ge�i�ini g�ncelle
        private void HandleUpdatedCountdown(float remainingTime)
        {
            slider.value = remainingTime / _initialRemainingTime; // Slider de�erini g�ncelle
            _sliderFillImage.color = sliderGradient.Evaluate(slider.value); // Slider renk ge�i�ini ayarla
        }

        // Sipari� teslim edildi�inde yap�lacak i�lemler
        private void HandleDelivered(Order order)
        {
            HandleDeliveredAsync(order);
        }

        private async void HandleDeliveredAsync(Order order)
        {
            await RotateAlertBasePanelAsync(Color.green, null); // Ye�il teslimat uyar�s� ve ses
            SlideUp(); // UI ��esini yukar� kayd�r
            Deactivate(); // UI ��esini devre d��� b�rak
        }

        // UI ��esini devre d��� b�rak (titre�im ve event'leri durdur)
        private void Deactivate()
        {
            StopShake(); // Titre�imi durdur
            UnsubscribeEvents(); // Olay aboneliklerini kald�r
            bottomPanel.anchoredPosition = _bottomPanelInitialAnchoredPosition; // Alt panelin pozisyonunu s�f�rla
        }

        // Sipari�i ekran�n i�ine kayd�rarak g�ster
        public void SlideInSpawn(float desiredX)
        {
            CurrentAnchorX = desiredX;
            float initialSlideDuration = 1f; // Kayd�rma s�resi

            Vector2 small = new Vector2(0.8f, 1f); // Ba�lang��ta biraz daha k���k

            // UI ��esini kayd�r
            rootRectTransform
                .anchoredPositionTransition_X(desiredX, initialSlideDuration, LeanEase.Decelerate)
                .JoinTransition()
                .PlaySoundTransition(popAudio); // Ses efekti �al

            // Base panelini kayd�r ve b�y�t
            basePanel.
                localRotationTransition(Quaternion.identity, initialSlideDuration, LeanEase.Decelerate)
                .JoinTransition()
                .localScaleTransition_XY(small, 0.125f, LeanEase.Elastic)
                .JoinTransition()
                .localScaleTransition_XY(Vector2.one, 0.125f, LeanEase.Smooth);

            // Alt paneli yukar� kayd�r
            bottomPanel
                .JoinDelayTransition(initialSlideDuration + 0.25f)
                .PlaySoundTransition(notificationAudio) // Ses efekti �al
                .JoinTransition()
                .anchoredPositionTransition_Y(-75f, 0.3f, LeanEase.Bounce);
        }

        // UI ��esini sola kayd�r
        public void SlideLeft(float desiredX)
        {
            CurrentAnchorX = desiredX;
            float initialSlideDuration = 0.5f; // Kayd�rma s�resi

            rootRectTransform
                .anchoredPositionTransition_X(desiredX, initialSlideDuration, LeanEase.Decelerate);
        }

        // Titre�imi ba�lat
        private void StartShake()
        {
            _shake = true;
            ShakeAsync();
        }

        // Titre�imi asenkron olarak yap
        private async Task ShakeAsync()
        {
            const float deltaX = 7f; // Titre�im mesafesi

            while (_shake)
            {
                basePanel
                    .anchoredPositionTransition_X(-deltaX, 0.15f)
                    .JoinTransition()
                    .anchoredPositionTransition_X(deltaX, 0.3f)
                    .JoinTransition()
                    .anchoredPositionTransition_X(0, 0.15f)
                    .JoinTransition();
                await Task.Delay(ShakeIntervalTimeMs); // Belirli aral�klarla titre�imi devam ettir
            }

            basePanel.anchoredPositionTransition_X(0, 0.15f); // Sonra paneli s�f�rla
        }

        // Titre�imi durdur
        private void StopShake() => _shake = false;

        // Uyar� efekti (d�nme ve renk de�i�imi)
        private async Task RotateAlertBasePanelAsync(Color flickerColor, AudioClip audioClip = null)
        {
            const string colorProperty = "_Color"; // Renk i�in shader �zelli�i
            const float deltaZ = 10f; // D�nd�rme a��s�
            const int minorDelayMs = 200; // K���k gecikme s�resi
            const float minorDelaySeconds = minorDelayMs / 1000f;

            basePanel
                .localRotationTransition(Quaternion.Euler(0f, 0f, deltaZ), minorDelaySeconds)
                .JoinTransition()
                .localRotationTransition(Quaternion.Euler(0f, 0f, -deltaZ), minorDelaySeconds)
                .JoinTransition()
                .localRotationTransition(Quaternion.Euler(0f, 0f, deltaZ), minorDelaySeconds)
                .JoinTransition()
                .localRotationTransition(Quaternion.Euler(0f, 0f, -deltaZ), minorDelaySeconds)
                .JoinTransition()
                .localRotationTransition(Quaternion.identity, .1f);

            _uiMaterial.colorTransition(colorProperty, flickerColor, minorDelaySeconds); // Renk ge�i�ini uygula
            await Task.Delay(minorDelayMs); // K�sa bir bekleme
            _uiMaterial.colorTransition(colorProperty, Color.white, minorDelaySeconds); // Beyaz renge ge�
            await Task.Delay(minorDelayMs);
            _uiMaterial.colorTransition(colorProperty, flickerColor, minorDelaySeconds);
            await Task.Delay(minorDelayMs);
            _uiMaterial.colorTransition(colorProperty, Color.white, minorDelaySeconds);
            await Task.Delay(minorDelayMs);

            if (audioClip != null)
            {
                basePanel.PlaySoundTransition(audioClip); // Ses �al
            }
        }

        // UI ��esini yukar� kayd�r
        private void SlideUp()
        {
            const float deltaY = 400f; // Kayd�rma mesafesi
            basePanel
                .localScaleTransition_XY(new Vector2(1.1f, 1.1f), .3f, LeanEase.Bounce)
                .JoinTransition()
                .localPositionTransition_Y(deltaY, .5f, LeanEase.Decelerate);
        }

    }
}
