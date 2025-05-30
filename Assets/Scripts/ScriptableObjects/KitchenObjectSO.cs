using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{

    // Bu sýnýf, bir mutfak objesinin özelliklerini tanýmlar.
    // Oyundaki her mutfak objesi, bu sýnýfýn bir örneði olarak temsil edilir.

    // Bu mutfak objesinin sahnede görünen modelini temsil eder (örneðin, 3D model veya prefab).
    public Transform prefab;

    // Bu mutfak objesinin ikonu, genellikle UI'de kullanýlmak üzere sprite olarak gösterilir.
    public Sprite sprite;

    // Mutfak objesinin adý, genellikle UI veya etkileþimlerde kullanýlýr (örneðin, "Tava", "Býçak").
    public string objectName;

}
