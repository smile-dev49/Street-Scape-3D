using UnityEngine;
using StreetEscape.Lanes;
using System.Collections.Generic;

namespace StreetEscape.Vehicles
{
    /// <summary>
    /// Spawns vehicles on lanes using object pooling. Spawn speed and difficulty from ScriptableObject config.
    /// </summary>
    public class VehicleSpawner : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private VehicleSpawnConfig spawnConfig;

        [Header("References")]
        [SerializeField] private LaneManager laneManager;
        [SerializeField] private VehiclePool vehiclePool;

        [Header("Fallback (when spawnConfig is null)")]
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private float spawnIntervalMin = 0.5f;
        [SerializeField] private float despawnDistance = 50f;

        private float _nextSpawnTime;
        private readonly List<Vehicle> _activeVehicles = new List<Vehicle>();

        private void Start()
        {
            if (spawnConfig != null && vehiclePool != null)
                vehiclePool.Initialize(spawnConfig.PoolSize);

            _nextSpawnTime = Time.time + GetCurrentInterval();
        }

        private void Update()
        {
            TrySpawn();
            ReturnOffscreenVehicles();
        }

        private void TrySpawn()
        {
            if (Time.time < _nextSpawnTime) return;
            if (laneManager == null || vehiclePool == null) return;

            var laneIndex = Random.Range(0, laneManager.LaneCount);
            var pos = laneManager.GetLanePosition(laneIndex);

            var vehicle = vehiclePool.Get();
            if (vehicle != null)
            {
                vehicle.Activate(pos);
                _activeVehicles.Add(vehicle);
            }

            _nextSpawnTime = Time.time + GetCurrentInterval();
        }

        private float GetCurrentInterval()
        {
            if (spawnConfig == null)
                return spawnInterval;

            var elapsed = Time.timeSinceLevelLoad;
            var scaled = spawnConfig.SpawnInterval - elapsed * spawnConfig.DifficultyScaleRate;
            return Mathf.Max(spawnConfig.SpawnIntervalMin, scaled);
        }

        private void ReturnOffscreenVehicles()
        {
            if (vehiclePool == null) return;
            var limit = spawnConfig != null ? spawnConfig.DespawnDistance : despawnDistance;

            for (var i = _activeVehicles.Count - 1; i >= 0; i--)
            {
                var v = _activeVehicles[i];
                if (v == null || !v.gameObject.activeInHierarchy) { _activeVehicles.RemoveAt(i); continue; }

                if (v.DistanceFromSpawn > limit)
                {
                    vehiclePool.Return(v);
                    _activeVehicles.RemoveAt(i);
                }
            }
        }
    }
}
