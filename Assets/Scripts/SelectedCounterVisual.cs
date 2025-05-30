using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    // BaseCounter nesnesine referans. Bu, görselin hangi sayaçla iliþkili olduðunu belirler
    [SerializeField] private BaseCounter baseCounter;

    // Seçilen sayacýn görsel temsilini içeren GameObject dizisi
    [SerializeField] private GameObject[] visualGameObjectArray;

    // Baþlangýçta, Player sýnýfýnýn OnSelectedCounterChanged olayýna abone oluyoruz
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    // Seçilen sayaç deðiþtiðinde tetiklenen metod
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        // Eðer Player'ýn seçtiði sayaç, bu sýnýfýn baseCounter nesnesine eþitse
        if (e.selectedCounter == baseCounter)
        {
            Show();  // Görseli göster
        }
        else
        {
            Hide();  // Görseli gizle
        }
    }

    // Görselleri aktif hale getirir, yani gösterir
    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    // Görselleri pasif hale getirir, yani gizler
    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
