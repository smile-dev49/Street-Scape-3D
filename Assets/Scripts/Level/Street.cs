using UnityEngine;
using StreetEscape.Vehicles;

namespace StreetEscape.Level
{
    /// <summary>
    /// Represents a single street with multiple lanes.
    /// Holds spawn points and VehicleSpawner for this street.
    /// Attach to Street prefab.
    /// </summary>
    public class Street : MonoBehaviour
    {
        [Header("Lanes")]
        [SerializeField] private Transform[] laneTransforms;

        [Header("Spawner")]
        [SerializeField] private VehicleSpawner vehicleSpawner;

        [Header("Street Index")]
        [Tooltip("Index of this street in the level (0 = first street).")]
        [SerializeField] private int streetIndex;

        // ==================== PROPERTIES ====================

        public int StreetIndex => streetIndex;
        public Transform[] LaneTransforms => laneTransforms;
        public VehicleSpawner Spawner => vehicleSpawner;

        // ==================== UNITY LIFECYCLE ====================

        private void Awake()
        {
            if (vehicleSpawner != null && laneTransforms != null && laneTransforms.Length > 0)
            {
                vehicleSpawner.SetSpawnPoints(laneTransforms);
            }
        }

        // ==================== PUBLIC API ====================

        /// <summary>
        /// Initialize street with index and spawner level.
        /// </summary>
        public void Initialize(int index, int level)
        {
            streetIndex = index;

            if (vehicleSpawner != null)
            {
                vehicleSpawner.SetLevel(level);
            }
        }
    }
}
