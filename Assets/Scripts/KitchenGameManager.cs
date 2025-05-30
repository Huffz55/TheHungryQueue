using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Oyun durumlarýný yöneten ana kontrol sýnýfý
public class KitchenGameManager : MonoBehaviour
{

    // Singleton pattern: Tüm oyun boyunca bu sýnýfa tek bir eriþim noktasý saðlar
    public static KitchenGameManager Instance { get; private set; }

    // Oyun durumu deðiþtiðinde tetiklenir
    public event EventHandler OnStateChanged;
    // Oyun duraklatýldýðýnda tetiklenir
    public event EventHandler OnGamePaused;
    // Oyun duraklatmadan çýktýðýnda tetiklenir
    public event EventHandler OnGameUnpaused;

    // Olasý oyun durumlarýný tanýmlayan enum
    private enum State
    {
        WaitingToStart,       // Oyunun baþlamasýný bekliyor
        CountdownToStart,     // Baþlama geri sayýmý yapýlýyor
        GamePlaying,          // Oyun oynanýyor
        GameOver,             // Oyun sona erdi
    }

    private State state; // Þu anki oyun durumu
    private float countdownToStartTimer = 3f; // Baþlangýç için geri sayým süresi
    private float gamePlayingTimer; // Oynama süresi sayacý
    private float gamePlayingTimerMax = 90f; // Maksimum oyun süresi
    private bool isGamePaused = false; // Oyun þu anda duraklatýlmýþ mý?

    // Awake: Singleton ayarý ve baþlangýç durumu
    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    // Start: Giriþler (input) için event dinleyicileri eklenir
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    // Oyuncu "etkileþim" giriþine bastýðýnda baþlatma sürecini tetikler
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // Oyuncu "pause" giriþine bastýðýnda duraklatmayý aç/kapa yapar
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    // Oyun durumu güncellemesi her karede yapýlýr
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                // Hiçbir þey yapýlmaz, oyuncunun baþlatmasý beklenir
                break;
            case State.CountdownToStart:
                // Geri sayým azalýr
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    // Geri sayým bitti, oyun baþlar
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                // Oynama süresi azalýr
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    // Süre bitti, oyun sona erer
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                // Oyun sona erdiðinde bir þey yapýlmaz
                break;
        }
    }

    // Oyun þu anda aktif olarak oynanýyor mu?
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    // Baþlama geri sayýmý aktif mi?
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    // Geri sayýmýn kalan süresi
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    // Oyun sona erdi mi?
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    // Oynama süresinin ne kadarýnýn geçtiðini normalize þekilde (0-1 arasý) döndürür
    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    // Oyunun duraklatýlýp duraklatýlmadýðýný kontrol eder ve durumu deðiþtirir
    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            // Zamaný durdur
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Zamaný devam ettir
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
