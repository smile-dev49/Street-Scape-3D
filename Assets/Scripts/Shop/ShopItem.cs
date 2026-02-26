using UnityEngine;

namespace StreetEscape.Shop
{
    [CreateAssetMenu(fileName = "ShopItem", menuName = "Street Escape/Shop Item")]
    public class ShopItem : ScriptableObject
    {
        public enum ItemType
        {
            Character,
            Skin,
            PowerUp
        }

        [Header("Identity")]
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        [Header("Type")]
        [SerializeField] private ItemType itemType;

        [Header("Pricing")]
        [SerializeField] private int coinPrice;
        [SerializeField] private int gemPrice;
        [SerializeField] private string iapProductId;

        [Header("Visuals")]
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject prefab;

        [Header("Unlock")]
        [SerializeField] private bool isDefaultUnlocked;

        public string Id => id;
        public string DisplayName => displayName;
        public string Description => description;
        public ItemType Type => itemType;
        public int CoinPrice => coinPrice;
        public int GemPrice => gemPrice;
        public string IAPProductId => iapProductId;
        public Sprite Icon => icon;
        public GameObject Prefab => prefab;
        public bool IsDefaultUnlocked => isDefaultUnlocked;

        public bool IsPurchasableWithCoins => coinPrice > 0;
        public bool IsPurchasableWithGems => gemPrice > 0;
        public bool IsIAPItem => !string.IsNullOrEmpty(iapProductId);
    }
}
