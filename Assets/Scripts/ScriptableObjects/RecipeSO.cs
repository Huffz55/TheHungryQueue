using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bu sýnýf, bir tarifin verilerini tutar.
// Tarifin adý ve tarifte kullanýlan mutfak objelerinin (malzemelerin) listesini içerir.
[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{

    // Tarifin içinde kullanýlacak mutfak objelerinin (malzemelerin) listesi.
    // Bu liste, tarife dahil olan tüm mutfak objelerinin türlerini belirtir.
    public List<KitchenObjectSO> kitchenObjectSOList;

    // Tarifin adýný tutar.
    // Örneðin:"Cheeseburger" gibi.
    public string recipeName;

}
