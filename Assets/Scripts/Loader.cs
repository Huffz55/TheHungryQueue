using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Loader: Sahne (scene) geçiþlerini yöneten yardýmcý sýnýf
public static class Loader
{

    // Yüklenebilecek sahnelerin enum listesi
    public enum Scene
    {
        MainMenuScene,    // Ana menü sahnesi
        GameScene,        // Oyun sahnesi
        LoadingScene      // Yükleme sahnesi (ara geçiþ için)
    }

    // Geçiþ yapýlmak istenen hedef sahne (statik olarak saklanýr)
    private static Scene targetScene;

    // Sahne yükleme iþlemini baþlatýr
    public static void Load(Scene targetScene)
    {
        // Hedef sahneyi sakla
        Loader.targetScene = targetScene;

        // Önce Yükleme sahnesini baþlat (örneðin bir yükleniyor ekraný gösterilebilir)
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    // Yükleme sahnesi tarafýndan çaðrýlýr: gerçek sahneye geçiþ yapýlýr
    public static void LoaderCallback()
    {
        // Saklanan hedef sahne yüklenir
        SceneManager.LoadScene(targetScene.ToString());
    }
}
