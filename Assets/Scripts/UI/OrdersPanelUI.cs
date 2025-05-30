using System;
using System.Collections.Generic;
using System.Linq;
using Undercooked.Managers;
using Undercooked.Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    public class OrdersPanelUI : MonoBehaviour
    {
        // OrderUI prefab'� UI panelinde her bir sipari�i temsil etmek i�in kullan�lacak
        [SerializeField] private OrderUI orderUIPrefab;
        // Aktif sipari� UI elemanlar�n� tutacak liste
        private readonly List<OrderUI> _ordersUI = new List<OrderUI>();
        // Sipari� UI elemanlar�n� yeniden kullanmak i�in kullan�lacak kuyruk (pool)
        private readonly Queue<OrderUI> _orderUIPool = new Queue<OrderUI>();

        private void Awake()
        {
#if UNITY_EDITOR
            // Unity editor ortam�nda prefab'�n atan�p atanmad���n� kontrol et
            Assert.IsNotNull(orderUIPrefab);
#endif
        }

        // UI ��elerini pool'dan almak i�in bir y�ntem
        private OrderUI GetOrderUIFromPool()
        {
            // Pool'da mevcut sipari� UI ��esi varsa, onu al; yoksa yeni bir tane olu�tur
            return _orderUIPool.Count > 0 ? _orderUIPool.Dequeue() : Instantiate(orderUIPrefab, transform);
        }

        // OnEnable metodunda, sipari� olu�turuldu�unda tetiklenecek eventi dinlemeye ba�la
        private void OnEnable()
        {
            OrderManager.OnOrderSpawned += HandleOrderSpawned;
        }

        // OnDisable metodunda, sipari� olu�turulma eventi dinlemeyi durdur
        private void OnDisable()
        {
            OrderManager.OnOrderSpawned -= HandleOrderSpawned;
        }

        // Sipari� olu�turuldu�unda �a�r�lacak metot
        private void HandleOrderSpawned(Order order)
        {
            // En sa�daki sipari� UI eleman�n�n X koordinat�n� al
            var rightmostX = GetRightmostXFromLastElement();
            // Yeni sipari� UI ��esini al
            var orderUI = GetOrderUIFromPool();
            // Yeni sipari�i UI ��esine ata
            orderUI.Setup(order);
            // UI ��esini listeye ekle
            _ordersUI.Add(orderUI);
            // UI ��esini sa�dan sola kayd�rarak g�ster
            orderUI.SlideInSpawn(rightmostX);
        }

        // Son sipari� UI ��esinin en sa�daki X koordinat�n� hesaplar
        private float GetRightmostXFromLastElement()
        {
            // E�er sipari�ler listesi bo�sa, s�f�r d�nd�r
            if (_ordersUI.Count == 0) return 0;

            float rightmostX = 0f;

            // Teslim edilmemi� sipari�leri, soldan sa�a do�ru s�ralar
            List<OrderUI> orderUisNotDeliveredOrderedByLeftToRight = _ordersUI
                .Where(x => x.Order.IsDelivered == false)
                .OrderBy(y => y.CurrentAnchorX).ToList();

            // E�er teslim edilmemi� sipari� yoksa, s�f�r d�nd�r
            if (orderUisNotDeliveredOrderedByLeftToRight.Count == 0) return 0;

            // En son teslim edilmemi� sipari�i al ve sa�daki X koordinat�n� hesapla
            var last = orderUisNotDeliveredOrderedByLeftToRight.Last();
            rightmostX = last.CurrentAnchorX + last.SizeDeltaX;

            return rightmostX;
        }

        // T�m aktif sipari� UI elemanlar�n� sola kayd�rarak yeniden d�zenler
        public void RegroupPanelsLeft()
        {
            float leftmostX = 0f;

            // T�m sipari�leri s�rayla i�lemeye ba�la
            for (var i = 0; i < _ordersUI.Count; i++)
            {
                var orderUI = _ordersUI[i];
                // E�er sipari� teslim edildiyse, UI ��esini pool'a ekle ve listeden ��kar
                if (orderUI.Order.IsDelivered)
                {
                    _orderUIPool.Enqueue(orderUI);
                    _ordersUI.RemoveAt(i);
                    i--; // Listeden ��e ��kar�nca indeksi bir azalt
                }
                else
                {
                    // Sipari�i sola kayd�r ve yeni konumunu ayarla
                    orderUI.SlideLeft(leftmostX);
                    leftmostX += orderUI.SizeDeltaX; // Yeni sol konumu g�ncelle
                }
            }
        }
    }
}
