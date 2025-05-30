using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bu s�n�f, bir tarifin verilerini tutar.
// Tarifin ad� ve tarifte kullan�lan mutfak objelerinin (malzemelerin) listesini i�erir.
[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{

    // Tarifin i�inde kullan�lacak mutfak objelerinin (malzemelerin) listesi.
    // Bu liste, tarife dahil olan t�m mutfak objelerinin t�rlerini belirtir.
    public List<KitchenObjectSO> kitchenObjectSOList;

    // Tarifin ad�n� tutar.
    // �rne�in:"Cheeseburger" gibi.
    public string recipeName;

}
