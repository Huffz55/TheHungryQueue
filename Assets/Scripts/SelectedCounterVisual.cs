using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    // BaseCounter nesnesine referans. Bu, g�rselin hangi saya�la ili�kili oldu�unu belirler
    [SerializeField] private BaseCounter baseCounter;

    // Se�ilen sayac�n g�rsel temsilini i�eren GameObject dizisi
    [SerializeField] private GameObject[] visualGameObjectArray;

    // Ba�lang��ta, Player s�n�f�n�n OnSelectedCounterChanged olay�na abone oluyoruz
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    // Se�ilen saya� de�i�ti�inde tetiklenen metod
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        // E�er Player'�n se�ti�i saya�, bu s�n�f�n baseCounter nesnesine e�itse
        if (e.selectedCounter == baseCounter)
        {
            Show();  // G�rseli g�ster
        }
        else
        {
            Hide();  // G�rseli gizle
        }
    }

    // G�rselleri aktif hale getirir, yani g�sterir
    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    // G�rselleri pasif hale getirir, yani gizler
    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
