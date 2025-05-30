using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LoaderCallback: Yükleme sahnesinde ilk frame'de hedef sahneye geçiþ yapan sýnýf
public class LoaderCallback : MonoBehaviour
{

    // Bu flag sadece ilk Update çaðrýsýnda çalýþmak için kullanýlýr
    private bool isFirstUpdate = true;

    private void Update()
    {
        // Sadece ilk Update çalýþtýðýnda LoaderCallback çaðrýlýr
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            // Asýl hedef sahneye geçiþ yapýlýr
            Loader.LoaderCallback();
        }
    }
}
