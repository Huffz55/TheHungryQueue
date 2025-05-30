using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{

    // PlatesCounter sýnýfýndan alýnan referanslar
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint; // Tabaklarýn yerleþtirileceði nokta
    [SerializeField] private Transform plateVisualPrefab; // Tabak görseli prefab'ý

    // Tabak görsel nesnelerini tutan liste
    private List<GameObject> plateVisualGameObjectList;

    // Awake metodunda listeyi baþlatýyoruz
    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    // Start metodunda PlatesCounter event'lerine abone oluyoruz
    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned; // Yeni bir tabak spawn olduðunda tetiklenir
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved; // Bir tabak alýndýðýnda tetiklenir
    }

    // Tabak alýndýðýnda tetiklenen event metodu
    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        // Liste sonundaki tabak görselini al
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        // Tabak görselini listeden çýkar
        plateVisualGameObjectList.Remove(plateGameObject);
        // Tabak görselini yok et
        Destroy(plateGameObject);
    }

    // Tabak spawn olduðunda tetiklenen event metodu
    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        // Tabak görselini prefab'tan instantiate et
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        // Tabaklarýn Y ekseninde üst üste yerleþtirilmesi için offset
        float plateOffsetY = .1f;
        // Y ekseninde offset ile pozisyonu ayarla
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        // Yeni tabak görselini listeye ekle
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
