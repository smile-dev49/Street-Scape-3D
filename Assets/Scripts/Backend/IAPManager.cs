using UnityEngine;
using System;

namespace StreetEscape.Backend
{
    public class IAPManager : MonoBehaviour
    {
        public static IAPManager Instance { get; private set; }

        [Header("Product IDs")]
        [SerializeField] private string[] productIds;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        private void Initialize()
        {
            // TODO: Initialize Unity IAP
            // UnityPurchasing.Initialize(this, builder);
        }

        public void PurchaseProduct(string productId, Action onSuccess)
        {
            // TODO: Implement Unity IAP purchase flow
            // Use IStoreController.Purchase(productId)
            Debug.Log($"[IAP] Would purchase: {productId}");
            onSuccess?.Invoke();
        }

        public bool IsProductPurchased(string productId)
        {
            // TODO: Check IStoreExtension.GetProduct(productId)?.hasReceipt
            return false;
        }
    }
}
