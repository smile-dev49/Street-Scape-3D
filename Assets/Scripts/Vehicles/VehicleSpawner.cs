using UnityEngine;
using StreetEscape.Lanes;

namespace StreetEscape.Vehicles
{
    public class VehicleSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LaneManager laneManager;
        [SerializeField] private VehiclePool vehiclePool;

        [Header("Spawn Config")]
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private float spawnIntervalMin = 0.5f;
        [SerializeField] private float difficultyScaleRate = 0.01f;

        private float _nextSpawnTime;

        private void Update()
        {
            if (Time.time < _nextSpawnTime) return;

            SpawnVehicle();
            _nextSpawnTime = Time.time + GetCurrentInterval();
        }

        private void SpawnVehicle()
        {
            if (laneManager == null || vehiclePool == null) return;

            var laneIndex = Random.Range(0, laneManager.LaneCount);
            var pos = laneManager.GetLanePosition(laneIndex);

            var vehicle = vehiclePool.Get();
            if (vehicle != null)
                vehicle.Activate(pos);
        }

        private float GetCurrentInterval()
        {
            var elapsed = Time.timeSinceLevelLoad;
            var scaled = spawnInterval - elapsed * difficultyScaleRate;
            return Mathf.Max(spawnIntervalMin, scaled);
        }
    }
}
