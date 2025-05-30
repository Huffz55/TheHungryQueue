using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    /// <summary>
    /// UI elemanlar�, Overlay modunda, hedefi d�nyada takip eder
    /// </summary>
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform target;  // Takip edilecek hedef (Transform) bile�eni
        [SerializeField] private Vector3 offset;   // Hedef ile UI eleman� aras�ndaki ofset mesafesi
        [SerializeField] private Camera cam;       // UI eleman�n�n yerini hesaplamak i�in kullan�lan kamera

        // Awake metodunda kamera ve hedef kontrol� yap�l�yor
        private void Awake()
        {
            cam = Camera.main;  // Ana kameray� al�yoruz

#if UNITY_EDITOR
            // Editor s�ras�nda hedefin ve kameran�n atan�p atanmad���n� kontrol ediyoruz
            Assert.IsNotNull(target);  // Hedefin atan�p atanmad���n� kontrol ediyoruz
            Assert.IsNotNull(cam);     // Kameran�n atan�p atanmad���n� kontrol ediyoruz
#endif
        }

        // Her frame sonras�nda (LateUpdate) �al��acak metod
        private void LateUpdate()
        {
            // Hedefin d�nya koordinatlar�n� ekran koordinatlar�na �eviriyoruz ve ofset ekliyoruz
            Vector3 position = cam.WorldToScreenPoint(target.position + offset);

            // E�er UI eleman�n�n mevcut pozisyonu hedefin pozisyonundan farkl�ysa, g�ncelleme yap�yoruz
            if (transform.position != position)
            {
                transform.position = position;  // UI eleman�n� hedefin pozisyonuna ta��yoruz
            }
        }
    }
}
