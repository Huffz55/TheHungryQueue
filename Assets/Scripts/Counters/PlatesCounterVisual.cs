using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{

    // PlatesCounter s�n�f�ndan al�nan referanslar
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint; // Tabaklar�n yerle�tirilece�i nokta
    [SerializeField] private Transform plateVisualPrefab; // Tabak g�rseli prefab'�

    // Tabak g�rsel nesnelerini tutan liste
    private List<GameObject> plateVisualGameObjectList;

    // Awake metodunda listeyi ba�lat�yoruz
    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    // Start metodunda PlatesCounter event'lerine abone oluyoruz
    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned; // Yeni bir tabak spawn oldu�unda tetiklenir
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved; // Bir tabak al�nd���nda tetiklenir
    }

    // Tabak al�nd���nda tetiklenen event metodu
    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        // Liste sonundaki tabak g�rselini al
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        // Tabak g�rselini listeden ��kar
        plateVisualGameObjectList.Remove(plateGameObject);
        // Tabak g�rselini yok et
        Destroy(plateGameObject);
    }

    // Tabak spawn oldu�unda tetiklenen event metodu
    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        // Tabak g�rselini prefab'tan instantiate et
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        // Tabaklar�n Y ekseninde �st �ste yerle�tirilmesi i�in offset
        float plateOffsetY = .1f;
        // Y ekseninde offset ile pozisyonu ayarla
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        // Yeni tabak g�rselini listeye ekle
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
