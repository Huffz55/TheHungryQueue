using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    // PlayerPrefs'te ses efektlerinin ses seviyesini tutan anahtar
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";


    // Singleton �rne�i, SoundManager'a global eri�im sa�lar
    public static SoundManager Instance { get; private set; }

    // Ses dosyalar�n� tutan referanslar (�rne�in; object pickup, chop gibi)
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    // Ses seviyesi (0 ile 1 aras�nda)
    private float volume = 1f;

    // Singleton �rne�i kurulur ve ses seviyesi PlayerPrefs'ten okunur
    private void Awake()
    {
        Instance = this;

        // E�er �nceden kaydedilmi� bir ses seviyesi varsa, onu al, yoksa 1.0'� kullan
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    // Ba�lang��ta ses olaylar�na abone olunur
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    // ��p kutusuna bir �ey at�ld���nda ses �alar
    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    // Bir nesne sayaca b�rak�ld���nda ses �alar
    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    // Bir nesne al�nd���nda ses �alar
    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    // Kesme i�lemi yap�ld���nda ses �alar
    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    // Yemek teslimi ba�ar�s�z oldu�unda ses �alar
    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    // Yemek teslimi ba�ar�l� oldu�unda ses �alar
    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    // Ses �almak i�in kullan�lan yard�mc� metod (birden fazla ses se�ene�inden rastgele birini �alar)
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    // Belirli bir ses dosyas�n� �alar
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        // Sesin �al�nmas�
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    // Ayak sesi �alar (y�r�rken)
    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    // Uyar� sesi (countdown) �alar
    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }

    // Genel uyar� sesi �alar
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position);
    }

    // Ses seviyesini de�i�tirir. E�er 1'den fazla ise s�f�rlan�r
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;  // 1'den fazla ise s�f�rla
        }

        // Yeni ses seviyesini PlayerPrefs'e kaydeder
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // Ge�erli ses seviyesini d�nd�r�r
    public float GetVolume()
    {
        return volume;
    }

}
