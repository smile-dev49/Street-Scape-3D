using UnityEngine;
using StreetEscape.Systems;

namespace StreetEscape.Shop
{
    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance { get; private set; }

        private int _coins;
        private int _gems;

        public int Coins => _coins;
        public int Gems => _gems;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            _coins = SaveManager.Instance != null ? SaveManager.Instance.LoadCurrency() : 0;
            _gems = PlayerPrefs.GetInt("Gems", 0);
        }

        public void AddCoins(int amount)
        {
            _coins += amount;
            SaveManager.Instance?.SaveCurrency(_coins);
        }

        public void SpendCoins(int amount)
        {
            if (_coins < amount) return;
            _coins -= amount;
            SaveManager.Instance?.SaveCurrency(_coins);
        }

        public void AddGems(int amount)
        {
            _gems += amount;
            PlayerPrefs.SetInt("Gems", _gems);
        }

        public void SpendGems(int amount)
        {
            if (_gems < amount) return;
            _gems -= amount;
            PlayerPrefs.SetInt("Gems", _gems);
        }
    }
}
