using UnityEngine;

namespace StreetEscape.Lanes
{
    public class Lane : MonoBehaviour
    {
        [SerializeField] private int laneIndex;
        [SerializeField] private float width = 3f;

        public int LaneIndex => laneIndex;
        public float Width => width;
        public Vector3 Center => transform.position;

        public bool ContainsPosition(Vector3 worldPos)
        {
            var localPos = transform.InverseTransformPoint(worldPos);
            return Mathf.Abs(localPos.x) <= width * 0.5f;
        }
    }
}
