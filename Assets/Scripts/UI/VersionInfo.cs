using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    public class VersionInfo : MonoBehaviour
    {
        // Uygulama sürüm bilgisini gösterecek olan TextMeshPro referansı
        [SerializeField] private TextMeshProUGUI versionInfo;

        private void Awake()
        {
#if UNITY_EDITOR
            // Unity Editor'de versionInfo referansının null olmadığından emin ol
            Assert.IsNotNull(versionInfo);
#endif
            // Uygulama sürümünü alıp TextMeshPro UI elementine yaz
            versionInfo.text = $"v {Application.version}";
        }
    }
}
