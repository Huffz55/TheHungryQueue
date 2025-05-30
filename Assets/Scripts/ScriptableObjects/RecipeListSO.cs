using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bu s�n�f, bir dizi tarif (recipe) objesini tutan bir veri yap�s�d�r.
// Tarifi temsil eden `RecipeSO` nesnelerinin bir listesini i�erir.
// Bu s�n�f, tariflerin bir koleksiyonunu depolamak ve oyun i�inde bu tariflere eri�mek i�in kullan�l�r.
public class RecipeListSO : ScriptableObject
{

    // RecipeSO t�r�ndeki tariflerin listesini tutar.
    // Her `RecipeSO` objesi, bir tarifin �zelliklerini (gerekli malzemeler, pi�irme s�resi vb.) i�erir.
    public List<RecipeSO> recipeSOList;

}
