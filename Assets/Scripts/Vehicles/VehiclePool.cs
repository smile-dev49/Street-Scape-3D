using UnityEngine;
using System.Collections.Generic;

namespace StreetEscape.Vehicles
{
    /// <summary>
    /// Object pool for vehicles. Pre-warms and reuses instances to avoid allocations.
    /// </summary>
    public class VehiclePool : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private Vehicle vehiclePrefab;

        [Header("Pool (overridden by VehicleSpawnConfig if provided to spawner)")]
        [SerializeField] private int poolSize = 10;
        [SerializeField] private Transform parent;

        private readonly Queue<Vehicle> _pool = new Queue<Vehicle>();
        private bool _initialized;

        /// <summary>Initialize pool with given size (e.g. from VehicleSpawnConfig). Call before first Get.</summary>
        public void Initialize(int size)
        {
            poolSize = Mathf.Max(1, size);
            WarmPool();
        }

        private void WarmPool()
        {
            if (_initialized || vehiclePrefab == null) return;

            for (var i = 0; i < poolSize; i++)
            {
                var v = Instantiate(vehiclePrefab, parent != null ? parent : transform);
                v.Deactivate();
                _pool.Enqueue(v);
            }
            _initialized = true;
        }

        private void Awake()
        {
            if (!_initialized)
                WarmPool();
        }

        public Vehicle Get()
        {
            if (vehiclePrefab == null) return null;
            if (!_initialized) WarmPool();

            if (_pool.Count == 0)
            {
                var v = Instantiate(vehiclePrefab, parent != null ? parent : transform);
                return v;
            }
            return _pool.Dequeue();
        }

        public void Return(Vehicle vehicle)
        {
            if (vehicle == null) return;
            vehicle.Deactivate();
            _pool.Enqueue(vehicle);
        }
    }
}
