using Lean.Transition;
using TMPro;
using Undercooked.Data;
using Undercooked.Managers;
using Undercooked.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Undercooked.UI
{
    public class MenuPanelUI : Singleton<MenuPanelUI>
    {
        [Header("InitialMenu")]
        [SerializeField] private GameObject initialMenu;  // Baþlangýç menüsü
        private CanvasGroup _initalMenuCanvasGroup;  // Baþlangýç menüsünün CanvasGroup'ý
        [Space]

        [Header("PauseMenu")]
        [SerializeField] private GameObject pauseMenu;  // Pause menüsü
        private CanvasGroup _pauseMenuCanvasGroup;  // Pause menüsünün CanvasGroup'ý

        [Header("Buttons")]
        [SerializeField] private GameObject firstSelectedPauseMenu;  // Pause menüsünde ilk seçilecek buton
        [SerializeField] private Button resumeButton_Pause;  // Pause menüsündeki devam et butonu
        [SerializeField] private Button restartButton_Pause;  // Pause menüsündeki yeniden baþlat butonu
        [SerializeField] private Button quitButton_Pause;  // Pause menüsündeki çýkýþ butonu
        [Space]

        [Header("GameOverMenu")]
        [SerializeField] private GameObject gameOverMenu;  // Oyun bitti menüsü
        private CanvasGroup _gameOverMenuCanvasGroup;  // Oyun bitti menüsünün CanvasGroup'ý
        [SerializeField] private GameObject firstSelectedGameOverMenu;  // Oyun bitti menüsünde ilk seçilecek buton
        [SerializeField] private AudioClip successClip;  // Baþarý ses klibi

        [Header("Buttons")]
        [SerializeField] private Button restartButton_GameOver;  // Oyun bitti menüsündeki yeniden baþlat butonu
        [SerializeField] private Button quitButton_GameOver;  // Oyun bitti menüsündeki çýkýþ butonu

        [Header("GameOver Stars")]
        [SerializeField] private Image star1;  // Oyun bitti menüsündeki ilk yýldýz
        [SerializeField] private Image star2;  // Oyun bitti menüsündeki ikinci yýldýz
        [SerializeField] private Image star3;  // Oyun bitti menüsündeki üçüncü yýldýz
        [SerializeField] private TextMeshProUGUI scoreStar1Text;  // Birinci yýldýzýn skor metni
        [SerializeField] private TextMeshProUGUI scoreStar2Text;  // Ýkinci yýldýzýn skor metni
        [SerializeField] private TextMeshProUGUI scoreStar3Text;  // Üçüncü yýldýzýn skor metni
        [SerializeField] private TextMeshProUGUI scoreText;  // Toplam skor metni

        public delegate void ButtonPressed();  // Buton basýldýðýnda tetiklenen olaylar
        public static ButtonPressed OnResumeButton;  // Devam et butonuna basýldýðýnda tetiklenen olay
        public static ButtonPressed OnRestartButton;  // Yeniden baþlat butonuna basýldýðýnda tetiklenen olay
        public static ButtonPressed OnQuitButton;  // Çýkýþ butonuna basýldýðýnda tetiklenen olay


        private void Awake()
        {
            // CanvasGroup'larý baþlat
            _initalMenuCanvasGroup = initialMenu.GetComponent<CanvasGroup>();
            _pauseMenuCanvasGroup = pauseMenu.GetComponent<CanvasGroup>();
            _gameOverMenuCanvasGroup = gameOverMenu.GetComponent<CanvasGroup>();

#if UNITY_EDITOR
            // Editor ortamýnda tüm referanslarýn null olmamasýný saðla
            Assert.IsNotNull(initialMenu);
            Assert.IsNotNull(pauseMenu);
            Assert.IsNotNull(gameOverMenu);
            Assert.IsNotNull(_initalMenuCanvasGroup);
            Assert.IsNotNull(_gameOverMenuCanvasGroup);
            Assert.IsNotNull(_pauseMenuCanvasGroup);

            Assert.IsNotNull(resumeButton_Pause);
            Assert.IsNotNull(restartButton_Pause);
            Assert.IsNotNull(quitButton_Pause);
            Assert.IsNotNull(restartButton_GameOver);
            Assert.IsNotNull(quitButton_GameOver);

            Assert.IsNotNull(star1);
            Assert.IsNotNull(star2);
            Assert.IsNotNull(star3);
            Assert.IsNotNull(scoreStar1Text);
            Assert.IsNotNull(scoreStar2Text);
            Assert.IsNotNull(scoreStar3Text);
            Assert.IsNotNull(scoreText);
#endif

            // Baþlangýçta tüm menüleri devre dýþý býrak
            initialMenu.SetActive(false);
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(false);
            _initalMenuCanvasGroup.alpha = 0f;
            _pauseMenuCanvasGroup.alpha = 0f;
            _gameOverMenuCanvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            // Butonlara týklama olaylarý ekle
            AddButtonListeners();
        }

        private void OnDisable()
        {
            // Butonlara týklama olaylarýný kaldýr
            RemoveButtonListeners();
        }

        // Butonlara týklama olaylarýný ekle
        private void AddButtonListeners()
        {
            resumeButton_Pause.onClick.AddListener(HandleResumeButton);
            restartButton_Pause.onClick.AddListener(HandleRestartButton);
            quitButton_Pause.onClick.AddListener(HandleQuitButton);
            restartButton_GameOver.onClick.AddListener(HandleRestartButton);
            quitButton_GameOver.onClick.AddListener(HandleQuitButton);
        }

        // Butonlara týklama olaylarýný kaldýr
        private void RemoveButtonListeners()
        {
            resumeButton_Pause.onClick.RemoveAllListeners();
            restartButton_Pause.onClick.RemoveAllListeners();
            quitButton_Pause.onClick.RemoveAllListeners();
            restartButton_GameOver.onClick.RemoveAllListeners();
            quitButton_GameOver.onClick.RemoveAllListeners();
        }

        // Devam et butonuna basýldýðýnda tetiklenen olay
        private static void HandleResumeButton()
        {
            OnResumeButton?.Invoke();
        }

        // Yeniden baþlat butonuna basýldýðýnda tetiklenen olay
        private static void HandleRestartButton()
        {
            GameOverMenu();  // Oyun bitti menüsünü göster
            OnRestartButton?.Invoke();
        }

        // Çýkýþ butonuna basýldýðýnda tetiklenen olay
        private static void HandleQuitButton()
        {
            OnQuitButton?.Invoke();
        }

        // Baþlangýç menüsünü aktif et veya devre dýþý býrak
        public static void InitialMenuSetActive(bool active)
        {
            Instance.initialMenu.SetActive(active);
            Instance._initalMenuCanvasGroup.alphaTransition(active ? 1f : 0f, 2f);
        }

        // Pause menüsünü açma/kapama iþlemi
        public static void PauseUnpause()
        {
            if (Instance.pauseMenu.activeInHierarchy == false)
            {
                // Pause menüsünü aktif et ve zaman akýþýný durdur
                Instance.pauseMenu.SetActive(true);
                Time.timeScale = 0;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(Instance.firstSelectedPauseMenu);

                Instance.pauseMenu.SetActive(true);
                Instance._pauseMenuCanvasGroup.alphaTransition(1f, .5f);
            }
            else
            {
                // Pause menüsünü devre dýþý býrak ve zaman akýþýný baþlat
                Instance.pauseMenu.SetActive(false);
                Instance._pauseMenuCanvasGroup
                    .alphaTransition(0f, .5f)
                    .JoinTransition()
                    .EventTransition(() => Instance.pauseMenu.SetActive(false))
                    .EventTransition(() => Time.timeScale = 1);
            }
        }

        // Oyun bitti menüsünü açma/kapama iþlemi
        public static void GameOverMenu()
        {
            if (Instance.gameOverMenu.activeInHierarchy == false)
            {
                // Eðer Pause menüsü açýksa kapat
                if (Instance.pauseMenu.activeInHierarchy)
                {
                    PauseUnpause();
                }

                Instance.gameOverMenu.SetActive(true);
                Time.timeScale = 0;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(Instance.firstSelectedGameOverMenu);

                Instance.gameOverMenu.SetActive(true);
                Instance._gameOverMenuCanvasGroup.alphaTransition(1f, .5f);

                UpdateStars();  // Yýldýzlarý güncelle
            }
            else
            {
                // Oyun bitti menüsünü kapat ve zaman akýþýný baþlat
                Instance.gameOverMenu.SetActive(false);
                Instance._gameOverMenuCanvasGroup
                    .alphaTransition(0f, .5f)
                    .JoinTransition()
                    .EventTransition(() => Instance.gameOverMenu.SetActive(false))
                    .EventTransition(() => Time.timeScale = 1);
            }
        }

        // Yýldýzlarý güncelleme iþlemi
        private static void UpdateStars()
        {
            int score = GameManager.Score;  // Mevcut skor
            LevelData levelData = GameManager.LevelData;
            int star1Score = levelData.star1Score;  // Birinci yýldýz skoru
            int star2Score = levelData.star2Score;  // Ýkinci yýldýz skoru
            int star3Score = levelData.star3Score;  // Üçüncü yýldýz skoru
            Instance.scoreStar1Text.text = star1Score.ToString();
            Instance.scoreStar2Text.text = star2Score.ToString();
            Instance.scoreStar3Text.text = star3Score.ToString();
            Instance.scoreText.text = $"Score {score.ToString()}";  // Toplam skoru göster

            // Yýldýzlarý animasyonla göster
            Instance.star1.gameObject.transform.localScale = Vector3.zero;
            Instance.star2.gameObject.transform.localScale = Vector3.zero;
            Instance.star3.gameObject.transform.localScale = Vector3.zero;

            if (score < star1Score) return;

            if (score < star2Score)
            {
                Instance.star1.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce);  // Birinci yýldýzý büyüt
            }
            else if (score < star3Score)
            {
                Instance.star1.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce)
                    .JoinTransition();
                Instance.star2.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce);  // Ýkinci yýldýzý büyüt
            }
            else
            {
                Instance.star1.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce)
                    .JoinTransition();
                Instance.star2.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce)
                    .JoinTransition();
                Instance.star3.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce);  // Üçüncü yýldýzý büyüt
            }
        }
    }
}
