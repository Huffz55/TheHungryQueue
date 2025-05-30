using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Oyun durumlar�n� y�neten ana kontrol s�n�f�
public class KitchenGameManager : MonoBehaviour
{

    // Singleton pattern: T�m oyun boyunca bu s�n�fa tek bir eri�im noktas� sa�lar
    public static KitchenGameManager Instance { get; private set; }

    // Oyun durumu de�i�ti�inde tetiklenir
    public event EventHandler OnStateChanged;
    // Oyun duraklat�ld���nda tetiklenir
    public event EventHandler OnGamePaused;
    // Oyun duraklatmadan ��kt���nda tetiklenir
    public event EventHandler OnGameUnpaused;

    // Olas� oyun durumlar�n� tan�mlayan enum
    private enum State
    {
        WaitingToStart,       // Oyunun ba�lamas�n� bekliyor
        CountdownToStart,     // Ba�lama geri say�m� yap�l�yor
        GamePlaying,          // Oyun oynan�yor
        GameOver,             // Oyun sona erdi
    }

    private State state; // �u anki oyun durumu
    private float countdownToStartTimer = 3f; // Ba�lang�� i�in geri say�m s�resi
    private float gamePlayingTimer; // Oynama s�resi sayac�
    private float gamePlayingTimerMax = 90f; // Maksimum oyun s�resi
    private bool isGamePaused = false; // Oyun �u anda duraklat�lm�� m�?

    // Awake: Singleton ayar� ve ba�lang�� durumu
    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    // Start: Giri�ler (input) i�in event dinleyicileri eklenir
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    // Oyuncu "etkile�im" giri�ine bast���nda ba�latma s�recini tetikler
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // Oyuncu "pause" giri�ine bast���nda duraklatmay� a�/kapa yapar
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    // Oyun durumu g�ncellemesi her karede yap�l�r
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                // Hi�bir �ey yap�lmaz, oyuncunun ba�latmas� beklenir
                break;
            case State.CountdownToStart:
                // Geri say�m azal�r
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    // Geri say�m bitti, oyun ba�lar
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                // Oynama s�resi azal�r
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    // S�re bitti, oyun sona erer
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                // Oyun sona erdi�inde bir �ey yap�lmaz
                break;
        }
    }

    // Oyun �u anda aktif olarak oynan�yor mu?
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    // Ba�lama geri say�m� aktif mi?
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    // Geri say�m�n kalan s�resi
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    // Oyun sona erdi mi?
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    // Oynama s�resinin ne kadar�n�n ge�ti�ini normalize �ekilde (0-1 aras�) d�nd�r�r
    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    // Oyunun duraklat�l�p duraklat�lmad���n� kontrol eder ve durumu de�i�tirir
    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            // Zaman� durdur
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Zaman� devam ettir
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
