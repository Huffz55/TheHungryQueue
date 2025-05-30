using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Loader: Sahne (scene) ge�i�lerini y�neten yard�mc� s�n�f
public static class Loader
{

    // Y�klenebilecek sahnelerin enum listesi
    public enum Scene
    {
        MainMenuScene,    // Ana men� sahnesi
        GameScene,        // Oyun sahnesi
        LoadingScene      // Y�kleme sahnesi (ara ge�i� i�in)
    }

    // Ge�i� yap�lmak istenen hedef sahne (statik olarak saklan�r)
    private static Scene targetScene;

    // Sahne y�kleme i�lemini ba�lat�r
    public static void Load(Scene targetScene)
    {
        // Hedef sahneyi sakla
        Loader.targetScene = targetScene;

        // �nce Y�kleme sahnesini ba�lat (�rne�in bir y�kleniyor ekran� g�sterilebilir)
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    // Y�kleme sahnesi taraf�ndan �a�r�l�r: ger�ek sahneye ge�i� yap�l�r
    public static void LoaderCallback()
    {
        // Saklanan hedef sahne y�klenir
        SceneManager.LoadScene(targetScene.ToString());
    }
}
