using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{

    // Çeþitli ses klipleri için diziler

    // Kesme sesleri (örneðin, býçakla kesme sesleri)
    public AudioClip[] chop;

    // Teslimat baþarýsýzlýk sesleri (teslimat sýrasýnda baþarýsýz olunduðunda çalan sesler)
    public AudioClip[] deliveryFail;

    // Teslimat baþarý sesleri (teslimat baþarýlý olduðunda çalan sesler)
    public AudioClip[] deliverySuccess;

    // Ayak sesi (yürürken duyulan ayak sesleri)
    public AudioClip[] footstep;

    // Nesne düþürme sesleri (oyuncu bir nesneyi yere düþürdüðünde çalan sesler)
    public AudioClip[] objectDrop;

    // Nesne alma sesleri (oyuncu bir nesneyi aldýðýnda çalan sesler)
    public AudioClip[] objectPickup;

    // Ocak cýzýrtý sesi (ocak üstünde piþen yemeklerin cýzýrtý sesi)
    public AudioClip stoveSizzle;

    // Çöp sesleri (çöp kutusuna atýlan nesnelerin sesleri)
    public AudioClip[] trash;

    // Uyarý sesleri (oyuncuyu uyaran alarm veya uyarý sesleri)
    public AudioClip[] warning;

}
