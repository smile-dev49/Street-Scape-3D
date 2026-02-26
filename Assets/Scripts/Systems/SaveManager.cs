using UnityEngine;

namespace StreetEscape.Systems
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private const string CurrencyKey = "Currency";
        private const string UnlocksKey = "Unlocks";
        private const string SettingsKey = "Settings";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public int LoadCurrency()
        {
            return PlayerPrefs.GetInt(CurrencyKey, 0);
        }

        public void SaveCurrency(int amount)
        {
            PlayerPrefs.SetInt(CurrencyKey, amount);
        }

        public void SaveUnlock(string itemId, bool unlocked)
        {
            var key = $"{UnlocksKey}_{itemId}";
            PlayerPrefs.SetInt(key, unlocked ? 1 : 0);
        }

        public bool LoadUnlock(string itemId)
        {
            var key = $"{UnlocksKey}_{itemId}";
            return PlayerPrefs.GetInt(key, 0) == 1;
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}
