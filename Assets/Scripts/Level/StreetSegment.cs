using UnityEngine;

namespace StreetEscape.Level
{
    /// <summary>
    /// Represents a single road/lane segment (visual mesh or collider).
    /// Used as a building block for the Street prefab.
    /// </summary>
    public class StreetSegment : MonoBehaviour
    {
        [Header("Segment")]
        [SerializeField] private int laneIndex;

        [SerializeField] private bool hasVehicleSpawnPoint;

        // ==================== PROPERTIES ====================

        public int LaneIndex => laneIndex;
        public bool HasVehicleSpawnPoint => hasVehicleSpawnPoint;

        /// <summary>
        /// Returns this transform as the spawn point for vehicles on this lane.
        /// </summary>
        public Transform SpawnPoint => transform;
    }
}
