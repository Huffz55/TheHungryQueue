using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{

    // �e�itli ses klipleri i�in diziler

    // Kesme sesleri (�rne�in, b��akla kesme sesleri)
    public AudioClip[] chop;

    // Teslimat ba�ar�s�zl�k sesleri (teslimat s�ras�nda ba�ar�s�z olundu�unda �alan sesler)
    public AudioClip[] deliveryFail;

    // Teslimat ba�ar� sesleri (teslimat ba�ar�l� oldu�unda �alan sesler)
    public AudioClip[] deliverySuccess;

    // Ayak sesi (y�r�rken duyulan ayak sesleri)
    public AudioClip[] footstep;

    // Nesne d���rme sesleri (oyuncu bir nesneyi yere d���rd���nde �alan sesler)
    public AudioClip[] objectDrop;

    // Nesne alma sesleri (oyuncu bir nesneyi ald���nda �alan sesler)
    public AudioClip[] objectPickup;

    // Ocak c�z�rt� sesi (ocak �st�nde pi�en yemeklerin c�z�rt� sesi)
    public AudioClip stoveSizzle;

    // ��p sesleri (��p kutusuna at�lan nesnelerin sesleri)
    public AudioClip[] trash;

    // Uyar� sesleri (oyuncuyu uyaran alarm veya uyar� sesleri)
    public AudioClip[] warning;

}
