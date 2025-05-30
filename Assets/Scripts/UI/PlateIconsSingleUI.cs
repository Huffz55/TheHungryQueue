using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconsSingleUI : MonoBehaviour
{

    // UI'daki g�rsel ��esi (Sprite) i�in bir referans
    [SerializeField] private Image image;

    // Bu fonksiyon, verilen KitchenObjectSO nesnesine g�re UI g�rselini ayarlar
    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        // KitchenObjectSO nesnesindeki sprite'� UI g�rseline uygula
        image.sprite = kitchenObjectSO.sprite;
    }

}
