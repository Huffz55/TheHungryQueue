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
        [SerializeField] private GameObject initialMenu;  // Ba�lang�� men�s�
        private CanvasGroup _initalMenuCanvasGroup;  // Ba�lang�� men�s�n�n CanvasGroup'�
        [Space]

        [Header("PauseMenu")]
        [SerializeField] private GameObject pauseMenu;  // Pause men�s�
        private CanvasGroup _pauseMenuCanvasGroup;  // Pause men�s�n�n CanvasGroup'�

        [Header("Buttons")]
        [SerializeField] private GameObject firstSelectedPauseMenu;  // Pause men�s�nde ilk se�ilecek buton
        [SerializeField] private Button resumeButton_Pause;  // Pause men�s�ndeki devam et butonu
        [SerializeField] private Button restartButton_Pause;  // Pause men�s�ndeki yeniden ba�lat butonu
        [SerializeField] private Button quitButton_Pause;  // Pause men�s�ndeki ��k�� butonu
        [Space]

        [Header("GameOverMenu")]
        [SerializeField] private GameObject gameOverMenu;  // Oyun bitti men�s�
        private CanvasGroup _gameOverMenuCanvasGroup;  // Oyun bitti men�s�n�n CanvasGroup'�
        [SerializeField] private GameObject firstSelectedGameOverMenu;  // Oyun bitti men�s�nde ilk se�ilecek buton
        [SerializeField] private AudioClip successClip;  // Ba�ar� ses klibi

        [Header("Buttons")]
        [SerializeField] private Button restartButton_GameOver;  // Oyun bitti men�s�ndeki yeniden ba�lat butonu
        [SerializeField] private Button quitButton_GameOver;  // Oyun bitti men�s�ndeki ��k�� butonu

        [Header("GameOver Stars")]
        [SerializeField] private Image star1;  // Oyun bitti men�s�ndeki ilk y�ld�z
        [SerializeField] private Image star2;  // Oyun bitti men�s�ndeki ikinci y�ld�z
        [SerializeField] private Image star3;  // Oyun bitti men�s�ndeki ���nc� y�ld�z
        [SerializeField] private TextMeshProUGUI scoreStar1Text;  // Birinci y�ld�z�n skor metni
        [SerializeField] private TextMeshProUGUI scoreStar2Text;  // �kinci y�ld�z�n skor metni
        [SerializeField] private TextMeshProUGUI scoreStar3Text;  // ���nc� y�ld�z�n skor metni
        [SerializeField] private TextMeshProUGUI scoreText;  // Toplam skor metni

        public delegate void ButtonPressed();  // Buton bas�ld���nda tetiklenen olaylar
        public static ButtonPressed OnResumeButton;  // Devam et butonuna bas�ld���nda tetiklenen olay
        public static ButtonPressed OnRestartButton;  // Yeniden ba�lat butonuna bas�ld���nda tetiklenen olay
        public static ButtonPressed OnQuitButton;  // ��k�� butonuna bas�ld���nda tetiklenen olay


        private void Awake()
        {
            // CanvasGroup'lar� ba�lat
            _initalMenuCanvasGroup = initialMenu.GetComponent<CanvasGroup>();
            _pauseMenuCanvasGroup = pauseMenu.GetComponent<CanvasGroup>();
            _gameOverMenuCanvasGroup = gameOverMenu.GetComponent<CanvasGroup>();

#if UNITY_EDITOR
            // Editor ortam�nda t�m referanslar�n null olmamas�n� sa�la
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

            // Ba�lang��ta t�m men�leri devre d��� b�rak
            initialMenu.SetActive(false);
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(false);
            _initalMenuCanvasGroup.alpha = 0f;
            _pauseMenuCanvasGroup.alpha = 0f;
            _gameOverMenuCanvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            // Butonlara t�klama olaylar� ekle
            AddButtonListeners();
        }

        private void OnDisable()
        {
            // Butonlara t�klama olaylar�n� kald�r
            RemoveButtonListeners();
        }

        // Butonlara t�klama olaylar�n� ekle
        private void AddButtonListeners()
        {
            resumeButton_Pause.onClick.AddListener(HandleResumeButton);
            restartButton_Pause.onClick.AddListener(HandleRestartButton);
            quitButton_Pause.onClick.AddListener(HandleQuitButton);
            restartButton_GameOver.onClick.AddListener(HandleRestartButton);
            quitButton_GameOver.onClick.AddListener(HandleQuitButton);
        }

        // Butonlara t�klama olaylar�n� kald�r
        private void RemoveButtonListeners()
        {
            resumeButton_Pause.onClick.RemoveAllListeners();
            restartButton_Pause.onClick.RemoveAllListeners();
            quitButton_Pause.onClick.RemoveAllListeners();
            restartButton_GameOver.onClick.RemoveAllListeners();
            quitButton_GameOver.onClick.RemoveAllListeners();
        }

        // Devam et butonuna bas�ld���nda tetiklenen olay
        private static void HandleResumeButton()
        {
            OnResumeButton?.Invoke();
        }

        // Yeniden ba�lat butonuna bas�ld���nda tetiklenen olay
        private static void HandleRestartButton()
        {
            GameOverMenu();  // Oyun bitti men�s�n� g�ster
            OnRestartButton?.Invoke();
        }

        // ��k�� butonuna bas�ld���nda tetiklenen olay
        private static void HandleQuitButton()
        {
            OnQuitButton?.Invoke();
        }

        // Ba�lang�� men�s�n� aktif et veya devre d��� b�rak
        public static void InitialMenuSetActive(bool active)
        {
            Instance.initialMenu.SetActive(active);
            Instance._initalMenuCanvasGroup.alphaTransition(active ? 1f : 0f, 2f);
        }

        // Pause men�s�n� a�ma/kapama i�lemi
        public static void PauseUnpause()
        {
            if (Instance.pauseMenu.activeInHierarchy == false)
            {
                // Pause men�s�n� aktif et ve zaman ak���n� durdur
                Instance.pauseMenu.SetActive(true);
                Time.timeScale = 0;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(Instance.firstSelectedPauseMenu);

                Instance.pauseMenu.SetActive(true);
                Instance._pauseMenuCanvasGroup.alphaTransition(1f, .5f);
            }
            else
            {
                // Pause men�s�n� devre d��� b�rak ve zaman ak���n� ba�lat
                Instance.pauseMenu.SetActive(false);
                Instance._pauseMenuCanvasGroup
                    .alphaTransition(0f, .5f)
                    .JoinTransition()
                    .EventTransition(() => Instance.pauseMenu.SetActive(false))
                    .EventTransition(() => Time.timeScale = 1);
            }
        }

        // Oyun bitti men�s�n� a�ma/kapama i�lemi
        public static void GameOverMenu()
        {
            if (Instance.gameOverMenu.activeInHierarchy == false)
            {
                // E�er Pause men�s� a��ksa kapat
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

                UpdateStars();  // Y�ld�zlar� g�ncelle
            }
            else
            {
                // Oyun bitti men�s�n� kapat ve zaman ak���n� ba�lat
                Instance.gameOverMenu.SetActive(false);
                Instance._gameOverMenuCanvasGroup
                    .alphaTransition(0f, .5f)
                    .JoinTransition()
                    .EventTransition(() => Instance.gameOverMenu.SetActive(false))
                    .EventTransition(() => Time.timeScale = 1);
            }
        }

        // Y�ld�zlar� g�ncelleme i�lemi
        private static void UpdateStars()
        {
            int score = GameManager.Score;  // Mevcut skor
            LevelData levelData = GameManager.LevelData;
            int star1Score = levelData.star1Score;  // Birinci y�ld�z skoru
            int star2Score = levelData.star2Score;  // �kinci y�ld�z skoru
            int star3Score = levelData.star3Score;  // ���nc� y�ld�z skoru
            Instance.scoreStar1Text.text = star1Score.ToString();
            Instance.scoreStar2Text.text = star2Score.ToString();
            Instance.scoreStar3Text.text = star3Score.ToString();
            Instance.scoreText.text = $"Score {score.ToString()}";  // Toplam skoru g�ster

            // Y�ld�zlar� animasyonla g�ster
            Instance.star1.gameObject.transform.localScale = Vector3.zero;
            Instance.star2.gameObject.transform.localScale = Vector3.zero;
            Instance.star3.gameObject.transform.localScale = Vector3.zero;

            if (score < star1Score) return;

            if (score < star2Score)
            {
                Instance.star1.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce);  // Birinci y�ld�z� b�y�t
            }
            else if (score < star3Score)
            {
                Instance.star1.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce)
                    .JoinTransition();
                Instance.star2.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce);  // �kinci y�ld�z� b�y�t
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
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce);  // ���nc� y�ld�z� b�y�t
            }
        }
    }
}
