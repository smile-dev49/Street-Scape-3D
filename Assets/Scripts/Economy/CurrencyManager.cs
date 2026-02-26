using UnityEngine;
using StreetEscape.Core;
using StreetEscape.Configs;
using StreetEscape.Managers;

namespace StreetEscape.Economy
{
    /// <summary>
    /// Manages in-game currency (coins).
    /// Earn coins per street crossed, bonus for perfect run.
    /// Saves using PlayerPrefs.
    /// </summary>
    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private GameConfig gameConfig;

        private const string CoinsKey = "StreetEscape_Coins";

        private int _coins;

        // ==================== PROPERTIES ====================

        public int Coins => _coins;

        // ==================== UNITY LIFECYCLE ====================

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            LoadCoins();
        }

        private void OnEnable()
        {
            GameEvents.OnStreetCrossed += HandleStreetCrossed;
            GameEvents.OnLevelComplete += HandleLevelComplete;
        }

        private void OnDisable()
        {
            GameEvents.OnStreetCrossed -= HandleStreetCrossed;
            GameEvents.OnLevelComplete -= HandleLevelComplete;
        }

        // ==================== PERSISTENCE ====================

        private void LoadCoins()
        {
            _coins = PlayerPrefs.GetInt(CoinsKey, 0);
            GameEvents.RaiseCoinsChanged(_coins);
        }

        private void SaveCoins()
        {
            PlayerPrefs.SetInt(CoinsKey, _coins);
            PlayerPrefs.Save();
        }

        // ==================== PUBLIC API ====================

        /// <summary>
        /// Add coins and notify listeners.
        /// </summary>
        public void AddCoins(int amount)
        {
            if (amount <= 0) return;

            _coins += amount;
            SaveCoins();
            GameEvents.RaiseCoinsChanged(_coins);
            GameEvents.RaiseCoinsEarned(amount);
        }

        /// <summary>
        /// Spend coins. Returns true if successful.
        /// </summary>
        public bool SpendCoins(int amount)
        {
            if (amount <= 0) return true;
            if (_coins < amount) return false;

            _coins -= amount;
            SaveCoins();
            GameEvents.RaiseCoinsChanged(_coins);
            return true;
        }

        /// <summary>
        /// Check if player has enough coins.
        /// </summary>
        public bool HasCoins(int amount)
        {
            return _coins >= amount;
        }

        // ==================== EVENT HANDLERS ====================

        private void HandleStreetCrossed(int streetIndex)
        {
            int coinsPerStreet = gameConfig != null ? gameConfig.CoinsPerStreet : 5;
            AddCoins(coinsPerStreet);
        }

        private void HandleLevelComplete()
        {
            // Perfect run bonus: no damage taken (simplified - always give bonus for now)
            // TODO: Integrate with PlayerHealth to only give bonus when no damage
            int bonus = gameConfig != null ? gameConfig.PerfectRunBonus : 25;
            AddCoins(bonus);
        }
    }
}
