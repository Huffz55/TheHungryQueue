using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

// MusicManager: Oyun müziðinin ses seviyesini yönetir
public class MusicManager : MonoBehaviour
{

    // PlayerPrefs'te müzik ses seviyesi için saklanan anahtar
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    // Singleton örneði (tek bir MusicManager örneði olmasýný saðlar)
    public static MusicManager Instance { get; private set; }

    // Müzik için kullanýlan AudioSource bileþeni
    private AudioSource audioSource;

    // Müzik ses seviyesi (baþlangýçta .3f olarak ayarlanýr)
    private float volume = .3f;

    // Baþlangýçta, AudioSource bileþeni ve ses seviyesi yüklenir
    private void Awake()
    {
        Instance = this;

        // AudioSource bileþenini al
        audioSource = GetComponent<AudioSource>();

        // PlayerPrefs'ten ses seviyesi alýnýr, eðer yoksa varsayýlan olarak .3f kullanýlýr
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        audioSource.volume = volume;
    }

    // Ses seviyesini deðiþtirir ve PlayerPrefs'e kaydeder
    public void ChangeVolume()
    {
        // Ses seviyesini artýr
        volume += .1f;

        // Eðer ses seviyesi 1'den büyükse, sýfýrlanýr (0'dan baþlar)
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

    // Þu anki ses seviyesini döndürür
    public float GetVolume()
    {
        return volume;
    }
}
