using UnityEngine;
using StreetEscape.Vehicles;

namespace StreetEscape.Configs
{
    /// <summary>
    /// ScriptableObject for vehicle spawn configuration.
    /// Define vehicle prefab, speed range, weight for random selection.
    /// </summary>
    [CreateAssetMenu(fileName = "VehicleSpawnConfig", menuName = "Street Escape/Vehicle Spawn Config")]
    public class VehicleSpawnConfig : ScriptableObject
    {
        [Header("Vehicle")]
        [Tooltip("Vehicle prefab to spawn")]
        [SerializeField] private Vehicle vehiclePrefab;

        [Header("Speed")]
        [Tooltip("Minimum speed")]
        [SerializeField] private float minSpeed = 3f;

        [Tooltip("Maximum speed")]
        [SerializeField] private float maxSpeed = 8f;

        [Header("Spawn Weight")]
        [Tooltip("Higher weight = more likely to spawn")]
        [SerializeField] private int spawnWeight = 10;

        // ==================== PUBLIC ACCESSORS ====================

        public Vehicle VehiclePrefab => vehiclePrefab;
        public float MinSpeed => minSpeed;
        public float MaxSpeed => maxSpeed;
        public int SpawnWeight => spawnWeight;

        public float RandomSpeed => Random.Range(minSpeed, maxSpeed);
    }
}
