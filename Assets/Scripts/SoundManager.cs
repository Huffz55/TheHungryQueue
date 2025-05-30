using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    // PlayerPrefs'te ses efektlerinin ses seviyesini tutan anahtar
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";


    // Singleton örneði, SoundManager'a global eriþim saðlar
    public static SoundManager Instance { get; private set; }

    // Ses dosyalarýný tutan referanslar (örneðin; object pickup, chop gibi)
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    // Ses seviyesi (0 ile 1 arasýnda)
    private float volume = 1f;

    // Singleton örneði kurulur ve ses seviyesi PlayerPrefs'ten okunur
    private void Awake()
    {
        Instance = this;

        // Eðer önceden kaydedilmiþ bir ses seviyesi varsa, onu al, yoksa 1.0'ý kullan
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    // Baþlangýçta ses olaylarýna abone olunur
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    // Çöp kutusuna bir þey atýldýðýnda ses çalar
    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    // Bir nesne sayaca býrakýldýðýnda ses çalar
    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    // Bir nesne alýndýðýnda ses çalar
    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    // Kesme iþlemi yapýldýðýnda ses çalar
    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    // Yemek teslimi baþarýsýz olduðunda ses çalar
    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    // Yemek teslimi baþarýlý olduðunda ses çalar
    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    // Ses çalmak için kullanýlan yardýmcý metod (birden fazla ses seçeneðinden rastgele birini çalar)
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    // Belirli bir ses dosyasýný çalar
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        // Sesin çalýnmasý
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    // Ayak sesi çalar (yürürken)
    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    // Uyarý sesi (countdown) çalar
    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }

    // Genel uyarý sesi çalar
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position);
    }

    // Ses seviyesini deðiþtirir. Eðer 1'den fazla ise sýfýrlanýr
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;  // 1'den fazla ise sýfýrla
        }

        // Yeni ses seviyesini PlayerPrefs'e kaydeder
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // Geçerli ses seviyesini döndürür
    public float GetVolume()
    {
        return volume;
    }

}
