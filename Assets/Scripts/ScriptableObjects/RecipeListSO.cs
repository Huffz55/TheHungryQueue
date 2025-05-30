using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bu sýnýf, bir dizi tarif (recipe) objesini tutan bir veri yapýsýdýr.
// Tarifi temsil eden `RecipeSO` nesnelerinin bir listesini içerir.
// Bu sýnýf, tariflerin bir koleksiyonunu depolamak ve oyun içinde bu tariflere eriþmek için kullanýlýr.
public class RecipeListSO : ScriptableObject
{

    // RecipeSO türündeki tariflerin listesini tutar.
    // Her `RecipeSO` objesi, bir tarifin özelliklerini (gerekli malzemeler, piþirme süresi vb.) içerir.
    public List<RecipeSO> recipeSOList;

}
