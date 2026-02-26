using UnityEngine;
using StreetEscape.Systems;
using StreetEscape.Backend;

namespace StreetEscape.Shop
{
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Instance { get; private set; }

        [Header("Items")]
        [SerializeField] private ShopItem[] shopItems;

        [Header("References")]
        [SerializeField] private CurrencyManager currencyManager;
        [SerializeField] private IAPManager iapManager;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public bool CanPurchase(ShopItem item)
        {
            if (item == null) return false;
            if (item.IsDefaultUnlocked) return false;

            if (item.IsIAPItem)
                return iapManager != null;

            if (item.IsPurchasableWithCoins && currencyManager != null)
                return currencyManager.Coins >= item.CoinPrice;
            if (item.IsPurchasableWithGems && currencyManager != null)
                return currencyManager.Gems >= item.GemPrice;

            return false;
        }

        public void PurchaseWithCoins(ShopItem item)
        {
            if (item == null || currencyManager == null) return;
            if (!item.IsPurchasableWithCoins || currencyManager.Coins < item.CoinPrice) return;

            currencyManager.SpendCoins(item.CoinPrice);
            UnlockItem(item);
        }

        public void PurchaseWithGems(ShopItem item)
        {
            if (item == null || currencyManager == null) return;
            if (!item.IsPurchasableWithGems || currencyManager.Gems < item.GemPrice) return;

            currencyManager.SpendGems(item.GemPrice);
            UnlockItem(item);
        }

        public void PurchaseWithIAP(ShopItem item)
        {
            if (item == null || iapManager == null || string.IsNullOrEmpty(item.IAPProductId))
                return;

            iapManager.PurchaseProduct(item.IAPProductId, () => UnlockItem(item));
        }

        private void UnlockItem(ShopItem item)
        {
            SaveManager.Instance?.SaveUnlock(item.Id, true);
            GameEvents.OnPurchaseComplete?.Invoke(item.Id);
        }

        public bool IsUnlocked(ShopItem item)
        {
            if (item == null) return false;
            if (item.IsDefaultUnlocked) return true;
            return SaveManager.Instance != null && SaveManager.Instance.LoadUnlock(item.Id);
        }

        public ShopItem[] GetShopItems() => shopItems ?? new ShopItem[0];
    }
}
