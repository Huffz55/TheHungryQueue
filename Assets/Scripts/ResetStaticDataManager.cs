using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{

    // Awake metodu, oyun ba�lad���nda veya bu script y�klendi�inde ilk �al��acak metoddur
    private void Awake()
    {
        // CuttingCounter s�n�f�ndaki statik verileri s�f�rl�yoruz
        CuttingCounter.ResetStaticData();

        // BaseCounter s�n�f�ndaki statik verileri s�f�rl�yoruz
        BaseCounter.ResetStaticData();

        // TrashCounter s�n�f�ndaki statik verileri s�f�rl�yoruz
        TrashCounter.ResetStaticData();
    }
}
