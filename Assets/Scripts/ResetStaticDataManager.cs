using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{

    // Awake metodu, oyun baþladýðýnda veya bu script yüklendiðinde ilk çalýþacak metoddur
    private void Awake()
    {
        // CuttingCounter sýnýfýndaki statik verileri sýfýrlýyoruz
        CuttingCounter.ResetStaticData();

        // BaseCounter sýnýfýndaki statik verileri sýfýrlýyoruz
        BaseCounter.ResetStaticData();

        // TrashCounter sýnýfýndaki statik verileri sýfýrlýyoruz
        TrashCounter.ResetStaticData();
    }
}
