using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LookAtCamera: Nesnenin yönünü kameraya göre ayarlamak için kullanýlýr
public class LookAtCamera : MonoBehaviour
{

    // Yönlendirme modu seçenekleri
    private enum Mode
    {
        LookAt,                 // Kameraya doðru bak
        LookAtInverted,        // Kameraya ters yönde bak
        CameraForward,         // Kameranýn baktýðý yöne doðru bak
        CameraForwardInverted, // Kameranýn baktýðý yönün tersine bak
    }

    // Editörde seçilecek yönlendirme modu
    [SerializeField] private Mode mode;

    // Update'den sonra çaðrýlýr, böylece kameranýn hareketleri tamamlandýktan sonra yönlendirme yapýlýr
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                // Kameraya doðru bakar
                transform.LookAt(Camera.main.transform);
                break;

            case Mode.LookAtInverted:
                // Kameraya ters yönde bakar
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;

            case Mode.CameraForward:
                // Nesnenin yönü kameranýn baktýðý yöne eþit olur
                transform.forward = Camera.main.transform.forward;
                break;

            case Mode.CameraForwardInverted:
                // Nesnenin yönü kameranýn tersine bakar
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
