using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LookAtCamera: Nesnenin y�n�n� kameraya g�re ayarlamak i�in kullan�l�r
public class LookAtCamera : MonoBehaviour
{

    // Y�nlendirme modu se�enekleri
    private enum Mode
    {
        LookAt,                 // Kameraya do�ru bak
        LookAtInverted,        // Kameraya ters y�nde bak
        CameraForward,         // Kameran�n bakt��� y�ne do�ru bak
        CameraForwardInverted, // Kameran�n bakt��� y�n�n tersine bak
    }

    // Edit�rde se�ilecek y�nlendirme modu
    [SerializeField] private Mode mode;

    // Update'den sonra �a�r�l�r, b�ylece kameran�n hareketleri tamamland�ktan sonra y�nlendirme yap�l�r
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                // Kameraya do�ru bakar
                transform.LookAt(Camera.main.transform);
                break;

            case Mode.LookAtInverted:
                // Kameraya ters y�nde bakar
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;

            case Mode.CameraForward:
                // Nesnenin y�n� kameran�n bakt��� y�ne e�it olur
                transform.forward = Camera.main.transform.forward;
                break;

            case Mode.CameraForwardInverted:
                // Nesnenin y�n� kameran�n tersine bakar
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
