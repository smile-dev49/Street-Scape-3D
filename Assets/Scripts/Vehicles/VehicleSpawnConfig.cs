using UnityEngine;

namespace StreetEscape.Vehicles
{
    [CreateAssetMenu(fileName = "VehicleSpawnConfig", menuName = "Street Escape/Vehicle Spawn Config")]
    public class VehicleSpawnConfig : ScriptableObject
    {
        [Header("Spawn Speed")]
        [Tooltip("Seconds between spawns. Lower = faster spawn rate.")]
        [SerializeField] private float spawnInterval = 2f;

        [Tooltip("Minimum interval (caps how fast spawns can get).")]
        [SerializeField] private float spawnIntervalMin = 0.5f;

        [Header("Difficulty")]
        [Tooltip("How much to reduce spawn interval per second of gameplay (0 = constant rate).")]
        [SerializeField] private float difficultyScaleRate = 0.02f;

        [Header("Pool")]
        [SerializeField] private int poolSize = 15;

        [Header("Despawn")]
        [Tooltip("Return vehicle to pool when it moves this far from spawn (e.g. past camera).")]
        [SerializeField] private float despawnDistance = 50f;

        public float SpawnInterval => spawnInterval;
        public float SpawnIntervalMin => spawnIntervalMin;
        public float DifficultyScaleRate => difficultyScaleRate;
        public int PoolSize => poolSize;
        public float DespawnDistance => despawnDistance;
    }
}
