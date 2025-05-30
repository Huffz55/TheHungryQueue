using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    // Bildirim UI s�n�f�, ekranda merkezi bir bildirim g�stermek i�in kullan�l�r
    public class NotificationUI : MonoBehaviour
    {
        // Bildirim metnini g�steren TextMeshPro bile�eni
        private static TextMeshProUGUI _text;

        private void Awake()
        {
            // Bildirim metni TextMeshPro bile�enini al
            _text = GetComponentInChildren<TextMeshProUGUI>();

#if UNITY_EDITOR
            // Editor ortam�nda _text bile�eninin null olmamas�n� sa�la
            Assert.IsNotNull(_text);
#endif
        }

        // Merkezi bildirim g�sterme i�lemi
        public static async Task DisplayCenterNotificationAsync(
            string textToDisplay, Color outlineColor, float timeToDisplayInSeconds = 2f)
        {
            // Bildirimi metni ve d�� hat rengini ayarla
            _text.text = textToDisplay;
            _text.outlineColor = outlineColor;

            // Bildirim s�resini bekle (verilen s�re boyunca g�sterilecek)
            await Task.Delay((int)(timeToDisplayInSeconds * 1000f));

            // S�re tamamland�ktan sonra metni temizle
            _text.text = string.Empty;
        }
    }
}
