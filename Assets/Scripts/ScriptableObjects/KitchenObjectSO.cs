using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{

    // Bu s�n�f, bir mutfak objesinin �zelliklerini tan�mlar.
    // Oyundaki her mutfak objesi, bu s�n�f�n bir �rne�i olarak temsil edilir.

    // Bu mutfak objesinin sahnede g�r�nen modelini temsil eder (�rne�in, 3D model veya prefab).
    public Transform prefab;

    // Bu mutfak objesinin ikonu, genellikle UI'de kullan�lmak �zere sprite olarak g�sterilir.
    public Sprite sprite;

    // Mutfak objesinin ad�, genellikle UI veya etkile�imlerde kullan�l�r (�rne�in, "Tava", "B��ak").
    public string objectName;

}
