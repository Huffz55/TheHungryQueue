using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconsSingleUI : MonoBehaviour
{

    // UI'daki görsel öðesi (Sprite) için bir referans
    [SerializeField] private Image image;

    // Bu fonksiyon, verilen KitchenObjectSO nesnesine göre UI görselini ayarlar
    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        // KitchenObjectSO nesnesindeki sprite'ý UI görseline uygula
        image.sprite = kitchenObjectSO.sprite;
    }

}
