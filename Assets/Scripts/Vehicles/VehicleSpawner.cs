using UnityEngine;
using StreetEscape.Configs;
using StreetEscape.Managers;

namespace StreetEscape.Vehicles
{
    /// <summary>
    /// Spawns vehicles on lanes with random timing and speed.
    /// Speed and spawn rate scale with level difficulty.
    /// </summary>
    public class VehicleSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private VehicleSpawnConfig[] spawnConfigs;

        [Header("Spawn Area")]
        [SerializeField] private Transform[] laneSpawnPoints;
        [SerializeField] private float spawnOffsetZ = 0f;

        [Header("Timing")]
        [SerializeField] private float minSpawnInterval = 1f;
        [SerializeField] private float maxSpawnInterval = 3f;

        private float _nextSpawnTime;
        private bool _isSpawning;
        private int _currentLevel = 1;
        private float _speedMultiplier = 1f;

        // ==================== UNITY LIFECYCLE ====================

        private void Start()
        {
            if (GameManager.Instance != null && GameManager.Instance.Config != null)
            {
                var config = GameManager.Instance.Config;
                minSpawnInterval = Mathf.Max(0.5f, config.BaseSpawnInterval - config.SpawnIntervalDecreasePerLevel * _currentLevel);
                maxSpawnInterval = minSpawnInterval + 1.5f;
                _speedMultiplier = config.BaseSpeedMultiplier + config.SpeedIncreasePerLevel * _currentLevel;
            }

            _nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }

        private void OnEnable()
        {
            StreetEscape.Core.GameEvents.OnGameStarted += HandleGameStarted;
            StreetEscape.Core.GameEvents.OnGameOver += HandleGameOver;
            StreetEscape.Core.GameEvents.OnLevelRestart += HandleGameRestart;
        }

        private void OnDisable()
        {
            StreetEscape.Core.GameEvents.OnGameStarted -= HandleGameStarted;
            StreetEscape.Core.GameEvents.OnGameOver -= HandleGameOver;
            StreetEscape.Core.GameEvents.OnLevelRestart -= HandleGameRestart;
        }

        private void Update()
        {
            if (!_isSpawning) return;

            if (Time.time >= _nextSpawnTime)
            {
                SpawnVehicle();
                _nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
            }
        }

        // ==================== PUBLIC API ====================

        /// <summary>
        /// Set level for difficulty scaling.
        /// </summary>
        public void SetLevel(int level)
        {
            _currentLevel = level;

            if (GameManager.Instance != null && GameManager.Instance.Config != null)
            {
                var config = GameManager.Instance.Config;
                minSpawnInterval = Mathf.Max(0.5f, config.BaseSpawnInterval - config.SpawnIntervalDecreasePerLevel * level);
                maxSpawnInterval = minSpawnInterval + 1.5f;
                _speedMultiplier = config.BaseSpeedMultiplier + config.SpeedIncreasePerLevel * level;
            }
        }

        /// <summary>
        /// Set spawn points (lanes) for this street.
        /// </summary>
        public void SetSpawnPoints(Transform[] points)
        {
            laneSpawnPoints = points;
        }

        // ==================== SPAWNING ====================

        private void SpawnVehicle()
        {
            if (spawnConfigs == null || spawnConfigs.Length == 0) return;
            if (laneSpawnPoints == null || laneSpawnPoints.Length == 0) return;

            VehicleSpawnConfig config = GetRandomConfig();
            if (config == null || config.VehiclePrefab == null) return;

            Transform spawnPoint = laneSpawnPoints[Random.Range(0, laneSpawnPoints.Length)];
            Vector3 pos = spawnPoint.position + Vector3.forward * spawnOffsetZ;

            // Random direction: left or right
            Vector3 direction = Random.value > 0.5f ? Vector3.right : Vector3.left;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            if (direction.x < 0) rot *= Quaternion.Euler(0, 180, 0);

            Vehicle vehicle;

            if (VehiclePool.Instance != null)
            {
                vehicle = VehiclePool.Instance.Get(config.VehiclePrefab, pos, rot);
            }
            else
            {
                vehicle = Instantiate(config.VehiclePrefab, pos, rot);
            }

            float speed = config.RandomSpeed * _speedMultiplier;
            vehicle.Activate(speed, direction);
        }

        private VehicleSpawnConfig GetRandomConfig()
        {
            int totalWeight = 0;
            foreach (var c in spawnConfigs)
            {
                if (c != null) totalWeight += c.SpawnWeight;
            }

            if (totalWeight <= 0) return spawnConfigs[0];

            int r = Random.Range(0, totalWeight);
            foreach (var c in spawnConfigs)
            {
                if (c == null) continue;
                totalWeight -= c.SpawnWeight;
                if (totalWeight <= 0) return c;
            }

            return spawnConfigs[0];
        }

        // ==================== EVENT HANDLERS ====================

        private void HandleGameStarted()
        {
            _isSpawning = true;
        }

        private void HandleGameOver()
        {
            _isSpawning = false;
        }

        private void HandleGameRestart()
        {
            _isSpawning = true;
        }
    }
}
