using UnityEngine;
using UnityEngine.UI;

namespace StreetEscape.Shop
{
    public class ShopItemView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Text nameText;
        [SerializeField] private Text priceText;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private GameObject ownedBadge;

        private ShopItem _item;

        public void Setup(ShopItem item)
        {
            _item = item;
            if (item == null) return;

            if (iconImage != null && item.Icon != null)
                iconImage.sprite = item.Icon;
            if (nameText != null)
                nameText.text = item.DisplayName;
            if (ownedBadge != null)
                ownedBadge.SetActive(ShopManager.Instance != null && ShopManager.Instance.IsUnlocked(item));

            UpdatePrice();
            if (purchaseButton != null)
                purchaseButton.onClick.RemoveAllListeners();
        }

        private void UpdatePrice()
        {
            if (_item == null || priceText == null) return;

            var unlocked = ShopManager.Instance != null && ShopManager.Instance.IsUnlocked(_item);
            if (unlocked)
            {
                priceText.text = "Owned";
                if (purchaseButton != null)
                    purchaseButton.interactable = false;
                return;
            }

            if (_item.IsPurchasableWithCoins)
                priceText.text = $"{_item.CoinPrice} Coins";
            else if (_item.IsPurchasableWithGems)
                priceText.text = $"{_item.GemPrice} Gems";
            else if (_item.IsIAPItem)
                priceText.text = "Buy";

            if (purchaseButton != null)
            {
                purchaseButton.interactable = ShopManager.Instance != null && ShopManager.Instance.CanPurchase(_item);
                purchaseButton.onClick.AddListener(OnPurchaseClicked);
            }
        }

        private void OnPurchaseClicked()
        {
            if (_item == null || ShopManager.Instance == null) return;

            if (_item.IsPurchasableWithCoins)
                ShopManager.Instance.PurchaseWithCoins(_item);
            else if (_item.IsPurchasableWithGems)
                ShopManager.Instance.PurchaseWithGems(_item);
            else if (_item.IsIAPItem)
                ShopManager.Instance.PurchaseWithIAP(_item);

            UpdatePrice();
        }
    }
}
