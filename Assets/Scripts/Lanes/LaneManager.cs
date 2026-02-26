using UnityEngine;

namespace StreetEscape.Lanes
{
    public class LaneManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private int laneCount = 3;
        [SerializeField] private float laneSpacing = 3f;
        [SerializeField] private Vector3 laneDirection = Vector3.forward;

        [Header("Lane Positions (optional - auto-generated if empty)")]
        [SerializeField] private Transform[] laneTransforms;

        private Vector3[] _lanePositions;

        private void Awake()
        {
            if (laneTransforms != null && laneTransforms.Length > 0)
            {
                _lanePositions = new Vector3[laneTransforms.Length];
                for (var i = 0; i < laneTransforms.Length; i++)
                    _lanePositions[i] = laneTransforms[i].position;
            }
            else
            {
                _lanePositions = new Vector3[laneCount];
                var center = (laneCount - 1) * 0.5f;
                for (var i = 0; i < laneCount; i++)
                {
                    var offset = (i - center) * laneSpacing;
                    var right = Vector3.Cross(laneDirection, Vector3.up).normalized;
                    _lanePositions[i] = transform.position + right * offset;
                }
            }
        }

        public int GetCenterLaneIndex() => laneCount / 2;
        public int LaneCount => laneCount;
        public bool IsValidLane(int index) => index >= 0 && index < laneCount;
        public Vector3 GetLanePosition(int index) => _lanePositions[index];
    }
}
