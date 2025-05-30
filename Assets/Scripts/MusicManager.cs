using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

// MusicManager: Oyun m�zi�inin ses seviyesini y�netir
public class MusicManager : MonoBehaviour
{

    // PlayerPrefs'te m�zik ses seviyesi i�in saklanan anahtar
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    // Singleton �rne�i (tek bir MusicManager �rne�i olmas�n� sa�lar)
    public static MusicManager Instance { get; private set; }

    // M�zik i�in kullan�lan AudioSource bile�eni
    private AudioSource audioSource;

    // M�zik ses seviyesi (ba�lang��ta .3f olarak ayarlan�r)
    private float volume = .3f;

    // Ba�lang��ta, AudioSource bile�eni ve ses seviyesi y�klenir
    private void Awake()
    {
        Instance = this;

        // AudioSource bile�enini al
        audioSource = GetComponent<AudioSource>();

        // PlayerPrefs'ten ses seviyesi al�n�r, e�er yoksa varsay�lan olarak .3f kullan�l�r
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        audioSource.volume = volume;
    }

    // Ses seviyesini de�i�tirir ve PlayerPrefs'e kaydeder
    public void ChangeVolume()
    {
        // Ses seviyesini art�r
        volume += .1f;

        // E�er ses seviyesi 1'den b�y�kse, s�f�rlan�r (0'dan ba�lar)
        if (volume > 1f)
        {
            volume = 0f;
        }

        // Ses seviyesini AudioSource'a uygula
        audioSource.volume = volume;

        // Yeni ses seviyesini PlayerPrefs'e kaydet
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // �u anki ses seviyesini d�nd�r�r
    public float GetVolume()
    {
        return volume;
    }
}
