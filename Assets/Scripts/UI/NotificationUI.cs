using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    // Bildirim UI sýnýfý, ekranda merkezi bir bildirim göstermek için kullanýlýr
    public class NotificationUI : MonoBehaviour
    {
        // Bildirim metnini gösteren TextMeshPro bileþeni
        private static TextMeshProUGUI _text;

        private void Awake()
        {
            // Bildirim metni TextMeshPro bileþenini al
            _text = GetComponentInChildren<TextMeshProUGUI>();

#if UNITY_EDITOR
            // Editor ortamýnda _text bileþeninin null olmamasýný saðla
            Assert.IsNotNull(_text);
#endif
        }

        // Merkezi bildirim gösterme iþlemi
        public static async Task DisplayCenterNotificationAsync(
            string textToDisplay, Color outlineColor, float timeToDisplayInSeconds = 2f)
        {
            // Bildirimi metni ve dýþ hat rengini ayarla
            _text.text = textToDisplay;
            _text.outlineColor = outlineColor;

            // Bildirim süresini bekle (verilen süre boyunca gösterilecek)
            await Task.Delay((int)(timeToDisplayInSeconds * 1000f));

            // Süre tamamlandýktan sonra metni temizle
            _text.text = string.Empty;
        }
    }
}
