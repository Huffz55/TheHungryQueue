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
        // UI öðeleri
        [SerializeField] private RectTransform rootRectTransform;
        [SerializeField] private RectTransform basePanel;
        [SerializeField] private RectTransform bottomPanel;
        [SerializeField] private Image orderImage;
        [SerializeField] private Slider slider;
        [SerializeField] private List<Image> ingredientImages = new List<Image>();

        [Header("Audio")]
        [SerializeField] private AudioClip popAudio; // Sipariþ görseli ekranýn içine kayarken çalan ses
        [SerializeField] private AudioClip notificationAudio; // Sipariþ oluþturulurken çalan ses
        [SerializeField] private AudioClip buzzerAudio; // Sipariþ zaman aþýmýna uðradýðýnda çalan ses

        [SerializeField] private Image[] images; // UI öðelerindeki görseller
        [SerializeField] private Gradient sliderGradient; // Sliderýn renk geçiþi

        // Sabitler ve özel deðiþkenler
        private const float UIWidth = 190f; // UI öðesinin geniþliði
        private const int ShakeIntervalTimeMs = 600; // Titreþim için aralýk süresi (milisaniye cinsinden)
        private Image _sliderFillImage; // Slider'ýn dolum kýsmýnýn görseli
        private bool _shake; // Titreþim durumunu kontrol etmek için bayrak
        private float _initialRemainingTime; // Sipariþin baþlangýçta kalan süresi
        private Material _uiMaterial; // UI öðelerinin materyali (titreþimde kullanýlacak)
        private Vector2 _bottomPanelInitialAnchoredPosition; // Alt panelin baþlangýçtaki konumu

        public float CurrentAnchorX { get; private set; } // UI öðesinin X koordinatý
        public float SizeDeltaX => rootRectTransform.sizeDelta.x; // UI öðesinin geniþliði
        public Order Order { get; private set; } // Bu UI'nin temsil ettiði sipariþ nesnesi

        private void Awake()
        {
            // Gerekli bileþenlere referanslarý al
            rootRectTransform = GetComponent<RectTransform>();
            _sliderFillImage = slider.fillRect.GetComponent<Image>();
            _bottomPanelInitialAnchoredPosition = bottomPanel.anchoredPosition;
            DuplicateMaterial(); // Materyali çoðalt (titreþim için)

#if UNITY_EDITOR
            // Unity Editor'de öðelerin atanýp atanmadýðýný kontrol et
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

        // Sipariþin süresi bittiðinde yapýlacak iþlemler
        private async void HandleExpired(Order order)
        {
            StopShake(); // Titreþimi durdur
            await RotateAlertBasePanelAsync(Color.red, buzzerAudio); // Kýrmýzý uyarý efekti ve ses çal
        }

        // Sipariþ alarm süresine yaklaþtýðýnda yapýlacak iþlemler
        private void HandleAlertTime(Order order)
        {
            StartShake(); // Titreþimi baþlat
        }

        // UI materyalini çoðalt (titreþim efekti için)
        private void DuplicateMaterial()
        {
            images = GetComponentsInChildren<Image>(); // Alt bileþenlerdeki tüm Image öðelerini al

            if (images.Length <= 0) return;
            _uiMaterial = Instantiate(images[0].material); // Ýlk görselin materyalini kopyala
            foreach (var image in images)
            {
                image.material = _uiMaterial; // Tüm görsellerin materyalini ayarla
            }
        }

        // Sipariþ UI öðesini baþlat ve düzenle
        public void Setup(Order order)
        {
            Order = order; // Sipariþi ata
            rootRectTransform.anchoredPosition = new Vector2(Screen.width + 300f, 0f); // Baþlangýçta UI öðesini ekran dýþýna yerleþtir
            var sizeDelta = rootRectTransform.sizeDelta;
            rootRectTransform.sizeDelta = new Vector2(UIWidth, sizeDelta.y); // UI öðesinin geniþliðini ayarla
            var randomRotation = Random.Range(-45f, +45f); // Sipariþ panelini rastgele döndür
            basePanel.localRotation = Quaternion.Euler(0f, 0f, randomRotation); // Döndürmeyi uygula

            basePanel.localPosition = new Vector3(basePanel.localPosition.x, 0f, 0f); // BasePanel pozisyonunu sýfýrla

            orderImage.sprite = Order.OrderData.sprite; // Sipariþ resmini ayarla
            _initialRemainingTime = Order.InitialRemainingTime; // Baþlangýç zamanýný kaydet

            // Sipariþin içeriklerini UI'ye yerleþtir
            for (var i = 0; i < Order.OrderData.ingredients.Count; i++)
            {
                ingredientImages[i].sprite = Order.OrderData.ingredients[i].sprite;
            }
            SubscribeEvents(); // Olaylarý abone et
        }

        // Sipariþin event'lerine abone ol
        private void SubscribeEvents()
        {
            Order.OnDelivered += HandleDelivered;
            Order.OnAlertTime += HandleAlertTime;
            Order.OnExpired += HandleExpired;
            Order.OnUpdatedCountdown += HandleUpdatedCountdown;
        }

        // Sipariþ event'lerinden abonelikleri kaldýr
        private void UnsubscribeEvents()
        {
            Order.OnDelivered -= HandleDelivered;
            Order.OnAlertTime -= HandleAlertTime;
            Order.OnExpired -= HandleExpired;
            Order.OnUpdatedCountdown -= HandleUpdatedCountdown;
        }

        // Sayacýn güncellenmesi durumunda slider'ý ve renk geçiþini güncelle
        private void HandleUpdatedCountdown(float remainingTime)
        {
            slider.value = remainingTime / _initialRemainingTime; // Slider deðerini güncelle
            _sliderFillImage.color = sliderGradient.Evaluate(slider.value); // Slider renk geçiþini ayarla
        }

        // Sipariþ teslim edildiðinde yapýlacak iþlemler
        private void HandleDelivered(Order order)
        {
            HandleDeliveredAsync(order);
        }

        private async void HandleDeliveredAsync(Order order)
        {
            await RotateAlertBasePanelAsync(Color.green, null); // Yeþil teslimat uyarýsý ve ses
            SlideUp(); // UI öðesini yukarý kaydýr
            Deactivate(); // UI öðesini devre dýþý býrak
        }

        // UI öðesini devre dýþý býrak (titreþim ve event'leri durdur)
        private void Deactivate()
        {
            StopShake(); // Titreþimi durdur
            UnsubscribeEvents(); // Olay aboneliklerini kaldýr
            bottomPanel.anchoredPosition = _bottomPanelInitialAnchoredPosition; // Alt panelin pozisyonunu sýfýrla
        }

        // Sipariþi ekranýn içine kaydýrarak göster
        public void SlideInSpawn(float desiredX)
        {
            CurrentAnchorX = desiredX;
            float initialSlideDuration = 1f; // Kaydýrma süresi

            Vector2 small = new Vector2(0.8f, 1f); // Baþlangýçta biraz daha küçük

            // UI öðesini kaydýr
            rootRectTransform
                .anchoredPositionTransition_X(desiredX, initialSlideDuration, LeanEase.Decelerate)
                .JoinTransition()
                .PlaySoundTransition(popAudio); // Ses efekti çal

            // Base panelini kaydýr ve büyüt
            basePanel.
                localRotationTransition(Quaternion.identity, initialSlideDuration, LeanEase.Decelerate)
                .JoinTransition()
                .localScaleTransition_XY(small, 0.125f, LeanEase.Elastic)
                .JoinTransition()
                .localScaleTransition_XY(Vector2.one, 0.125f, LeanEase.Smooth);

            // Alt paneli yukarý kaydýr
            bottomPanel
                .JoinDelayTransition(initialSlideDuration + 0.25f)
                .PlaySoundTransition(notificationAudio) // Ses efekti çal
                .JoinTransition()
                .anchoredPositionTransition_Y(-75f, 0.3f, LeanEase.Bounce);
        }

        // UI öðesini sola kaydýr
        public void SlideLeft(float desiredX)
        {
            CurrentAnchorX = desiredX;
            float initialSlideDuration = 0.5f; // Kaydýrma süresi

            rootRectTransform
                .anchoredPositionTransition_X(desiredX, initialSlideDuration, LeanEase.Decelerate);
        }

        // Titreþimi baþlat
        private void StartShake()
        {
            _shake = true;
            ShakeAsync();
        }

        // Titreþimi asenkron olarak yap
        private async Task ShakeAsync()
        {
            const float deltaX = 7f; // Titreþim mesafesi

            while (_shake)
            {
                basePanel
                    .anchoredPositionTransition_X(-deltaX, 0.15f)
                    .JoinTransition()
                    .anchoredPositionTransition_X(deltaX, 0.3f)
                    .JoinTransition()
                    .anchoredPositionTransition_X(0, 0.15f)
                    .JoinTransition();
                await Task.Delay(ShakeIntervalTimeMs); // Belirli aralýklarla titreþimi devam ettir
            }

            basePanel.anchoredPositionTransition_X(0, 0.15f); // Sonra paneli sýfýrla
        }

        // Titreþimi durdur
        private void StopShake() => _shake = false;

        // Uyarý efekti (dönme ve renk deðiþimi)
        private async Task RotateAlertBasePanelAsync(Color flickerColor, AudioClip audioClip = null)
        {
            const string colorProperty = "_Color"; // Renk için shader özelliði
            const float deltaZ = 10f; // Döndürme açýsý
            const int minorDelayMs = 200; // Küçük gecikme süresi
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

            _uiMaterial.colorTransition(colorProperty, flickerColor, minorDelaySeconds); // Renk geçiþini uygula
            await Task.Delay(minorDelayMs); // Kýsa bir bekleme
            _uiMaterial.colorTransition(colorProperty, Color.white, minorDelaySeconds); // Beyaz renge geç
            await Task.Delay(minorDelayMs);
            _uiMaterial.colorTransition(colorProperty, flickerColor, minorDelaySeconds);
            await Task.Delay(minorDelayMs);
            _uiMaterial.colorTransition(colorProperty, Color.white, minorDelaySeconds);
            await Task.Delay(minorDelayMs);

            if (audioClip != null)
            {
                basePanel.PlaySoundTransition(audioClip); // Ses çal
            }
        }

        // UI öðesini yukarý kaydýr
        private void SlideUp()
        {
            const float deltaY = 400f; // Kaydýrma mesafesi
            basePanel
                .localScaleTransition_XY(new Vector2(1.1f, 1.1f), .3f, LeanEase.Bounce)
                .JoinTransition()
                .localPositionTransition_Y(deltaY, .5f, LeanEase.Decelerate);
        }

    }
}
