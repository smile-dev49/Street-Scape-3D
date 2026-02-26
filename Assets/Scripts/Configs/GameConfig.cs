using UnityEngine;

namespace StreetEscape.Configs
{
    /// <summary>
    /// ScriptableObject holding global game configuration.
    /// Create asset: Right-click > Create > Street Escape > Game Config
    /// Allows designers to tune values without code changes.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Street Escape/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Level Settings")]
        [Tooltip("Base number of streets for level 1")]
        [SerializeField] private int baseStreetsCount = 3;

        [Tooltip("Additional streets added per level")]
        [SerializeField] private int streetsPerLevel = 1;

        [Tooltip("Base speed multiplier for vehicles")]
        [SerializeField] private float baseSpeedMultiplier = 1f;

        [Tooltip("Speed increase per level")]
        [SerializeField] private float speedIncreasePerLevel = 0.1f;

        [Tooltip("Base spawn rate (seconds between spawns)")]
        [SerializeField] private float baseSpawnInterval = 2f;

        [Tooltip("Spawn interval decrease per level (minimum 0.5s)")]
        [SerializeField] private float spawnIntervalDecreasePerLevel = 0.05f;

        [Header("Player Settings")]
        [Tooltip("Player movement speed")]
        [SerializeField] private float playerMoveSpeed = 5f;

        [Tooltip("Number of lanes per street")]
        [SerializeField] private int lanesPerStreet = 3;

        [Tooltip("Distance between lanes")]
        [SerializeField] private float laneSpacing = 2f;

        [Header("Economy Settings")]
        [Tooltip("Coins earned per street crossed")]
        [SerializeField] private int coinsPerStreet = 5;

        [Tooltip("Bonus coins for perfect run (no damage)")]
        [SerializeField] private int perfectRunBonus = 25;

        // ==================== PUBLIC ACCESSORS ====================

        public int BaseStreetsCount => baseStreetsCount;
        public int StreetsPerLevel => streetsPerLevel;
        public float BaseSpeedMultiplier => baseSpeedMultiplier;
        public float SpeedIncreasePerLevel => speedIncreasePerLevel;
        public float BaseSpawnInterval => baseSpawnInterval;
        public float SpawnIntervalDecreasePerLevel => spawnIntervalDecreasePerLevel;
        public float PlayerMoveSpeed => playerMoveSpeed;
        public int LanesPerStreet => lanesPerStreet;
        public float LaneSpacing => laneSpacing;
        public int CoinsPerStreet => coinsPerStreet;
        public int PerfectRunBonus => perfectRunBonus;
    }
}
