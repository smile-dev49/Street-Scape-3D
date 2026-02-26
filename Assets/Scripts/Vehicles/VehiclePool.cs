using System.Collections.Generic;
using UnityEngine;

namespace StreetEscape.Vehicles
{
    /// <summary>
    /// Object pool for vehicles. Spawns and reuses vehicles to avoid allocations.
    /// Reduces garbage collection and improves performance on mobile.
    /// </summary>
    public class VehiclePool : MonoBehaviour
    {
        public static VehiclePool Instance { get; private set; }

        [Header("Pool Settings")]
        [SerializeField] private Vehicle[] vehiclePrefabs;
        [SerializeField] private int initialPoolSize = 10;
        [SerializeField] private int maxPoolSize = 50;

        private readonly Dictionary<Vehicle, Queue<Vehicle>> _pools = new Dictionary<Vehicle, Queue<Vehicle>>();
        private Transform _poolContainer;

        // ==================== UNITY LIFECYCLE ====================

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            _poolContainer = new GameObject("VehiclePool").transform;
            _poolContainer.SetParent(transform);

            InitializePools();
        }

        // ==================== INITIALIZATION ====================

        private void InitializePools()
        {
            if (vehiclePrefabs == null || vehiclePrefabs.Length == 0)
            {
                Debug.LogWarning("[VehiclePool] No vehicle prefabs assigned.");
                return;
            }

            foreach (Vehicle prefab in vehiclePrefabs)
            {
                if (prefab == null) continue;

                var queue = new Queue<Vehicle>();

                for (int i = 0; i < initialPoolSize; i++)
                {
                    Vehicle instance = CreateVehicle(prefab);
                    queue.Enqueue(instance);
                }

                _pools[prefab] = queue;
            }
        }

        private Vehicle CreateVehicle(Vehicle prefab)
        {
            Vehicle instance = Instantiate(prefab, _poolContainer);
            instance.gameObject.SetActive(false);
            return instance;
        }

        // ==================== PUBLIC API ====================

        /// <summary>
        /// Get a vehicle from the pool. Creates new if pool is empty (up to max).
        /// </summary>
        public Vehicle Get(Vehicle prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
            {
                Debug.LogError("[VehiclePool] Prefab is null.");
                return null;
            }

            if (!_pools.TryGetValue(prefab, out Queue<Vehicle> queue))
            {
                _pools[prefab] = new Queue<Vehicle>();
                queue = _pools[prefab];
            }

            Vehicle vehicle;

            if (queue.Count > 0)
            {
                vehicle = queue.Dequeue();
            }
            else
            {
                int totalActive = 0;
                foreach (var q in _pools.Values) totalActive += q.Count;
                // Rough check: if we've spawned many, limit
                vehicle = CreateVehicle(prefab);
            }

            vehicle.transform.SetPositionAndRotation(position, rotation);
            vehicle.transform.SetParent(null);
            return vehicle;
        }

        /// <summary>
        /// Return a vehicle to the pool.
        /// </summary>
        public void ReturnToPool(Vehicle vehicle)
        {
            if (vehicle == null) return;

            vehicle.transform.SetParent(_poolContainer);
            vehicle.gameObject.SetActive(false);

            Vehicle prefab = GetPrefabForVehicle(vehicle);
            if (prefab != null && _pools.TryGetValue(prefab, out Queue<Vehicle> queue))
            {
                queue.Enqueue(vehicle);
            }
            else
            {
                Destroy(vehicle.gameObject);
            }
        }

        private Vehicle GetPrefabForVehicle(Vehicle vehicle)
        {
            foreach (Vehicle prefab in vehiclePrefabs)
            {
                if (prefab != null && vehicle.GetType() == prefab.GetType())
                {
                    return prefab;
                }
            }
            return vehiclePrefabs.Length > 0 ? vehiclePrefabs[0] : null;
        }
    }
}
