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
        // OrderUI prefab'ý UI panelinde her bir sipariþi temsil etmek için kullanýlacak
        [SerializeField] private OrderUI orderUIPrefab;
        // Aktif sipariþ UI elemanlarýný tutacak liste
        private readonly List<OrderUI> _ordersUI = new List<OrderUI>();
        // Sipariþ UI elemanlarýný yeniden kullanmak için kullanýlacak kuyruk (pool)
        private readonly Queue<OrderUI> _orderUIPool = new Queue<OrderUI>();

        private void Awake()
        {
#if UNITY_EDITOR
            // Unity editor ortamýnda prefab'ýn atanýp atanmadýðýný kontrol et
            Assert.IsNotNull(orderUIPrefab);
#endif
        }

        // UI öðelerini pool'dan almak için bir yöntem
        private OrderUI GetOrderUIFromPool()
        {
            // Pool'da mevcut sipariþ UI öðesi varsa, onu al; yoksa yeni bir tane oluþtur
            return _orderUIPool.Count > 0 ? _orderUIPool.Dequeue() : Instantiate(orderUIPrefab, transform);
        }

        // OnEnable metodunda, sipariþ oluþturulduðunda tetiklenecek eventi dinlemeye baþla
        private void OnEnable()
        {
            OrderManager.OnOrderSpawned += HandleOrderSpawned;
        }

        // OnDisable metodunda, sipariþ oluþturulma eventi dinlemeyi durdur
        private void OnDisable()
        {
            OrderManager.OnOrderSpawned -= HandleOrderSpawned;
        }

        // Sipariþ oluþturulduðunda çaðrýlacak metot
        private void HandleOrderSpawned(Order order)
        {
            // En saðdaki sipariþ UI elemanýnýn X koordinatýný al
            var rightmostX = GetRightmostXFromLastElement();
            // Yeni sipariþ UI öðesini al
            var orderUI = GetOrderUIFromPool();
            // Yeni sipariþi UI öðesine ata
            orderUI.Setup(order);
            // UI öðesini listeye ekle
            _ordersUI.Add(orderUI);
            // UI öðesini saðdan sola kaydýrarak göster
            orderUI.SlideInSpawn(rightmostX);
        }

        // Son sipariþ UI öðesinin en saðdaki X koordinatýný hesaplar
        private float GetRightmostXFromLastElement()
        {
            // Eðer sipariþler listesi boþsa, sýfýr döndür
            if (_ordersUI.Count == 0) return 0;

            float rightmostX = 0f;

            // Teslim edilmemiþ sipariþleri, soldan saða doðru sýralar
            List<OrderUI> orderUisNotDeliveredOrderedByLeftToRight = _ordersUI
                .Where(x => x.Order.IsDelivered == false)
                .OrderBy(y => y.CurrentAnchorX).ToList();

            // Eðer teslim edilmemiþ sipariþ yoksa, sýfýr döndür
            if (orderUisNotDeliveredOrderedByLeftToRight.Count == 0) return 0;

            // En son teslim edilmemiþ sipariþi al ve saðdaki X koordinatýný hesapla
            var last = orderUisNotDeliveredOrderedByLeftToRight.Last();
            rightmostX = last.CurrentAnchorX + last.SizeDeltaX;

            return rightmostX;
        }

        // Tüm aktif sipariþ UI elemanlarýný sola kaydýrarak yeniden düzenler
        public void RegroupPanelsLeft()
        {
            float leftmostX = 0f;

            // Tüm sipariþleri sýrayla iþlemeye baþla
            for (var i = 0; i < _ordersUI.Count; i++)
            {
                var orderUI = _ordersUI[i];
                // Eðer sipariþ teslim edildiyse, UI öðesini pool'a ekle ve listeden çýkar
                if (orderUI.Order.IsDelivered)
                {
                    _orderUIPool.Enqueue(orderUI);
                    _ordersUI.RemoveAt(i);
                    i--; // Listeden öðe çýkarýnca indeksi bir azalt
                }
                else
                {
                    // Sipariþi sola kaydýr ve yeni konumunu ayarla
                    orderUI.SlideLeft(leftmostX);
                    leftmostX += orderUI.SizeDeltaX; // Yeni sol konumu güncelle
                }
            }
        }
    }
}
