using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    /// <summary>
    /// UI elemanlarý, Overlay modunda, hedefi dünyada takip eder
    /// </summary>
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform target;  // Takip edilecek hedef (Transform) bileþeni
        [SerializeField] private Vector3 offset;   // Hedef ile UI elemaný arasýndaki ofset mesafesi
        [SerializeField] private Camera cam;       // UI elemanýnýn yerini hesaplamak için kullanýlan kamera

        // Awake metodunda kamera ve hedef kontrolü yapýlýyor
        private void Awake()
        {
            cam = Camera.main;  // Ana kamerayý alýyoruz

#if UNITY_EDITOR
            // Editor sýrasýnda hedefin ve kameranýn atanýp atanmadýðýný kontrol ediyoruz
            Assert.IsNotNull(target);  // Hedefin atanýp atanmadýðýný kontrol ediyoruz
            Assert.IsNotNull(cam);     // Kameranýn atanýp atanmadýðýný kontrol ediyoruz
#endif
        }

        // Her frame sonrasýnda (LateUpdate) çalýþacak metod
        private void LateUpdate()
        {
            // Hedefin dünya koordinatlarýný ekran koordinatlarýna çeviriyoruz ve ofset ekliyoruz
            Vector3 position = cam.WorldToScreenPoint(target.position + offset);

            // Eðer UI elemanýnýn mevcut pozisyonu hedefin pozisyonundan farklýysa, güncelleme yapýyoruz
            if (transform.position != position)
            {
                transform.position = position;  // UI elemanýný hedefin pozisyonuna taþýyoruz
            }
        }
    }
}
