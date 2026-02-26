using UnityEngine;

namespace StreetEscape.Lanes
{
    [CreateAssetMenu(fileName = "LaneConfig", menuName = "Street Escape/Lane Config")]
    public class LaneConfig : ScriptableObject
    {
        [Header("Layout")]
        [SerializeField] private int laneCount = 3;
        [SerializeField] private float laneSpacing = 3f;
        [SerializeField] private Vector3 laneDirection = Vector3.forward;

        public int LaneCount => laneCount;
        public float LaneSpacing => laneSpacing;
        public Vector3 LaneDirection => laneDirection.normalized;
    }
}
