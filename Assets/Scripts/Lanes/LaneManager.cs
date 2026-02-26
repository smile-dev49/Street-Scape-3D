using UnityEngine;

namespace StreetEscape.Lanes
{
    /// <summary>
    /// Provides lane positions for spawning and player movement. Uses LaneConfig SO or inline fields.
    /// </summary>
    public class LaneManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private LaneConfig laneConfig;

        [Header("Override (used when LaneConfig is null)")]
        [SerializeField] private int laneCount = 3;
        [SerializeField] private float laneSpacing = 3f;
        [SerializeField] private Vector3 laneDirection = Vector3.forward;

        [Header("Lane Positions (optional - auto-generated if empty)")]
        [SerializeField] private Transform[] laneTransforms;

        private Vector3[] _lanePositions;
        private int _laneCount;

        private void Awake()
        {
            if (laneTransforms != null && laneTransforms.Length > 0)
            {
                _laneCount = laneTransforms.Length;
                _lanePositions = new Vector3[_laneCount];
                for (var i = 0; i < _laneCount; i++)
                    _lanePositions[i] = laneTransforms[i].position;
            }
            else
            {
                var count = laneConfig != null ? laneConfig.LaneCount : laneCount;
                var spacing = laneConfig != null ? laneConfig.LaneSpacing : laneSpacing;
                var dir = laneConfig != null ? laneConfig.LaneDirection : laneDirection.normalized;

                _laneCount = count;
                _lanePositions = new Vector3[_laneCount];
                var center = (_laneCount - 1) * 0.5f;
                var right = Vector3.Cross(dir, Vector3.up).normalized;

                for (var i = 0; i < _laneCount; i++)
                {
                    var offset = (i - center) * spacing;
                    _lanePositions[i] = transform.position + right * offset;
                }
            }
        }

        public int GetCenterLaneIndex() => _laneCount / 2;
        public int LaneCount => _laneCount;
        public bool IsValidLane(int index) => index >= 0 && index < _laneCount;
        public Vector3 GetLanePosition(int index) => _lanePositions[index];
    }
}
