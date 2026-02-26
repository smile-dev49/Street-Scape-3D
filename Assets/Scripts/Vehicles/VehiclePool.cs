using UnityEngine;
using System.Collections.Generic;

namespace StreetEscape.Vehicles
{
    public class VehiclePool : MonoBehaviour
    {
        [SerializeField] private Vehicle vehiclePrefab;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private Transform parent;

        private readonly Queue<Vehicle> _pool = new Queue<Vehicle>();

        private void Awake()
        {
            for (var i = 0; i < poolSize; i++)
            {
                var v = Instantiate(vehiclePrefab, parent != null ? parent : transform);
                v.Deactivate();
                _pool.Enqueue(v);
            }
        }

        public Vehicle Get()
        {
            if (_pool.Count == 0)
            {
                var v = Instantiate(vehiclePrefab, parent != null ? parent : transform);
                return v;
            }

            return _pool.Dequeue();
        }

        public void Return(Vehicle vehicle)
        {
            vehicle.Deactivate();
            _pool.Enqueue(vehicle);
        }
    }
}
