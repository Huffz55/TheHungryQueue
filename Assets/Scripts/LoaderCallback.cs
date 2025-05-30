using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LoaderCallback: Y�kleme sahnesinde ilk frame'de hedef sahneye ge�i� yapan s�n�f
public class LoaderCallback : MonoBehaviour
{

    // Bu flag sadece ilk Update �a�r�s�nda �al��mak i�in kullan�l�r
    private bool isFirstUpdate = true;

    private void Update()
    {
        // Sadece ilk Update �al��t���nda LoaderCallback �a�r�l�r
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            // As�l hedef sahneye ge�i� yap�l�r
            Loader.LoaderCallback();
        }
    }
}
